using System;
using Unity.Mathematics;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Baselib.LowLevel;
using Unity.Burst;
using Unity.Collections;

namespace Unity.Collections.LowLevel.Unsafe
{
    [BurstCompatible]
    public struct VMRange
    {
        public IntPtr ptr;

        public byte log2PageSize;

        public uint pageCount;

        public uint PageSizeInBytes
        {
            get
            {
                return (uint)(1 << (int)log2PageSize);
            }
            set
            {
                log2PageSize = (byte)(32 - math.lzcnt(math.max(1, (int)value) - 1));
            }
        }

        public ulong SizeInBytes => (ulong)PageSizeInBytes * (ulong)pageCount;

        public VMRange(IntPtr rangePtr, byte rangeLog2PageSize, uint rangePageCount)
        {
            ptr = rangePtr;
            log2PageSize = rangeLog2PageSize;
            pageCount = rangePageCount;
        }

        public VMRange(IntPtr rangePtr, uint rangeBytes, uint pageSizeInBytes)
        {
            this = new VMRange
            {
                ptr = rangePtr,
                pageCount = VirtualMemoryUtility.BytesToPageCount(rangeBytes, pageSizeInBytes),
                PageSizeInBytes = pageSizeInBytes
            };
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    [BurstCompatible]
    public struct VirtualMemoryUtility
    {
        [BurstCompatible]
        internal struct PageSizeInfo
        {
            private byte log2DefaultPageSize;

            public int availableLog2PageSizes;

            public uint DefaultPageSizeInBytes => (uint)(1 << (int)log2DefaultPageSize);

            public int AvailablePageSizeCount => math.countbits(availableLog2PageSizes);

            public unsafe PageSizeInfo(ulong defaultPageSize, ulong* availablePageSizes, ulong numAvailablePageSizes)
            {
                log2DefaultPageSize = (byte)(64 - math.lzcnt(math.max(1uL, defaultPageSize) - 1));
                availableLog2PageSizes = 1 << (int)log2DefaultPageSize;
                for (int i = 1; i < (int)numAvailablePageSizes; i++)
                {
                    availableLog2PageSizes |= 1 << (int)availablePageSizes[i];
                }
            }
        }

        [BurstCompatible]
        internal sealed class StaticPageSizeInfo
        {
            public static readonly SharedStatic<PageSizeInfo> Ref = SharedStatic<PageSizeInfo>.GetOrCreate<PageSizeInfo>();

            private StaticPageSizeInfo()
            {
            }
        }

        public unsafe static uint DefaultPageSizeInBytes
        {
            get
            {
                if (GetPageSizeInfo().DefaultPageSizeInBytes == 1)
                {
                    Binding.Baselib_Memory_PageSizeInfo baselib_Memory_PageSizeInfo = default(Binding.Baselib_Memory_PageSizeInfo);
                    Binding.Baselib_Memory_GetPageSizeInfo(&baselib_Memory_PageSizeInfo);
                    StaticPageSizeInfo.Ref.Data = new PageSizeInfo(baselib_Memory_PageSizeInfo.defaultPageSize, &baselib_Memory_PageSizeInfo.pageSizes0, baselib_Memory_PageSizeInfo.pageSizesLen);
                }

                return StaticPageSizeInfo.Ref.Data.DefaultPageSizeInBytes;
            }
        }

        internal static PageSizeInfo GetPageSizeInfo()
        {
            return StaticPageSizeInfo.Ref.Data;
        }

        public unsafe static void ReportWrappedBaselibError(BaselibErrorState wrappedErrorState)
        {
            if (wrappedErrorState.code != 0)
            {
                Binding.Baselib_ErrorState baselib_ErrorState = default(Binding.Baselib_ErrorState);
                baselib_ErrorState.code = (Binding.Baselib_ErrorCode)wrappedErrorState.code;
                baselib_ErrorState.nativeErrorCodeType = (Binding.Baselib_ErrorState_NativeErrorCodeType)wrappedErrorState.nativeErrorCodeType;
                baselib_ErrorState.nativeErrorCode = wrappedErrorState.nativeErrorCode;
                baselib_ErrorState.sourceLocation.file = wrappedErrorState.sourceLocation.file;
                baselib_ErrorState.sourceLocation.function = wrappedErrorState.sourceLocation.function;
                baselib_ErrorState.sourceLocation.lineNumber = wrappedErrorState.sourceLocation.lineNumber;
                Unity.Collections.FixedString512Bytes fs = "Baselib error: ";
                byte* buffer = fs.GetUnsafePtr() + fs.Length;
                int num = fs.Capacity - fs.Length;
                uint num2 = Binding.Baselib_ErrorState_Explain(&baselib_ErrorState, buffer, (uint)num, Binding.Baselib_ErrorState_ExplainVerbosity.ErrorType_SourceLocation_Explanation);
                if (num2 > num)
                {
                    byte* ptr = stackalloc byte[(int)num2];
                    Binding.Baselib_ErrorState_Explain(&baselib_ErrorState, ptr, num2, Binding.Baselib_ErrorState_ExplainVerbosity.ErrorType_SourceLocation_Explanation);
                    fs.Append(ptr, num);
                }
                else
                {
                    fs.Length += (int)num2;
                }

                Debug.LogError(fs);
            }
        }

        private unsafe static BaselibErrorState CreateWrappedBaselibErrorState(Binding.Baselib_ErrorState errorState)
        {
            BaselibSourceLocation sourceLocation = default(BaselibSourceLocation);
            sourceLocation.file = errorState.sourceLocation.file;
            sourceLocation.function = errorState.sourceLocation.function;
            sourceLocation.lineNumber = errorState.sourceLocation.lineNumber;
            BaselibErrorState result = default(BaselibErrorState);
            result.code = (uint)errorState.code;
            result.nativeErrorCodeType = (byte)errorState.nativeErrorCodeType;
            result.nativeErrorCode = errorState.nativeErrorCode;
            result.sourceLocation = sourceLocation;
            return result;
        }

        public unsafe static VMRange ReserveAddressSpace(ulong sizeOfAddressRangeInPages, ulong pageSizeInBytes, out BaselibErrorState outErrorState)
        {
            ulong alignmentInMultipleOfPageSize = 1uL;
            Binding.Baselib_Memory_PageState pageState = Binding.Baselib_Memory_PageState.Reserved;
            Binding.Baselib_ErrorState errorState = default(Binding.Baselib_ErrorState);
            Binding.Baselib_Memory_PageAllocation baselib_Memory_PageAllocation = Binding.Baselib_Memory_AllocatePages(pageSizeInBytes, sizeOfAddressRangeInPages, alignmentInMultipleOfPageSize, pageState, &errorState);
            outErrorState = CreateWrappedBaselibErrorState(errorState);
            VMRange result = default(VMRange);
            result.ptr = baselib_Memory_PageAllocation.ptr;
            result.PageSizeInBytes = (uint)baselib_Memory_PageAllocation.pageSize;
            result.pageCount = (uint)baselib_Memory_PageAllocation.pageCount;
            return result;
        }

        public unsafe static void CommitMemory(VMRange rangeToCommit, out BaselibErrorState outErrorState)
        {
            Binding.Baselib_ErrorState errorState = default(Binding.Baselib_ErrorState);
            Binding.Baselib_Memory_SetPageState(rangeToCommit.ptr, rangeToCommit.PageSizeInBytes, rangeToCommit.pageCount, Binding.Baselib_Memory_PageState.ReadWrite, &errorState);
            outErrorState = CreateWrappedBaselibErrorState(errorState);
        }

        public unsafe static void DecommitMemory(VMRange rangeToFree, out BaselibErrorState outErrorState)
        {
            Binding.Baselib_ErrorState errorState = default(Binding.Baselib_ErrorState);
            Binding.Baselib_Memory_SetPageState(rangeToFree.ptr, rangeToFree.PageSizeInBytes, rangeToFree.pageCount, Binding.Baselib_Memory_PageState.Reserved, &errorState);
            outErrorState = CreateWrappedBaselibErrorState(errorState);
        }

        public unsafe static void FreeAddressSpace(VMRange reservedAddressRange, out BaselibErrorState outErrorState)
        {
            Binding.Baselib_Memory_PageAllocation baselib_Memory_PageAllocation = default(Binding.Baselib_Memory_PageAllocation);
            baselib_Memory_PageAllocation.ptr = reservedAddressRange.ptr;
            baselib_Memory_PageAllocation.pageSize = reservedAddressRange.PageSizeInBytes;
            baselib_Memory_PageAllocation.pageCount = reservedAddressRange.pageCount;
            Binding.Baselib_Memory_PageAllocation pageAllocation = baselib_Memory_PageAllocation;
            Binding.Baselib_ErrorState errorState = default(Binding.Baselib_ErrorState);
            Binding.Baselib_Memory_ReleasePages(pageAllocation, &errorState);
            outErrorState = CreateWrappedBaselibErrorState(errorState);
        }

        public static uint BytesToPageCount(uint bytes, uint pageSizeInBytes)
        {
            return (bytes + pageSizeInBytes - 1) / pageSizeInBytes;
        }
    }

    [BurstCompatible]
    public struct BaselibErrorState
    {
        public uint code;

        public byte nativeErrorCodeType;

        public ulong nativeErrorCode;

        public BaselibSourceLocation sourceLocation;

        public bool Success => code == 0;

        public bool OutOfMemory => code == 16777216;

        public bool InvalidAddressRange => code == 16777218;
    }
    public struct BaselibSourceLocation
    {
        public unsafe byte* file;

        public unsafe byte* function;

        public uint lineNumber;
    }
}