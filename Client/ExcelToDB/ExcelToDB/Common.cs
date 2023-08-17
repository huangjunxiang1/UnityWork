using Microsoft.VisualBasic;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Common
{
    static char[] arraySplit = new char[] { '[', ']', ',' };
    static char[] VectorSplit = new char[] { '(', ')', ',' };
    static char[] VectorArraySplit = new char[] { '[', ']', '(', ')', ',' };

    public static FType GetFtype1(string s)
    {
        if (s.Contains("bool")) return FType.fbool;
        if (s.Contains("int")) return FType.fint;
        if (s.Contains("uint")) return FType.fuint;
        if (s.Contains("long")) return FType.flong;
        if (s.Contains("ulong")) return FType.fulong;
        if (s.Contains("float")) return FType.ffloat;
        if (s.Contains("str")) return FType.fstring;
        if (s.Contains("v2i")) return FType.fv2i;
        if (s.Contains("v3i")) return FType.fv3i;
        if (s.Contains("v2f")) return FType.fv2f;
        if (s.Contains("v3f")) return FType.fv3f;

        throw new Exception("未识别类型=" + s);
    }
    public static FType2 GetFtype2(string s)
    {
        if (s.Contains("[][]")) return FType2.Array2;
        if (s.Contains("[]")) return FType2.Array;
        return FType2.Value;
    }
    public static string GetFTypeStr(string s)
    {
        return s.Replace("str", "string")
            .Replace("v2i", "Vector2Int")
            .Replace("v3i", "Vector3Int")
            .Replace("v2f", "Vector2")
            .Replace("v3f", "Vector3");
    }
    public static string GetFTypeStrECS(string s)
    {
        return s.Replace("str", "string")
            .Replace("v2i", "int2")
            .Replace("v3i", "int3")
            .Replace("v2f", "float2")
            .Replace("v3f", "float3")
            .Replace("[]", "");
    }
    public static string GetFRealType(string s)
    {
        return s.Replace("str", "string")
            .Replace("v2i", "int")
            .Replace("v3i", "int")
            .Replace("v2f", "float")
            .Replace("v3f", "float")
            .Replace("[]", "");
    }
    public static bool GetFv(DField f, string s, out DFieldValue fv)
    {
        fv = new DFieldValue();

        if (string.IsNullOrEmpty(s))
            return true;

        if (f.f1 == FType.fbool && f.f2 == FType2.Value)
        {
            if (!int.TryParse(s, out var v) || (v != 0 && v != 1))
                return false;
            fv.v64 = v;
            return true;
        }
        if (f.f1 == FType.fbool && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
            bool[] temp = new bool[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!int.TryParse(arr[i], out var v) || (v != 0 && v != 1))
                    return false;
                temp[i] = v == 1;
            }
            fv.vo = temp;
            return true;
        }
        if (f.f1 == FType.fbool && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<bool[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<bool[]> arrs = new List<bool[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                bool[] temp = new bool[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v) || (v != 0 && v != 1))
                        return false;
                    temp[i] = v == 1;
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fint && f.f2 == FType2.Value)
        {
            if (!int.TryParse(s, out var v))
                return false;
            fv.v64 = v;
            return true;
        }
        if (f.f1 == FType.fint && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
            int[] temp = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!int.TryParse(arr[i], out var v))
                    return false;
                temp[i] = v;
            }
            fv.vo = temp;
            return true;
        }
        if (f.f1 == FType.fint && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<int[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<int[]> arrs = new List<int[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                int[] temp = new int[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v))
                        return false;
                    temp[i] = v;
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fuint && f.f2 == FType2.Value)
        {
            if (!uint.TryParse(s, out var v))
                return false;
            fv.v64 = v;
            return true;
        }
        if (f.f1 == FType.fuint && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
            uint[] temp = new uint[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!uint.TryParse(arr[i], out var v))
                    return false;
                temp[i] = v;
            }
            fv.vo = temp;
            return true;
        }
        if (f.f1 == FType.fuint && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<uint[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<uint[]> arrs = new List<uint[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                uint[] temp = new uint[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!uint.TryParse(arr[i], out var v))
                        return false;
                    temp[i] = v;
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.flong && f.f2 == FType2.Value)
        {
            if (!long.TryParse(s, out var v))
                return false;
            fv.v64 = v;
            return true;
        }
        if (f.f1 == FType.flong && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
            long[] temp = new long[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!long.TryParse(arr[i], out var v))
                    return false;
                temp[i] = v;
            }
            fv.vo = temp;
            return true;
        }
        if (f.f1 == FType.flong && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<long[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<long[]> arrs = new List<long[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                long[] temp = new long[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!long.TryParse(arr[i], out var v))
                        return false;
                    temp[i] = v;
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fulong && f.f2 == FType2.Value)
        {
            if (!ulong.TryParse(s, out var v))
                return false;
            fv.v64 = (long)v;
            return true;
        }
        if (f.f1 == FType.fulong && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
            ulong[] temp = new ulong[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!ulong.TryParse(arr[i], out var v))
                    return false;
                temp[i] = v;
            }
            fv.vo = temp;
            return true;
        }
        if (f.f1 == FType.fulong && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<ulong[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<ulong[]> arrs = new List<ulong[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                ulong[] temp = new ulong[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!ulong.TryParse(arr[i], out var v))
                        return false;
                    temp[i] = v;
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.ffloat && f.f2 == FType2.Value)
        {
            if (!float.TryParse(s, out var v))
                return false;
            fv.vf = v;
            return true;
        }
        if (f.f1 == FType.ffloat && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
            float[] temp = new float[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!float.TryParse(arr[i], out var v))
                    return false;
                temp[i] = v;
            }
            fv.vo = temp;
            return true;
        }
        if (f.f1 == FType.ffloat && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<float[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<float[]> arrs = new List<float[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                float[] temp = new float[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!float.TryParse(arr[i], out var v))
                        return false;
                    temp[i] = v;
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fstring && f.f2 == FType2.Value)
        {
            fv.vo = s;
            return true;
        }
        if (f.f1 == FType.fstring && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
            fv.vo = arr;
            return true;
        }
        if (f.f1 == FType.fstring && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<string[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<string[]> arrs = new List<string[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                arrs.Add(arr);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fv2i && f.f2 == FType2.Value)
        {
            string[] arr = s.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 2 || !int.TryParse(arr[0], out var v1) || !int.TryParse(arr[1], out var v2))
                return false;
            fv.xi = v1;
            fv.yi = v2;
            return true;
        }
        if (f.f1 == FType.fv2i && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 2 != 0)
                return false;

            int[] fs = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!int.TryParse(arr[i], out var v))
                    return false;
                fs[i] = v;
            }
            fv.vo = fs;
            return true;
        }
        if (f.f1 == FType.fv2i && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<V2int[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<V2int[]> arrs = new List<V2int[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                V2int[] temp = new V2int[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr.Length != 2 || !int.TryParse(arr[0], out var v1) || !int.TryParse(arr[1], out var v2))
                        return false;
                    temp[i] = new V2int { x = v1, y = v2 };
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fv3i && f.f2 == FType2.Value)
        {
            string[] arr = s.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 3 || !int.TryParse(arr[0], out var v1) || !int.TryParse(arr[1], out var v2) || !int.TryParse(arr[2], out var v3))
                return false;
            fv.xi = v1;
            fv.yi = v2;
            fv.zi = v3;
            return true;
        }
        if (f.f1 == FType.fv3i && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 3 != 0)
                return false;

            int[] fs = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!int.TryParse(arr[i], out var v))
                    return false;
                fs[i] = v;
            }
            fv.vo = fs;
            return true;
        }
        if (f.f1 == FType.fv3i && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<V3int[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<V3int[]> arrs = new List<V3int[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                V3int[] temp = new V3int[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr.Length != 3 || !int.TryParse(arr[0], out var v1) || !int.TryParse(arr[1], out var v2) || !int.TryParse(arr[2], out var v3))
                        return false;
                    temp[i] = new V3int { x = v1, y = v2, z = v3 };
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fv2f && f.f2 == FType2.Value)
        {
            string[] arr = s.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 2 || !float.TryParse(arr[0], out var v1) || !float.TryParse(arr[1], out var v2))
                return false;
            fv.xf = v1;
            fv.yf = v2;
            return true;
        }
        if (f.f1 == FType.fv2f && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 2 != 0)
                return false;

            float[] fs = new float[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!float.TryParse(arr[i], out var v))
                    return false;
                fs[i] = v;
            }
            fv.vo = fs;
            return true;
        }
        if (f.f1 == FType.fv2f && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<V2float[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<V2float[]> arrs = new List<V2float[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                V2float[] temp = new V2float[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr.Length != 2 || !float.TryParse(arr[0], out var v1) || !float.TryParse(arr[1], out var v2))
                        return false;
                    temp[i] = new V2float { x = v1, y = v2 };
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        if (f.f1 == FType.fv3f && f.f2 == FType2.Value)
        {
            string[] arr = s.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 3 || !float.TryParse(arr[0], out var v1) || !float.TryParse(arr[1], out var v2) || !float.TryParse(arr[2], out var v3))
                return false;
            fv.xf = v1;
            fv.yf = v2;
            fv.zf = v3;
            return true;
        }
        if (f.f1 == FType.fv3f && f.f2 == FType2.Array)
        {
            string[] arr = s.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 3 != 0)
                return false;

            float[] fs = new float[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                if (!float.TryParse(arr[i], out var v))
                    return false;
                fs[i] = v;
            }
            fv.vo = fs;
            return true;
        }
        if (f.f1 == FType.fv3f && f.f2 == FType2.Array2)
        {
            if (string.IsNullOrEmpty(s))
            {
                fv.vo = Array.Empty<V3float[]>();
                return true;
            }
            if (!s.Contains("[[") || !s.Contains("]]"))
                return false;
            int index = s.IndexOf('[', s.IndexOf('[') + 1);
            int last = s.LastIndexOf(']');
            List<V3float[]> arrs = new List<V3float[]>();
            while (true)
            {
                int index2 = s.IndexOf(']', index);
                string ss = s.Substring(index, index2 - index + 1);

                string[] arr = ss.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);
                V3float[] temp = new V3float[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr.Length != 3 || !float.TryParse(arr[0], out var v1) || !float.TryParse(arr[1], out var v2) || !float.TryParse(arr[2], out var v3))
                        return false;
                    temp[i] = new V3float { x = v1, y = v2, z = v3 };
                }
                arrs.Add(temp);

                index = index2 + 1;
                if (index >= last)
                    break;
            }
            fv.vo = arrs.ToArray();
            return true;
        }

        throw new Exception("未识别类型=" + f.typeStr);
    }
    public static void WriteFv(DField f, DFieldValue fv, DBuffer buffer, DBuffer arrTemp)
    {
        if (f.f2 == FType2.Value)
        {
            switch (f.f1)
            {
                case FType.fbool:
                    buffer.Write(fv.v64 == 1);
                    break;
                case FType.fint:
                    buffer.Write((int)fv.v64);
                    break;
                case FType.fuint:
                    buffer.Write((uint)fv.v64);
                    break;
                case FType.flong:
                    buffer.Write((long)fv.v64);
                    break;
                case FType.fulong:
                    buffer.Write((ulong)fv.v64);
                    break;
                case FType.ffloat:
                    buffer.Write(fv.vf);
                    break;
                case FType.fstring:
                    buffer.Write((string)fv.vo);
                    break;
                case FType.fv2i:
                    buffer.Write(fv.xi);
                    buffer.Write(fv.yi);
                    break;
                case FType.fv3i:
                    buffer.Write(fv.xi);
                    buffer.Write(fv.yi);
                    buffer.Write(fv.zi);
                    break;
                case FType.fv2f:
                    buffer.Write(fv.xf);
                    buffer.Write(fv.yf);
                    break;
                case FType.fv3f:
                    buffer.Write(fv.xf);
                    buffer.Write(fv.yf);
                    buffer.Write(fv.zf);
                    break;
                default:
                    throw new($"写入值错误  类型：{f.f1} {f.f2}");
            }
        }
        else if (f.f2 == FType2.Array)
        {
            if (arrTemp!=null)
            {
                arrTemp.Seek(0);
                switch (f.f1)
                {
                    case FType.fbool:
                        arrTemp.Write((bool[])fv.vo);
                        break;
                    case FType.fint:
                        arrTemp.Write((int[])fv.vo);
                        break;
                    case FType.fuint:
                        arrTemp.Write((uint[])fv.vo);
                        break;
                    case FType.flong:
                        arrTemp.Write((long[])fv.vo);
                        break;
                    case FType.fulong:
                        arrTemp.Write((ulong[])fv.vo);
                        break;
                    case FType.ffloat:
                        arrTemp.Write((float[])fv.vo);
                        break;
                    case FType.fstring:
                        arrTemp.Write((string[])fv.vo);
                        break;
                    case FType.fv2i:
                        {
                            int[] arr = (int[])fv.vo;
                            arrTemp.Write(arr.Length / 2);
                            for (int i = 0; i < arr.Length; i++)
                                arrTemp.Write(arr[i]);
                        }
                        break;
                    case FType.fv3i:
                        {
                            int[] arr = (int[])fv.vo;
                            arrTemp.Write(arr.Length / 3);
                            for (int i = 0; i < arr.Length; i++)
                                arrTemp.Write(arr[i]);
                        }
                        break;
                    case FType.fv2f:
                        {
                            float[] arr = (float[])fv.vo;
                            arrTemp.Write(arr.Length / 2);
                            for (int i = 0; i < arr.Length; i++)
                                arrTemp.Write(arr[i]);
                        }
                        break;
                    case FType.fv3f:
                        {
                            float[] arr = (float[])fv.vo;
                            arrTemp.Write(arr.Length / 3);
                            for (int i = 0; i < arr.Length; i++)
                                arrTemp.Write(arr[i]);
                        }
                        break;
                    default:
                        throw new($"写入值错误  类型：{f.f1} {f.f2}");
                }
                buffer.Write(arrTemp);
            }
            else
            {
                switch (f.f1)
                {
                    case FType.fbool:
                        buffer.Write((bool[])fv.vo);
                        break;
                    case FType.fint:
                        buffer.Write((int[])fv.vo);
                        break;
                    case FType.fuint:
                        buffer.Write((uint[])fv.vo);
                        break;
                    case FType.flong:
                        buffer.Write((long[])fv.vo);
                        break;
                    case FType.fulong:
                        buffer.Write((ulong[])fv.vo);
                        break;
                    case FType.ffloat:
                        buffer.Write((float[])fv.vo);
                        break;
                    case FType.fv2i:
                        {
                            int[] arr = (int[])fv.vo;
                            buffer.Write(arr.Length / 2);
                            for (int i = 0; i < arr.Length; i++)
                                buffer.Write(arr[i]);
                        }
                        break;
                    case FType.fv3i:
                        {
                            int[] arr = (int[])fv.vo;
                            buffer.Write(arr.Length / 3);
                            for (int i = 0; i < arr.Length; i++)
                                buffer.Write(arr[i]);
                        }
                        break;
                    case FType.fv2f:
                        {
                            float[] arr = (float[])fv.vo;
                            buffer.Write(arr.Length / 2);
                            for (int i = 0; i < arr.Length; i++)
                                buffer.Write(arr[i]);
                        }
                        break;
                    case FType.fv3f:
                        {
                            float[] arr = (float[])fv.vo;
                            buffer.Write(arr.Length / 3);
                            for (int i = 0; i < arr.Length; i++)
                                buffer.Write(arr[i]);
                        }
                        break;
                    default:
                        throw new($"不支持的ECS类型：{f.f1} {f.f2}");
                }
            }
        }

        else if (f.f2 == FType2.Array2)
        {
            if (arrTemp != null)
            {
                arrTemp.Seek(0);
                switch (f.f1)
                {
                    case FType.fbool:
                        arrTemp.Write((bool[][])fv.vo);
                        break;
                    case FType.fint:
                        arrTemp.Write((int[][])fv.vo);
                        break;
                    case FType.fuint:
                        arrTemp.Write((uint[][])fv.vo);
                        break;
                    case FType.flong:
                        arrTemp.Write((long[][])fv.vo);
                        break;
                    case FType.fulong:
                        arrTemp.Write((ulong[][])fv.vo);
                        break;
                    case FType.ffloat:
                        arrTemp.Write((float[][])fv.vo);
                        break;
                    case FType.fstring:
                        arrTemp.Write((string[][])fv.vo);
                        break;
                    case FType.fv2i:
                        {
                            V2int[][] arr = (V2int[][])fv.vo;
                            arrTemp.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                arrTemp.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    arrTemp.Write(arr[i][j].x);
                                    arrTemp.Write(arr[i][j].y);
                                }
                            }
                        }
                        break;
                    case FType.fv3i:
                        {
                            V3int[][] arr = (V3int[][])fv.vo;
                            arrTemp.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                arrTemp.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    arrTemp.Write(arr[i][j].x);
                                    arrTemp.Write(arr[i][j].y);
                                    arrTemp.Write(arr[i][j].z);
                                }
                            }
                        }
                        break;
                    case FType.fv2f:
                        {
                            V2float[][] arr = (V2float[][])fv.vo;
                            arrTemp.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                arrTemp.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    arrTemp.Write(arr[i][j].x);
                                    arrTemp.Write(arr[i][j].y);
                                }
                            }
                        }
                        break;
                    case FType.fv3f:
                        {
                            V3float[][] arr = (V3float[][])fv.vo;
                            arrTemp.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                arrTemp.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    arrTemp.Write(arr[i][j].x);
                                    arrTemp.Write(arr[i][j].y);
                                    arrTemp.Write(arr[i][j].z);
                                }
                            }
                        }
                        break;
                    default:
                        throw new($"写入值错误  类型：{f.f1} {f.f2}");
                }
                buffer.Write(arrTemp);
            }
            else
            {
                switch (f.f1)
                {
                    case FType.fbool:
                        buffer.Write((bool[][])fv.vo);
                        break;
                    case FType.fint:
                        buffer.Write((int[][])fv.vo);
                        break;
                    case FType.fuint:
                        buffer.Write((uint[][])fv.vo);
                        break;
                    case FType.flong:
                        buffer.Write((long[][])fv.vo);
                        break;
                    case FType.fulong:
                        buffer.Write((ulong[][])fv.vo);
                        break;
                    case FType.ffloat:
                        buffer.Write((float[][])fv.vo);
                        break;
                    case FType.fv2i:
                        {
                            V2int[][] arr = (V2int[][])fv.vo;
                            buffer.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                buffer.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    buffer.Write(arr[i][j].x);
                                    buffer.Write(arr[i][j].y);
                                }
                            }
                        }
                        break;
                    case FType.fv3i:
                        {
                            V3int[][] arr = (V3int[][])fv.vo;
                            buffer.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                buffer.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    buffer.Write(arr[i][j].x);
                                    buffer.Write(arr[i][j].y);
                                    buffer.Write(arr[i][j].z);
                                }
                            }
                        }
                        break;
                    case FType.fv2f:
                        {
                            V2float[][] arr = (V2float[][])fv.vo;
                            buffer.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                buffer.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    buffer.Write(arr[i][j].x);
                                    buffer.Write(arr[i][j].y);
                                }
                            }
                        }
                        break;
                    case FType.fv3f:
                        {
                            V3float[][] arr = (V3float[][])fv.vo;
                            buffer.Write(arr.Length);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                buffer.Write(arr[i].Length);
                                for (int j = 0; j < arr[i].Length; j++)
                                {
                                    buffer.Write(arr[i][j].x);
                                    buffer.Write(arr[i][j].y);
                                    buffer.Write(arr[i][j].z);
                                }
                            }
                        }
                        break;
                    default:
                        throw new($"不支持的ECS类型：{f.f1} {f.f2}");
                }
            }
        }
        else
            throw new($"写入值错误  类型：{f.f1} {f.f2}");
    }


    public static string getType(string sType)
    {
        if (sType == "v2") sType = "Vector2";
        else if (sType == "v2[]") sType = "Vector2[]";

        else if (sType == "v2i") sType = "Vector2Int";
        else if (sType == "v2i[]") sType = "Vector2Int[]";

        else if (sType == "v3") sType = "Vector3";
        else if (sType == "v3[]") sType = "Vector3[]";

        else if (sType == "v3i") sType = "Vector3Int";
        else if (sType == "v3i[]") sType = "Vector3Int[]";

        else if (sType == "str") sType = "string";
        else if (sType == "str[]") sType = "string[]";

        return sType;
    }

    public static void WriteValue(DBuffer buffer, DBuffer arrayTemp, string sType, string text, FileInfo fi, int lineIdx, int idx)
    {
        ///arrayTemp  如果写入的是数组 arrayTemp不为空时  数据先写入数组的数据总大小 arrayTemp为空则不写入数组的数据总大小
        sType = getType(sType);
        if (sType == "bool")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(false);
                return;
            }
            if (!int.TryParse(text, out var v) || (v != 0 && v != 1))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v == 1);
        }
        else if (sType == "bool[]")
        {
            string[] arr = text.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);

            bool[] temp = new bool[arr.Length];
            if (arrayTemp != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v) || (v != 0 && v != 1))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    temp[i] = v == 1;
                }
                arrayTemp.Seek(0);
                arrayTemp.Write(temp);
                buffer.Write(arrayTemp);
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v) || (v != 0 && v != 1))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    temp[i] = v == 1;
                }
                buffer.Write(temp);
            }
        }

        else if (sType == "byte")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write((byte)0);
                return;
            }
            if (!byte.TryParse(text, out var v))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v);
        }
        else if (sType == "byte[]")
        {
            string[] arr = text.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!byte.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!byte.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else if (sType == "int")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                return;
            }
            if (!int.TryParse(text, out var v))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v);
        }
        else if (sType == "int[]")
        {
            string[] arr = text.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else if (sType == "uint")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0u);
                return;
            }
            if (!uint.TryParse(text, out var v))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v);
        }
        else if (sType == "uint[]")
        {
            string[] arr = text.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!uint.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!uint.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else if (sType == "long")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                return;
            }
            if (!long.TryParse(text, out var v))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v);
        }
        else if (sType == "long[]")
        {
            string[] arr = text.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!long.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!long.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else if (sType == "string")
        {
            buffer.Write(text);
        }
        else if (sType == "string[]")
        {
            string[] arr = text.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                    arrayTemp.Write(arr[i]);
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                    buffer.Write(arr[i]);
            }
        }

        else if (sType == "Vector2")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2 || !float.TryParse(s[0], out var v1) || !float.TryParse(s[1], out var v2))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
        }
        else if (sType == "Vector2[]")
        {
            string[] arr = text.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 2 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                }
            }
        }

        else if (sType == "Vector2Int")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2 || !int.TryParse(s[0], out var v1) || !int.TryParse(s[1], out var v2))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
        }
        else if (sType == "Vector2Int[]")
        {
            string[] arr = text.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 2 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                }
            }
        }

        else if (sType == "Vector3")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 3 || !float.TryParse(s[0], out var v1) || !float.TryParse(s[1], out var v2) || !float.TryParse(s[2], out var v3))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
            buffer.Write(v3);
        }
        else if (sType == "Vector3[]")
        {
            string[] arr = text.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 3 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2) || !float.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                    arrayTemp.Write(v3);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2) || !float.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                    buffer.Write(v3);
                }
            }
        }

        else if (sType == "Vector3Int")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(VectorSplit, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 3 || !int.TryParse(s[0], out var v1) || !int.TryParse(s[1], out var v2) || !int.TryParse(s[2], out var v3))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
            buffer.Write(v3);
        }
        else if (sType == "Vector3Int[]")
        {
            string[] arr = text.Split(VectorArraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 3 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2) || !int.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                    arrayTemp.Write(v3);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2) || !int.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                    buffer.Write(v3);
                }
            }
        }

        else if (sType == "float")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0f);
                return;
            }
            if (!float.TryParse(text, out var v))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v);
        }
        else if (sType == "float[]")
        {
            string[] arr = text.Split(arraySplit, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!float.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!float.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else
        {
            Console.WriteLine("未识别类型->" + sType);
            Console.ReadLine();
        }
    }

    public static List<string> getFiles(string path)
    {
        List<string> files = new List<string>();
        string[] s = Directory.GetFiles(path);
        for (int i = 0; i < s.Length; i++)
        {
            FileInfo fi = new FileInfo(s[i]);
            if (fi.Name.StartsWith("~")) continue;
            if (fi.Extension != ".xls" && fi.Extension != ".xlsx") continue;
            files.Add(s[i]);
        }
        return files;
    }

    //解析头 获得类型和字段名
    public static void GetHead(List<int> coIdx, ExcelPackage pkg, out temp1 t)
    {
        t = new temp1();
        t.keyType = pkg.Workbook.Worksheets[0].Cells[2, 1].Text;
        t.keyName = pkg.Workbook.Worksheets[0].Cells[3, 1].Text;
        if (string.IsNullOrEmpty(t.keyType) || string.IsNullOrEmpty(t.keyName))
        {
            Console.WriteLine(pkg.File.Name + "表没有key索引");
            Console.ReadLine();
            return;
        }
        var array = (object[,])pkg.Workbook.Worksheets[0].Cells.Value;
        int len = array.GetLength(1);
        for (int i = 1; i <= len; i++)
        {
            if (string.IsNullOrEmpty(pkg.Workbook.Worksheets[0].Cells[2, i].Text))
                continue;
            if (string.IsNullOrEmpty(pkg.Workbook.Worksheets[0].Cells[3, i].Text))
                continue;
            if (i > 1)
            {
                string s = pkg.Workbook.Worksheets[0].Cells[4, i].Text.ToLower();
                if (string.IsNullOrEmpty(s) || !s.Contains('c'))
                    continue;
            }
            coIdx.Add(i);
        }
    }

}
class temp1
{
    public string keyType;
    public string keyName;
}
class temp3
{
    public FileInfo fi;
    public List<int> dataLines;
    public int keyType;//0=int 1=string
}
class Lan
{
    public DBuffer buff_int;
    public DBuffer buff_str;
    public string name;
}
