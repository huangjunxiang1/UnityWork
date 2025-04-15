using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Shit.Runtime.dll",
		"Unity.Burst.dll",
		"Unity.Collections.dll",
		"Unity.Entities.dll",
		"Unity.InputSystem.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Core.STree.<>c__19<object>
	// STask<System.ValueTuple<Unity.Entities.Entity,object>>
	// STask<Unity.Entities.Entity>
	// STask<object>
	// STaskBuilder1<object>
	// System.Action<UnityEngine.InputSystem.InputAction.CallbackContext>
	// System.Action<byte>
	// System.Action<object,object>
	// System.Action<object>
	// System.ByReference<System.IntPtr>
	// System.ByReference<Unity.Entities.Entity>
	// System.ByReference<Unity.Entities.PerWorldSystemInfo>
	// System.ByReference<Unity.Mathematics.float2>
	// System.ByReference<int>
	// System.ByReference<long>
	// System.ByReference<ushort>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<Unity.Entities.Entity>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.ComparisonComparer<Unity.Entities.Entity>
	// System.Collections.Generic.ComparisonComparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<Core.EventSystem.EventKey,object>
	// System.Collections.Generic.Dictionary.Enumerator<int,TabMapping>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<Core.EventSystem.EventKey,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,TabMapping>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<Core.EventSystem.EventKey,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,TabMapping>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<Core.EventSystem.EventKey,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,TabMapping>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<Core.EventSystem.EventKey,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,TabMapping>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<Core.EventSystem.EventKey,object>
	// System.Collections.Generic.Dictionary<int,TabMapping>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<Core.EventSystem.EventKey>
	// System.Collections.Generic.EqualityComparer<TabMapping>
	// System.Collections.Generic.EqualityComparer<Unity.Entities.Entity>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Core.EventSystem.EventKey,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,TabMapping>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<Core.EventSystem.EventKey,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,TabMapping>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<Core.EventSystem.EventKey,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,TabMapping>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<Core.EventSystem.EventKey>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<Core.EventSystem.EventKey,object>
	// System.Collections.Generic.KeyValuePair<int,TabMapping>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<Unity.Entities.Entity>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<Core.EventSystem.EventKey>
	// System.Collections.Generic.ObjectEqualityComparer<TabMapping>
	// System.Collections.Generic.ObjectEqualityComparer<Unity.Entities.Entity>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<Unity.Entities.Entity>
	// System.Comparison<object>
	// System.Func<byte>
	// System.Func<object,byte>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.IEquatable<int>
	// System.IEquatable<long>
	// System.Nullable<byte>
	// System.Nullable<int>
	// System.Predicate<object>
	// System.ReadOnlySpan<System.IntPtr>
	// System.ReadOnlySpan<Unity.Entities.Entity>
	// System.ReadOnlySpan<Unity.Entities.PerWorldSystemInfo>
	// System.ReadOnlySpan<Unity.Mathematics.float2>
	// System.ReadOnlySpan<int>
	// System.ReadOnlySpan<long>
	// System.ReadOnlySpan<ushort>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<byte>
	// System.Runtime.CompilerServices.TaskAwaiter<byte>
	// System.Span<System.IntPtr>
	// System.Span<Unity.Entities.Entity>
	// System.Span<Unity.Entities.PerWorldSystemInfo>
	// System.Span<Unity.Mathematics.float2>
	// System.Span<int>
	// System.Span<long>
	// System.Span<ushort>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<byte>
	// System.Threading.Tasks.Task<byte>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<byte>
	// System.Threading.Tasks.TaskFactory<byte>
	// System.ValueTuple<Unity.Entities.Entity,object>
	// UIPropertyBinding.<>c__DisplayClass5_0<object,object,object>
	// UIPropertyBinding<object,byte>
	// UIPropertyBinding<object,object>
	// Unity.Burst.SharedStatic<TabM_ST>
	// Unity.Burst.SharedStatic<Unity.Entities.TypeIndex>
	// Unity.Burst.SharedStatic<int>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList.ParallelReader<Unity.Entities.Entity>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList.ParallelReader<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList.ParallelWriter<Unity.Entities.Entity>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList.ParallelWriter<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList.ReadOnly<Unity.Entities.Entity>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList.ReadOnly<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList<Unity.Entities.Entity>
	// Unity.Collections.LowLevel.Unsafe.UnsafeList<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.LowLevel.Unsafe.UnsafeParallelHashMap.ReadOnly<int,System.IntPtr>
	// Unity.Collections.LowLevel.Unsafe.UnsafeParallelHashMap<int,System.IntPtr>
	// Unity.Collections.LowLevel.Unsafe.UnsafeParallelHashMapBase<int,System.IntPtr>
	// Unity.Collections.LowLevel.Unsafe.UnsafeParallelHashMapBase<long,ushort>
	// Unity.Collections.LowLevel.Unsafe.UnsafeParallelMultiHashMap.ReadOnly<long,ushort>
	// Unity.Collections.LowLevel.Unsafe.UnsafeParallelMultiHashMap<long,ushort>
	// Unity.Collections.NativeArray.Enumerator<System.IntPtr>
	// Unity.Collections.NativeArray.Enumerator<Unity.Entities.Entity>
	// Unity.Collections.NativeArray.Enumerator<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.NativeArray.Enumerator<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray.Enumerator<int>
	// Unity.Collections.NativeArray.Enumerator<long>
	// Unity.Collections.NativeArray.Enumerator<ushort>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<System.IntPtr>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Entities.Entity>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<int>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<long>
	// Unity.Collections.NativeArray.ReadOnly.Enumerator<ushort>
	// Unity.Collections.NativeArray.ReadOnly<System.IntPtr>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Entities.Entity>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.NativeArray.ReadOnly<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray.ReadOnly<int>
	// Unity.Collections.NativeArray.ReadOnly<long>
	// Unity.Collections.NativeArray.ReadOnly<ushort>
	// Unity.Collections.NativeArray<System.IntPtr>
	// Unity.Collections.NativeArray<Unity.Entities.Entity>
	// Unity.Collections.NativeArray<Unity.Entities.PerWorldSystemInfo>
	// Unity.Collections.NativeArray<Unity.Mathematics.float2>
	// Unity.Collections.NativeArray<int>
	// Unity.Collections.NativeArray<long>
	// Unity.Collections.NativeArray<ushort>
	// Unity.Collections.NativeKeyValueArrays<int,System.IntPtr>
	// Unity.Collections.NativeKeyValueArrays<long,ushort>
	// Unity.Collections.NativeList.ParallelWriter<Unity.Entities.Entity>
	// Unity.Collections.NativeList<Unity.Entities.Entity>
	// Unity.Collections.NativeParallelHashMap.ReadOnly<int,System.IntPtr>
	// Unity.Collections.NativeParallelHashMap<int,System.IntPtr>
	// Unity.Entities.TypeManager.SharedTypeIndex<HDRPMaterialPropertyEmissiveColor2>
	// UnityEngine.InputSystem.InputBindingComposite<UnityEngine.Vector2>
	// UnityEngine.InputSystem.InputControl<UnityEngine.Vector2>
	// UnityEngine.InputSystem.InputProcessor<UnityEngine.Vector2>
	// UnityEngine.InputSystem.Utilities.InlinedArray<object>
	// UnityEngine.InputSystem.Utilities.ReadOnlyArray.Enumerator<object>
	// UnityEngine.InputSystem.Utilities.ReadOnlyArray<object>
	// }}

	public void RefMethods()
	{
		// System.Void Core.EventSystem.RigisteEvent<object>(System.Action<object>,long,int)
		// System.Void Core.EventSystem.RunEvent<object>(object,long,long,int)
		// STask Core.EventSystem.RunEventAsync<object>(object,long,long,int)
		// System.Void Core.EventSystem.EvtQueue.Run<object>(object)
		// STask Core.EventSystem.EvtQueue.RunAsync<object>(object)
		// object Core.SObject.AddComponent<object>()
		// object Core.SObject.AddComponent<object>(object)
		// object Core.STree.GetChild<object>()
		// object Game.SAsset.Load<object>(string)
		// STask<object> Game.SAsset.LoadAsync<object>(string)
		// STask<object> Game.UIManager.OpenAsync<object>(object[])
		// System.Void STaskBuilder.AwaitUnsafeOnCompleted<object,FUIFighting4.<OnTask>d__14>(object&,FUIFighting4.<OnTask>d__14&)
		// System.Void STaskBuilder.AwaitUnsafeOnCompleted<object,FUILoading.<EC_InScene>d__1>(object&,FUILoading.<EC_InScene>d__1&)
		// System.Void STaskBuilder.AwaitUnsafeOnCompleted<object,FUILoading.<EC_OutScene>d__0>(object&,FUILoading.<EC_OutScene>d__0&)
		// System.Void STaskBuilder.AwaitUnsafeOnCompleted<object,FUIRooms.<OnTask>d__0>(object&,FUIRooms.<OnTask>d__0&)
		// System.Void STaskBuilder.AwaitUnsafeOnCompleted<object,Handler.<Init>d__1>(object&,Handler.<Init>d__1&)
		// System.Void STaskBuilder.AwaitUnsafeOnCompleted<object,UIPkg.<Init>d__12>(object&,UIPkg.<Init>d__12&)
		// System.Void STaskBuilder.AwaitUnsafeOnCompleted<object,UUILoading.<EC_InScene>d__1>(object&,UUILoading.<EC_InScene>d__1&)
		// System.Void STaskBuilder.Start<Core.EventSystem.EvtQueue.<RunAsync>d__7<object>>(Core.EventSystem.EvtQueue.<RunAsync>d__7<object>&)
		// System.Void STaskBuilder.Start<FUIFighting4.<OnTask>d__14>(FUIFighting4.<OnTask>d__14&)
		// System.Void STaskBuilder.Start<FUILoading.<EC_InScene>d__1>(FUILoading.<EC_InScene>d__1&)
		// System.Void STaskBuilder.Start<FUILoading.<EC_OutScene>d__0>(FUILoading.<EC_OutScene>d__0&)
		// System.Void STaskBuilder.Start<FUIRooms.<OnTask>d__0>(FUIRooms.<OnTask>d__0&)
		// System.Void STaskBuilder.Start<Handler.<Init>d__1>(Handler.<Init>d__1&)
		// System.Void STaskBuilder.Start<UIPkg.<Init>d__12>(UIPkg.<Init>d__12&)
		// System.Void STaskBuilder.Start<UUILoading.<EC_InScene>d__1>(UUILoading.<EC_InScene>d__1&)
		// System.Void STaskBuilder.Start<UUILoading.<EC_OutScene>d__0>(UUILoading.<EC_OutScene>d__0&)
		// System.Void STaskBuilder1<object>.Start<Game.SAsset.<LoadAsync>d__9<object>>(Game.SAsset.<LoadAsync>d__9<object>&)
		// System.Void STaskBuilder1<object>.Start<Game.UIManager.<OpenAsync>d__5<object>>(Game.UIManager.<OpenAsync>d__5<object>&)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,FUILogin.<login>d__4>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,FUILogin.<login>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUIFighting.<OnEnter>d__1>(object&,FUIFighting.<OnEnter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUIFighting2.<OnEnter>d__5>(object&,FUIFighting2.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUIFighting3.<OnEnter>d__6>(object&,FUIFighting3.<OnEnter>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUIGame.<_gen_cube>d__7>(object&,FUIGame.<_gen_cube>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUILogin.<EC_InScene>d__0>(object&,FUILogin.<EC_InScene>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUILogin.<asServer>d__6>(object&,FUILogin.<asServer>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUILogin.<enter>d__5>(object&,FUILogin.<enter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUILogin.<login>d__4>(object&,FUILogin.<login>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUIRooms.<create>d__4>(object&,FUIRooms.<create>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUIRooms.<join>d__5>(object&,FUIRooms.<join>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,FUIRooms.<refRoom>d__3>(object&,FUIRooms.<refRoom>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,GLoader3DPropertyBinding.<View>d__1>(object&,GLoader3DPropertyBinding.<View>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,Program.<Main>d__0>(object&,Program.<Main>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,SettingL.<loadLocationText>d__9>(object&,SettingL.<loadLocationText>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,UIPkg.<fguiLoader>d__13>(object&,UIPkg.<fguiLoader>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<object,UUILogin.<onValue>d__3>(object&,UUILogin.<onValue>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Core.EventSystem.EvtQueue.<Run>d__6<object>>(Core.EventSystem.EvtQueue.<Run>d__6<object>&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUIFighting.<OnEnter>d__1>(FUIFighting.<OnEnter>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUIFighting2.<OnEnter>d__5>(FUIFighting2.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUIFighting3.<OnEnter>d__6>(FUIFighting3.<OnEnter>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUIGame.<_gen_cube>d__7>(FUIGame.<_gen_cube>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUILogin.<EC_InScene>d__0>(FUILogin.<EC_InScene>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUILogin.<asServer>d__6>(FUILogin.<asServer>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUILogin.<enter>d__5>(FUILogin.<enter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUILogin.<login>d__4>(FUILogin.<login>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUIRooms.<create>d__4>(FUIRooms.<create>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUIRooms.<join>d__5>(FUIRooms.<join>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<FUIRooms.<refRoom>d__3>(FUIRooms.<refRoom>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GLoader3DPropertyBinding.<View>d__1>(GLoader3DPropertyBinding.<View>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Program.<Main>d__0>(Program.<Main>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<SettingL.<loadLocationText>d__9>(SettingL.<loadLocationText>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UIPkg.<fguiLoader>d__13>(UIPkg.<fguiLoader>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UUILogin.<onValue>d__3>(UUILogin.<onValue>d__3&)
		// System.Void UIPropertyBinding<object,object>.Binding<object>(System.Func<object,object>)
		// long Unity.Burst.BurstRuntime.GetHashCode64<Demo3Sys>()
		// System.Void* Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<Unity.Mathematics.float2>(Unity.Collections.NativeArray<Unity.Mathematics.float2>)
		// System.Void* Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr<int>(Unity.Collections.NativeArray<int>)
		// System.Void* Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf<UnityEngine.Vector2>(UnityEngine.Vector2&)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.CopyStructureToPtr<AI_XunLuo>(AI_XunLuo&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.CopyStructureToPtr<Demo1Delay>(Demo1Delay&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.CopyStructureToPtr<Demo3Com>(Demo3Com&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.CopyStructureToPtr<HDRPMaterialPropertyEmissiveColor2>(HDRPMaterialPropertyEmissiveColor2&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.CopyStructureToPtr<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>(Unity.Rendering.HDRPMaterialPropertyEmissiveColor&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.CopyStructureToPtr<Unity.Transforms.LocalToWorld>(Unity.Transforms.LocalToWorld&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.InternalCopyStructureToPtr<AI_XunLuo>(AI_XunLuo&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.InternalCopyStructureToPtr<Demo1Delay>(Demo1Delay&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.InternalCopyStructureToPtr<Demo3Com>(Demo3Com&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.InternalCopyStructureToPtr<HDRPMaterialPropertyEmissiveColor2>(HDRPMaterialPropertyEmissiveColor2&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.InternalCopyStructureToPtr<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>(Unity.Rendering.HDRPMaterialPropertyEmissiveColor&,System.Void*)
		// System.Void Unity.Collections.LowLevel.Unsafe.UnsafeUtility.InternalCopyStructureToPtr<Unity.Transforms.LocalToWorld>(Unity.Transforms.LocalToWorld&,System.Void*)
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<Demo3Sys>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<Unity.Mathematics.float2>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<UnityEngine.Vector2>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<int>()
		// Unity.Entities.ComponentType Unity.Entities.ComponentType.ReadWrite<AI_XunLuo>()
		// Unity.Entities.ComponentType Unity.Entities.ComponentType.ReadWrite<Demo1Delay>()
		// Unity.Entities.ComponentType Unity.Entities.ComponentType.ReadWrite<Demo3Com>()
		// Unity.Entities.ComponentType Unity.Entities.ComponentType.ReadWrite<HDRPMaterialPropertyEmissiveColor2>()
		// Unity.Entities.ComponentType Unity.Entities.ComponentType.ReadWrite<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>()
		// System.Void Unity.Entities.EntityDataAccess.SetComponentData<AI_XunLuo>(Unity.Entities.Entity,AI_XunLuo,Unity.Entities.SystemHandle&)
		// System.Void Unity.Entities.EntityDataAccess.SetComponentData<Demo1Delay>(Unity.Entities.Entity,Demo1Delay,Unity.Entities.SystemHandle&)
		// System.Void Unity.Entities.EntityDataAccess.SetComponentData<Demo3Com>(Unity.Entities.Entity,Demo3Com,Unity.Entities.SystemHandle&)
		// System.Void Unity.Entities.EntityDataAccess.SetComponentData<HDRPMaterialPropertyEmissiveColor2>(Unity.Entities.Entity,HDRPMaterialPropertyEmissiveColor2,Unity.Entities.SystemHandle&)
		// System.Void Unity.Entities.EntityDataAccess.SetComponentData<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>(Unity.Entities.Entity,Unity.Rendering.HDRPMaterialPropertyEmissiveColor,Unity.Entities.SystemHandle&)
		// System.Void Unity.Entities.EntityDataAccess.SetComponentData<Unity.Transforms.LocalToWorld>(Unity.Entities.Entity,Unity.Transforms.LocalToWorld,Unity.Entities.SystemHandle&)
		// bool Unity.Entities.EntityManager.AddComponentData<AI_XunLuo>(Unity.Entities.Entity,AI_XunLuo)
		// bool Unity.Entities.EntityManager.AddComponentData<Demo1Delay>(Unity.Entities.Entity,Demo1Delay)
		// bool Unity.Entities.EntityManager.AddComponentData<Demo3Com>(Unity.Entities.Entity,Demo3Com)
		// bool Unity.Entities.EntityManager.AddComponentData<HDRPMaterialPropertyEmissiveColor2>(Unity.Entities.Entity,HDRPMaterialPropertyEmissiveColor2)
		// bool Unity.Entities.EntityManager.AddComponentData<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>(Unity.Entities.Entity,Unity.Rendering.HDRPMaterialPropertyEmissiveColor)
		// System.Void Unity.Entities.EntityManager.SetComponentData<AI_XunLuo>(Unity.Entities.Entity,AI_XunLuo)
		// System.Void Unity.Entities.EntityManager.SetComponentData<Demo1Delay>(Unity.Entities.Entity,Demo1Delay)
		// System.Void Unity.Entities.EntityManager.SetComponentData<Demo3Com>(Unity.Entities.Entity,Demo3Com)
		// System.Void Unity.Entities.EntityManager.SetComponentData<HDRPMaterialPropertyEmissiveColor2>(Unity.Entities.Entity,HDRPMaterialPropertyEmissiveColor2)
		// System.Void Unity.Entities.EntityManager.SetComponentData<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>(Unity.Entities.Entity,Unity.Rendering.HDRPMaterialPropertyEmissiveColor)
		// System.Void Unity.Entities.EntityManager.SetComponentData<Unity.Transforms.LocalToWorld>(Unity.Entities.Entity,Unity.Transforms.LocalToWorld)
		// Unity.Entities.SystemTypeIndex Unity.Entities.TypeManager.GetSystemTypeIndex<Demo3Sys>()
		// Unity.Entities.TypeIndex Unity.Entities.TypeManager.GetTypeIndex<AI_XunLuo>()
		// Unity.Entities.TypeIndex Unity.Entities.TypeManager.GetTypeIndex<Demo1Delay>()
		// Unity.Entities.TypeIndex Unity.Entities.TypeManager.GetTypeIndex<Demo3Com>()
		// Unity.Entities.TypeIndex Unity.Entities.TypeManager.GetTypeIndex<HDRPMaterialPropertyEmissiveColor2>()
		// Unity.Entities.TypeIndex Unity.Entities.TypeManager.GetTypeIndex<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>()
		// Unity.Entities.TypeIndex Unity.Entities.TypeManager.GetTypeIndex<Unity.Transforms.LocalToWorld>()
		// System.Void Unity.Entities.TypeManager.ManagedException<AI_XunLuo>()
		// System.Void Unity.Entities.TypeManager.ManagedException<Demo1Delay>()
		// System.Void Unity.Entities.TypeManager.ManagedException<Demo3Com>()
		// System.Void Unity.Entities.TypeManager.ManagedException<Demo3Sys>()
		// System.Void Unity.Entities.TypeManager.ManagedException<HDRPMaterialPropertyEmissiveColor2>()
		// System.Void Unity.Entities.TypeManager.ManagedException<Unity.Rendering.HDRPMaterialPropertyEmissiveColor>()
		// System.Void Unity.Entities.TypeManager.ManagedException<Unity.Transforms.LocalToWorld>()
		// object Unity.Entities.World.GetExistingSystemManaged<object>()
		// Unity.Entities.SystemHandle Unity.Entities.WorldExtensions.CreateSystem<Demo3Sys>(Unity.Entities.World)
		// Unity.Entities.SystemHandle Unity.Entities.WorldUnmanaged.CreateUnmanagedSystem<Demo3Sys>(Unity.Entities.World,bool)
		// Unity.Entities.SystemHandle Unity.Entities.WorldUnmanagedImpl.CreateUnmanagedSystem<Demo3Sys>(Unity.Entities.World,bool)
		// object UnityEngine.Component.GetComponent<object>()
		// System.Void UnityEngine.ComputeBuffer.SetData<Unity.Mathematics.float2>(Unity.Collections.NativeArray<Unity.Mathematics.float2>)
		// System.Void UnityEngine.ComputeBuffer.SetData<int>(Unity.Collections.NativeArray<int>)
		// object UnityEngine.GameObject.GetComponent<object>()
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputAction.ReadValue<UnityEngine.Vector2>()
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputAction.CallbackContext.ReadValue<UnityEngine.Vector2>()
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputActionState.ApplyProcessors<UnityEngine.Vector2>(int,UnityEngine.Vector2,UnityEngine.InputSystem.InputControl<UnityEngine.Vector2>)
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputActionState.ReadValue<UnityEngine.Vector2>(int,int,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
	}
}