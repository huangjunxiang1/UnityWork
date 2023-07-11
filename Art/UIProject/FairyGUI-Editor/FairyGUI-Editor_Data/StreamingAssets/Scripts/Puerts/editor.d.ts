﻿   
declare module 'csharp' {
    import * as CSharp from 'csharp';
    export default CSharp;
}
declare module 'csharp' {
    interface $Ref<T> {
        value: T
    }
    namespace System {
        interface Array$1<T> extends System.Array {
            get_Item(index: number):T;
            set_Item(index: number, value: T):void;
        }
    }
    type $Task<T> = System.Threading.Tasks.Task$1<T>
    namespace System {
        class Int32 extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>, System.IFormattable
        {
        }
        class ValueType extends System.Object
        {
        }
        class Object
        {
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public GetHashCode () : number
            public GetType () : System.Type
            public ToString () : string
            public static ReferenceEquals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        interface IComparable
        {
        }
        interface IComparable$1<T>
        {
        }
        interface IConvertible
        {
        }
        interface IEquatable$1<T>
        {
        }
        interface IFormattable
        {
        }
        class Void extends System.ValueType
        {
        }
        class Boolean extends System.ValueType implements System.IComparable, System.IComparable$1<boolean>, System.IConvertible, System.IEquatable$1<boolean>
        {
        }
        interface Converter$2<TInput, TOutput>
        { (input: TInput) : TOutput; }
        interface MulticastDelegate
        { (...args:any[]) : any; }
        var MulticastDelegate: { new (func: (...args:any[]) => any): MulticastDelegate; }
        class Delegate extends System.Object implements System.ICloneable, System.Runtime.Serialization.ISerializable
        {
            public get Method(): System.Reflection.MethodInfo;
            public get Target(): any;
            public static CreateDelegate ($type: System.Type, $firstArgument: any, $method: System.Reflection.MethodInfo, $throwOnBindFailure: boolean) : Function
            public static CreateDelegate ($type: System.Type, $firstArgument: any, $method: System.Reflection.MethodInfo) : Function
            public static CreateDelegate ($type: System.Type, $method: System.Reflection.MethodInfo, $throwOnBindFailure: boolean) : Function
            public static CreateDelegate ($type: System.Type, $method: System.Reflection.MethodInfo) : Function
            public static CreateDelegate ($type: System.Type, $target: any, $method: string) : Function
            public static CreateDelegate ($type: System.Type, $target: System.Type, $method: string, $ignoreCase: boolean, $throwOnBindFailure: boolean) : Function
            public static CreateDelegate ($type: System.Type, $target: System.Type, $method: string) : Function
            public static CreateDelegate ($type: System.Type, $target: System.Type, $method: string, $ignoreCase: boolean) : Function
            public static CreateDelegate ($type: System.Type, $target: any, $method: string, $ignoreCase: boolean, $throwOnBindFailure: boolean) : Function
            public static CreateDelegate ($type: System.Type, $target: any, $method: string, $ignoreCase: boolean) : Function
            public DynamicInvoke (...args: any[]) : any
            public Clone () : any
            public GetObjectData ($info: System.Runtime.Serialization.SerializationInfo, $context: System.Runtime.Serialization.StreamingContext) : void
            public GetInvocationList () : System.Array$1<Function>
            public static Combine ($a: Function, $b: Function) : Function
            public static Combine (...delegates: Function[]) : Function
            public static Remove ($source: Function, $value: Function) : Function
            public static RemoveAll ($source: Function, $value: Function) : Function
            public static op_Equality ($d1: Function, $d2: Function) : boolean
            public static op_Inequality ($d1: Function, $d2: Function) : boolean
        }
        interface ICloneable
        {
        }
        interface Predicate$1<T>
        { (obj: T) : boolean; }
        interface Action$1<T>
        { (obj: T) : void; }
        interface IDisposable
        {
        }
        interface Comparison$1<T>
        { (x: T, y: T) : number; }
        class Type extends System.Reflection.MemberInfo implements System.Reflection.IReflect, System.Runtime.InteropServices._Type, System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._MemberInfo
        {
            public static FilterAttribute : System.Reflection.MemberFilter
            public static FilterName : System.Reflection.MemberFilter
            public static FilterNameIgnoreCase : System.Reflection.MemberFilter
            public static Missing : any
            public static Delimiter : number
            public static EmptyTypes : System.Array$1<System.Type>
            public get MemberType(): System.Reflection.MemberTypes;
            public get DeclaringType(): System.Type;
            public get DeclaringMethod(): System.Reflection.MethodBase;
            public get ReflectedType(): System.Type;
            public get StructLayoutAttribute(): System.Runtime.InteropServices.StructLayoutAttribute;
            public get GUID(): System.Guid;
            public static get DefaultBinder(): System.Reflection.Binder;
            public get Module(): System.Reflection.Module;
            public get Assembly(): System.Reflection.Assembly;
            public get TypeHandle(): System.RuntimeTypeHandle;
            public get FullName(): string;
            public get Namespace(): string;
            public get AssemblyQualifiedName(): string;
            public get BaseType(): System.Type;
            public get TypeInitializer(): System.Reflection.ConstructorInfo;
            public get IsNested(): boolean;
            public get Attributes(): System.Reflection.TypeAttributes;
            public get GenericParameterAttributes(): System.Reflection.GenericParameterAttributes;
            public get IsVisible(): boolean;
            public get IsNotPublic(): boolean;
            public get IsPublic(): boolean;
            public get IsNestedPublic(): boolean;
            public get IsNestedPrivate(): boolean;
            public get IsNestedFamily(): boolean;
            public get IsNestedAssembly(): boolean;
            public get IsNestedFamANDAssem(): boolean;
            public get IsNestedFamORAssem(): boolean;
            public get IsAutoLayout(): boolean;
            public get IsLayoutSequential(): boolean;
            public get IsExplicitLayout(): boolean;
            public get IsClass(): boolean;
            public get IsInterface(): boolean;
            public get IsValueType(): boolean;
            public get IsAbstract(): boolean;
            public get IsSealed(): boolean;
            public get IsEnum(): boolean;
            public get IsSpecialName(): boolean;
            public get IsImport(): boolean;
            public get IsSerializable(): boolean;
            public get IsAnsiClass(): boolean;
            public get IsUnicodeClass(): boolean;
            public get IsAutoClass(): boolean;
            public get IsArray(): boolean;
            public get IsGenericType(): boolean;
            public get IsGenericTypeDefinition(): boolean;
            public get IsConstructedGenericType(): boolean;
            public get IsGenericParameter(): boolean;
            public get GenericParameterPosition(): number;
            public get ContainsGenericParameters(): boolean;
            public get IsByRef(): boolean;
            public get IsPointer(): boolean;
            public get IsPrimitive(): boolean;
            public get IsCOMObject(): boolean;
            public get HasElementType(): boolean;
            public get IsContextful(): boolean;
            public get IsMarshalByRef(): boolean;
            public get GenericTypeArguments(): System.Array$1<System.Type>;
            public get IsSecurityCritical(): boolean;
            public get IsSecuritySafeCritical(): boolean;
            public get IsSecurityTransparent(): boolean;
            public get UnderlyingSystemType(): System.Type;
            public get IsSZArray(): boolean;
            public static GetType ($typeName: string, $assemblyResolver: System.Func$2<System.Reflection.AssemblyName, System.Reflection.Assembly>, $typeResolver: System.Func$4<System.Reflection.Assembly, string, boolean, System.Type>) : System.Type
            public static GetType ($typeName: string, $assemblyResolver: System.Func$2<System.Reflection.AssemblyName, System.Reflection.Assembly>, $typeResolver: System.Func$4<System.Reflection.Assembly, string, boolean, System.Type>, $throwOnError: boolean) : System.Type
            public static GetType ($typeName: string, $assemblyResolver: System.Func$2<System.Reflection.AssemblyName, System.Reflection.Assembly>, $typeResolver: System.Func$4<System.Reflection.Assembly, string, boolean, System.Type>, $throwOnError: boolean, $ignoreCase: boolean) : System.Type
            public MakePointerType () : System.Type
            public MakeByRefType () : System.Type
            public MakeArrayType () : System.Type
            public MakeArrayType ($rank: number) : System.Type
            public static GetTypeFromProgID ($progID: string) : System.Type
            public static GetTypeFromProgID ($progID: string, $throwOnError: boolean) : System.Type
            public static GetTypeFromProgID ($progID: string, $server: string) : System.Type
            public static GetTypeFromProgID ($progID: string, $server: string, $throwOnError: boolean) : System.Type
            public static GetTypeFromCLSID ($clsid: System.Guid) : System.Type
            public static GetTypeFromCLSID ($clsid: System.Guid, $throwOnError: boolean) : System.Type
            public static GetTypeFromCLSID ($clsid: System.Guid, $server: string) : System.Type
            public static GetTypeFromCLSID ($clsid: System.Guid, $server: string, $throwOnError: boolean) : System.Type
            public static GetTypeCode ($type: System.Type) : System.TypeCode
            public InvokeMember ($name: string, $invokeAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $target: any, $args: System.Array$1<any>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>, $culture: System.Globalization.CultureInfo, $namedParameters: System.Array$1<string>) : any
            public InvokeMember ($name: string, $invokeAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $target: any, $args: System.Array$1<any>, $culture: System.Globalization.CultureInfo) : any
            public InvokeMember ($name: string, $invokeAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $target: any, $args: System.Array$1<any>) : any
            public static GetTypeHandle ($o: any) : System.RuntimeTypeHandle
            public GetArrayRank () : number
            public GetConstructor ($bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $callConvention: System.Reflection.CallingConventions, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>) : System.Reflection.ConstructorInfo
            public GetConstructor ($bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>) : System.Reflection.ConstructorInfo
            public GetConstructor ($types: System.Array$1<System.Type>) : System.Reflection.ConstructorInfo
            public GetConstructors () : System.Array$1<System.Reflection.ConstructorInfo>
            public GetConstructors ($bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.ConstructorInfo>
            public GetMethod ($name: string, $bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $callConvention: System.Reflection.CallingConventions, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>) : System.Reflection.MethodInfo
            public GetMethod ($name: string, $bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>) : System.Reflection.MethodInfo
            public GetMethod ($name: string, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>) : System.Reflection.MethodInfo
            public GetMethod ($name: string, $types: System.Array$1<System.Type>) : System.Reflection.MethodInfo
            public GetMethod ($name: string, $bindingAttr: System.Reflection.BindingFlags) : System.Reflection.MethodInfo
            public GetMethod ($name: string) : System.Reflection.MethodInfo
            public GetMethods () : System.Array$1<System.Reflection.MethodInfo>
            public GetMethods ($bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.MethodInfo>
            public GetField ($name: string, $bindingAttr: System.Reflection.BindingFlags) : System.Reflection.FieldInfo
            public GetField ($name: string) : System.Reflection.FieldInfo
            public GetFields () : System.Array$1<System.Reflection.FieldInfo>
            public GetFields ($bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.FieldInfo>
            public GetInterface ($name: string) : System.Type
            public GetInterface ($name: string, $ignoreCase: boolean) : System.Type
            public GetInterfaces () : System.Array$1<System.Type>
            public FindInterfaces ($filter: System.Reflection.TypeFilter, $filterCriteria: any) : System.Array$1<System.Type>
            public GetEvent ($name: string) : System.Reflection.EventInfo
            public GetEvent ($name: string, $bindingAttr: System.Reflection.BindingFlags) : System.Reflection.EventInfo
            public GetEvents () : System.Array$1<System.Reflection.EventInfo>
            public GetEvents ($bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.EventInfo>
            public GetProperty ($name: string, $bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $returnType: System.Type, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>) : System.Reflection.PropertyInfo
            public GetProperty ($name: string, $returnType: System.Type, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>) : System.Reflection.PropertyInfo
            public GetProperty ($name: string, $bindingAttr: System.Reflection.BindingFlags) : System.Reflection.PropertyInfo
            public GetProperty ($name: string, $returnType: System.Type, $types: System.Array$1<System.Type>) : System.Reflection.PropertyInfo
            public GetProperty ($name: string, $types: System.Array$1<System.Type>) : System.Reflection.PropertyInfo
            public GetProperty ($name: string, $returnType: System.Type) : System.Reflection.PropertyInfo
            public GetProperty ($name: string) : System.Reflection.PropertyInfo
            public GetProperties ($bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.PropertyInfo>
            public GetProperties () : System.Array$1<System.Reflection.PropertyInfo>
            public GetNestedTypes () : System.Array$1<System.Type>
            public GetNestedTypes ($bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Type>
            public GetNestedType ($name: string) : System.Type
            public GetNestedType ($name: string, $bindingAttr: System.Reflection.BindingFlags) : System.Type
            public GetMember ($name: string) : System.Array$1<System.Reflection.MemberInfo>
            public GetMember ($name: string, $bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.MemberInfo>
            public GetMember ($name: string, $type: System.Reflection.MemberTypes, $bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.MemberInfo>
            public GetMembers () : System.Array$1<System.Reflection.MemberInfo>
            public GetMembers ($bindingAttr: System.Reflection.BindingFlags) : System.Array$1<System.Reflection.MemberInfo>
            public GetDefaultMembers () : System.Array$1<System.Reflection.MemberInfo>
            public FindMembers ($memberType: System.Reflection.MemberTypes, $bindingAttr: System.Reflection.BindingFlags, $filter: System.Reflection.MemberFilter, $filterCriteria: any) : System.Array$1<System.Reflection.MemberInfo>
            public GetGenericParameterConstraints () : System.Array$1<System.Type>
            public MakeGenericType (...typeArguments: System.Type[]) : System.Type
            public GetElementType () : System.Type
            public GetGenericArguments () : System.Array$1<System.Type>
            public GetGenericTypeDefinition () : System.Type
            public GetEnumNames () : System.Array$1<string>
            public GetEnumValues () : System.Array
            public GetEnumUnderlyingType () : System.Type
            public IsEnumDefined ($value: any) : boolean
            public GetEnumName ($value: any) : string
            public IsSubclassOf ($c: System.Type) : boolean
            public IsInstanceOfType ($o: any) : boolean
            public IsAssignableFrom ($c: System.Type) : boolean
            public IsEquivalentTo ($other: System.Type) : boolean
            public static GetTypeArray ($args: System.Array$1<any>) : System.Array$1<System.Type>
            public Equals ($o: any) : boolean
            public Equals ($o: System.Type) : boolean
            public static op_Equality ($left: System.Type, $right: System.Type) : boolean
            public static op_Inequality ($left: System.Type, $right: System.Type) : boolean
            public GetInterfaceMap ($interfaceType: System.Type) : System.Reflection.InterfaceMapping
            public GetType () : System.Type
            public static GetType ($typeName: string) : System.Type
            public static GetType ($typeName: string, $throwOnError: boolean) : System.Type
            public static GetType ($typeName: string, $throwOnError: boolean, $ignoreCase: boolean) : System.Type
            public static ReflectionOnlyGetType ($typeName: string, $throwIfNotFound: boolean, $ignoreCase: boolean) : System.Type
            public static GetTypeFromHandle ($handle: System.RuntimeTypeHandle) : System.Type
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
        }
        class String extends System.Object implements System.ICloneable, System.Collections.IEnumerable, System.IComparable, System.IComparable$1<string>, System.IConvertible, System.IEquatable$1<string>, System.Collections.Generic.IEnumerable$1<number>
        {
        }
        class Char extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>
        {
        }
        class Array extends System.Object implements System.ICloneable, System.Collections.IEnumerable, System.Collections.IList, System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.Collections.ICollection
        {
            public get LongLength(): bigint;
            public get IsFixedSize(): boolean;
            public get IsReadOnly(): boolean;
            public get IsSynchronized(): boolean;
            public get SyncRoot(): any;
            public get Length(): number;
            public get Rank(): number;
            public static CreateInstance ($elementType: System.Type, ...lengths: bigint[]) : System.Array
            public CopyTo ($array: System.Array, $index: number) : void
            public Clone () : any
            public static BinarySearch ($array: System.Array, $value: any) : number
            public static Copy ($sourceArray: System.Array, $destinationArray: System.Array, $length: bigint) : void
            public static Copy ($sourceArray: System.Array, $sourceIndex: bigint, $destinationArray: System.Array, $destinationIndex: bigint, $length: bigint) : void
            public CopyTo ($array: System.Array, $index: bigint) : void
            public GetLongLength ($dimension: number) : bigint
            public GetValue ($index: bigint) : any
            public GetValue ($index1: bigint, $index2: bigint) : any
            public GetValue ($index1: bigint, $index2: bigint, $index3: bigint) : any
            public GetValue (...indices: bigint[]) : any
            public static BinarySearch ($array: System.Array, $index: number, $length: number, $value: any) : number
            public static BinarySearch ($array: System.Array, $value: any, $comparer: System.Collections.IComparer) : number
            public static BinarySearch ($array: System.Array, $index: number, $length: number, $value: any, $comparer: System.Collections.IComparer) : number
            public static IndexOf ($array: System.Array, $value: any) : number
            public static IndexOf ($array: System.Array, $value: any, $startIndex: number) : number
            public static IndexOf ($array: System.Array, $value: any, $startIndex: number, $count: number) : number
            public static LastIndexOf ($array: System.Array, $value: any) : number
            public static LastIndexOf ($array: System.Array, $value: any, $startIndex: number) : number
            public static LastIndexOf ($array: System.Array, $value: any, $startIndex: number, $count: number) : number
            public static Reverse ($array: System.Array) : void
            public static Reverse ($array: System.Array, $index: number, $length: number) : void
            public SetValue ($value: any, $index: bigint) : void
            public SetValue ($value: any, $index1: bigint, $index2: bigint) : void
            public SetValue ($value: any, $index1: bigint, $index2: bigint, $index3: bigint) : void
            public SetValue ($value: any, ...indices: bigint[]) : void
            public static Sort ($array: System.Array) : void
            public static Sort ($array: System.Array, $index: number, $length: number) : void
            public static Sort ($array: System.Array, $comparer: System.Collections.IComparer) : void
            public static Sort ($array: System.Array, $index: number, $length: number, $comparer: System.Collections.IComparer) : void
            public static Sort ($keys: System.Array, $items: System.Array) : void
            public static Sort ($keys: System.Array, $items: System.Array, $comparer: System.Collections.IComparer) : void
            public static Sort ($keys: System.Array, $items: System.Array, $index: number, $length: number) : void
            public static Sort ($keys: System.Array, $items: System.Array, $index: number, $length: number, $comparer: System.Collections.IComparer) : void
            public GetEnumerator () : System.Collections.IEnumerator
            public GetLength ($dimension: number) : number
            public GetLowerBound ($dimension: number) : number
            public GetValue (...indices: number[]) : any
            public SetValue ($value: any, ...indices: number[]) : void
            public GetUpperBound ($dimension: number) : number
            public GetValue ($index: number) : any
            public GetValue ($index1: number, $index2: number) : any
            public GetValue ($index1: number, $index2: number, $index3: number) : any
            public SetValue ($value: any, $index: number) : void
            public SetValue ($value: any, $index1: number, $index2: number) : void
            public SetValue ($value: any, $index1: number, $index2: number, $index3: number) : void
            public static CreateInstance ($elementType: System.Type, $length: number) : System.Array
            public static CreateInstance ($elementType: System.Type, $length1: number, $length2: number) : System.Array
            public static CreateInstance ($elementType: System.Type, $length1: number, $length2: number, $length3: number) : System.Array
            public static CreateInstance ($elementType: System.Type, ...lengths: number[]) : System.Array
            public static CreateInstance ($elementType: System.Type, $lengths: System.Array$1<number>, $lowerBounds: System.Array$1<number>) : System.Array
            public static Clear ($array: System.Array, $index: number, $length: number) : void
            public static Copy ($sourceArray: System.Array, $destinationArray: System.Array, $length: number) : void
            public static Copy ($sourceArray: System.Array, $sourceIndex: number, $destinationArray: System.Array, $destinationIndex: number, $length: number) : void
            public static ConstrainedCopy ($sourceArray: System.Array, $sourceIndex: number, $destinationArray: System.Array, $destinationIndex: number, $length: number) : void
            public Initialize () : void
        }
        class Int64 extends System.ValueType implements System.IComparable, System.IComparable$1<bigint>, System.IConvertible, System.IEquatable$1<bigint>, System.IFormattable
        {
        }
        class Enum extends System.ValueType implements System.IComparable, System.IConvertible, System.IFormattable
        {
        }
        interface Func$2<T, TResult>
        { (arg: T) : TResult; }
        interface Func$4<T1, T2, T3, TResult>
        { (arg1: T1, arg2: T2, arg3: T3) : TResult; }
        class Attribute extends System.Object implements System.Runtime.InteropServices._Attribute
        {
        }
        class Guid extends System.ValueType implements System.IComparable, System.IComparable$1<System.Guid>, System.IEquatable$1<System.Guid>, System.IFormattable
        {
        }
        enum TypeCode
        { Empty = 0, Object = 1, DBNull = 2, Boolean = 3, Char = 4, SByte = 5, Byte = 6, Int16 = 7, UInt16 = 8, Int32 = 9, UInt32 = 10, Int64 = 11, UInt64 = 12, Single = 13, Double = 14, Decimal = 15, DateTime = 16, String = 18 }
        interface IFormatProvider
        {
        }
        class RuntimeTypeHandle extends System.ValueType implements System.Runtime.Serialization.ISerializable
        {
        }
        class MarshalByRefObject extends System.Object
        {
        }
        class DateTime extends System.ValueType implements System.IComparable, System.IComparable$1<Date>, System.IConvertible, System.IEquatable$1<Date>, System.Runtime.Serialization.ISerializable, System.IFormattable
        {
        }
        class Byte extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>, System.IFormattable
        {
        }
        class Single extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>, System.IFormattable
        {
        }
        interface Single {
            FormattedString ($fractionDigits?: number) : string;
        }
        class UInt32 extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>, System.IFormattable
        {
        }
        class UInt64 extends System.ValueType implements System.IComparable, System.IComparable$1<bigint>, System.IConvertible, System.IEquatable$1<bigint>, System.IFormattable
        {
        }
        class Double extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>, System.IFormattable
        {
        }
        class IntPtr extends System.ValueType implements System.Runtime.Serialization.ISerializable
        {
        }
        interface Func$1<TResult>
        { () : TResult; }
        interface Action
        { () : void; }
        var Action: { new (func: () => void): Action; }
        class Exception extends System.Object implements System.Runtime.InteropServices._Exception, System.Runtime.Serialization.ISerializable
        {
        }
        class UInt16 extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>, System.IFormattable
        {
        }
        interface IAsyncResult
        {
        }
        interface Action$2<T1, T2>
        { (arg1: T1, arg2: T2) : void; }
        interface Action$3<T1, T2, T3>
        { (arg1: T1, arg2: T2, arg3: T3) : void; }
        class Nullable$1<T> extends System.ValueType
        {
        }
        class Int16 extends System.ValueType implements System.IComparable, System.IComparable$1<number>, System.IConvertible, System.IEquatable$1<number>, System.IFormattable
        {
        }
    }
    namespace System.Collections.Generic {
        interface IList$1<T> extends System.Collections.IEnumerable, System.Collections.Generic.ICollection$1<T>, System.Collections.Generic.IEnumerable$1<T>
        {
            get_Item ($index: number) : T
            set_Item ($index: number, $value: T) : void
            IndexOf ($item: T) : number
            Insert ($index: number, $item: T) : void
            RemoveAt ($index: number) : void
        }
        interface ICollection$1<T> extends System.Collections.IEnumerable, System.Collections.Generic.IEnumerable$1<T>
        {
        }
        interface IEnumerable$1<T> extends System.Collections.IEnumerable
        {
        }
        class List$1<T> extends System.Object implements System.Collections.IEnumerable, System.Collections.Generic.IList$1<T>, System.Collections.Generic.IReadOnlyCollection$1<T>, System.Collections.Generic.IReadOnlyList$1<T>, System.Collections.IList, System.Collections.Generic.ICollection$1<T>, System.Collections.ICollection, System.Collections.Generic.IEnumerable$1<T>
        {
            public get Capacity(): number;
            public set Capacity(value: number);
            public get Count(): number;
            public get_Item ($index: number) : T
            public set_Item ($index: number, $value: T) : void
            public Add ($item: T) : void
            public AddRange ($collection: System.Collections.Generic.IEnumerable$1<T>) : void
            public AsReadOnly () : System.Collections.ObjectModel.ReadOnlyCollection$1<T>
            public BinarySearch ($index: number, $count: number, $item: T, $comparer: System.Collections.Generic.IComparer$1<T>) : number
            public BinarySearch ($item: T) : number
            public BinarySearch ($item: T, $comparer: System.Collections.Generic.IComparer$1<T>) : number
            public Clear () : void
            public Contains ($item: T) : boolean
            public CopyTo ($array: System.Array$1<T>) : void
            public CopyTo ($index: number, $array: System.Array$1<T>, $arrayIndex: number, $count: number) : void
            public CopyTo ($array: System.Array$1<T>, $arrayIndex: number) : void
            public Exists ($match: System.Predicate$1<T>) : boolean
            public Find ($match: System.Predicate$1<T>) : T
            public FindAll ($match: System.Predicate$1<T>) : System.Collections.Generic.List$1<T>
            public FindIndex ($match: System.Predicate$1<T>) : number
            public FindIndex ($startIndex: number, $match: System.Predicate$1<T>) : number
            public FindIndex ($startIndex: number, $count: number, $match: System.Predicate$1<T>) : number
            public FindLast ($match: System.Predicate$1<T>) : T
            public FindLastIndex ($match: System.Predicate$1<T>) : number
            public FindLastIndex ($startIndex: number, $match: System.Predicate$1<T>) : number
            public FindLastIndex ($startIndex: number, $count: number, $match: System.Predicate$1<T>) : number
            public ForEach ($action: System.Action$1<T>) : void
            public GetEnumerator () : System.Collections.Generic.List$1.Enumerator<T>
            public GetRange ($index: number, $count: number) : System.Collections.Generic.List$1<T>
            public IndexOf ($item: T) : number
            public IndexOf ($item: T, $index: number) : number
            public IndexOf ($item: T, $index: number, $count: number) : number
            public Insert ($index: number, $item: T) : void
            public InsertRange ($index: number, $collection: System.Collections.Generic.IEnumerable$1<T>) : void
            public LastIndexOf ($item: T) : number
            public LastIndexOf ($item: T, $index: number) : number
            public LastIndexOf ($item: T, $index: number, $count: number) : number
            public Remove ($item: T) : boolean
            public RemoveAll ($match: System.Predicate$1<T>) : number
            public RemoveAt ($index: number) : void
            public RemoveRange ($index: number, $count: number) : void
            public Reverse () : void
            public Reverse ($index: number, $count: number) : void
            public Sort () : void
            public Sort ($comparer: System.Collections.Generic.IComparer$1<T>) : void
            public Sort ($index: number, $count: number, $comparer: System.Collections.Generic.IComparer$1<T>) : void
            public Sort ($comparison: System.Comparison$1<T>) : void
            public ToArray () : System.Array$1<T>
            public TrimExcess () : void
            public TrueForAll ($match: System.Predicate$1<T>) : boolean
            public constructor ()
            public constructor ($capacity: number)
            public constructor ($collection: System.Collections.Generic.IEnumerable$1<T>)
        }
        interface IReadOnlyCollection$1<T> extends System.Collections.IEnumerable, System.Collections.Generic.IEnumerable$1<T>
        {
        }
        interface IReadOnlyList$1<T> extends System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection$1<T>, System.Collections.Generic.IEnumerable$1<T>
        {
        }
        interface IComparer$1<T>
        {
        }
        interface IEnumerator$1<T> extends System.Collections.IEnumerator, System.IDisposable
        {
        }
        class Dictionary$2<TKey, TValue> extends System.Object implements System.Collections.IDictionary, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>, System.Collections.Generic.IReadOnlyDictionary$2<TKey, TValue>, System.Runtime.Serialization.IDeserializationCallback, System.Collections.Generic.ICollection$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>, System.Runtime.Serialization.ISerializable, System.Collections.ICollection, System.Collections.Generic.IDictionary$2<TKey, TValue>, System.Collections.Generic.IEnumerable$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>
        {
            public get Comparer(): System.Collections.Generic.IEqualityComparer$1<TKey>;
            public get Count(): number;
            public get Keys(): System.Collections.Generic.Dictionary$2.KeyCollection<TKey, TValue>;
            public get Values(): System.Collections.Generic.Dictionary$2.ValueCollection<TKey, TValue>;
            public get_Item ($key: TKey) : TValue
            public set_Item ($key: TKey, $value: TValue) : void
            public Add ($key: TKey, $value: TValue) : void
            public Clear () : void
            public ContainsKey ($key: TKey) : boolean
            public ContainsValue ($value: TValue) : boolean
            public GetEnumerator () : System.Collections.Generic.Dictionary$2.Enumerator<TKey, TValue>
            public GetObjectData ($info: System.Runtime.Serialization.SerializationInfo, $context: System.Runtime.Serialization.StreamingContext) : void
            public OnDeserialization ($sender: any) : void
            public Remove ($key: TKey) : boolean
            public Remove ($key: TKey, $value: $Ref<TValue>) : boolean
            public TryGetValue ($key: TKey, $value: $Ref<TValue>) : boolean
            public TryAdd ($key: TKey, $value: TValue) : boolean
            public constructor ()
            public constructor ($capacity: number)
            public constructor ($comparer: System.Collections.Generic.IEqualityComparer$1<TKey>)
            public constructor ($capacity: number, $comparer: System.Collections.Generic.IEqualityComparer$1<TKey>)
            public constructor ($dictionary: System.Collections.Generic.IDictionary$2<TKey, TValue>)
            public constructor ($dictionary: System.Collections.Generic.IDictionary$2<TKey, TValue>, $comparer: System.Collections.Generic.IEqualityComparer$1<TKey>)
            public constructor ($collection: System.Collections.Generic.IEnumerable$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>)
            public constructor ($collection: System.Collections.Generic.IEnumerable$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>, $comparer: System.Collections.Generic.IEqualityComparer$1<TKey>)
        }
        class KeyValuePair$2<TKey, TValue> extends System.ValueType
        {
        }
        interface IReadOnlyDictionary$2<TKey, TValue> extends System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>, System.Collections.Generic.IEnumerable$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>
        {
        }
        interface IDictionary$2<TKey, TValue> extends System.Collections.IEnumerable, System.Collections.Generic.ICollection$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>, System.Collections.Generic.IEnumerable$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>
        {
            Keys : System.Collections.Generic.ICollection$1<TKey>
            Values : System.Collections.Generic.ICollection$1<TValue>
            get_Item ($key: TKey) : TValue
            set_Item ($key: TKey, $value: TValue) : void
            ContainsKey ($key: TKey) : boolean
            Add ($key: TKey, $value: TValue) : void
            Remove ($key: TKey) : boolean
            TryGetValue ($key: TKey, $value: $Ref<TValue>) : boolean
        }
        interface IEqualityComparer$1<T>
        {
        }
    }
    namespace System.Collections {
        interface IEnumerable
        {
        }
        interface IList extends System.Collections.IEnumerable, System.Collections.ICollection
        {
        }
        interface ICollection extends System.Collections.IEnumerable
        {
        }
        interface IEnumerator
        {
        }
        interface IDictionary extends System.Collections.IEnumerable, System.Collections.ICollection
        {
        }
        interface IDictionaryEnumerator extends System.Collections.IEnumerator
        {
        }
        interface IStructuralComparable
        {
        }
        interface IStructuralEquatable
        {
        }
        interface IComparer
        {
        }
        class Hashtable extends System.Object implements System.Collections.IDictionary, System.ICloneable, System.Collections.IEnumerable, System.Runtime.Serialization.IDeserializationCallback, System.Runtime.Serialization.ISerializable, System.Collections.ICollection
        {
        }
    }
    namespace System.Collections.ObjectModel {
        class ReadOnlyCollection$1<T> extends System.Object implements System.Collections.IEnumerable, System.Collections.Generic.IList$1<T>, System.Collections.Generic.IReadOnlyCollection$1<T>, System.Collections.Generic.IReadOnlyList$1<T>, System.Collections.IList, System.Collections.Generic.ICollection$1<T>, System.Collections.ICollection, System.Collections.Generic.IEnumerable$1<T>
        {
            public get_Item ($index: number) : T
            public set_Item ($index: number, $value: T) : void
            public IndexOf ($item: T) : number
            public Insert ($index: number, $item: T) : void
            public RemoveAt ($index: number) : void
        }
    }
    namespace System.Runtime.Serialization {
        interface ISerializable
        {
        }
        interface IDeserializationCallback
        {
        }
        class SerializationInfo extends System.Object
        {
        }
        class StreamingContext extends System.ValueType
        {
        }
    }
    namespace System.Collections.Generic.List$1 {
        class Enumerator<T> extends System.ValueType implements System.Collections.Generic.IEnumerator$1<T>, System.Collections.IEnumerator, System.IDisposable
        {
        }
    }
    namespace System.Collections.Generic.Dictionary$2 {
        class KeyCollection<TKey, TValue> extends System.Object implements System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection$1<TKey>, System.Collections.Generic.ICollection$1<TKey>, System.Collections.ICollection, System.Collections.Generic.IEnumerable$1<TKey>
        {
        }
        class ValueCollection<TKey, TValue> extends System.Object implements System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection$1<TValue>, System.Collections.Generic.ICollection$1<TValue>, System.Collections.ICollection, System.Collections.Generic.IEnumerable$1<TValue>
        {
        }
        class Enumerator<TKey, TValue> extends System.ValueType implements System.Collections.Generic.IEnumerator$1<System.Collections.Generic.KeyValuePair$2<TKey, TValue>>, System.Collections.IDictionaryEnumerator, System.Collections.IEnumerator, System.IDisposable
        {
        }
    }
    namespace System.Reflection {
        class MethodInfo extends System.Reflection.MethodBase implements System.Runtime.InteropServices._MethodBase, System.Runtime.InteropServices._MethodInfo, System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._MemberInfo
        {
        }
        class MethodBase extends System.Reflection.MemberInfo implements System.Runtime.InteropServices._MethodBase, System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._MemberInfo
        {
        }
        class MemberInfo extends System.Object implements System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._MemberInfo
        {
        }
        interface ICustomAttributeProvider
        {
        }
        interface IReflect
        {
        }
        interface MemberFilter
        { (m: System.Reflection.MemberInfo, filterCriteria: any) : boolean; }
        var MemberFilter: { new (func: (m: System.Reflection.MemberInfo, filterCriteria: any) => boolean): MemberFilter; }
        enum MemberTypes
        { Constructor = 1, Event = 2, Field = 4, Method = 8, Property = 16, TypeInfo = 32, Custom = 64, NestedType = 128, All = 191 }
        class AssemblyName extends System.Object implements System.ICloneable, System.Runtime.Serialization.IDeserializationCallback, System.Runtime.InteropServices._AssemblyName, System.Runtime.Serialization.ISerializable
        {
        }
        class Assembly extends System.Object implements System.Security.IEvidenceFactory, System.Runtime.InteropServices._Assembly, System.Reflection.ICustomAttributeProvider, System.Runtime.Serialization.ISerializable
        {
        }
        class Binder extends System.Object
        {
        }
        enum BindingFlags
        { Default = 0, IgnoreCase = 1, DeclaredOnly = 2, Instance = 4, Static = 8, Public = 16, NonPublic = 32, FlattenHierarchy = 64, InvokeMethod = 256, CreateInstance = 512, GetField = 1024, SetField = 2048, GetProperty = 4096, SetProperty = 8192, PutDispProperty = 16384, PutRefDispProperty = 32768, ExactBinding = 65536, SuppressChangeType = 131072, OptionalParamBinding = 262144, IgnoreReturn = 16777216 }
        class ParameterModifier extends System.ValueType
        {
        }
        class Module extends System.Object implements System.Runtime.InteropServices._Module, System.Reflection.ICustomAttributeProvider, System.Runtime.Serialization.ISerializable
        {
        }
        class ConstructorInfo extends System.Reflection.MethodBase implements System.Runtime.InteropServices._MethodBase, System.Runtime.InteropServices._ConstructorInfo, System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._MemberInfo
        {
        }
        enum CallingConventions
        { Standard = 1, VarArgs = 2, Any = 3, HasThis = 32, ExplicitThis = 64 }
        class FieldInfo extends System.Reflection.MemberInfo implements System.Runtime.InteropServices._FieldInfo, System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._MemberInfo
        {
        }
        interface TypeFilter
        { (m: System.Type, filterCriteria: any) : boolean; }
        var TypeFilter: { new (func: (m: System.Type, filterCriteria: any) => boolean): TypeFilter; }
        class EventInfo extends System.Reflection.MemberInfo implements System.Runtime.InteropServices._EventInfo, System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._MemberInfo
        {
        }
        class PropertyInfo extends System.Reflection.MemberInfo implements System.Reflection.ICustomAttributeProvider, System.Runtime.InteropServices._PropertyInfo, System.Runtime.InteropServices._MemberInfo
        {
        }
        enum TypeAttributes
        { VisibilityMask = 7, NotPublic = 0, Public = 1, NestedPublic = 2, NestedPrivate = 3, NestedFamily = 4, NestedAssembly = 5, NestedFamANDAssem = 6, NestedFamORAssem = 7, LayoutMask = 24, AutoLayout = 0, SequentialLayout = 8, ExplicitLayout = 16, ClassSemanticsMask = 32, Class = 0, Interface = 32, Abstract = 128, Sealed = 256, SpecialName = 1024, Import = 4096, Serializable = 8192, WindowsRuntime = 16384, StringFormatMask = 196608, AnsiClass = 0, UnicodeClass = 65536, AutoClass = 131072, CustomFormatClass = 196608, CustomFormatMask = 12582912, BeforeFieldInit = 1048576, ReservedMask = 264192, RTSpecialName = 2048, HasSecurity = 262144 }
        enum GenericParameterAttributes
        { None = 0, VarianceMask = 3, Covariant = 1, Contravariant = 2, SpecialConstraintMask = 28, ReferenceTypeConstraint = 4, NotNullableValueTypeConstraint = 8, DefaultConstructorConstraint = 16 }
        class InterfaceMapping extends System.ValueType
        {
        }
    }
    namespace System.Runtime.InteropServices {
        interface _MemberInfo
        {
        }
        interface _MethodBase
        {
        }
        interface _MethodInfo
        {
        }
        interface _Type
        {
        }
        interface _AssemblyName
        {
        }
        interface _Assembly
        {
        }
        class StructLayoutAttribute extends System.Attribute implements System.Runtime.InteropServices._Attribute
        {
        }
        interface _Attribute
        {
        }
        interface _Module
        {
        }
        interface _ConstructorInfo
        {
        }
        interface _FieldInfo
        {
        }
        interface _EventInfo
        {
        }
        interface _PropertyInfo
        {
        }
        interface _Exception
        {
        }
    }
    namespace System.Security {
        interface IEvidenceFactory
        {
        }
    }
    namespace System.Globalization {
        class CultureInfo extends System.Object implements System.ICloneable, System.IFormatProvider
        {
        }
    }
    namespace System.IO {
        class File extends System.Object
        {
            public static AppendAllText ($path: string, $contents: string) : void
            public static AppendAllText ($path: string, $contents: string, $encoding: System.Text.Encoding) : void
            public static AppendText ($path: string) : System.IO.StreamWriter
            public static Copy ($sourceFileName: string, $destFileName: string) : void
            public static Copy ($sourceFileName: string, $destFileName: string, $overwrite: boolean) : void
            public static Create ($path: string) : System.IO.FileStream
            public static Create ($path: string, $bufferSize: number) : System.IO.FileStream
            public static Create ($path: string, $bufferSize: number, $options: System.IO.FileOptions) : System.IO.FileStream
            public static Create ($path: string, $bufferSize: number, $options: System.IO.FileOptions, $fileSecurity: System.Security.AccessControl.FileSecurity) : System.IO.FileStream
            public static CreateText ($path: string) : System.IO.StreamWriter
            public static Delete ($path: string) : void
            public static Exists ($path: string) : boolean
            public static GetAccessControl ($path: string) : System.Security.AccessControl.FileSecurity
            public static GetAccessControl ($path: string, $includeSections: System.Security.AccessControl.AccessControlSections) : System.Security.AccessControl.FileSecurity
            public static GetAttributes ($path: string) : System.IO.FileAttributes
            public static GetCreationTime ($path: string) : Date
            public static GetCreationTimeUtc ($path: string) : Date
            public static GetLastAccessTime ($path: string) : Date
            public static GetLastAccessTimeUtc ($path: string) : Date
            public static GetLastWriteTime ($path: string) : Date
            public static GetLastWriteTimeUtc ($path: string) : Date
            public static Move ($sourceFileName: string, $destFileName: string) : void
            public static Open ($path: string, $mode: System.IO.FileMode) : System.IO.FileStream
            public static Open ($path: string, $mode: System.IO.FileMode, $access: System.IO.FileAccess) : System.IO.FileStream
            public static Open ($path: string, $mode: System.IO.FileMode, $access: System.IO.FileAccess, $share: System.IO.FileShare) : System.IO.FileStream
            public static OpenRead ($path: string) : System.IO.FileStream
            public static OpenText ($path: string) : System.IO.StreamReader
            public static OpenWrite ($path: string) : System.IO.FileStream
            public static Replace ($sourceFileName: string, $destinationFileName: string, $destinationBackupFileName: string) : void
            public static Replace ($sourceFileName: string, $destinationFileName: string, $destinationBackupFileName: string, $ignoreMetadataErrors: boolean) : void
            public static SetAccessControl ($path: string, $fileSecurity: System.Security.AccessControl.FileSecurity) : void
            public static SetAttributes ($path: string, $fileAttributes: System.IO.FileAttributes) : void
            public static SetCreationTime ($path: string, $creationTime: Date) : void
            public static SetCreationTimeUtc ($path: string, $creationTimeUtc: Date) : void
            public static SetLastAccessTime ($path: string, $lastAccessTime: Date) : void
            public static SetLastAccessTimeUtc ($path: string, $lastAccessTimeUtc: Date) : void
            public static SetLastWriteTime ($path: string, $lastWriteTime: Date) : void
            public static SetLastWriteTimeUtc ($path: string, $lastWriteTimeUtc: Date) : void
            public static ReadAllBytes ($path: string) : System.Array$1<number>
            public static ReadAllLines ($path: string) : System.Array$1<string>
            public static ReadAllLines ($path: string, $encoding: System.Text.Encoding) : System.Array$1<string>
            public static ReadAllText ($path: string) : string
            public static ReadAllText ($path: string, $encoding: System.Text.Encoding) : string
            public static WriteAllBytes ($path: string, $bytes: System.Array$1<number>) : void
            public static WriteAllLines ($path: string, $contents: System.Array$1<string>) : void
            public static WriteAllLines ($path: string, $contents: System.Array$1<string>, $encoding: System.Text.Encoding) : void
            public static WriteAllText ($path: string, $contents: string) : void
            public static WriteAllText ($path: string, $contents: string, $encoding: System.Text.Encoding) : void
            public static Encrypt ($path: string) : void
            public static Decrypt ($path: string) : void
            public static ReadLines ($path: string) : System.Collections.Generic.IEnumerable$1<string>
            public static ReadLines ($path: string, $encoding: System.Text.Encoding) : System.Collections.Generic.IEnumerable$1<string>
            public static AppendAllLines ($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>) : void
            public static AppendAllLines ($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>, $encoding: System.Text.Encoding) : void
            public static WriteAllLines ($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>) : void
            public static WriteAllLines ($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>, $encoding: System.Text.Encoding) : void
        }
        class StreamWriter extends System.IO.TextWriter implements System.IDisposable
        {
        }
        class TextWriter extends System.MarshalByRefObject implements System.IDisposable
        {
        }
        class FileStream extends System.IO.Stream implements System.IDisposable
        {
        }
        class Stream extends System.MarshalByRefObject implements System.IDisposable
        {
        }
        enum FileOptions
        { None = 0, Encrypted = 16384, DeleteOnClose = 67108864, SequentialScan = 134217728, RandomAccess = 268435456, Asynchronous = 1073741824, WriteThrough = -2147483648 }
        enum FileAttributes
        { Archive = 32, Compressed = 2048, Device = 64, Directory = 16, Encrypted = 16384, Hidden = 2, Normal = 128, NotContentIndexed = 8192, Offline = 4096, ReadOnly = 1, ReparsePoint = 1024, SparseFile = 512, System = 4, Temporary = 256, IntegrityStream = 32768, NoScrubData = 131072 }
        enum FileMode
        { CreateNew = 1, Create = 2, Open = 3, OpenOrCreate = 4, Truncate = 5, Append = 6 }
        enum FileAccess
        { Read = 1, Write = 2, ReadWrite = 3 }
        enum FileShare
        { None = 0, Read = 1, Write = 2, ReadWrite = 3, Delete = 4, Inheritable = 16 }
        class StreamReader extends System.IO.TextReader implements System.IDisposable
        {
        }
        class TextReader extends System.MarshalByRefObject implements System.IDisposable
        {
        }
        class Directory extends System.Object
        {
            public static GetFiles ($path: string) : System.Array$1<string>
            public static GetFiles ($path: string, $searchPattern: string) : System.Array$1<string>
            public static GetFiles ($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption) : System.Array$1<string>
            public static GetDirectories ($path: string) : System.Array$1<string>
            public static GetDirectories ($path: string, $searchPattern: string) : System.Array$1<string>
            public static GetDirectories ($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption) : System.Array$1<string>
            public static GetFileSystemEntries ($path: string) : System.Array$1<string>
            public static GetFileSystemEntries ($path: string, $searchPattern: string) : System.Array$1<string>
            public static GetFileSystemEntries ($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption) : System.Array$1<string>
            public static EnumerateDirectories ($path: string) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateDirectories ($path: string, $searchPattern: string) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateDirectories ($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateFiles ($path: string) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateFiles ($path: string, $searchPattern: string) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateFiles ($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateFileSystemEntries ($path: string) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateFileSystemEntries ($path: string, $searchPattern: string) : System.Collections.Generic.IEnumerable$1<string>
            public static EnumerateFileSystemEntries ($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption) : System.Collections.Generic.IEnumerable$1<string>
            public static GetDirectoryRoot ($path: string) : string
            public static CreateDirectory ($path: string) : System.IO.DirectoryInfo
            public static CreateDirectory ($path: string, $directorySecurity: System.Security.AccessControl.DirectorySecurity) : System.IO.DirectoryInfo
            public static Delete ($path: string) : void
            public static Delete ($path: string, $recursive: boolean) : void
            public static Exists ($path: string) : boolean
            public static GetLastAccessTime ($path: string) : Date
            public static GetLastAccessTimeUtc ($path: string) : Date
            public static GetLastWriteTime ($path: string) : Date
            public static GetLastWriteTimeUtc ($path: string) : Date
            public static GetCreationTime ($path: string) : Date
            public static GetCreationTimeUtc ($path: string) : Date
            public static GetCurrentDirectory () : string
            public static GetLogicalDrives () : System.Array$1<string>
            public static GetParent ($path: string) : System.IO.DirectoryInfo
            public static Move ($sourceDirName: string, $destDirName: string) : void
            public static SetAccessControl ($path: string, $directorySecurity: System.Security.AccessControl.DirectorySecurity) : void
            public static SetCreationTime ($path: string, $creationTime: Date) : void
            public static SetCreationTimeUtc ($path: string, $creationTimeUtc: Date) : void
            public static SetCurrentDirectory ($path: string) : void
            public static SetLastAccessTime ($path: string, $lastAccessTime: Date) : void
            public static SetLastAccessTimeUtc ($path: string, $lastAccessTimeUtc: Date) : void
            public static SetLastWriteTime ($path: string, $lastWriteTime: Date) : void
            public static SetLastWriteTimeUtc ($path: string, $lastWriteTimeUtc: Date) : void
            public static GetAccessControl ($path: string, $includeSections: System.Security.AccessControl.AccessControlSections) : System.Security.AccessControl.DirectorySecurity
            public static GetAccessControl ($path: string) : System.Security.AccessControl.DirectorySecurity
        }
        enum SearchOption
        { TopDirectoryOnly = 0, AllDirectories = 1 }
        class DirectoryInfo extends System.IO.FileSystemInfo implements System.Runtime.Serialization.ISerializable
        {
            public get Exists(): boolean;
            public get Name(): string;
            public get Parent(): System.IO.DirectoryInfo;
            public get Root(): System.IO.DirectoryInfo;
            public Create () : void
            public CreateSubdirectory ($path: string) : System.IO.DirectoryInfo
            public GetFiles () : System.Array$1<System.IO.FileInfo>
            public GetFiles ($searchPattern: string) : System.Array$1<System.IO.FileInfo>
            public GetDirectories () : System.Array$1<System.IO.DirectoryInfo>
            public GetDirectories ($searchPattern: string) : System.Array$1<System.IO.DirectoryInfo>
            public GetFileSystemInfos () : System.Array$1<System.IO.FileSystemInfo>
            public GetFileSystemInfos ($searchPattern: string) : System.Array$1<System.IO.FileSystemInfo>
            public GetFileSystemInfos ($searchPattern: string, $searchOption: System.IO.SearchOption) : System.Array$1<System.IO.FileSystemInfo>
            public Delete () : void
            public Delete ($recursive: boolean) : void
            public MoveTo ($destDirName: string) : void
            public GetDirectories ($searchPattern: string, $searchOption: System.IO.SearchOption) : System.Array$1<System.IO.DirectoryInfo>
            public GetFiles ($searchPattern: string, $searchOption: System.IO.SearchOption) : System.Array$1<System.IO.FileInfo>
            public Create ($directorySecurity: System.Security.AccessControl.DirectorySecurity) : void
            public CreateSubdirectory ($path: string, $directorySecurity: System.Security.AccessControl.DirectorySecurity) : System.IO.DirectoryInfo
            public GetAccessControl () : System.Security.AccessControl.DirectorySecurity
            public GetAccessControl ($includeSections: System.Security.AccessControl.AccessControlSections) : System.Security.AccessControl.DirectorySecurity
            public SetAccessControl ($directorySecurity: System.Security.AccessControl.DirectorySecurity) : void
            public EnumerateDirectories () : System.Collections.Generic.IEnumerable$1<System.IO.DirectoryInfo>
            public EnumerateDirectories ($searchPattern: string) : System.Collections.Generic.IEnumerable$1<System.IO.DirectoryInfo>
            public EnumerateDirectories ($searchPattern: string, $searchOption: System.IO.SearchOption) : System.Collections.Generic.IEnumerable$1<System.IO.DirectoryInfo>
            public EnumerateFiles () : System.Collections.Generic.IEnumerable$1<System.IO.FileInfo>
            public EnumerateFiles ($searchPattern: string) : System.Collections.Generic.IEnumerable$1<System.IO.FileInfo>
            public EnumerateFiles ($searchPattern: string, $searchOption: System.IO.SearchOption) : System.Collections.Generic.IEnumerable$1<System.IO.FileInfo>
            public EnumerateFileSystemInfos () : System.Collections.Generic.IEnumerable$1<System.IO.FileSystemInfo>
            public EnumerateFileSystemInfos ($searchPattern: string) : System.Collections.Generic.IEnumerable$1<System.IO.FileSystemInfo>
            public EnumerateFileSystemInfos ($searchPattern: string, $searchOption: System.IO.SearchOption) : System.Collections.Generic.IEnumerable$1<System.IO.FileSystemInfo>
            public constructor ($path: string)
            public constructor ()
        }
        class FileSystemInfo extends System.MarshalByRefObject implements System.Runtime.Serialization.ISerializable
        {
        }
        class FileInfo extends System.IO.FileSystemInfo implements System.Runtime.Serialization.ISerializable
        {
            public get Name(): string;
            public get Length(): bigint;
            public get DirectoryName(): string;
            public get Directory(): System.IO.DirectoryInfo;
            public get IsReadOnly(): boolean;
            public set IsReadOnly(value: boolean);
            public get Exists(): boolean;
            public GetAccessControl () : System.Security.AccessControl.FileSecurity
            public GetAccessControl ($includeSections: System.Security.AccessControl.AccessControlSections) : System.Security.AccessControl.FileSecurity
            public SetAccessControl ($fileSecurity: System.Security.AccessControl.FileSecurity) : void
            public OpenText () : System.IO.StreamReader
            public CreateText () : System.IO.StreamWriter
            public AppendText () : System.IO.StreamWriter
            public CopyTo ($destFileName: string) : System.IO.FileInfo
            public CopyTo ($destFileName: string, $overwrite: boolean) : System.IO.FileInfo
            public Create () : System.IO.FileStream
            public Decrypt () : void
            public Encrypt () : void
            public Open ($mode: System.IO.FileMode) : System.IO.FileStream
            public Open ($mode: System.IO.FileMode, $access: System.IO.FileAccess) : System.IO.FileStream
            public Open ($mode: System.IO.FileMode, $access: System.IO.FileAccess, $share: System.IO.FileShare) : System.IO.FileStream
            public OpenRead () : System.IO.FileStream
            public OpenWrite () : System.IO.FileStream
            public MoveTo ($destFileName: string) : void
            public Replace ($destinationFileName: string, $destinationBackupFileName: string) : System.IO.FileInfo
            public Replace ($destinationFileName: string, $destinationBackupFileName: string, $ignoreMetadataErrors: boolean) : System.IO.FileInfo
            public constructor ($fileName: string)
            public constructor ()
        }
        class Path extends System.Object
        {
            public static AltDirectorySeparatorChar : number
            public static DirectorySeparatorChar : number
            public static PathSeparator : number
            public static VolumeSeparatorChar : number
            public static ChangeExtension ($path: string, $extension: string) : string
            public static Combine ($path1: string, $path2: string) : string
            public static GetDirectoryName ($path: string) : string
            public static GetExtension ($path: string) : string
            public static GetFileName ($path: string) : string
            public static GetFileNameWithoutExtension ($path: string) : string
            public static GetFullPath ($path: string) : string
            public static GetPathRoot ($path: string) : string
            public static GetTempFileName () : string
            public static GetTempPath () : string
            public static HasExtension ($path: string) : boolean
            public static IsPathRooted ($path: string) : boolean
            public static GetInvalidFileNameChars () : System.Array$1<number>
            public static GetInvalidPathChars () : System.Array$1<number>
            public static GetRandomFileName () : string
            public static Combine (...paths: string[]) : string
            public static Combine ($path1: string, $path2: string, $path3: string) : string
            public static Combine ($path1: string, $path2: string, $path3: string, $path4: string) : string
        }
    }
    namespace System.Text {
        class Encoding extends System.Object implements System.ICloneable
        {
        }
        class StringBuilder extends System.Object implements System.Runtime.Serialization.ISerializable
        {
        }
    }
    namespace System.Security.AccessControl {
        class FileSecurity extends System.Security.AccessControl.FileSystemSecurity
        {
        }
        class FileSystemSecurity extends System.Security.AccessControl.NativeObjectSecurity
        {
        }
        class NativeObjectSecurity extends System.Security.AccessControl.CommonObjectSecurity
        {
        }
        class CommonObjectSecurity extends System.Security.AccessControl.ObjectSecurity
        {
        }
        class ObjectSecurity extends System.Object
        {
        }
        enum AccessControlSections
        { None = 0, Audit = 1, Access = 2, Owner = 4, Group = 8, All = 15 }
        class DirectorySecurity extends System.Security.AccessControl.FileSystemSecurity
        {
        }
    }
    namespace UnityEngine {
        /** Base class for all objects Unity can reference. */
        class Object extends System.Object
        {
        /** The name of the object. */
            public get name(): string;
            public set name(value: string);
            /** Should the object be hidden, saved with the Scene or modifiable by the user? */
            public get hideFlags(): UnityEngine.HideFlags;
            public set hideFlags(value: UnityEngine.HideFlags);
            public GetInstanceID () : number
            public static op_Implicit ($exists: UnityEngine.Object) : boolean
            /** Clones the object original and returns the clone.
            * @param original An existing object that you want to make a copy of.
            * @param position Position for the new object.
            * @param rotation Orientation of the new object.
            * @param parent Parent that will be assigned to the new object.
            * @param instantiateInWorldSpace Pass true when assigning a parent Object to maintain the world position of the Object, instead of setting its position relative to the new parent. Pass false to set the Object's position relative to its new parent.
            * @returns The instantiated clone. 
            */
            public static Instantiate ($original: UnityEngine.Object, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion) : UnityEngine.Object
            /** Clones the object original and returns the clone.
            * @param original An existing object that you want to make a copy of.
            * @param position Position for the new object.
            * @param rotation Orientation of the new object.
            * @param parent Parent that will be assigned to the new object.
            * @param instantiateInWorldSpace Pass true when assigning a parent Object to maintain the world position of the Object, instead of setting its position relative to the new parent. Pass false to set the Object's position relative to its new parent.
            * @returns The instantiated clone. 
            */
            public static Instantiate ($original: UnityEngine.Object, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $parent: UnityEngine.Transform) : UnityEngine.Object
            /** Clones the object original and returns the clone.
            * @param original An existing object that you want to make a copy of.
            * @param position Position for the new object.
            * @param rotation Orientation of the new object.
            * @param parent Parent that will be assigned to the new object.
            * @param instantiateInWorldSpace Pass true when assigning a parent Object to maintain the world position of the Object, instead of setting its position relative to the new parent. Pass false to set the Object's position relative to its new parent.
            * @returns The instantiated clone. 
            */
            public static Instantiate ($original: UnityEngine.Object) : UnityEngine.Object
            /** Clones the object original and returns the clone.
            * @param original An existing object that you want to make a copy of.
            * @param position Position for the new object.
            * @param rotation Orientation of the new object.
            * @param parent Parent that will be assigned to the new object.
            * @param instantiateInWorldSpace Pass true when assigning a parent Object to maintain the world position of the Object, instead of setting its position relative to the new parent. Pass false to set the Object's position relative to its new parent.
            * @returns The instantiated clone. 
            */
            public static Instantiate ($original: UnityEngine.Object, $parent: UnityEngine.Transform) : UnityEngine.Object
            /** Clones the object original and returns the clone.
            * @param original An existing object that you want to make a copy of.
            * @param position Position for the new object.
            * @param rotation Orientation of the new object.
            * @param parent Parent that will be assigned to the new object.
            * @param instantiateInWorldSpace Pass true when assigning a parent Object to maintain the world position of the Object, instead of setting its position relative to the new parent. Pass false to set the Object's position relative to its new parent.
            * @returns The instantiated clone. 
            */
            public static Instantiate ($original: UnityEngine.Object, $parent: UnityEngine.Transform, $instantiateInWorldSpace: boolean) : UnityEngine.Object
            public static Instantiate ($original: UnityEngine.Object, $parent: UnityEngine.Transform, $worldPositionStays: boolean) : UnityEngine.Object
            /** Removes a gameobject, component or asset. * @param obj The object to destroy.
            * @param t The optional amount of time to delay before destroying the object.
            */
            public static Destroy ($obj: UnityEngine.Object, $t: number) : void
            /** Removes a gameobject, component or asset. * @param obj The object to destroy.
            * @param t The optional amount of time to delay before destroying the object.
            */
            public static Destroy ($obj: UnityEngine.Object) : void
            /** Destroys the object obj immediately. You are strongly recommended to use Destroy instead. * @param obj Object to be destroyed.
            * @param allowDestroyingAssets Set to true to allow assets to be destroyed.
            */
            public static DestroyImmediate ($obj: UnityEngine.Object, $allowDestroyingAssets: boolean) : void
            /** Destroys the object obj immediately. You are strongly recommended to use Destroy instead. * @param obj Object to be destroyed.
            * @param allowDestroyingAssets Set to true to allow assets to be destroyed.
            */
            public static DestroyImmediate ($obj: UnityEngine.Object) : void
            /** Returns a list of all active loaded objects of Type type.
            * @param type The type of object to find.
            * @returns The array of objects found matching the type specified. 
            */
            public static FindObjectsOfType ($type: System.Type) : System.Array$1<UnityEngine.Object>
            /** Do not destroy the target Object when loading a new Scene. * @param target An Object not destroyed on Scene change.
            */
            public static DontDestroyOnLoad ($target: UnityEngine.Object) : void
            /** Returns the first active loaded object of Type type.
            * @param type The type of object to find.
            * @returns This returns the  Object that matches the specified type. It returns null if no Object matches the type. 
            */
            public static FindObjectOfType ($type: System.Type) : UnityEngine.Object
            public static op_Equality ($x: UnityEngine.Object, $y: UnityEngine.Object) : boolean
            public static op_Inequality ($x: UnityEngine.Object, $y: UnityEngine.Object) : boolean
            public constructor ()
        }
        /** Representation of 3D vectors and points. */
        class Vector3 extends System.ValueType implements System.IEquatable$1<UnityEngine.Vector3>
        {
            public static kEpsilon : number
            public static kEpsilonNormalSqrt : number/** X component of the vector. */
            public x : number/** Y component of the vector. */
            public y : number/** Z component of the vector. */
            public z : number/** Returns this vector with a magnitude of 1 (Read Only). */
            public get normalized(): UnityEngine.Vector3;
            /** Returns the length of this vector (Read Only). */
            public get magnitude(): number;
            /** Returns the squared length of this vector (Read Only). */
            public get sqrMagnitude(): number;
            /** Shorthand for writing Vector3(0, 0, 0). */
            public static get zero(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(1, 1, 1). */
            public static get one(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(0, 0, 1). */
            public static get forward(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(0, 0, -1). */
            public static get back(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(0, 1, 0). */
            public static get up(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(0, -1, 0). */
            public static get down(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(-1, 0, 0). */
            public static get left(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(1, 0, 0). */
            public static get right(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity). */
            public static get positiveInfinity(): UnityEngine.Vector3;
            /** Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity). */
            public static get negativeInfinity(): UnityEngine.Vector3;
            /** Spherically interpolates between two vectors. */
            public static Slerp ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number) : UnityEngine.Vector3
            /** Spherically interpolates between two vectors. */
            public static SlerpUnclamped ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number) : UnityEngine.Vector3
            /** Makes vectors normalized and orthogonal to each other. */
            public static OrthoNormalize ($normal: $Ref<UnityEngine.Vector3>, $tangent: $Ref<UnityEngine.Vector3>) : void
            /** Makes vectors normalized and orthogonal to each other. */
            public static OrthoNormalize ($normal: $Ref<UnityEngine.Vector3>, $tangent: $Ref<UnityEngine.Vector3>, $binormal: $Ref<UnityEngine.Vector3>) : void
            /** Rotates a vector current towards target.
            * @param current The vector being managed.
            * @param target The vector.
            * @param maxRadiansDelta The distance between the two vectors  in radians.
            * @param maxMagnitudeDelta The length of the radian.
            * @returns The location that RotateTowards generates. 
            */
            public static RotateTowards ($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $maxRadiansDelta: number, $maxMagnitudeDelta: number) : UnityEngine.Vector3
            /** Linearly interpolates between two vectors. */
            public static Lerp ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number) : UnityEngine.Vector3
            /** Linearly interpolates between two vectors. */
            public static LerpUnclamped ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number) : UnityEngine.Vector3
            /** Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.
            * @param current The position to move from.
            * @param target The position to move towards.
            * @param maxDistanceDelta Distance to move current per call.
            * @returns The new position. 
            */
            public static MoveTowards ($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $maxDistanceDelta: number) : UnityEngine.Vector3
            /** Gradually changes a vector towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $currentVelocity: $Ref<UnityEngine.Vector3>, $smoothTime: number, $maxSpeed: number) : UnityEngine.Vector3
            /** Gradually changes a vector towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $currentVelocity: $Ref<UnityEngine.Vector3>, $smoothTime: number) : UnityEngine.Vector3
            /** Gradually changes a vector towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $currentVelocity: $Ref<UnityEngine.Vector3>, $smoothTime: number, $maxSpeed: number, $deltaTime: number) : UnityEngine.Vector3
            public get_Item ($index: number) : number
            public set_Item ($index: number, $value: number) : void
            /** Set x, y and z components of an existing Vector3. */
            public Set ($newX: number, $newY: number, $newZ: number) : void
            /** Multiplies two vectors component-wise. */
            public static Scale ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Multiplies every component of this vector by the same component of scale. */
            public Scale ($scale: UnityEngine.Vector3) : void
            /** Cross Product of two vectors. */
            public static Cross ($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Returns true if the given vector is exactly equal to this vector. */
            public Equals ($other: any) : boolean
            public Equals ($other: UnityEngine.Vector3) : boolean
            /** Reflects a vector off the plane defined by a normal. */
            public static Reflect ($inDirection: UnityEngine.Vector3, $inNormal: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Makes this vector have a magnitude of 1. */
            public static Normalize ($value: UnityEngine.Vector3) : UnityEngine.Vector3
            public Normalize () : void
            /** Dot Product of two vectors. */
            public static Dot ($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3) : number
            /** Projects a vector onto another vector. */
            public static Project ($vector: UnityEngine.Vector3, $onNormal: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Projects a vector onto a plane defined by a normal orthogonal to the plane.
            * @param planeNormal The direction from the vector towards the plane.
            * @param vector The location of the vector above the plane.
            * @returns The location of the vector on the plane. 
            */
            public static ProjectOnPlane ($vector: UnityEngine.Vector3, $planeNormal: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Returns the angle in degrees between from and to.
            * @param from The vector from which the angular difference is measured.
            * @param to The vector to which the angular difference is measured.
            * @returns The angle in degrees between the two vectors. 
            */
            public static Angle ($from: UnityEngine.Vector3, $to: UnityEngine.Vector3) : number
            /** Returns the signed angle in degrees between from and to. * @param from The vector from which the angular difference is measured.
            * @param to The vector to which the angular difference is measured.
            * @param axis A vector around which the other vectors are rotated.
            */
            public static SignedAngle ($from: UnityEngine.Vector3, $to: UnityEngine.Vector3, $axis: UnityEngine.Vector3) : number
            /** Returns the distance between a and b. */
            public static Distance ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3) : number
            /** Returns a copy of vector with its magnitude clamped to maxLength. */
            public static ClampMagnitude ($vector: UnityEngine.Vector3, $maxLength: number) : UnityEngine.Vector3
            public static Magnitude ($vector: UnityEngine.Vector3) : number
            public static SqrMagnitude ($vector: UnityEngine.Vector3) : number
            /** Returns a vector that is made from the smallest components of two vectors. */
            public static Min ($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Returns a vector that is made from the largest components of two vectors. */
            public static Max ($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3) : UnityEngine.Vector3
            public static op_Addition ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3) : UnityEngine.Vector3
            public static op_Subtraction ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3) : UnityEngine.Vector3
            public static op_UnaryNegation ($a: UnityEngine.Vector3) : UnityEngine.Vector3
            public static op_Multiply ($a: UnityEngine.Vector3, $d: number) : UnityEngine.Vector3
            public static op_Multiply ($d: number, $a: UnityEngine.Vector3) : UnityEngine.Vector3
            public static op_Division ($a: UnityEngine.Vector3, $d: number) : UnityEngine.Vector3
            public static op_Equality ($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3) : boolean
            public static op_Inequality ($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3) : boolean
            public ToString () : string
            /** Returns a nicely formatted string for this vector. */
            public ToString ($format: string) : string
            public constructor ($x: number, $y: number, $z: number)
            public constructor ($x: number, $y: number)
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        /** Quaternions are used to represent rotations. */
        class Quaternion extends System.ValueType implements System.IEquatable$1<UnityEngine.Quaternion>
        {
        /** X component of the Quaternion. Don't modify this directly unless you know quaternions inside out. */
            public x : number/** Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out. */
            public y : number/** Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out. */
            public z : number/** W component of the Quaternion. Do not directly modify quaternions. */
            public w : number
            public static kEpsilon : number/** The identity rotation (Read Only). */
            public static get identity(): UnityEngine.Quaternion;
            /** Returns or sets the euler angle representation of the rotation. */
            public get eulerAngles(): UnityEngine.Vector3;
            public set eulerAngles(value: UnityEngine.Vector3);
            /** Returns this quaternion with a magnitude of 1 (Read Only). */
            public get normalized(): UnityEngine.Quaternion;
            /** Creates a rotation which rotates from fromDirection to toDirection. */
            public static FromToRotation ($fromDirection: UnityEngine.Vector3, $toDirection: UnityEngine.Vector3) : UnityEngine.Quaternion
            /** Returns the Inverse of rotation. */
            public static Inverse ($rotation: UnityEngine.Quaternion) : UnityEngine.Quaternion
            /** Spherically interpolates between a and b by t. The parameter t is clamped to the range [0, 1]. */
            public static Slerp ($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number) : UnityEngine.Quaternion
            /** Spherically interpolates between a and b by t. The parameter t is not clamped. */
            public static SlerpUnclamped ($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number) : UnityEngine.Quaternion
            /** Interpolates between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1]. */
            public static Lerp ($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number) : UnityEngine.Quaternion
            /** Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped. */
            public static LerpUnclamped ($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number) : UnityEngine.Quaternion
            /** Creates a rotation which rotates angle degrees around axis. */
            public static AngleAxis ($angle: number, $axis: UnityEngine.Vector3) : UnityEngine.Quaternion
            /** Creates a rotation with the specified forward and upwards directions. * @param forward The direction to look in.
            * @param upwards The vector that defines in which direction up is.
            */
            public static LookRotation ($forward: UnityEngine.Vector3, $upwards: UnityEngine.Vector3) : UnityEngine.Quaternion
            /** Creates a rotation with the specified forward and upwards directions. * @param forward The direction to look in.
            * @param upwards The vector that defines in which direction up is.
            */
            public static LookRotation ($forward: UnityEngine.Vector3) : UnityEngine.Quaternion
            public get_Item ($index: number) : number
            public set_Item ($index: number, $value: number) : void
            /** Set x, y, z and w components of an existing Quaternion. */
            public Set ($newX: number, $newY: number, $newZ: number, $newW: number) : void
            public static op_Multiply ($lhs: UnityEngine.Quaternion, $rhs: UnityEngine.Quaternion) : UnityEngine.Quaternion
            public static op_Multiply ($rotation: UnityEngine.Quaternion, $point: UnityEngine.Vector3) : UnityEngine.Vector3
            public static op_Equality ($lhs: UnityEngine.Quaternion, $rhs: UnityEngine.Quaternion) : boolean
            public static op_Inequality ($lhs: UnityEngine.Quaternion, $rhs: UnityEngine.Quaternion) : boolean
            /** The dot product between two rotations. */
            public static Dot ($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion) : number
            /** Creates a rotation with the specified forward and upwards directions. * @param view The direction to look in.
            * @param up The vector that defines in which direction up is.
            */
            public SetLookRotation ($view: UnityEngine.Vector3) : void
            /** Creates a rotation with the specified forward and upwards directions. * @param view The direction to look in.
            * @param up The vector that defines in which direction up is.
            */
            public SetLookRotation ($view: UnityEngine.Vector3, $up: UnityEngine.Vector3) : void
            /** Returns the angle in degrees between two rotations a and b. */
            public static Angle ($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion) : number
            /** Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis. */
            public static Euler ($x: number, $y: number, $z: number) : UnityEngine.Quaternion
            /** Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis. */
            public static Euler ($euler: UnityEngine.Vector3) : UnityEngine.Quaternion
            /** Converts a rotation to angle-axis representation (angles in degrees). */
            public ToAngleAxis ($angle: $Ref<number>, $axis: $Ref<UnityEngine.Vector3>) : void
            /** Creates a rotation which rotates from fromDirection to toDirection. */
            public SetFromToRotation ($fromDirection: UnityEngine.Vector3, $toDirection: UnityEngine.Vector3) : void
            /** Rotates a rotation from towards to. */
            public static RotateTowards ($from: UnityEngine.Quaternion, $to: UnityEngine.Quaternion, $maxDegreesDelta: number) : UnityEngine.Quaternion
            /** Converts this quaternion to one with the same orientation but with a magnitude of 1. */
            public static Normalize ($q: UnityEngine.Quaternion) : UnityEngine.Quaternion
            public Normalize () : void
            public Equals ($other: any) : boolean
            public Equals ($other: UnityEngine.Quaternion) : boolean
            public ToString () : string
            /** Returns a nicely formatted string of the Quaternion. */
            public ToString ($format: string) : string
            public constructor ($x: number, $y: number, $z: number, $w: number)
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        /** Position, rotation and scale of an object. */
        class Transform extends UnityEngine.Component implements System.Collections.IEnumerable
        {
        /** The world space position of the Transform. */
            public get position(): UnityEngine.Vector3;
            public set position(value: UnityEngine.Vector3);
            /** Position of the transform relative to the parent transform. */
            public get localPosition(): UnityEngine.Vector3;
            public set localPosition(value: UnityEngine.Vector3);
            /** The rotation as Euler angles in degrees. */
            public get eulerAngles(): UnityEngine.Vector3;
            public set eulerAngles(value: UnityEngine.Vector3);
            /** The rotation as Euler angles in degrees relative to the parent transform's rotation. */
            public get localEulerAngles(): UnityEngine.Vector3;
            public set localEulerAngles(value: UnityEngine.Vector3);
            /** The red axis of the transform in world space. */
            public get right(): UnityEngine.Vector3;
            public set right(value: UnityEngine.Vector3);
            /** The green axis of the transform in world space. */
            public get up(): UnityEngine.Vector3;
            public set up(value: UnityEngine.Vector3);
            /** The blue axis of the transform in world space. */
            public get forward(): UnityEngine.Vector3;
            public set forward(value: UnityEngine.Vector3);
            /** A quaternion that stores the rotation of the Transform in world space. */
            public get rotation(): UnityEngine.Quaternion;
            public set rotation(value: UnityEngine.Quaternion);
            /** The rotation of the transform relative to the transform rotation of the parent. */
            public get localRotation(): UnityEngine.Quaternion;
            public set localRotation(value: UnityEngine.Quaternion);
            /** The scale of the transform relative to the parent. */
            public get localScale(): UnityEngine.Vector3;
            public set localScale(value: UnityEngine.Vector3);
            /** The parent of the transform. */
            public get parent(): UnityEngine.Transform;
            public set parent(value: UnityEngine.Transform);
            /** Matrix that transforms a point from world space into local space (Read Only). */
            public get worldToLocalMatrix(): UnityEngine.Matrix4x4;
            /** Matrix that transforms a point from local space into world space (Read Only). */
            public get localToWorldMatrix(): UnityEngine.Matrix4x4;
            /** Returns the topmost transform in the hierarchy. */
            public get root(): UnityEngine.Transform;
            /** The number of children the parent Transform has. */
            public get childCount(): number;
            /** The global scale of the object (Read Only). */
            public get lossyScale(): UnityEngine.Vector3;
            /** Has the transform changed since the last time the flag was set to 'false'? */
            public get hasChanged(): boolean;
            public set hasChanged(value: boolean);
            /** The transform capacity of the transform's hierarchy data structure. */
            public get hierarchyCapacity(): number;
            public set hierarchyCapacity(value: number);
            /** The number of transforms in the transform's hierarchy data structure. */
            public get hierarchyCount(): number;
            /** Set the parent of the transform. * @param parent The parent Transform to use.
            * @param worldPositionStays If true, the parent-relative position, scale and
            rotation are modified such that the object keeps the same world space position,
            rotation and scale as before.
            */
            public SetParent ($p: UnityEngine.Transform) : void
            /** Set the parent of the transform. * @param parent The parent Transform to use.
            * @param worldPositionStays If true, the parent-relative position, scale and
            rotation are modified such that the object keeps the same world space position,
            rotation and scale as before.
            */
            public SetParent ($parent: UnityEngine.Transform, $worldPositionStays: boolean) : void
            /** Sets the world space position and rotation of the Transform component. */
            public SetPositionAndRotation ($position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion) : void
            /** Moves the transform in the direction and distance of translation. */
            public Translate ($translation: UnityEngine.Vector3, $relativeTo: UnityEngine.Space) : void
            /** Moves the transform in the direction and distance of translation. */
            public Translate ($translation: UnityEngine.Vector3) : void
            /** Moves the transform by x along the x axis, y along the y axis, and z along the z axis. */
            public Translate ($x: number, $y: number, $z: number, $relativeTo: UnityEngine.Space) : void
            /** Moves the transform by x along the x axis, y along the y axis, and z along the z axis. */
            public Translate ($x: number, $y: number, $z: number) : void
            /** Moves the transform in the direction and distance of translation. */
            public Translate ($translation: UnityEngine.Vector3, $relativeTo: UnityEngine.Transform) : void
            /** Moves the transform by x along the x axis, y along the y axis, and z along the z axis. */
            public Translate ($x: number, $y: number, $z: number, $relativeTo: UnityEngine.Transform) : void
            /** Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order). * @param eulers The rotation to apply.
            * @param relativeTo Determines whether to rotate the GameObject either locally to  the GameObject or relative to the Scene in world space.
            */
            public Rotate ($eulers: UnityEngine.Vector3, $relativeTo: UnityEngine.Space) : void
            public Rotate ($eulers: UnityEngine.Vector3) : void
            /** Applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order). * @param relativeTo Determines whether to rotate the GameObject either locally to the GameObject or relative to the Scene in world space.
            * @param xAngle Degrees to rotate the GameObject around the X axis.
            * @param yAngle Degrees to rotate the GameObject around the Y axis.
            * @param zAngle Degrees to rotate the GameObject around the Z axis.
            */
            public Rotate ($xAngle: number, $yAngle: number, $zAngle: number, $relativeTo: UnityEngine.Space) : void
            public Rotate ($xAngle: number, $yAngle: number, $zAngle: number) : void
            /** Rotates the object around the given axis by the number of degrees defined by the given angle. * @param angle The degrees of rotation to apply.
            * @param axis The axis to apply rotation to.
            * @param relativeTo Determines whether to rotate the GameObject either locally to the GameObject or relative to the Scene in world space.
            */
            public Rotate ($axis: UnityEngine.Vector3, $angle: number, $relativeTo: UnityEngine.Space) : void
            public Rotate ($axis: UnityEngine.Vector3, $angle: number) : void
            /** Rotates the transform about axis passing through point in world coordinates by angle degrees. */
            public RotateAround ($point: UnityEngine.Vector3, $axis: UnityEngine.Vector3, $angle: number) : void
            /** Rotates the transform so the forward vector points at target's current position. * @param target Object to point towards.
            * @param worldUp Vector specifying the upward direction.
            */
            public LookAt ($target: UnityEngine.Transform, $worldUp: UnityEngine.Vector3) : void
            /** Rotates the transform so the forward vector points at target's current position. * @param target Object to point towards.
            * @param worldUp Vector specifying the upward direction.
            */
            public LookAt ($target: UnityEngine.Transform) : void
            /** Rotates the transform so the forward vector points at worldPosition. * @param worldPosition Point to look at.
            * @param worldUp Vector specifying the upward direction.
            */
            public LookAt ($worldPosition: UnityEngine.Vector3, $worldUp: UnityEngine.Vector3) : void
            /** Rotates the transform so the forward vector points at worldPosition. * @param worldPosition Point to look at.
            * @param worldUp Vector specifying the upward direction.
            */
            public LookAt ($worldPosition: UnityEngine.Vector3) : void
            /** Transforms direction from local space to world space. */
            public TransformDirection ($direction: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms direction x, y, z from local space to world space. */
            public TransformDirection ($x: number, $y: number, $z: number) : UnityEngine.Vector3
            /** Transforms a direction from world space to local space. The opposite of Transform.TransformDirection. */
            public InverseTransformDirection ($direction: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms the direction x, y, z from world space to local space. The opposite of Transform.TransformDirection. */
            public InverseTransformDirection ($x: number, $y: number, $z: number) : UnityEngine.Vector3
            /** Transforms vector from local space to world space. */
            public TransformVector ($vector: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms vector x, y, z from local space to world space. */
            public TransformVector ($x: number, $y: number, $z: number) : UnityEngine.Vector3
            /** Transforms a vector from world space to local space. The opposite of Transform.TransformVector. */
            public InverseTransformVector ($vector: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms the vector x, y, z from world space to local space. The opposite of Transform.TransformVector. */
            public InverseTransformVector ($x: number, $y: number, $z: number) : UnityEngine.Vector3
            /** Transforms position from local space to world space. */
            public TransformPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms the position x, y, z from local space to world space. */
            public TransformPoint ($x: number, $y: number, $z: number) : UnityEngine.Vector3
            /** Transforms position from world space to local space. */
            public InverseTransformPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms the position x, y, z from world space to local space. The opposite of Transform.TransformPoint. */
            public InverseTransformPoint ($x: number, $y: number, $z: number) : UnityEngine.Vector3
            public DetachChildren () : void
            public SetAsFirstSibling () : void
            public SetAsLastSibling () : void
            /** Sets the sibling index. * @param index Index to set.
            */
            public SetSiblingIndex ($index: number) : void
            public GetSiblingIndex () : number
            /** Finds a child by n and returns it.
            * @param n Name of child to be found.
            * @returns The returned child transform or null if no child is found. 
            */
            public Find ($n: string) : UnityEngine.Transform
            /** Is this transform a child of parent? */
            public IsChildOf ($parent: UnityEngine.Transform) : boolean
            public GetEnumerator () : System.Collections.IEnumerator
            /** Returns a transform child by index.
            * @param index Index of the child transform to return. Must be smaller than Transform.childCount.
            * @returns Transform child by index. 
            */
            public GetChild ($index: number) : UnityEngine.Transform
        }
        /** Base class for everything attached to GameObjects. */
        class Component extends UnityEngine.Object
        {
        /** The Transform attached to this GameObject. */
            public get transform(): UnityEngine.Transform;
            /** The game object this component is attached to. A component is always attached to a game object. */
            public get gameObject(): UnityEngine.GameObject;
            /** The tag of this game object. */
            public get tag(): string;
            public set tag(value: string);
            /** Returns the component of Type type if the game object has one attached, null if it doesn't. * @param type The type of Component to retrieve.
            */
            public GetComponent ($type: System.Type) : UnityEngine.Component
            /** Returns the component with name type if the game object has one attached, null if it doesn't. */
            public GetComponent ($type: string) : UnityEngine.Component
            public GetComponentInChildren ($t: System.Type, $includeInactive: boolean) : UnityEngine.Component
            /** Returns the component of Type type in the GameObject or any of its children using depth first search.
            * @param t The type of Component to retrieve.
            * @returns A component of the matching type, if found. 
            */
            public GetComponentInChildren ($t: System.Type) : UnityEngine.Component
            /** Returns all components of Type type in the GameObject or any of its children. * @param t The type of Component to retrieve.
            * @param includeInactive Should Components on inactive GameObjects be included in the found set? includeInactive decides which children of the GameObject will be searched.  The GameObject that you call GetComponentsInChildren on is always searched regardless.
            */
            public GetComponentsInChildren ($t: System.Type, $includeInactive: boolean) : System.Array$1<UnityEngine.Component>
            public GetComponentsInChildren ($t: System.Type) : System.Array$1<UnityEngine.Component>
            /** Returns the component of Type type in the GameObject or any of its parents.
            * @param t The type of Component to retrieve.
            * @returns A component of the matching type, if found. 
            */
            public GetComponentInParent ($t: System.Type) : UnityEngine.Component
            /** Returns all components of Type type in the GameObject or any of its parents. * @param t The type of Component to retrieve.
            * @param includeInactive Should inactive Components be included in the found set?
            */
            public GetComponentsInParent ($t: System.Type, $includeInactive: boolean) : System.Array$1<UnityEngine.Component>
            public GetComponentsInParent ($t: System.Type) : System.Array$1<UnityEngine.Component>
            /** Returns all components of Type type in the GameObject. * @param type The type of Component to retrieve.
            */
            public GetComponents ($type: System.Type) : System.Array$1<UnityEngine.Component>
            public GetComponents ($type: System.Type, $results: System.Collections.Generic.List$1<UnityEngine.Component>) : void
            /** Is this game object tagged with tag ? * @param tag The tag to compare.
            */
            public CompareTag ($tag: string) : boolean
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName Name of method to call.
            * @param value Optional parameter value for the method.
            * @param options Should an error be raised if the method does not exist on the target object?
            */
            public SendMessageUpwards ($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName Name of method to call.
            * @param value Optional parameter value for the method.
            * @param options Should an error be raised if the method does not exist on the target object?
            */
            public SendMessageUpwards ($methodName: string, $value: any) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName Name of method to call.
            * @param value Optional parameter value for the method.
            * @param options Should an error be raised if the method does not exist on the target object?
            */
            public SendMessageUpwards ($methodName: string) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName Name of method to call.
            * @param value Optional parameter value for the method.
            * @param options Should an error be raised if the method does not exist on the target object?
            */
            public SendMessageUpwards ($methodName: string, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName Name of the method to call.
            * @param value Optional parameter for the method.
            * @param options Should an error be raised if the target object doesn't implement the method for the message?
            */
            public SendMessage ($methodName: string, $value: any) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName Name of the method to call.
            * @param value Optional parameter for the method.
            * @param options Should an error be raised if the target object doesn't implement the method for the message?
            */
            public SendMessage ($methodName: string) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName Name of the method to call.
            * @param value Optional parameter for the method.
            * @param options Should an error be raised if the target object doesn't implement the method for the message?
            */
            public SendMessage ($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName Name of the method to call.
            * @param value Optional parameter for the method.
            * @param options Should an error be raised if the target object doesn't implement the method for the message?
            */
            public SendMessage ($methodName: string, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. * @param methodName Name of the method to call.
            * @param parameter Optional parameter to pass to the method (can be any value).
            * @param options Should an error be raised if the method does not exist for a given target object?
            */
            public BroadcastMessage ($methodName: string, $parameter: any, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. * @param methodName Name of the method to call.
            * @param parameter Optional parameter to pass to the method (can be any value).
            * @param options Should an error be raised if the method does not exist for a given target object?
            */
            public BroadcastMessage ($methodName: string, $parameter: any) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. * @param methodName Name of the method to call.
            * @param parameter Optional parameter to pass to the method (can be any value).
            * @param options Should an error be raised if the method does not exist for a given target object?
            */
            public BroadcastMessage ($methodName: string) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. * @param methodName Name of the method to call.
            * @param parameter Optional parameter to pass to the method (can be any value).
            * @param options Should an error be raised if the method does not exist for a given target object?
            */
            public BroadcastMessage ($methodName: string, $options: UnityEngine.SendMessageOptions) : void
            public constructor ()
        }
        /** Bit mask that controls object destruction, saving and visibility in inspectors. */
        enum HideFlags
        { None = 0, HideInHierarchy = 1, HideInInspector = 2, DontSaveInEditor = 4, NotEditable = 8, DontSaveInBuild = 16, DontUnloadUnusedAsset = 32, DontSave = 52, HideAndDontSave = 61 }
        /** Base class for all entities in Unity Scenes. */
        class GameObject extends UnityEngine.Object
        {
        /** The Transform attached to this GameObject. */
            public get transform(): UnityEngine.Transform;
            /** The layer the game object is in. */
            public get layer(): number;
            public set layer(value: number);
            /** The local active state of this GameObject. (Read Only) */
            public get activeSelf(): boolean;
            /** Defines whether the GameObject is active in the Scene. */
            public get activeInHierarchy(): boolean;
            /** Editor only API that specifies if a game object is static. */
            public get isStatic(): boolean;
            public set isStatic(value: boolean);
            /** The tag of this game object. */
            public get tag(): string;
            public set tag(value: string);
            /** Scene that the GameObject is part of. */
            public get scene(): UnityEngine.SceneManagement.Scene;
            public get gameObject(): UnityEngine.GameObject;
            /** Creates a game object with a primitive mesh renderer and appropriate collider. * @param type The type of primitive object to create.
            */
            public static CreatePrimitive ($type: UnityEngine.PrimitiveType) : UnityEngine.GameObject
            /** Returns the component of Type type if the game object has one attached, null if it doesn't. * @param type The type of Component to retrieve.
            */
            public GetComponent ($type: System.Type) : UnityEngine.Component
            /** Returns the component with name type if the game object has one attached, null if it doesn't. * @param type The type of Component to retrieve.
            */
            public GetComponent ($type: string) : UnityEngine.Component
            /** Returns the component of Type type in the GameObject or any of its children using depth first search.
            * @param type The type of Component to retrieve.
            * @returns A component of the matching type, if found. 
            */
            public GetComponentInChildren ($type: System.Type, $includeInactive: boolean) : UnityEngine.Component
            /** Returns the component of Type type in the GameObject or any of its children using depth first search.
            * @param type The type of Component to retrieve.
            * @returns A component of the matching type, if found. 
            */
            public GetComponentInChildren ($type: System.Type) : UnityEngine.Component
            /** Returns the component of Type type in the GameObject or any of its parents. * @param type Type of component to find.
            */
            public GetComponentInParent ($type: System.Type) : UnityEngine.Component
            /** Returns all components of Type type in the GameObject. * @param type The type of Component to retrieve.
            */
            public GetComponents ($type: System.Type) : System.Array$1<UnityEngine.Component>
            public GetComponents ($type: System.Type, $results: System.Collections.Generic.List$1<UnityEngine.Component>) : void
            /** Returns all components of Type type in the GameObject or any of its children. * @param type The type of Component to retrieve.
            * @param includeInactive Should Components on inactive GameObjects be included in the found set?
            */
            public GetComponentsInChildren ($type: System.Type) : System.Array$1<UnityEngine.Component>
            /** Returns all components of Type type in the GameObject or any of its children. * @param type The type of Component to retrieve.
            * @param includeInactive Should Components on inactive GameObjects be included in the found set?
            */
            public GetComponentsInChildren ($type: System.Type, $includeInactive: boolean) : System.Array$1<UnityEngine.Component>
            public GetComponentsInParent ($type: System.Type) : System.Array$1<UnityEngine.Component>
            /** Returns all components of Type type in the GameObject or any of its parents. * @param type The type of Component to retrieve.
            * @param includeInactive Should inactive Components be included in the found set?
            */
            public GetComponentsInParent ($type: System.Type, $includeInactive: boolean) : System.Array$1<UnityEngine.Component>
            /** Returns one active GameObject tagged tag. Returns null if no GameObject was found. * @param tag The tag to search for.
            */
            public static FindWithTag ($tag: string) : UnityEngine.GameObject
            public SendMessageUpwards ($methodName: string, $options: UnityEngine.SendMessageOptions) : void
            public SendMessage ($methodName: string, $options: UnityEngine.SendMessageOptions) : void
            public BroadcastMessage ($methodName: string, $options: UnityEngine.SendMessageOptions) : void
            /** Adds a component class of type componentType to the game object. C# Users can use a generic version. */
            public AddComponent ($componentType: System.Type) : UnityEngine.Component
            /** ActivatesDeactivates the GameObject, depending on the given true or false/ value. * @param value Activate or deactivate the object, where true activates the GameObject and false deactivates the GameObject.
            */
            public SetActive ($value: boolean) : void
            /** Is this game object tagged with tag ? * @param tag The tag to compare.
            */
            public CompareTag ($tag: string) : boolean
            public static FindGameObjectWithTag ($tag: string) : UnityEngine.GameObject
            /** Returns a list of active GameObjects tagged tag. Returns empty array if no GameObject was found. * @param tag The name of the tag to search GameObjects for.
            */
            public static FindGameObjectsWithTag ($tag: string) : System.Array$1<UnityEngine.GameObject>
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName The name of the method to call.
            * @param value An optional parameter value to pass to the called method.
            * @param options Should an error be raised if the method doesn't exist on the target object?
            */
            public SendMessageUpwards ($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName The name of the method to call.
            * @param value An optional parameter value to pass to the called method.
            * @param options Should an error be raised if the method doesn't exist on the target object?
            */
            public SendMessageUpwards ($methodName: string, $value: any) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. * @param methodName The name of the method to call.
            * @param value An optional parameter value to pass to the called method.
            * @param options Should an error be raised if the method doesn't exist on the target object?
            */
            public SendMessageUpwards ($methodName: string) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName The name of the method to call.
            * @param value An optional parameter value to pass to the called method.
            * @param options Should an error be raised if the method doesn't exist on the target object?
            */
            public SendMessage ($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName The name of the method to call.
            * @param value An optional parameter value to pass to the called method.
            * @param options Should an error be raised if the method doesn't exist on the target object?
            */
            public SendMessage ($methodName: string, $value: any) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object. * @param methodName The name of the method to call.
            * @param value An optional parameter value to pass to the called method.
            * @param options Should an error be raised if the method doesn't exist on the target object?
            */
            public SendMessage ($methodName: string) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. */
            public BroadcastMessage ($methodName: string, $parameter: any, $options: UnityEngine.SendMessageOptions) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. */
            public BroadcastMessage ($methodName: string, $parameter: any) : void
            /** Calls the method named methodName on every MonoBehaviour in this game object or any of its children. */
            public BroadcastMessage ($methodName: string) : void
            /** Finds a GameObject by name and returns it. */
            public static Find ($name: string) : UnityEngine.GameObject
            public constructor ($name: string)
            public constructor ()
            public constructor ($name: string, ...components: System.Type[])
        }
        /** The various primitives that can be created using the GameObject.CreatePrimitive function. */
        enum PrimitiveType
        { Sphere = 0, Capsule = 1, Cylinder = 2, Cube = 3, Plane = 4, Quad = 5 }
        /** Options for how to send a message. */
        enum SendMessageOptions
        { RequireReceiver = 0, DontRequireReceiver = 1 }
        /** Behaviours are Components that can be enabled or disabled. */
        class Behaviour extends UnityEngine.Component
        {
        /** Enabled Behaviours are Updated, disabled Behaviours are not. */
            public get enabled(): boolean;
            public set enabled(value: boolean);
            /** Has the Behaviour had active and enabled called? */
            public get isActiveAndEnabled(): boolean;
            public constructor ()
        }
        /** A standard 4x4 transformation matrix. */
        class Matrix4x4 extends System.ValueType implements System.IEquatable$1<UnityEngine.Matrix4x4>
        {
            public m00 : number
            public m10 : number
            public m20 : number
            public m30 : number
            public m01 : number
            public m11 : number
            public m21 : number
            public m31 : number
            public m02 : number
            public m12 : number
            public m22 : number
            public m32 : number
            public m03 : number
            public m13 : number
            public m23 : number
            public m33 : number/** Attempts to get a rotation quaternion from this matrix. */
            public get rotation(): UnityEngine.Quaternion;
            /** Attempts to get a scale value from the matrix. */
            public get lossyScale(): UnityEngine.Vector3;
            /** Is this the identity matrix? */
            public get isIdentity(): boolean;
            /** The determinant of the matrix. */
            public get determinant(): number;
            /** This property takes a projection matrix and returns the six plane coordinates that define a projection frustum. */
            public get decomposeProjection(): UnityEngine.FrustumPlanes;
            /** The inverse of this matrix (Read Only). */
            public get inverse(): UnityEngine.Matrix4x4;
            /** Returns the transpose of this matrix (Read Only). */
            public get transpose(): UnityEngine.Matrix4x4;
            /** Returns a matrix with all elements set to zero (Read Only). */
            public static get zero(): UnityEngine.Matrix4x4;
            /** Returns the identity matrix (Read Only). */
            public static get identity(): UnityEngine.Matrix4x4;
            public ValidTRS () : boolean
            public static Determinant ($m: UnityEngine.Matrix4x4) : number
            /** Creates a translation, rotation and scaling matrix. */
            public static TRS ($pos: UnityEngine.Vector3, $q: UnityEngine.Quaternion, $s: UnityEngine.Vector3) : UnityEngine.Matrix4x4
            /** Sets this matrix to a translation, rotation and scaling matrix. */
            public SetTRS ($pos: UnityEngine.Vector3, $q: UnityEngine.Quaternion, $s: UnityEngine.Vector3) : void
            public static Inverse ($m: UnityEngine.Matrix4x4) : UnityEngine.Matrix4x4
            public static Transpose ($m: UnityEngine.Matrix4x4) : UnityEngine.Matrix4x4
            /** Creates an orthogonal projection matrix. */
            public static Ortho ($left: number, $right: number, $bottom: number, $top: number, $zNear: number, $zFar: number) : UnityEngine.Matrix4x4
            /** Creates a perspective projection matrix. */
            public static Perspective ($fov: number, $aspect: number, $zNear: number, $zFar: number) : UnityEngine.Matrix4x4
            /** Given a source point, a target point, and an up vector, computes a transformation matrix that corresponds to a camera viewing the target from the source, such that the right-hand vector is perpendicular to the up vector.
            * @param from The source point.
            * @param to The target point.
            * @param up The vector describing the up direction (typically Vector3.up).
            * @returns The resulting transformation matrix. 
            */
            public static LookAt ($from: UnityEngine.Vector3, $to: UnityEngine.Vector3, $up: UnityEngine.Vector3) : UnityEngine.Matrix4x4
            /** This function returns a projection matrix with viewing frustum that has a near plane defined by the coordinates that were passed in.
            * @param left The X coordinate of the left side of the near projection plane in view space.
            * @param right The X coordinate of the right side of the near projection plane in view space.
            * @param bottom The Y coordinate of the bottom side of the near projection plane in view space.
            * @param top The Y coordinate of the top side of the near projection plane in view space.
            * @param zNear Z distance to the near plane from the origin in view space.
            * @param zFar Z distance to the far plane from the origin in view space.
            * @param frustumPlanes Frustum planes struct that contains the view space coordinates of that define a viewing frustum.
            * @returns A projection matrix with a viewing frustum defined by the plane coordinates passed in. 
            */
            public static Frustum ($left: number, $right: number, $bottom: number, $top: number, $zNear: number, $zFar: number) : UnityEngine.Matrix4x4
            /** This function returns a projection matrix with viewing frustum that has a near plane defined by the coordinates that were passed in.
            * @param left The X coordinate of the left side of the near projection plane in view space.
            * @param right The X coordinate of the right side of the near projection plane in view space.
            * @param bottom The Y coordinate of the bottom side of the near projection plane in view space.
            * @param top The Y coordinate of the top side of the near projection plane in view space.
            * @param zNear Z distance to the near plane from the origin in view space.
            * @param zFar Z distance to the far plane from the origin in view space.
            * @param frustumPlanes Frustum planes struct that contains the view space coordinates of that define a viewing frustum.
            * @returns A projection matrix with a viewing frustum defined by the plane coordinates passed in. 
            */
            public static Frustum ($fp: UnityEngine.FrustumPlanes) : UnityEngine.Matrix4x4
            public get_Item ($index: number) : number
            public set_Item ($index: number, $value: number) : void
            public Equals ($other: any) : boolean
            public Equals ($other: UnityEngine.Matrix4x4) : boolean
            public static op_Multiply ($lhs: UnityEngine.Matrix4x4, $rhs: UnityEngine.Matrix4x4) : UnityEngine.Matrix4x4
            public static op_Multiply ($lhs: UnityEngine.Matrix4x4, $vector: UnityEngine.Vector4) : UnityEngine.Vector4
            public static op_Equality ($lhs: UnityEngine.Matrix4x4, $rhs: UnityEngine.Matrix4x4) : boolean
            public static op_Inequality ($lhs: UnityEngine.Matrix4x4, $rhs: UnityEngine.Matrix4x4) : boolean
            /** Get a column of the matrix. */
            public GetColumn ($index: number) : UnityEngine.Vector4
            /** Returns a row of the matrix. */
            public GetRow ($index: number) : UnityEngine.Vector4
            /** Sets a column of the matrix. */
            public SetColumn ($index: number, $column: UnityEngine.Vector4) : void
            /** Sets a row of the matrix. */
            public SetRow ($index: number, $row: UnityEngine.Vector4) : void
            /** Transforms a position by this matrix (generic). */
            public MultiplyPoint ($point: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms a position by this matrix (fast). */
            public MultiplyPoint3x4 ($point: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms a direction by this matrix. */
            public MultiplyVector ($vector: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Returns a plane that is transformed in space. */
            public TransformPlane ($plane: UnityEngine.Plane) : UnityEngine.Plane
            /** Creates a scaling matrix. */
            public static Scale ($vector: UnityEngine.Vector3) : UnityEngine.Matrix4x4
            /** Creates a translation matrix. */
            public static Translate ($vector: UnityEngine.Vector3) : UnityEngine.Matrix4x4
            /** Creates a rotation matrix. */
            public static Rotate ($q: UnityEngine.Quaternion) : UnityEngine.Matrix4x4
            public ToString () : string
            /** Returns a nicely formatted string for this matrix. */
            public ToString ($format: string) : string
            public constructor ($column0: UnityEngine.Vector4, $column1: UnityEngine.Vector4, $column2: UnityEngine.Vector4, $column3: UnityEngine.Vector4)
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        /** The coordinate space in which to operate. */
        enum Space
        { World = 0, Self = 1 }
        /** Interface to control the Mecanim animation system. */
        class Animator extends UnityEngine.Behaviour
        {
        /** Returns true if the current rig is optimizable with AnimatorUtility.OptimizeTransformHierarchy. */
            public get isOptimizable(): boolean;
            /** Returns true if the current rig is humanoid, false if it is generic. */
            public get isHuman(): boolean;
            /** Returns true if the current rig has root motion. */
            public get hasRootMotion(): boolean;
            /** Returns the scale of the current Avatar for a humanoid rig, (1 by default if the rig is generic). */
            public get humanScale(): number;
            /** Returns whether the animator is initialized successfully. */
            public get isInitialized(): boolean;
            /** Gets the avatar delta position for the last evaluated frame. */
            public get deltaPosition(): UnityEngine.Vector3;
            /** Gets the avatar delta rotation for the last evaluated frame. */
            public get deltaRotation(): UnityEngine.Quaternion;
            /** Gets the avatar velocity  for the last evaluated frame. */
            public get velocity(): UnityEngine.Vector3;
            /** Gets the avatar angular velocity for the last evaluated frame. */
            public get angularVelocity(): UnityEngine.Vector3;
            /** The root position, the position of the game object. */
            public get rootPosition(): UnityEngine.Vector3;
            public set rootPosition(value: UnityEngine.Vector3);
            /** The root rotation, the rotation of the game object. */
            public get rootRotation(): UnityEngine.Quaternion;
            public set rootRotation(value: UnityEngine.Quaternion);
            /** Should root motion be applied? */
            public get applyRootMotion(): boolean;
            public set applyRootMotion(value: boolean);
            /** Specifies the update mode of the Animator. */
            public get updateMode(): UnityEngine.AnimatorUpdateMode;
            public set updateMode(value: UnityEngine.AnimatorUpdateMode);
            /** Returns true if the object has a transform hierarchy. */
            public get hasTransformHierarchy(): boolean;
            /** The current gravity weight based on current animations that are played. */
            public get gravityWeight(): number;
            /** The position of the body center of mass. */
            public get bodyPosition(): UnityEngine.Vector3;
            public set bodyPosition(value: UnityEngine.Vector3);
            /** The rotation of the body center of mass. */
            public get bodyRotation(): UnityEngine.Quaternion;
            public set bodyRotation(value: UnityEngine.Quaternion);
            /** Automatic stabilization of feet during transition and blending. */
            public get stabilizeFeet(): boolean;
            public set stabilizeFeet(value: boolean);
            /** Returns the number of layers in the controller. */
            public get layerCount(): number;
            /** The AnimatorControllerParameter list used by the animator. (Read Only) */
            public get parameters(): System.Array$1<UnityEngine.AnimatorControllerParameter>;
            /** Returns the number of parameters in the controller. */
            public get parameterCount(): number;
            /** Blends pivot point between body center of mass and feet pivot. */
            public get feetPivotActive(): number;
            public set feetPivotActive(value: number);
            /** Gets the pivot weight. */
            public get pivotWeight(): number;
            /** Get the current position of the pivot. */
            public get pivotPosition(): UnityEngine.Vector3;
            /** If automatic matching is active. */
            public get isMatchingTarget(): boolean;
            /** The playback speed of the Animator. 1 is normal playback speed. */
            public get speed(): number;
            public set speed(value: number);
            /** Returns the position of the target specified by SetTarget. */
            public get targetPosition(): UnityEngine.Vector3;
            /** Returns the rotation of the target specified by SetTarget. */
            public get targetRotation(): UnityEngine.Quaternion;
            /** Controls culling of this Animator component. */
            public get cullingMode(): UnityEngine.AnimatorCullingMode;
            public set cullingMode(value: UnityEngine.AnimatorCullingMode);
            /** Sets the playback position in the recording buffer. */
            public get playbackTime(): number;
            public set playbackTime(value: number);
            /** Start time of the first frame of the buffer relative to the frame at which StartRecording was called. */
            public get recorderStartTime(): number;
            public set recorderStartTime(value: number);
            /** End time of the recorded clip relative to when StartRecording was called. */
            public get recorderStopTime(): number;
            public set recorderStopTime(value: number);
            /** Gets the mode of the Animator recorder. */
            public get recorderMode(): UnityEngine.AnimatorRecorderMode;
            /** The runtime representation of AnimatorController that controls the Animator. */
            public get runtimeAnimatorController(): UnityEngine.RuntimeAnimatorController;
            public set runtimeAnimatorController(value: UnityEngine.RuntimeAnimatorController);
            /** Returns true if Animator has any playables assigned to it. */
            public get hasBoundPlayables(): boolean;
            /** Gets/Sets the current Avatar. */
            public get avatar(): UnityEngine.Avatar;
            public set avatar(value: UnityEngine.Avatar);
            /** The PlayableGraph created by the Animator. */
            public get playableGraph(): UnityEngine.Playables.PlayableGraph;
            /** Additional layers affects the center of mass. */
            public get layersAffectMassCenter(): boolean;
            public set layersAffectMassCenter(value: boolean);
            /** Get left foot bottom height. */
            public get leftFeetBottomHeight(): number;
            /** Get right foot bottom height. */
            public get rightFeetBottomHeight(): number;
            public get logWarnings(): boolean;
            public set logWarnings(value: boolean);
            /** Sets whether the Animator sends events of type AnimationEvent. */
            public get fireEvents(): boolean;
            public set fireEvents(value: boolean);
            /** Controls the behaviour of the Animator component when a GameObject is disabled. */
            public get keepAnimatorControllerStateOnDisable(): boolean;
            public set keepAnimatorControllerStateOnDisable(value: boolean);
            /** Returns the value of the given float parameter.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns The value of the parameter. 
            */
            public GetFloat ($name: string) : number
            /** Returns the value of the given float parameter.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns The value of the parameter. 
            */
            public GetFloat ($id: number) : number
            /** Send float values to the Animator to affect transitions. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            * @param dampTime The damper total time.
            * @param deltaTime The delta time to give to the damper.
            */
            public SetFloat ($name: string, $value: number) : void
            /** Send float values to the Animator to affect transitions. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            * @param dampTime The damper total time.
            * @param deltaTime The delta time to give to the damper.
            */
            public SetFloat ($name: string, $value: number, $dampTime: number, $deltaTime: number) : void
            /** Send float values to the Animator to affect transitions. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            * @param dampTime The damper total time.
            * @param deltaTime The delta time to give to the damper.
            */
            public SetFloat ($id: number, $value: number) : void
            /** Send float values to the Animator to affect transitions. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            * @param dampTime The damper total time.
            * @param deltaTime The delta time to give to the damper.
            */
            public SetFloat ($id: number, $value: number, $dampTime: number, $deltaTime: number) : void
            /** Returns the value of the given boolean parameter.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns The value of the parameter. 
            */
            public GetBool ($name: string) : boolean
            /** Returns the value of the given boolean parameter.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns The value of the parameter. 
            */
            public GetBool ($id: number) : boolean
            /** Sets the value of the given boolean parameter. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            */
            public SetBool ($name: string, $value: boolean) : void
            /** Sets the value of the given boolean parameter. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            */
            public SetBool ($id: number, $value: boolean) : void
            /** Returns the value of the given integer parameter.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns The value of the parameter. 
            */
            public GetInteger ($name: string) : number
            /** Returns the value of the given integer parameter.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns The value of the parameter. 
            */
            public GetInteger ($id: number) : number
            /** Sets the value of the given integer parameter. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            */
            public SetInteger ($name: string, $value: number) : void
            /** Sets the value of the given integer parameter. * @param name The parameter name.
            * @param id The parameter ID.
            * @param value The new parameter value.
            */
            public SetInteger ($id: number, $value: number) : void
            /** Sets the value of the given trigger parameter. * @param name The parameter name.
            * @param id The parameter ID.
            */
            public SetTrigger ($name: string) : void
            /** Sets the value of the given trigger parameter. * @param name The parameter name.
            * @param id The parameter ID.
            */
            public SetTrigger ($id: number) : void
            /** Resets the value of the given trigger parameter. * @param name The parameter name.
            * @param id The parameter ID.
            */
            public ResetTrigger ($name: string) : void
            /** Resets the value of the given trigger parameter. * @param name The parameter name.
            * @param id The parameter ID.
            */
            public ResetTrigger ($id: number) : void
            /** Returns true if the parameter is controlled by a curve, false otherwise.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns True if the parameter is controlled by a curve, false otherwise. 
            */
            public IsParameterControlledByCurve ($name: string) : boolean
            /** Returns true if the parameter is controlled by a curve, false otherwise.
            * @param name The parameter name.
            * @param id The parameter ID.
            * @returns True if the parameter is controlled by a curve, false otherwise. 
            */
            public IsParameterControlledByCurve ($id: number) : boolean
            /** Gets the position of an IK goal.
            * @param goal The AvatarIKGoal that is queried.
            * @returns Return the current position of this IK goal in world space. 
            */
            public GetIKPosition ($goal: UnityEngine.AvatarIKGoal) : UnityEngine.Vector3
            /** Sets the position of an IK goal. * @param goal The AvatarIKGoal that is set.
            * @param goalPosition The position in world space.
            */
            public SetIKPosition ($goal: UnityEngine.AvatarIKGoal, $goalPosition: UnityEngine.Vector3) : void
            /** Gets the rotation of an IK goal. * @param goal The AvatarIKGoal that is is queried.
            */
            public GetIKRotation ($goal: UnityEngine.AvatarIKGoal) : UnityEngine.Quaternion
            /** Sets the rotation of an IK goal. * @param goal The AvatarIKGoal that is set.
            * @param goalRotation The rotation in world space.
            */
            public SetIKRotation ($goal: UnityEngine.AvatarIKGoal, $goalRotation: UnityEngine.Quaternion) : void
            /** Gets the translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal). * @param goal The AvatarIKGoal that is queried.
            */
            public GetIKPositionWeight ($goal: UnityEngine.AvatarIKGoal) : number
            /** Sets the translative weight of an IK goal (0 = at the original animation before IK, 1 = at the goal). * @param goal The AvatarIKGoal that is set.
            * @param value The translative weight.
            */
            public SetIKPositionWeight ($goal: UnityEngine.AvatarIKGoal, $value: number) : void
            /** Gets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal). * @param goal The AvatarIKGoal that is queried.
            */
            public GetIKRotationWeight ($goal: UnityEngine.AvatarIKGoal) : number
            /** Sets the rotational weight of an IK goal (0 = rotation before IK, 1 = rotation at the IK goal). * @param goal The AvatarIKGoal that is set.
            * @param value The rotational weight.
            */
            public SetIKRotationWeight ($goal: UnityEngine.AvatarIKGoal, $value: number) : void
            /** Gets the position of an IK hint.
            * @param hint The AvatarIKHint that is queried.
            * @returns Return the current position of this IK hint in world space. 
            */
            public GetIKHintPosition ($hint: UnityEngine.AvatarIKHint) : UnityEngine.Vector3
            /** Sets the position of an IK hint. * @param hint The AvatarIKHint that is set.
            * @param hintPosition The position in world space.
            */
            public SetIKHintPosition ($hint: UnityEngine.AvatarIKHint, $hintPosition: UnityEngine.Vector3) : void
            /** Gets the translative weight of an IK Hint (0 = at the original animation before IK, 1 = at the hint).
            * @param hint The AvatarIKHint that is queried.
            * @returns Return translative weight. 
            */
            public GetIKHintPositionWeight ($hint: UnityEngine.AvatarIKHint) : number
            /** Sets the translative weight of an IK hint (0 = at the original animation before IK, 1 = at the hint). * @param hint The AvatarIKHint that is set.
            * @param value The translative weight.
            */
            public SetIKHintPositionWeight ($hint: UnityEngine.AvatarIKHint, $value: number) : void
            /** Sets the look at position. * @param lookAtPosition The position to lookAt.
            */
            public SetLookAtPosition ($lookAtPosition: UnityEngine.Vector3) : void
            /** Set look at weights. * @param weight (0-1) the global weight of the LookAt, multiplier for other parameters.
            * @param bodyWeight (0-1) determines how much the body is involved in the LookAt.
            * @param headWeight (0-1) determines how much the head is involved in the LookAt.
            * @param eyesWeight (0-1) determines how much the eyes are involved in the LookAt.
            * @param clampWeight (0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).
            */
            public SetLookAtWeight ($weight: number) : void
            /** Set look at weights. * @param weight (0-1) the global weight of the LookAt, multiplier for other parameters.
            * @param bodyWeight (0-1) determines how much the body is involved in the LookAt.
            * @param headWeight (0-1) determines how much the head is involved in the LookAt.
            * @param eyesWeight (0-1) determines how much the eyes are involved in the LookAt.
            * @param clampWeight (0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).
            */
            public SetLookAtWeight ($weight: number, $bodyWeight: number) : void
            /** Set look at weights. * @param weight (0-1) the global weight of the LookAt, multiplier for other parameters.
            * @param bodyWeight (0-1) determines how much the body is involved in the LookAt.
            * @param headWeight (0-1) determines how much the head is involved in the LookAt.
            * @param eyesWeight (0-1) determines how much the eyes are involved in the LookAt.
            * @param clampWeight (0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).
            */
            public SetLookAtWeight ($weight: number, $bodyWeight: number, $headWeight: number) : void
            /** Set look at weights. * @param weight (0-1) the global weight of the LookAt, multiplier for other parameters.
            * @param bodyWeight (0-1) determines how much the body is involved in the LookAt.
            * @param headWeight (0-1) determines how much the head is involved in the LookAt.
            * @param eyesWeight (0-1) determines how much the eyes are involved in the LookAt.
            * @param clampWeight (0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).
            */
            public SetLookAtWeight ($weight: number, $bodyWeight: number, $headWeight: number, $eyesWeight: number) : void
            /** Set look at weights. * @param weight (0-1) the global weight of the LookAt, multiplier for other parameters.
            * @param bodyWeight (0-1) determines how much the body is involved in the LookAt.
            * @param headWeight (0-1) determines how much the head is involved in the LookAt.
            * @param eyesWeight (0-1) determines how much the eyes are involved in the LookAt.
            * @param clampWeight (0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).
            */
            public SetLookAtWeight ($weight: number, $bodyWeight: number, $headWeight: number, $eyesWeight: number, $clampWeight: number) : void
            /** Sets local rotation of a human bone during a IK pass. * @param humanBoneId The human bone Id.
            * @param rotation The local rotation.
            */
            public SetBoneLocalRotation ($humanBoneId: UnityEngine.HumanBodyBones, $rotation: UnityEngine.Quaternion) : void
            public GetBehaviours ($fullPathHash: number, $layerIndex: number) : System.Array$1<UnityEngine.StateMachineBehaviour>
            /** Returns the layer name.
            * @param layerIndex The layer index.
            * @returns The layer name. 
            */
            public GetLayerName ($layerIndex: number) : string
            /** Returns the index of the layer with the given name.
            * @param layerName The layer name.
            * @returns The layer index. 
            */
            public GetLayerIndex ($layerName: string) : number
            /** Returns the weight of the layer at the specified index.
            * @param layerIndex The layer index.
            * @returns The layer weight. 
            */
            public GetLayerWeight ($layerIndex: number) : number
            /** Sets the weight of the layer at the given index. * @param layerIndex The layer index.
            * @param weight The new layer weight.
            */
            public SetLayerWeight ($layerIndex: number, $weight: number) : void
            /** Returns an AnimatorStateInfo with the information on the current state.
            * @param layerIndex The layer index.
            * @returns An AnimatorStateInfo with the information on the current state. 
            */
            public GetCurrentAnimatorStateInfo ($layerIndex: number) : UnityEngine.AnimatorStateInfo
            /** Returns an AnimatorStateInfo with the information on the next state.
            * @param layerIndex The layer index.
            * @returns An AnimatorStateInfo with the information on the next state. 
            */
            public GetNextAnimatorStateInfo ($layerIndex: number) : UnityEngine.AnimatorStateInfo
            /** Returns an AnimatorTransitionInfo with the informations on the current transition.
            * @param layerIndex The layer's index.
            * @returns An AnimatorTransitionInfo with the informations on the current transition. 
            */
            public GetAnimatorTransitionInfo ($layerIndex: number) : UnityEngine.AnimatorTransitionInfo
            /** Returns the number of AnimatorClipInfo in the current state.
            * @param layerIndex The layer index.
            * @returns The number of AnimatorClipInfo in the current state. 
            */
            public GetCurrentAnimatorClipInfoCount ($layerIndex: number) : number
            /** Returns the number of AnimatorClipInfo in the next state.
            * @param layerIndex The layer index.
            * @returns The number of AnimatorClipInfo in the next state. 
            */
            public GetNextAnimatorClipInfoCount ($layerIndex: number) : number
            /** Returns an array of all the AnimatorClipInfo in the current state of the given layer.
            * @param layerIndex The layer index.
            * @returns An array of all the AnimatorClipInfo in the current state. 
            */
            public GetCurrentAnimatorClipInfo ($layerIndex: number) : System.Array$1<UnityEngine.AnimatorClipInfo>
            /** Returns an array of all the AnimatorClipInfo in the next state of the given layer.
            * @param layerIndex The layer index.
            * @returns An array of all the AnimatorClipInfo in the next state. 
            */
            public GetNextAnimatorClipInfo ($layerIndex: number) : System.Array$1<UnityEngine.AnimatorClipInfo>
            public GetCurrentAnimatorClipInfo ($layerIndex: number, $clips: System.Collections.Generic.List$1<UnityEngine.AnimatorClipInfo>) : void
            public GetNextAnimatorClipInfo ($layerIndex: number, $clips: System.Collections.Generic.List$1<UnityEngine.AnimatorClipInfo>) : void
            /** Returns true if there is a transition on the given layer, false otherwise.
            * @param layerIndex The layer index.
            * @returns True if there is a transition on the given layer, false otherwise. 
            */
            public IsInTransition ($layerIndex: number) : boolean
            /** See AnimatorController.parameters. */
            public GetParameter ($index: number) : UnityEngine.AnimatorControllerParameter
            public MatchTarget ($matchPosition: UnityEngine.Vector3, $matchRotation: UnityEngine.Quaternion, $targetBodyPart: UnityEngine.AvatarTarget, $weightMask: UnityEngine.MatchTargetWeightMask, $startNormalizedTime: number) : void
            /** Automatically adjust the GameObject position and rotation. * @param matchPosition The position we want the body part to reach.
            * @param matchRotation The rotation in which we want the body part to be.
            * @param targetBodyPart The body part that is involved in the match.
            * @param weightMask Structure that contains weights for matching position and rotation.
            * @param startNormalizedTime Start time within the animation clip (0 - beginning of clip, 1 - end of clip).
            * @param targetNormalizedTime End time within the animation clip (0 - beginning of clip, 1 - end of clip), values greater than 1 can be set to trigger a match after a certain number of loops. Ex: 2.3 means at 30% of 2nd loop.
            */
            public MatchTarget ($matchPosition: UnityEngine.Vector3, $matchRotation: UnityEngine.Quaternion, $targetBodyPart: UnityEngine.AvatarTarget, $weightMask: UnityEngine.MatchTargetWeightMask, $startNormalizedTime: number, $targetNormalizedTime: number) : void
            public InterruptMatchTarget () : void
            /** Interrupts the automatic target matching. */
            public InterruptMatchTarget ($completeMatch: boolean) : void
            public CrossFadeInFixedTime ($stateName: string, $fixedTransitionDuration: number) : void
            public CrossFadeInFixedTime ($stateName: string, $fixedTransitionDuration: number, $layer: number) : void
            public CrossFadeInFixedTime ($stateName: string, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number) : void
            /** Creates a crossfade from the current state to any other state using times in seconds. * @param stateName The name of the state.
            * @param stateHashName The hash name of the state.
            * @param fixedTransitionDuration The duration of the transition (in seconds).
            * @param layer The layer where the crossfade occurs.
            * @param fixedTimeOffset The time of the state (in seconds).
            * @param normalizedTransitionTime The time of the transition (normalized).
            */
            public CrossFadeInFixedTime ($stateName: string, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number, $normalizedTransitionTime: number) : void
            public CrossFadeInFixedTime ($stateHashName: number, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number) : void
            public CrossFadeInFixedTime ($stateHashName: number, $fixedTransitionDuration: number, $layer: number) : void
            public CrossFadeInFixedTime ($stateHashName: number, $fixedTransitionDuration: number) : void
            /** Creates a crossfade from the current state to any other state using times in seconds. * @param stateName The name of the state.
            * @param stateHashName The hash name of the state.
            * @param fixedTransitionDuration The duration of the transition (in seconds).
            * @param layer The layer where the crossfade occurs.
            * @param fixedTimeOffset The time of the state (in seconds).
            * @param normalizedTransitionTime The time of the transition (normalized).
            */
            public CrossFadeInFixedTime ($stateHashName: number, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number, $normalizedTransitionTime: number) : void
            public WriteDefaultValues () : void
            public CrossFade ($stateName: string, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number) : void
            public CrossFade ($stateName: string, $normalizedTransitionDuration: number, $layer: number) : void
            public CrossFade ($stateName: string, $normalizedTransitionDuration: number) : void
            /** Creates a crossfade from the current state to any other state using normalized times. * @param stateName The name of the state.
            * @param stateHashName The hash name of the state.
            * @param normalizedTransitionDuration The duration of the transition (normalized).
            * @param layer The layer where the crossfade occurs.
            * @param normalizedTimeOffset The time of the state (normalized).
            * @param normalizedTransitionTime The time of the transition (normalized).
            */
            public CrossFade ($stateName: string, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number, $normalizedTransitionTime: number) : void
            /** Creates a crossfade from the current state to any other state using normalized times. * @param stateName The name of the state.
            * @param stateHashName The hash name of the state.
            * @param normalizedTransitionDuration The duration of the transition (normalized).
            * @param layer The layer where the crossfade occurs.
            * @param normalizedTimeOffset The time of the state (normalized).
            * @param normalizedTransitionTime The time of the transition (normalized).
            */
            public CrossFade ($stateHashName: number, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number, $normalizedTransitionTime: number) : void
            public CrossFade ($stateHashName: number, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number) : void
            public CrossFade ($stateHashName: number, $normalizedTransitionDuration: number, $layer: number) : void
            public CrossFade ($stateHashName: number, $normalizedTransitionDuration: number) : void
            public PlayInFixedTime ($stateName: string, $layer: number) : void
            public PlayInFixedTime ($stateName: string) : void
            /** Plays a state. * @param stateName The state name.
            * @param stateNameHash The state hash name. If stateNameHash is 0, it changes the current state time.
            * @param layer The layer index. If layer is -1, it plays the first state with the given state name or hash.
            * @param fixedTime The time offset (in seconds).
            */
            public PlayInFixedTime ($stateName: string, $layer: number, $fixedTime: number) : void
            /** Plays a state. * @param stateName The state name.
            * @param stateNameHash The state hash name. If stateNameHash is 0, it changes the current state time.
            * @param layer The layer index. If layer is -1, it plays the first state with the given state name or hash.
            * @param fixedTime The time offset (in seconds).
            */
            public PlayInFixedTime ($stateNameHash: number, $layer: number, $fixedTime: number) : void
            public PlayInFixedTime ($stateNameHash: number, $layer: number) : void
            public PlayInFixedTime ($stateNameHash: number) : void
            public Play ($stateName: string, $layer: number) : void
            public Play ($stateName: string) : void
            /** Plays a state. * @param stateName The state name.
            * @param stateNameHash The state hash name. If stateNameHash is 0, it changes the current state time.
            * @param layer The layer index. If layer is -1, it plays the first state with the given state name or hash.
            * @param normalizedTime The time offset between zero and one.
            */
            public Play ($stateName: string, $layer: number, $normalizedTime: number) : void
            /** Plays a state. * @param stateName The state name.
            * @param stateNameHash The state hash name. If stateNameHash is 0, it changes the current state time.
            * @param layer The layer index. If layer is -1, it plays the first state with the given state name or hash.
            * @param normalizedTime The time offset between zero and one.
            */
            public Play ($stateNameHash: number, $layer: number, $normalizedTime: number) : void
            public Play ($stateNameHash: number, $layer: number) : void
            public Play ($stateNameHash: number) : void
            /** Sets an AvatarTarget and a targetNormalizedTime for the current state. * @param targetIndex The avatar body part that is queried.
            * @param targetNormalizedTime The current state Time that is queried.
            */
            public SetTarget ($targetIndex: UnityEngine.AvatarTarget, $targetNormalizedTime: number) : void
            /** Returns Transform mapped to this human bone id. * @param humanBoneId The human bone that is queried, see enum HumanBodyBones for a list of possible values.
            */
            public GetBoneTransform ($humanBoneId: UnityEngine.HumanBodyBones) : UnityEngine.Transform
            public StartPlayback () : void
            public StopPlayback () : void
            /** Sets the animator in recording mode, and allocates a circular buffer of size frameCount. * @param frameCount The number of frames (updates) that will be recorded. If frameCount is 0, the recording will continue until the user calls StopRecording. The maximum value for frameCount is 10000.
            */
            public StartRecording ($frameCount: number) : void
            public StopRecording () : void
            /** Returns true if the state exists in this layer, false otherwise.
            * @param layerIndex The layer index.
            * @param stateID The state ID.
            * @returns True if the state exists in this layer, false otherwise. 
            */
            public HasState ($layerIndex: number, $stateID: number) : boolean
            /** Generates an parameter id from a string. * @param name The string to convert to Id.
            */
            public static StringToHash ($name: string) : number
            /** Evaluates the animator based on deltaTime. * @param deltaTime The time delta.
            */
            public Update ($deltaTime: number) : void
            public Rebind () : void
            public ApplyBuiltinRootMotion () : void
            public constructor ()
        }
        /** Information about what animation clips is played and its weight. */
        class AnimationInfo extends System.ValueType
        {
        }
        /** The update mode of the Animator. */
        enum AnimatorUpdateMode
        { Normal = 0, AnimatePhysics = 1, UnscaledTime = 2 }
        /** IK Goal. */
        enum AvatarIKGoal
        { LeftFoot = 0, RightFoot = 1, LeftHand = 2, RightHand = 3 }
        /** IK Hint. */
        enum AvatarIKHint
        { LeftKnee = 0, RightKnee = 1, LeftElbow = 2, RightElbow = 3 }
        /** Human Body Bones. */
        enum HumanBodyBones
        { Hips = 0, LeftUpperLeg = 1, RightUpperLeg = 2, LeftLowerLeg = 3, RightLowerLeg = 4, LeftFoot = 5, RightFoot = 6, Spine = 7, Chest = 8, UpperChest = 54, Neck = 9, Head = 10, LeftShoulder = 11, RightShoulder = 12, LeftUpperArm = 13, RightUpperArm = 14, LeftLowerArm = 15, RightLowerArm = 16, LeftHand = 17, RightHand = 18, LeftToes = 19, RightToes = 20, LeftEye = 21, RightEye = 22, Jaw = 23, LeftThumbProximal = 24, LeftThumbIntermediate = 25, LeftThumbDistal = 26, LeftIndexProximal = 27, LeftIndexIntermediate = 28, LeftIndexDistal = 29, LeftMiddleProximal = 30, LeftMiddleIntermediate = 31, LeftMiddleDistal = 32, LeftRingProximal = 33, LeftRingIntermediate = 34, LeftRingDistal = 35, LeftLittleProximal = 36, LeftLittleIntermediate = 37, LeftLittleDistal = 38, RightThumbProximal = 39, RightThumbIntermediate = 40, RightThumbDistal = 41, RightIndexProximal = 42, RightIndexIntermediate = 43, RightIndexDistal = 44, RightMiddleProximal = 45, RightMiddleIntermediate = 46, RightMiddleDistal = 47, RightRingProximal = 48, RightRingIntermediate = 49, RightRingDistal = 50, RightLittleProximal = 51, RightLittleIntermediate = 52, RightLittleDistal = 53, LastBone = 55 }
        /** StateMachineBehaviour is a component that can be added to a state machine state. It's the base class every script on a state derives from. */
        class StateMachineBehaviour extends UnityEngine.ScriptableObject
        {
        }
        /** A class you can derive from if you want to create objects that don't need to be attached to game objects. */
        class ScriptableObject extends UnityEngine.Object
        {
        /** Creates an instance of a scriptable object.
            * @param className The type of the ScriptableObject to create, as the name of the type.
            * @param type The type of the ScriptableObject to create, as a System.Type instance.
            * @returns The created ScriptableObject. 
            */
            public static CreateInstance ($className: string) : UnityEngine.ScriptableObject
            /** Creates an instance of a scriptable object.
            * @param className The type of the ScriptableObject to create, as the name of the type.
            * @param type The type of the ScriptableObject to create, as a System.Type instance.
            * @returns The created ScriptableObject. 
            */
            public static CreateInstance ($type: System.Type) : UnityEngine.ScriptableObject
            public constructor ()
        }
        /** Information about the current or next state. */
        class AnimatorStateInfo extends System.ValueType
        {
        }
        /** Information about the current transition. */
        class AnimatorTransitionInfo extends System.ValueType
        {
        }
        /** Information about clip being played and blended by the Animator. */
        class AnimatorClipInfo extends System.ValueType
        {
        }
        /** Used to communicate between scripting and the controller. Some parameters can be set in scripting and used by the controller, while other parameters are based on Custom Curves in Animation Clips and can be sampled using the scripting API. */
        class AnimatorControllerParameter extends System.Object
        {
        }
        /** Target. */
        enum AvatarTarget
        { Root = 0, Body = 1, LeftFoot = 2, RightFoot = 3, LeftHand = 4, RightHand = 5 }
        /** Use this struct to specify the position and rotation weight mask for Animator.MatchTarget. */
        class MatchTargetWeightMask extends System.ValueType
        {
        }
        /** Culling mode for the Animator. */
        enum AnimatorCullingMode
        { AlwaysAnimate = 0, CullUpdateTransforms = 1, CullCompletely = 2, BasedOnRenderers = 1 }
        /** The mode of the Animator's recorder. */
        enum AnimatorRecorderMode
        { Offline = 0, Playback = 1, Record = 2 }
        /** The runtime representation of the AnimatorController. Use this representation to change the Animator Controller during runtime. */
        class RuntimeAnimatorController extends UnityEngine.Object
        {
        }
        /** Avatar definition. */
        class Avatar extends UnityEngine.Object
        {
        }
        /** AssetBundles let you stream additional assets via the UnityWebRequest class and instantiate them at runtime. AssetBundles are created via BuildPipeline.BuildAssetBundle. */
        class AssetBundle extends UnityEngine.Object
        {
        /** Return true if the AssetBundle is a streamed Scene AssetBundle. */
            public get isStreamedSceneAssetBundle(): boolean;
            /** Unloads all currently loaded Asset Bundles. * @param unloadAllObjects Determines whether the current instances of objects loaded from Asset Bundles will also be unloaded.
            */
            public static UnloadAllAssetBundles ($unloadAllObjects: boolean) : void
            public static GetAllLoadedAssetBundles () : System.Collections.Generic.IEnumerable$1<UnityEngine.AssetBundle>
            public static LoadFromFileAsync ($path: string) : UnityEngine.AssetBundleCreateRequest
            public static LoadFromFileAsync ($path: string, $crc: number) : UnityEngine.AssetBundleCreateRequest
            /** Asynchronously loads an AssetBundle from a file on disk.
            * @param path Path of the file on disk.
            * @param crc An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.
            * @param offset An optional byte offset. This value specifies where to start reading the AssetBundle from.
            * @returns Asynchronous create request for an AssetBundle. Use AssetBundleCreateRequest.assetBundle property to get an AssetBundle once it is loaded. 
            */
            public static LoadFromFileAsync ($path: string, $crc: number, $offset: bigint) : UnityEngine.AssetBundleCreateRequest
            public static LoadFromFile ($path: string) : UnityEngine.AssetBundle
            public static LoadFromFile ($path: string, $crc: number) : UnityEngine.AssetBundle
            /** Synchronously loads an AssetBundle from a file on disk.
            * @param path Path of the file on disk.
            * @param crc An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.
            * @param offset An optional byte offset. This value specifies where to start reading the AssetBundle from.
            * @returns Loaded AssetBundle object or null if failed. 
            */
            public static LoadFromFile ($path: string, $crc: number, $offset: bigint) : UnityEngine.AssetBundle
            public static LoadFromMemoryAsync ($binary: System.Array$1<number>) : UnityEngine.AssetBundleCreateRequest
            /** Asynchronously create an AssetBundle from a memory region.
            * @param binary Array of bytes with the AssetBundle data.
            * @param crc An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.
            * @returns Asynchronous create request for an AssetBundle. Use AssetBundleCreateRequest.assetBundle property to get an AssetBundle once it is loaded. 
            */
            public static LoadFromMemoryAsync ($binary: System.Array$1<number>, $crc: number) : UnityEngine.AssetBundleCreateRequest
            public static LoadFromMemory ($binary: System.Array$1<number>) : UnityEngine.AssetBundle
            /** Synchronously create an AssetBundle from a memory region.
            * @param binary Array of bytes with the AssetBundle data.
            * @param crc An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.
            * @returns Loaded AssetBundle object or null if failed. 
            */
            public static LoadFromMemory ($binary: System.Array$1<number>, $crc: number) : UnityEngine.AssetBundle
            /** Asynchronously loads an AssetBundle from a managed Stream.
            * @param stream The managed Stream object. Unity calls Read(), Seek() and the Length property on this object to load the AssetBundle data.
            * @param crc An optional CRC-32 checksum of the uncompressed content.
            * @param managedReadBufferSize You can use this to override the size of the read buffer Unity uses while loading data. The default size is 32KB.
            * @returns Asynchronous create request for an AssetBundle. Use AssetBundleCreateRequest.assetBundle property to get an AssetBundle once it is loaded. 
            */
            public static LoadFromStreamAsync ($stream: System.IO.Stream, $crc: number, $managedReadBufferSize: number) : UnityEngine.AssetBundleCreateRequest
            public static LoadFromStreamAsync ($stream: System.IO.Stream, $crc: number) : UnityEngine.AssetBundleCreateRequest
            public static LoadFromStreamAsync ($stream: System.IO.Stream) : UnityEngine.AssetBundleCreateRequest
            /** Synchronously loads an AssetBundle from a managed Stream.
            * @param stream The managed Stream object. Unity calls Read(), Seek() and the Length property on this object to load the AssetBundle data.
            * @param crc An optional CRC-32 checksum of the uncompressed content.
            * @param managedReadBufferSize You can use this to override the size of the read buffer Unity uses while loading data. The default size is 32KB.
            * @returns The loaded AssetBundle object or null when the object fails to load. 
            */
            public static LoadFromStream ($stream: System.IO.Stream, $crc: number, $managedReadBufferSize: number) : UnityEngine.AssetBundle
            public static LoadFromStream ($stream: System.IO.Stream, $crc: number) : UnityEngine.AssetBundle
            public static LoadFromStream ($stream: System.IO.Stream) : UnityEngine.AssetBundle
            /** Provide decrypt key for protected assetbundle. */
            public static SetAssetBundleDecryptKey ($password: string) : void
            /** Check if an AssetBundle contains a specific object. */
            public Contains ($name: string) : boolean
            /** Loads asset with name of type T from the bundle. */
            public LoadAsset ($name: string) : UnityEngine.Object
            /** Loads asset with name of a given type from the bundle. */
            public LoadAsset ($name: string, $type: System.Type) : UnityEngine.Object
            /** Asynchronously loads asset with name of a given T from the bundle. */
            public LoadAssetAsync ($name: string) : UnityEngine.AssetBundleRequest
            /** Asynchronously loads asset with name of a given type from the bundle. */
            public LoadAssetAsync ($name: string, $type: System.Type) : UnityEngine.AssetBundleRequest
            /** Loads asset and sub assets with name of type T from the bundle. */
            public LoadAssetWithSubAssets ($name: string) : System.Array$1<UnityEngine.Object>
            /** Loads asset and sub assets with name of a given type from the bundle. */
            public LoadAssetWithSubAssets ($name: string, $type: System.Type) : System.Array$1<UnityEngine.Object>
            /** Loads asset with sub assets with name of type T from the bundle asynchronously. */
            public LoadAssetWithSubAssetsAsync ($name: string) : UnityEngine.AssetBundleRequest
            /** Loads asset with sub assets with name of a given type from the bundle asynchronously. */
            public LoadAssetWithSubAssetsAsync ($name: string, $type: System.Type) : UnityEngine.AssetBundleRequest
            public LoadAllAssets () : System.Array$1<UnityEngine.Object>
            /** Loads all assets contained in the asset bundle that inherit from type. */
            public LoadAllAssets ($type: System.Type) : System.Array$1<UnityEngine.Object>
            public LoadAllAssetsAsync () : UnityEngine.AssetBundleRequest
            /** Loads all assets contained in the asset bundle that inherit from type asynchronously. */
            public LoadAllAssetsAsync ($type: System.Type) : UnityEngine.AssetBundleRequest
            /** Unloads assets in the bundle. */
            public Unload ($unloadAllLoadedObjects: boolean) : void
            public GetAllAssetNames () : System.Array$1<string>
            public GetAllScenePaths () : System.Array$1<string>
            /** Asynchronously recompress a downloaded/stored AssetBundle from one BuildCompression to another. * @param inputPath Path to the AssetBundle to recompress.
            * @param outputPath Path to the recompressed AssetBundle to be generated. Can be the same as inputPath.
            * @param method The compression method, level and blocksize to use during recompression. Only some BuildCompression types are supported (see note).
            * @param expectedCRC CRC of the AssetBundle to test against. Testing this requires additional file reading and computation. Pass in 0 to skip this check. Unity does not compute a CRC when the source and destination BuildCompression are the same, so no CRC verification takes place (see note).
            * @param priority The priority at which the recompression operation should run. This sets thread priority during the operation and does not effect the order in which operations are performed. Recompression operations run on a background worker thread.
            */
            public static RecompressAssetBundleAsync ($inputPath: string, $outputPath: string, $method: UnityEngine.BuildCompression, $expectedCRC?: number, $priority?: UnityEngine.ThreadPriority) : UnityEngine.AssetBundleRecompressOperation
        }
        /** Asynchronous create request for an AssetBundle. */
        class AssetBundleCreateRequest extends UnityEngine.AsyncOperation
        {
        /** Asset object being loaded (Read Only). */
            public get assetBundle(): UnityEngine.AssetBundle;
            public constructor ()
        }
        /** Asynchronous operation coroutine. */
        class AsyncOperation extends UnityEngine.YieldInstruction
        {
        /** Has the operation finished? (Read Only) */
            public get isDone(): boolean;
            /** What's the operation's progress. (Read Only) */
            public get progress(): number;
            /** Priority lets you tweak in which order async operation calls will be performed. */
            public get priority(): number;
            public set priority(value: number);
            /** Allow Scenes to be activated as soon as it is ready. */
            public get allowSceneActivation(): boolean;
            public set allowSceneActivation(value: boolean);
            public add_completed ($value: System.Action$1<UnityEngine.AsyncOperation>) : void
            public remove_completed ($value: System.Action$1<UnityEngine.AsyncOperation>) : void
            public constructor ()
        }
        /** Base class for all yield instructions. */
        class YieldInstruction extends System.Object
        {
        }
        /** Asynchronous load request from an AssetBundle. */
        class AssetBundleRequest extends UnityEngine.AsyncOperation
        {
        /** Asset object being loaded (Read Only). */
            public get asset(): UnityEngine.Object;
            /** Asset objects with sub assets being loaded. (Read Only) */
            public get allAssets(): System.Array$1<UnityEngine.Object>;
            public constructor ()
        }
        /** Asynchronous AssetBundle recompression from one compression method and level to another. */
        class AssetBundleRecompressOperation extends UnityEngine.AsyncOperation
        {
        }
        /** Contains information about compression methods, compression levels and block sizes that are supported by Asset Bundle compression at build time and recompression at runtime. */
        class BuildCompression extends System.ValueType
        {
        }
        /** Priority of a thread. */
        enum ThreadPriority
        { Low = 0, BelowNormal = 1, Normal = 2, High = 4 }
        /** A container for audio data. */
        class AudioClip extends UnityEngine.Object
        {
        /** The length of the audio clip in seconds. (Read Only) */
            public get length(): number;
            /** The length of the audio clip in samples. (Read Only) */
            public get samples(): number;
            /** The number of channels in the audio clip. (Read Only) */
            public get channels(): number;
            /** The sample frequency of the clip in Hertz. (Read Only) */
            public get frequency(): number;
            /** The load type of the clip (read-only). */
            public get loadType(): UnityEngine.AudioClipLoadType;
            /** Preloads audio data of the clip when the clip asset is loaded. When this flag is off, scripts have to call AudioClip.LoadAudioData() to load the data before the clip can be played. Properties like length, channels and format are available before the audio data has been loaded. */
            public get preloadAudioData(): boolean;
            /** Returns true if this audio clip is ambisonic (read-only). */
            public get ambisonic(): boolean;
            /** Returns the current load state of the audio data associated with an AudioClip. */
            public get loadState(): UnityEngine.AudioDataLoadState;
            /** Corresponding to the "Load In Background" flag in the inspector, when this flag is set, the loading will happen delayed without blocking the main thread. */
            public get loadInBackground(): boolean;
            public LoadAudioData () : boolean
            public UnloadAudioData () : boolean
            /** Fills an array with sample data from the clip. */
            public GetData ($data: System.Array$1<number>, $offsetSamples: number) : boolean
            /** Set sample data in a clip. */
            public SetData ($data: System.Array$1<number>, $offsetSamples: number) : boolean
            /** Creates a user AudioClip with a name and with the given length in samples, channels and frequency.
            * @param name Name of clip.
            * @param lengthSamples Number of sample frames.
            * @param channels Number of channels per frame.
            * @param frequency Sample frequency of clip.
            * @param _3D Audio clip is played back in 3D.
            * @param stream True if clip is streamed, that is if the pcmreadercallback generates data on the fly.
            * @param pcmreadercallback This callback is invoked to generate a block of sample data. Non-streamed clips call this only once at creation time while streamed clips call this continuously.
            * @param pcmsetpositioncallback This callback is invoked whenever the clip loops or changes playback position.
            * @returns A reference to the created AudioClip. 
            */
            public static Create ($name: string, $lengthSamples: number, $channels: number, $frequency: number, $stream: boolean) : UnityEngine.AudioClip
            public static Create ($name: string, $lengthSamples: number, $channels: number, $frequency: number, $stream: boolean, $pcmreadercallback: UnityEngine.AudioClip.PCMReaderCallback) : UnityEngine.AudioClip
            public static Create ($name: string, $lengthSamples: number, $channels: number, $frequency: number, $stream: boolean, $pcmreadercallback: UnityEngine.AudioClip.PCMReaderCallback, $pcmsetpositioncallback: UnityEngine.AudioClip.PCMSetPositionCallback) : UnityEngine.AudioClip
        }
        /** Determines how the audio clip is loaded in. */
        enum AudioClipLoadType
        { DecompressOnLoad = 0, CompressedInMemory = 1, Streaming = 2 }
        /** Value describing the current load state of the audio data associated with an AudioClip. */
        enum AudioDataLoadState
        { Unloaded = 0, Loading = 1, Loaded = 2, Failed = 3 }
        /** Representation of a listener in 3D space. */
        class AudioListener extends UnityEngine.AudioBehaviour
        {
        /** Controls the game sound volume (0.0 to 1.0). */
            public static get volume(): number;
            public static set volume(value: number);
            /** The paused state of the audio system. */
            public static get pause(): boolean;
            public static set pause(value: boolean);
            /** This lets you set whether the Audio Listener should be updated in the fixed or dynamic update. */
            public get velocityUpdateMode(): UnityEngine.AudioVelocityUpdateMode;
            public set velocityUpdateMode(value: UnityEngine.AudioVelocityUpdateMode);
            /** Provides a block of the listener (master)'s output data. * @param samples The array to populate with audio samples. Its length must be a power of 2.
            * @param channel The channel to sample from.
            */
            public static GetOutputData ($samples: System.Array$1<number>, $channel: number) : void
            /** Provides a block of the listener (master)'s spectrum data. * @param samples The array to populate with audio samples. Its length must be a power of 2.
            * @param channel The channel to sample from.
            * @param window The FFTWindow type to use when sampling.
            */
            public static GetSpectrumData ($samples: System.Array$1<number>, $channel: number, $window: UnityEngine.FFTWindow) : void
            public constructor ()
        }
        class AudioBehaviour extends UnityEngine.Behaviour
        {
        }
        /** Describes when an AudioSource or AudioListener is updated. */
        enum AudioVelocityUpdateMode
        { Auto = 0, Fixed = 1, Dynamic = 2 }
        /** Spectrum analysis windowing types. */
        enum FFTWindow
        { Rectangular = 0, Triangle = 1, Hamming = 2, Hanning = 3, Blackman = 4, BlackmanHarris = 5 }
        /** A representation of audio sources in 3D. */
        class AudioSource extends UnityEngine.AudioBehaviour
        {
        /** The volume of the audio source (0.0 to 1.0). */
            public get volume(): number;
            public set volume(value: number);
            /** The pitch of the audio source. */
            public get pitch(): number;
            public set pitch(value: number);
            /** Playback position in seconds. */
            public get time(): number;
            public set time(value: number);
            /** Playback position in PCM samples. */
            public get timeSamples(): number;
            public set timeSamples(value: number);
            /** The default AudioClip to play. */
            public get clip(): UnityEngine.AudioClip;
            public set clip(value: UnityEngine.AudioClip);
            /** The target group to which the AudioSource should route its signal. */
            public get outputAudioMixerGroup(): UnityEngine.Audio.AudioMixerGroup;
            public set outputAudioMixerGroup(value: UnityEngine.Audio.AudioMixerGroup);
            /** Is the clip playing right now (Read Only)? */
            public get isPlaying(): boolean;
            /** True if all sounds played by the AudioSource (main sound started by Play() or playOnAwake as well as one-shots) are culled by the audio system. */
            public get isVirtual(): boolean;
            /** Is the audio clip looping? */
            public get loop(): boolean;
            public set loop(value: boolean);
            /** This makes the audio source not take into account the volume of the audio listener. */
            public get ignoreListenerVolume(): boolean;
            public set ignoreListenerVolume(value: boolean);
            /** If set to true, the audio source will automatically start playing on awake. */
            public get playOnAwake(): boolean;
            public set playOnAwake(value: boolean);
            /** Allows AudioSource to play even though AudioListener.pause is set to true. This is useful for the menu element sounds or background music in pause menus. */
            public get ignoreListenerPause(): boolean;
            public set ignoreListenerPause(value: boolean);
            /** Whether the Audio Source should be updated in the fixed or dynamic update. */
            public get velocityUpdateMode(): UnityEngine.AudioVelocityUpdateMode;
            public set velocityUpdateMode(value: UnityEngine.AudioVelocityUpdateMode);
            /** Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo. */
            public get panStereo(): number;
            public set panStereo(value: number);
            /** Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D. */
            public get spatialBlend(): number;
            public set spatialBlend(value: number);
            /** Enables or disables spatialization. */
            public get spatialize(): boolean;
            public set spatialize(value: boolean);
            /** Determines if the spatializer effect is inserted before or after the effect filters. */
            public get spatializePostEffects(): boolean;
            public set spatializePostEffects(value: boolean);
            /** The amount by which the signal from the AudioSource will be mixed into the global reverb associated with the Reverb Zones. */
            public get reverbZoneMix(): number;
            public set reverbZoneMix(value: number);
            /** Bypass effects (Applied from filter components or global listener filters). */
            public get bypassEffects(): boolean;
            public set bypassEffects(value: boolean);
            /** When set global effects on the AudioListener will not be applied to the audio signal generated by the AudioSource. Does not apply if the AudioSource is playing into a mixer group. */
            public get bypassListenerEffects(): boolean;
            public set bypassListenerEffects(value: boolean);
            /** When set doesn't route the signal from an AudioSource into the global reverb associated with reverb zones. */
            public get bypassReverbZones(): boolean;
            public set bypassReverbZones(value: boolean);
            /** Sets the Doppler scale for this AudioSource. */
            public get dopplerLevel(): number;
            public set dopplerLevel(value: number);
            /** Sets the spread angle (in degrees) of a 3d stereo or multichannel sound in speaker space. */
            public get spread(): number;
            public set spread(value: number);
            /** Sets the priority of the AudioSource. */
            public get priority(): number;
            public set priority(value: number);
            /** Un- / Mutes the AudioSource. Mute sets the volume=0, Un-Mute restore the original volume. */
            public get mute(): boolean;
            public set mute(value: boolean);
            /** Within the Min distance the AudioSource will cease to grow louder in volume. */
            public get minDistance(): number;
            public set minDistance(value: number);
            /** (Logarithmic rolloff) MaxDistance is the distance a sound stops attenuating at. */
            public get maxDistance(): number;
            public set maxDistance(value: number);
            /** Sets/Gets how the AudioSource attenuates over distance. */
            public get rolloffMode(): UnityEngine.AudioRolloffMode;
            public set rolloffMode(value: UnityEngine.AudioRolloffMode);
            /** Plays the clip. * @param delay Deprecated. Delay in number of samples, assuming a 44100Hz sample rate (meaning that Play(44100) will delay the playing by exactly 1 sec).
            */
            public Play ($delay: bigint) : void
            /** Plays the clip. * @param delay Deprecated. Delay in number of samples, assuming a 44100Hz sample rate (meaning that Play(44100) will delay the playing by exactly 1 sec).
            */
            public Play () : void
            /** Plays the clip with a delay specified in seconds. Users are advised to use this function instead of the old Play(delay) function that took a delay specified in samples relative to a reference rate of 44.1 kHz as an argument. * @param delay Delay time specified in seconds.
            */
            public PlayDelayed ($delay: number) : void
            /** Plays the clip at a specific time on the absolute time-line that AudioSettings.dspTime reads from. * @param time Time in seconds on the absolute time-line that AudioSettings.dspTime refers to for when the sound should start playing.
            */
            public PlayScheduled ($time: number) : void
            /** Changes the time at which a sound that has already been scheduled to play will start. * @param time Time in seconds.
            */
            public SetScheduledStartTime ($time: number) : void
            /** Changes the time at which a sound that has already been scheduled to play will end. Notice that depending on the timing not all rescheduling requests can be fulfilled. * @param time Time in seconds.
            */
            public SetScheduledEndTime ($time: number) : void
            public Stop () : void
            public Pause () : void
            public UnPause () : void
            /** Plays an AudioClip, and scales the AudioSource volume by volumeScale. * @param clip The clip being played.
            * @param volumeScale The scale of the volume (0-1).
            */
            public PlayOneShot ($clip: UnityEngine.AudioClip) : void
            /** Plays an AudioClip, and scales the AudioSource volume by volumeScale. * @param clip The clip being played.
            * @param volumeScale The scale of the volume (0-1).
            */
            public PlayOneShot ($clip: UnityEngine.AudioClip, $volumeScale: number) : void
            /** Plays an AudioClip at a given position in world space. * @param clip Audio data to play.
            * @param position Position in world space from which sound originates.
            * @param volume Playback volume.
            */
            public static PlayClipAtPoint ($clip: UnityEngine.AudioClip, $position: UnityEngine.Vector3) : void
            /** Plays an AudioClip at a given position in world space. * @param clip Audio data to play.
            * @param position Position in world space from which sound originates.
            * @param volume Playback volume.
            */
            public static PlayClipAtPoint ($clip: UnityEngine.AudioClip, $position: UnityEngine.Vector3, $volume: number) : void
            /** Set the custom curve for the given AudioSourceCurveType. * @param type The curve type that should be set.
            * @param curve The curve that should be applied to the given curve type.
            */
            public SetCustomCurve ($type: UnityEngine.AudioSourceCurveType, $curve: UnityEngine.AnimationCurve) : void
            /** Get the current custom curve for the given AudioSourceCurveType.
            * @param type The curve type to get.
            * @returns The custom AnimationCurve corresponding to the given curve type. 
            */
            public GetCustomCurve ($type: UnityEngine.AudioSourceCurveType) : UnityEngine.AnimationCurve
            /** Provides a block of the currently playing source's output data. * @param samples The array to populate with audio samples. Its length must be a power of 2.
            * @param channel The channel to sample from.
            */
            public GetOutputData ($samples: System.Array$1<number>, $channel: number) : void
            /** Provides a block of the currently playing audio source's spectrum data. * @param samples The array to populate with audio samples. Its length must be a power of 2.
            * @param channel The channel to sample from.
            * @param window The FFTWindow type to use when sampling.
            */
            public GetSpectrumData ($samples: System.Array$1<number>, $channel: number, $window: UnityEngine.FFTWindow) : void
            /** Sets a user-defined parameter of a custom spatializer effect that is attached to an AudioSource.
            * @param index Zero-based index of user-defined parameter to be set.
            * @param value New value of the user-defined parameter.
            * @returns True, if the parameter could be set. 
            */
            public SetSpatializerFloat ($index: number, $value: number) : boolean
            /** Reads a user-defined parameter of a custom spatializer effect that is attached to an AudioSource.
            * @param index Zero-based index of user-defined parameter to be read.
            * @param value Return value of the user-defined parameter that is read.
            * @returns True, if the parameter could be read. 
            */
            public GetSpatializerFloat ($index: number, $value: $Ref<number>) : boolean
            /** Sets a user-defined parameter of a custom ambisonic decoder effect that is attached to an AudioSource.
            * @param index Zero-based index of user-defined parameter to be set.
            * @param value New value of the user-defined parameter.
            * @returns True, if the parameter could be set. 
            */
            public SetAmbisonicDecoderFloat ($index: number, $value: number) : boolean
            /** Reads a user-defined parameter of a custom ambisonic decoder effect that is attached to an AudioSource.
            * @param index Zero-based index of user-defined parameter to be read.
            * @param value Return value of the user-defined parameter that is read.
            * @returns True, if the parameter could be read. 
            */
            public GetAmbisonicDecoderFloat ($index: number, $value: $Ref<number>) : boolean
            public constructor ()
        }
        /** This defines the curve type of the different custom curves that can be queried and set within the AudioSource. */
        enum AudioSourceCurveType
        { CustomRolloff = 0, SpatialBlend = 1, ReverbZoneMix = 2, Spread = 3 }
        /** Store a collection of Keyframes that can be evaluated over time. */
        class AnimationCurve extends System.Object implements System.IEquatable$1<UnityEngine.AnimationCurve>
        {
        }
        /** Rolloff modes that a 3D sound can have in an audio source. */
        enum AudioRolloffMode
        { Logarithmic = 0, Linear = 1, Custom = 2 }
        /** Base class for texture handling. Contains functionality that is common to both Texture2D and RenderTexture classes. */
        class Texture extends UnityEngine.Object
        {
            public static get masterTextureLimit(): number;
            public static set masterTextureLimit(value: number);
            public static get anisotropicFiltering(): UnityEngine.AnisotropicFiltering;
            public static set anisotropicFiltering(value: UnityEngine.AnisotropicFiltering);
            /** Width of the texture in pixels. (Read Only) */
            public get width(): number;
            public set width(value: number);
            /** Height of the texture in pixels. (Read Only) */
            public get height(): number;
            public set height(value: number);
            /** Dimensionality (type) of the texture (Read Only). */
            public get dimension(): UnityEngine.Rendering.TextureDimension;
            public set dimension(value: UnityEngine.Rendering.TextureDimension);
            /** Returns true if the Read/Write Enabled checkbox was checked when the texture was imported; otherwise returns false. For a dynamic Texture created from script, always returns true. For additional information, see TextureImporter.isReadable. */
            public get isReadable(): boolean;
            /** Texture coordinate wrapping mode. */
            public get wrapMode(): UnityEngine.TextureWrapMode;
            public set wrapMode(value: UnityEngine.TextureWrapMode);
            /** Texture U coordinate wrapping mode. */
            public get wrapModeU(): UnityEngine.TextureWrapMode;
            public set wrapModeU(value: UnityEngine.TextureWrapMode);
            /** Texture V coordinate wrapping mode. */
            public get wrapModeV(): UnityEngine.TextureWrapMode;
            public set wrapModeV(value: UnityEngine.TextureWrapMode);
            /** Texture W coordinate wrapping mode for Texture3D. */
            public get wrapModeW(): UnityEngine.TextureWrapMode;
            public set wrapModeW(value: UnityEngine.TextureWrapMode);
            /** Filtering mode of the texture. */
            public get filterMode(): UnityEngine.FilterMode;
            public set filterMode(value: UnityEngine.FilterMode);
            /** Anisotropic filtering level of the texture. */
            public get anisoLevel(): number;
            public set anisoLevel(value: number);
            /** Mip map bias of the texture. */
            public get mipMapBias(): number;
            public set mipMapBias(value: number);
            public get texelSize(): UnityEngine.Vector2;
            /** This counter is incremented when the texture is updated. */
            public get updateCount(): number;
            /** The hash value of the Texture. */
            public get imageContentsHash(): UnityEngine.Hash128;
            public set imageContentsHash(value: UnityEngine.Hash128);
            /** The total amount of memory that would be used by all textures at mipmap level 0. */
            public static get totalTextureMemory(): bigint;
            /** This amount of texture memory would be used before the texture streaming budget is applied. */
            public static get desiredTextureMemory(): bigint;
            /** The amount of memory used by textures after the mipmap streaming and budget are applied and loading is complete. */
            public static get targetTextureMemory(): bigint;
            /** The amount of memory currently being used by textures. */
            public static get currentTextureMemory(): bigint;
            /** Total amount of memory being used by non-streaming textures. */
            public static get nonStreamingTextureMemory(): bigint;
            /** How many times has a texture been uploaded due to texture mipmap streaming. */
            public static get streamingMipmapUploadCount(): bigint;
            /** Number of renderers registered with the texture streaming system. */
            public static get streamingRendererCount(): bigint;
            /** Number of streaming textures. */
            public static get streamingTextureCount(): bigint;
            /** Number of non-streaming textures. */
            public static get nonStreamingTextureCount(): bigint;
            /** Number of streaming textures with outstanding mipmaps to be loaded. */
            public static get streamingTexturePendingLoadCount(): bigint;
            /** Number of streaming textures with mipmaps currently loading. */
            public static get streamingTextureLoadingCount(): bigint;
            /** Force streaming textures to load all mipmap levels. */
            public static get streamingTextureForceLoadAll(): boolean;
            public static set streamingTextureForceLoadAll(value: boolean);
            /** Force the streaming texture system to discard all unused mipmaps immediately, rather than caching them until the texture memory budget is exceeded. */
            public static get streamingTextureDiscardUnusedMips(): boolean;
            public static set streamingTextureDiscardUnusedMips(value: boolean);
            /** Sets Anisotropic limits. */
            public static SetGlobalAnisotropicFilteringLimits ($forcedMin: number, $globalMax: number) : void
            public GetNativeTexturePtr () : System.IntPtr
            public IncrementUpdateCount () : void
            public static SetStreamingTextureMaterialDebugProperties () : void
        }
        /** Anisotropic filtering mode. */
        enum AnisotropicFiltering
        { Disable = 0, Enable = 1, ForceEnable = 2 }
        /** Wrap mode for textures. */
        enum TextureWrapMode
        { Repeat = 0, Clamp = 1, Mirror = 2, MirrorOnce = 3 }
        /** Filtering mode for textures. Corresponds to the settings in a. */
        enum FilterMode
        { Point = 0, Bilinear = 1, Trilinear = 2 }
        /** Representation of 2D vectors and points. */
        class Vector2 extends System.ValueType implements System.IEquatable$1<UnityEngine.Vector2>
        {
        /** X component of the vector. */
            public x : number/** Y component of the vector. */
            public y : number
            public static kEpsilon : number
            public static kEpsilonNormalSqrt : number/** Returns this vector with a magnitude of 1 (Read Only). */
            public get normalized(): UnityEngine.Vector2;
            /** Returns the length of this vector (Read Only). */
            public get magnitude(): number;
            /** Returns the squared length of this vector (Read Only). */
            public get sqrMagnitude(): number;
            /** Shorthand for writing Vector2(0, 0). */
            public static get zero(): UnityEngine.Vector2;
            /** Shorthand for writing Vector2(1, 1). */
            public static get one(): UnityEngine.Vector2;
            /** Shorthand for writing Vector2(0, 1). */
            public static get up(): UnityEngine.Vector2;
            /** Shorthand for writing Vector2(0, -1). */
            public static get down(): UnityEngine.Vector2;
            /** Shorthand for writing Vector2(-1, 0). */
            public static get left(): UnityEngine.Vector2;
            /** Shorthand for writing Vector2(1, 0). */
            public static get right(): UnityEngine.Vector2;
            /** Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity). */
            public static get positiveInfinity(): UnityEngine.Vector2;
            /** Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity). */
            public static get negativeInfinity(): UnityEngine.Vector2;
            public get_Item ($index: number) : number
            public set_Item ($index: number, $value: number) : void
            /** Set x and y components of an existing Vector2. */
            public Set ($newX: number, $newY: number) : void
            /** Linearly interpolates between vectors a and b by t. */
            public static Lerp ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2, $t: number) : UnityEngine.Vector2
            /** Linearly interpolates between vectors a and b by t. */
            public static LerpUnclamped ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2, $t: number) : UnityEngine.Vector2
            /** Moves a point current towards target. */
            public static MoveTowards ($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $maxDistanceDelta: number) : UnityEngine.Vector2
            /** Multiplies two vectors component-wise. */
            public static Scale ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2) : UnityEngine.Vector2
            /** Multiplies every component of this vector by the same component of scale. */
            public Scale ($scale: UnityEngine.Vector2) : void
            public Normalize () : void
            public ToString () : string
            /** Returns a nicely formatted string for this vector. */
            public ToString ($format: string) : string
            /** Returns true if the given vector is exactly equal to this vector. */
            public Equals ($other: any) : boolean
            public Equals ($other: UnityEngine.Vector2) : boolean
            /** Reflects a vector off the vector defined by a normal. */
            public static Reflect ($inDirection: UnityEngine.Vector2, $inNormal: UnityEngine.Vector2) : UnityEngine.Vector2
            /** Returns the 2D vector perpendicular to this 2D vector. The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.
            * @param inDirection The input direction.
            * @returns The perpendicular direction. 
            */
            public static Perpendicular ($inDirection: UnityEngine.Vector2) : UnityEngine.Vector2
            /** Dot Product of two vectors. */
            public static Dot ($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2) : number
            /** Returns the unsigned angle in degrees between from and to. * @param from The vector from which the angular difference is measured.
            * @param to The vector to which the angular difference is measured.
            */
            public static Angle ($from: UnityEngine.Vector2, $to: UnityEngine.Vector2) : number
            /** Returns the signed angle in degrees between from and to. * @param from The vector from which the angular difference is measured.
            * @param to The vector to which the angular difference is measured.
            */
            public static SignedAngle ($from: UnityEngine.Vector2, $to: UnityEngine.Vector2) : number
            /** Returns the distance between a and b. */
            public static Distance ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2) : number
            /** Returns a copy of vector with its magnitude clamped to maxLength. */
            public static ClampMagnitude ($vector: UnityEngine.Vector2, $maxLength: number) : UnityEngine.Vector2
            public static SqrMagnitude ($a: UnityEngine.Vector2) : number
            public SqrMagnitude () : number
            /** Returns a vector that is made from the smallest components of two vectors. */
            public static Min ($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2) : UnityEngine.Vector2
            /** Returns a vector that is made from the largest components of two vectors. */
            public static Max ($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2) : UnityEngine.Vector2
            /** Gradually changes a vector towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $currentVelocity: $Ref<UnityEngine.Vector2>, $smoothTime: number, $maxSpeed: number) : UnityEngine.Vector2
            /** Gradually changes a vector towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $currentVelocity: $Ref<UnityEngine.Vector2>, $smoothTime: number) : UnityEngine.Vector2
            /** Gradually changes a vector towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $currentVelocity: $Ref<UnityEngine.Vector2>, $smoothTime: number, $maxSpeed: number, $deltaTime: number) : UnityEngine.Vector2
            public static op_Addition ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2) : UnityEngine.Vector2
            public static op_Subtraction ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2) : UnityEngine.Vector2
            public static op_Multiply ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2) : UnityEngine.Vector2
            public static op_Division ($a: UnityEngine.Vector2, $b: UnityEngine.Vector2) : UnityEngine.Vector2
            public static op_UnaryNegation ($a: UnityEngine.Vector2) : UnityEngine.Vector2
            public static op_Multiply ($a: UnityEngine.Vector2, $d: number) : UnityEngine.Vector2
            public static op_Multiply ($d: number, $a: UnityEngine.Vector2) : UnityEngine.Vector2
            public static op_Division ($a: UnityEngine.Vector2, $d: number) : UnityEngine.Vector2
            public static op_Equality ($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2) : boolean
            public static op_Inequality ($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2) : boolean
            public static op_Implicit ($v: UnityEngine.Vector3) : UnityEngine.Vector2
            public static op_Implicit ($v: UnityEngine.Vector2) : UnityEngine.Vector3
            public constructor ($x: number, $y: number)
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        /** Represent the hash value. */
        class Hash128 extends System.ValueType implements System.IComparable, System.IComparable$1<UnityEngine.Hash128>, System.IEquatable$1<UnityEngine.Hash128>
        {
        }
        /** Representation of RGBA colors. */
        class Color extends System.ValueType implements System.IEquatable$1<UnityEngine.Color>
        {
        /** Red component of the color. */
            public r : number/** Green component of the color. */
            public g : number/** Blue component of the color. */
            public b : number/** Alpha component of the color (0 is transparent, 1 is opaque). */
            public a : number/** Solid red. RGBA is (1, 0, 0, 1). */
            public static get red(): UnityEngine.Color;
            /** Solid green. RGBA is (0, 1, 0, 1). */
            public static get green(): UnityEngine.Color;
            /** Solid blue. RGBA is (0, 0, 1, 1). */
            public static get blue(): UnityEngine.Color;
            /** Solid white. RGBA is (1, 1, 1, 1). */
            public static get white(): UnityEngine.Color;
            /** Solid black. RGBA is (0, 0, 0, 1). */
            public static get black(): UnityEngine.Color;
            /** Yellow. RGBA is (1, 0.92, 0.016, 1), but the color is nice to look at! */
            public static get yellow(): UnityEngine.Color;
            /** Cyan. RGBA is (0, 1, 1, 1). */
            public static get cyan(): UnityEngine.Color;
            /** Magenta. RGBA is (1, 0, 1, 1). */
            public static get magenta(): UnityEngine.Color;
            /** Gray. RGBA is (0.5, 0.5, 0.5, 1). */
            public static get gray(): UnityEngine.Color;
            /** English spelling for gray. RGBA is the same (0.5, 0.5, 0.5, 1). */
            public static get grey(): UnityEngine.Color;
            /** Completely transparent. RGBA is (0, 0, 0, 0). */
            public static get clear(): UnityEngine.Color;
            /** The grayscale value of the color. (Read Only) */
            public get grayscale(): number;
            /** A linear value of an sRGB color. */
            public get linear(): UnityEngine.Color;
            /** A version of the color that has had the gamma curve applied. */
            public get gamma(): UnityEngine.Color;
            /** Returns the maximum color component value: Max(r,g,b). */
            public get maxColorComponent(): number;
            public ToString () : string
            /** Returns a nicely formatted string of this color. */
            public ToString ($format: string) : string
            public Equals ($other: any) : boolean
            public Equals ($other: UnityEngine.Color) : boolean
            public static op_Addition ($a: UnityEngine.Color, $b: UnityEngine.Color) : UnityEngine.Color
            public static op_Subtraction ($a: UnityEngine.Color, $b: UnityEngine.Color) : UnityEngine.Color
            public static op_Multiply ($a: UnityEngine.Color, $b: UnityEngine.Color) : UnityEngine.Color
            public static op_Multiply ($a: UnityEngine.Color, $b: number) : UnityEngine.Color
            public static op_Multiply ($b: number, $a: UnityEngine.Color) : UnityEngine.Color
            public static op_Division ($a: UnityEngine.Color, $b: number) : UnityEngine.Color
            public static op_Equality ($lhs: UnityEngine.Color, $rhs: UnityEngine.Color) : boolean
            public static op_Inequality ($lhs: UnityEngine.Color, $rhs: UnityEngine.Color) : boolean
            /** Linearly interpolates between colors a and b by t. * @param a Color a.
            * @param b Color b.
            * @param t Float for combining a and b.
            */
            public static Lerp ($a: UnityEngine.Color, $b: UnityEngine.Color, $t: number) : UnityEngine.Color
            /** Linearly interpolates between colors a and b by t. */
            public static LerpUnclamped ($a: UnityEngine.Color, $b: UnityEngine.Color, $t: number) : UnityEngine.Color
            public static op_Implicit ($c: UnityEngine.Color) : UnityEngine.Vector4
            public static op_Implicit ($v: UnityEngine.Vector4) : UnityEngine.Color
            public get_Item ($index: number) : number
            public set_Item ($index: number, $value: number) : void
            /** Calculates the hue, saturation and value of an RGB input color. * @param rgbColor An input color.
            * @param H Output variable for hue.
            * @param S Output variable for saturation.
            * @param V Output variable for value.
            */
            public static RGBToHSV ($rgbColor: UnityEngine.Color, $H: $Ref<number>, $S: $Ref<number>, $V: $Ref<number>) : void
            /** Creates an RGB colour from HSV input.
            * @param H Hue [0..1].
            * @param S Saturation [0..1].
            * @param V Brightness value [0..1].
            * @param hdr Output HDR colours. If true, the returned colour will not be clamped to [0..1].
            * @returns An opaque colour with HSV matching the input. 
            */
            public static HSVToRGB ($H: number, $S: number, $V: number) : UnityEngine.Color
            /** Creates an RGB colour from HSV input.
            * @param H Hue [0..1].
            * @param S Saturation [0..1].
            * @param V Brightness value [0..1].
            * @param hdr Output HDR colours. If true, the returned colour will not be clamped to [0..1].
            * @returns An opaque colour with HSV matching the input. 
            */
            public static HSVToRGB ($H: number, $S: number, $V: number, $hdr: boolean) : UnityEngine.Color
            public constructor ($r: number, $g: number, $b: number, $a: number)
            public constructor ($r: number, $g: number, $b: number)
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        /** Representation of four-dimensional vectors. */
        class Vector4 extends System.ValueType implements System.IEquatable$1<UnityEngine.Vector4>
        {
            public static kEpsilon : number/** X component of the vector. */
            public x : number/** Y component of the vector. */
            public y : number/** Z component of the vector. */
            public z : number/** W component of the vector. */
            public w : number/** Returns this vector with a magnitude of 1 (Read Only). */
            public get normalized(): UnityEngine.Vector4;
            /** Returns the length of this vector (Read Only). */
            public get magnitude(): number;
            /** Returns the squared length of this vector (Read Only). */
            public get sqrMagnitude(): number;
            /** Shorthand for writing Vector4(0,0,0,0). */
            public static get zero(): UnityEngine.Vector4;
            /** Shorthand for writing Vector4(1,1,1,1). */
            public static get one(): UnityEngine.Vector4;
            /** Shorthand for writing Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity). */
            public static get positiveInfinity(): UnityEngine.Vector4;
            /** Shorthand for writing Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity). */
            public static get negativeInfinity(): UnityEngine.Vector4;
            public get_Item ($index: number) : number
            public set_Item ($index: number, $value: number) : void
            /** Set x, y, z and w components of an existing Vector4. */
            public Set ($newX: number, $newY: number, $newZ: number, $newW: number) : void
            /** Linearly interpolates between two vectors. */
            public static Lerp ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4, $t: number) : UnityEngine.Vector4
            /** Linearly interpolates between two vectors. */
            public static LerpUnclamped ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4, $t: number) : UnityEngine.Vector4
            /** Moves a point current towards target. */
            public static MoveTowards ($current: UnityEngine.Vector4, $target: UnityEngine.Vector4, $maxDistanceDelta: number) : UnityEngine.Vector4
            /** Multiplies two vectors component-wise. */
            public static Scale ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4) : UnityEngine.Vector4
            /** Multiplies every component of this vector by the same component of scale. */
            public Scale ($scale: UnityEngine.Vector4) : void
            /** Returns true if the given vector is exactly equal to this vector. */
            public Equals ($other: any) : boolean
            public Equals ($other: UnityEngine.Vector4) : boolean
            public static Normalize ($a: UnityEngine.Vector4) : UnityEngine.Vector4
            public Normalize () : void
            /** Dot Product of two vectors. */
            public static Dot ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4) : number
            /** Projects a vector onto another vector. */
            public static Project ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4) : UnityEngine.Vector4
            /** Returns the distance between a and b. */
            public static Distance ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4) : number
            public static Magnitude ($a: UnityEngine.Vector4) : number
            /** Returns a vector that is made from the smallest components of two vectors. */
            public static Min ($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4) : UnityEngine.Vector4
            /** Returns a vector that is made from the largest components of two vectors. */
            public static Max ($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4) : UnityEngine.Vector4
            public static op_Addition ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4) : UnityEngine.Vector4
            public static op_Subtraction ($a: UnityEngine.Vector4, $b: UnityEngine.Vector4) : UnityEngine.Vector4
            public static op_UnaryNegation ($a: UnityEngine.Vector4) : UnityEngine.Vector4
            public static op_Multiply ($a: UnityEngine.Vector4, $d: number) : UnityEngine.Vector4
            public static op_Multiply ($d: number, $a: UnityEngine.Vector4) : UnityEngine.Vector4
            public static op_Division ($a: UnityEngine.Vector4, $d: number) : UnityEngine.Vector4
            public static op_Equality ($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4) : boolean
            public static op_Inequality ($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4) : boolean
            public static op_Implicit ($v: UnityEngine.Vector3) : UnityEngine.Vector4
            public static op_Implicit ($v: UnityEngine.Vector4) : UnityEngine.Vector3
            public static op_Implicit ($v: UnityEngine.Vector2) : UnityEngine.Vector4
            public static op_Implicit ($v: UnityEngine.Vector4) : UnityEngine.Vector2
            public ToString () : string
            /** Return the Vector4 formatted as a string. */
            public ToString ($format: string) : string
            public static SqrMagnitude ($a: UnityEngine.Vector4) : number
            public SqrMagnitude () : number
            public constructor ($x: number, $y: number, $z: number, $w: number)
            public constructor ($x: number, $y: number, $z: number)
            public constructor ($x: number, $y: number)
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        /** Structure describing the status of a finger touching the screen. */
        class Touch extends System.ValueType
        {
        /** The unique index for the touch. */
            public get fingerId(): number;
            public set fingerId(value: number);
            /** The position of the touch in pixel coordinates. */
            public get position(): UnityEngine.Vector2;
            public set position(value: UnityEngine.Vector2);
            /** The raw position used for the touch. */
            public get rawPosition(): UnityEngine.Vector2;
            public set rawPosition(value: UnityEngine.Vector2);
            /** The position delta since last change. */
            public get deltaPosition(): UnityEngine.Vector2;
            public set deltaPosition(value: UnityEngine.Vector2);
            /** Amount of time that has passed since the last recorded change in Touch values. */
            public get deltaTime(): number;
            public set deltaTime(value: number);
            /** Number of taps. */
            public get tapCount(): number;
            public set tapCount(value: number);
            /** Describes the phase of the touch. */
            public get phase(): UnityEngine.TouchPhase;
            public set phase(value: UnityEngine.TouchPhase);
            /** The current amount of pressure being applied to a touch.  1.0f is considered to be the pressure of an average touch.  If Input.touchPressureSupported returns false, the value of this property will always be 1.0f. */
            public get pressure(): number;
            public set pressure(value: number);
            /** The maximum possible pressure value for a platform.  If Input.touchPressureSupported returns false, the value of this property will always be 1.0f. */
            public get maximumPossiblePressure(): number;
            public set maximumPossiblePressure(value: number);
            /** A value that indicates whether a touch was of Direct, Indirect (or remote), or Stylus type. */
            public get type(): UnityEngine.TouchType;
            public set type(value: UnityEngine.TouchType);
            /** Value of 0 radians indicates that the stylus is parallel to the surface, pi/2 indicates that it is perpendicular. */
            public get altitudeAngle(): number;
            public set altitudeAngle(value: number);
            /** Value of 0 radians indicates that the stylus is pointed along the x-axis of the device. */
            public get azimuthAngle(): number;
            public set azimuthAngle(value: number);
            /** An estimated value of the radius of a touch.  Add radiusVariance to get the maximum touch size, subtract it to get the minimum touch size. */
            public get radius(): number;
            public set radius(value: number);
            /** This value determines the accuracy of the touch radius. Add this value to the radius to get the maximum touch size, subtract it to get the minimum touch size. */
            public get radiusVariance(): number;
            public set radiusVariance(value: number);
        }
        /** Describes phase of a finger touch. */
        enum TouchPhase
        { Began = 0, Moved = 1, Stationary = 2, Ended = 3, Canceled = 4 }
        /** Describes whether a touch is direct, indirect (or remote), or from a stylus. */
        enum TouchType
        { Direct = 0, Indirect = 1, Stylus = 2 }
        /** Access to application run-time data. */
        class Application extends System.Object
        {
        /** Returns true when called in any kind of built Player, or when called in the Editor in Play Mode (Read Only). */
            public static get isPlaying(): boolean;
            /** Whether the player currently has focus. Read-only. */
            public static get isFocused(): boolean;
            /** Returns the platform the game is running on (Read Only). */
            public static get platform(): UnityEngine.RuntimePlatform;
            /** Returns a GUID for this build (Read Only). */
            public static get buildGUID(): string;
            /** Is the current Runtime platform a known mobile platform. */
            public static get isMobilePlatform(): boolean;
            /** Is the current Runtime platform a known console platform. */
            public static get isConsolePlatform(): boolean;
            /** Should the player be running when the application is in the background? */
            public static get runInBackground(): boolean;
            public static set runInBackground(value: boolean);
            /** Returns true when Unity is launched with the -batchmode flag from the command line (Read Only). */
            public static get isBatchMode(): boolean;
            /** Contains the path to the game data folder (Read Only). */
            public static get dataPath(): string;
            /** The path to the StreamingAssets folder (Read Only). */
            public static get streamingAssetsPath(): string;
            /** Contains the path to a persistent data directory (Read Only). */
            public static get persistentDataPath(): string;
            /** Contains the path to a temporary data / cache directory (Read Only). */
            public static get temporaryCachePath(): string;
            /** The URL of the document (what is shown in a browser's address bar) for WebGL (Read Only). */
            public static get absoluteURL(): string;
            /** The version of the Unity runtime used to play the content. */
            public static get unityVersion(): string;
            /** Returns application version number  (Read Only). */
            public static get version(): string;
            /** Returns the name of the store or package that installed the application (Read Only). */
            public static get installerName(): string;
            /** Returns application identifier at runtime. On Apple platforms this is the 'bundleIdentifier' saved in the info.plist file, on Android it's the 'package' from the AndroidManifest.xml.  */
            public static get identifier(): string;
            /** Returns application install mode (Read Only). */
            public static get installMode(): UnityEngine.ApplicationInstallMode;
            /** Returns application running in sandbox (Read Only). */
            public static get sandboxType(): UnityEngine.ApplicationSandboxType;
            /** Returns application product name (Read Only). */
            public static get productName(): string;
            /** Return application company name (Read Only). */
            public static get companyName(): string;
            /** A unique cloud project identifier. It is unique for every project (Read Only). */
            public static get cloudProjectId(): string;
            /** Instructs the game to try to render at a specified frame rate. */
            public static get targetFrameRate(): number;
            public static set targetFrameRate(value: number);
            /** The language the user's operating system is running in. */
            public static get systemLanguage(): UnityEngine.SystemLanguage;
            /** Returns the path to the console log file, or an empty string if the current platform does not support log files. */
            public static get consoleLogPath(): string;
            /** Priority of background loading thread. */
            public static get backgroundLoadingPriority(): UnityEngine.ThreadPriority;
            public static set backgroundLoadingPriority(value: UnityEngine.ThreadPriority);
            /** Returns the type of Internet reachability currently possible on the device. */
            public static get internetReachability(): UnityEngine.NetworkReachability;
            /** Returns false if application is altered in any way after it was built. */
            public static get genuine(): boolean;
            /** Returns true if application integrity can be confirmed. */
            public static get genuineCheckAvailable(): boolean;
            /** Are we running inside the Unity editor? (Read Only) */
            public static get isEditor(): boolean;
            public static Quit ($exitCode: number) : void
            public static Quit () : void
            public static Unload () : void
            /** Can the streamed level be loaded? */
            public static CanStreamedLevelBeLoaded ($levelIndex: number) : boolean
            /** Can the streamed level be loaded? */
            public static CanStreamedLevelBeLoaded ($levelName: string) : boolean
            /** Returns true if the given object is part of the playing world either in any kind of built Player or in Play Mode.
            * @param obj The object to test.
            * @returns True if the object is part of the playing world. 
            */
            public static IsPlaying ($obj: UnityEngine.Object) : boolean
            public static GetBuildTags () : System.Array$1<string>
            /** Set an array of feature tags for this build. */
            public static SetBuildTags ($buildTags: System.Array$1<string>) : void
            public static HasProLicense () : boolean
            public static RequestAdvertisingIdentifierAsync ($delegateMethod: UnityEngine.Application.AdvertisingIdentifierCallback) : boolean
            /** Opens the url in a browser. */
            public static OpenURL ($url: string) : void
            /** Get stack trace logging options. The default value is StackTraceLogType.ScriptOnly. */
            public static GetStackTraceLogType ($logType: UnityEngine.LogType) : UnityEngine.StackTraceLogType
            /** Set stack trace logging options. The default value is StackTraceLogType.ScriptOnly. */
            public static SetStackTraceLogType ($logType: UnityEngine.LogType, $stackTraceType: UnityEngine.StackTraceLogType) : void
            /** Request authorization to use the webcam or microphone on iOS. */
            public static RequestUserAuthorization ($mode: UnityEngine.UserAuthorization) : UnityEngine.AsyncOperation
            /** Check if the user has authorized use of the webcam or microphone in the Web Player. */
            public static HasUserAuthorization ($mode: UnityEngine.UserAuthorization) : boolean
            public static add_lowMemory ($value: UnityEngine.Application.LowMemoryCallback) : void
            public static remove_lowMemory ($value: UnityEngine.Application.LowMemoryCallback) : void
            public static add_logMessageReceived ($value: UnityEngine.Application.LogCallback) : void
            public static remove_logMessageReceived ($value: UnityEngine.Application.LogCallback) : void
            public static add_logMessageReceivedThreaded ($value: UnityEngine.Application.LogCallback) : void
            public static remove_logMessageReceivedThreaded ($value: UnityEngine.Application.LogCallback) : void
            public static add_onBeforeRender ($value: UnityEngine.Events.UnityAction) : void
            public static remove_onBeforeRender ($value: UnityEngine.Events.UnityAction) : void
            public static add_focusChanged ($value: System.Action$1<boolean>) : void
            public static remove_focusChanged ($value: System.Action$1<boolean>) : void
            public static add_wantsToQuit ($value: System.Func$1<boolean>) : void
            public static remove_wantsToQuit ($value: System.Func$1<boolean>) : void
            public static add_quitting ($value: System.Action) : void
            public static remove_quitting ($value: System.Action) : void
            public constructor ()
        }
        /** The platform application is running. Returned by Application.platform. */
        enum RuntimePlatform
        { OSXEditor = 0, OSXPlayer = 1, WindowsPlayer = 2, OSXWebPlayer = 3, OSXDashboardPlayer = 4, WindowsWebPlayer = 5, WindowsEditor = 7, IPhonePlayer = 8, XBOX360 = 10, PS3 = 9, Android = 11, NaCl = 12, FlashPlayer = 15, LinuxPlayer = 13, LinuxEditor = 16, WebGLPlayer = 17, MetroPlayerX86 = 18, WSAPlayerX86 = 18, MetroPlayerX64 = 19, WSAPlayerX64 = 19, MetroPlayerARM = 20, WSAPlayerARM = 20, WP8Player = 21, BB10Player = 22, BlackBerryPlayer = 22, TizenPlayer = 23, PSP2 = 24, PS4 = 25, PSM = 26, XboxOne = 27, SamsungTVPlayer = 28, WiiU = 30, tvOS = 31, Switch = 32, Lumin = 33 }
        /** Application installation mode (Read Only). */
        enum ApplicationInstallMode
        { Unknown = 0, Store = 1, DeveloperBuild = 2, Adhoc = 3, Enterprise = 4, Editor = 5 }
        /** Application sandbox type. */
        enum ApplicationSandboxType
        { Unknown = 0, NotSandboxed = 1, Sandboxed = 2, SandboxBroken = 3 }
        /** The language the user's operating system is running in. Returned by Application.systemLanguage. */
        enum SystemLanguage
        { Afrikaans = 0, Arabic = 1, Basque = 2, Belarusian = 3, Bulgarian = 4, Catalan = 5, Chinese = 6, Czech = 7, Danish = 8, Dutch = 9, English = 10, Estonian = 11, Faroese = 12, Finnish = 13, French = 14, German = 15, Greek = 16, Hebrew = 17, Hugarian = 18, Icelandic = 19, Indonesian = 20, Italian = 21, Japanese = 22, Korean = 23, Latvian = 24, Lithuanian = 25, Norwegian = 26, Polish = 27, Portuguese = 28, Romanian = 29, Russian = 30, SerboCroatian = 31, Slovak = 32, Slovenian = 33, Spanish = 34, Swedish = 35, Thai = 36, Turkish = 37, Ukrainian = 38, Vietnamese = 39, ChineseSimplified = 40, ChineseTraditional = 41, Unknown = 42, Hungarian = 18 }
        /** Stack trace logging options. */
        enum StackTraceLogType
        { None = 0, ScriptOnly = 1, Full = 2 }
        /** The type of the log message in Debug.unityLogger.Log or delegate registered with Application.RegisterLogCallback. */
        enum LogType
        { Error = 0, Assert = 1, Warning = 2, Log = 3, Exception = 4 }
        /** Describes network reachability options. */
        enum NetworkReachability
        { NotReachable = 0, ReachableViaCarrierDataNetwork = 1, ReachableViaLocalAreaNetwork = 2 }
        /** Constants to pass to Application.RequestUserAuthorization. */
        enum UserAuthorization
        { WebCam = 1, Microphone = 2 }
        /** The material class. */
        class Material extends UnityEngine.Object
        {
        /** The shader used by the material. */
            public get shader(): UnityEngine.Shader;
            public set shader(value: UnityEngine.Shader);
            /** The main material's color. */
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            /** The material's texture. */
            public get mainTexture(): UnityEngine.Texture;
            public set mainTexture(value: UnityEngine.Texture);
            /** The texture offset of the main texture. */
            public get mainTextureOffset(): UnityEngine.Vector2;
            public set mainTextureOffset(value: UnityEngine.Vector2);
            /** The texture scale of the main texture. */
            public get mainTextureScale(): UnityEngine.Vector2;
            public set mainTextureScale(value: UnityEngine.Vector2);
            /** Render queue of this material. */
            public get renderQueue(): number;
            public set renderQueue(value: number);
            /** Defines how the material should interact with lightmaps and lightprobes. */
            public get globalIlluminationFlags(): UnityEngine.MaterialGlobalIlluminationFlags;
            public set globalIlluminationFlags(value: UnityEngine.MaterialGlobalIlluminationFlags);
            /** Gets and sets whether the Double Sided Global Illumination setting is enabled for this material. */
            public get doubleSidedGI(): boolean;
            public set doubleSidedGI(value: boolean);
            /** Gets and sets whether GPU instancing is enabled for this material. */
            public get enableInstancing(): boolean;
            public set enableInstancing(value: boolean);
            /** How many passes are in this material (Read Only). */
            public get passCount(): number;
            /** Additional shader keywords set by this material. */
            public get shaderKeywords(): System.Array$1<string>;
            public set shaderKeywords(value: System.Array$1<string>);
            /** Checks if material's shader has a property of a given name. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public HasProperty ($nameID: number) : boolean
            /** Checks if material's shader has a property of a given name. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public HasProperty ($name: string) : boolean
            /** Sets a shader keyword that is enabled by this material. */
            public EnableKeyword ($keyword: string) : void
            /** Unset a shader keyword. */
            public DisableKeyword ($keyword: string) : void
            /** Is the shader keyword enabled on this material? */
            public IsKeywordEnabled ($keyword: string) : boolean
            /** Enables or disables a Shader pass on a per-Material level. * @param passName Shader pass name (case insensitive).
            * @param enabled Flag indicating whether this Shader pass should be enabled.
            */
            public SetShaderPassEnabled ($passName: string, $enabled: boolean) : void
            /** Checks whether a given Shader pass is enabled on this Material.
            * @param passName Shader pass name (case insensitive).
            * @returns True if the Shader pass is enabled. 
            */
            public GetShaderPassEnabled ($passName: string) : boolean
            /** Returns the name of the shader pass at index pass. */
            public GetPassName ($pass: number) : string
            /** Returns the index of the pass passName. */
            public FindPass ($passName: string) : number
            /** Sets an override tag/value on the material. * @param tag Name of the tag to set.
            * @param val Name of the value to set. Empty string to clear the override flag.
            */
            public SetOverrideTag ($tag: string, $val: string) : void
            /** Get the value of material's shader tag. */
            public GetTag ($tag: string, $searchFallbacks: boolean, $defaultValue: string) : string
            /** Get the value of material's shader tag. */
            public GetTag ($tag: string, $searchFallbacks: boolean) : string
            /** Interpolate properties between two materials. */
            public Lerp ($start: UnityEngine.Material, $end: UnityEngine.Material, $t: number) : void
            /** Activate the given pass for rendering.
            * @param pass Shader pass number to setup.
            * @returns If false is returned, no rendering should be done. 
            */
            public SetPass ($pass: number) : boolean
            /** Copy properties from other material into this material. */
            public CopyPropertiesFromMaterial ($mat: UnityEngine.Material) : void
            public GetTexturePropertyNames () : System.Array$1<string>
            public GetTexturePropertyNameIDs () : System.Array$1<number>
            public GetTexturePropertyNames ($outNames: System.Collections.Generic.List$1<string>) : void
            public GetTexturePropertyNameIDs ($outNames: System.Collections.Generic.List$1<number>) : void
            /** Sets a named float value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param value Float value to set.
            * @param name Property name, e.g. "_Glossiness".
            */
            public SetFloat ($name: string, $value: number) : void
            /** Sets a named float value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param value Float value to set.
            * @param name Property name, e.g. "_Glossiness".
            */
            public SetFloat ($nameID: number, $value: number) : void
            /** Sets a named integer value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param value Integer value to set.
            * @param name Property name, e.g. "_SrcBlend".
            */
            public SetInt ($name: string, $value: number) : void
            /** Sets a named integer value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param value Integer value to set.
            * @param name Property name, e.g. "_SrcBlend".
            */
            public SetInt ($nameID: number, $value: number) : void
            /** Sets a named color value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_Color".
            * @param value Color value to set.
            */
            public SetColor ($name: string, $value: UnityEngine.Color) : void
            /** Sets a named color value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_Color".
            * @param value Color value to set.
            */
            public SetColor ($nameID: number, $value: UnityEngine.Color) : void
            /** Sets a named vector value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_WaveAndDistance".
            * @param value Vector value to set.
            */
            public SetVector ($name: string, $value: UnityEngine.Vector4) : void
            /** Sets a named vector value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_WaveAndDistance".
            * @param value Vector value to set.
            */
            public SetVector ($nameID: number, $value: UnityEngine.Vector4) : void
            /** Sets a named matrix for the shader. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_CubemapRotation".
            * @param value Matrix value to set.
            */
            public SetMatrix ($name: string, $value: UnityEngine.Matrix4x4) : void
            /** Sets a named matrix for the shader. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_CubemapRotation".
            * @param value Matrix value to set.
            */
            public SetMatrix ($nameID: number, $value: UnityEngine.Matrix4x4) : void
            /** Sets a named texture. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_MainTex".
            * @param value Texture to set.
            */
            public SetTexture ($name: string, $value: UnityEngine.Texture) : void
            /** Sets a named texture. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_MainTex".
            * @param value Texture to set.
            */
            public SetTexture ($nameID: number, $value: UnityEngine.Texture) : void
            /** Sets a named ComputeBuffer value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name.
            * @param value The ComputeBuffer value to set.
            */
            public SetBuffer ($name: string, $value: UnityEngine.ComputeBuffer) : void
            /** Sets a named ComputeBuffer value. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name.
            * @param value The ComputeBuffer value to set.
            */
            public SetBuffer ($nameID: number, $value: UnityEngine.ComputeBuffer) : void
            public SetFloatArray ($name: string, $values: System.Collections.Generic.List$1<number>) : void
            public SetFloatArray ($nameID: number, $values: System.Collections.Generic.List$1<number>) : void
            /** Sets a float array property. * @param name Property name.
            * @param nameID Property name ID. Use Shader.PropertyToID to get this ID.
            * @param values Array of values to set.
            */
            public SetFloatArray ($name: string, $values: System.Array$1<number>) : void
            /** Sets a float array property. * @param name Property name.
            * @param nameID Property name ID. Use Shader.PropertyToID to get this ID.
            * @param values Array of values to set.
            */
            public SetFloatArray ($nameID: number, $values: System.Array$1<number>) : void
            public SetColorArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Color>) : void
            public SetColorArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Color>) : void
            /** Sets a color array property. * @param name Property name.
            * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param values Array of values to set.
            */
            public SetColorArray ($name: string, $values: System.Array$1<UnityEngine.Color>) : void
            /** Sets a color array property. * @param name Property name.
            * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param values Array of values to set.
            */
            public SetColorArray ($nameID: number, $values: System.Array$1<UnityEngine.Color>) : void
            public SetVectorArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public SetVectorArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            /** Sets a vector array property. * @param name Property name.
            * @param values Array of values to set.
            * @param nameID Property name ID, use Shader.PropertyToID to get it.
            */
            public SetVectorArray ($name: string, $values: System.Array$1<UnityEngine.Vector4>) : void
            /** Sets a vector array property. * @param name Property name.
            * @param values Array of values to set.
            * @param nameID Property name ID, use Shader.PropertyToID to get it.
            */
            public SetVectorArray ($nameID: number, $values: System.Array$1<UnityEngine.Vector4>) : void
            public SetMatrixArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            public SetMatrixArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            /** Sets a matrix array property. * @param name Property name.
            * @param values Array of values to set.
            * @param nameID Property name ID, use Shader.PropertyToID to get it.
            */
            public SetMatrixArray ($name: string, $values: System.Array$1<UnityEngine.Matrix4x4>) : void
            /** Sets a matrix array property. * @param name Property name.
            * @param values Array of values to set.
            * @param nameID Property name ID, use Shader.PropertyToID to get it.
            */
            public SetMatrixArray ($nameID: number, $values: System.Array$1<UnityEngine.Matrix4x4>) : void
            /** Get a named float value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetFloat ($name: string) : number
            /** Get a named float value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetFloat ($nameID: number) : number
            /** Get a named integer value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetInt ($name: string) : number
            /** Get a named integer value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetInt ($nameID: number) : number
            /** Get a named color value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetColor ($name: string) : UnityEngine.Color
            /** Get a named color value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetColor ($nameID: number) : UnityEngine.Color
            /** Get a named vector value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetVector ($name: string) : UnityEngine.Vector4
            /** Get a named vector value. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetVector ($nameID: number) : UnityEngine.Vector4
            /** Get a named matrix value from the shader. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetMatrix ($name: string) : UnityEngine.Matrix4x4
            /** Get a named matrix value from the shader. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetMatrix ($nameID: number) : UnityEngine.Matrix4x4
            /** Get a named texture. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetTexture ($name: string) : UnityEngine.Texture
            /** Get a named texture. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetTexture ($nameID: number) : UnityEngine.Texture
            /** Get a named float array. * @param name The name of the property.
            * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            */
            public GetFloatArray ($name: string) : System.Array$1<number>
            /** Get a named float array. * @param name The name of the property.
            * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            */
            public GetFloatArray ($nameID: number) : System.Array$1<number>
            /** Get a named color array. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetColorArray ($name: string) : System.Array$1<UnityEngine.Color>
            /** Get a named color array. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetColorArray ($nameID: number) : System.Array$1<UnityEngine.Color>
            /** Get a named vector array. * @param name The name of the property.
            * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            */
            public GetVectorArray ($name: string) : System.Array$1<UnityEngine.Vector4>
            /** Get a named vector array. * @param name The name of the property.
            * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            */
            public GetVectorArray ($nameID: number) : System.Array$1<UnityEngine.Vector4>
            /** Get a named matrix array. * @param name The name of the property.
            * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            */
            public GetMatrixArray ($name: string) : System.Array$1<UnityEngine.Matrix4x4>
            /** Get a named matrix array. * @param name The name of the property.
            * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            */
            public GetMatrixArray ($nameID: number) : System.Array$1<UnityEngine.Matrix4x4>
            public GetFloatArray ($name: string, $values: System.Collections.Generic.List$1<number>) : void
            public GetFloatArray ($nameID: number, $values: System.Collections.Generic.List$1<number>) : void
            public GetColorArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Color>) : void
            public GetColorArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Color>) : void
            public GetVectorArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public GetVectorArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public GetMatrixArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            public GetMatrixArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            /** Sets the placement offset of texture propertyName. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, for example: "_MainTex".
            * @param value Texture placement offset.
            */
            public SetTextureOffset ($name: string, $value: UnityEngine.Vector2) : void
            /** Sets the placement offset of texture propertyName. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, for example: "_MainTex".
            * @param value Texture placement offset.
            */
            public SetTextureOffset ($nameID: number, $value: UnityEngine.Vector2) : void
            /** Sets the placement scale of texture propertyName. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_MainTex".
            * @param value Texture placement scale.
            */
            public SetTextureScale ($name: string, $value: UnityEngine.Vector2) : void
            /** Sets the placement scale of texture propertyName. * @param nameID Property name ID, use Shader.PropertyToID to get it.
            * @param name Property name, e.g. "_MainTex".
            * @param value Texture placement scale.
            */
            public SetTextureScale ($nameID: number, $value: UnityEngine.Vector2) : void
            /** Gets the placement offset of texture propertyName. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetTextureOffset ($name: string) : UnityEngine.Vector2
            /** Gets the placement offset of texture propertyName. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetTextureOffset ($nameID: number) : UnityEngine.Vector2
            /** Gets the placement scale of texture propertyName. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetTextureScale ($name: string) : UnityEngine.Vector2
            /** Gets the placement scale of texture propertyName. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public GetTextureScale ($nameID: number) : UnityEngine.Vector2
            public constructor ($shader: UnityEngine.Shader)
            public constructor ($source: UnityEngine.Material)
            public constructor ()
        }
        /** Shader scripts used for all rendering. */
        class Shader extends UnityEngine.Object
        {
        /** Shader LOD level for this shader. */
            public get maximumLOD(): number;
            public set maximumLOD(value: number);
            /** Shader LOD level for all shaders. */
            public static get globalMaximumLOD(): number;
            public static set globalMaximumLOD(value: number);
            /** Can this shader run on the end-users graphics card? (Read Only) */
            public get isSupported(): boolean;
            /** Render pipeline currently in use. */
            public static get globalRenderPipeline(): string;
            public static set globalRenderPipeline(value: string);
            /** Render queue of this shader. (Read Only) */
            public get renderQueue(): number;
            /** Finds a shader with the given name. */
            public static Find ($name: string) : UnityEngine.Shader
            /** Set a global shader keyword. */
            public static EnableKeyword ($keyword: string) : void
            /** Unset a global shader keyword. */
            public static DisableKeyword ($keyword: string) : void
            /** Is global shader keyword enabled? */
            public static IsKeywordEnabled ($keyword: string) : boolean
            public static WarmupAllShaders () : void
            /** Gets unique identifier for a shader property name.
            * @param name Shader property name.
            * @returns Unique integer for the name. 
            */
            public static PropertyToID ($name: string) : number
            /** Sets a global float property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalFloat ($name: string, $value: number) : void
            /** Sets a global float property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalFloat ($nameID: number, $value: number) : void
            /** Sets a global int property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalInt ($name: string, $value: number) : void
            /** Sets a global int property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalInt ($nameID: number, $value: number) : void
            /** Sets a global vector property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalVector ($name: string, $value: UnityEngine.Vector4) : void
            /** Sets a global vector property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalVector ($nameID: number, $value: UnityEngine.Vector4) : void
            /** Sets a global color property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalColor ($name: string, $value: UnityEngine.Color) : void
            /** Sets a global color property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalColor ($nameID: number, $value: UnityEngine.Color) : void
            /** Sets a global matrix property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalMatrix ($name: string, $value: UnityEngine.Matrix4x4) : void
            /** Sets a global matrix property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalMatrix ($nameID: number, $value: UnityEngine.Matrix4x4) : void
            /** Sets a global texture property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalTexture ($name: string, $value: UnityEngine.Texture) : void
            /** Sets a global texture property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalTexture ($nameID: number, $value: UnityEngine.Texture) : void
            /** Sets a global compute buffer property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalBuffer ($name: string, $value: UnityEngine.ComputeBuffer) : void
            /** Sets a global compute buffer property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalBuffer ($nameID: number, $value: UnityEngine.ComputeBuffer) : void
            public static SetGlobalFloatArray ($name: string, $values: System.Collections.Generic.List$1<number>) : void
            public static SetGlobalFloatArray ($nameID: number, $values: System.Collections.Generic.List$1<number>) : void
            /** Sets a global float array property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalFloatArray ($name: string, $values: System.Array$1<number>) : void
            /** Sets a global float array property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalFloatArray ($nameID: number, $values: System.Array$1<number>) : void
            public static SetGlobalVectorArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public static SetGlobalVectorArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            /** Sets a global vector array property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalVectorArray ($name: string, $values: System.Array$1<UnityEngine.Vector4>) : void
            /** Sets a global vector array property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalVectorArray ($nameID: number, $values: System.Array$1<UnityEngine.Vector4>) : void
            public static SetGlobalMatrixArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            public static SetGlobalMatrixArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            /** Sets a global matrix array property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalMatrixArray ($name: string, $values: System.Array$1<UnityEngine.Matrix4x4>) : void
            /** Sets a global matrix array property for all shaders. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static SetGlobalMatrixArray ($nameID: number, $values: System.Array$1<UnityEngine.Matrix4x4>) : void
            /** Gets a global float property for all shaders previously set using SetGlobalFloat. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalFloat ($name: string) : number
            /** Gets a global float property for all shaders previously set using SetGlobalFloat. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalFloat ($nameID: number) : number
            /** Gets a global int property for all shaders previously set using SetGlobalInt. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalInt ($name: string) : number
            /** Gets a global int property for all shaders previously set using SetGlobalInt. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalInt ($nameID: number) : number
            /** Gets a global vector property for all shaders previously set using SetGlobalVector. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalVector ($name: string) : UnityEngine.Vector4
            /** Gets a global vector property for all shaders previously set using SetGlobalVector. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalVector ($nameID: number) : UnityEngine.Vector4
            /** Gets a global color property for all shaders previously set using SetGlobalColor. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalColor ($name: string) : UnityEngine.Color
            /** Gets a global color property for all shaders previously set using SetGlobalColor. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalColor ($nameID: number) : UnityEngine.Color
            /** Gets a global matrix property for all shaders previously set using SetGlobalMatrix. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalMatrix ($name: string) : UnityEngine.Matrix4x4
            /** Gets a global matrix property for all shaders previously set using SetGlobalMatrix. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalMatrix ($nameID: number) : UnityEngine.Matrix4x4
            /** Gets a global texture property for all shaders previously set using SetGlobalTexture. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalTexture ($name: string) : UnityEngine.Texture
            /** Gets a global texture property for all shaders previously set using SetGlobalTexture. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalTexture ($nameID: number) : UnityEngine.Texture
            /** Gets a global float array for all shaders previously set using SetGlobalFloatArray. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalFloatArray ($name: string) : System.Array$1<number>
            /** Gets a global float array for all shaders previously set using SetGlobalFloatArray. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalFloatArray ($nameID: number) : System.Array$1<number>
            /** Gets a global vector array for all shaders previously set using SetGlobalVectorArray. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalVectorArray ($name: string) : System.Array$1<UnityEngine.Vector4>
            /** Gets a global vector array for all shaders previously set using SetGlobalVectorArray. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalVectorArray ($nameID: number) : System.Array$1<UnityEngine.Vector4>
            /** Gets a global matrix array for all shaders previously set using SetGlobalMatrixArray. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalMatrixArray ($name: string) : System.Array$1<UnityEngine.Matrix4x4>
            /** Gets a global matrix array for all shaders previously set using SetGlobalMatrixArray. * @param nameID The name ID of the property retrieved by Shader.PropertyToID.
            * @param name The name of the property.
            */
            public static GetGlobalMatrixArray ($nameID: number) : System.Array$1<UnityEngine.Matrix4x4>
            public static GetGlobalFloatArray ($name: string, $values: System.Collections.Generic.List$1<number>) : void
            public static GetGlobalFloatArray ($nameID: number, $values: System.Collections.Generic.List$1<number>) : void
            public static GetGlobalVectorArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public static GetGlobalVectorArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public static GetGlobalMatrixArray ($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            public static GetGlobalMatrixArray ($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
        }
        /** How the material interacts with lightmaps and lightprobes. */
        enum MaterialGlobalIlluminationFlags
        { None = 0, RealtimeEmissive = 1, BakedEmissive = 2, EmissiveIsBlack = 4, AnyEmissive = 3 }
        /** GPU data buffer, mostly for use with compute shaders. */
        class ComputeBuffer extends System.Object implements System.IDisposable
        {
        }
        /** Representation of rays. */
        class Ray extends System.ValueType
        {
        /** The origin point of the ray. */
            public get origin(): UnityEngine.Vector3;
            public set origin(value: UnityEngine.Vector3);
            /** The direction of the ray. */
            public get direction(): UnityEngine.Vector3;
            public set direction(value: UnityEngine.Vector3);
            /** Returns a point at distance units along the ray. */
            public GetPoint ($distance: number) : UnityEngine.Vector3
            public ToString () : string
            /** Returns a nicely formatted string for this ray. */
            public ToString ($format: string) : string
            public constructor ($origin: UnityEngine.Vector3, $direction: UnityEngine.Vector3)
            public constructor ()
        }
        /** A Camera is a device through which the player views the world. */
        class Camera extends UnityEngine.Behaviour
        {
        /** Event that is fired before any camera starts culling. */
            public static onPreCull : UnityEngine.Camera.CameraCallback/** Event that is fired before any camera starts rendering. */
            public static onPreRender : UnityEngine.Camera.CameraCallback/** Event that is fired after any camera finishes rendering. */
            public static onPostRender : UnityEngine.Camera.CameraCallback/** The near clipping plane distance. */
            public get nearClipPlane(): number;
            public set nearClipPlane(value: number);
            /** The far clipping plane distance. */
            public get farClipPlane(): number;
            public set farClipPlane(value: number);
            /** The field of view of the camera in degrees. */
            public get fieldOfView(): number;
            public set fieldOfView(value: number);
            /** The rendering path that should be used, if possible. */
            public get renderingPath(): UnityEngine.RenderingPath;
            public set renderingPath(value: UnityEngine.RenderingPath);
            /** The rendering path that is currently being used (Read Only). */
            public get actualRenderingPath(): UnityEngine.RenderingPath;
            /** High dynamic range rendering. */
            public get allowHDR(): boolean;
            public set allowHDR(value: boolean);
            /** MSAA rendering. */
            public get allowMSAA(): boolean;
            public set allowMSAA(value: boolean);
            /** Dynamic Resolution Scaling. */
            public get allowDynamicResolution(): boolean;
            public set allowDynamicResolution(value: boolean);
            /** Should camera rendering be forced into a RenderTexture. */
            public get forceIntoRenderTexture(): boolean;
            public set forceIntoRenderTexture(value: boolean);
            /** Camera's half-size when in orthographic mode. */
            public get orthographicSize(): number;
            public set orthographicSize(value: number);
            /** Is the camera orthographic (true) or perspective (false)? */
            public get orthographic(): boolean;
            public set orthographic(value: boolean);
            /** Opaque object sorting mode. */
            public get opaqueSortMode(): UnityEngine.Rendering.OpaqueSortMode;
            public set opaqueSortMode(value: UnityEngine.Rendering.OpaqueSortMode);
            /** Transparent object sorting mode. */
            public get transparencySortMode(): UnityEngine.TransparencySortMode;
            public set transparencySortMode(value: UnityEngine.TransparencySortMode);
            /** An axis that describes the direction along which the distances of objects are measured for the purpose of sorting. */
            public get transparencySortAxis(): UnityEngine.Vector3;
            public set transparencySortAxis(value: UnityEngine.Vector3);
            /** Camera's depth in the camera rendering order. */
            public get depth(): number;
            public set depth(value: number);
            /** The aspect ratio (width divided by height). */
            public get aspect(): number;
            public set aspect(value: number);
            /** Get the world-space speed of the camera (Read Only). */
            public get velocity(): UnityEngine.Vector3;
            /** This is used to render parts of the Scene selectively. */
            public get cullingMask(): number;
            public set cullingMask(value: number);
            /** Mask to select which layers can trigger events on the camera. */
            public get eventMask(): number;
            public set eventMask(value: number);
            /** How to perform per-layer culling for a Camera. */
            public get layerCullSpherical(): boolean;
            public set layerCullSpherical(value: boolean);
            /** Identifies what kind of camera this is. */
            public get cameraType(): UnityEngine.CameraType;
            public set cameraType(value: UnityEngine.CameraType);
            /** Per-layer culling distances. */
            public get layerCullDistances(): System.Array$1<number>;
            public set layerCullDistances(value: System.Array$1<number>);
            /** Whether or not the Camera will use occlusion culling during rendering. */
            public get useOcclusionCulling(): boolean;
            public set useOcclusionCulling(value: boolean);
            /** Sets a custom matrix for the camera to use for all culling queries. */
            public get cullingMatrix(): UnityEngine.Matrix4x4;
            public set cullingMatrix(value: UnityEngine.Matrix4x4);
            /** The color with which the screen will be cleared. */
            public get backgroundColor(): UnityEngine.Color;
            public set backgroundColor(value: UnityEngine.Color);
            /** How the camera clears the background. */
            public get clearFlags(): UnityEngine.CameraClearFlags;
            public set clearFlags(value: UnityEngine.CameraClearFlags);
            /** How and if camera generates a depth texture. */
            public get depthTextureMode(): UnityEngine.DepthTextureMode;
            public set depthTextureMode(value: UnityEngine.DepthTextureMode);
            /** Should the camera clear the stencil buffer after the deferred light pass? */
            public get clearStencilAfterLightingPass(): boolean;
            public set clearStencilAfterLightingPass(value: boolean);
            /** Enable [UsePhysicalProperties] to use physical camera properties to compute the field of view and the frustum. */
            public get usePhysicalProperties(): boolean;
            public set usePhysicalProperties(value: boolean);
            /** The size of the camera sensor, expressed in millimeters. */
            public get sensorSize(): UnityEngine.Vector2;
            public set sensorSize(value: UnityEngine.Vector2);
            /** The lens offset of the camera. The lens shift is relative to the sensor size. For example, a lens shift of 0.5 offsets the sensor by half its horizontal size. */
            public get lensShift(): UnityEngine.Vector2;
            public set lensShift(value: UnityEngine.Vector2);
            /** The camera focal length, expressed in millimeters. To use this property, enable UsePhysicalProperties. */
            public get focalLength(): number;
            public set focalLength(value: number);
            /** There are two gates for a camera, the sensor gate and the resolution gate. The physical camera sensor gate is defined by the sensorSize property, the resolution gate is defined by the render target area. */
            public get gateFit(): UnityEngine.Camera.GateFitMode;
            public set gateFit(value: UnityEngine.Camera.GateFitMode);
            /** Where on the screen is the camera rendered in normalized coordinates. */
            public get rect(): UnityEngine.Rect;
            public set rect(value: UnityEngine.Rect);
            /** Where on the screen is the camera rendered in pixel coordinates. */
            public get pixelRect(): UnityEngine.Rect;
            public set pixelRect(value: UnityEngine.Rect);
            /** How wide is the camera in pixels (not accounting for dynamic resolution scaling) (Read Only). */
            public get pixelWidth(): number;
            /** How tall is the camera in pixels (not accounting for dynamic resolution scaling) (Read Only). */
            public get pixelHeight(): number;
            /** How wide is the camera in pixels (accounting for dynamic resolution scaling) (Read Only). */
            public get scaledPixelWidth(): number;
            /** How tall is the camera in pixels (accounting for dynamic resolution scaling) (Read Only). */
            public get scaledPixelHeight(): number;
            /** Destination render texture. */
            public get targetTexture(): UnityEngine.RenderTexture;
            public set targetTexture(value: UnityEngine.RenderTexture);
            /** Gets the temporary RenderTexture target for this Camera. */
            public get activeTexture(): UnityEngine.RenderTexture;
            /** Set the target display for this Camera. */
            public get targetDisplay(): number;
            public set targetDisplay(value: number);
            /** Matrix that transforms from camera space to world space (Read Only). */
            public get cameraToWorldMatrix(): UnityEngine.Matrix4x4;
            /** Matrix that transforms from world to camera space. */
            public get worldToCameraMatrix(): UnityEngine.Matrix4x4;
            public set worldToCameraMatrix(value: UnityEngine.Matrix4x4);
            /** Set a custom projection matrix. */
            public get projectionMatrix(): UnityEngine.Matrix4x4;
            public set projectionMatrix(value: UnityEngine.Matrix4x4);
            /** Get or set the raw projection matrix with no camera offset (no jittering). */
            public get nonJitteredProjectionMatrix(): UnityEngine.Matrix4x4;
            public set nonJitteredProjectionMatrix(value: UnityEngine.Matrix4x4);
            /** Should the jittered matrix be used for transparency rendering? */
            public get useJitteredProjectionMatrixForTransparentRendering(): boolean;
            public set useJitteredProjectionMatrixForTransparentRendering(value: boolean);
            /** Get the view projection matrix used on the last frame. */
            public get previousViewProjectionMatrix(): UnityEngine.Matrix4x4;
            /** The first enabled camera tagged "MainCamera" (Read Only). */
            public static get main(): UnityEngine.Camera;
            /** The camera we are currently rendering with, for low-level render control only (Read Only). */
            public static get current(): UnityEngine.Camera;
            /** If not null, the camera will only render the contents of the specified Scene. */
            public get scene(): UnityEngine.SceneManagement.Scene;
            public set scene(value: UnityEngine.SceneManagement.Scene);
            /** Stereoscopic rendering. */
            public get stereoEnabled(): boolean;
            /** The distance between the virtual eyes. Use this to query or set the current eye separation. Note that most VR devices provide this value, in which case setting the value will have no effect. */
            public get stereoSeparation(): number;
            public set stereoSeparation(value: number);
            /** Distance to a point where virtual eyes converge. */
            public get stereoConvergence(): number;
            public set stereoConvergence(value: number);
            /** Determines whether the stereo view matrices are suitable to allow for a single pass cull. */
            public get areVRStereoViewMatricesWithinSingleCullTolerance(): boolean;
            /** Defines which eye of a VR display the Camera renders into. */
            public get stereoTargetEye(): UnityEngine.StereoTargetEyeMask;
            public set stereoTargetEye(value: UnityEngine.StereoTargetEyeMask);
            /** Returns the eye that is currently rendering.
            If called when stereo is not enabled it will return Camera.MonoOrStereoscopicEye.Mono.
            If called during a camera rendering callback such as OnRenderImage it will return the currently rendering eye.
            If called outside of a rendering callback and stereo is enabled, it will return the default eye which is Camera.MonoOrStereoscopicEye.Left. */
            public get stereoActiveEye(): UnityEngine.Camera.MonoOrStereoscopicEye;
            /** The number of cameras in the current Scene. */
            public static get allCamerasCount(): number;
            /** Returns all enabled cameras in the Scene. */
            public static get allCameras(): System.Array$1<UnityEngine.Camera>;
            /** Number of command buffers set up on this camera (Read Only). */
            public get commandBufferCount(): number;
            public Reset () : void
            public ResetTransparencySortSettings () : void
            public ResetAspect () : void
            public ResetCullingMatrix () : void
            /** Make the camera render with shader replacement. */
            public SetReplacementShader ($shader: UnityEngine.Shader, $replacementTag: string) : void
            public ResetReplacementShader () : void
            /** Sets the Camera to render to the chosen buffers of one or more RenderTextures. * @param colorBuffer The RenderBuffer(s) to which color information will be rendered.
            * @param depthBuffer The RenderBuffer to which depth information will be rendered.
            */
            public SetTargetBuffers ($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer) : void
            /** Sets the Camera to render to the chosen buffers of one or more RenderTextures. * @param colorBuffer The RenderBuffer(s) to which color information will be rendered.
            * @param depthBuffer The RenderBuffer to which depth information will be rendered.
            */
            public SetTargetBuffers ($colorBuffer: System.Array$1<UnityEngine.RenderBuffer>, $depthBuffer: UnityEngine.RenderBuffer) : void
            public ResetWorldToCameraMatrix () : void
            public ResetProjectionMatrix () : void
            /** Calculates and returns oblique near-plane projection matrix.
            * @param clipPlane Vector4 that describes a clip plane.
            * @returns Oblique near-plane projection matrix. 
            */
            public CalculateObliqueMatrix ($clipPlane: UnityEngine.Vector4) : UnityEngine.Matrix4x4
            public WorldToScreenPoint ($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye) : UnityEngine.Vector3
            public WorldToViewportPoint ($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye) : UnityEngine.Vector3
            public ViewportToWorldPoint ($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye) : UnityEngine.Vector3
            public ScreenToWorldPoint ($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye) : UnityEngine.Vector3
            /** Transforms position from world space into screen space. * @param eye Optional argument that can be used to specify which eye transform to use. Default is Mono.
            */
            public WorldToScreenPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms position from world space into viewport space. * @param eye Optional argument that can be used to specify which eye transform to use. Default is Mono.
            */
            public WorldToViewportPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms position from viewport space into world space.
            * @param position The 3d vector in Viewport space.
            * @returns The 3d vector in World space. 
            */
            public ViewportToWorldPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms position from screen space into world space. */
            public ScreenToWorldPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms position from screen space into viewport space. */
            public ScreenToViewportPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Transforms position from viewport space into screen space. */
            public ViewportToScreenPoint ($position: UnityEngine.Vector3) : UnityEngine.Vector3
            public ViewportPointToRay ($pos: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye) : UnityEngine.Ray
            /** Returns a ray going from camera through a viewport point. * @param eye Optional argument that can be used to specify which eye transform to use. Default is Mono.
            */
            public ViewportPointToRay ($pos: UnityEngine.Vector3) : UnityEngine.Ray
            public ScreenPointToRay ($pos: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye) : UnityEngine.Ray
            /** Returns a ray going from camera through a screen point. * @param eye Optional argument that can be used to specify which eye transform to use. Default is Mono.
            */
            public ScreenPointToRay ($pos: UnityEngine.Vector3) : UnityEngine.Ray
            public CalculateFrustumCorners ($viewport: UnityEngine.Rect, $z: number, $eye: UnityEngine.Camera.MonoOrStereoscopicEye, $outCorners: System.Array$1<UnityEngine.Vector3>) : void
            public static CalculateProjectionMatrixFromPhysicalProperties ($output: $Ref<UnityEngine.Matrix4x4>, $focalLength: number, $sensorSize: UnityEngine.Vector2, $lensShift: UnityEngine.Vector2, $nearClip: number, $farClip: number, $gateFitParameters?: UnityEngine.Camera.GateFitParameters) : void
            /** Converts focal length to field of view.
            * @param focalLength Focal length in millimeters.
            * @param sensorSize Sensor size in millimeters. Use the sensor height to get the vertical field of view. Use the sensor width to get the horizontal field of view.
            * @returns field of view in degrees. 
            */
            public static FocalLengthToFOV ($focalLength: number, $sensorSize: number) : number
            /** Converts field of view to focal length. Use either sensor height and vertical field of view or sensor width and horizontal field of view.
            * @param fov field of view in degrees.
            * @param sensorSize Sensor size in millimeters.
            * @returns Focal length in millimeters. 
            */
            public static FOVToFocalLength ($fov: number, $sensorSize: number) : number
            public GetStereoNonJitteredProjectionMatrix ($eye: UnityEngine.Camera.StereoscopicEye) : UnityEngine.Matrix4x4
            public GetStereoViewMatrix ($eye: UnityEngine.Camera.StereoscopicEye) : UnityEngine.Matrix4x4
            public CopyStereoDeviceProjectionMatrixToNonJittered ($eye: UnityEngine.Camera.StereoscopicEye) : void
            public GetStereoProjectionMatrix ($eye: UnityEngine.Camera.StereoscopicEye) : UnityEngine.Matrix4x4
            public SetStereoProjectionMatrix ($eye: UnityEngine.Camera.StereoscopicEye, $matrix: UnityEngine.Matrix4x4) : void
            public ResetStereoProjectionMatrices () : void
            public SetStereoViewMatrix ($eye: UnityEngine.Camera.StereoscopicEye, $matrix: UnityEngine.Matrix4x4) : void
            public ResetStereoViewMatrices () : void
            /** Fills an array of Camera with the current cameras in the Scene, without allocating a new array. * @param cameras An array to be filled up with cameras currently in the Scene.
            */
            public static GetAllCameras ($cameras: System.Array$1<UnityEngine.Camera>) : number
            /** Render into a static cubemap from this camera.
            * @param cubemap The cube map to render to.
            * @param faceMask A bitmask which determines which of the six faces are rendered to.
            * @returns False if rendering fails, else true. 
            */
            public RenderToCubemap ($cubemap: UnityEngine.Cubemap, $faceMask: number) : boolean
            public RenderToCubemap ($cubemap: UnityEngine.Cubemap) : boolean
            /** Render into a cubemap from this camera.
            * @param faceMask A bitfield indicating which cubemap faces should be rendered into.
            * @param cubemap The texture to render to.
            * @returns False if rendering fails, else true. 
            */
            public RenderToCubemap ($cubemap: UnityEngine.RenderTexture, $faceMask: number) : boolean
            public RenderToCubemap ($cubemap: UnityEngine.RenderTexture) : boolean
            public RenderToCubemap ($cubemap: UnityEngine.RenderTexture, $faceMask: number, $stereoEye: UnityEngine.Camera.MonoOrStereoscopicEye) : boolean
            public Render () : void
            /** Render the camera with shader replacement. */
            public RenderWithShader ($shader: UnityEngine.Shader, $replacementTag: string) : void
            public RenderDontRestore () : void
            public static SetupCurrent ($cur: UnityEngine.Camera) : void
            /** Makes this camera's settings match other camera. * @param other Copy camera settings to the other camera.
            */
            public CopyFrom ($other: UnityEngine.Camera) : void
            /** Remove command buffers from execution at a specified place. * @param evt When to execute the command buffer during rendering.
            */
            public RemoveCommandBuffers ($evt: UnityEngine.Rendering.CameraEvent) : void
            public RemoveAllCommandBuffers () : void
            /** Add a command buffer to be executed at a specified place. * @param evt When to execute the command buffer during rendering.
            * @param buffer The buffer to execute.
            */
            public AddCommandBuffer ($evt: UnityEngine.Rendering.CameraEvent, $buffer: UnityEngine.Rendering.CommandBuffer) : void
            /** Adds a command buffer to the GPU's async compute queues and executes that command buffer when graphics processing reaches a given point. * @param evt The point during the graphics processing at which this command buffer should commence on the GPU.
            * @param buffer The buffer to execute.
            * @param queueType The desired async compute queue type to execute the buffer on.
            */
            public AddCommandBufferAsync ($evt: UnityEngine.Rendering.CameraEvent, $buffer: UnityEngine.Rendering.CommandBuffer, $queueType: UnityEngine.Rendering.ComputeQueueType) : void
            /** Remove command buffer from execution at a specified place. * @param evt When to execute the command buffer during rendering.
            * @param buffer The buffer to execute.
            */
            public RemoveCommandBuffer ($evt: UnityEngine.Rendering.CameraEvent, $buffer: UnityEngine.Rendering.CommandBuffer) : void
            /** Get command buffers to be executed at a specified place.
            * @param evt When to execute the command buffer during rendering.
            * @returns Array of command buffers. 
            */
            public GetCommandBuffers ($evt: UnityEngine.Rendering.CameraEvent) : System.Array$1<UnityEngine.Rendering.CommandBuffer>
            public constructor ()
        }
        /** Rendering path of a Camera. */
        enum RenderingPath
        { UsePlayerSettings = -1, VertexLit = 0, Forward = 1, DeferredLighting = 2, DeferredShading = 3 }
        /** Transparent object sorting mode of a Camera. */
        enum TransparencySortMode
        { Default = 0, Perspective = 1, Orthographic = 2, CustomAxis = 3 }
        /** Describes different types of camera. */
        enum CameraType
        { Game = 1, SceneView = 2, Preview = 4, VR = 8, Reflection = 16 }
        /** Values for Camera.clearFlags, determining what to clear when rendering a Camera. */
        enum CameraClearFlags
        { Skybox = 1, Color = 2, SolidColor = 2, Depth = 3, Nothing = 4 }
        /** Depth texture generation mode for Camera. */
        enum DepthTextureMode
        { None = 0, Depth = 1, DepthNormals = 2, MotionVectors = 4 }
        /** A 2D Rectangle defined by X and Y position, width and height. */
        class Rect extends System.ValueType implements System.IEquatable$1<UnityEngine.Rect>
        {
        /** Shorthand for writing new Rect(0,0,0,0). */
            public static get zero(): UnityEngine.Rect;
            /** The X coordinate of the rectangle. */
            public get x(): number;
            public set x(value: number);
            /** The Y coordinate of the rectangle. */
            public get y(): number;
            public set y(value: number);
            /** The X and Y position of the rectangle. */
            public get position(): UnityEngine.Vector2;
            public set position(value: UnityEngine.Vector2);
            /** The position of the center of the rectangle. */
            public get center(): UnityEngine.Vector2;
            public set center(value: UnityEngine.Vector2);
            /** The position of the minimum corner of the rectangle. */
            public get min(): UnityEngine.Vector2;
            public set min(value: UnityEngine.Vector2);
            /** The position of the maximum corner of the rectangle. */
            public get max(): UnityEngine.Vector2;
            public set max(value: UnityEngine.Vector2);
            /** The width of the rectangle, measured from the X position. */
            public get width(): number;
            public set width(value: number);
            /** The height of the rectangle, measured from the Y position. */
            public get height(): number;
            public set height(value: number);
            /** The width and height of the rectangle. */
            public get size(): UnityEngine.Vector2;
            public set size(value: UnityEngine.Vector2);
            /** The minimum X coordinate of the rectangle. */
            public get xMin(): number;
            public set xMin(value: number);
            /** The minimum Y coordinate of the rectangle. */
            public get yMin(): number;
            public set yMin(value: number);
            /** The maximum X coordinate of the rectangle. */
            public get xMax(): number;
            public set xMax(value: number);
            /** The maximum Y coordinate of the rectangle. */
            public get yMax(): number;
            public set yMax(value: number);
            /** Creates a rectangle from min/max coordinate values.
            * @param xmin The minimum X coordinate.
            * @param ymin The minimum Y coordinate.
            * @param xmax The maximum X coordinate.
            * @param ymax The maximum Y coordinate.
            * @returns A rectangle matching the specified coordinates. 
            */
            public static MinMaxRect ($xmin: number, $ymin: number, $xmax: number, $ymax: number) : UnityEngine.Rect
            /** Set components of an existing Rect. */
            public Set ($x: number, $y: number, $width: number, $height: number) : void
            /** Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.
            * @param point Point to test.
            * @param allowInverse Does the test allow the Rect's width and height to be negative?
            * @returns True if the point lies within the specified rectangle. 
            */
            public Contains ($point: UnityEngine.Vector2) : boolean
            /** Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.
            * @param point Point to test.
            * @param allowInverse Does the test allow the Rect's width and height to be negative?
            * @returns True if the point lies within the specified rectangle. 
            */
            public Contains ($point: UnityEngine.Vector3) : boolean
            /** Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the Rect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.
            * @param point Point to test.
            * @param allowInverse Does the test allow the Rect's width and height to be negative?
            * @returns True if the point lies within the specified rectangle. 
            */
            public Contains ($point: UnityEngine.Vector3, $allowInverse: boolean) : boolean
            /** Returns true if the other rectangle overlaps this one. If allowInverse is present and true, the widths and heights of the Rects are allowed to take negative values (ie, the min value is greater than the max), and the test will still work. * @param other Other rectangle to test overlapping with.
            * @param allowInverse Does the test allow the widths and heights of the Rects to be negative?
            */
            public Overlaps ($other: UnityEngine.Rect) : boolean
            /** Returns true if the other rectangle overlaps this one. If allowInverse is present and true, the widths and heights of the Rects are allowed to take negative values (ie, the min value is greater than the max), and the test will still work. * @param other Other rectangle to test overlapping with.
            * @param allowInverse Does the test allow the widths and heights of the Rects to be negative?
            */
            public Overlaps ($other: UnityEngine.Rect, $allowInverse: boolean) : boolean
            /** Returns a point inside a rectangle, given normalized coordinates. * @param rectangle Rectangle to get a point inside.
            * @param normalizedRectCoordinates Normalized coordinates to get a point for.
            */
            public static NormalizedToPoint ($rectangle: UnityEngine.Rect, $normalizedRectCoordinates: UnityEngine.Vector2) : UnityEngine.Vector2
            /** Returns the normalized coordinates cooresponding the the point. * @param rectangle Rectangle to get normalized coordinates inside.
            * @param point A point inside the rectangle to get normalized coordinates for.
            */
            public static PointToNormalized ($rectangle: UnityEngine.Rect, $point: UnityEngine.Vector2) : UnityEngine.Vector2
            public static op_Inequality ($lhs: UnityEngine.Rect, $rhs: UnityEngine.Rect) : boolean
            public static op_Equality ($lhs: UnityEngine.Rect, $rhs: UnityEngine.Rect) : boolean
            public Equals ($other: any) : boolean
            public Equals ($other: UnityEngine.Rect) : boolean
            public ToString () : string
            /** Returns a nicely formatted string for this Rect. */
            public ToString ($format: string) : string
            public constructor ($x: number, $y: number, $width: number, $height: number)
            public constructor ($position: UnityEngine.Vector2, $size: UnityEngine.Vector2)
            public constructor ($source: UnityEngine.Rect)
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
            public constructor ()
        }
        /** Render textures are textures that can be rendered to. */
        class RenderTexture extends UnityEngine.Texture
        {
        /** The width of the render texture in pixels. */
            public get width(): number;
            public set width(value: number);
            /** The height of the render texture in pixels. */
            public get height(): number;
            public set height(value: number);
            /** Dimensionality (type) of the render texture. */
            public get dimension(): UnityEngine.Rendering.TextureDimension;
            public set dimension(value: UnityEngine.Rendering.TextureDimension);
            /** Render texture has mipmaps when this flag is set. */
            public get useMipMap(): boolean;
            public set useMipMap(value: boolean);
            /** Does this render texture use sRGB read/write conversions? (Read Only). */
            public get sRGB(): boolean;
            /** The color format of the render texture. */
            public get format(): UnityEngine.RenderTextureFormat;
            public set format(value: UnityEngine.RenderTextureFormat);
            /** If this RenderTexture is a VR eye texture used in stereoscopic rendering, this property decides what special rendering occurs, if any. */
            public get vrUsage(): UnityEngine.VRTextureUsage;
            public set vrUsage(value: UnityEngine.VRTextureUsage);
            /** The render texture memoryless mode property. */
            public get memorylessMode(): UnityEngine.RenderTextureMemoryless;
            public set memorylessMode(value: UnityEngine.RenderTextureMemoryless);
            /** Mipmap levels are generated automatically when this flag is set. */
            public get autoGenerateMips(): boolean;
            public set autoGenerateMips(value: boolean);
            /** Volume extent of a 3D render texture or number of slices of array texture. */
            public get volumeDepth(): number;
            public set volumeDepth(value: number);
            /** The antialiasing level for the RenderTexture. */
            public get antiAliasing(): number;
            public set antiAliasing(value: number);
            /** If true and antiAliasing is greater than 1, the render texture will not be resolved by default.  Use this if the render texture needs to be bound as a multisampled texture in a shader. */
            public get bindTextureMS(): boolean;
            public set bindTextureMS(value: boolean);
            /** Enable random access write into this render texture on Shader Model 5.0 level shaders. */
            public get enableRandomWrite(): boolean;
            public set enableRandomWrite(value: boolean);
            /** Is the render texture marked to be scaled by the Dynamic Resolution system. */
            public get useDynamicScale(): boolean;
            public set useDynamicScale(value: boolean);
            public get isPowerOfTwo(): boolean;
            public set isPowerOfTwo(value: boolean);
            /** Currently active render texture. */
            public static get active(): UnityEngine.RenderTexture;
            public static set active(value: UnityEngine.RenderTexture);
            /** Color buffer of the render texture (Read Only). */
            public get colorBuffer(): UnityEngine.RenderBuffer;
            /** Depth/stencil buffer of the render texture (Read Only). */
            public get depthBuffer(): UnityEngine.RenderBuffer;
            /** The precision of the render texture's depth buffer in bits (0, 16, 24/32 are supported). */
            public get depth(): number;
            public set depth(value: number);
            /** This struct contains all the information required to create a RenderTexture. It can be copied, cached, and reused to easily create RenderTextures that all share the same properties. */
            public get descriptor(): UnityEngine.RenderTextureDescriptor;
            public set descriptor(value: UnityEngine.RenderTextureDescriptor);
            public GetNativeDepthBufferPtr () : System.IntPtr
            /** Hint the GPU driver that the contents of the RenderTexture will not be used. * @param discardColor Should the colour buffer be discarded?
            * @param discardDepth Should the depth buffer be discarded?
            */
            public DiscardContents ($discardColor: boolean, $discardDepth: boolean) : void
            public MarkRestoreExpected () : void
            public DiscardContents () : void
            public ResolveAntiAliasedSurface () : void
            /** Force an antialiased render texture to be resolved. * @param target The render texture to resolve into.  If set, the target render texture must have the same dimensions and format as the source.
            */
            public ResolveAntiAliasedSurface ($target: UnityEngine.RenderTexture) : void
            /** Assigns this RenderTexture as a global shader property named propertyName. */
            public SetGlobalShaderProperty ($propertyName: string) : void
            public Create () : boolean
            public Release () : void
            public IsCreated () : boolean
            public GenerateMips () : void
            public ConvertToEquirect ($equirect: UnityEngine.RenderTexture, $eye?: UnityEngine.Camera.MonoOrStereoscopicEye) : void
            /** Does a RenderTexture have stencil buffer? * @param rt Render texture, or null for main screen.
            */
            public static SupportsStencil ($rt: UnityEngine.RenderTexture) : boolean
            /** Release a temporary texture allocated with GetTemporary. */
            public static ReleaseTemporary ($temp: UnityEngine.RenderTexture) : void
            /** Allocate a temporary render texture. * @param width Width in pixels.
            * @param height Height in pixels.
            * @param depthBuffer Depth buffer bits (0, 16 or 24). Note that only 24 bit depth has stencil buffer.
            * @param format Render texture format.
            * @param readWrite Color space conversion mode.
            * @param antiAliasing Number of antialiasing samples to store in the texture. Valid values are 1, 2, 4, and 8. Throws an exception if any other value is passed.
            * @param memorylessMode Render texture memoryless mode.
            * @param desc Use this RenderTextureDesc for the settings when creating the temporary RenderTexture.
            */
            public static GetTemporary ($desc: UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
            /** Allocate a temporary render texture. * @param width Width in pixels.
            * @param height Height in pixels.
            * @param depthBuffer Depth buffer bits (0, 16 or 24). Note that only 24 bit depth has stencil buffer.
            * @param format Render texture format.
            * @param readWrite Color space conversion mode.
            * @param antiAliasing Number of antialiasing samples to store in the texture. Valid values are 1, 2, 4, and 8. Throws an exception if any other value is passed.
            * @param memorylessMode Render texture memoryless mode.
            * @param desc Use this RenderTextureDesc for the settings when creating the temporary RenderTexture.
            */
            public static GetTemporary ($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number, $memorylessMode: UnityEngine.RenderTextureMemoryless, $vrUsage: UnityEngine.VRTextureUsage, $useDynamicScale: boolean) : UnityEngine.RenderTexture
            public static GetTemporary ($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number, $memorylessMode: UnityEngine.RenderTextureMemoryless, $vrUsage: UnityEngine.VRTextureUsage) : UnityEngine.RenderTexture
            public static GetTemporary ($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number, $memorylessMode: UnityEngine.RenderTextureMemoryless) : UnityEngine.RenderTexture
            public static GetTemporary ($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number) : UnityEngine.RenderTexture
            public static GetTemporary ($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite) : UnityEngine.RenderTexture
            public static GetTemporary ($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat) : UnityEngine.RenderTexture
            public static GetTemporary ($width: number, $height: number, $depthBuffer: number) : UnityEngine.RenderTexture
            public static GetTemporary ($width: number, $height: number) : UnityEngine.RenderTexture
            public constructor ($desc: UnityEngine.RenderTextureDescriptor)
            public constructor ($textureToCopy: UnityEngine.RenderTexture)
            public constructor ($width: number, $height: number, $depth: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat)
            public constructor ($width: number, $height: number, $depth: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite)
            public constructor ($width: number, $height: number, $depth: number, $format: UnityEngine.RenderTextureFormat)
            public constructor ($width: number, $height: number, $depth: number)
            public constructor ()
        }
        /** Color or depth buffer part of a RenderTexture. */
        class RenderBuffer extends System.ValueType
        {
        }
        /** Enum values for the Camera's targetEye property. */
        enum StereoTargetEyeMask
        { None = 0, Left = 1, Right = 2, Both = 3 }
        /** Class for handling cube maps, Use this to create or modify existing. */
        class Cubemap extends UnityEngine.Texture
        {
        }
        /** This struct contains the view space coordinates of the near projection plane. */
        class FrustumPlanes extends System.ValueType
        {
        }
        /** Representation of a plane in 3D space. */
        class Plane extends System.ValueType
        {
        /** Normal vector of the plane. */
            public get normal(): UnityEngine.Vector3;
            public set normal(value: UnityEngine.Vector3);
            /** Distance from the origin to the plane. */
            public get distance(): number;
            public set distance(value: number);
            /** Returns a copy of the plane that faces in the opposite direction. */
            public get flipped(): UnityEngine.Plane;
            /** Sets a plane using a point that lies within it along with a normal to orient it. * @param inNormal The plane's normal vector.
            * @param inPoint A point that lies on the plane.
            */
            public SetNormalAndPosition ($inNormal: UnityEngine.Vector3, $inPoint: UnityEngine.Vector3) : void
            /** Sets a plane using three points that lie within it.  The points go around clockwise as you look down on the top surface of the plane. * @param a First point in clockwise order.
            * @param b Second point in clockwise order.
            * @param c Third point in clockwise order.
            */
            public Set3Points ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $c: UnityEngine.Vector3) : void
            public Flip () : void
            /** Moves the plane in space by the translation vector. * @param translation The offset in space to move the plane with.
            */
            public Translate ($translation: UnityEngine.Vector3) : void
            /** Returns a copy of the given plane that is moved in space by the given translation.
            * @param plane The plane to move in space.
            * @param translation The offset in space to move the plane with.
            * @returns The translated plane. 
            */
            public static Translate ($plane: UnityEngine.Plane, $translation: UnityEngine.Vector3) : UnityEngine.Plane
            /** For a given point returns the closest point on the plane.
            * @param point The point to project onto the plane.
            * @returns A point on the plane that is closest to point. 
            */
            public ClosestPointOnPlane ($point: UnityEngine.Vector3) : UnityEngine.Vector3
            /** Returns a signed distance from plane to point. */
            public GetDistanceToPoint ($point: UnityEngine.Vector3) : number
            /** Is a point on the positive side of the plane? */
            public GetSide ($point: UnityEngine.Vector3) : boolean
            /** Are two points on the same side of the plane? */
            public SameSide ($inPt0: UnityEngine.Vector3, $inPt1: UnityEngine.Vector3) : boolean
            /** Intersects a ray with the plane. */
            public Raycast ($ray: UnityEngine.Ray, $enter: $Ref<number>) : boolean
            public ToString () : string
            public ToString ($format: string) : string
            public constructor ($inNormal: UnityEngine.Vector3, $inPoint: UnityEngine.Vector3)
            public constructor ($inNormal: UnityEngine.Vector3, $d: number)
            public constructor ($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $c: UnityEngine.Vector3)
            public constructor ()
        }
        enum TexGenMode
        { None = 0, SphereMap = 1, Object = 2, EyeLinear = 3, CubeReflect = 4, CubeNormal = 5 }
        /** Format of a RenderTexture. */
        enum RenderTextureFormat
        { ARGB32 = 0, Depth = 1, ARGBHalf = 2, Shadowmap = 3, RGB565 = 4, ARGB4444 = 5, ARGB1555 = 6, Default = 7, ARGB2101010 = 8, DefaultHDR = 9, ARGB64 = 10, ARGBFloat = 11, RGFloat = 12, RGHalf = 13, RFloat = 14, RHalf = 15, R8 = 16, ARGBInt = 17, RGInt = 18, RInt = 19, BGRA32 = 20, RGB111110Float = 22, RG32 = 23, RGBAUShort = 24, RG16 = 25, BGRA10101010_XR = 26, BGR101010_XR = 27, R16 = 28 }
        /** This enum describes how the RenderTexture is used as a VR eye texture. Instead of using the values of this enum manually, use the value returned by XR.XRSettings.eyeTextureDesc|eyeTextureDesc or other VR functions returning a RenderTextureDescriptor. */
        enum VRTextureUsage
        { None = 0, OneEye = 1, TwoEyes = 2 }
        /** Flags enumeration of the render texture memoryless modes. */
        enum RenderTextureMemoryless
        { None = 0, Color = 1, Depth = 2, MSAA = 4 }
        /** This struct contains all the information required to create a RenderTexture. It can be copied, cached, and reused to easily create RenderTextures that all share the same properties. */
        class RenderTextureDescriptor extends System.ValueType
        {
        }
        /** Color space conversion mode of a RenderTexture. */
        enum RenderTextureReadWrite
        { Default = 0, Linear = 1, sRGB = 2 }
        /** Class containing methods to ease debugging while developing a game. */
        class Debug extends System.Object
        {
        /** Get default debug logger. */
            public static get unityLogger(): UnityEngine.ILogger;
            /** Reports whether the development console is visible. The development console cannot be made to appear using: */
            public static get developerConsoleVisible(): boolean;
            public static set developerConsoleVisible(value: boolean);
            /** In the Build Settings dialog there is a check box called "Development Build". */
            public static get isDebugBuild(): boolean;
            /** Draws a line between specified start and end points. * @param start Point in world space where the line should start.
            * @param end Point in world space where the line should end.
            * @param color Color of the line.
            * @param duration How long the line should be visible for.
            * @param depthTest Should the line be obscured by objects closer to the camera?
            */
            public static DrawLine ($start: UnityEngine.Vector3, $end: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number) : void
            /** Draws a line between specified start and end points. * @param start Point in world space where the line should start.
            * @param end Point in world space where the line should end.
            * @param color Color of the line.
            * @param duration How long the line should be visible for.
            * @param depthTest Should the line be obscured by objects closer to the camera?
            */
            public static DrawLine ($start: UnityEngine.Vector3, $end: UnityEngine.Vector3, $color: UnityEngine.Color) : void
            /** Draws a line between specified start and end points. * @param start Point in world space where the line should start.
            * @param end Point in world space where the line should end.
            * @param color Color of the line.
            * @param duration How long the line should be visible for.
            * @param depthTest Should the line be obscured by objects closer to the camera?
            */
            public static DrawLine ($start: UnityEngine.Vector3, $end: UnityEngine.Vector3) : void
            /** Draws a line between specified start and end points. * @param start Point in world space where the line should start.
            * @param end Point in world space where the line should end.
            * @param color Color of the line.
            * @param duration How long the line should be visible for.
            * @param depthTest Should the line be obscured by objects closer to the camera?
            */
            public static DrawLine ($start: UnityEngine.Vector3, $end: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number, $depthTest: boolean) : void
            /** Draws a line from start to start + dir in world coordinates. * @param start Point in world space where the ray should start.
            * @param dir Direction and length of the ray.
            * @param color Color of the drawn line.
            * @param duration How long the line will be visible for (in seconds).
            * @param depthTest Should the line be obscured by other objects closer to the camera?
            */
            public static DrawRay ($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number) : void
            /** Draws a line from start to start + dir in world coordinates. * @param start Point in world space where the ray should start.
            * @param dir Direction and length of the ray.
            * @param color Color of the drawn line.
            * @param duration How long the line will be visible for (in seconds).
            * @param depthTest Should the line be obscured by other objects closer to the camera?
            */
            public static DrawRay ($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3, $color: UnityEngine.Color) : void
            /** Draws a line from start to start + dir in world coordinates. * @param start Point in world space where the ray should start.
            * @param dir Direction and length of the ray.
            * @param color Color of the drawn line.
            * @param duration How long the line will be visible for (in seconds).
            * @param depthTest Should the line be obscured by other objects closer to the camera?
            */
            public static DrawRay ($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3) : void
            /** Draws a line from start to start + dir in world coordinates. * @param start Point in world space where the ray should start.
            * @param dir Direction and length of the ray.
            * @param color Color of the drawn line.
            * @param duration How long the line will be visible for (in seconds).
            * @param depthTest Should the line be obscured by other objects closer to the camera?
            */
            public static DrawRay ($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number, $depthTest: boolean) : void
            public static Break () : void
            public static DebugBreak () : void
            /** Log a message to the Unity Console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static Log ($message: any) : void
            /** Log a message to the Unity Console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static Log ($message: any, $context: UnityEngine.Object) : void
            /** Logs a formatted message to the Unity Console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogFormat ($format: string, ...args: any[]) : void
            /** Logs a formatted message to the Unity Console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogFormat ($context: UnityEngine.Object, $format: string, ...args: any[]) : void
            /** A variant of Debug.Log that logs an error message to the console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static LogError ($message: any) : void
            /** A variant of Debug.Log that logs an error message to the console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static LogError ($message: any, $context: UnityEngine.Object) : void
            /** Logs a formatted error message to the Unity console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogErrorFormat ($format: string, ...args: any[]) : void
            /** Logs a formatted error message to the Unity console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogErrorFormat ($context: UnityEngine.Object, $format: string, ...args: any[]) : void
            public static ClearDeveloperConsole () : void
            /** A variant of Debug.Log that logs an error message to the console. * @param context Object to which the message applies.
            * @param exception Runtime Exception.
            */
            public static LogException ($exception: System.Exception) : void
            /** A variant of Debug.Log that logs an error message to the console. * @param context Object to which the message applies.
            * @param exception Runtime Exception.
            */
            public static LogException ($exception: System.Exception, $context: UnityEngine.Object) : void
            /** A variant of Debug.Log that logs a warning message to the console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static LogWarning ($message: any) : void
            /** A variant of Debug.Log that logs a warning message to the console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static LogWarning ($message: any, $context: UnityEngine.Object) : void
            /** Logs a formatted warning message to the Unity Console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogWarningFormat ($format: string, ...args: any[]) : void
            /** Logs a formatted warning message to the Unity Console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogWarningFormat ($context: UnityEngine.Object, $format: string, ...args: any[]) : void
            /** Assert a condition and logs an error message to the Unity console on failure. * @param condition Condition you expect to be true.
            * @param context Object to which the message applies.
            * @param message String or object to be converted to string representation for display.
            */
            public static Assert ($condition: boolean) : void
            /** Assert a condition and logs an error message to the Unity console on failure. * @param condition Condition you expect to be true.
            * @param context Object to which the message applies.
            * @param message String or object to be converted to string representation for display.
            */
            public static Assert ($condition: boolean, $context: UnityEngine.Object) : void
            /** Assert a condition and logs an error message to the Unity console on failure. * @param condition Condition you expect to be true.
            * @param context Object to which the message applies.
            * @param message String or object to be converted to string representation for display.
            */
            public static Assert ($condition: boolean, $message: any) : void
            public static Assert ($condition: boolean, $message: string) : void
            /** Assert a condition and logs an error message to the Unity console on failure. * @param condition Condition you expect to be true.
            * @param context Object to which the message applies.
            * @param message String or object to be converted to string representation for display.
            */
            public static Assert ($condition: boolean, $message: any, $context: UnityEngine.Object) : void
            public static Assert ($condition: boolean, $message: string, $context: UnityEngine.Object) : void
            /** Assert a condition and logs a formatted error message to the Unity console on failure. * @param condition Condition you expect to be true.
            * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static AssertFormat ($condition: boolean, $format: string, ...args: any[]) : void
            /** Assert a condition and logs a formatted error message to the Unity console on failure. * @param condition Condition you expect to be true.
            * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static AssertFormat ($condition: boolean, $context: UnityEngine.Object, $format: string, ...args: any[]) : void
            /** A variant of Debug.Log that logs an assertion message to the console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static LogAssertion ($message: any) : void
            /** A variant of Debug.Log that logs an assertion message to the console. * @param message String or object to be converted to string representation for display.
            * @param context Object to which the message applies.
            */
            public static LogAssertion ($message: any, $context: UnityEngine.Object) : void
            /** Logs a formatted assertion message to the Unity console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogAssertionFormat ($format: string, ...args: any[]) : void
            /** Logs a formatted assertion message to the Unity console. * @param format A composite format string.
            * @param args Format arguments.
            * @param context Object to which the message applies.
            */
            public static LogAssertionFormat ($context: UnityEngine.Object, $format: string, ...args: any[]) : void
            public constructor ()
        }
        interface ILogger extends UnityEngine.ILogHandler
        {
        }
        interface ILogHandler
        {
        }
        /** Provides access to a display / screen for rendering operations. */
        class Display extends System.Object
        {
        /** The list of currently connected Displays. Contains at least one (main) display. */
            public static displays : System.Array$1<UnityEngine.Display>/** Horizontal resolution that the display is rendering at. */
            public get renderingWidth(): number;
            /** Vertical resolution that the display is rendering at. */
            public get renderingHeight(): number;
            /** Horizontal native display resolution. */
            public get systemWidth(): number;
            /** Vertical native display resolution. */
            public get systemHeight(): number;
            /** Color RenderBuffer. */
            public get colorBuffer(): UnityEngine.RenderBuffer;
            /** Depth RenderBuffer. */
            public get depthBuffer(): UnityEngine.RenderBuffer;
            /** Gets the state of the display and returns true if the display is active and false if otherwise. */
            public get active(): boolean;
            /** Main Display. */
            public static get main(): UnityEngine.Display;
            public Activate () : void
            /** This overloaded function available for Windows allows specifying desired Window Width, Height and Refresh Rate. * @param width Desired Width of the Window (for Windows only. On Linux and Mac uses Screen Width).
            * @param height Desired Height of the Window (for Windows only. On Linux and Mac uses Screen Height).
            * @param refreshRate Desired Refresh Rate.
            */
            public Activate ($width: number, $height: number, $refreshRate: number) : void
            /** Set rendering size and position on screen (Windows only). * @param width Change Window Width (Windows Only).
            * @param height Change Window Height (Windows Only).
            * @param x Change Window Position X (Windows Only).
            * @param y Change Window Position Y (Windows Only).
            */
            public SetParams ($width: number, $height: number, $x: number, $y: number) : void
            /** Sets rendering resolution for the display. * @param w Rendering width in pixels.
            * @param h Rendering height in pixels.
            */
            public SetRenderingResolution ($w: number, $h: number) : void
            /** Query relative mouse coordinates. * @param inputMouseCoordinates Mouse Input Position as Coordinates.
            */
            public static RelativeMouseAt ($inputMouseCoordinates: UnityEngine.Vector3) : UnityEngine.Vector3
            public static add_onDisplaysUpdated ($value: UnityEngine.Display.DisplaysUpdatedDelegate) : void
            public static remove_onDisplaysUpdated ($value: UnityEngine.Display.DisplaysUpdatedDelegate) : void
        }
        /** Gradient used for animating colors. */
        class Gradient extends System.Object implements System.IEquatable$1<UnityEngine.Gradient>
        {
        /** All color keys defined in the gradient. */
            public get colorKeys(): System.Array$1<UnityEngine.GradientColorKey>;
            public set colorKeys(value: System.Array$1<UnityEngine.GradientColorKey>);
            /** All alpha keys defined in the gradient. */
            public get alphaKeys(): System.Array$1<UnityEngine.GradientAlphaKey>;
            public set alphaKeys(value: System.Array$1<UnityEngine.GradientAlphaKey>);
            /** Control how the gradient is evaluated. */
            public get mode(): UnityEngine.GradientMode;
            public set mode(value: UnityEngine.GradientMode);
            /** Calculate color at a given time. * @param time Time of the key (0 - 1).
            */
            public Evaluate ($time: number) : UnityEngine.Color
            /** Setup Gradient with an array of color keys and alpha keys. * @param colorKeys Color keys of the gradient (maximum 8 color keys).
            * @param alphaKeys Alpha keys of the gradient (maximum 8 alpha keys).
            */
            public SetKeys ($colorKeys: System.Array$1<UnityEngine.GradientColorKey>, $alphaKeys: System.Array$1<UnityEngine.GradientAlphaKey>) : void
            public Equals ($o: any) : boolean
            public Equals ($other: UnityEngine.Gradient) : boolean
            public constructor ()
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
        }
        /** Color key used by Gradient. */
        class GradientColorKey extends System.ValueType
        {
        }
        /** Alpha key used by Gradient. */
        class GradientAlphaKey extends System.ValueType
        {
        }
        /** Select how gradients will be evaluated. */
        enum GradientMode
        { Blend = 0, Fixed = 1 }
        /** Access to display information. */
        class Screen extends System.Object
        {
        /** The current width of the screen window in pixels (Read Only). */
            public static get width(): number;
            /** The current height of the screen window in pixels (Read Only). */
            public static get height(): number;
            /** The current DPI of the screen / device (Read Only). */
            public static get dpi(): number;
            /** Specifies logical orientation of the screen. */
            public static get orientation(): UnityEngine.ScreenOrientation;
            public static set orientation(value: UnityEngine.ScreenOrientation);
            /** A power saving setting, allowing the screen to dim some time after the last active user interaction. */
            public static get sleepTimeout(): number;
            public static set sleepTimeout(value: number);
            /** Allow auto-rotation to portrait? */
            public static get autorotateToPortrait(): boolean;
            public static set autorotateToPortrait(value: boolean);
            /** Allow auto-rotation to portrait, upside down? */
            public static get autorotateToPortraitUpsideDown(): boolean;
            public static set autorotateToPortraitUpsideDown(value: boolean);
            /** Allow auto-rotation to landscape left? */
            public static get autorotateToLandscapeLeft(): boolean;
            public static set autorotateToLandscapeLeft(value: boolean);
            /** Allow auto-rotation to landscape right? */
            public static get autorotateToLandscapeRight(): boolean;
            public static set autorotateToLandscapeRight(value: boolean);
            /** The current screen resolution (Read Only). */
            public static get currentResolution(): UnityEngine.Resolution;
            /** Is the game running full-screen? */
            public static get fullScreen(): boolean;
            public static set fullScreen(value: boolean);
            /** Set this property to one of the values in FullScreenMode to change the display mode of your application. */
            public static get fullScreenMode(): UnityEngine.FullScreenMode;
            public static set fullScreenMode(value: UnityEngine.FullScreenMode);
            /** Returns the safe area of the screen in pixels (Read Only). */
            public static get safeArea(): UnityEngine.Rect;
            /** All full-screen resolutions supported by the monitor (Read Only). */
            public static get resolutions(): System.Array$1<UnityEngine.Resolution>;
            /** Switches the screen resolution. */
            public static SetResolution ($width: number, $height: number, $fullscreenMode: UnityEngine.FullScreenMode, $preferredRefreshRate: number) : void
            public static SetResolution ($width: number, $height: number, $fullscreenMode: UnityEngine.FullScreenMode) : void
            /** Switches the screen resolution. */
            public static SetResolution ($width: number, $height: number, $fullscreen: boolean, $preferredRefreshRate: number) : void
            /** Switches the screen resolution. */
            public static SetResolution ($width: number, $height: number, $fullscreen: boolean) : void
            public constructor ()
        }
        /** Describes screen orientation. */
        enum ScreenOrientation
        { Unknown = 0, Portrait = 1, PortraitUpsideDown = 2, LandscapeLeft = 3, LandscapeRight = 4, AutoRotation = 5, Landscape = 3 }
        /** Represents a display resolution. */
        class Resolution extends System.ValueType
        {
        }
        /** Platform agnostic fullscreen mode. Not all platforms support all modes. */
        enum FullScreenMode
        { ExclusiveFullScreen = 0, FullScreenWindow = 1, MaximizedWindow = 2, Windowed = 3 }
        /** Raw interface to Unity's drawing functions. */
        class Graphics extends System.Object
        {
        /** Returns the currently active color gamut. */
            public static get activeColorGamut(): UnityEngine.ColorGamut;
            /** Graphics Tier classification for current device.
            Changing this value affects any subsequently loaded shaders. Initially this value is auto-detected from the hardware in use. */
            public static get activeTier(): UnityEngine.Rendering.GraphicsTier;
            public static set activeTier(value: UnityEngine.Rendering.GraphicsTier);
            /** Currently active color buffer (Read Only). */
            public static get activeColorBuffer(): UnityEngine.RenderBuffer;
            /** Currently active depth/stencil buffer (Read Only). */
            public static get activeDepthBuffer(): UnityEngine.RenderBuffer;
            public static ClearRandomWriteTargets () : void
            /** Execute a command buffer. * @param buffer The buffer to execute.
            */
            public static ExecuteCommandBuffer ($buffer: UnityEngine.Rendering.CommandBuffer) : void
            /** Executes a command buffer on an async compute queue with the queue selected based on the ComputeQueueType parameter passed.
            It is required that all of the commands within the command buffer be of a type suitable for execution on the async compute queues. If the buffer contains any commands that are not appropriate then an error will be logged and displayed in the editor window.  Specifically the following commands are permitted in a CommandBuffer intended for async execution:
            CommandBuffer.BeginSample
            CommandBuffer.CopyCounterValue
            CommandBuffer.CopyTexture
            CommandBuffer.CreateGPUFence
            CommandBuffer.DispatchCompute
            CommandBuffer.EndSample
            CommandBuffer.IssuePluginEvent
            CommandBuffer.SetComputeBufferParam
            CommandBuffer.SetComputeFloatParam
            CommandBuffer.SetComputeFloatParams
            CommandBuffer.SetComputeTextureParam
            CommandBuffer.SetComputeVectorParam
            CommandBuffer.WaitOnGPUFence
            All of the commands within the buffer are guaranteed to be executed on the same queue. If the target platform does not support async compute queues then the work is dispatched on the graphics queue. * @param buffer The CommandBuffer to be executed.
            * @param queueType Describes the desired async compute queue the suuplied CommandBuffer should be executed on.
            */
            public static ExecuteCommandBufferAsync ($buffer: UnityEngine.Rendering.CommandBuffer, $queueType: UnityEngine.Rendering.ComputeQueueType) : void
            /** Sets current render target. * @param rt RenderTexture to set as active render target.
            * @param mipLevel Mipmap level to render into (use 0 if not mipmapped).
            * @param face Cubemap face to render into (use Unknown if not a cubemap).
            * @param depthSlice Depth slice to render into (use 0 if not a 3D or 2DArray render target).
            * @param colorBuffer Color buffer to render into.
            * @param depthBuffer Depth buffer to render into.
            * @param colorBuffers Color buffers to render into (for multiple render target effects).
            * @param setup Full render target setup information.
            */
            public static SetRenderTarget ($rt: UnityEngine.RenderTexture, $mipLevel: number, $face: UnityEngine.CubemapFace, $depthSlice: number) : void
            /** Sets current render target. * @param rt RenderTexture to set as active render target.
            * @param mipLevel Mipmap level to render into (use 0 if not mipmapped).
            * @param face Cubemap face to render into (use Unknown if not a cubemap).
            * @param depthSlice Depth slice to render into (use 0 if not a 3D or 2DArray render target).
            * @param colorBuffer Color buffer to render into.
            * @param depthBuffer Depth buffer to render into.
            * @param colorBuffers Color buffers to render into (for multiple render target effects).
            * @param setup Full render target setup information.
            */
            public static SetRenderTarget ($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer, $mipLevel: number, $face: UnityEngine.CubemapFace, $depthSlice: number) : void
            /** Sets current render target. * @param rt RenderTexture to set as active render target.
            * @param mipLevel Mipmap level to render into (use 0 if not mipmapped).
            * @param face Cubemap face to render into (use Unknown if not a cubemap).
            * @param depthSlice Depth slice to render into (use 0 if not a 3D or 2DArray render target).
            * @param colorBuffer Color buffer to render into.
            * @param depthBuffer Depth buffer to render into.
            * @param colorBuffers Color buffers to render into (for multiple render target effects).
            * @param setup Full render target setup information.
            */
            public static SetRenderTarget ($colorBuffers: System.Array$1<UnityEngine.RenderBuffer>, $depthBuffer: UnityEngine.RenderBuffer) : void
            /** Sets current render target. * @param rt RenderTexture to set as active render target.
            * @param mipLevel Mipmap level to render into (use 0 if not mipmapped).
            * @param face Cubemap face to render into (use Unknown if not a cubemap).
            * @param depthSlice Depth slice to render into (use 0 if not a 3D or 2DArray render target).
            * @param colorBuffer Color buffer to render into.
            * @param depthBuffer Depth buffer to render into.
            * @param colorBuffers Color buffers to render into (for multiple render target effects).
            * @param setup Full render target setup information.
            */
            public static SetRenderTarget ($setup: UnityEngine.RenderTargetSetup) : void
            /** Set random write target for level pixel shaders. * @param index Index of the random write target in the shader.
            * @param uav RenderTexture to set as write target.
            * @param preserveCounterValue Whether to leave the append/consume counter value unchanged.
            */
            public static SetRandomWriteTarget ($index: number, $uav: UnityEngine.RenderTexture) : void
            /** Set random write target for level pixel shaders. * @param index Index of the random write target in the shader.
            * @param uav RenderTexture to set as write target.
            * @param preserveCounterValue Whether to leave the append/consume counter value unchanged.
            */
            public static SetRandomWriteTarget ($index: number, $uav: UnityEngine.ComputeBuffer, $preserveCounterValue: boolean) : void
            /** Copy texture contents. * @param src Source texture.
            * @param dst Destination texture.
            * @param srcElement Source texture element (cubemap face, texture array layer or 3D texture depth slice).
            * @param srcMip Source texture mipmap level.
            * @param dstElement Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
            * @param dstMip Destination texture mipmap level.
            * @param srcX X coordinate of source texture region to copy (left side is zero).
            * @param srcY Y coordinate of source texture region to copy (bottom is zero).
            * @param srcWidth Width of source texture region to copy.
            * @param srcHeight Height of source texture region to copy.
            * @param dstX X coordinate of where to copy region in destination texture (left side is zero).
            * @param dstY Y coordinate of where to copy region in destination texture (bottom is zero).
            */
            public static CopyTexture ($src: UnityEngine.Texture, $dst: UnityEngine.Texture) : void
            public static CopyTexture ($src: UnityEngine.Texture, $srcElement: number, $dst: UnityEngine.Texture, $dstElement: number) : void
            /** Copy texture contents. * @param src Source texture.
            * @param dst Destination texture.
            * @param srcElement Source texture element (cubemap face, texture array layer or 3D texture depth slice).
            * @param srcMip Source texture mipmap level.
            * @param dstElement Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
            * @param dstMip Destination texture mipmap level.
            * @param srcX X coordinate of source texture region to copy (left side is zero).
            * @param srcY Y coordinate of source texture region to copy (bottom is zero).
            * @param srcWidth Width of source texture region to copy.
            * @param srcHeight Height of source texture region to copy.
            * @param dstX X coordinate of where to copy region in destination texture (left side is zero).
            * @param dstY Y coordinate of where to copy region in destination texture (bottom is zero).
            */
            public static CopyTexture ($src: UnityEngine.Texture, $srcElement: number, $srcMip: number, $dst: UnityEngine.Texture, $dstElement: number, $dstMip: number) : void
            /** Copy texture contents. * @param src Source texture.
            * @param dst Destination texture.
            * @param srcElement Source texture element (cubemap face, texture array layer or 3D texture depth slice).
            * @param srcMip Source texture mipmap level.
            * @param dstElement Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
            * @param dstMip Destination texture mipmap level.
            * @param srcX X coordinate of source texture region to copy (left side is zero).
            * @param srcY Y coordinate of source texture region to copy (bottom is zero).
            * @param srcWidth Width of source texture region to copy.
            * @param srcHeight Height of source texture region to copy.
            * @param dstX X coordinate of where to copy region in destination texture (left side is zero).
            * @param dstY Y coordinate of where to copy region in destination texture (bottom is zero).
            */
            public static CopyTexture ($src: UnityEngine.Texture, $srcElement: number, $srcMip: number, $srcX: number, $srcY: number, $srcWidth: number, $srcHeight: number, $dst: UnityEngine.Texture, $dstElement: number, $dstMip: number, $dstX: number, $dstY: number) : void
            /** This function provides an efficient way to convert between textures of different formats and dimensions.
            The destination texture format should be uncompressed and correspond to a supported RenderTextureFormat.
            * @param src Source texture.
            * @param dst Destination texture.
            * @param srcElement Source element (e.g. cubemap face).  Set this to 0 for 2d source textures.
            * @param dstElement Destination element (e.g. cubemap face or texture array element).
            * @returns True if the call succeeded. 
            */
            public static ConvertTexture ($src: UnityEngine.Texture, $dst: UnityEngine.Texture) : boolean
            /** This function provides an efficient way to convert between textures of different formats and dimensions.
            The destination texture format should be uncompressed and correspond to a supported RenderTextureFormat.
            * @param src Source texture.
            * @param dst Destination texture.
            * @param srcElement Source element (e.g. cubemap face).  Set this to 0 for 2d source textures.
            * @param dstElement Destination element (e.g. cubemap face or texture array element).
            * @returns True if the call succeeded. 
            */
            public static ConvertTexture ($src: UnityEngine.Texture, $srcElement: number, $dst: UnityEngine.Texture, $dstElement: number) : boolean
            /** Creates a GPUFence which will be passed after the last Blit, Clear, Draw, Dispatch or Texture Copy command prior to this call has been completed on the GPU.
            * @param stage On some platforms there is a significant gap between the vertex processing completing and the pixel processing begining for a given draw call. This parameter allows for the fence to be passed after either the vertex or pixel processing for the proceeding draw has completed. If a compute shader dispatch was the last task submitted then this parameter is ignored.
            * @returns Returns a new GPUFence. 
            */
            public static CreateGPUFence ($stage: UnityEngine.Rendering.SynchronisationStage) : UnityEngine.Rendering.GPUFence
            /** Instructs the GPU's processing of the graphics queue to wait until the given GPUFence is passed. * @param fence The GPUFence that the GPU will be instructed to wait upon before proceeding with its processing of the graphics queue.
            * @param stage On some platforms there is a significant gap between the vertex processing completing and the pixel processing begining for a given draw call. This parameter allows for requested wait to be before the next items vertex or pixel processing begins. If a compute shader dispatch is the next item to be submitted then this parameter is ignored.
            */
            public static WaitOnGPUFence ($fence: UnityEngine.Rendering.GPUFence, $stage: UnityEngine.Rendering.SynchronisationStage) : void
            public static CreateGPUFence () : UnityEngine.Rendering.GPUFence
            public static WaitOnGPUFence ($fence: UnityEngine.Rendering.GPUFence) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $color: UnityEngine.Color, $mat: UnityEngine.Material, $pass: number) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material, $pass: number) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material, $pass: number) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $mat: UnityEngine.Material, $pass: number) : void
            /** Draw a mesh immediately. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.
            * @param materialIndex Subset of the mesh to draw.
            */
            public static DrawMeshNow ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $materialIndex: number) : void
            /** Draw a mesh immediately. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.
            * @param materialIndex Subset of the mesh to draw.
            */
            public static DrawMeshNow ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $materialIndex: number) : void
            /** Draw a mesh immediately. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.
            * @param materialIndex Subset of the mesh to draw.
            */
            public static DrawMeshNow ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion) : void
            /** Draw a mesh immediately. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.
            * @param materialIndex Subset of the mesh to draw.
            */
            public static DrawMeshNow ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4) : void
            /** Draw a mesh. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations).
            * @param material Material to use.
            * @param layer  to use.
            * @param camera If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
            * @param submeshIndex Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
            * @param properties Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.
            * @param castShadows Determines whether the mesh can cast shadows.
            * @param receiveShadows Determines whether the mesh can receive shadows.
            * @param useLightProbes Should the mesh use light probes?
            * @param probeAnchor If used, the mesh will use this Transform's position to sample light probes and find the matching reflection probe.
            * @param lightProbeUsage LightProbeUsage for the mesh.
            */
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean, $useLightProbes: boolean) : void
            /** Draw a mesh. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations).
            * @param material Material to use.
            * @param layer  to use.
            * @param camera If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
            * @param submeshIndex Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
            * @param properties Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.
            * @param castShadows Determines whether the mesh can cast shadows.
            * @param receiveShadows Determines whether the mesh can receive shadows.
            * @param useLightProbes Should the mesh use light probes?
            * @param probeAnchor If used, the mesh will use this Transform's position to sample light probes and find the matching reflection probe.
            * @param lightProbeUsage LightProbeUsage for the mesh.
            */
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $useLightProbes: boolean) : void
            /** Draw a mesh. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations).
            * @param material Material to use.
            * @param layer  to use.
            * @param camera If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
            * @param submeshIndex Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
            * @param properties Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.
            * @param castShadows Determines whether the mesh can cast shadows.
            * @param receiveShadows Determines whether the mesh can receive shadows.
            * @param useLightProbes Should the mesh use light probes?
            * @param probeAnchor If used, the mesh will use this Transform's position to sample light probes and find the matching reflection probe.
            * @param lightProbeUsage LightProbeUsage for the mesh.
            */
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean, $useLightProbes: boolean) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume) : void
            /** Draw the same mesh multiple times using GPU instancing. * @param mesh The Mesh to draw.
            * @param submeshIndex Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
            * @param material Material to use.
            * @param matrices The array of object transformation matrices.
            * @param count The number of instances to be drawn.
            * @param properties Additional material properties to apply. See MaterialPropertyBlock.
            * @param castShadows Should the meshes cast shadows?
            * @param receiveShadows Should the meshes receive shadows?
            * @param layer  to use.
            * @param camera If null (default), the mesh will be drawn in all cameras. Otherwise it will be drawn in the given camera only.
            * @param lightProbeUsage LightProbeUsage for the instances.
            */
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume) : void
            /** Draw the same mesh multiple times using GPU instancing. * @param mesh The Mesh to draw.
            * @param submeshIndex Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
            * @param material Material to use.
            * @param bounds The bounding volume surrounding the instances you intend to draw.
            * @param bufferWithArgs The GPU buffer containing the arguments for how many instances of this mesh to draw.
            * @param argsOffset The byte offset into the buffer, where the draw arguments start.
            * @param properties Additional material properties to apply. See MaterialPropertyBlock.
            * @param castShadows Determines whether the mesh can cast shadows.
            * @param receiveShadows Determines whether the mesh can receive shadows.
            * @param layer  to use.
            * @param camera If null (default), the mesh will be drawn in all cameras. Otherwise it will be drawn in the given camera only.
            * @param lightProbeUsage LightProbeUsage for the instances.
            */
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume) : void
            /** Draws a fully procedural geometry on the GPU. */
            public static DrawProcedural ($topology: UnityEngine.MeshTopology, $vertexCount: number, $instanceCount: number) : void
            /** Draws a fully procedural geometry on the GPU. * @param topology Topology of the procedural geometry.
            * @param bufferWithArgs Buffer with draw arguments.
            * @param argsOffset Byte offset where in the buffer the draw arguments are.
            */
            public static DrawProceduralIndirect ($topology: UnityEngine.MeshTopology, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number) : void
            /** Copies source texture into destination render texture with a shader. * @param source Source texture.
            * @param dest The destination RenderTexture. Set this to null to blit directly to screen. See description for more information.
            * @param mat Material to use. Material's shader could do some post-processing effect, for example.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            * @param offset Offset applied to the source texture coordinate.
            * @param scale Scale applied to the source texture coordinate.
            */
            public static Blit ($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture) : void
            /** Copies source texture into destination render texture with a shader. * @param source Source texture.
            * @param dest The destination RenderTexture. Set this to null to blit directly to screen. See description for more information.
            * @param mat Material to use. Material's shader could do some post-processing effect, for example.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            * @param offset Offset applied to the source texture coordinate.
            * @param scale Scale applied to the source texture coordinate.
            */
            public static Blit ($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $scale: UnityEngine.Vector2, $offset: UnityEngine.Vector2) : void
            /** Copies source texture into destination render texture with a shader. * @param source Source texture.
            * @param dest The destination RenderTexture. Set this to null to blit directly to screen. See description for more information.
            * @param mat Material to use. Material's shader could do some post-processing effect, for example.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            * @param offset Offset applied to the source texture coordinate.
            * @param scale Scale applied to the source texture coordinate.
            */
            public static Blit ($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $mat: UnityEngine.Material, $pass: number) : void
            public static Blit ($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $mat: UnityEngine.Material) : void
            /** Copies source texture into destination render texture with a shader. * @param source Source texture.
            * @param dest The destination RenderTexture. Set this to null to blit directly to screen. See description for more information.
            * @param mat Material to use. Material's shader could do some post-processing effect, for example.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            * @param offset Offset applied to the source texture coordinate.
            * @param scale Scale applied to the source texture coordinate.
            */
            public static Blit ($source: UnityEngine.Texture, $mat: UnityEngine.Material, $pass: number) : void
            public static Blit ($source: UnityEngine.Texture, $mat: UnityEngine.Material) : void
            /** Copies source texture into destination, for multi-tap shader. * @param source Source texture.
            * @param dest Destination RenderTexture, or null to blit directly to screen.
            * @param mat Material to use for copying. Material's shader should do some post-processing effect.
            * @param offsets Variable number of filtering offsets. Offsets are given in pixels.
            */
            public static BlitMultiTap ($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $mat: UnityEngine.Material, ...offsets: UnityEngine.Vector2[]) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean) : void
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform) : void
            /** Draw a mesh. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations).
            * @param material Material to use.
            * @param layer  to use.
            * @param camera If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
            * @param submeshIndex Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
            * @param properties Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.
            * @param castShadows Determines whether the mesh can cast shadows.
            * @param receiveShadows Determines whether the mesh can receive shadows.
            * @param useLightProbes Should the mesh use light probes?
            * @param probeAnchor If used, the mesh will use this Transform's position to sample light probes and find the matching reflection probe.
            * @param lightProbeUsage LightProbeUsage for the mesh.
            */
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $useLightProbes: boolean) : void
            /** Draw a mesh. * @param mesh The Mesh to draw.
            * @param position Position of the mesh.
            * @param rotation Rotation of the mesh.
            * @param matrix Transformation matrix of the mesh (combines position, rotation and other transformations).
            * @param material Material to use.
            * @param layer  to use.
            * @param camera If null (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
            * @param submeshIndex Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
            * @param properties Additional material properties to apply onto material just before this mesh will be drawn. See MaterialPropertyBlock.
            * @param castShadows Determines whether the mesh can cast shadows.
            * @param receiveShadows Determines whether the mesh can receive shadows.
            * @param useLightProbes Should the mesh use light probes?
            * @param probeAnchor If used, the mesh will use this Transform's position to sample light probes and find the matching reflection probe.
            * @param lightProbeUsage LightProbeUsage for the mesh.
            */
            public static DrawMesh ($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera) : void
            public static DrawMeshInstanced ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera) : void
            public static DrawMeshInstancedIndirect ($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $color: UnityEngine.Color, $mat: UnityEngine.Material) : void
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $color: UnityEngine.Color) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material) : void
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material) : void
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number) : void
            /** Draw a texture in screen coordinates. * @param screenRect Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
            * @param texture Texture to draw.
            * @param sourceRect Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
            * @param leftBorder Number of pixels from the left that are not affected by scale.
            * @param rightBorder Number of pixels from the right that are not affected by scale.
            * @param topBorder Number of pixels from the top that are not affected by scale.
            * @param bottomBorder Number of pixels from the bottom that are not affected by scale.
            * @param color Color that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
            * @param mat Custom Material that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
            * @param pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
            */
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $mat: UnityEngine.Material) : void
            public static DrawTexture ($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture) : void
            public static DrawProcedural ($topology: UnityEngine.MeshTopology, $vertexCount: number) : void
            public static DrawProceduralIndirect ($topology: UnityEngine.MeshTopology, $bufferWithArgs: UnityEngine.ComputeBuffer) : void
            public static SetRenderTarget ($rt: UnityEngine.RenderTexture) : void
            public static SetRenderTarget ($rt: UnityEngine.RenderTexture, $mipLevel: number) : void
            public static SetRenderTarget ($rt: UnityEngine.RenderTexture, $mipLevel: number, $face: UnityEngine.CubemapFace) : void
            public static SetRenderTarget ($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer) : void
            public static SetRenderTarget ($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer, $mipLevel: number) : void
            public static SetRenderTarget ($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer, $mipLevel: number, $face: UnityEngine.CubemapFace) : void
            public static SetRandomWriteTarget ($index: number, $uav: UnityEngine.ComputeBuffer) : void
            public constructor ()
        }
        /** Represents a color gamut. */
        enum ColorGamut
        { sRGB = 0, Rec709 = 1, Rec2020 = 2, DisplayP3 = 3, HDR10 = 4, DolbyHDR = 5 }
        /** Cubemap face. */
        enum CubemapFace
        { Unknown = -1, PositiveX = 0, NegativeX = 1, PositiveY = 2, NegativeY = 3, PositiveZ = 4, NegativeZ = 5 }
        /** Fully describes setup of RenderTarget. */
        class RenderTargetSetup extends System.ValueType
        {
        }
        /** A class that allows creating or modifying meshes from scripts. */
        class Mesh extends UnityEngine.Object
        {
        /** Format of the mesh index buffer data. */
            public get indexFormat(): UnityEngine.Rendering.IndexFormat;
            public set indexFormat(value: UnityEngine.Rendering.IndexFormat);
            /** Gets the number of vertex buffers present in the Mesh. (Read Only) */
            public get vertexBufferCount(): number;
            /** Returns BlendShape count on this mesh. */
            public get blendShapeCount(): number;
            /** The bone weights of each vertex. */
            public get boneWeights(): System.Array$1<UnityEngine.BoneWeight>;
            public set boneWeights(value: System.Array$1<UnityEngine.BoneWeight>);
            /** The bind poses. The bind pose at each index refers to the bone with the same index. */
            public get bindposes(): System.Array$1<UnityEngine.Matrix4x4>;
            public set bindposes(value: System.Array$1<UnityEngine.Matrix4x4>);
            /** Returns true if the Mesh is read/write enabled, or false if it is not. */
            public get isReadable(): boolean;
            /** Returns the number of vertices in the Mesh (Read Only). */
            public get vertexCount(): number;
            /** The number of sub-meshes inside the Mesh object. */
            public get subMeshCount(): number;
            public set subMeshCount(value: number);
            /** The bounding volume of the mesh. */
            public get bounds(): UnityEngine.Bounds;
            public set bounds(value: UnityEngine.Bounds);
            /** Returns a copy of the vertex positions or assigns a new vertex positions array. */
            public get vertices(): System.Array$1<UnityEngine.Vector3>;
            public set vertices(value: System.Array$1<UnityEngine.Vector3>);
            /** The normals of the Mesh. */
            public get normals(): System.Array$1<UnityEngine.Vector3>;
            public set normals(value: System.Array$1<UnityEngine.Vector3>);
            /** The tangents of the Mesh. */
            public get tangents(): System.Array$1<UnityEngine.Vector4>;
            public set tangents(value: System.Array$1<UnityEngine.Vector4>);
            /** The base texture coordinates of the Mesh. */
            public get uv(): System.Array$1<UnityEngine.Vector2>;
            public set uv(value: System.Array$1<UnityEngine.Vector2>);
            /** The second texture coordinate set of the mesh, if present. */
            public get uv2(): System.Array$1<UnityEngine.Vector2>;
            public set uv2(value: System.Array$1<UnityEngine.Vector2>);
            /** The third texture coordinate set of the mesh, if present. */
            public get uv3(): System.Array$1<UnityEngine.Vector2>;
            public set uv3(value: System.Array$1<UnityEngine.Vector2>);
            /** The fourth texture coordinate set of the mesh, if present. */
            public get uv4(): System.Array$1<UnityEngine.Vector2>;
            public set uv4(value: System.Array$1<UnityEngine.Vector2>);
            /** The fifth texture coordinate set of the mesh, if present. */
            public get uv5(): System.Array$1<UnityEngine.Vector2>;
            public set uv5(value: System.Array$1<UnityEngine.Vector2>);
            /** The sixth texture coordinate set of the mesh, if present. */
            public get uv6(): System.Array$1<UnityEngine.Vector2>;
            public set uv6(value: System.Array$1<UnityEngine.Vector2>);
            /** The seventh texture coordinate set of the mesh, if present. */
            public get uv7(): System.Array$1<UnityEngine.Vector2>;
            public set uv7(value: System.Array$1<UnityEngine.Vector2>);
            /** The eighth texture coordinate set of the mesh, if present. */
            public get uv8(): System.Array$1<UnityEngine.Vector2>;
            public set uv8(value: System.Array$1<UnityEngine.Vector2>);
            /** Vertex colors of the Mesh. */
            public get colors(): System.Array$1<UnityEngine.Color>;
            public set colors(value: System.Array$1<UnityEngine.Color>);
            /** Vertex colors of the Mesh. */
            public get colors32(): System.Array$1<UnityEngine.Color32>;
            public set colors32(value: System.Array$1<UnityEngine.Color32>);
            /** An array containing all triangles in the Mesh. */
            public get triangles(): System.Array$1<number>;
            public set triangles(value: System.Array$1<number>);
            /** Retrieves a native (underlying graphics API) pointer to the vertex buffer.
            * @param bufferIndex Which vertex buffer to get (some Meshes might have more than one). See vertexBufferCount.
            * @returns Pointer to the underlying graphics API vertex buffer. 
            */
            public GetNativeVertexBufferPtr ($index: number) : System.IntPtr
            public GetNativeIndexBufferPtr () : System.IntPtr
            public ClearBlendShapes () : void
            /** Returns name of BlendShape by given index. */
            public GetBlendShapeName ($shapeIndex: number) : string
            /** Returns index of BlendShape by given name. */
            public GetBlendShapeIndex ($blendShapeName: string) : number
            /** Returns the frame count for a blend shape. * @param shapeIndex The shape index to get frame count from.
            */
            public GetBlendShapeFrameCount ($shapeIndex: number) : number
            /** Returns the weight of a blend shape frame. * @param shapeIndex The shape index of the frame.
            * @param frameIndex The frame index to get the weight from.
            */
            public GetBlendShapeFrameWeight ($shapeIndex: number, $frameIndex: number) : number
            /** Retreives deltaVertices, deltaNormals and deltaTangents of a blend shape frame. * @param shapeIndex The shape index of the frame.
            * @param frameIndex The frame index to get the weight from.
            * @param deltaVertices Delta vertices output array for the frame being retreived.
            * @param deltaNormals Delta normals output array for the frame being retreived.
            * @param deltaTangents Delta tangents output array for the frame being retreived.
            */
            public GetBlendShapeFrameVertices ($shapeIndex: number, $frameIndex: number, $deltaVertices: System.Array$1<UnityEngine.Vector3>, $deltaNormals: System.Array$1<UnityEngine.Vector3>, $deltaTangents: System.Array$1<UnityEngine.Vector3>) : void
            /** Adds a new blend shape frame. * @param shapeName Name of the blend shape to add a frame to.
            * @param frameWeight Weight for the frame being added.
            * @param deltaVertices Delta vertices for the frame being added.
            * @param deltaNormals Delta normals for the frame being added.
            * @param deltaTangents Delta tangents for the frame being added.
            */
            public AddBlendShapeFrame ($shapeName: string, $frameWeight: number, $deltaVertices: System.Array$1<UnityEngine.Vector3>, $deltaNormals: System.Array$1<UnityEngine.Vector3>, $deltaTangents: System.Array$1<UnityEngine.Vector3>) : void
            /** The UV distribution metric can be used to calculate the desired mipmap level based on the position of the camera.
            * @param uvSetIndex UV set index to return the UV distibution metric for. 0 for first.
            * @returns Average of triangle area / uv area. 
            */
            public GetUVDistributionMetric ($uvSetIndex: number) : number
            public GetVertices ($vertices: System.Collections.Generic.List$1<UnityEngine.Vector3>) : void
            public SetVertices ($inVertices: System.Collections.Generic.List$1<UnityEngine.Vector3>) : void
            public GetNormals ($normals: System.Collections.Generic.List$1<UnityEngine.Vector3>) : void
            public SetNormals ($inNormals: System.Collections.Generic.List$1<UnityEngine.Vector3>) : void
            public GetTangents ($tangents: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public SetTangents ($inTangents: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public GetColors ($colors: System.Collections.Generic.List$1<UnityEngine.Color>) : void
            public SetColors ($inColors: System.Collections.Generic.List$1<UnityEngine.Color>) : void
            public GetColors ($colors: System.Collections.Generic.List$1<UnityEngine.Color32>) : void
            public SetColors ($inColors: System.Collections.Generic.List$1<UnityEngine.Color32>) : void
            public SetUVs ($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector2>) : void
            public SetUVs ($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector3>) : void
            public SetUVs ($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            public GetUVs ($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector2>) : void
            public GetUVs ($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector3>) : void
            public GetUVs ($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector4>) : void
            /** Fetches the triangle list for the specified sub-mesh on this object. * @param triangles A list of vertex indices to populate.
            * @param submesh The sub-mesh index. See subMeshCount.
            * @param applyBaseVertex True (default value) will apply base vertex offset to returned indices.
            */
            public GetTriangles ($submesh: number) : System.Array$1<number>
            /** Fetches the triangle list for the specified sub-mesh on this object. * @param triangles A list of vertex indices to populate.
            * @param submesh The sub-mesh index. See subMeshCount.
            * @param applyBaseVertex True (default value) will apply base vertex offset to returned indices.
            */
            public GetTriangles ($submesh: number, $applyBaseVertex: boolean) : System.Array$1<number>
            public GetTriangles ($triangles: System.Collections.Generic.List$1<number>, $submesh: number) : void
            public GetTriangles ($triangles: System.Collections.Generic.List$1<number>, $submesh: number, $applyBaseVertex: boolean) : void
            /** Fetches the index list for the specified sub-mesh. * @param indices A list of indices to populate.
            * @param submesh The sub-mesh index. See subMeshCount.
            * @param applyBaseVertex True (default value) will apply base vertex offset to returned indices.
            */
            public GetIndices ($submesh: number) : System.Array$1<number>
            public GetIndices ($submesh: number, $applyBaseVertex: boolean) : System.Array$1<number>
            public GetIndices ($indices: System.Collections.Generic.List$1<number>, $submesh: number) : void
            public GetIndices ($indices: System.Collections.Generic.List$1<number>, $submesh: number, $applyBaseVertex: boolean) : void
            /** Gets the starting index location within the Mesh's index buffer, for the given sub-mesh. */
            public GetIndexStart ($submesh: number) : number
            /** Gets the index count of the given sub-mesh. */
            public GetIndexCount ($submesh: number) : number
            /** Gets the base vertex index of the given sub-mesh.
            * @param submesh The sub-mesh index. See subMeshCount.
            * @returns The offset applied to all vertex indices of this sub-mesh. 
            */
            public GetBaseVertex ($submesh: number) : number
            /** Sets the triangle list for the sub-mesh. * @param triangles The list of indices that define the triangles.
            * @param submesh The sub-mesh to modify.
            * @param calculateBounds Calculate the bounding box of the Mesh after setting the triangles. This is done by default.
            Use false when you want to use the existing bounding box and reduce the CPU cost of setting the triangles.
            * @param baseVertex Optional vertex offset that is added to all triangle vertex indices.
            */
            public SetTriangles ($triangles: System.Array$1<number>, $submesh: number) : void
            public SetTriangles ($triangles: System.Array$1<number>, $submesh: number, $calculateBounds: boolean) : void
            /** Sets the triangle list for the sub-mesh. * @param triangles The list of indices that define the triangles.
            * @param submesh The sub-mesh to modify.
            * @param calculateBounds Calculate the bounding box of the Mesh after setting the triangles. This is done by default.
            Use false when you want to use the existing bounding box and reduce the CPU cost of setting the triangles.
            * @param baseVertex Optional vertex offset that is added to all triangle vertex indices.
            */
            public SetTriangles ($triangles: System.Array$1<number>, $submesh: number, $calculateBounds: boolean, $baseVertex: number) : void
            public SetTriangles ($triangles: System.Collections.Generic.List$1<number>, $submesh: number) : void
            public SetTriangles ($triangles: System.Collections.Generic.List$1<number>, $submesh: number, $calculateBounds: boolean) : void
            public SetTriangles ($triangles: System.Collections.Generic.List$1<number>, $submesh: number, $calculateBounds: boolean, $baseVertex: number) : void
            /** Sets the index buffer for the sub-mesh. * @param indices The array of indices that define the Mesh.
            * @param topology The topology of the Mesh, e.g: Triangles, Lines, Quads, Points, etc. See MeshTopology.
            * @param submesh The sub-mesh to modify.
            * @param calculateBounds Calculate the bounding box of the Mesh after setting the indices. This is done by default.
            Use false when you want to use the existing bounding box and reduce the CPU cost of setting the indices.
            * @param baseVertex Optional vertex offset that is added to all triangle vertex indices.
            */
            public SetIndices ($indices: System.Array$1<number>, $topology: UnityEngine.MeshTopology, $submesh: number) : void
            /** Sets the index buffer for the sub-mesh. * @param indices The array of indices that define the Mesh.
            * @param topology The topology of the Mesh, e.g: Triangles, Lines, Quads, Points, etc. See MeshTopology.
            * @param submesh The sub-mesh to modify.
            * @param calculateBounds Calculate the bounding box of the Mesh after setting the indices. This is done by default.
            Use false when you want to use the existing bounding box and reduce the CPU cost of setting the indices.
            * @param baseVertex Optional vertex offset that is added to all triangle vertex indices.
            */
            public SetIndices ($indices: System.Array$1<number>, $topology: UnityEngine.MeshTopology, $submesh: number, $calculateBounds: boolean) : void
            /** Sets the index buffer for the sub-mesh. * @param indices The array of indices that define the Mesh.
            * @param topology The topology of the Mesh, e.g: Triangles, Lines, Quads, Points, etc. See MeshTopology.
            * @param submesh The sub-mesh to modify.
            * @param calculateBounds Calculate the bounding box of the Mesh after setting the indices. This is done by default.
            Use false when you want to use the existing bounding box and reduce the CPU cost of setting the indices.
            * @param baseVertex Optional vertex offset that is added to all triangle vertex indices.
            */
            public SetIndices ($indices: System.Array$1<number>, $topology: UnityEngine.MeshTopology, $submesh: number, $calculateBounds: boolean, $baseVertex: number) : void
            public GetBindposes ($bindposes: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>) : void
            public GetBoneWeights ($boneWeights: System.Collections.Generic.List$1<UnityEngine.BoneWeight>) : void
            /** Clears all vertex data and all triangle indices. */
            public Clear ($keepVertexLayout: boolean) : void
            public Clear () : void
            public RecalculateBounds () : void
            public RecalculateNormals () : void
            public RecalculateTangents () : void
            public MarkDynamic () : void
            /** Upload previously done Mesh modifications to the graphics API. * @param markNoLongerReadable Frees up system memory copy of mesh data when set to true.
            */
            public UploadMeshData ($markNoLongerReadable: boolean) : void
            /** Gets the topology of a sub-mesh. */
            public GetTopology ($submesh: number) : UnityEngine.MeshTopology
            /** Combines several Meshes into this Mesh. * @param combine Descriptions of the Meshes to combine.
            * @param mergeSubMeshes Defines whether Meshes should be combined into a single sub-mesh.
            * @param useMatrices Defines whether the transforms supplied in the CombineInstance array should be used or ignored.
            */
            public CombineMeshes ($combine: System.Array$1<UnityEngine.CombineInstance>, $mergeSubMeshes: boolean, $useMatrices: boolean, $hasLightmapData: boolean) : void
            public CombineMeshes ($combine: System.Array$1<UnityEngine.CombineInstance>, $mergeSubMeshes: boolean, $useMatrices: boolean) : void
            public CombineMeshes ($combine: System.Array$1<UnityEngine.CombineInstance>, $mergeSubMeshes: boolean) : void
            public CombineMeshes ($combine: System.Array$1<UnityEngine.CombineInstance>) : void
            public constructor ()
        }
        /** A block of material values to apply. */
        class MaterialPropertyBlock extends System.Object
        {
        }
        /** The Light Probe Proxy Volume component offers the possibility to use higher resolution lighting for large non-static GameObjects. */
        class LightProbeProxyVolume extends UnityEngine.Behaviour
        {
        }
        /** Represents an axis aligned bounding box. */
        class Bounds extends System.ValueType implements System.IEquatable$1<UnityEngine.Bounds>
        {
        }
        /** Topology of Mesh faces. */
        enum MeshTopology
        { Triangles = 0, Quads = 2, Lines = 3, LineStrip = 4, Points = 5 }
        /** Skinning bone weights of a vertex in the mesh. */
        class BoneWeight extends System.ValueType implements System.IEquatable$1<UnityEngine.BoneWeight>
        {
        }
        /** Representation of RGBA colors in 32 bit format. */
        class Color32 extends System.ValueType
        {
        }
        /** Struct used to describe meshes to be combined using Mesh.CombineMeshes. */
        class CombineInstance extends System.ValueType
        {
        }
        /** Low-level graphics library. */
        class GL extends System.Object
        {
        /** Mode for Begin: draw triangles. */
            public static TRIANGLES : number/** Mode for Begin: draw triangle strip. */
            public static TRIANGLE_STRIP : number/** Mode for Begin: draw quads. */
            public static QUADS : number/** Mode for Begin: draw lines. */
            public static LINES : number/** Mode for Begin: draw line strip. */
            public static LINE_STRIP : number/** Should rendering be done in wireframe? */
            public static get wireframe(): boolean;
            public static set wireframe(value: boolean);
            /** Controls whether Linear-to-sRGB color conversion is performed while rendering. */
            public static get sRGBWrite(): boolean;
            public static set sRGBWrite(value: boolean);
            /** Select whether to invert the backface culling (true) or not (false). */
            public static get invertCulling(): boolean;
            public static set invertCulling(value: boolean);
            /** The current modelview matrix. */
            public static get modelview(): UnityEngine.Matrix4x4;
            public static set modelview(value: UnityEngine.Matrix4x4);
            /** Submit a vertex. */
            public static Vertex3 ($x: number, $y: number, $z: number) : void
            /** Submit a vertex. */
            public static Vertex ($v: UnityEngine.Vector3) : void
            /** Sets current texture coordinate (x,y,z) for all texture units. */
            public static TexCoord3 ($x: number, $y: number, $z: number) : void
            /** Sets current texture coordinate (v.x,v.y,v.z) for all texture units. */
            public static TexCoord ($v: UnityEngine.Vector3) : void
            /** Sets current texture coordinate (x,y) for all texture units. */
            public static TexCoord2 ($x: number, $y: number) : void
            /** Sets current texture coordinate (x,y,z) to the actual texture unit. */
            public static MultiTexCoord3 ($unit: number, $x: number, $y: number, $z: number) : void
            /** Sets current texture coordinate (v.x,v.y,v.z) to the actual texture unit. */
            public static MultiTexCoord ($unit: number, $v: UnityEngine.Vector3) : void
            /** Sets current texture coordinate (x,y) for the actual texture unit. */
            public static MultiTexCoord2 ($unit: number, $x: number, $y: number) : void
            /** Sets current vertex color. */
            public static Color ($c: UnityEngine.Color) : void
            public static Flush () : void
            public static RenderTargetBarrier () : void
            /** Sets the current modelview matrix to the one specified. */
            public static MultMatrix ($m: UnityEngine.Matrix4x4) : void
            public static PushMatrix () : void
            public static PopMatrix () : void
            public static LoadIdentity () : void
            public static LoadOrtho () : void
            public static LoadPixelMatrix () : void
            /** Load an arbitrary matrix to the current projection matrix. */
            public static LoadProjectionMatrix ($mat: UnityEngine.Matrix4x4) : void
            public static InvalidateState () : void
            /** Compute GPU projection matrix from camera's projection matrix.
            * @param proj Source projection matrix.
            * @param renderIntoTexture Will this projection be used for rendering into a RenderTexture?
            * @returns Adjusted projection matrix for the current graphics API. 
            */
            public static GetGPUProjectionMatrix ($proj: UnityEngine.Matrix4x4, $renderIntoTexture: boolean) : UnityEngine.Matrix4x4
            /** Setup a matrix for pixel-correct rendering. */
            public static LoadPixelMatrix ($left: number, $right: number, $bottom: number, $top: number) : void
            /** Send a user-defined event to a native code plugin. * @param eventID User defined id to send to the callback.
            * @param callback Native code callback to queue for Unity's renderer to invoke.
            */
            public static IssuePluginEvent ($callback: System.IntPtr, $eventID: number) : void
            /** Begin drawing 3D primitives. * @param mode Primitives to draw: can be TRIANGLES, TRIANGLE_STRIP, QUADS or LINES.
            */
            public static Begin ($mode: number) : void
            public static End () : void
            /** Clear the current render buffer. * @param clearDepth Should the depth buffer be cleared?
            * @param clearColor Should the color buffer be cleared?
            * @param backgroundColor The color to clear with, used only if clearColor is true.
            * @param depth The depth to clear Z buffer with, used only if clearDepth is true.
            */
            public static Clear ($clearDepth: boolean, $clearColor: boolean, $backgroundColor: UnityEngine.Color, $depth: number) : void
            public static Clear ($clearDepth: boolean, $clearColor: boolean, $backgroundColor: UnityEngine.Color) : void
            /** Set the rendering viewport. */
            public static Viewport ($pixelRect: UnityEngine.Rect) : void
            /** Clear the current render buffer with camera's skybox. * @param clearDepth Should the depth buffer be cleared?
            * @param camera Camera to get projection parameters and skybox from.
            */
            public static ClearWithSkybox ($clearDepth: boolean, $camera: UnityEngine.Camera) : void
            public constructor ()
        }
        /** Class for texture handling. */
        class Texture2D extends UnityEngine.Texture
        {
        /** How many mipmap levels are in this texture (Read Only). */
            public get mipmapCount(): number;
            /** The format of the pixel data in the texture (Read Only). */
            public get format(): UnityEngine.TextureFormat;
            /** Get a small texture with all white pixels. */
            public static get whiteTexture(): UnityEngine.Texture2D;
            /** Get a small texture with all black pixels. */
            public static get blackTexture(): UnityEngine.Texture2D;
            /** Returns true if the Read/Write Enabled checkbox was checked when the texture was imported; otherwise returns false. For a dynamic Texture created from script, always returns true. For additional information, see TextureImporter.isReadable. */
            public get isReadable(): boolean;
            /** Has mipmap streaming been enabled for this texture. */
            public get streamingMipmaps(): boolean;
            /** Relative priority for this texture when reducing memory size in order to hit the memory budget. */
            public get streamingMipmapsPriority(): number;
            /** The mipmap level to load. */
            public get requestedMipmapLevel(): number;
            public set requestedMipmapLevel(value: number);
            /** The mipmap level which would have been loaded by the streaming system before memory budgets are applied. */
            public get desiredMipmapLevel(): number;
            /** Which mipmap level is in the process of being loaded by the mipmap streaming system. */
            public get loadingMipmapLevel(): number;
            /** Which mipmap level is currently loaded by the streaming system. */
            public get loadedMipmapLevel(): number;
            /** Indicates whether this texture was imported with TextureImporter.alphaIsTransparency enabled. This setting is available only in the Editor scripts. Note that changing this setting will have no effect; it must be enabled in TextureImporter instead. */
            public get alphaIsTransparency(): boolean;
            public set alphaIsTransparency(value: boolean);
            /** Compress texture into DXT format. */
            public Compress ($highQuality: boolean) : void
            public ClearRequestedMipmapLevel () : void
            public IsRequestedMipmapLevelLoaded () : boolean
            /** Updates Unity texture to use different native texture object. * @param nativeTex Native 2D texture object.
            */
            public UpdateExternalTexture ($nativeTex: System.IntPtr) : void
            public GetRawTextureData () : System.Array$1<number>
            /** Get a block of pixel colors.
            * @param x The x position of the pixel array to fetch.
            * @param y The y position of the pixel array to fetch.
            * @param blockWidth The width length of the pixel array to fetch.
            * @param blockHeight The height length of the pixel array to fetch.
            * @param miplevel The mipmap level to fetch the pixels. Defaults to zero, and is
            optional.
            * @returns The array of pixels in the texture that have been selected. 
            */
            public GetPixels ($x: number, $y: number, $blockWidth: number, $blockHeight: number, $miplevel: number) : System.Array$1<UnityEngine.Color>
            public GetPixels ($x: number, $y: number, $blockWidth: number, $blockHeight: number) : System.Array$1<UnityEngine.Color>
            /** Get a block of pixel colors in Color32 format. */
            public GetPixels32 ($miplevel: number) : System.Array$1<UnityEngine.Color32>
            public GetPixels32 () : System.Array$1<UnityEngine.Color32>
            /** Packs multiple Textures into a texture atlas.
            * @param textures Array of textures to pack into the atlas.
            * @param padding Padding in pixels between the packed textures.
            * @param maximumAtlasSize Maximum size of the resulting texture.
            * @param makeNoLongerReadable Should the texture be marked as no longer readable?
            * @returns An array of rectangles containing the UV coordinates in the atlas for each input texture, or null if packing fails. 
            */
            public PackTextures ($textures: System.Array$1<UnityEngine.Texture2D>, $padding: number, $maximumAtlasSize: number, $makeNoLongerReadable: boolean) : System.Array$1<UnityEngine.Rect>
            public PackTextures ($textures: System.Array$1<UnityEngine.Texture2D>, $padding: number, $maximumAtlasSize: number) : System.Array$1<UnityEngine.Rect>
            public PackTextures ($textures: System.Array$1<UnityEngine.Texture2D>, $padding: number) : System.Array$1<UnityEngine.Rect>
            /** Creates Unity Texture out of externally created native texture object. * @param nativeTex Native 2D texture object.
            * @param width Width of texture in pixels.
            * @param height Height of texture in pixels.
            * @param format Format of underlying texture object.
            * @param mipmap Does the texture have mipmaps?
            * @param linear Is texture using linear color space?
            */
            public static CreateExternalTexture ($width: number, $height: number, $format: UnityEngine.TextureFormat, $mipChain: boolean, $linear: boolean, $nativeTex: System.IntPtr) : UnityEngine.Texture2D
            /** Sets pixel color at coordinates (x,y). */
            public SetPixel ($x: number, $y: number, $color: UnityEngine.Color) : void
            /** Set a block of pixel colors. */
            public SetPixels ($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color>, $miplevel: number) : void
            public SetPixels ($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color>) : void
            /** Set a block of pixel colors. * @param colors The array of pixel colours to assign (a 2D image flattened to a 1D array).
            * @param miplevel The mip level of the texture to write to.
            */
            public SetPixels ($colors: System.Array$1<UnityEngine.Color>, $miplevel: number) : void
            public SetPixels ($colors: System.Array$1<UnityEngine.Color>) : void
            /** Returns pixel color at coordinates (x, y). */
            public GetPixel ($x: number, $y: number) : UnityEngine.Color
            /** Returns filtered pixel color at normalized coordinates (u, v). */
            public GetPixelBilinear ($x: number, $y: number) : UnityEngine.Color
            /** Fills texture pixels with raw preformatted data. * @param data Raw data array to initialize texture pixels with.
            * @param size Size of data in bytes.
            */
            public LoadRawTextureData ($data: System.IntPtr, $size: number) : void
            /** Fills texture pixels with raw preformatted data. * @param data Raw data array to initialize texture pixels with.
            * @param size Size of data in bytes.
            */
            public LoadRawTextureData ($data: System.Array$1<number>) : void
            /** Actually apply all previous SetPixel and SetPixels changes. * @param updateMipmaps When set to true, mipmap levels are recalculated.
            * @param makeNoLongerReadable When set to true, system memory copy of a texture is released.
            */
            public Apply ($updateMipmaps: boolean, $makeNoLongerReadable: boolean) : void
            public Apply ($updateMipmaps: boolean) : void
            public Apply () : void
            /** Resizes the texture. */
            public Resize ($width: number, $height: number) : boolean
            /** Resizes the texture. */
            public Resize ($width: number, $height: number, $format: UnityEngine.TextureFormat, $hasMipMap: boolean) : boolean
            /** Read pixels from screen into the saved texture data. * @param source Rectangular region of the view to read from. Pixels are read from current render target.
            * @param destX Horizontal pixel position in the texture to place the pixels that are read.
            * @param destY Vertical pixel position in the texture to place the pixels that are read.
            * @param recalculateMipMaps Should the texture's mipmaps be recalculated after reading?
            */
            public ReadPixels ($source: UnityEngine.Rect, $destX: number, $destY: number, $recalculateMipMaps: boolean) : void
            public ReadPixels ($source: UnityEngine.Rect, $destX: number, $destY: number) : void
            public static GenerateAtlas ($sizes: System.Array$1<UnityEngine.Vector2>, $padding: number, $atlasSize: number, $results: System.Collections.Generic.List$1<UnityEngine.Rect>) : boolean
            /** Set a block of pixel colors. */
            public SetPixels32 ($colors: System.Array$1<UnityEngine.Color32>, $miplevel: number) : void
            public SetPixels32 ($colors: System.Array$1<UnityEngine.Color32>) : void
            /** Set a block of pixel colors. */
            public SetPixels32 ($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color32>, $miplevel: number) : void
            public SetPixels32 ($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color32>) : void
            /** Get the pixel colors from the texture.
            * @param miplevel The mipmap level to fetch the pixels from. Defaults to zero.
            * @returns The array of all pixels in the mipmap level of the texture. 
            */
            public GetPixels ($miplevel: number) : System.Array$1<UnityEngine.Color>
            public GetPixels () : System.Array$1<UnityEngine.Color>
            public constructor ($width: number, $height: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat, $flags: UnityEngine.Experimental.Rendering.TextureCreationFlags)
            public constructor ($width: number, $height: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean, $linear: boolean)
            public constructor ($width: number, $height: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean)
            public constructor ($width: number, $height: number)
            public constructor ()
        }
        /** Format used when creating textures from scripts. */
        enum TextureFormat
        { Alpha8 = 1, ARGB4444 = 2, RGB24 = 3, RGBA32 = 4, ARGB32 = 5, RGB565 = 7, R16 = 9, DXT1 = 10, DXT5 = 12, RGBA4444 = 13, BGRA32 = 14, RHalf = 15, RGHalf = 16, RGBAHalf = 17, RFloat = 18, RGFloat = 19, RGBAFloat = 20, YUY2 = 21, RGB9e5Float = 22, BC4 = 26, BC5 = 27, BC6H = 24, BC7 = 25, DXT1Crunched = 28, DXT5Crunched = 29, PVRTC_RGB2 = 30, PVRTC_RGBA2 = 31, PVRTC_RGB4 = 32, PVRTC_RGBA4 = 33, ETC_RGB4 = 34, ATC_RGB4 = -127, ATC_RGBA8 = -127, EAC_R = 41, EAC_R_SIGNED = 42, EAC_RG = 43, EAC_RG_SIGNED = 44, ETC2_RGB = 45, ETC2_RGBA1 = 46, ETC2_RGBA8 = 47, ASTC_RGB_4x4 = 48, ASTC_RGB_5x5 = 49, ASTC_RGB_6x6 = 50, ASTC_RGB_8x8 = 51, ASTC_RGB_10x10 = 52, ASTC_RGB_12x12 = 53, ASTC_RGBA_4x4 = 54, ASTC_RGBA_5x5 = 55, ASTC_RGBA_6x6 = 56, ASTC_RGBA_8x8 = 57, ASTC_RGBA_10x10 = 58, ASTC_RGBA_12x12 = 59, ETC_RGB4_3DS = 60, ETC_RGBA8_3DS = 61, RG16 = 62, R8 = 63, ETC_RGB4Crunched = 64, ETC2_RGBA8Crunched = 65, PVRTC_2BPP_RGB = -127, PVRTC_2BPP_RGBA = -127, PVRTC_4BPP_RGB = -127, PVRTC_4BPP_RGBA = -127 }
        /** Script interface for. */
        class QualitySettings extends UnityEngine.Object
        {
        /** The maximum number of pixel lights that should affect any object. */
            public static get pixelLightCount(): number;
            public static set pixelLightCount(value: number);
            /** Realtime Shadows type to be used. */
            public static get shadows(): UnityEngine.ShadowQuality;
            public static set shadows(value: UnityEngine.ShadowQuality);
            /** Directional light shadow projection. */
            public static get shadowProjection(): UnityEngine.ShadowProjection;
            public static set shadowProjection(value: UnityEngine.ShadowProjection);
            /** Number of cascades to use for directional light shadows. */
            public static get shadowCascades(): number;
            public static set shadowCascades(value: number);
            /** Shadow drawing distance. */
            public static get shadowDistance(): number;
            public static set shadowDistance(value: number);
            /** The default resolution of the shadow maps. */
            public static get shadowResolution(): UnityEngine.ShadowResolution;
            public static set shadowResolution(value: UnityEngine.ShadowResolution);
            /** The rendering mode of Shadowmask. */
            public static get shadowmaskMode(): UnityEngine.ShadowmaskMode;
            public static set shadowmaskMode(value: UnityEngine.ShadowmaskMode);
            /** Offset shadow frustum near plane. */
            public static get shadowNearPlaneOffset(): number;
            public static set shadowNearPlaneOffset(value: number);
            /** The normalized cascade distribution for a 2 cascade setup. The value defines the position of the cascade with respect to Zero. */
            public static get shadowCascade2Split(): number;
            public static set shadowCascade2Split(value: number);
            /** The normalized cascade start position for a 4 cascade setup. Each member of the vector defines the normalized position of the coresponding cascade with respect to Zero. */
            public static get shadowCascade4Split(): UnityEngine.Vector3;
            public static set shadowCascade4Split(value: UnityEngine.Vector3);
            /** Global multiplier for the LOD's switching distance. */
            public static get lodBias(): number;
            public static set lodBias(value: number);
            /** Global anisotropic filtering mode. */
            public static get anisotropicFiltering(): UnityEngine.AnisotropicFiltering;
            public static set anisotropicFiltering(value: UnityEngine.AnisotropicFiltering);
            /** A texture size limit applied to all textures. */
            public static get masterTextureLimit(): number;
            public static set masterTextureLimit(value: number);
            /** A maximum LOD level. All LOD groups. */
            public static get maximumLODLevel(): number;
            public static set maximumLODLevel(value: number);
            /** Budget for how many ray casts can be performed per frame for approximate collision testing. */
            public static get particleRaycastBudget(): number;
            public static set particleRaycastBudget(value: number);
            /** Should soft blending be used for particles? */
            public static get softParticles(): boolean;
            public static set softParticles(value: boolean);
            /** Use a two-pass shader for the vegetation in the terrain engine. */
            public static get softVegetation(): boolean;
            public static set softVegetation(value: boolean);
            /** The VSync Count. */
            public static get vSyncCount(): number;
            public static set vSyncCount(value: number);
            /** Set The AA Filtering option. */
            public static get antiAliasing(): number;
            public static set antiAliasing(value: number);
            /** Async texture upload provides timesliced async texture upload on the render thread with tight control over memory and timeslicing. There are no allocations except for the ones which driver has to do. To read data and upload texture data a ringbuffer whose size can be controlled is re-used.
            Use asyncUploadTimeSlice to set the time-slice in milliseconds for asynchronous texture uploads per
            frame. Minimum value is 1 and maximum is 33. */
            public static get asyncUploadTimeSlice(): number;
            public static set asyncUploadTimeSlice(value: number);
            /** Async texture upload provides timesliced async texture upload on the render thread with tight control over memory and timeslicing. There are no allocations except for the ones which driver has to do. To read data and upload texture data a ringbuffer whose size can be controlled is re-used.
            Use asyncUploadBufferSize to set the buffer size for asynchronous texture uploads. The size is in megabytes. Minimum value is 2 and maximum is 512. Although the buffer will resize automatically to fit the largest texture currently loading, it is recommended to set the value approximately to the size of biggest texture used in the Scene to avoid re-sizing of the buffer which can incur performance cost. */
            public static get asyncUploadBufferSize(): number;
            public static set asyncUploadBufferSize(value: number);
            /** This flag controls if the async upload pipeline's ring buffer remains allocated when there are no active loading operations.
            To make the ring buffer allocation persist after all upload operations have completed, set this to true.
            If you have issues with excessive memory usage, you can set this to false. This means you reduce the runtime memory footprint, but memory fragmentation can occur.
            The default value is true. */
            public static get asyncUploadPersistentBuffer(): boolean;
            public static set asyncUploadPersistentBuffer(value: boolean);
            /** Enables realtime reflection probes. */
            public static get realtimeReflectionProbes(): boolean;
            public static set realtimeReflectionProbes(value: boolean);
            /** If enabled, billboards will face towards camera position rather than camera orientation. */
            public static get billboardsFaceCameraPosition(): boolean;
            public static set billboardsFaceCameraPosition(value: boolean);
            /** In resolution scaling mode, this factor is used to multiply with the target Fixed DPI specified to get the actual Fixed DPI to use for this quality setting. */
            public static get resolutionScalingFixedDPIFactor(): number;
            public static set resolutionScalingFixedDPIFactor(value: number);
            /** Blend weights. */
            public static get blendWeights(): UnityEngine.BlendWeights;
            public static set blendWeights(value: UnityEngine.BlendWeights);
            /** Enable automatic streaming of texture mipmap levels based on their distance from all active cameras. */
            public static get streamingMipmapsActive(): boolean;
            public static set streamingMipmapsActive(value: boolean);
            /** The total amount of memory to be used by streaming and non-streaming textures. */
            public static get streamingMipmapsMemoryBudget(): number;
            public static set streamingMipmapsMemoryBudget(value: number);
            /** Number of renderers used to process each frame during the calculation of desired mipmap levels for the associated textures. */
            public static get streamingMipmapsRenderersPerFrame(): number;
            public static set streamingMipmapsRenderersPerFrame(value: number);
            /** The maximum number of mipmap levels to discard for each texture. */
            public static get streamingMipmapsMaxLevelReduction(): number;
            public static set streamingMipmapsMaxLevelReduction(value: number);
            /** Process all enabled Cameras for texture streaming (rather than just those with StreamingController components). */
            public static get streamingMipmapsAddAllCameras(): boolean;
            public static set streamingMipmapsAddAllCameras(value: boolean);
            /** The maximum number of active texture file IO requests from the texture streaming system. */
            public static get streamingMipmapsMaxFileIORequests(): number;
            public static set streamingMipmapsMaxFileIORequests(value: number);
            /** Maximum number of frames queued up by graphics driver. */
            public static get maxQueuedFrames(): number;
            public static set maxQueuedFrames(value: number);
            /** The indexed list of available Quality Settings. */
            public static get names(): System.Array$1<string>;
            /** Desired color space (Read Only). */
            public static get desiredColorSpace(): UnityEngine.ColorSpace;
            /** Active color space (Read Only). */
            public static get activeColorSpace(): UnityEngine.ColorSpace;
            /** Increase the current quality level. * @param applyExpensiveChanges Should expensive changes be applied (Anti-aliasing etc).
            */
            public static IncreaseLevel ($applyExpensiveChanges: boolean) : void
            /** Decrease the current quality level. * @param applyExpensiveChanges Should expensive changes be applied (Anti-aliasing etc).
            */
            public static DecreaseLevel ($applyExpensiveChanges: boolean) : void
            public static SetQualityLevel ($index: number) : void
            public static IncreaseLevel () : void
            public static DecreaseLevel () : void
            public static GetQualityLevel () : number
            /** Sets a new graphics quality level. * @param index Quality index to set.
            * @param applyExpensiveChanges Should expensive changes be applied (Anti-aliasing etc).
            */
            public static SetQualityLevel ($index: number, $applyExpensiveChanges: boolean) : void
        }
        enum QualityLevel
        { Fastest = 0, Fast = 1, Simple = 2, Good = 3, Beautiful = 4, Fantastic = 5 }
        /** Determines which type of shadows should be used. */
        enum ShadowQuality
        { Disable = 0, HardOnly = 1, All = 2 }
        /** Shadow projection type for. */
        enum ShadowProjection
        { CloseFit = 0, StableFit = 1 }
        /** Default shadow resolution. */
        enum ShadowResolution
        { Low = 0, Medium = 1, High = 2, VeryHigh = 3 }
        /** The rendering mode of Shadowmask. */
        enum ShadowmaskMode
        { Shadowmask = 0, DistanceShadowmask = 1 }
        /** Blend weights. */
        enum BlendWeights
        { OneBone = 1, TwoBones = 2, FourBones = 4 }
        /** Color space for player settings. */
        enum ColorSpace
        { Uninitialized = -1, Gamma = 0, Linear = 1 }
        /** A class to access the Mesh of the. */
        class MeshFilter extends UnityEngine.Component
        {
        /** Returns the shared mesh of the mesh filter. */
            public get sharedMesh(): UnityEngine.Mesh;
            public set sharedMesh(value: UnityEngine.Mesh);
            /** Returns the instantiated Mesh assigned to the mesh filter. */
            public get mesh(): UnityEngine.Mesh;
            public set mesh(value: UnityEngine.Mesh);
            public constructor ()
        }
        /** Interface into the Input system. */
        class Input extends System.Object
        {
        /** Enables/Disables mouse simulation with touches. By default this option is enabled. */
            public static get simulateMouseWithTouches(): boolean;
            public static set simulateMouseWithTouches(value: boolean);
            /** Is any key or mouse button currently held down? (Read Only) */
            public static get anyKey(): boolean;
            /** Returns true the first frame the user hits any key or mouse button. (Read Only) */
            public static get anyKeyDown(): boolean;
            /** Returns the keyboard input entered this frame. (Read Only) */
            public static get inputString(): string;
            /** The current mouse position in pixel coordinates. (Read Only) */
            public static get mousePosition(): UnityEngine.Vector3;
            /** The current mouse scroll delta. (Read Only) */
            public static get mouseScrollDelta(): UnityEngine.Vector2;
            /** Controls enabling and disabling of IME input composition. */
            public static get imeCompositionMode(): UnityEngine.IMECompositionMode;
            public static set imeCompositionMode(value: UnityEngine.IMECompositionMode);
            /** The current IME composition string being typed by the user. */
            public static get compositionString(): string;
            /** Does the user have an IME keyboard input source selected? */
            public static get imeIsSelected(): boolean;
            /** The current text input position used by IMEs to open windows. */
            public static get compositionCursorPos(): UnityEngine.Vector2;
            public static set compositionCursorPos(value: UnityEngine.Vector2);
            /** Indicates if a mouse device is detected. */
            public static get mousePresent(): boolean;
            /** Number of touches. Guaranteed not to change throughout the frame. (Read Only) */
            public static get touchCount(): number;
            /** Bool value which let's users check if touch pressure is supported. */
            public static get touchPressureSupported(): boolean;
            /** Returns true when Stylus Touch is supported by a device or platform. */
            public static get stylusTouchSupported(): boolean;
            /** Returns whether the device on which application is currently running supports touch input. */
            public static get touchSupported(): boolean;
            /** Property indicating whether the system handles multiple touches. */
            public static get multiTouchEnabled(): boolean;
            public static set multiTouchEnabled(value: boolean);
            /** Device physical orientation as reported by OS. (Read Only) */
            public static get deviceOrientation(): UnityEngine.DeviceOrientation;
            /** Last measured linear acceleration of a device in three-dimensional space. (Read Only) */
            public static get acceleration(): UnityEngine.Vector3;
            /** This property controls if input sensors should be compensated for screen orientation. */
            public static get compensateSensors(): boolean;
            public static set compensateSensors(value: boolean);
            /** Number of acceleration measurements which occurred during last frame. */
            public static get accelerationEventCount(): number;
            /** Should  Back button quit the application?
            Only usable on Android, Windows Phone or Windows Tablets. */
            public static get backButtonLeavesApp(): boolean;
            public static set backButtonLeavesApp(value: boolean);
            /** Property for accessing device location (handheld devices only). (Read Only) */
            public static get location(): UnityEngine.LocationService;
            /** Property for accessing compass (handheld devices only). (Read Only) */
            public static get compass(): UnityEngine.Compass;
            /** Returns default gyroscope. */
            public static get gyro(): UnityEngine.Gyroscope;
            /** Returns list of objects representing status of all touches during last frame. (Read Only) (Allocates temporary variables). */
            public static get touches(): System.Array$1<UnityEngine.Touch>;
            /** Returns list of acceleration measurements which occurred during the last frame. (Read Only) (Allocates temporary variables). */
            public static get accelerationEvents(): System.Array$1<UnityEngine.AccelerationEvent>;
            /** Returns the value of the virtual axis identified by axisName. */
            public static GetAxis ($axisName: string) : number
            /** Returns the value of the virtual axis identified by axisName with no smoothing filtering applied. */
            public static GetAxisRaw ($axisName: string) : number
            /** Returns true while the virtual button identified by buttonName is held down.
            * @param buttonName The name of the button such as Jump.
            * @returns True when an axis has been pressed and not released. 
            */
            public static GetButton ($buttonName: string) : boolean
            /** Returns true during the frame the user pressed down the virtual button identified by buttonName. */
            public static GetButtonDown ($buttonName: string) : boolean
            /** Returns true the first frame the user releases the virtual button identified by buttonName. */
            public static GetButtonUp ($buttonName: string) : boolean
            /** Returns whether the given mouse button is held down. */
            public static GetMouseButton ($button: number) : boolean
            /** Returns true during the frame the user pressed the given mouse button. */
            public static GetMouseButtonDown ($button: number) : boolean
            /** Returns true during the frame the user releases the given mouse button. */
            public static GetMouseButtonUp ($button: number) : boolean
            public static ResetInputAxes () : void
            /** Determine whether a particular joystick model has been preconfigured by Unity. (Linux-only).
            * @param joystickName The name of the joystick to check (returned by Input.GetJoystickNames).
            * @returns True if the joystick layout has been preconfigured; false otherwise. 
            */
            public static IsJoystickPreconfigured ($joystickName: string) : boolean
            public static GetJoystickNames () : System.Array$1<string>
            /** Call Input.GetTouch to obtain a Touch struct.
            * @param index The touch input on the device screen.
            * @returns Touch details in the struct. 
            */
            public static GetTouch ($index: number) : UnityEngine.Touch
            /** Returns specific acceleration measurement which occurred during last frame. (Does not allocate temporary variables). */
            public static GetAccelerationEvent ($index: number) : UnityEngine.AccelerationEvent
            /** Returns true while the user holds down the key identified by the key KeyCode enum parameter. */
            public static GetKey ($key: UnityEngine.KeyCode) : boolean
            /** Returns true while the user holds down the key identified by name. */
            public static GetKey ($name: string) : boolean
            /** Returns true during the frame the user releases the key identified by the key KeyCode enum parameter. */
            public static GetKeyUp ($key: UnityEngine.KeyCode) : boolean
            /** Returns true during the frame the user releases the key identified by name. */
            public static GetKeyUp ($name: string) : boolean
            /** Returns true during the frame the user starts pressing down the key identified by the key KeyCode enum parameter. */
            public static GetKeyDown ($key: UnityEngine.KeyCode) : boolean
            /** Returns true during the frame the user starts pressing down the key identified by name. */
            public static GetKeyDown ($name: string) : boolean
            public constructor ()
        }
        /** Structure describing acceleration status of the device. */
        class AccelerationEvent extends System.ValueType
        {
        }
        /** Key codes returned by Event.keyCode. These map directly to a physical key on the keyboard. */
        enum KeyCode
        { None = 0, Backspace = 8, Delete = 127, Tab = 9, Clear = 12, Return = 13, Pause = 19, Escape = 27, Space = 32, Keypad0 = 256, Keypad1 = 257, Keypad2 = 258, Keypad3 = 259, Keypad4 = 260, Keypad5 = 261, Keypad6 = 262, Keypad7 = 263, Keypad8 = 264, Keypad9 = 265, KeypadPeriod = 266, KeypadDivide = 267, KeypadMultiply = 268, KeypadMinus = 269, KeypadPlus = 270, KeypadEnter = 271, KeypadEquals = 272, UpArrow = 273, DownArrow = 274, RightArrow = 275, LeftArrow = 276, Insert = 277, Home = 278, End = 279, PageUp = 280, PageDown = 281, F1 = 282, F2 = 283, F3 = 284, F4 = 285, F5 = 286, F6 = 287, F7 = 288, F8 = 289, F9 = 290, F10 = 291, F11 = 292, F12 = 293, F13 = 294, F14 = 295, F15 = 296, Alpha0 = 48, Alpha1 = 49, Alpha2 = 50, Alpha3 = 51, Alpha4 = 52, Alpha5 = 53, Alpha6 = 54, Alpha7 = 55, Alpha8 = 56, Alpha9 = 57, Exclaim = 33, DoubleQuote = 34, Hash = 35, Dollar = 36, Percent = 37, Ampersand = 38, Quote = 39, LeftParen = 40, RightParen = 41, Asterisk = 42, Plus = 43, Comma = 44, Minus = 45, Period = 46, Slash = 47, Colon = 58, Semicolon = 59, Less = 60, Equals = 61, Greater = 62, Question = 63, At = 64, LeftBracket = 91, Backslash = 92, RightBracket = 93, Caret = 94, Underscore = 95, BackQuote = 96, A = 97, B = 98, C = 99, D = 100, E = 101, F = 102, G = 103, H = 104, I = 105, J = 106, K = 107, L = 108, M = 109, N = 110, O = 111, P = 112, Q = 113, R = 114, S = 115, T = 116, U = 117, V = 118, W = 119, X = 120, Y = 121, Z = 122, LeftCurlyBracket = 123, Pipe = 124, RightCurlyBracket = 125, Tilde = 126, Numlock = 300, CapsLock = 301, ScrollLock = 302, RightShift = 303, LeftShift = 304, RightControl = 305, LeftControl = 306, RightAlt = 307, LeftAlt = 308, LeftCommand = 310, LeftApple = 310, LeftWindows = 311, RightCommand = 309, RightApple = 309, RightWindows = 312, AltGr = 313, Help = 315, Print = 316, SysReq = 317, Break = 318, Menu = 319, Mouse0 = 323, Mouse1 = 324, Mouse2 = 325, Mouse3 = 326, Mouse4 = 327, Mouse5 = 328, Mouse6 = 329, JoystickButton0 = 330, JoystickButton1 = 331, JoystickButton2 = 332, JoystickButton3 = 333, JoystickButton4 = 334, JoystickButton5 = 335, JoystickButton6 = 336, JoystickButton7 = 337, JoystickButton8 = 338, JoystickButton9 = 339, JoystickButton10 = 340, JoystickButton11 = 341, JoystickButton12 = 342, JoystickButton13 = 343, JoystickButton14 = 344, JoystickButton15 = 345, JoystickButton16 = 346, JoystickButton17 = 347, JoystickButton18 = 348, JoystickButton19 = 349, Joystick1Button0 = 350, Joystick1Button1 = 351, Joystick1Button2 = 352, Joystick1Button3 = 353, Joystick1Button4 = 354, Joystick1Button5 = 355, Joystick1Button6 = 356, Joystick1Button7 = 357, Joystick1Button8 = 358, Joystick1Button9 = 359, Joystick1Button10 = 360, Joystick1Button11 = 361, Joystick1Button12 = 362, Joystick1Button13 = 363, Joystick1Button14 = 364, Joystick1Button15 = 365, Joystick1Button16 = 366, Joystick1Button17 = 367, Joystick1Button18 = 368, Joystick1Button19 = 369, Joystick2Button0 = 370, Joystick2Button1 = 371, Joystick2Button2 = 372, Joystick2Button3 = 373, Joystick2Button4 = 374, Joystick2Button5 = 375, Joystick2Button6 = 376, Joystick2Button7 = 377, Joystick2Button8 = 378, Joystick2Button9 = 379, Joystick2Button10 = 380, Joystick2Button11 = 381, Joystick2Button12 = 382, Joystick2Button13 = 383, Joystick2Button14 = 384, Joystick2Button15 = 385, Joystick2Button16 = 386, Joystick2Button17 = 387, Joystick2Button18 = 388, Joystick2Button19 = 389, Joystick3Button0 = 390, Joystick3Button1 = 391, Joystick3Button2 = 392, Joystick3Button3 = 393, Joystick3Button4 = 394, Joystick3Button5 = 395, Joystick3Button6 = 396, Joystick3Button7 = 397, Joystick3Button8 = 398, Joystick3Button9 = 399, Joystick3Button10 = 400, Joystick3Button11 = 401, Joystick3Button12 = 402, Joystick3Button13 = 403, Joystick3Button14 = 404, Joystick3Button15 = 405, Joystick3Button16 = 406, Joystick3Button17 = 407, Joystick3Button18 = 408, Joystick3Button19 = 409, Joystick4Button0 = 410, Joystick4Button1 = 411, Joystick4Button2 = 412, Joystick4Button3 = 413, Joystick4Button4 = 414, Joystick4Button5 = 415, Joystick4Button6 = 416, Joystick4Button7 = 417, Joystick4Button8 = 418, Joystick4Button9 = 419, Joystick4Button10 = 420, Joystick4Button11 = 421, Joystick4Button12 = 422, Joystick4Button13 = 423, Joystick4Button14 = 424, Joystick4Button15 = 425, Joystick4Button16 = 426, Joystick4Button17 = 427, Joystick4Button18 = 428, Joystick4Button19 = 429, Joystick5Button0 = 430, Joystick5Button1 = 431, Joystick5Button2 = 432, Joystick5Button3 = 433, Joystick5Button4 = 434, Joystick5Button5 = 435, Joystick5Button6 = 436, Joystick5Button7 = 437, Joystick5Button8 = 438, Joystick5Button9 = 439, Joystick5Button10 = 440, Joystick5Button11 = 441, Joystick5Button12 = 442, Joystick5Button13 = 443, Joystick5Button14 = 444, Joystick5Button15 = 445, Joystick5Button16 = 446, Joystick5Button17 = 447, Joystick5Button18 = 448, Joystick5Button19 = 449, Joystick6Button0 = 450, Joystick6Button1 = 451, Joystick6Button2 = 452, Joystick6Button3 = 453, Joystick6Button4 = 454, Joystick6Button5 = 455, Joystick6Button6 = 456, Joystick6Button7 = 457, Joystick6Button8 = 458, Joystick6Button9 = 459, Joystick6Button10 = 460, Joystick6Button11 = 461, Joystick6Button12 = 462, Joystick6Button13 = 463, Joystick6Button14 = 464, Joystick6Button15 = 465, Joystick6Button16 = 466, Joystick6Button17 = 467, Joystick6Button18 = 468, Joystick6Button19 = 469, Joystick7Button0 = 470, Joystick7Button1 = 471, Joystick7Button2 = 472, Joystick7Button3 = 473, Joystick7Button4 = 474, Joystick7Button5 = 475, Joystick7Button6 = 476, Joystick7Button7 = 477, Joystick7Button8 = 478, Joystick7Button9 = 479, Joystick7Button10 = 480, Joystick7Button11 = 481, Joystick7Button12 = 482, Joystick7Button13 = 483, Joystick7Button14 = 484, Joystick7Button15 = 485, Joystick7Button16 = 486, Joystick7Button17 = 487, Joystick7Button18 = 488, Joystick7Button19 = 489, Joystick8Button0 = 490, Joystick8Button1 = 491, Joystick8Button2 = 492, Joystick8Button3 = 493, Joystick8Button4 = 494, Joystick8Button5 = 495, Joystick8Button6 = 496, Joystick8Button7 = 497, Joystick8Button8 = 498, Joystick8Button9 = 499, Joystick8Button10 = 500, Joystick8Button11 = 501, Joystick8Button12 = 502, Joystick8Button13 = 503, Joystick8Button14 = 504, Joystick8Button15 = 505, Joystick8Button16 = 506, Joystick8Button17 = 507, Joystick8Button18 = 508, Joystick8Button19 = 509 }
        /** Controls IME input. */
        enum IMECompositionMode
        { Auto = 0, On = 1, Off = 2 }
        /** Describes physical orientation of the device as determined by the OS. */
        enum DeviceOrientation
        { Unknown = 0, Portrait = 1, PortraitUpsideDown = 2, LandscapeLeft = 3, LandscapeRight = 4, FaceUp = 5, FaceDown = 6 }
        /** Interface into location functionality. */
        class LocationService extends System.Object
        {
        }
        /** Interface into compass functionality. */
        class Compass extends System.Object
        {
        }
        /** Interface into the Gyroscope. */
        class Gyroscope extends System.Object
        {
        }
        /** Specifies Layers to use in a Physics.Raycast. */
        class LayerMask extends System.ValueType
        {
        /** Converts a layer mask value to an integer value. */
            public get value(): number;
            public set value(value: number);
            public static op_Implicit ($mask: UnityEngine.LayerMask) : number
            public static op_Implicit ($intVal: number) : UnityEngine.LayerMask
            /** Given a layer number, returns the name of the layer as defined in either a Builtin or a User Layer in the. */
            public static LayerToName ($layer: number) : string
            /** Given a layer name, returns the layer index as defined by either a Builtin or a User Layer in the. */
            public static NameToLayer ($layerName: string) : number
            /** Given a set of layer names as defined by either a Builtin or a User Layer in the, returns the equivalent layer mask for all of them.
            * @param layerNames List of layer names to convert to a layer mask.
            * @returns The layer mask created from the layerNames. 
            */
            public static GetMask (...layerNames: string[]) : number
        }
        /** A collection of common math functions. */
        class Mathf extends System.ValueType
        {
        /** The well-known 3.14159265358979... value (Read Only). */
            public static PI : number/** A representation of positive infinity (Read Only). */
            public static Infinity : number/** A representation of negative infinity (Read Only). */
            public static NegativeInfinity : number/** Degrees-to-radians conversion constant (Read Only). */
            public static Deg2Rad : number/** Radians-to-degrees conversion constant (Read Only). */
            public static Rad2Deg : number/** A tiny floating point value (Read Only). */
            public static Epsilon : number/** Returns the closest power of two value. */
            public static ClosestPowerOfTwo ($value: number) : number
            /** Returns true if the value is power of two. */
            public static IsPowerOfTwo ($value: number) : boolean
            /** Returns the next power of two that is equal to, or greater than, the argument. */
            public static NextPowerOfTwo ($value: number) : number
            /** Converts the given value from gamma (sRGB) to linear color space. */
            public static GammaToLinearSpace ($value: number) : number
            /** Converts the given value from linear to gamma (sRGB) color space. */
            public static LinearToGammaSpace ($value: number) : number
            /** Convert a color temperature in Kelvin to RGB color.
            * @param kelvin Temperature in Kelvin. Range 1000 to 40000 Kelvin.
            * @returns Correlated Color Temperature as floating point RGB color. 
            */
            public static CorrelatedColorTemperatureToRGB ($kelvin: number) : UnityEngine.Color
            public static FloatToHalf ($val: number) : number
            public static HalfToFloat ($val: number) : number
            /** Generate 2D Perlin noise.
            * @param x X-coordinate of sample point.
            * @param y Y-coordinate of sample point.
            * @returns Value between 0.0 and 1.0. (Return value might be slightly beyond 1.0.) 
            */
            public static PerlinNoise ($x: number, $y: number) : number
            /** Returns the sine of angle f.
            * @param f The input angle, in radians.
            * @returns The return value between -1 and +1. 
            */
            public static Sin ($f: number) : number
            /** Returns the cosine of angle f.
            * @param f The input angle, in radians.
            * @returns The return value between -1 and 1. 
            */
            public static Cos ($f: number) : number
            /** Returns the tangent of angle f in radians. */
            public static Tan ($f: number) : number
            /** Returns the arc-sine of f - the angle in radians whose sine is f. */
            public static Asin ($f: number) : number
            /** Returns the arc-cosine of f - the angle in radians whose cosine is f. */
            public static Acos ($f: number) : number
            /** Returns the arc-tangent of f - the angle in radians whose tangent is f. */
            public static Atan ($f: number) : number
            /** Returns the angle in radians whose Tan is y/x. */
            public static Atan2 ($y: number, $x: number) : number
            /** Returns square root of f. */
            public static Sqrt ($f: number) : number
            /** Returns the absolute value of f. */
            public static Abs ($f: number) : number
            /** Returns the absolute value of value. */
            public static Abs ($value: number) : number
            /** Returns the smallest of two or more values. */
            public static Min ($a: number, $b: number) : number
            /** Returns the smallest of two or more values. */
            public static Min (...values: number[]) : number
            /** Returns largest of two or more values. */
            public static Max ($a: number, $b: number) : number
            /** Returns largest of two or more values. */
            public static Max (...values: number[]) : number
            /** Returns f raised to power p. */
            public static Pow ($f: number, $p: number) : number
            /** Returns e raised to the specified power. */
            public static Exp ($power: number) : number
            /** Returns the logarithm of a specified number in a specified base. */
            public static Log ($f: number, $p: number) : number
            /** Returns the natural (base e) logarithm of a specified number. */
            public static Log ($f: number) : number
            /** Returns the base 10 logarithm of a specified number. */
            public static Log10 ($f: number) : number
            /** Returns the smallest integer greater to or equal to f. */
            public static Ceil ($f: number) : number
            /** Returns the largest integer smaller than or equal to f. */
            public static Floor ($f: number) : number
            /** Returns f rounded to the nearest integer. */
            public static Round ($f: number) : number
            /** Returns the smallest integer greater to or equal to f. */
            public static CeilToInt ($f: number) : number
            /** Returns the largest integer smaller to or equal to f. */
            public static FloorToInt ($f: number) : number
            /** Returns f rounded to the nearest integer. */
            public static RoundToInt ($f: number) : number
            /** Returns the sign of f. */
            public static Sign ($f: number) : number
            /** Clamps the given value between the given minimum float and maximum float values.  Returns the given value if it is within the min and max range.
            * @param value The floating point value to restrict inside the range defined by the min and max values.
            * @param min The minimum floating point value to compare against.
            * @param max The maximum floating point value to compare against.
            * @returns The float result between the min and max values. 
            */
            public static Clamp ($value: number, $min: number, $max: number) : number
            /** Clamps value between 0 and 1 and returns value. */
            public static Clamp01 ($value: number) : number
            /** Linearly interpolates between a and b by t.
            * @param a The start value.
            * @param b The end value.
            * @param t The interpolation value between the two floats.
            * @returns The interpolated float result between the two float values. 
            */
            public static Lerp ($a: number, $b: number, $t: number) : number
            /** Linearly interpolates between a and b by t with no limit to t.
            * @param a The start value.
            * @param b The end value.
            * @param t The interpolation between the two floats.
            * @returns The float value as a result from the linear interpolation. 
            */
            public static LerpUnclamped ($a: number, $b: number, $t: number) : number
            /** Same as Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees. */
            public static LerpAngle ($a: number, $b: number, $t: number) : number
            /** Moves a value current towards target. * @param current The current value.
            * @param target The value to move towards.
            * @param maxDelta The maximum change that should be applied to the value.
            */
            public static MoveTowards ($current: number, $target: number, $maxDelta: number) : number
            /** Same as MoveTowards but makes sure the values interpolate correctly when they wrap around 360 degrees. */
            public static MoveTowardsAngle ($current: number, $target: number, $maxDelta: number) : number
            /** Interpolates between min and max with smoothing at the limits. */
            public static SmoothStep ($from: number, $to: number, $t: number) : number
            public static Gamma ($value: number, $absmax: number, $gamma: number) : number
            /** Compares two floating point values and returns true if they are similar. */
            public static Approximately ($a: number, $b: number) : boolean
            /** Gradually changes a value towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number) : number
            /** Gradually changes a value towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number) : number
            /** Gradually changes a value towards a desired goal over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDamp ($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number, $deltaTime: number) : number
            /** Gradually changes an angle given in degrees towards a desired goal angle over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDampAngle ($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number) : number
            /** Gradually changes an angle given in degrees towards a desired goal angle over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDampAngle ($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number) : number
            /** Gradually changes an angle given in degrees towards a desired goal angle over time. * @param current The current position.
            * @param target The position we are trying to reach.
            * @param currentVelocity The current velocity, this value is modified by the function every time you call it.
            * @param smoothTime Approximately the time it will take to reach the target. A smaller value will reach the target faster.
            * @param maxSpeed Optionally allows you to clamp the maximum speed.
            * @param deltaTime The time since the last call to this function. By default Time.deltaTime.
            */
            public static SmoothDampAngle ($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number, $deltaTime: number) : number
            /** Loops the value t, so that it is never larger than length and never smaller than 0. */
            public static Repeat ($t: number, $length: number) : number
            /** PingPongs the value t, so that it is never larger than length and never smaller than 0. */
            public static PingPong ($t: number, $length: number) : number
            /** Calculates the linear parameter t that produces the interpolant value within the range [a, b].
            * @param a Start value.
            * @param b End value.
            * @param value Value between start and end.
            * @returns Percentage of value between start and end. 
            */
            public static InverseLerp ($a: number, $b: number, $value: number) : number
            /** Calculates the shortest difference between two given angles given in degrees. */
            public static DeltaAngle ($current: number, $target: number) : number
        }
        /** MonoBehaviour is the base class from which every Unity script derives. */
        class MonoBehaviour extends UnityEngine.Behaviour
        {
        /** Disabling this lets you skip the GUI layout phase. */
            public get useGUILayout(): boolean;
            public set useGUILayout(value: boolean);
            /** Allow a specific instance of a MonoBehaviour to run in edit mode (only available in the editor). */
            public get runInEditMode(): boolean;
            public set runInEditMode(value: boolean);
            public IsInvoking () : boolean
            public CancelInvoke () : void
            /** Invokes the method methodName in time seconds. */
            public Invoke ($methodName: string, $time: number) : void
            /** Invokes the method methodName in time seconds, then repeatedly every repeatRate seconds. */
            public InvokeRepeating ($methodName: string, $time: number, $repeatRate: number) : void
            /** Cancels all Invoke calls with name methodName on this behaviour. */
            public CancelInvoke ($methodName: string) : void
            /** Is any invoke on methodName pending? */
            public IsInvoking ($methodName: string) : boolean
            /** Starts a coroutine named methodName. */
            public StartCoroutine ($methodName: string) : UnityEngine.Coroutine
            /** Starts a coroutine named methodName. */
            public StartCoroutine ($methodName: string, $value: any) : UnityEngine.Coroutine
            /** Starts a coroutine. */
            public StartCoroutine ($routine: System.Collections.IEnumerator) : UnityEngine.Coroutine
            /** Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour. * @param methodName Name of coroutine.
            * @param routine Name of the function in code, including coroutines.
            */
            public StopCoroutine ($routine: System.Collections.IEnumerator) : void
            /** Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour. * @param methodName Name of coroutine.
            * @param routine Name of the function in code, including coroutines.
            */
            public StopCoroutine ($routine: UnityEngine.Coroutine) : void
            /** Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour. * @param methodName Name of coroutine.
            * @param routine Name of the function in code, including coroutines.
            */
            public StopCoroutine ($methodName: string) : void
            public StopAllCoroutines () : void
            /** Logs message to the Unity Console (identical to Debug.Log). */
            public static print ($message: any) : void
            public constructor ()
        }
        /** MonoBehaviour.StartCoroutine returns a Coroutine. Instances of this class are only used to reference these coroutines, and do not hold any exposed properties or functions. */
        class Coroutine extends UnityEngine.YieldInstruction
        {
        }
        /** Stores and accesses player preferences between game sessions. */
        class PlayerPrefs extends System.Object
        {
        /** Sets the value of the preference identified by key. */
            public static SetInt ($key: string, $value: number) : void
            /** Returns the value corresponding to key in the preference file if it exists. */
            public static GetInt ($key: string, $defaultValue: number) : number
            /** Returns the value corresponding to key in the preference file if it exists. */
            public static GetInt ($key: string) : number
            /** Sets the value of the preference identified by key. */
            public static SetFloat ($key: string, $value: number) : void
            /** Returns the value corresponding to key in the preference file if it exists. */
            public static GetFloat ($key: string, $defaultValue: number) : number
            /** Returns the value corresponding to key in the preference file if it exists. */
            public static GetFloat ($key: string) : number
            /** Sets the value of the preference identified by key. */
            public static SetString ($key: string, $value: string) : void
            /** Returns the value corresponding to key in the preference file if it exists. */
            public static GetString ($key: string, $defaultValue: string) : string
            /** Returns the value corresponding to key in the preference file if it exists. */
            public static GetString ($key: string) : string
            /** Returns true if key exists in the preferences. */
            public static HasKey ($key: string) : boolean
            /** Removes key and its corresponding value from the preferences. */
            public static DeleteKey ($key: string) : void
            public static DeleteAll () : void
            public static Save () : void
            public constructor ()
        }
        /** Class for generating random data. */
        class Random extends System.Object
        {
        /** Gets/Sets the full internal state of the random number generator. */
            public static get state(): UnityEngine.Random.State;
            public static set state(value: UnityEngine.Random.State);
            /** Returns a random number between 0.0 [inclusive] and 1.0 [inclusive] (Read Only). */
            public static get value(): number;
            /** Returns a random point inside a sphere with radius 1 (Read Only). */
            public static get insideUnitSphere(): UnityEngine.Vector3;
            /** Returns a random point inside a circle with radius 1 (Read Only). */
            public static get insideUnitCircle(): UnityEngine.Vector2;
            /** Returns a random point on the surface of a sphere with radius 1 (Read Only). */
            public static get onUnitSphere(): UnityEngine.Vector3;
            /** Returns a random rotation (Read Only). */
            public static get rotation(): UnityEngine.Quaternion;
            /** Returns a random rotation with uniform distribution (Read Only). */
            public static get rotationUniform(): UnityEngine.Quaternion;
            /** Initializes the random number generator state with a seed. * @param seed Seed used to initialize the random number generator.
            */
            public static InitState ($seed: number) : void
            /** Return a random float number between min [inclusive] and max [inclusive] (Read Only). */
            public static Range ($min: number, $max: number) : number
            public static ColorHSV () : UnityEngine.Color
            /** Generates a random color from HSV and alpha ranges.
            * @param hueMin Minimum hue [0..1].
            * @param hueMax Maximum hue [0..1].
            * @param saturationMin Minimum saturation [0..1].
            * @param saturationMax Maximum saturation[0..1].
            * @param valueMin Minimum value [0..1].
            * @param valueMax Maximum value [0..1].
            * @param alphaMin Minimum alpha [0..1].
            * @param alphaMax Maximum alpha [0..1].
            * @returns A random color with HSV and alpha values in the input ranges. 
            */
            public static ColorHSV ($hueMin: number, $hueMax: number) : UnityEngine.Color
            /** Generates a random color from HSV and alpha ranges.
            * @param hueMin Minimum hue [0..1].
            * @param hueMax Maximum hue [0..1].
            * @param saturationMin Minimum saturation [0..1].
            * @param saturationMax Maximum saturation[0..1].
            * @param valueMin Minimum value [0..1].
            * @param valueMax Maximum value [0..1].
            * @param alphaMin Minimum alpha [0..1].
            * @param alphaMax Maximum alpha [0..1].
            * @returns A random color with HSV and alpha values in the input ranges. 
            */
            public static ColorHSV ($hueMin: number, $hueMax: number, $saturationMin: number, $saturationMax: number) : UnityEngine.Color
            /** Generates a random color from HSV and alpha ranges.
            * @param hueMin Minimum hue [0..1].
            * @param hueMax Maximum hue [0..1].
            * @param saturationMin Minimum saturation [0..1].
            * @param saturationMax Maximum saturation[0..1].
            * @param valueMin Minimum value [0..1].
            * @param valueMax Maximum value [0..1].
            * @param alphaMin Minimum alpha [0..1].
            * @param alphaMax Maximum alpha [0..1].
            * @returns A random color with HSV and alpha values in the input ranges. 
            */
            public static ColorHSV ($hueMin: number, $hueMax: number, $saturationMin: number, $saturationMax: number, $valueMin: number, $valueMax: number) : UnityEngine.Color
            /** Generates a random color from HSV and alpha ranges.
            * @param hueMin Minimum hue [0..1].
            * @param hueMax Maximum hue [0..1].
            * @param saturationMin Minimum saturation [0..1].
            * @param saturationMax Maximum saturation[0..1].
            * @param valueMin Minimum value [0..1].
            * @param valueMax Maximum value [0..1].
            * @param alphaMin Minimum alpha [0..1].
            * @param alphaMax Maximum alpha [0..1].
            * @returns A random color with HSV and alpha values in the input ranges. 
            */
            public static ColorHSV ($hueMin: number, $hueMax: number, $saturationMin: number, $saturationMax: number, $valueMin: number, $valueMax: number, $alphaMin: number, $alphaMax: number) : UnityEngine.Color
            public constructor ()
        }
        /** The Resources class allows you to find and access Objects including assets. */
        class Resources extends System.Object
        {
        /** Returns a list of all objects of Type type.
            * @param type Type of the class to match while searching.
            * @returns An array of objects whose class is type or is derived from type. 
            */
            public static FindObjectsOfTypeAll ($type: System.Type) : System.Array$1<UnityEngine.Object>
            /** Loads an asset stored at path in a Resources folder.
            * @param path Pathname of the target folder. When using the empty string (i.e., ""), the function will load the entire contents of the Resources folder.
            * @param systemTypeInstance Type filter for objects returned.
            * @returns The requested asset returned as an Object. 
            */
            public static Load ($path: string) : UnityEngine.Object
            /** Loads an asset stored at path in a Resources folder.
            * @param path Pathname of the target folder. When using the empty string (i.e., ""), the function will load the entire contents of the Resources folder.
            * @param systemTypeInstance Type filter for objects returned.
            * @returns The requested asset returned as an Object. 
            */
            public static Load ($path: string, $systemTypeInstance: System.Type) : UnityEngine.Object
            /** Asynchronously loads an asset stored at path in a Resources folder. * @param path Pathname of the target folder. When using the empty string (i.e., ""), the function will load the entire contents of the Resources folder.
            */
            public static LoadAsync ($path: string) : UnityEngine.ResourceRequest
            /** Asynchronously loads an asset stored at path in a Resources folder. * @param path Pathname of the target folder. When using the empty string (i.e., ""), the function will load the entire contents of the Resources folder.
            * @param systemTypeInstance Type filter for objects returned.
            */
            public static LoadAsync ($path: string, $type: System.Type) : UnityEngine.ResourceRequest
            /** Loads all assets in a folder or file at path in a Resources folder. * @param path Pathname of the target folder. When using the empty string (i.e., ""), the function will load the entire contents of the Resources folder.
            * @param systemTypeInstance Type filter for objects returned.
            */
            public static LoadAll ($path: string, $systemTypeInstance: System.Type) : System.Array$1<UnityEngine.Object>
            /** Loads all assets in a folder or file at path in a Resources folder. * @param path Pathname of the target folder. When using the empty string (i.e., ""), the function will load the entire contents of the Resources folder.
            */
            public static LoadAll ($path: string) : System.Array$1<UnityEngine.Object>
            public static GetBuiltinResource ($type: System.Type, $path: string) : UnityEngine.Object
            /** Unloads assetToUnload from memory. */
            public static UnloadAsset ($assetToUnload: UnityEngine.Object) : void
            public static UnloadUnusedAssets () : UnityEngine.AsyncOperation
            public constructor ()
        }
        /** Asynchronous load request from the Resources bundle. */
        class ResourceRequest extends UnityEngine.AsyncOperation
        {
        }
        /** Access system and hardware information. */
        class SystemInfo extends System.Object
        {
        /** Value returned by SystemInfo string properties which are not supported on the current platform. */
            public static unsupportedIdentifier : string/** The current battery level (Read Only). */
            public static get batteryLevel(): number;
            /** Returns the current status of the device's battery (Read Only). */
            public static get batteryStatus(): UnityEngine.BatteryStatus;
            /** Operating system name with version (Read Only). */
            public static get operatingSystem(): string;
            /** Returns the operating system family the game is running on (Read Only). */
            public static get operatingSystemFamily(): UnityEngine.OperatingSystemFamily;
            /** Processor name (Read Only). */
            public static get processorType(): string;
            /** Processor frequency in MHz (Read Only). */
            public static get processorFrequency(): number;
            /** Number of processors present (Read Only). */
            public static get processorCount(): number;
            /** Amount of system memory present (Read Only). */
            public static get systemMemorySize(): number;
            /** A unique device identifier. It is guaranteed to be unique for every device (Read Only). */
            public static get deviceUniqueIdentifier(): string;
            /** The user defined name of the device (Read Only). */
            public static get deviceName(): string;
            /** The model of the device (Read Only). */
            public static get deviceModel(): string;
            /** Is an accelerometer available on the device? */
            public static get supportsAccelerometer(): boolean;
            /** Is a gyroscope available on the device? */
            public static get supportsGyroscope(): boolean;
            /** Is the device capable of reporting its location? */
            public static get supportsLocationService(): boolean;
            /** Is the device capable of providing the user haptic feedback by vibration? */
            public static get supportsVibration(): boolean;
            /** Is there an Audio device available for playback? (Read Only) */
            public static get supportsAudio(): boolean;
            /** Returns the kind of device the application is running on (Read Only). */
            public static get deviceType(): UnityEngine.DeviceType;
            /** Amount of video memory present (Read Only). */
            public static get graphicsMemorySize(): number;
            /** The name of the graphics device (Read Only). */
            public static get graphicsDeviceName(): string;
            /** The vendor of the graphics device (Read Only). */
            public static get graphicsDeviceVendor(): string;
            /** The identifier code of the graphics device (Read Only). */
            public static get graphicsDeviceID(): number;
            /** The identifier code of the graphics device vendor (Read Only). */
            public static get graphicsDeviceVendorID(): number;
            /** The graphics API type used by the graphics device (Read Only). */
            public static get graphicsDeviceType(): UnityEngine.Rendering.GraphicsDeviceType;
            /** Returns true if the texture UV coordinate convention for this platform has Y starting at the top of the image. */
            public static get graphicsUVStartsAtTop(): boolean;
            /** The graphics API type and driver version used by the graphics device (Read Only). */
            public static get graphicsDeviceVersion(): string;
            /** Graphics device shader capability level (Read Only). */
            public static get graphicsShaderLevel(): number;
            /** Is graphics device using multi-threaded rendering (Read Only)? */
            public static get graphicsMultiThreaded(): boolean;
            /** True if the GPU supports hidden surface removal. */
            public static get hasHiddenSurfaceRemovalOnGPU(): boolean;
            /** Returns true when the GPU has native support for indexing uniform arrays in fragment shaders without restrictions. */
            public static get hasDynamicUniformArrayIndexingInFragmentShaders(): boolean;
            /** Are built-in shadows supported? (Read Only) */
            public static get supportsShadows(): boolean;
            /** Is sampling raw depth from shadowmaps supported? (Read Only) */
            public static get supportsRawShadowDepthSampling(): boolean;
            /** Whether motion vectors are supported on this platform. */
            public static get supportsMotionVectors(): boolean;
            /** Are cubemap render textures supported? (Read Only) */
            public static get supportsRenderToCubemap(): boolean;
            /** Are image effects supported? (Read Only) */
            public static get supportsImageEffects(): boolean;
            /** Are 3D (volume) textures supported? (Read Only) */
            public static get supports3DTextures(): boolean;
            /** Are 2D Array textures supported? (Read Only) */
            public static get supports2DArrayTextures(): boolean;
            /** Are 3D (volume) RenderTextures supported? (Read Only) */
            public static get supports3DRenderTextures(): boolean;
            /** Are Cubemap Array textures supported? (Read Only) */
            public static get supportsCubemapArrayTextures(): boolean;
            /** Support for various Graphics.CopyTexture cases (Read Only). */
            public static get copyTextureSupport(): UnityEngine.Rendering.CopyTextureSupport;
            /** Are compute shaders supported? (Read Only) */
            public static get supportsComputeShaders(): boolean;
            /** Is GPU draw call instancing supported? (Read Only) */
            public static get supportsInstancing(): boolean;
            /** Does the hardware support quad topology? (Read Only) */
            public static get supportsHardwareQuadTopology(): boolean;
            /** Are 32-bit index buffers supported? (Read Only) */
            public static get supports32bitsIndexBuffer(): boolean;
            /** Are sparse textures supported? (Read Only) */
            public static get supportsSparseTextures(): boolean;
            /** How many simultaneous render targets (MRTs) are supported? (Read Only) */
            public static get supportedRenderTargetCount(): number;
            /** Returns true when the platform supports different blend modes when rendering to multiple render targets, or false otherwise. */
            public static get supportsSeparatedRenderTargetsBlend(): boolean;
            /** Are multisampled textures supported? (Read Only) */
            public static get supportsMultisampledTextures(): number;
            /** Returns true if multisampled textures are resolved automatically */
            public static get supportsMultisampleAutoResolve(): boolean;
            /** Returns true if the 'Mirror Once' texture wrap mode is supported. (Read Only) */
            public static get supportsTextureWrapMirrorOnce(): number;
            /** This property is true if the current platform uses a reversed depth buffer (where values range from 1 at the near plane and 0 at far plane), and false if the depth buffer is normal (0 is near, 1 is far). (Read Only) */
            public static get usesReversedZBuffer(): boolean;
            /** What NPOT (non-power of two size) texture support does the GPU provide? (Read Only) */
            public static get npotSupport(): UnityEngine.NPOTSupport;
            /** Maximum texture size (Read Only). */
            public static get maxTextureSize(): number;
            /** Maximum Cubemap texture size (Read Only). */
            public static get maxCubemapSize(): number;
            /** Returns true when the platform supports asynchronous compute queues and false if otherwise.
            Note that asynchronous compute queues are only supported on PS4. */
            public static get supportsAsyncCompute(): boolean;
            /** Returns true when the platform supports GPUFences and false if otherwise.
            Note that GPUFences are only supported on PS4. */
            public static get supportsGPUFence(): boolean;
            /** Returns true if asynchronous readback of GPU data is available for this device and false otherwise. */
            public static get supportsAsyncGPUReadback(): boolean;
            /** Is streaming of texture mip maps supported? (Read Only) */
            public static get supportsMipStreaming(): boolean;
            /** Is render texture format supported?
            * @param format The format to look up.
            * @returns True if the format is supported. 
            */
            public static SupportsRenderTextureFormat ($format: UnityEngine.RenderTextureFormat) : boolean
            /** Is blending supported on render texture format?
            * @param format The format to look up.
            * @returns True if blending is supported on the given format. 
            */
            public static SupportsBlendingOnRenderTextureFormat ($format: UnityEngine.RenderTextureFormat) : boolean
            /** Is texture format supported on this device?
            * @param format The TextureFormat format to look up.
            * @returns True if the format is supported. 
            */
            public static SupportsTextureFormat ($format: UnityEngine.TextureFormat) : boolean
            public static IsFormatSupported ($format: UnityEngine.Experimental.Rendering.GraphicsFormat, $usage: UnityEngine.Experimental.Rendering.FormatUsage) : boolean
            public constructor ()
        }
        /** Enumeration for SystemInfo.batteryStatus which represents the current status of the device's battery. */
        enum BatteryStatus
        { Unknown = 0, Charging = 1, Discharging = 2, NotCharging = 3, Full = 4 }
        /** Enumeration for SystemInfo.operatingSystemFamily. */
        enum OperatingSystemFamily
        { Other = 0, MacOSX = 1, Windows = 2, Linux = 3 }
        /** Enumeration for SystemInfo.deviceType, denotes a coarse grouping of kinds of devices. */
        enum DeviceType
        { Unknown = 0, Handheld = 1, Console = 2, Desktop = 3 }
        /** NPOT Texture2D|textures support. */
        enum NPOTSupport
        { None = 0, Restricted = 1, Full = 2 }
        /** Text file assets. */
        class TextAsset extends UnityEngine.Object
        {
        /** The text contents of the .txt file as a string. (Read Only) */
            public get text(): string;
            /** The raw bytes of the text asset. (Read Only) */
            public get bytes(): System.Array$1<number>;
            public constructor ()
            public constructor ($text: string)
        }
        /** Class for handling 3D Textures, Use this to create. */
        class Texture3D extends UnityEngine.Texture
        {
        /** The depth of the texture (Read Only). */
            public get depth(): number;
            /** The format of the pixel data in the texture (Read Only). */
            public get format(): UnityEngine.TextureFormat;
            /** Returns true if this 3D texture is Read/Write Enabled; otherwise returns false. For dynamic textures created from script, always returns true. */
            public get isReadable(): boolean;
            /** Returns an array of pixel colors representing one mip level of the 3D texture. */
            public GetPixels ($miplevel: number) : System.Array$1<UnityEngine.Color>
            public GetPixels () : System.Array$1<UnityEngine.Color>
            /** Returns an array of pixel colors representing one mip level of the 3D texture. */
            public GetPixels32 ($miplevel: number) : System.Array$1<UnityEngine.Color32>
            public GetPixels32 () : System.Array$1<UnityEngine.Color32>
            /** Sets pixel colors of a 3D texture. * @param colors The colors to set the pixels to.
            * @param miplevel The mipmap level to be affected by the new colors.
            */
            public SetPixels ($colors: System.Array$1<UnityEngine.Color>, $miplevel: number) : void
            public SetPixels ($colors: System.Array$1<UnityEngine.Color>) : void
            /** Sets pixel colors of a 3D texture. * @param colors The colors to set the pixels to.
            * @param miplevel The mipmap level to be affected by the new colors.
            */
            public SetPixels32 ($colors: System.Array$1<UnityEngine.Color32>, $miplevel: number) : void
            public SetPixels32 ($colors: System.Array$1<UnityEngine.Color32>) : void
            /** Actually apply all previous SetPixels changes. * @param updateMipmaps When set to true, mipmap levels are recalculated.
            * @param makeNoLongerReadable When set to true, system memory copy of a texture is released.
            */
            public Apply ($updateMipmaps: boolean, $makeNoLongerReadable: boolean) : void
            public Apply ($updateMipmaps: boolean) : void
            public Apply () : void
            public constructor ($width: number, $height: number, $depth: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat, $flags: UnityEngine.Experimental.Rendering.TextureCreationFlags)
            public constructor ($width: number, $height: number, $depth: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean)
            public constructor ()
        }
        /** Class for handling 2D texture arrays. */
        class Texture2DArray extends UnityEngine.Texture
        {
        /** Number of elements in a texture array (Read Only). */
            public get depth(): number;
            /** Texture format (Read Only). */
            public get format(): UnityEngine.TextureFormat;
            /** Returns true if this texture array is Read/Write Enabled; otherwise returns false. For dynamic textures created from script, always returns true. */
            public get isReadable(): boolean;
            /** Returns pixel colors of a single array slice.
            * @param arrayElement Array slice to read pixels from.
            * @param miplevel Mipmap level to read pixels from.
            * @returns Array of pixel colors. 
            */
            public GetPixels ($arrayElement: number, $miplevel: number) : System.Array$1<UnityEngine.Color>
            public GetPixels ($arrayElement: number) : System.Array$1<UnityEngine.Color>
            /** Returns pixel colors of a single array slice.
            * @param arrayElement Array slice to read pixels from.
            * @param miplevel Mipmap level to read pixels from.
            * @returns Array of pixel colors in low precision (8 bits/channel) format. 
            */
            public GetPixels32 ($arrayElement: number, $miplevel: number) : System.Array$1<UnityEngine.Color32>
            public GetPixels32 ($arrayElement: number) : System.Array$1<UnityEngine.Color32>
            /** Set pixel colors for the whole mip level. * @param colors An array of pixel colors.
            * @param arrayElement The texture array element index.
            * @param miplevel The mip level.
            */
            public SetPixels ($colors: System.Array$1<UnityEngine.Color>, $arrayElement: number, $miplevel: number) : void
            public SetPixels ($colors: System.Array$1<UnityEngine.Color>, $arrayElement: number) : void
            /** Set pixel colors for the whole mip level. * @param colors An array of pixel colors.
            * @param arrayElement The texture array element index.
            * @param miplevel The mip level.
            */
            public SetPixels32 ($colors: System.Array$1<UnityEngine.Color32>, $arrayElement: number, $miplevel: number) : void
            public SetPixels32 ($colors: System.Array$1<UnityEngine.Color32>, $arrayElement: number) : void
            /** Actually apply all previous SetPixels changes. * @param updateMipmaps When set to true, mipmap levels are recalculated.
            * @param makeNoLongerReadable When set to true, system memory copy of a texture is released.
            */
            public Apply ($updateMipmaps: boolean, $makeNoLongerReadable: boolean) : void
            public Apply ($updateMipmaps: boolean) : void
            public Apply () : void
            public constructor ($width: number, $height: number, $depth: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat, $flags: UnityEngine.Experimental.Rendering.TextureCreationFlags)
            public constructor ($width: number, $height: number, $depth: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean, $linear: boolean)
            public constructor ($width: number, $height: number, $depth: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean)
            public constructor ()
        }
        /** The interface to get time information from Unity. */
        class Time extends System.Object
        {
        /** The time at the beginning of this frame (Read Only). This is the time in seconds since the start of the game. */
            public static get time(): number;
            /** The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded. */
            public static get timeSinceLevelLoad(): number;
            /** The completion time in seconds since the last frame (Read Only). */
            public static get deltaTime(): number;
            /** The time the latest MonoBehaviour.FixedUpdate has started (Read Only). This is the time in seconds since the start of the game. */
            public static get fixedTime(): number;
            /** The timeScale-independant time for this frame (Read Only). This is the time in seconds since the start of the game. */
            public static get unscaledTime(): number;
            /** The TimeScale-independant time the latest MonoBehaviour.FixedUpdate has started (Read Only). This is the time in seconds since the start of the game. */
            public static get fixedUnscaledTime(): number;
            /** The timeScale-independent interval in seconds from the last frame to the current one (Read Only). */
            public static get unscaledDeltaTime(): number;
            /** The timeScale-independent interval in seconds from the last fixed frame to the current one (Read Only). */
            public static get fixedUnscaledDeltaTime(): number;
            /** The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour.FixedUpdate) are performed. */
            public static get fixedDeltaTime(): number;
            public static set fixedDeltaTime(value: number);
            /** The maximum time a frame can take. Physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour.FixedUpdate) will be performed only for this duration of time per frame. */
            public static get maximumDeltaTime(): number;
            public static set maximumDeltaTime(value: number);
            /** A smoothed out Time.deltaTime (Read Only). */
            public static get smoothDeltaTime(): number;
            /** The maximum time a frame can spend on particle updates. If the frame takes longer than this, then updates are split into multiple smaller updates. */
            public static get maximumParticleDeltaTime(): number;
            public static set maximumParticleDeltaTime(value: number);
            /** The scale at which the time is passing. This can be used for slow motion effects. */
            public static get timeScale(): number;
            public static set timeScale(value: number);
            /** The total number of frames that have passed (Read Only). */
            public static get frameCount(): number;
            public static get renderedFrameCount(): number;
            /** The real time in seconds since the game started (Read Only). */
            public static get realtimeSinceStartup(): number;
            /** Slows game playback time to allow screenshots to be saved between frames. */
            public static get captureFramerate(): number;
            public static set captureFramerate(value: number);
            /** Returns true if called inside a fixed time step callback (like MonoBehaviour's MonoBehaviour.FixedUpdate), otherwise returns false. */
            public static get inFixedTimeStep(): boolean;
            public constructor ()
        }
        /** Script interface for. */
        class Font extends UnityEngine.Object
        {
        /** The material used for the font display. */
            public get material(): UnityEngine.Material;
            public set material(value: UnityEngine.Material);
            public get fontNames(): System.Array$1<string>;
            public set fontNames(value: System.Array$1<string>);
            /** Is the font a dynamic font. */
            public get dynamic(): boolean;
            /** The ascent of the font. */
            public get ascent(): number;
            /** The default size of the font. */
            public get fontSize(): number;
            /** Access an array of all characters contained in the font texture. */
            public get characterInfo(): System.Array$1<UnityEngine.CharacterInfo>;
            public set characterInfo(value: System.Array$1<UnityEngine.CharacterInfo>);
            /** The line height of the font. */
            public get lineHeight(): number;
            public static add_textureRebuilt ($value: System.Action$1<UnityEngine.Font>) : void
            public static remove_textureRebuilt ($value: System.Action$1<UnityEngine.Font>) : void
            /** Creates a Font object which lets you render a font installed on the user machine.
            * @param fontname The name of the OS font to use for this font object.
            * @param size The default character size of the generated font.
            * @param fontnames Am array of names of OS fonts to use for this font object. When rendering characters using this font object, the first font which is installed on the machine, which contains the requested character will be used.
            * @returns The generate Font object. 
            */
            public static CreateDynamicFontFromOSFont ($fontname: string, $size: number) : UnityEngine.Font
            /** Creates a Font object which lets you render a font installed on the user machine.
            * @param fontname The name of the OS font to use for this font object.
            * @param size The default character size of the generated font.
            * @param fontnames Am array of names of OS fonts to use for this font object. When rendering characters using this font object, the first font which is installed on the machine, which contains the requested character will be used.
            * @returns The generate Font object. 
            */
            public static CreateDynamicFontFromOSFont ($fontnames: System.Array$1<string>, $size: number) : UnityEngine.Font
            /** Returns the maximum number of verts that the text generator may return for a given string. * @param str Input string.
            */
            public static GetMaxVertsForString ($str: string) : number
            /** Does this font have a specific character?
            * @param c The character to check for.
            * @returns Whether or not the font has the character specified. 
            */
            public HasCharacter ($c: number) : boolean
            public static GetOSInstalledFontNames () : System.Array$1<string>
            public static GetPathsToOSFonts () : System.Array$1<string>
            /** Get rendering info for a specific character. * @param ch The character you need rendering information for.
            * @param info Returns the CharacterInfo struct with the rendering information for the character (if available).
            * @param size The size of the character (default value of zero will use font default size).
            * @param style The style of the character.
            */
            public GetCharacterInfo ($ch: number, $info: $Ref<UnityEngine.CharacterInfo>, $size: number, $style: UnityEngine.FontStyle) : boolean
            /** Get rendering info for a specific character. * @param ch The character you need rendering information for.
            * @param info Returns the CharacterInfo struct with the rendering information for the character (if available).
            * @param size The size of the character (default value of zero will use font default size).
            * @param style The style of the character.
            */
            public GetCharacterInfo ($ch: number, $info: $Ref<UnityEngine.CharacterInfo>, $size: number) : boolean
            /** Get rendering info for a specific character. * @param ch The character you need rendering information for.
            * @param info Returns the CharacterInfo struct with the rendering information for the character (if available).
            * @param size The size of the character (default value of zero will use font default size).
            * @param style The style of the character.
            */
            public GetCharacterInfo ($ch: number, $info: $Ref<UnityEngine.CharacterInfo>) : boolean
            /** Request characters to be added to the font texture (dynamic fonts only). * @param characters The characters which are needed to be in the font texture.
            * @param size The size of the requested characters (the default value of zero will use the font's default size).
            * @param style The style of the requested characters.
            */
            public RequestCharactersInTexture ($characters: string, $size: number, $style: UnityEngine.FontStyle) : void
            public RequestCharactersInTexture ($characters: string, $size: number) : void
            public RequestCharactersInTexture ($characters: string) : void
            public constructor ()
            public constructor ($name: string)
        }
        /** Specification for how to render a character from the font texture. See Font.characterInfo. */
        class CharacterInfo extends System.ValueType
        {
        }
        /** Font Style applied to GUI Texts, Text Meshes or GUIStyles. */
        enum FontStyle
        { Normal = 0, Bold = 1, Italic = 2, BoldAndItalic = 3 }
        /** Script interface for Particle Systems. */
        class ParticleSystem extends UnityEngine.Component
        {
        /** Determines whether the Particle System is playing. */
            public get isPlaying(): boolean;
            /** Determines whether the Particle System is emitting particles. A Particle System may stop emitting when its emission module has finished, it has been paused or if the system has been stopped using ParticleSystem.Stop|Stop with the ParticleSystemStopBehavior.StopEmitting|StopEmitting flag. Resume emitting by calling ParticleSystem.Play|Play. */
            public get isEmitting(): boolean;
            /** Determines whether the Particle System is stopped. */
            public get isStopped(): boolean;
            /** Determines whether the Particle System is paused. */
            public get isPaused(): boolean;
            /** The current number of particles (Read Only). */
            public get particleCount(): number;
            /** Playback position in seconds. */
            public get time(): number;
            public set time(value: number);
            /** Override the random seed used for the Particle System emission. */
            public get randomSeed(): number;
            public set randomSeed(value: number);
            /** Controls whether the Particle System uses an automatically-generated random number to seed the random number generator. */
            public get useAutoRandomSeed(): boolean;
            public set useAutoRandomSeed(value: boolean);
            /** Does this system support Procedural Simulation? */
            public get proceduralSimulationSupported(): boolean;
            /** Access the main Particle System settings. */
            public get main(): UnityEngine.ParticleSystem.MainModule;
            /** Script interface for the Particle System emission module. */
            public get emission(): UnityEngine.ParticleSystem.EmissionModule;
            /** Script interface for the Particle System Shape module. */
            public get shape(): UnityEngine.ParticleSystem.ShapeModule;
            /** Script interface for the Particle System Velocity over Lifetime module. */
            public get velocityOverLifetime(): UnityEngine.ParticleSystem.VelocityOverLifetimeModule;
            /** Script interface for the Particle System Limit Velocity over Lifetime module. */
            public get limitVelocityOverLifetime(): UnityEngine.ParticleSystem.LimitVelocityOverLifetimeModule;
            /** Script interface for the Particle System velocity inheritance module. */
            public get inheritVelocity(): UnityEngine.ParticleSystem.InheritVelocityModule;
            /** Script interface for the Particle System force over lifetime module. */
            public get forceOverLifetime(): UnityEngine.ParticleSystem.ForceOverLifetimeModule;
            /** Script interface for the Particle System color over lifetime module. */
            public get colorOverLifetime(): UnityEngine.ParticleSystem.ColorOverLifetimeModule;
            /** Script interface for the Particle System color by lifetime module. */
            public get colorBySpeed(): UnityEngine.ParticleSystem.ColorBySpeedModule;
            /** Script interface for the Particle System Size over Lifetime module. */
            public get sizeOverLifetime(): UnityEngine.ParticleSystem.SizeOverLifetimeModule;
            /** Script interface for the Particle System Size by Speed module. */
            public get sizeBySpeed(): UnityEngine.ParticleSystem.SizeBySpeedModule;
            /** Script interface for the Particle System Rotation over Lifetime module. */
            public get rotationOverLifetime(): UnityEngine.ParticleSystem.RotationOverLifetimeModule;
            /** Script interface for the Particle System Rotation by Speed  module. */
            public get rotationBySpeed(): UnityEngine.ParticleSystem.RotationBySpeedModule;
            /** Script interface for the Particle System external forces module. */
            public get externalForces(): UnityEngine.ParticleSystem.ExternalForcesModule;
            /** Script interface for the Particle System Noise module. */
            public get noise(): UnityEngine.ParticleSystem.NoiseModule;
            /** Script interface for the Particle System collision module. */
            public get collision(): UnityEngine.ParticleSystem.CollisionModule;
            /** Script interface for the Particle System Trigger module. */
            public get trigger(): UnityEngine.ParticleSystem.TriggerModule;
            /** Script interface for the Particle System Sub Emitters module. */
            public get subEmitters(): UnityEngine.ParticleSystem.SubEmittersModule;
            /** Script interface for the Particle System Texture Sheet Animation module. */
            public get textureSheetAnimation(): UnityEngine.ParticleSystem.TextureSheetAnimationModule;
            /** Script interface for the Particle System Lights module. */
            public get lights(): UnityEngine.ParticleSystem.LightsModule;
            /** Script interface for the Particle System Trails module. */
            public get trails(): UnityEngine.ParticleSystem.TrailModule;
            /** Script interface for the Particle System Custom Data module. */
            public get customData(): UnityEngine.ParticleSystem.CustomDataModule;
            public SetCustomParticleData ($customData: System.Collections.Generic.List$1<UnityEngine.Vector4>, $streamIndex: UnityEngine.ParticleSystemCustomData) : void
            public GetCustomParticleData ($customData: System.Collections.Generic.List$1<UnityEngine.Vector4>, $streamIndex: UnityEngine.ParticleSystemCustomData) : number
            /** Triggers the specified sub emitter on all particles of the Particle System. * @param subEmitterIndex Index of the sub emitter to trigger.
            */
            public TriggerSubEmitter ($subEmitterIndex: number) : void
            public TriggerSubEmitter ($subEmitterIndex: number, $particle: $Ref<UnityEngine.ParticleSystem.Particle>) : void
            public TriggerSubEmitter ($subEmitterIndex: number, $particles: System.Collections.Generic.List$1<UnityEngine.ParticleSystem.Particle>) : void
            public SetParticles ($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number) : void
            public SetParticles ($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number) : void
            public SetParticles ($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>) : void
            public SetParticles ($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number) : void
            public SetParticles ($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number) : void
            public SetParticles ($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>) : void
            public GetParticles ($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number) : number
            public GetParticles ($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number) : number
            public GetParticles ($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>) : number
            public GetParticles ($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number) : number
            public GetParticles ($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number) : number
            public GetParticles ($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>) : number
            /** Fast-forwards the Particle System by simulating particles over the given period of time, then pauses it. * @param t Time period in seconds to advance the ParticleSystem simulation by. If restart is true, the ParticleSystem will be reset to 0 time, and then advanced by this value. If restart is false, the ParticleSystem simulation will be advanced in time from its current state by this value.
            * @param withChildren Fast-forward all child Particle Systems as well.
            * @param restart Restart and start from the beginning.
            * @param fixedTimeStep Only update the system at fixed intervals, based on the value in "Fixed Time" in the Time options.
            */
            public Simulate ($t: number, $withChildren: boolean, $restart: boolean, $fixedTimeStep: boolean) : void
            /** Fast-forwards the Particle System by simulating particles over the given period of time, then pauses it. * @param t Time period in seconds to advance the ParticleSystem simulation by. If restart is true, the ParticleSystem will be reset to 0 time, and then advanced by this value. If restart is false, the ParticleSystem simulation will be advanced in time from its current state by this value.
            * @param withChildren Fast-forward all child Particle Systems as well.
            * @param restart Restart and start from the beginning.
            * @param fixedTimeStep Only update the system at fixed intervals, based on the value in "Fixed Time" in the Time options.
            */
            public Simulate ($t: number, $withChildren: boolean, $restart: boolean) : void
            /** Fast-forwards the Particle System by simulating particles over the given period of time, then pauses it. * @param t Time period in seconds to advance the ParticleSystem simulation by. If restart is true, the ParticleSystem will be reset to 0 time, and then advanced by this value. If restart is false, the ParticleSystem simulation will be advanced in time from its current state by this value.
            * @param withChildren Fast-forward all child Particle Systems as well.
            * @param restart Restart and start from the beginning.
            * @param fixedTimeStep Only update the system at fixed intervals, based on the value in "Fixed Time" in the Time options.
            */
            public Simulate ($t: number, $withChildren: boolean) : void
            /** Fast-forwards the Particle System by simulating particles over the given period of time, then pauses it. * @param t Time period in seconds to advance the ParticleSystem simulation by. If restart is true, the ParticleSystem will be reset to 0 time, and then advanced by this value. If restart is false, the ParticleSystem simulation will be advanced in time from its current state by this value.
            * @param withChildren Fast-forward all child Particle Systems as well.
            * @param restart Restart and start from the beginning.
            * @param fixedTimeStep Only update the system at fixed intervals, based on the value in "Fixed Time" in the Time options.
            */
            public Simulate ($t: number) : void
            /** Starts the Particle System. * @param withChildren Play all child Particle Systems as well.
            */
            public Play ($withChildren: boolean) : void
            public Play () : void
            /** Pauses the system so no new particles are emitted and the existing particles are not updated. * @param withChildren Pause all child Particle Systems as well.
            */
            public Pause ($withChildren: boolean) : void
            public Pause () : void
            /** Stops playing the Particle System using the supplied stop behaviour. * @param withChildren Stop all child Particle Systems as well.
            * @param stopBehavior Stop emitting or stop emitting and clear the system.
            */
            public Stop ($withChildren: boolean, $stopBehavior: UnityEngine.ParticleSystemStopBehavior) : void
            /** Stops playing the Particle System using the supplied stop behaviour. * @param withChildren Stop all child Particle Systems as well.
            * @param stopBehavior Stop emitting or stop emitting and clear the system.
            */
            public Stop ($withChildren: boolean) : void
            public Stop () : void
            /** Remove all particles in the Particle System. * @param withChildren Clear all child Particle Systems as well.
            */
            public Clear ($withChildren: boolean) : void
            public Clear () : void
            /** Does the Particle System contain any live particles, or will it produce more?
            * @param withChildren Check all child Particle Systems as well.
            * @returns True if the Particle System contains live particles or is still creating new particles. False if the Particle System has stopped emitting particles and all particles are dead. 
            */
            public IsAlive ($withChildren: boolean) : boolean
            public IsAlive () : boolean
            /** Emit count particles immediately. * @param count Number of particles to emit.
            */
            public Emit ($count: number) : void
            public Emit ($emitParams: UnityEngine.ParticleSystem.EmitParams, $count: number) : void
            public static ResetPreMappedBufferMemory () : void
            public constructor ()
        }
        /** Which stream of custom particle data to set. */
        enum ParticleSystemCustomData
        { Custom1 = 0, Custom2 = 1 }
        /** The space to simulate particles in. */
        enum ParticleSystemSimulationSpace
        { Local = 0, World = 1, Custom = 2 }
        /** Control how particle systems apply transform scale. */
        enum ParticleSystemScalingMode
        { Hierarchy = 0, Local = 1, Shape = 2 }
        /** The behavior to apply when calling ParticleSystem.Stop|Stop. */
        enum ParticleSystemStopBehavior
        { StopEmittingAndClear = 0, StopEmitting = 1 }
        /** RenderMode for the Canvas. */
        enum RenderMode
        { ScreenSpaceOverlay = 0, ScreenSpaceCamera = 1, WorldSpace = 2 }
        /** A base class of all colliders. */
        class Collider extends UnityEngine.Component
        {
        }
        /** Structure used to get information back from a raycast. */
        class RaycastHit extends System.ValueType
        {
        }
        /** A mesh collider allows you to do between meshes and primitives. */
        class MeshCollider extends UnityEngine.Collider
        {
        }
        /** Renders meshes inserted by the MeshFilter or TextMesh. */
        class MeshRenderer extends UnityEngine.Renderer
        {
        }
        /** General functionality for all renderers. */
        class Renderer extends UnityEngine.Component
        {
        }
        /** Represents a Sprite object for use in 2D gameplay. */
        class Sprite extends UnityEngine.Object
        {
        }
        /** Types of modifier key that can be active during a keystroke event. */
        enum EventModifiers
        { None = 0, Shift = 1, Control = 2, Alt = 4, Command = 8, Numeric = 16, CapsLock = 32, FunctionKey = 64 }
    }
    namespace UnityEngine.SceneManagement {
        /** Run-time data structure for *.unity file. */
        class Scene extends System.ValueType
        {
        }
    }
    namespace UnityEngine.Playables {
        /** Use the PlayableGraph to manage Playable creations and destructions. */
        class PlayableGraph extends System.ValueType
        {
        }
    }
    namespace UnityEngine.AudioClip {
        interface PCMReaderCallback
        { (data: System.Array$1<number>) : void; }
        var PCMReaderCallback: { new (func: (data: System.Array$1<number>) => void): PCMReaderCallback; }
        interface PCMSetPositionCallback
        { (position: number) : void; }
        var PCMSetPositionCallback: { new (func: (position: number) => void): PCMSetPositionCallback; }
    }
    namespace UnityEngine.Audio {
        /** Object representing a group in the mixer. */
        class AudioMixerGroup extends UnityEngine.Object implements UnityEngine.Internal.ISubAssetNotDuplicatable
        {
        }
    }
    namespace UnityEngine.Internal {
        interface ISubAssetNotDuplicatable
        {
        }
    }
    namespace UnityEngine.Rendering {
        /** Texture "dimension" (type). */
        enum TextureDimension
        { Unknown = -1, None = 0, Any = 1, Tex2D = 2, Tex3D = 3, Cube = 4, Tex2DArray = 5, CubeArray = 6 }
        /** Opaque object sorting mode of a Camera. */
        enum OpaqueSortMode
        { Default = 0, FrontToBack = 1, NoDistanceSort = 2 }
        /** Defines a place in camera's rendering to attach Rendering.CommandBuffer objects to. */
        enum CameraEvent
        { BeforeDepthTexture = 0, AfterDepthTexture = 1, BeforeDepthNormalsTexture = 2, AfterDepthNormalsTexture = 3, BeforeGBuffer = 4, AfterGBuffer = 5, BeforeLighting = 6, AfterLighting = 7, BeforeFinalPass = 8, AfterFinalPass = 9, BeforeForwardOpaque = 10, AfterForwardOpaque = 11, BeforeImageEffectsOpaque = 12, AfterImageEffectsOpaque = 13, BeforeSkybox = 14, AfterSkybox = 15, BeforeForwardAlpha = 16, AfterForwardAlpha = 17, BeforeImageEffects = 18, AfterImageEffects = 19, AfterEverything = 20, BeforeReflections = 21, AfterReflections = 22, BeforeHaloAndLensFlares = 23, AfterHaloAndLensFlares = 24 }
        /** List of graphics commands to execute. */
        class CommandBuffer extends System.Object implements System.IDisposable
        {
        }
        /** Describes the desired characteristics with respect to prioritisation and load balancing of the queue that a command buffer being submitted via Graphics.ExecuteCommandBufferAsync or [[ScriptableRenderContext.ExecuteCommandBufferAsync] should be sent to. */
        enum ComputeQueueType
        { Default = 0, Background = 1, Urgent = 2 }
        enum ShaderHardwareTier
        { Tier1 = 0, Tier2 = 1, Tier3 = 2 }
        /** Graphics Tier.
        See Also: Graphics.activeTier. */
        enum GraphicsTier
        { Tier1 = 0, Tier2 = 1, Tier3 = 2 }
        /** Used to manage synchronisation between tasks on async compute queues and the graphics queue. */
        class GPUFence extends System.ValueType
        {
        }
        /** Broadly describes the stages of processing a draw call on the GPU. */
        enum SynchronisationStage
        { VertexProcessing = 0, PixelProcessing = 1 }
        /** How shadows are cast from this object. */
        enum ShadowCastingMode
        { Off = 0, On = 1, TwoSided = 2, ShadowsOnly = 3 }
        /** Light probe interpolation type. */
        enum LightProbeUsage
        { Off = 0, BlendProbes = 1, UseProxyVolume = 2, CustomProvided = 4 }
        /** Format of the mesh index buffer data. */
        enum IndexFormat
        { UInt16 = 0, UInt32 = 1 }
        /** Graphics device API type. */
        enum GraphicsDeviceType
        { OpenGL2 = 0, Direct3D9 = 1, Direct3D11 = 2, PlayStation3 = 3, Null = 4, Xbox360 = 6, OpenGLES2 = 8, OpenGLES3 = 11, PlayStationVita = 12, PlayStation4 = 13, XboxOne = 14, PlayStationMobile = 15, Metal = 16, OpenGLCore = 17, Direct3D12 = 18, N3DS = 19, Vulkan = 21, Switch = 22, XboxOneD3D12 = 23 }
        /** Support for various Graphics.CopyTexture cases. */
        enum CopyTextureSupport
        { None = 0, Basic = 1, Copy3D = 2, DifferentTypes = 4, TextureToRT = 8, RTToTexture = 16 }
        /** Blend mode for controlling the blending. */
        enum BlendMode
        { Zero = 0, One = 1, DstColor = 2, SrcColor = 3, OneMinusDstColor = 4, SrcAlpha = 5, OneMinusSrcColor = 6, DstAlpha = 7, OneMinusDstAlpha = 8, SrcAlphaSaturate = 9, OneMinusSrcAlpha = 10 }
    }
    namespace UnityEngine.Application {
        interface AdvertisingIdentifierCallback
        { (advertisingId: string, trackingEnabled: boolean, errorMsg: string) : void; }
        var AdvertisingIdentifierCallback: { new (func: (advertisingId: string, trackingEnabled: boolean, errorMsg: string) => void): AdvertisingIdentifierCallback; }
        interface LowMemoryCallback
        { () : void; }
        var LowMemoryCallback: { new (func: () => void): LowMemoryCallback; }
        interface LogCallback
        { (condition: string, stackTrace: string, type: UnityEngine.LogType) : void; }
        var LogCallback: { new (func: (condition: string, stackTrace: string, type: UnityEngine.LogType) => void): LogCallback; }
    }
    namespace UnityEngine.Events {
        /** Zero argument delegate used by UnityEvents. */
        interface UnityAction
        { () : void; }
        var UnityAction: { new (func: () => void): UnityAction; }
    }
    namespace UnityEngine.Camera {
        interface CameraCallback
        { (cam: UnityEngine.Camera) : void; }
        var CameraCallback: { new (func: (cam: UnityEngine.Camera) => void): CameraCallback; }
        enum GateFitMode
        { Vertical = 1, Horizontal = 2, Fill = 3, Overscan = 4, None = 0 }
        enum MonoOrStereoscopicEye
        { Left = 0, Right = 1, Mono = 2 }
        class GateFitParameters extends System.ValueType
        {
        }
        enum StereoscopicEye
        { Left = 0, Right = 1 }
    }
    namespace UnityEngine.Experimental.Rendering {
        /** Use this format to create either Textures or RenderTextures from scripts. */
        enum GraphicsFormat
        { None = 0, R8_SRGB = 1, R8G8_SRGB = 2, R8G8B8_SRGB = 3, R8G8B8A8_SRGB = 4, R8_UNorm = 5, R8G8_UNorm = 6, R8G8B8_UNorm = 7, R8G8B8A8_UNorm = 8, R8_SNorm = 9, R8G8_SNorm = 10, R8G8B8_SNorm = 11, R8G8B8A8_SNorm = 12, R8_UInt = 13, R8G8_UInt = 14, R8G8B8_UInt = 15, R8G8B8A8_UInt = 16, R8_SInt = 17, R8G8_SInt = 18, R8G8B8_SInt = 19, R8G8B8A8_SInt = 20, R16_UNorm = 21, R16G16_UNorm = 22, R16G16B16_UNorm = 23, R16G16B16A16_UNorm = 24, R16_SNorm = 25, R16G16_SNorm = 26, R16G16B16_SNorm = 27, R16G16B16A16_SNorm = 28, R16_UInt = 29, R16G16_UInt = 30, R16G16B16_UInt = 31, R16G16B16A16_UInt = 32, R16_SInt = 33, R16G16_SInt = 34, R16G16B16_SInt = 35, R16G16B16A16_SInt = 36, R32_UInt = 37, R32G32_UInt = 38, R32G32B32_UInt = 39, R32G32B32A32_UInt = 40, R32_SInt = 41, R32G32_SInt = 42, R32G32B32_SInt = 43, R32G32B32A32_SInt = 44, R16_SFloat = 45, R16G16_SFloat = 46, R16G16B16_SFloat = 47, R16G16B16A16_SFloat = 48, R32_SFloat = 49, R32G32_SFloat = 50, R32G32B32_SFloat = 51, R32G32B32A32_SFloat = 52, B8G8R8_SRGB = 56, B8G8R8A8_SRGB = 57, B8G8R8_UNorm = 58, B8G8R8A8_UNorm = 59, B8G8R8_SNorm = 60, B8G8R8A8_SNorm = 61, B8G8R8_UInt = 62, B8G8R8A8_UInt = 63, B8G8R8_SInt = 64, B8G8R8A8_SInt = 65, R4G4B4A4_UNormPack16 = 66, B4G4R4A4_UNormPack16 = 67, R5G6B5_UNormPack16 = 68, B5G6R5_UNormPack16 = 69, R5G5B5A1_UNormPack16 = 70, B5G5R5A1_UNormPack16 = 71, A1R5G5B5_UNormPack16 = 72, E5B9G9R9_UFloatPack32 = 73, B10G11R11_UFloatPack32 = 74, A2B10G10R10_UNormPack32 = 75, A2B10G10R10_UIntPack32 = 76, A2B10G10R10_SIntPack32 = 77, A2R10G10B10_UNormPack32 = 78, A2R10G10B10_UIntPack32 = 79, A2R10G10B10_SIntPack32 = 80, A2R10G10B10_XRSRGBPack32 = 81, A2R10G10B10_XRUNormPack32 = 82, R10G10B10_XRSRGBPack32 = 83, R10G10B10_XRUNormPack32 = 84, A10R10G10B10_XRSRGBPack32 = 85, A10R10G10B10_XRUNormPack32 = 86, D16_UNorm = 90, D24_UNorm = 91, D24_UNorm_S8_UInt = 92, D32_SFloat = 93, D32_SFloat_S8_Uint = 94, S8_Uint = 95, RGB_DXT1_SRGB = 96, RGBA_DXT1_SRGB = 96, RGB_DXT1_UNorm = 97, RGBA_DXT1_UNorm = 97, RGBA_DXT3_SRGB = 98, RGBA_DXT3_UNorm = 99, RGBA_DXT5_SRGB = 100, RGBA_DXT5_UNorm = 101, R_BC4_UNorm = 102, R_BC4_SNorm = 103, RG_BC5_UNorm = 104, RG_BC5_SNorm = 105, RGB_BC6H_UFloat = 106, RGB_BC6H_SFloat = 107, RGBA_BC7_SRGB = 108, RGBA_BC7_UNorm = 109, RGB_PVRTC_2Bpp_SRGB = 110, RGB_PVRTC_2Bpp_UNorm = 111, RGB_PVRTC_4Bpp_SRGB = 112, RGB_PVRTC_4Bpp_UNorm = 113, RGBA_PVRTC_2Bpp_SRGB = 114, RGBA_PVRTC_2Bpp_UNorm = 115, RGBA_PVRTC_4Bpp_SRGB = 116, RGBA_PVRTC_4Bpp_UNorm = 117, RGB_ETC_UNorm = 118, RGB_ETC2_SRGB = 119, RGB_ETC2_UNorm = 120, RGB_A1_ETC2_SRGB = 121, RGB_A1_ETC2_UNorm = 122, RGBA_ETC2_SRGB = 123, RGBA_ETC2_UNorm = 124, R_EAC_UNorm = 125, R_EAC_SNorm = 126, RG_EAC_UNorm = 127, RG_EAC_SNorm = 128, RGBA_ASTC4X4_SRGB = 129, RGBA_ASTC4X4_UNorm = 130, RGBA_ASTC5X5_SRGB = 131, RGBA_ASTC5X5_UNorm = 132, RGBA_ASTC6X6_SRGB = 133, RGBA_ASTC6X6_UNorm = 134, RGBA_ASTC8X8_SRGB = 135, RGBA_ASTC8X8_UNorm = 136, RGBA_ASTC10X10_SRGB = 137, RGBA_ASTC10X10_UNorm = 138, RGBA_ASTC12X12_SRGB = 139, RGBA_ASTC12X12_UNorm = 140 }
        enum TextureCreationFlags
        { None = 0, MipChain = 1, Crunch = 64 }
        /** Use this format usages to figure out the capabilities of specific GraphicsFormat */
        enum FormatUsage
        { Sample = 0, Linear = 1, Render = 3, Blend = 4, LoadStore = 8, MSAA2x = 9, MSAA4x = 10, MSAA8x = 11 }
    }
    namespace UnityEngine.Display {
        interface DisplaysUpdatedDelegate
        { () : void; }
        var DisplaysUpdatedDelegate: { new (func: () => void): DisplaysUpdatedDelegate; }
    }
    namespace Unity.Collections {
        class NativeArray$1<T> extends System.ValueType implements System.Collections.IEnumerable, System.IDisposable, System.IEquatable$1<Unity.Collections.NativeArray$1<T>>, System.Collections.Generic.IEnumerable$1<T>
        {
        }
    }
    namespace UnityEngine.Random {
        class State extends System.ValueType
        {
        }
    }
    namespace UnityEngine.Font {
        interface FontTextureRebuildCallback
        { () : void; }
        var FontTextureRebuildCallback: { new (func: () => void): FontTextureRebuildCallback; }
    }
    namespace UnityEngine.ParticleSystem {
        class Particle extends System.ValueType
        {
        }
        class MainModule extends System.ValueType
        {
        }
        class EmissionModule extends System.ValueType
        {
        }
        class ShapeModule extends System.ValueType
        {
        }
        class VelocityOverLifetimeModule extends System.ValueType
        {
        }
        class LimitVelocityOverLifetimeModule extends System.ValueType
        {
        }
        class InheritVelocityModule extends System.ValueType
        {
        }
        class ForceOverLifetimeModule extends System.ValueType
        {
        }
        class ColorOverLifetimeModule extends System.ValueType
        {
        }
        class ColorBySpeedModule extends System.ValueType
        {
        }
        class SizeOverLifetimeModule extends System.ValueType
        {
        }
        class SizeBySpeedModule extends System.ValueType
        {
        }
        class RotationOverLifetimeModule extends System.ValueType
        {
        }
        class RotationBySpeedModule extends System.ValueType
        {
        }
        class ExternalForcesModule extends System.ValueType
        {
        }
        class NoiseModule extends System.ValueType
        {
        }
        class CollisionModule extends System.ValueType
        {
        }
        class TriggerModule extends System.ValueType
        {
        }
        class SubEmittersModule extends System.ValueType
        {
        }
        class TextureSheetAnimationModule extends System.ValueType
        {
        }
        class LightsModule extends System.ValueType
        {
        }
        class TrailModule extends System.ValueType
        {
        }
        class CustomDataModule extends System.ValueType
        {
        }
        class EmitParams extends System.ValueType
        {
        }
    }
    namespace FairyEditor.Dialog {
        class DialogBase extends FairyGUI.Window implements FairyGUI.IEventDispatcher
        {
            public __actionHandler : System.Action
            public __cancelHandler : System.Action
            public Center ($restraint: boolean) : void
            public Center():void            
            public ActionHandler () : void
            public CancelHandler () : void
            public constructor ()
        }
    }
    namespace FairyGUI {
        class Window extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public bringToFontOnClick : boolean
            public __onInit : System.Action
            public __onShown : System.Action
            public __onHide : System.Action
            public __doShowAnimation : System.Action
            public __doHideAnimation : System.Action
            public get contentPane(): FairyGUI.GComponent;
            public set contentPane(value: FairyGUI.GComponent);
            public get frame(): FairyGUI.GComponent;
            public get closeButton(): FairyGUI.GObject;
            public set closeButton(value: FairyGUI.GObject);
            public get dragArea(): FairyGUI.GObject;
            public set dragArea(value: FairyGUI.GObject);
            public get contentArea(): FairyGUI.GObject;
            public set contentArea(value: FairyGUI.GObject);
            public get modalWaitingPane(): FairyGUI.GObject;
            public get isShowing(): boolean;
            public get isTop(): boolean;
            public get modal(): boolean;
            public set modal(value: boolean);
            public get modalWaiting(): boolean;
            public AddUISource ($source: FairyGUI.IUISource) : void
            public Show () : void
            public ShowOn ($r: FairyGUI.GRoot) : void
            public Hide () : void
            public HideImmediately () : void
            public CenterOn ($r: FairyGUI.GRoot, $restraint: boolean) : void
            public ToggleStatus () : void
            public BringToFront () : void
            public ShowModalWait () : void
            public ShowModalWait ($requestingCmd: number) : void
            public CloseModalWait () : boolean
            public CloseModalWait ($requestingCmd: number) : boolean
            public Init () : void
            public constructor ()
        }
        class GComponent extends FairyGUI.GObject implements FairyGUI.IEventDispatcher
        {
            public __onConstruct : System.Action
            public __onDispose : System.Action
            public get rootContainer(): FairyGUI.Container;
            public get container(): FairyGUI.Container;
            public get scrollPane(): FairyGUI.ScrollPane;
            public get onDrop(): FairyGUI.EventListener;
            public get fairyBatching(): boolean;
            public set fairyBatching(value: boolean);
            public get opaque(): boolean;
            public set opaque(value: boolean);
            public get margin(): FairyGUI.Margin;
            public set margin(value: FairyGUI.Margin);
            public get childrenRenderOrder(): FairyGUI.ChildrenRenderOrder;
            public set childrenRenderOrder(value: FairyGUI.ChildrenRenderOrder);
            public get apexIndex(): number;
            public set apexIndex(value: number);
            public get tabStopChildren(): boolean;
            public set tabStopChildren(value: boolean);
            public get numChildren(): number;
            public get Controllers(): System.Collections.Generic.List$1<FairyGUI.Controller>;
            public get clipSoftness(): UnityEngine.Vector2;
            public set clipSoftness(value: UnityEngine.Vector2);
            public get mask(): FairyGUI.DisplayObject;
            public set mask(value: FairyGUI.DisplayObject);
            public get reversedMask(): boolean;
            public set reversedMask(value: boolean);
            public get baseUserData(): string;
            public get viewWidth(): number;
            public set viewWidth(value: number);
            public get viewHeight(): number;
            public set viewHeight(value: number);
            public InvalidateBatchingState ($childChanged: boolean) : void
            public AddChild ($child: FairyGUI.GObject) : FairyGUI.GObject
            public AddChildAt ($child: FairyGUI.GObject, $index: number) : FairyGUI.GObject
            public RemoveChild ($child: FairyGUI.GObject) : FairyGUI.GObject
            public RemoveChild ($child: FairyGUI.GObject, $dispose: boolean) : FairyGUI.GObject
            public RemoveChildAt ($index: number) : FairyGUI.GObject
            public RemoveChildAt ($index: number, $dispose: boolean) : FairyGUI.GObject
            public RemoveChildren () : void
            public RemoveChildren ($beginIndex: number, $endIndex: number, $dispose: boolean) : void
            public GetChildAt ($index: number) : FairyGUI.GObject
            public GetChild ($name: string) : FairyGUI.GObject
            public GetChildByPath ($path: string) : FairyGUI.GObject
            public GetVisibleChild ($name: string) : FairyGUI.GObject
            public GetChildInGroup ($group: FairyGUI.GGroup, $name: string) : FairyGUI.GObject
            public GetChildren () : System.Array$1<FairyGUI.GObject>
            public GetChildIndex ($child: FairyGUI.GObject) : number
            public SetChildIndex ($child: FairyGUI.GObject, $index: number) : void
            public SetChildIndexBefore ($child: FairyGUI.GObject, $index: number) : number
            public SwapChildren ($child1: FairyGUI.GObject, $child2: FairyGUI.GObject) : void
            public SwapChildrenAt ($index1: number, $index2: number) : void
            public IsAncestorOf ($obj: FairyGUI.GObject) : boolean
            public ChangeChildrenOrder ($objs: System.Collections.Generic.IList$1<FairyGUI.GObject>) : void
            public AddController ($controller: FairyGUI.Controller) : void
            public GetControllerAt ($index: number) : FairyGUI.Controller
            public GetController ($name: string) : FairyGUI.Controller
            public RemoveController ($c: FairyGUI.Controller) : void
            public GetTransitionAt ($index: number) : FairyGUI.Transition
            public GetTransition ($name: string) : FairyGUI.Transition
            public IsChildInView ($child: FairyGUI.GObject) : boolean
            public GetFirstChildInView () : number
            public SetBoundsChangedFlag () : void
            public EnsureBoundsCorrect () : void
            public GetSnappingPosition ($xValue: $Ref<number>, $yValue: $Ref<number>) : void
            public GetSnappingPositionWithDir ($xValue: $Ref<number>, $yValue: $Ref<number>, $xDir: number, $yDir: number) : void
            public ConstructFromXML ($xml: FairyGUI.Utils.XML) : void
            public constructor ()
            public InvalidateBatchingState () : void
        }
        class GObject extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public name : string
            public data : any
            public sourceWidth : number
            public sourceHeight : number
            public initWidth : number
            public initHeight : number
            public minWidth : number
            public maxWidth : number
            public minHeight : number
            public maxHeight : number
            public dragBounds : System.Nullable$1<UnityEngine.Rect>
            public packageItem : FairyGUI.PackageItem
            public get id(): string;
            public get relations(): FairyGUI.Relations;
            public get parent(): FairyGUI.GComponent;
            public get displayObject(): FairyGUI.DisplayObject;
            public static get draggingObject(): FairyGUI.GObject;
            public get onClick(): FairyGUI.EventListener;
            public get onRightClick(): FairyGUI.EventListener;
            public get onTouchBegin(): FairyGUI.EventListener;
            public get onTouchMove(): FairyGUI.EventListener;
            public get onTouchEnd(): FairyGUI.EventListener;
            public get onRollOver(): FairyGUI.EventListener;
            public get onRollOut(): FairyGUI.EventListener;
            public get onAddedToStage(): FairyGUI.EventListener;
            public get onRemovedFromStage(): FairyGUI.EventListener;
            public get onKeyDown(): FairyGUI.EventListener;
            public get onClickLink(): FairyGUI.EventListener;
            public get onPositionChanged(): FairyGUI.EventListener;
            public get onSizeChanged(): FairyGUI.EventListener;
            public get onDragStart(): FairyGUI.EventListener;
            public get onDragMove(): FairyGUI.EventListener;
            public get onDragEnd(): FairyGUI.EventListener;
            public get onGearStop(): FairyGUI.EventListener;
            public get onFocusIn(): FairyGUI.EventListener;
            public get onFocusOut(): FairyGUI.EventListener;
            public get x(): number;
            public set x(value: number);
            public get y(): number;
            public set y(value: number);
            public get z(): number;
            public set z(value: number);
            public get xy(): UnityEngine.Vector2;
            public set xy(value: UnityEngine.Vector2);
            public get position(): UnityEngine.Vector3;
            public set position(value: UnityEngine.Vector3);
            public get width(): number;
            public set width(value: number);
            public get height(): number;
            public set height(value: number);
            public get size(): UnityEngine.Vector2;
            public set size(value: UnityEngine.Vector2);
            public get actualWidth(): number;
            public get actualHeight(): number;
            public get xMin(): number;
            public set xMin(value: number);
            public get yMin(): number;
            public set yMin(value: number);
            public get scaleX(): number;
            public set scaleX(value: number);
            public get scaleY(): number;
            public set scaleY(value: number);
            public get scale(): UnityEngine.Vector2;
            public set scale(value: UnityEngine.Vector2);
            public get skew(): UnityEngine.Vector2;
            public set skew(value: UnityEngine.Vector2);
            public get pivotX(): number;
            public set pivotX(value: number);
            public get pivotY(): number;
            public set pivotY(value: number);
            public get pivot(): UnityEngine.Vector2;
            public set pivot(value: UnityEngine.Vector2);
            public get pivotAsAnchor(): boolean;
            public set pivotAsAnchor(value: boolean);
            public get touchable(): boolean;
            public set touchable(value: boolean);
            public get grayed(): boolean;
            public set grayed(value: boolean);
            public get enabled(): boolean;
            public set enabled(value: boolean);
            public get rotation(): number;
            public set rotation(value: number);
            public get rotationX(): number;
            public set rotationX(value: number);
            public get rotationY(): number;
            public set rotationY(value: number);
            public get alpha(): number;
            public set alpha(value: number);
            public get visible(): boolean;
            public set visible(value: boolean);
            public get sortingOrder(): number;
            public set sortingOrder(value: number);
            public get focusable(): boolean;
            public set focusable(value: boolean);
            public get tabStop(): boolean;
            public set tabStop(value: boolean);
            public get focused(): boolean;
            public get tooltips(): string;
            public set tooltips(value: string);
            public get cursor(): string;
            public set cursor(value: string);
            public get filter(): FairyGUI.IFilter;
            public set filter(value: FairyGUI.IFilter);
            public get blendMode(): FairyGUI.BlendMode;
            public set blendMode(value: FairyGUI.BlendMode);
            public get gameObjectName(): string;
            public set gameObjectName(value: string);
            public get inContainer(): boolean;
            public get onStage(): boolean;
            public get resourceURL(): string;
            public get gearXY(): FairyGUI.GearXY;
            public get gearSize(): FairyGUI.GearSize;
            public get gearLook(): FairyGUI.GearLook;
            public get group(): FairyGUI.GGroup;
            public set group(value: FairyGUI.GGroup);
            public get root(): FairyGUI.GRoot;
            public get text(): string;
            public set text(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get draggable(): boolean;
            public set draggable(value: boolean);
            public get dragging(): boolean;
            public get isDisposed(): boolean;
            public get asImage(): FairyGUI.GImage;
            public get asCom(): FairyGUI.GComponent;
            public get asButton(): FairyGUI.GButton;
            public get asLabel(): FairyGUI.GLabel;
            public get asProgress(): FairyGUI.GProgressBar;
            public get asSlider(): FairyGUI.GSlider;
            public get asComboBox(): FairyGUI.GComboBox;
            public get asTextField(): FairyGUI.GTextField;
            public get asRichTextField(): FairyGUI.GRichTextField;
            public get asTextInput(): FairyGUI.GTextInput;
            public get asLoader(): FairyGUI.GLoader;
            public get asLoader3D(): FairyGUI.GLoader3D;
            public get asList(): FairyGUI.GList;
            public get asGraph(): FairyGUI.GGraph;
            public get asGroup(): FairyGUI.GGroup;
            public get asMovieClip(): FairyGUI.GMovieClip;
            public get asTree(): FairyGUI.GTree;
            public get treeNode(): FairyGUI.GTreeNode;
            public SetXY ($xv: number, $yv: number) : void
            public SetXY ($xv: number, $yv: number, $topLeftValue: boolean) : void
            public SetPosition ($xv: number, $yv: number, $zv: number) : void
            public Center () : void
            public Center ($restraint: boolean) : void
            public MakeFullScreen () : void
            public SetSize ($wv: number, $hv: number) : void
            public SetSize ($wv: number, $hv: number, $ignorePivot: boolean) : void
            public SetScale ($wv: number, $hv: number) : void
            public SetPivot ($xv: number, $yv: number) : void
            public SetPivot ($xv: number, $yv: number, $asAnchor: boolean) : void
            public RequestFocus () : void
            public RequestFocus ($byKey: boolean) : void
            public SetHome ($obj: FairyGUI.GObject) : void
            public GetGear ($index: number) : FairyGUI.GearBase
            public InvalidateBatchingState () : void
            public HandleControllerChanged ($c: FairyGUI.Controller) : void
            public AddRelation ($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType) : void
            public AddRelation ($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType, $usePercent: boolean) : void
            public RemoveRelation ($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType) : void
            public RemoveFromParent () : void
            public StartDrag () : void
            public StartDrag ($touchId: number) : void
            public StopDrag () : void
            public LocalToGlobal ($pt: UnityEngine.Vector2) : UnityEngine.Vector2
            public GlobalToLocal ($pt: UnityEngine.Vector2) : UnityEngine.Vector2
            public LocalToGlobal ($rect: UnityEngine.Rect) : UnityEngine.Rect
            public GlobalToLocal ($rect: UnityEngine.Rect) : UnityEngine.Rect
            public LocalToRoot ($pt: UnityEngine.Vector2, $r: FairyGUI.GRoot) : UnityEngine.Vector2
            public RootToLocal ($pt: UnityEngine.Vector2, $r: FairyGUI.GRoot) : UnityEngine.Vector2
            public WorldToLocal ($pt: UnityEngine.Vector3) : UnityEngine.Vector2
            public WorldToLocal ($pt: UnityEngine.Vector3, $camera: UnityEngine.Camera) : UnityEngine.Vector2
            public TransformPoint ($pt: UnityEngine.Vector2, $targetSpace: FairyGUI.GObject) : UnityEngine.Vector2
            public TransformRect ($rect: UnityEngine.Rect, $targetSpace: FairyGUI.GObject) : UnityEngine.Rect
            public Dispose () : void
            public ConstructFromResource () : void
            public Setup_BeforeAdd ($buffer: FairyGUI.Utils.ByteBuffer, $beginPos: number) : void
            public Setup_AfterAdd ($buffer: FairyGUI.Utils.ByteBuffer, $beginPos: number) : void
            public TweenMove ($endValue: UnityEngine.Vector2, $duration: number) : FairyGUI.GTweener
            public TweenMoveX ($endValue: number, $duration: number) : FairyGUI.GTweener
            public TweenMoveY ($endValue: number, $duration: number) : FairyGUI.GTweener
            public TweenScale ($endValue: UnityEngine.Vector2, $duration: number) : FairyGUI.GTweener
            public TweenScaleX ($endValue: number, $duration: number) : FairyGUI.GTweener
            public TweenScaleY ($endValue: number, $duration: number) : FairyGUI.GTweener
            public TweenResize ($endValue: UnityEngine.Vector2, $duration: number) : FairyGUI.GTweener
            public TweenFade ($endValue: number, $duration: number) : FairyGUI.GTweener
            public TweenRotate ($endValue: number, $duration: number) : FairyGUI.GTweener
            public constructor ()
        }
        class EventDispatcher extends System.Object implements FairyGUI.IEventDispatcher
        {
            public AddEventListener ($strType: string, $callback: FairyGUI.EventCallback1) : void
            public AddEventListener ($strType: string, $callback: FairyGUI.EventCallback0) : void
            public RemoveEventListener ($strType: string, $callback: FairyGUI.EventCallback1) : void
            public RemoveEventListener ($strType: string, $callback: FairyGUI.EventCallback0) : void
            public AddCapture ($strType: string, $callback: FairyGUI.EventCallback1) : void
            public RemoveCapture ($strType: string, $callback: FairyGUI.EventCallback1) : void
            public RemoveEventListeners () : void
            public RemoveEventListeners ($strType: string) : void
            public hasEventListeners ($strType: string) : boolean
            public isDispatching ($strType: string) : boolean
            public DispatchEvent ($strType: string) : boolean
            public DispatchEvent ($strType: string, $data: any) : boolean
            public DispatchEvent ($strType: string, $data: any, $initiator: any) : boolean
            public DispatchEvent ($context: FairyGUI.EventContext) : boolean
            public BubbleEvent ($strType: string, $data: any) : boolean
            public BroadcastEvent ($strType: string, $data: any) : boolean
            public constructor ()
        }
        interface IEventDispatcher
        {
            AddEventListener ($strType: string, $callback: FairyGUI.EventCallback0) : void
            AddEventListener ($strType: string, $callback: FairyGUI.EventCallback1) : void
            RemoveEventListener ($strType: string, $callback: FairyGUI.EventCallback0) : void
            RemoveEventListener ($strType: string, $callback: FairyGUI.EventCallback1) : void
            DispatchEvent ($context: FairyGUI.EventContext) : boolean
            DispatchEvent ($strType: string) : boolean
            DispatchEvent ($strType: string, $data: any) : boolean
            DispatchEvent ($strType: string, $data: any, $initiator: any) : boolean
        }
        class GRoot extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public static get contentScaleFactor(): number;
            public static get contentScaleLevel(): number;
            public static get inst(): FairyGUI.GRoot;
            public get modalLayer(): FairyGUI.GGraph;
            public get hasModalWindow(): boolean;
            public get modalWaiting(): boolean;
            public get touchTarget(): FairyGUI.GObject;
            public get hasAnyPopup(): boolean;
            public get focus(): FairyGUI.GObject;
            public set focus(value: FairyGUI.GObject);
            public get soundVolume(): number;
            public set soundVolume(value: number);
            public SetContentScaleFactor ($designResolutionX: number, $designResolutionY: number) : void
            public SetContentScaleFactor ($designResolutionX: number, $designResolutionY: number, $screenMatchMode: FairyGUI.UIContentScaler.ScreenMatchMode) : void
            public SetContentScaleFactor ($constantScaleFactor: number) : void
            public ApplyContentScaleFactor () : void
            public ShowWindow ($win: FairyGUI.Window) : void
            public HideWindow ($win: FairyGUI.Window) : void
            public HideWindowImmediately ($win: FairyGUI.Window) : void
            public HideWindowImmediately ($win: FairyGUI.Window, $dispose: boolean) : void
            public BringToFront ($win: FairyGUI.Window) : void
            public ShowModalWait () : void
            public CloseModalWait () : void
            public CloseAllExceptModals () : void
            public CloseAllWindows () : void
            public GetTopWindow () : FairyGUI.Window
            public DisplayObjectToGObject ($obj: FairyGUI.DisplayObject) : FairyGUI.GObject
            public ShowPopup ($popup: FairyGUI.GObject) : void
            public ShowPopup ($popup: FairyGUI.GObject, $target: FairyGUI.GObject) : void
            public ShowPopup ($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection) : void
            public ShowPopup ($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection, $closeUntilUpEvent: boolean) : void
            public GetPoupPosition ($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection) : UnityEngine.Vector2
            public TogglePopup ($popup: FairyGUI.GObject) : void
            public TogglePopup ($popup: FairyGUI.GObject, $target: FairyGUI.GObject) : void
            public TogglePopup ($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection) : void
            public TogglePopup ($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection, $closeUntilUpEvent: boolean) : void
            public HidePopup () : void
            public HidePopup ($popup: FairyGUI.GObject) : void
            public ShowTooltips ($msg: string) : void
            public ShowTooltips ($msg: string, $delay: number) : void
            public ShowTooltipsWin ($tooltipWin: FairyGUI.GObject) : void
            public ShowTooltipsWin ($tooltipWin: FairyGUI.GObject, $delay: number) : void
            public HideTooltips () : void
            public EnableSound () : void
            public DisableSound () : void
            public PlayOneShotSound ($clip: UnityEngine.AudioClip, $volumeScale: number) : void
            public PlayOneShotSound ($clip: UnityEngine.AudioClip) : void
            public constructor ()
        }
        interface EventCallback1
        { (context: FairyGUI.EventContext) : void; }
        var EventCallback1: { new (func: (context: FairyGUI.EventContext) => void): EventCallback1; }
        class EventContext extends System.Object
        {
            public type : string
            public data : any
            public get sender(): FairyGUI.EventDispatcher;
            public get initiator(): any;
            public get inputEvent(): FairyGUI.InputEvent;
            public get isDefaultPrevented(): boolean;
            public StopPropagation () : void
            public PreventDefault () : void
            public CaptureTouch () : void
            public constructor ()
        }
        class GLoader extends FairyGUI.GObject implements FairyGUI.IEventDispatcher, FairyGUI.IAnimationGear, FairyGUI.IColorGear
        {
            public showErrorSign : boolean
            public get url(): string;
            public set url(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get align(): FairyGUI.AlignType;
            public set align(value: FairyGUI.AlignType);
            public get verticalAlign(): FairyGUI.VertAlignType;
            public set verticalAlign(value: FairyGUI.VertAlignType);
            public get fill(): FairyGUI.FillType;
            public set fill(value: FairyGUI.FillType);
            public get shrinkOnly(): boolean;
            public set shrinkOnly(value: boolean);
            public get autoSize(): boolean;
            public set autoSize(value: boolean);
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public get timeScale(): number;
            public set timeScale(value: number);
            public get ignoreEngineTimeScale(): boolean;
            public set ignoreEngineTimeScale(value: boolean);
            public get material(): UnityEngine.Material;
            public set material(value: UnityEngine.Material);
            public get shader(): string;
            public set shader(value: string);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get fillMethod(): FairyGUI.FillMethod;
            public set fillMethod(value: FairyGUI.FillMethod);
            public get fillOrigin(): number;
            public set fillOrigin(value: number);
            public get fillClockwise(): boolean;
            public set fillClockwise(value: boolean);
            public get fillAmount(): number;
            public set fillAmount(value: number);
            public get image(): FairyGUI.Image;
            public get movieClip(): FairyGUI.MovieClip;
            public get component(): FairyGUI.GComponent;
            public get texture(): FairyGUI.NTexture;
            public set texture(value: FairyGUI.NTexture);
            public get filter(): FairyGUI.IFilter;
            public set filter(value: FairyGUI.IFilter);
            public get blendMode(): FairyGUI.BlendMode;
            public set blendMode(value: FairyGUI.BlendMode);
            public Advance ($time: number) : void
            public constructor ()
        }
        interface IAnimationGear
        {
            playing : boolean
            frame : number
            timeScale : number
            ignoreEngineTimeScale : boolean
            Advance ($time: number) : void
        }
        interface IColorGear
        {
            color : UnityEngine.Color
        }
        class Image extends FairyGUI.DisplayObject implements FairyGUI.IEventDispatcher, FairyGUI.IMeshFactory
        {
            public get texture(): FairyGUI.NTexture;
            public set texture(value: FairyGUI.NTexture);
            public get textureScale(): UnityEngine.Vector2;
            public set textureScale(value: UnityEngine.Vector2);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get fillMethod(): FairyGUI.FillMethod;
            public set fillMethod(value: FairyGUI.FillMethod);
            public get fillOrigin(): number;
            public set fillOrigin(value: number);
            public get fillClockwise(): boolean;
            public set fillClockwise(value: boolean);
            public get fillAmount(): number;
            public set fillAmount(value: number);
            public get scale9Grid(): System.Nullable$1<UnityEngine.Rect>;
            public set scale9Grid(value: System.Nullable$1<UnityEngine.Rect>);
            public get scaleByTile(): boolean;
            public set scaleByTile(value: boolean);
            public get tileGridIndice(): number;
            public set tileGridIndice(value: number);
            public SetNativeSize () : void
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public SliceFill ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
            public constructor ($texture: FairyGUI.NTexture)
        }
        class DisplayObject extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public name : string
            public gOwner : FairyGUI.GObject
            public id : number
            public get parent(): FairyGUI.Container;
            public get gameObject(): UnityEngine.GameObject;
            public get cachedTransform(): UnityEngine.Transform;
            public get graphics(): FairyGUI.NGraphics;
            public get paintingGraphics(): FairyGUI.NGraphics;
            public get onClick(): FairyGUI.EventListener;
            public get onRightClick(): FairyGUI.EventListener;
            public get onTouchBegin(): FairyGUI.EventListener;
            public get onTouchMove(): FairyGUI.EventListener;
            public get onTouchEnd(): FairyGUI.EventListener;
            public get onRollOver(): FairyGUI.EventListener;
            public get onRollOut(): FairyGUI.EventListener;
            public get onMouseWheel(): FairyGUI.EventListener;
            public get onAddedToStage(): FairyGUI.EventListener;
            public get onRemovedFromStage(): FairyGUI.EventListener;
            public get onKeyDown(): FairyGUI.EventListener;
            public get onClickLink(): FairyGUI.EventListener;
            public get onFocusIn(): FairyGUI.EventListener;
            public get onFocusOut(): FairyGUI.EventListener;
            public get alpha(): number;
            public set alpha(value: number);
            public get grayed(): boolean;
            public set grayed(value: boolean);
            public get visible(): boolean;
            public set visible(value: boolean);
            public get x(): number;
            public set x(value: number);
            public get y(): number;
            public set y(value: number);
            public get z(): number;
            public set z(value: number);
            public get xy(): UnityEngine.Vector2;
            public set xy(value: UnityEngine.Vector2);
            public get position(): UnityEngine.Vector3;
            public set position(value: UnityEngine.Vector3);
            public get pixelPerfect(): boolean;
            public set pixelPerfect(value: boolean);
            public get width(): number;
            public set width(value: number);
            public get height(): number;
            public set height(value: number);
            public get size(): UnityEngine.Vector2;
            public set size(value: UnityEngine.Vector2);
            public get scaleX(): number;
            public set scaleX(value: number);
            public get scaleY(): number;
            public set scaleY(value: number);
            public get scale(): UnityEngine.Vector2;
            public set scale(value: UnityEngine.Vector2);
            public get rotation(): number;
            public set rotation(value: number);
            public get rotationX(): number;
            public set rotationX(value: number);
            public get rotationY(): number;
            public set rotationY(value: number);
            public get skew(): UnityEngine.Vector2;
            public set skew(value: UnityEngine.Vector2);
            public get perspective(): boolean;
            public set perspective(value: boolean);
            public get focalLength(): number;
            public set focalLength(value: number);
            public get pivot(): UnityEngine.Vector2;
            public set pivot(value: UnityEngine.Vector2);
            public get location(): UnityEngine.Vector3;
            public set location(value: UnityEngine.Vector3);
            public get material(): UnityEngine.Material;
            public set material(value: UnityEngine.Material);
            public get shader(): string;
            public set shader(value: string);
            public get renderingOrder(): number;
            public set renderingOrder(value: number);
            public get layer(): number;
            public set layer(value: number);
            public get focusable(): boolean;
            public set focusable(value: boolean);
            public get tabStop(): boolean;
            public set tabStop(value: boolean);
            public get focused(): boolean;
            public get cursor(): string;
            public set cursor(value: string);
            public get isDisposed(): boolean;
            public get topmost(): FairyGUI.Container;
            public get stage(): FairyGUI.Stage;
            public get worldSpaceContainer(): FairyGUI.Container;
            public get touchable(): boolean;
            public set touchable(value: boolean);
            public get touchDisabled(): boolean;
            public get paintingMode(): boolean;
            public get cacheAsBitmap(): boolean;
            public set cacheAsBitmap(value: boolean);
            public get filter(): FairyGUI.IFilter;
            public set filter(value: FairyGUI.IFilter);
            public get blendMode(): FairyGUI.BlendMode;
            public set blendMode(value: FairyGUI.BlendMode);
            public get home(): UnityEngine.Transform;
            public set home(value: UnityEngine.Transform);
            public add_onPaint ($value: System.Action) : void
            public remove_onPaint ($value: System.Action) : void
            public SetXY ($xv: number, $yv: number) : void
            public SetPosition ($xv: number, $yv: number, $zv: number) : void
            public SetSize ($wv: number, $hv: number) : void
            public EnsureSizeCorrect () : void
            public SetScale ($xv: number, $yv: number) : void
            public EnterPaintingMode () : void
            public EnterPaintingMode ($requestorId: number, $extend: System.Nullable$1<FairyGUI.Margin>) : void
            public EnterPaintingMode ($requestorId: number, $extend: System.Nullable$1<FairyGUI.Margin>, $scale: number) : void
            public LeavePaintingMode ($requestorId: number) : void
            public GetScreenShot ($extend: System.Nullable$1<FairyGUI.Margin>, $scale: number) : UnityEngine.Texture2D
            public GetBounds ($targetSpace: FairyGUI.DisplayObject) : UnityEngine.Rect
            public GlobalToLocal ($point: UnityEngine.Vector2) : UnityEngine.Vector2
            public LocalToGlobal ($point: UnityEngine.Vector2) : UnityEngine.Vector2
            public WorldToLocal ($worldPoint: UnityEngine.Vector3, $direction: UnityEngine.Vector3) : UnityEngine.Vector3
            public LocalToWorld ($localPoint: UnityEngine.Vector3) : UnityEngine.Vector3
            public TransformPoint ($point: UnityEngine.Vector2, $targetSpace: FairyGUI.DisplayObject) : UnityEngine.Vector2
            public TransformRect ($rect: UnityEngine.Rect, $targetSpace: FairyGUI.DisplayObject) : UnityEngine.Rect
            public RemoveFromParent () : void
            public InvalidateBatchingState () : void
            public Update ($context: FairyGUI.UpdateContext) : void
            public Dispose () : void
            public constructor ()
        }
        interface IMeshFactory
        {
            OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
        }
        class EventListener extends System.Object
        {
            public get type(): string;
            public get isEmpty(): boolean;
            public get isDispatching(): boolean;
            public AddCapture ($callback: FairyGUI.EventCallback1) : void
            public RemoveCapture ($callback: FairyGUI.EventCallback1) : void
            public Add ($callback: FairyGUI.EventCallback1) : void
            public Remove ($callback: FairyGUI.EventCallback1) : void
            public Add ($callback: FairyGUI.EventCallback0) : void
            public Remove ($callback: FairyGUI.EventCallback0) : void
            public Set ($callback: FairyGUI.EventCallback1) : void
            public Set ($callback: FairyGUI.EventCallback0) : void
            public Clear () : void
            public Call () : boolean
            public Call ($data: any) : boolean
            public BubbleCall ($data: any) : boolean
            public BubbleCall () : boolean
            public BroadcastCall ($data: any) : boolean
            public BroadcastCall () : boolean
            public constructor ($owner: FairyGUI.EventDispatcher, $type: string)
            public constructor ()
        }
        class NTexture extends System.Object
        {
            public uvRect : UnityEngine.Rect
            public rotated : boolean
            public refCount : number
            public lastActive : number
            public destroyMethod : FairyGUI.DestroyMethod
            public static get Empty(): FairyGUI.NTexture;
            public get width(): number;
            public get height(): number;
            public get offset(): UnityEngine.Vector2;
            public set offset(value: UnityEngine.Vector2);
            public get originalSize(): UnityEngine.Vector2;
            public set originalSize(value: UnityEngine.Vector2);
            public get root(): FairyGUI.NTexture;
            public get disposed(): boolean;
            public get nativeTexture(): UnityEngine.Texture;
            public get alphaTexture(): UnityEngine.Texture;
            public static add_CustomDestroyMethod ($value: System.Action$1<UnityEngine.Texture>) : void
            public static remove_CustomDestroyMethod ($value: System.Action$1<UnityEngine.Texture>) : void
            public add_onSizeChanged ($value: System.Action$1<FairyGUI.NTexture>) : void
            public remove_onSizeChanged ($value: System.Action$1<FairyGUI.NTexture>) : void
            public add_onRelease ($value: System.Action$1<FairyGUI.NTexture>) : void
            public remove_onRelease ($value: System.Action$1<FairyGUI.NTexture>) : void
            public static DisposeEmpty () : void
            public GetDrawRect ($drawRect: UnityEngine.Rect) : UnityEngine.Rect
            public GetUV ($uv: System.Array$1<UnityEngine.Vector2>) : void
            public GetMaterialManager ($shaderName: string) : FairyGUI.MaterialManager
            public Unload () : void
            public Unload ($destroyMaterials: boolean) : void
            public Reload ($nativeTexture: UnityEngine.Texture, $alphaTexture: UnityEngine.Texture) : void
            public AddRef () : void
            public ReleaseRef () : void
            public Dispose () : void
            public constructor ($texture: UnityEngine.Texture)
            public constructor ($texture: UnityEngine.Texture, $alphaTexture: UnityEngine.Texture, $xScale: number, $yScale: number)
            public constructor ($texture: UnityEngine.Texture, $region: UnityEngine.Rect)
            public constructor ($root: FairyGUI.NTexture, $region: UnityEngine.Rect, $rotated: boolean)
            public constructor ($root: FairyGUI.NTexture, $region: UnityEngine.Rect, $rotated: boolean, $originalSize: UnityEngine.Vector2, $offset: UnityEngine.Vector2)
            public constructor ($sprite: UnityEngine.Sprite)
            public constructor ()
        }
        class BitmapFont extends FairyGUI.BaseFont
        {
            public size : number
            public resizable : boolean
            public hasChannel : boolean
            public AddChar ($ch: number, $glyph: FairyGUI.BitmapFont.BMGlyph) : void
            public constructor ()
        }
        class BaseFont extends System.Object
        {
            public name : string
            public mainTexture : FairyGUI.NTexture
            public canTint : boolean
            public customBold : boolean
            public customBoldAndItalic : boolean
            public customOutline : boolean
            public shader : string
            public keepCrisp : boolean
            public version : number
            public UpdateGraphics ($graphics: FairyGUI.NGraphics) : void
            public SetFormat ($format: FairyGUI.TextFormat, $fontSizeScale: number) : void
            public PrepareCharacters ($text: string) : void
            public GetGlyph ($ch: number, $width: $Ref<number>, $height: $Ref<number>, $baseline: $Ref<number>) : boolean
            public DrawGlyph ($x: number, $y: number, $vertList: System.Collections.Generic.List$1<UnityEngine.Vector3>, $uvList: System.Collections.Generic.List$1<UnityEngine.Vector2>, $uv2List: System.Collections.Generic.List$1<UnityEngine.Vector2>, $colList: System.Collections.Generic.List$1<UnityEngine.Color32>) : number
            public DrawLine ($x: number, $y: number, $width: number, $fontSize: number, $type: number, $vertList: System.Collections.Generic.List$1<UnityEngine.Vector3>, $uvList: System.Collections.Generic.List$1<UnityEngine.Vector2>, $uv2List: System.Collections.Generic.List$1<UnityEngine.Vector2>, $colList: System.Collections.Generic.List$1<UnityEngine.Color32>) : number
            public HasCharacter ($ch: number) : boolean
            public GetLineHeight ($size: number) : number
            public Dispose () : void
            public constructor ()
        }
        enum AlignType
        { Left = 0, Center = 1, Right = 2 }
        enum VertAlignType
        { Top = 0, Middle = 1, Bottom = 2 }
        enum AutoSizeType
        { None = 0, Both = 1, Height = 2, Shrink = 3, Ellipsis = 4 }
        enum FlipType
        { None = 0, Horizontal = 1, Vertical = 2, Both = 3 }
        enum FillMethod
        { None = 0, Horizontal = 1, Vertical = 2, Radial90 = 3, Radial180 = 4, Radial360 = 5 }
        enum EaseType
        { Linear = 0, SineIn = 1, SineOut = 2, SineInOut = 3, QuadIn = 4, QuadOut = 5, QuadInOut = 6, CubicIn = 7, CubicOut = 8, CubicInOut = 9, QuartIn = 10, QuartOut = 11, QuartInOut = 12, QuintIn = 13, QuintOut = 14, QuintInOut = 15, ExpoIn = 16, ExpoOut = 17, ExpoInOut = 18, CircIn = 19, CircOut = 20, CircInOut = 21, ElasticIn = 22, ElasticOut = 23, ElasticInOut = 24, BackIn = 25, BackOut = 26, BackInOut = 27, BounceIn = 28, BounceOut = 29, BounceInOut = 30, Custom = 31 }
        class CustomEase extends System.Object
        {
            public Create ($pathPoints: System.Collections.Generic.IEnumerable$1<FairyGUI.GPathPoint>) : void
            public Evaluate ($time: number) : number
            public constructor ($pointDensity?: number)
            public constructor ()
        }
        class GPathPoint extends System.ValueType
        {
            public pos : UnityEngine.Vector3
            public control1 : UnityEngine.Vector3
            public control2 : UnityEngine.Vector3
            public curveType : FairyGUI.GPathPoint.CurveType
            public smooth : boolean
            public constructor ($pos: UnityEngine.Vector3)
            public constructor ($pos: UnityEngine.Vector3, $control: UnityEngine.Vector3)
            public constructor ($pos: UnityEngine.Vector3, $control1: UnityEngine.Vector3, $control2: UnityEngine.Vector3)
            public constructor ($pos: UnityEngine.Vector3, $curveType: FairyGUI.GPathPoint.CurveType)
            public constructor ()
        }
        class Container extends FairyGUI.DisplayObject implements FairyGUI.IEventDispatcher
        {
            public renderMode : UnityEngine.RenderMode
            public renderCamera : UnityEngine.Camera
            public opaque : boolean
            public clipSoftness : System.Nullable$1<UnityEngine.Vector4>
            public hitArea : FairyGUI.IHitTest
            public touchChildren : boolean
            public reversedMask : boolean
            public get numChildren(): number;
            public get clipRect(): System.Nullable$1<UnityEngine.Rect>;
            public set clipRect(value: System.Nullable$1<UnityEngine.Rect>);
            public get mask(): FairyGUI.DisplayObject;
            public set mask(value: FairyGUI.DisplayObject);
            public get fairyBatching(): boolean;
            public set fairyBatching(value: boolean);
            public get tabStopChildren(): boolean;
            public set tabStopChildren(value: boolean);
            public add_onUpdate ($value: System.Action) : void
            public remove_onUpdate ($value: System.Action) : void
            public AddChild ($child: FairyGUI.DisplayObject) : FairyGUI.DisplayObject
            public AddChildAt ($child: FairyGUI.DisplayObject, $index: number) : FairyGUI.DisplayObject
            public Contains ($child: FairyGUI.DisplayObject) : boolean
            public GetChildAt ($index: number) : FairyGUI.DisplayObject
            public GetChild ($name: string) : FairyGUI.DisplayObject
            public GetChildren () : System.Array$1<FairyGUI.DisplayObject>
            public GetChildIndex ($child: FairyGUI.DisplayObject) : number
            public RemoveChild ($child: FairyGUI.DisplayObject) : FairyGUI.DisplayObject
            public RemoveChild ($child: FairyGUI.DisplayObject, $dispose: boolean) : FairyGUI.DisplayObject
            public RemoveChildAt ($index: number) : FairyGUI.DisplayObject
            public RemoveChildAt ($index: number, $dispose: boolean) : FairyGUI.DisplayObject
            public RemoveChildren () : void
            public RemoveChildren ($beginIndex: number, $endIndex: number, $dispose: boolean) : void
            public SetChildIndex ($child: FairyGUI.DisplayObject, $index: number) : void
            public SwapChildren ($child1: FairyGUI.DisplayObject, $child2: FairyGUI.DisplayObject) : void
            public SwapChildrenAt ($index1: number, $index2: number) : void
            public ChangeChildrenOrder ($indice: System.Collections.Generic.IList$1<number>, $objs: System.Collections.Generic.IList$1<FairyGUI.DisplayObject>) : void
            public GetDescendants ($backward: boolean) : System.Collections.Generic.IEnumerator$1<FairyGUI.DisplayObject>
            public CreateGraphics () : void
            public GetRenderCamera () : UnityEngine.Camera
            public HitTest ($stagePoint: UnityEngine.Vector2, $forTouch: boolean) : FairyGUI.DisplayObject
            public IsAncestorOf ($obj: FairyGUI.DisplayObject) : boolean
            public InvalidateBatchingState ($childrenChanged: boolean) : void
            public SetChildrenLayer ($value: number) : void
            public constructor ()
            public constructor ($gameObjectName: string)
            public constructor ($attachTarget: UnityEngine.GameObject)
            public InvalidateBatchingState () : void
        }
        interface IHitTest
        {
            HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
        }
        class TextFormat extends System.Object
        {
            public size : number
            public font : string
            public color : UnityEngine.Color
            public lineSpacing : number
            public letterSpacing : number
            public bold : boolean
            public underline : boolean
            public italic : boolean
            public strikethrough : boolean
            public gradientColor : System.Array$1<UnityEngine.Color32>
            public align : FairyGUI.AlignType
            public specialStyle : FairyGUI.TextFormat.SpecialStyle
            public outline : number
            public outlineColor : UnityEngine.Color
            public shadowOffset : UnityEngine.Vector2
            public shadowColor : UnityEngine.Color
            public faceDilate : number
            public outlineSoftness : number
            public underlaySoftness : number
            public SetColor ($value: number) : void
            public EqualStyle ($aFormat: FairyGUI.TextFormat) : boolean
            public CopyFrom ($source: FairyGUI.TextFormat) : void
            public FillVertexColors ($vertexColors: System.Array$1<UnityEngine.Color32>) : void
            public constructor ()
        }
        class GTweener extends System.Object
        {
            public get delay(): number;
            public get duration(): number;
            public get repeat(): number;
            public get target(): any;
            public get userData(): any;
            public get startValue(): FairyGUI.TweenValue;
            public get endValue(): FairyGUI.TweenValue;
            public get value(): FairyGUI.TweenValue;
            public get deltaValue(): FairyGUI.TweenValue;
            public get normalizedTime(): number;
            public get completed(): boolean;
            public get allCompleted(): boolean;
            public SetDelay ($value: number) : FairyGUI.GTweener
            public SetDuration ($value: number) : FairyGUI.GTweener
            public SetBreakpoint ($value: number) : FairyGUI.GTweener
            public SetEase ($value: FairyGUI.EaseType) : FairyGUI.GTweener
            public SetEase ($value: FairyGUI.EaseType, $customEase: FairyGUI.CustomEase) : FairyGUI.GTweener
            public SetEasePeriod ($value: number) : FairyGUI.GTweener
            public SetEaseOvershootOrAmplitude ($value: number) : FairyGUI.GTweener
            public SetRepeat ($times: number, $yoyo?: boolean) : FairyGUI.GTweener
            public SetTimeScale ($value: number) : FairyGUI.GTweener
            public SetIgnoreEngineTimeScale ($value: boolean) : FairyGUI.GTweener
            public SetSnapping ($value: boolean) : FairyGUI.GTweener
            public SetPath ($value: FairyGUI.GPath) : FairyGUI.GTweener
            public SetTarget ($value: any) : FairyGUI.GTweener
            public SetTarget ($value: any, $propType: FairyGUI.TweenPropType) : FairyGUI.GTweener
            public SetUserData ($value: any) : FairyGUI.GTweener
            public OnUpdate ($callback: FairyGUI.GTweenCallback) : FairyGUI.GTweener
            public OnStart ($callback: FairyGUI.GTweenCallback) : FairyGUI.GTweener
            public OnComplete ($callback: FairyGUI.GTweenCallback) : FairyGUI.GTweener
            public OnUpdate ($callback: FairyGUI.GTweenCallback1) : FairyGUI.GTweener
            public OnStart ($callback: FairyGUI.GTweenCallback1) : FairyGUI.GTweener
            public OnComplete ($callback: FairyGUI.GTweenCallback1) : FairyGUI.GTweener
            public SetListener ($value: FairyGUI.ITweenListener) : FairyGUI.GTweener
            public SetPaused ($paused: boolean) : FairyGUI.GTweener
            public Seek ($time: number) : void
            public Kill ($complete?: boolean) : void
            public constructor ()
        }
        class GPath extends System.Object
        {
            public get length(): number;
            public get segmentCount(): number;
            public Create ($pt1: FairyGUI.GPathPoint, $pt2: FairyGUI.GPathPoint) : void
            public Create ($pt1: FairyGUI.GPathPoint, $pt2: FairyGUI.GPathPoint, $pt3: FairyGUI.GPathPoint) : void
            public Create ($pt1: FairyGUI.GPathPoint, $pt2: FairyGUI.GPathPoint, $pt3: FairyGUI.GPathPoint, $pt4: FairyGUI.GPathPoint) : void
            public Create ($points: System.Collections.Generic.IEnumerable$1<FairyGUI.GPathPoint>) : void
            public Clear () : void
            public GetPointAt ($t: number) : UnityEngine.Vector3
            public GetSegmentLength ($segmentIndex: number) : number
            public GetPointsInSegment ($segmentIndex: number, $t0: number, $t1: number, $points: System.Collections.Generic.List$1<UnityEngine.Vector3>, $ts?: System.Collections.Generic.List$1<number>, $pointDensity?: number) : void
            public GetAllPoints ($points: System.Collections.Generic.List$1<UnityEngine.Vector3>, $pointDensity?: number) : void
            public constructor ()
        }
        class RichTextField extends FairyGUI.Container implements FairyGUI.IEventDispatcher
        {
            public get htmlPageContext(): FairyGUI.Utils.IHtmlPageContext;
            public set htmlPageContext(value: FairyGUI.Utils.IHtmlPageContext);
            public get htmlParseOptions(): FairyGUI.Utils.HtmlParseOptions;
            public get emojies(): System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>;
            public set emojies(value: System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>);
            public get textField(): FairyGUI.TextField;
            public get text(): string;
            public set text(value: string);
            public get htmlText(): string;
            public set htmlText(value: string);
            public get textFormat(): FairyGUI.TextFormat;
            public set textFormat(value: FairyGUI.TextFormat);
            public get htmlElementCount(): number;
            public GetHtmlElement ($name: string) : FairyGUI.Utils.HtmlElement
            public GetHtmlElementAt ($index: number) : FairyGUI.Utils.HtmlElement
            public ShowHtmlObject ($index: number, $show: boolean) : void
            public constructor ()
        }
        class InputEvent extends System.Object
        {
            public get x(): number;
            public get y(): number;
            public get keyCode(): UnityEngine.KeyCode;
            public get character(): number;
            public get modifiers(): UnityEngine.EventModifiers;
            public get mouseWheelDelta(): number;
            public get touchId(): number;
            public get button(): number;
            public get clickCount(): number;
            public get holdTime(): number;
            public get position(): UnityEngine.Vector2;
            public get isDoubleClick(): boolean;
            public get ctrlOrCmd(): boolean;
            public get ctrl(): boolean;
            public get shift(): boolean;
            public get alt(): boolean;
            public get command(): boolean;
            public constructor ()
        }
        class GComboBox extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public visibleItemCount : number
            public dropdown : FairyGUI.GComponent
            public sound : FairyGUI.NAudioClip
            public soundVolumeScale : number
            public get onChanged(): FairyGUI.EventListener;
            public get icon(): string;
            public set icon(value: string);
            public get title(): string;
            public set title(value: string);
            public get text(): string;
            public set text(value: string);
            public get titleColor(): UnityEngine.Color;
            public set titleColor(value: UnityEngine.Color);
            public get titleFontSize(): number;
            public set titleFontSize(value: number);
            public get items(): System.Array$1<string>;
            public set items(value: System.Array$1<string>);
            public get icons(): System.Array$1<string>;
            public set icons(value: System.Array$1<string>);
            public get values(): System.Array$1<string>;
            public set values(value: System.Array$1<string>);
            public get itemList(): System.Collections.Generic.List$1<string>;
            public get valueList(): System.Collections.Generic.List$1<string>;
            public get iconList(): System.Collections.Generic.List$1<string>;
            public get selectedIndex(): number;
            public set selectedIndex(value: number);
            public get selectionController(): FairyGUI.Controller;
            public set selectionController(value: FairyGUI.Controller);
            public get value(): string;
            public set value(value: string);
            public get popupDirection(): FairyGUI.PopupDirection;
            public set popupDirection(value: FairyGUI.PopupDirection);
            public ApplyListChange () : void
            public GetTextField () : FairyGUI.GTextField
            public UpdateDropdownList () : void
            public constructor ()
        }
        class Shape extends FairyGUI.DisplayObject implements FairyGUI.IEventDispatcher
        {
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get isEmpty(): boolean;
            public DrawRect ($lineSize: number, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color) : void
            public DrawRect ($lineSize: number, $colors: System.Array$1<UnityEngine.Color32>) : void
            public DrawRoundRect ($lineSize: number, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color, $topLeftRadius: number, $topRightRadius: number, $bottomLeftRadius: number, $bottomRightRadius: number) : void
            public DrawEllipse ($fillColor: UnityEngine.Color) : void
            public DrawEllipse ($lineSize: number, $centerColor: UnityEngine.Color, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color, $startDegree: number, $endDegree: number) : void
            public DrawPolygon ($points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color) : void
            public DrawPolygon ($points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $colors: System.Array$1<UnityEngine.Color32>) : void
            public DrawPolygon ($points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color, $lineSize: number, $lineColor: UnityEngine.Color) : void
            public DrawRegularPolygon ($sides: number, $lineSize: number, $centerColor: UnityEngine.Color, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color, $rotation: number, $distances: System.Array$1<number>) : void
            public Clear () : void
            public constructor ()
        }
        class VertexBuffer extends System.Object
        {
            public contentRect : UnityEngine.Rect
            public uvRect : UnityEngine.Rect
            public vertexColor : UnityEngine.Color32
            public textureSize : UnityEngine.Vector2
            public vertices : System.Collections.Generic.List$1<UnityEngine.Vector3>
            public colors : System.Collections.Generic.List$1<UnityEngine.Color32>
            public uvs : System.Collections.Generic.List$1<UnityEngine.Vector2>
            public uvs2 : System.Collections.Generic.List$1<UnityEngine.Vector2>
            public triangles : System.Collections.Generic.List$1<number>
            public static NormalizedUV : System.Array$1<UnityEngine.Vector2>
            public static NormalizedPosition : System.Array$1<UnityEngine.Vector2>
            public get currentVertCount(): number;
            public static Begin () : FairyGUI.VertexBuffer
            public static Begin ($source: FairyGUI.VertexBuffer) : FairyGUI.VertexBuffer
            public End () : void
            public Clear () : void
            public AddVert ($position: UnityEngine.Vector3) : void
            public AddVert ($position: UnityEngine.Vector3, $color: UnityEngine.Color32) : void
            public AddVert ($position: UnityEngine.Vector3, $color: UnityEngine.Color32, $uv: UnityEngine.Vector2) : void
            public AddQuad ($vertRect: UnityEngine.Rect) : void
            public AddQuad ($vertRect: UnityEngine.Rect, $color: UnityEngine.Color32) : void
            public AddQuad ($vertRect: UnityEngine.Rect, $color: UnityEngine.Color32, $uvRect: UnityEngine.Rect) : void
            public RepeatColors ($value: System.Array$1<UnityEngine.Color32>, $startIndex: number, $count: number) : void
            public AddTriangle ($idx0: number, $idx1: number, $idx2: number) : void
            public AddTriangles ($idxList: System.Array$1<number>, $startVertexIndex?: number) : void
            public AddTriangles ($startVertexIndex?: number) : void
            public GetPosition ($index: number) : UnityEngine.Vector3
            public GetUVAtPosition ($position: UnityEngine.Vector2, $usePercent: boolean) : UnityEngine.Vector2
            public Append ($vb: FairyGUI.VertexBuffer) : void
            public Insert ($vb: FairyGUI.VertexBuffer) : void
        }
        class LineMesh extends System.Object implements FairyGUI.IMeshFactory
        {
            public path : FairyGUI.GPath
            public lineWidth : number
            public lineWidthCurve : UnityEngine.AnimationCurve
            public gradient : UnityEngine.Gradient
            public roundEdge : boolean
            public fillStart : number
            public fillEnd : number
            public pointDensity : number
            public repeatFill : boolean
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
        }
        class StraightLineMesh extends System.Object implements FairyGUI.IMeshFactory
        {
            public color : UnityEngine.Color
            public origin : UnityEngine.Vector3
            public end : UnityEngine.Vector3
            public lineWidth : number
            public repeatFill : boolean
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
            public constructor ($lineWidth: number, $color: UnityEngine.Color, $repeatFill: boolean)
        }
        enum PopupDirection
        { Auto = 0, Up = 1, Down = 2 }
        class GTree extends FairyGUI.GList implements FairyGUI.IEventDispatcher
        {
            public treeNodeRender : FairyGUI.GTree.TreeNodeRenderDelegate
            public treeNodeWillExpand : FairyGUI.GTree.TreeNodeWillExpandDelegate
            public get rootNode(): FairyGUI.GTreeNode;
            public get indent(): number;
            public set indent(value: number);
            public get clickToExpand(): number;
            public set clickToExpand(value: number);
            public GetSelectedNode () : FairyGUI.GTreeNode
            public GetSelectedNodes () : System.Collections.Generic.List$1<FairyGUI.GTreeNode>
            public GetSelectedNodes ($result: System.Collections.Generic.List$1<FairyGUI.GTreeNode>) : System.Collections.Generic.List$1<FairyGUI.GTreeNode>
            public SelectNode ($node: FairyGUI.GTreeNode) : void
            public SelectNode ($node: FairyGUI.GTreeNode, $scrollItToView: boolean) : void
            public UnselectNode ($node: FairyGUI.GTreeNode) : void
            public ExpandAll () : void
            public ExpandAll ($folderNode: FairyGUI.GTreeNode) : void
            public CollapseAll () : void
            public CollapseAll ($folderNode: FairyGUI.GTreeNode) : void
            public constructor ()
        }
        class GList extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public foldInvisibleItems : boolean
            public selectionMode : FairyGUI.ListSelectionMode
            public itemRenderer : FairyGUI.ListItemRenderer
            public itemProvider : FairyGUI.ListItemProvider
            public scrollItemToViewOnClick : boolean
            public get onClickItem(): FairyGUI.EventListener;
            public get onRightClickItem(): FairyGUI.EventListener;
            public get defaultItem(): string;
            public set defaultItem(value: string);
            public get layout(): FairyGUI.ListLayoutType;
            public set layout(value: FairyGUI.ListLayoutType);
            public get lineCount(): number;
            public set lineCount(value: number);
            public get columnCount(): number;
            public set columnCount(value: number);
            public get lineGap(): number;
            public set lineGap(value: number);
            public get columnGap(): number;
            public set columnGap(value: number);
            public get align(): FairyGUI.AlignType;
            public set align(value: FairyGUI.AlignType);
            public get verticalAlign(): FairyGUI.VertAlignType;
            public set verticalAlign(value: FairyGUI.VertAlignType);
            public get autoResizeItem(): boolean;
            public set autoResizeItem(value: boolean);
            public get defaultItemSize(): UnityEngine.Vector2;
            public set defaultItemSize(value: UnityEngine.Vector2);
            public get itemPool(): FairyGUI.GObjectPool;
            public get selectedIndex(): number;
            public set selectedIndex(value: number);
            public get selectionController(): FairyGUI.Controller;
            public set selectionController(value: FairyGUI.Controller);
            public get touchItem(): FairyGUI.GObject;
            public get isVirtual(): boolean;
            public get numItems(): number;
            public set numItems(value: number);
            public GetFromPool ($url: string) : FairyGUI.GObject
            public AddItemFromPool () : FairyGUI.GObject
            public AddItemFromPool ($url: string) : FairyGUI.GObject
            public RemoveChildToPoolAt ($index: number) : void
            public RemoveChildToPool ($child: FairyGUI.GObject) : void
            public RemoveChildrenToPool () : void
            public RemoveChildrenToPool ($beginIndex: number, $endIndex: number) : void
            public GetSelection () : System.Collections.Generic.List$1<number>
            public GetSelection ($result: System.Collections.Generic.List$1<number>) : System.Collections.Generic.List$1<number>
            public AddSelection ($index: number, $scrollItToView: boolean) : void
            public RemoveSelection ($index: number) : void
            public ClearSelection () : void
            public SelectAll () : void
            public SelectNone () : void
            public SelectReverse () : void
            public EnableSelectionFocusEvents ($enabled: boolean) : void
            public EnableArrowKeyNavigation ($enabled: boolean) : void
            public HandleArrowKey ($dir: number) : number
            public ResizeToFit () : void
            public ResizeToFit ($itemCount: number) : void
            public ResizeToFit ($itemCount: number, $minSize: number) : void
            public ScrollToView ($index: number) : void
            public ScrollToView ($index: number, $ani: boolean) : void
            public ScrollToView ($index: number, $ani: boolean, $setFirst: boolean) : void
            public ChildIndexToItemIndex ($index: number) : number
            public ItemIndexToChildIndex ($index: number) : number
            public SetVirtual () : void
            public SetVirtualAndLoop () : void
            public RefreshVirtualList () : void
            public constructor ()
        }
        class GTreeNode extends System.Object
        {
            public data : any
            public get parent(): FairyGUI.GTreeNode;
            public get tree(): FairyGUI.GTree;
            public get cell(): FairyGUI.GComponent;
            public get level(): number;
            public get expanded(): boolean;
            public set expanded(value: boolean);
            public get isFolder(): boolean;
            public get text(): string;
            public set text(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get numChildren(): number;
            public ExpandToRoot () : void
            public AddChild ($child: FairyGUI.GTreeNode) : FairyGUI.GTreeNode
            public AddChildAt ($child: FairyGUI.GTreeNode, $index: number) : FairyGUI.GTreeNode
            public RemoveChild ($child: FairyGUI.GTreeNode) : FairyGUI.GTreeNode
            public RemoveChildAt ($index: number) : FairyGUI.GTreeNode
            public RemoveChildren ($beginIndex?: number, $endIndex?: number) : void
            public GetChildAt ($index: number) : FairyGUI.GTreeNode
            public GetChildIndex ($child: FairyGUI.GTreeNode) : number
            public GetPrevSibling () : FairyGUI.GTreeNode
            public GetNextSibling () : FairyGUI.GTreeNode
            public SetChildIndex ($child: FairyGUI.GTreeNode, $index: number) : void
            public SwapChildren ($child1: FairyGUI.GTreeNode, $child2: FairyGUI.GTreeNode) : void
            public SwapChildrenAt ($index1: number, $index2: number) : void
            public constructor ($hasChild: boolean)
            public constructor ($hasChild: boolean, $resURL: string)
            public constructor ()
        }
        class GLabel extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public get icon(): string;
            public set icon(value: string);
            public get title(): string;
            public set title(value: string);
            public get text(): string;
            public set text(value: string);
            public get editable(): boolean;
            public set editable(value: boolean);
            public get titleColor(): UnityEngine.Color;
            public set titleColor(value: UnityEngine.Color);
            public get titleFontSize(): number;
            public set titleFontSize(value: number);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public GetTextField () : FairyGUI.GTextField
            public constructor ()
        }
        class GButton extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public sound : FairyGUI.NAudioClip
            public soundVolumeScale : number
            public changeStateOnClick : boolean
            public linkedPopup : FairyGUI.GObject
            public static UP : string
            public static DOWN : string
            public static OVER : string
            public static SELECTED_OVER : string
            public static DISABLED : string
            public static SELECTED_DISABLED : string
            public get onChanged(): FairyGUI.EventListener;
            public get icon(): string;
            public set icon(value: string);
            public get title(): string;
            public set title(value: string);
            public get text(): string;
            public set text(value: string);
            public get selectedIcon(): string;
            public set selectedIcon(value: string);
            public get selectedTitle(): string;
            public set selectedTitle(value: string);
            public get titleColor(): UnityEngine.Color;
            public set titleColor(value: UnityEngine.Color);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get titleFontSize(): number;
            public set titleFontSize(value: number);
            public get selected(): boolean;
            public set selected(value: boolean);
            public get mode(): FairyGUI.ButtonMode;
            public set mode(value: FairyGUI.ButtonMode);
            public get relatedController(): FairyGUI.Controller;
            public set relatedController(value: FairyGUI.Controller);
            public get relatedPageId(): string;
            public set relatedPageId(value: string);
            public FireClick ($downEffect: boolean, $clickCall?: boolean) : void
            public GetTextField () : FairyGUI.GTextField
            public constructor ()
        }
        class GTextField extends FairyGUI.GObject implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear, FairyGUI.ITextColorGear
        {
            public get text(): string;
            public set text(value: string);
            public get templateVars(): System.Collections.Generic.Dictionary$2<string, string>;
            public set templateVars(value: System.Collections.Generic.Dictionary$2<string, string>);
            public get textFormat(): FairyGUI.TextFormat;
            public set textFormat(value: FairyGUI.TextFormat);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get align(): FairyGUI.AlignType;
            public set align(value: FairyGUI.AlignType);
            public get verticalAlign(): FairyGUI.VertAlignType;
            public set verticalAlign(value: FairyGUI.VertAlignType);
            public get singleLine(): boolean;
            public set singleLine(value: boolean);
            public get stroke(): number;
            public set stroke(value: number);
            public get strokeColor(): UnityEngine.Color;
            public set strokeColor(value: UnityEngine.Color);
            public get shadowOffset(): UnityEngine.Vector2;
            public set shadowOffset(value: UnityEngine.Vector2);
            public get UBBEnabled(): boolean;
            public set UBBEnabled(value: boolean);
            public get autoSize(): FairyGUI.AutoSizeType;
            public set autoSize(value: FairyGUI.AutoSizeType);
            public get textWidth(): number;
            public get textHeight(): number;
            public SetVar ($name: string, $value: string) : FairyGUI.GTextField
            public FlushVars () : void
            public HasCharacter ($ch: number) : boolean
            public constructor ()
        }
        interface ITextColorGear extends FairyGUI.IColorGear
        {
            strokeColor : UnityEngine.Color
            color : UnityEngine.Color
        }
        enum GroupLayoutType
        { None = 0, Horizontal = 1, Vertical = 2 }
        class BlendModeUtils extends System.Object
        {
            public static Factors : System.Array$1<FairyGUI.BlendModeUtils.BlendFactor>
            public static Apply ($mat: UnityEngine.Material, $blendMode: FairyGUI.BlendMode) : void
            public static Override ($blendMode: FairyGUI.BlendMode, $srcFactor: UnityEngine.Rendering.BlendMode, $dstFactor: UnityEngine.Rendering.BlendMode) : void
            public constructor ()
        }
        enum BlendMode
        { Normal = 0, None = 1, Add = 2, Multiply = 3, Screen = 4, Erase = 5, Mask = 6, Below = 7, Off = 8, One_OneMinusSrcAlpha = 9, Custom1 = 10, Custom2 = 11, Custom3 = 12 }
        class CaptureCamera extends UnityEngine.MonoBehaviour
        {
            public cachedTransform : UnityEngine.Transform
            public cachedCamera : UnityEngine.Camera
            public static Name : string
            public static LayerName : string
            public static HiddenLayerName : string
            public static get layer(): number;
            public static get hiddenLayer(): number;
            public static CheckMain () : void
            public static CreateRenderTexture ($width: number, $height: number, $stencilSupport: boolean) : UnityEngine.RenderTexture
            public static Capture ($target: FairyGUI.DisplayObject, $texture: UnityEngine.RenderTexture, $contentHeight: number, $offset: UnityEngine.Vector2) : void
            public constructor ()
        }
        class UpdateContext extends System.Object
        {
            public clipped : boolean
            public clipInfo : FairyGUI.UpdateContext.ClipInfo
            public renderingOrder : number
            public batchingDepth : number
            public rectMaskDepth : number
            public stencilReferenceValue : number
            public stencilCompareValue : number
            public alpha : number
            public grayed : boolean
            public static current : FairyGUI.UpdateContext
            public static working : boolean
            public static add_OnBegin ($value: System.Action) : void
            public static remove_OnBegin ($value: System.Action) : void
            public static add_OnEnd ($value: System.Action) : void
            public static remove_OnEnd ($value: System.Action) : void
            public Begin () : void
            public End () : void
            public EnterClipping ($clipId: number, $clipRect: UnityEngine.Rect, $softness: System.Nullable$1<UnityEngine.Vector4>) : void
            public EnterClipping ($clipId: number, $reversedMask: boolean) : void
            public LeaveClipping () : void
            public EnterPaintingMode () : void
            public LeavePaintingMode () : void
            public ApplyClippingProperties ($mat: UnityEngine.Material, $isStdMaterial: boolean) : void
            public ApplyAlphaMaskProperties ($mat: UnityEngine.Material, $erasing: boolean) : void
            public constructor ()
        }
        class NGraphics extends System.Object implements FairyGUI.IMeshFactory
        {
            public blendMode : FairyGUI.BlendMode
            public dontClip : boolean
            public get gameObject(): UnityEngine.GameObject;
            public get meshFilter(): UnityEngine.MeshFilter;
            public get meshRenderer(): UnityEngine.MeshRenderer;
            public get mesh(): UnityEngine.Mesh;
            public get meshFactory(): FairyGUI.IMeshFactory;
            public set meshFactory(value: FairyGUI.IMeshFactory);
            public get contentRect(): UnityEngine.Rect;
            public set contentRect(value: UnityEngine.Rect);
            public get flip(): FairyGUI.FlipType;
            public set flip(value: FairyGUI.FlipType);
            public get texture(): FairyGUI.NTexture;
            public set texture(value: FairyGUI.NTexture);
            public get shader(): string;
            public set shader(value: string);
            public get material(): UnityEngine.Material;
            public set material(value: UnityEngine.Material);
            public get materialKeywords(): System.Array$1<string>;
            public set materialKeywords(value: System.Array$1<string>);
            public get enabled(): boolean;
            public set enabled(value: boolean);
            public get sortingOrder(): number;
            public set sortingOrder(value: number);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get vertexMatrix(): FairyGUI.NGraphics.VertexMatrix;
            public set vertexMatrix(value: FairyGUI.NGraphics.VertexMatrix);
            public get materialPropertyBlock(): UnityEngine.MaterialPropertyBlock;
            public add_meshModifier ($value: System.Action) : void
            public remove_meshModifier ($value: System.Action) : void
            public SetShaderAndTexture ($shader: string, $texture: FairyGUI.NTexture) : void
            public SetMaterial ($material: UnityEngine.Material) : void
            public ToggleKeyword ($keyword: string, $enabled: boolean) : void
            public Tint () : void
            public SetMeshDirty () : void
            public UpdateMesh () : boolean
            public Dispose () : void
            public Update ($context: FairyGUI.UpdateContext, $alpha: number, $grayed: boolean) : void
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ($gameObject: UnityEngine.GameObject)
            public constructor ()
        }
        class Stage extends FairyGUI.Container implements FairyGUI.IEventDispatcher
        {
            public get soundVolume(): number;
            public set soundVolume(value: number);
            public static get inst(): FairyGUI.Stage;
            public static get touchScreen(): boolean;
            public static set touchScreen(value: boolean);
            public static get keyboardInput(): boolean;
            public static set keyboardInput(value: boolean);
            public static get isTouchOnUI(): boolean;
            public static get devicePixelRatio(): number;
            public static set devicePixelRatio(value: number);
            public get onStageResized(): FairyGUI.EventListener;
            public get touchTarget(): FairyGUI.DisplayObject;
            public get focus(): FairyGUI.DisplayObject;
            public set focus(value: FairyGUI.DisplayObject);
            public get touchPosition(): UnityEngine.Vector2;
            public get touchCount(): number;
            public get keyboard(): FairyGUI.IKeyboard;
            public set keyboard(value: FairyGUI.IKeyboard);
            public get activeCursor(): string;
            public add_beforeUpdate ($value: System.Action) : void
            public remove_beforeUpdate ($value: System.Action) : void
            public add_afterUpdate ($value: System.Action) : void
            public remove_afterUpdate ($value: System.Action) : void
            public static Instantiate () : void
            public SetFocus ($newFocus: FairyGUI.DisplayObject, $byKey?: boolean) : void
            public DoKeyNavigate ($backward: boolean) : void
            public GetTouchPosition ($touchId: number) : UnityEngine.Vector2
            public GetTouchTarget ($touchId: number) : FairyGUI.DisplayObject
            public GetAllTouch ($result: System.Array$1<number>) : System.Array$1<number>
            public ResetInputState () : void
            public CancelClick ($touchId: number) : void
            public EnableSound () : void
            public DisableSound () : void
            public PlayOneShotSound ($clip: UnityEngine.AudioClip, $volumeScale: number) : void
            public PlayOneShotSound ($clip: UnityEngine.AudioClip) : void
            public OpenKeyboard ($text: string, $autocorrection: boolean, $multiline: boolean, $secure: boolean, $alert: boolean, $textPlaceholder: string, $keyboardType: number, $hideInput: boolean) : void
            public CloseKeyboard () : void
            public InputString ($value: string) : void
            public SetCustomInput ($screenPos: UnityEngine.Vector2, $buttonDown: boolean) : void
            public SetCustomInput ($screenPos: UnityEngine.Vector2, $buttonDown: boolean, $buttonUp: boolean) : void
            public SetCustomInput ($hit: $Ref<UnityEngine.RaycastHit>, $buttonDown: boolean) : void
            public SetCustomInput ($hit: $Ref<UnityEngine.RaycastHit>, $buttonDown: boolean, $buttonUp: boolean) : void
            public ForceUpdate () : void
            public ApplyPanelOrder ($target: FairyGUI.Container) : void
            public SortWorldSpacePanelsByZOrder ($panelSortingOrder: number) : void
            public MonitorTexture ($texture: FairyGUI.NTexture) : void
            public AddTouchMonitor ($touchId: number, $target: FairyGUI.EventDispatcher) : void
            public RemoveTouchMonitor ($target: FairyGUI.EventDispatcher) : void
            public IsTouchMonitoring ($target: FairyGUI.EventDispatcher) : boolean
            public RegisterCursor ($cursorName: string, $texture: UnityEngine.Texture2D, $hotspot: UnityEngine.Vector2) : void
            public constructor ()
        }
        class Margin extends System.ValueType
        {
            public left : number
            public right : number
            public top : number
            public bottom : number
        }
        interface IFilter
        {
            target : FairyGUI.DisplayObject
            Update () : void
            Dispose () : void
        }
        class DisplayObjectInfo extends UnityEngine.MonoBehaviour
        {
            public displayObject : FairyGUI.DisplayObject
            public constructor ()
        }
        class GoWrapper extends FairyGUI.DisplayObject implements FairyGUI.IEventDispatcher
        {
            public customCloneMaterials : System.Action$1<System.Collections.Generic.Dictionary$2<UnityEngine.Material, UnityEngine.Material>>
            public customRecoverMaterials : System.Action
            public get wrapTarget(): UnityEngine.GameObject;
            public set wrapTarget(value: UnityEngine.GameObject);
            public get renderingOrder(): number;
            public set renderingOrder(value: number);
            public add_onUpdate ($value: System.Action$1<FairyGUI.UpdateContext>) : void
            public remove_onUpdate ($value: System.Action$1<FairyGUI.UpdateContext>) : void
            public SetWrapTarget ($target: UnityEngine.GameObject, $cloneMaterial: boolean) : void
            public CacheRenderers () : void
            public constructor ()
            public constructor ($go: UnityEngine.GameObject)
        }
        class ColliderHitTest extends System.Object implements FairyGUI.IHitTest
        {
            public collider : UnityEngine.Collider
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
            public constructor ()
        }
        class HitTestContext extends System.Object
        {
            public static screenPoint : UnityEngine.Vector3
            public static worldPoint : UnityEngine.Vector3
            public static direction : UnityEngine.Vector3
            public static forTouch : boolean
            public static camera : UnityEngine.Camera
            public static layerMask : number
            public static maxDistance : number
            public static cachedMainCamera : UnityEngine.Camera
            public static GetRaycastHitFromCache ($camera: UnityEngine.Camera, $hit: $Ref<UnityEngine.RaycastHit>) : boolean
            public static CacheRaycastHit ($camera: UnityEngine.Camera, $hit: $Ref<UnityEngine.RaycastHit>) : void
            public static ClearRaycastHitCache () : void
            public constructor ()
        }
        class MeshColliderHitTest extends FairyGUI.ColliderHitTest implements FairyGUI.IHitTest
        {
            public lastHit : UnityEngine.Vector2
            public constructor ($collider: UnityEngine.MeshCollider)
            public constructor ()
        }
        class PixelHitTestData extends System.Object
        {
            public pixelWidth : number
            public scale : number
            public pixels : System.Array$1<number>
            public pixelsLength : number
            public pixelsOffset : number
            public Load ($ba: FairyGUI.Utils.ByteBuffer) : void
            public constructor ()
        }
        class PixelHitTest extends System.Object implements FairyGUI.IHitTest
        {
            public offsetX : number
            public offsetY : number
            public sourceWidth : number
            public sourceHeight : number
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
            public constructor ($data: FairyGUI.PixelHitTestData, $offsetX: number, $offsetY: number, $sourceWidth: number, $sourceHeight: number)
            public constructor ()
        }
        class RectHitTest extends System.Object implements FairyGUI.IHitTest
        {
            public rect : UnityEngine.Rect
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
            public constructor ()
        }
        class ShapeHitTest extends System.Object implements FairyGUI.IHitTest
        {
            public shape : FairyGUI.DisplayObject
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
            public constructor ($obj: FairyGUI.DisplayObject)
            public constructor ()
        }
        class MaterialManager extends System.Object
        {
            public firstMaterialInFrame : boolean
            public add_onCreateNewMaterial ($value: System.Action$1<UnityEngine.Material>) : void
            public remove_onCreateNewMaterial ($value: System.Action$1<UnityEngine.Material>) : void
            public GetFlagsByKeywords ($keywords: System.Collections.Generic.IList$1<string>) : number
            public GetMaterial ($flags: number, $blendMode: FairyGUI.BlendMode, $group: number) : UnityEngine.Material
            public DestroyMaterials () : void
            public RefreshMaterials () : void
        }
        class CompositeMesh extends System.Object implements FairyGUI.IHitTest, FairyGUI.IMeshFactory
        {
            public elements : System.Collections.Generic.List$1<FairyGUI.IMeshFactory>
            public activeIndex : number
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public HitTest ($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2) : boolean
            public constructor ()
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
        }
        class EllipseMesh extends System.Object implements FairyGUI.IHitTest, FairyGUI.IMeshFactory
        {
            public drawRect : System.Nullable$1<UnityEngine.Rect>
            public lineWidth : number
            public lineColor : UnityEngine.Color32
            public centerColor : System.Nullable$1<UnityEngine.Color32>
            public fillColor : System.Nullable$1<UnityEngine.Color32>
            public startDegree : number
            public endDegreee : number
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public HitTest ($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2) : boolean
            public constructor ()
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
        }
        class FillMesh extends System.Object implements FairyGUI.IMeshFactory
        {
            public method : FairyGUI.FillMethod
            public origin : number
            public amount : number
            public clockwise : boolean
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
        }
        class PlaneMesh extends System.Object implements FairyGUI.IMeshFactory
        {
            public gridSize : number
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
        }
        class PolygonMesh extends System.Object implements FairyGUI.IHitTest, FairyGUI.IMeshFactory
        {
            public points : System.Collections.Generic.List$1<UnityEngine.Vector2>
            public texcoords : System.Collections.Generic.List$1<UnityEngine.Vector2>
            public lineWidth : number
            public lineColor : UnityEngine.Color32
            public fillColor : System.Nullable$1<UnityEngine.Color32>
            public colors : System.Array$1<UnityEngine.Color32>
            public usePercentPositions : boolean
            public Add ($point: UnityEngine.Vector2) : void
            public Add ($point: UnityEngine.Vector2, $texcoord: UnityEngine.Vector2) : void
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public HitTest ($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2) : boolean
            public constructor ()
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
        }
        class RectMesh extends System.Object implements FairyGUI.IHitTest, FairyGUI.IMeshFactory
        {
            public drawRect : System.Nullable$1<UnityEngine.Rect>
            public lineWidth : number
            public lineColor : UnityEngine.Color32
            public fillColor : System.Nullable$1<UnityEngine.Color32>
            public colors : System.Array$1<UnityEngine.Color32>
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public HitTest ($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2) : boolean
            public constructor ()
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
        }
        class RegularPolygonMesh extends System.Object implements FairyGUI.IHitTest, FairyGUI.IMeshFactory
        {
            public drawRect : System.Nullable$1<UnityEngine.Rect>
            public sides : number
            public lineWidth : number
            public lineColor : UnityEngine.Color32
            public centerColor : System.Nullable$1<UnityEngine.Color32>
            public fillColor : System.Nullable$1<UnityEngine.Color32>
            public distances : System.Array$1<number>
            public rotation : number
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public HitTest ($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2) : boolean
            public constructor ()
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
        }
        class RoundedRectMesh extends System.Object implements FairyGUI.IHitTest, FairyGUI.IMeshFactory
        {
            public drawRect : System.Nullable$1<UnityEngine.Rect>
            public lineWidth : number
            public lineColor : UnityEngine.Color32
            public fillColor : System.Nullable$1<UnityEngine.Color32>
            public topLeftRadius : number
            public topRightRadius : number
            public bottomLeftRadius : number
            public bottomRightRadius : number
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public HitTest ($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2) : boolean
            public constructor ()
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
        }
        class MovieClip extends FairyGUI.Image implements FairyGUI.IEventDispatcher, FairyGUI.IMeshFactory
        {
            public interval : number
            public swing : boolean
            public repeatDelay : number
            public timeScale : number
            public ignoreEngineTimeScale : boolean
            public get onPlayEnd(): FairyGUI.EventListener;
            public get frames(): System.Array$1<FairyGUI.MovieClip.Frame>;
            public set frames(value: System.Array$1<FairyGUI.MovieClip.Frame>);
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public Rewind () : void
            public SyncStatus ($anotherMc: FairyGUI.MovieClip) : void
            public Advance ($time: number) : void
            public SetPlaySettings () : void
            public SetPlaySettings ($start: number, $end: number, $times: number, $endAt: number) : void
            public constructor ()
        }
        class NAudioClip extends System.Object
        {
            public static CustomDestroyMethod : System.Action$1<UnityEngine.AudioClip>
            public destroyMethod : FairyGUI.DestroyMethod
            public nativeClip : UnityEngine.AudioClip
            public Unload () : void
            public Reload ($audioClip: UnityEngine.AudioClip) : void
            public constructor ($audioClip: UnityEngine.AudioClip)
            public constructor ()
        }
        enum DestroyMethod
        { Destroy = 0, Unload = 1, None = 2, ReleaseTemp = 3, Custom = 4 }
        class ShaderConfig extends System.Object
        {
            public static Get : FairyGUI.ShaderConfig.GetFunction
            public static imageShader : string
            public static textShader : string
            public static bmFontShader : string
            public static TMPFontShader : string
            public static ID_ClipBox : number
            public static ID_ClipSoftness : number
            public static ID_AlphaTex : number
            public static ID_StencilComp : number
            public static ID_Stencil : number
            public static ID_StencilOp : number
            public static ID_StencilReadMask : number
            public static ID_ColorMask : number
            public static ID_ColorMatrix : number
            public static ID_ColorOffset : number
            public static ID_BlendSrcFactor : number
            public static ID_BlendDstFactor : number
            public static ID_ColorOption : number
            public static ID_Stencil2 : number
            public static GetShader ($name: string) : UnityEngine.Shader
        }
        interface IKeyboard
        {
            done : boolean
            supportsCaret : boolean
            GetInput () : string
            Open ($text: string, $autocorrection: boolean, $multiline: boolean, $secure: boolean, $alert: boolean, $textPlaceholder: string, $keyboardType: number, $hideInput: boolean) : void
            Close () : void
        }
        class StageCamera extends UnityEngine.MonoBehaviour
        {
            public constantSize : boolean
            public unitsPerPixel : number
            public cachedTransform : UnityEngine.Transform
            public cachedCamera : UnityEngine.Camera
            public static main : UnityEngine.Camera
            public static screenSizeVer : number
            public static Name : string
            public static LayerName : string
            public static DefaultCameraSize : number
            public static DefaultUnitsPerPixel : number
            public ApplyModifiedProperties () : void
            public static CheckMainCamera () : void
            public static CheckCaptureCamera () : void
            public static CreateCamera ($name: string, $cullingMask: number) : UnityEngine.Camera
            public constructor ()
        }
        class StageEngine extends UnityEngine.MonoBehaviour
        {
            public ObjectsOnStage : number
            public GraphicsOnStage : number
            public static beingQuit : boolean
            public constructor ()
        }
        class Stats extends System.Object
        {
            public static ObjectCount : number
            public static GraphicsCount : number
            public static LatestObjectCreation : number
            public static LatestGraphicsCreation : number
            public constructor ()
        }
        class DynamicFont extends FairyGUI.BaseFont
        {
            public get nativeFont(): UnityEngine.Font;
            public set nativeFont(value: UnityEngine.Font);
            public constructor ()
            public constructor ($name: string, $font: UnityEngine.Font)
        }
        class Emoji extends System.Object
        {
            public url : string
            public width : number
            public height : number
            public constructor ($url: string, $width: number, $height: number)
            public constructor ($url: string)
            public constructor ()
        }
        class FontManager extends System.Object
        {
            public static sFontFactory : System.Collections.Generic.Dictionary$2<string, FairyGUI.BaseFont>
            public static RegisterFont ($font: FairyGUI.BaseFont, $alias?: string) : void
            public static UnregisterFont ($font: FairyGUI.BaseFont) : void
            public static GetFont ($name: string) : FairyGUI.BaseFont
            public static Clear () : void
            public constructor ()
        }
        class InputTextField extends FairyGUI.RichTextField implements FairyGUI.IEventDispatcher
        {
            public static onCopy : System.Action$2<FairyGUI.InputTextField, string>
            public static onPaste : System.Action$1<FairyGUI.InputTextField>
            public static contextMenu : FairyGUI.PopupMenu
            public get maxLength(): number;
            public set maxLength(value: number);
            public get keyboardInput(): boolean;
            public set keyboardInput(value: boolean);
            public get keyboardType(): number;
            public set keyboardType(value: number);
            public get hideInput(): boolean;
            public set hideInput(value: boolean);
            public get disableIME(): boolean;
            public set disableIME(value: boolean);
            public get mouseWheelEnabled(): boolean;
            public set mouseWheelEnabled(value: boolean);
            public get onChanged(): FairyGUI.EventListener;
            public get onSubmit(): FairyGUI.EventListener;
            public get text(): string;
            public set text(value: string);
            public get textFormat(): FairyGUI.TextFormat;
            public set textFormat(value: FairyGUI.TextFormat);
            public get restrict(): string;
            public set restrict(value: string);
            public get caretPosition(): number;
            public set caretPosition(value: number);
            public get selectionBeginIndex(): number;
            public get selectionEndIndex(): number;
            public get promptText(): string;
            public set promptText(value: string);
            public get displayAsPassword(): boolean;
            public set displayAsPassword(value: boolean);
            public get editable(): boolean;
            public set editable(value: boolean);
            public get border(): number;
            public set border(value: number);
            public get corner(): number;
            public set corner(value: number);
            public get borderColor(): UnityEngine.Color;
            public set borderColor(value: UnityEngine.Color);
            public get backgroundColor(): UnityEngine.Color;
            public set backgroundColor(value: UnityEngine.Color);
            public SetSelection ($start: number, $length: number) : void
            public ReplaceSelection ($value: string) : void
            public ReplaceText ($value: string) : void
            public GetSelection () : string
            public constructor ()
        }
        class PopupMenu extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public visibleItemCount : number
            public hideOnClickItem : boolean
            public autoSize : boolean
            public get onPopup(): FairyGUI.EventListener;
            public get onClose(): FairyGUI.EventListener;
            public get itemCount(): number;
            public get contentPane(): FairyGUI.GComponent;
            public get list(): FairyGUI.GList;
            public AddItem ($caption: string, $callback: FairyGUI.EventCallback0) : FairyGUI.GButton
            public AddItem ($caption: string, $callback: FairyGUI.EventCallback1) : FairyGUI.GButton
            public AddItemAt ($caption: string, $index: number, $callback: FairyGUI.EventCallback1) : FairyGUI.GButton
            public AddItemAt ($caption: string, $index: number, $callback: FairyGUI.EventCallback0) : FairyGUI.GButton
            public AddSeperator () : void
            public AddSeperator ($index: number) : void
            public GetItemName ($index: number) : string
            public SetItemText ($name: string, $caption: string) : void
            public SetItemVisible ($name: string, $visible: boolean) : void
            public SetItemGrayed ($name: string, $grayed: boolean) : void
            public SetItemCheckable ($name: string, $checkable: boolean) : void
            public SetItemChecked ($name: string, $check: boolean) : void
            public IsItemChecked ($name: string) : boolean
            public RemoveItem ($name: string) : void
            public ClearItems () : void
            public Dispose () : void
            public Show () : void
            public Show ($target: FairyGUI.GObject) : void
            public Show ($target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection) : void
            public Show ($target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection, $parentMenu: FairyGUI.PopupMenu) : void
            public Hide () : void
            public constructor ()
            public constructor ($resourceURL: string)
        }
        class TextField extends FairyGUI.DisplayObject implements FairyGUI.IEventDispatcher, FairyGUI.IMeshFactory
        {
            public get textFormat(): FairyGUI.TextFormat;
            public set textFormat(value: FairyGUI.TextFormat);
            public get align(): FairyGUI.AlignType;
            public set align(value: FairyGUI.AlignType);
            public get verticalAlign(): FairyGUI.VertAlignType;
            public set verticalAlign(value: FairyGUI.VertAlignType);
            public get text(): string;
            public set text(value: string);
            public get htmlText(): string;
            public set htmlText(value: string);
            public get parsedText(): string;
            public get autoSize(): FairyGUI.AutoSizeType;
            public set autoSize(value: FairyGUI.AutoSizeType);
            public get wordWrap(): boolean;
            public set wordWrap(value: boolean);
            public get singleLine(): boolean;
            public set singleLine(value: boolean);
            public get stroke(): number;
            public set stroke(value: number);
            public get strokeColor(): UnityEngine.Color;
            public set strokeColor(value: UnityEngine.Color);
            public get shadowOffset(): UnityEngine.Vector2;
            public set shadowOffset(value: UnityEngine.Vector2);
            public get textWidth(): number;
            public get textHeight(): number;
            public get maxWidth(): number;
            public set maxWidth(value: number);
            public get htmlElements(): System.Collections.Generic.List$1<FairyGUI.Utils.HtmlElement>;
            public get lines(): System.Collections.Generic.List$1<FairyGUI.TextField.LineInfo>;
            public get charPositions(): System.Collections.Generic.List$1<FairyGUI.TextField.CharPosition>;
            public get richTextField(): FairyGUI.RichTextField;
            public EnableCharPositionSupport () : void
            public ApplyFormat () : void
            public Redraw () : boolean
            public HasCharacter ($ch: number) : boolean
            public GetLinesShape ($startLine: number, $startCharX: number, $endLine: number, $endCharX: number, $clipped: boolean, $resultRects: System.Collections.Generic.List$1<UnityEngine.Rect>) : void
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
        }
        class RTLSupport extends System.Object
        {
            public static BaseDirection : FairyGUI.RTLSupport.DirectionType
            public static IsArabicLetter ($ch: number) : boolean
            public static ConvertNumber ($strNumber: string) : string
            public static ContainsArabicLetters ($text: string) : boolean
            public static DetectTextDirection ($text: string) : FairyGUI.RTLSupport.DirectionType
            public static DoMapping ($input: string) : string
            public static ConvertLineL ($source: string) : string
            public static ConvertLineR ($source: string) : string
            public constructor ()
        }
        class SelectionShape extends FairyGUI.DisplayObject implements FairyGUI.IEventDispatcher, FairyGUI.IMeshFactory
        {
            public rects : System.Collections.Generic.List$1<UnityEngine.Rect>
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public Refresh () : void
            public Clear () : void
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
        }
        class TouchScreenKeyboard extends System.Object implements FairyGUI.IKeyboard
        {
            public get done(): boolean;
            public get supportsCaret(): boolean;
            public GetInput () : string
            public Open ($text: string, $autocorrection: boolean, $multiline: boolean, $secure: boolean, $alert: boolean, $textPlaceholder: string, $keyboardType: number, $hideInput: boolean) : void
            public Close () : void
            public constructor ()
        }
        class TypingEffect extends System.Object
        {
            public Start () : void
            public Print () : boolean
            public Print ($interval: number) : System.Collections.IEnumerator
            public PrintAll ($interval: number) : void
            public Cancel () : void
            public constructor ($textField: FairyGUI.TextField)
            public constructor ($textField: FairyGUI.GTextField)
            public constructor ()
        }
        interface EventCallback0
        { () : void; }
        var EventCallback0: { new (func: () => void): EventCallback0; }
        class GLoader3D extends FairyGUI.GObject implements FairyGUI.IEventDispatcher, FairyGUI.IAnimationGear, FairyGUI.IColorGear
        {
            public get armatureComponent(): DragonBones.UnityArmatureComponent;
            public get spineAnimation(): Spine.Unity.SkeletonAnimation;
            public get url(): string;
            public set url(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get align(): FairyGUI.AlignType;
            public set align(value: FairyGUI.AlignType);
            public get verticalAlign(): FairyGUI.VertAlignType;
            public set verticalAlign(value: FairyGUI.VertAlignType);
            public get fill(): FairyGUI.FillType;
            public set fill(value: FairyGUI.FillType);
            public get shrinkOnly(): boolean;
            public set shrinkOnly(value: boolean);
            public get autoSize(): boolean;
            public set autoSize(value: boolean);
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public get timeScale(): number;
            public set timeScale(value: number);
            public get ignoreEngineTimeScale(): boolean;
            public set ignoreEngineTimeScale(value: boolean);
            public get loop(): boolean;
            public set loop(value: boolean);
            public get animationName(): string;
            public set animationName(value: string);
            public get skinName(): string;
            public set skinName(value: string);
            public get material(): UnityEngine.Material;
            public set material(value: UnityEngine.Material);
            public get shader(): string;
            public set shader(value: string);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get wrapTarget(): UnityEngine.GameObject;
            public get filter(): FairyGUI.IFilter;
            public set filter(value: FairyGUI.IFilter);
            public get blendMode(): FairyGUI.BlendMode;
            public set blendMode(value: FairyGUI.BlendMode);
            public SetDragonBones ($asset: DragonBones.DragonBonesData, $width: number, $height: number, $anchor: UnityEngine.Vector2) : void
            public SetSpine ($asset: Spine.Unity.SkeletonDataAsset, $width: number, $height: number, $anchor: UnityEngine.Vector2) : void
            public SetSpine ($asset: Spine.Unity.SkeletonDataAsset, $width: number, $height: number, $anchor: UnityEngine.Vector2, $cloneMaterial: boolean) : void
            public Advance ($time: number) : void
            public SetWrapTarget ($gameObject: UnityEngine.GameObject, $cloneMaterial: boolean, $width: number, $height: number) : void
            public constructor ()
        }
        enum FillType
        { None = 0, Scale = 1, ScaleMatchHeight = 2, ScaleMatchWidth = 3, ScaleFree = 4, ScaleNoBorder = 5 }
        class ExternalFont extends FairyGUI.BaseFont
        {
            public get samplePointSize(): number;
            public set samplePointSize(value: number);
            public get renderMode(): UnityEngine.TextCore.LowLevel.GlyphRenderMode;
            public set renderMode(value: UnityEngine.TextCore.LowLevel.GlyphRenderMode);
            public Load ($file: string) : void
            public constructor ()
        }
        class ExternalTMPFont extends FairyGUI.TMPFont
        {
            public Load ($file: string, $samplePointSize: number, $atlasPadding: number) : void
            public constructor ()
        }
        class TMPFont extends FairyGUI.BaseFont
        {
            public get fontAsset(): TMPro.TMP_FontAsset;
            public set fontAsset(value: TMPro.TMP_FontAsset);
            public get fontWeight(): TMPro.FontWeight;
            public set fontWeight(value: TMPro.FontWeight);
            public constructor ()
        }
        class BlurFilter extends System.Object implements FairyGUI.IFilter
        {
            public blurSize : number
            public get target(): FairyGUI.DisplayObject;
            public set target(value: FairyGUI.DisplayObject);
            public Dispose () : void
            public Update () : void
            public constructor ()
        }
        class ColorFilter extends System.Object implements FairyGUI.IFilter
        {
            public get target(): FairyGUI.DisplayObject;
            public set target(value: FairyGUI.DisplayObject);
            public Dispose () : void
            public Update () : void
            public Invert () : void
            public AdjustSaturation ($sat: number) : void
            public AdjustContrast ($value: number) : void
            public AdjustBrightness ($value: number) : void
            public AdjustHue ($value: number) : void
            public Tint ($color: UnityEngine.Color, $amount?: number) : void
            public Reset () : void
            public ConcatValues (...values: number[]) : void
            public constructor ()
        }
        class LongPressGesture extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public trigger : number
            public interval : number
            public once : boolean
            public holdRangeRadius : number
            public static TRIGGER : number
            public static INTERVAL : number
            public get host(): FairyGUI.GObject;
            public get onBegin(): FairyGUI.EventListener;
            public get onEnd(): FairyGUI.EventListener;
            public get onAction(): FairyGUI.EventListener;
            public Dispose () : void
            public Enable ($value: boolean) : void
            public Cancel () : void
            public constructor ($host: FairyGUI.GObject)
            public constructor ()
        }
        class PinchGesture extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public scale : number
            public delta : number
            public get host(): FairyGUI.GObject;
            public get onBegin(): FairyGUI.EventListener;
            public get onEnd(): FairyGUI.EventListener;
            public get onAction(): FairyGUI.EventListener;
            public Dispose () : void
            public Enable ($value: boolean) : void
            public constructor ($host: FairyGUI.GObject)
            public constructor ()
        }
        class RotationGesture extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public rotation : number
            public delta : number
            public snapping : boolean
            public get host(): FairyGUI.GObject;
            public get onBegin(): FairyGUI.EventListener;
            public get onEnd(): FairyGUI.EventListener;
            public get onAction(): FairyGUI.EventListener;
            public Dispose () : void
            public Enable ($value: boolean) : void
            public constructor ($host: FairyGUI.GObject)
            public constructor ()
        }
        class SwipeGesture extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public velocity : UnityEngine.Vector2
            public position : UnityEngine.Vector2
            public delta : UnityEngine.Vector2
            public actionDistance : number
            public snapping : boolean
            public static ACTION_DISTANCE : number
            public get host(): FairyGUI.GObject;
            public get onBegin(): FairyGUI.EventListener;
            public get onEnd(): FairyGUI.EventListener;
            public get onMove(): FairyGUI.EventListener;
            public get onAction(): FairyGUI.EventListener;
            public Dispose () : void
            public Enable ($value: boolean) : void
            public constructor ($host: FairyGUI.GObject)
            public constructor ()
        }
        class EaseManager extends System.Object
        {
            public static Evaluate ($easeType: FairyGUI.EaseType, $time: number, $duration: number, $overshootOrAmplitude?: number, $period?: number, $customEase?: FairyGUI.CustomEase) : number
        }
        class GTween extends System.Object
        {
            public static catchCallbackExceptions : boolean
            public static To ($startValue: number, $endValue: number, $duration: number) : FairyGUI.GTweener
            public static To ($startValue: UnityEngine.Vector2, $endValue: UnityEngine.Vector2, $duration: number) : FairyGUI.GTweener
            public static To ($startValue: UnityEngine.Vector3, $endValue: UnityEngine.Vector3, $duration: number) : FairyGUI.GTweener
            public static To ($startValue: UnityEngine.Vector4, $endValue: UnityEngine.Vector4, $duration: number) : FairyGUI.GTweener
            public static To ($startValue: UnityEngine.Color, $endValue: UnityEngine.Color, $duration: number) : FairyGUI.GTweener
            public static ToDouble ($startValue: number, $endValue: number, $duration: number) : FairyGUI.GTweener
            public static DelayedCall ($delay: number) : FairyGUI.GTweener
            public static Shake ($startValue: UnityEngine.Vector3, $amplitude: number, $duration: number) : FairyGUI.GTweener
            public static IsTweening ($target: any) : boolean
            public static IsTweening ($target: any, $propType: FairyGUI.TweenPropType) : boolean
            public static Kill ($target: any) : void
            public static Kill ($target: any, $complete: boolean) : void
            public static Kill ($target: any, $propType: FairyGUI.TweenPropType, $complete: boolean) : void
            public static GetTween ($target: any) : FairyGUI.GTweener
            public static GetTween ($target: any, $propType: FairyGUI.TweenPropType) : FairyGUI.GTweener
            public static Clean () : void
            public constructor ()
        }
        enum TweenPropType
        { None = 0, X = 1, Y = 2, Z = 3, XY = 4, Position = 5, Width = 6, Height = 7, Size = 8, ScaleX = 9, ScaleY = 10, Scale = 11, Rotation = 12, RotationX = 13, RotationY = 14, Alpha = 15, Progress = 16 }
        interface ITweenListener
        {
            OnTweenStart ($tweener: FairyGUI.GTweener) : void
            OnTweenUpdate ($tweener: FairyGUI.GTweener) : void
            OnTweenComplete ($tweener: FairyGUI.GTweener) : void
        }
        interface GTweenCallback
        { () : void; }
        var GTweenCallback: { new (func: () => void): GTweenCallback; }
        interface GTweenCallback1
        { (tweener: FairyGUI.GTweener) : void; }
        var GTweenCallback1: { new (func: (tweener: FairyGUI.GTweener) => void): GTweenCallback1; }
        class TweenValue extends System.Object
        {
            public x : number
            public y : number
            public z : number
            public w : number
            public d : number
            public get vec2(): UnityEngine.Vector2;
            public set vec2(value: UnityEngine.Vector2);
            public get vec3(): UnityEngine.Vector3;
            public set vec3(value: UnityEngine.Vector3);
            public get vec4(): UnityEngine.Vector4;
            public set vec4(value: UnityEngine.Vector4);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get_Item ($index: number) : number
            public set_Item ($index: number, $value: number) : void
            public SetZero () : void
            public constructor ()
        }
        class ChangePageAction extends FairyGUI.ControllerAction
        {
            public objectId : string
            public controllerName : string
            public targetPage : string
            public constructor ()
        }
        class ControllerAction extends System.Object
        {
            public fromPage : System.Array$1<string>
            public toPage : System.Array$1<string>
            public static CreateAction ($type: FairyGUI.ControllerAction.ActionType) : FairyGUI.ControllerAction
            public Run ($controller: FairyGUI.Controller, $prevPage: string, $curPage: string) : void
            public Setup ($buffer: FairyGUI.Utils.ByteBuffer) : void
            public constructor ()
        }
        class Controller extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public name : string
            public get onChanged(): FairyGUI.EventListener;
            public get selectedIndex(): number;
            public set selectedIndex(value: number);
            public get selectedPage(): string;
            public set selectedPage(value: string);
            public get previsousIndex(): number;
            public get previousPage(): string;
            public get pageCount(): number;
            public Dispose () : void
            public SetSelectedIndex ($value: number) : void
            public SetSelectedPage ($value: string) : void
            public GetPageName ($index: number) : string
            public GetPageId ($index: number) : string
            public GetPageIdByName ($aName: string) : string
            public AddPage ($name: string) : void
            public AddPageAt ($name: string, $index: number) : void
            public RemovePage ($name: string) : void
            public RemovePageAt ($index: number) : void
            public ClearPages () : void
            public HasPage ($aName: string) : boolean
            public RunActions () : void
            public Setup ($buffer: FairyGUI.Utils.ByteBuffer) : void
            public constructor ()
        }
        class PlayTransitionAction extends FairyGUI.ControllerAction
        {
            public transitionName : string
            public playTimes : number
            public delay : number
            public stopOnExit : boolean
            public constructor ()
        }
        class AsyncCreationHelper extends System.Object
        {
            public static CreateObject ($item: FairyGUI.PackageItem, $callback: FairyGUI.UIPackage.CreateObjectCallback) : void
            public constructor ()
        }
        class PackageItem extends System.Object
        {
            public owner : FairyGUI.UIPackage
            public type : FairyGUI.PackageItemType
            public objectType : FairyGUI.ObjectType
            public id : string
            public name : string
            public width : number
            public height : number
            public file : string
            public exported : boolean
            public texture : FairyGUI.NTexture
            public rawData : FairyGUI.Utils.ByteBuffer
            public branches : System.Array$1<string>
            public highResolution : System.Array$1<string>
            public scale9Grid : System.Nullable$1<UnityEngine.Rect>
            public scaleByTile : boolean
            public tileGridIndice : number
            public pixelHitTestData : FairyGUI.PixelHitTestData
            public interval : number
            public repeatDelay : number
            public swing : boolean
            public frames : System.Array$1<FairyGUI.MovieClip.Frame>
            public translated : boolean
            public extensionCreator : FairyGUI.UIObjectFactory.GComponentCreator
            public bitmapFont : FairyGUI.BitmapFont
            public audioClip : FairyGUI.NAudioClip
            public skeletonAnchor : UnityEngine.Vector2
            public skeletonAsset : any
            public Load () : any
            public getBranch () : FairyGUI.PackageItem
            public getHighResolution () : FairyGUI.PackageItem
            public constructor ()
        }
        class DragDropManager extends System.Object
        {
            public static get inst(): FairyGUI.DragDropManager;
            public get dragAgent(): FairyGUI.GLoader;
            public get dragging(): boolean;
            public StartDrag ($source: FairyGUI.GObject, $icon: string, $sourceData: any, $touchPointID?: number) : void
            public Cancel () : void
            public constructor ()
        }
        interface EMRenderTarget
        {
            EM_sortingOrder : number
            EM_BeforeUpdate () : void
            EM_Update ($context: FairyGUI.UpdateContext) : void
            EM_Reload () : void
        }
        class EMRenderSupport extends System.Object
        {
            public static orderChanged : boolean
            public static get packageListReady(): boolean;
            public static get hasTarget(): boolean;
            public static Add ($value: FairyGUI.EMRenderTarget) : void
            public static Remove ($value: FairyGUI.EMRenderTarget) : void
            public static Update () : void
            public static Reload () : void
            public constructor ()
        }
        enum ButtonMode
        { Common = 0, Check = 1, Radio = 2 }
        class ScrollPane extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public static TWEEN_TIME_GO : number
            public static TWEEN_TIME_DEFAULT : number
            public static PULL_RATIO : number
            public static get draggingPane(): FairyGUI.ScrollPane;
            public get onScroll(): FairyGUI.EventListener;
            public get onScrollEnd(): FairyGUI.EventListener;
            public get onPullDownRelease(): FairyGUI.EventListener;
            public get onPullUpRelease(): FairyGUI.EventListener;
            public get owner(): FairyGUI.GComponent;
            public get hzScrollBar(): FairyGUI.GScrollBar;
            public get vtScrollBar(): FairyGUI.GScrollBar;
            public get header(): FairyGUI.GComponent;
            public get footer(): FairyGUI.GComponent;
            public get bouncebackEffect(): boolean;
            public set bouncebackEffect(value: boolean);
            public get touchEffect(): boolean;
            public set touchEffect(value: boolean);
            public get inertiaDisabled(): boolean;
            public set inertiaDisabled(value: boolean);
            public get softnessOnTopOrLeftSide(): boolean;
            public set softnessOnTopOrLeftSide(value: boolean);
            public get scrollStep(): number;
            public set scrollStep(value: number);
            public get snapToItem(): boolean;
            public set snapToItem(value: boolean);
            public get pageMode(): boolean;
            public set pageMode(value: boolean);
            public get pageController(): FairyGUI.Controller;
            public set pageController(value: FairyGUI.Controller);
            public get mouseWheelEnabled(): boolean;
            public set mouseWheelEnabled(value: boolean);
            public get decelerationRate(): number;
            public set decelerationRate(value: number);
            public get isDragged(): boolean;
            public get percX(): number;
            public set percX(value: number);
            public get percY(): number;
            public set percY(value: number);
            public get posX(): number;
            public set posX(value: number);
            public get posY(): number;
            public set posY(value: number);
            public get isBottomMost(): boolean;
            public get isRightMost(): boolean;
            public get currentPageX(): number;
            public set currentPageX(value: number);
            public get currentPageY(): number;
            public set currentPageY(value: number);
            public get scrollingPosX(): number;
            public get scrollingPosY(): number;
            public get contentWidth(): number;
            public get contentHeight(): number;
            public get viewWidth(): number;
            public set viewWidth(value: number);
            public get viewHeight(): number;
            public set viewHeight(value: number);
            public Setup ($buffer: FairyGUI.Utils.ByteBuffer) : void
            public Dispose () : void
            public SetPercX ($value: number, $ani: boolean) : void
            public SetPercY ($value: number, $ani: boolean) : void
            public SetPosX ($value: number, $ani: boolean) : void
            public SetPosY ($value: number, $ani: boolean) : void
            public SetCurrentPageX ($value: number, $ani: boolean) : void
            public SetCurrentPageY ($value: number, $ani: boolean) : void
            public ScrollTop () : void
            public ScrollTop ($ani: boolean) : void
            public ScrollBottom () : void
            public ScrollBottom ($ani: boolean) : void
            public ScrollUp () : void
            public ScrollUp ($ratio: number, $ani: boolean) : void
            public ScrollDown () : void
            public ScrollDown ($ratio: number, $ani: boolean) : void
            public ScrollLeft () : void
            public ScrollLeft ($ratio: number, $ani: boolean) : void
            public ScrollRight () : void
            public ScrollRight ($ratio: number, $ani: boolean) : void
            public ScrollToView ($obj: FairyGUI.GObject) : void
            public ScrollToView ($obj: FairyGUI.GObject, $ani: boolean) : void
            public ScrollToView ($obj: FairyGUI.GObject, $ani: boolean, $setFirst: boolean) : void
            public ScrollToView ($rect: UnityEngine.Rect, $ani: boolean, $setFirst: boolean) : void
            public IsChildInView ($obj: FairyGUI.GObject) : boolean
            public CancelDragging () : void
            public LockHeader ($size: number) : void
            public LockFooter ($size: number) : void
            public UpdateScrollBarVisible () : void
            public constructor ($owner: FairyGUI.GComponent)
            public constructor ()
        }
        enum ChildrenRenderOrder
        { Ascent = 0, Descent = 1, Arch = 2 }
        class GGroup extends FairyGUI.GObject implements FairyGUI.IEventDispatcher
        {
            public get layout(): FairyGUI.GroupLayoutType;
            public set layout(value: FairyGUI.GroupLayoutType);
            public get lineGap(): number;
            public set lineGap(value: number);
            public get columnGap(): number;
            public set columnGap(value: number);
            public get excludeInvisibles(): boolean;
            public set excludeInvisibles(value: boolean);
            public get autoSizeDisabled(): boolean;
            public set autoSizeDisabled(value: boolean);
            public get mainGridMinSize(): number;
            public set mainGridMinSize(value: number);
            public get mainGridIndex(): number;
            public set mainGridIndex(value: number);
            public SetBoundsChangedFlag ($positionChangedOnly?: boolean) : void
            public EnsureBoundsCorrect () : void
            public constructor ()
        }
        class Transition extends System.Object implements FairyGUI.ITweenListener
        {
            public invalidateBatchingEveryFrame : boolean
            public get name(): string;
            public get playing(): boolean;
            public get totalDuration(): number;
            public get timeScale(): number;
            public set timeScale(value: number);
            public get ignoreEngineTimeScale(): boolean;
            public set ignoreEngineTimeScale(value: boolean);
            public Play () : void
            public Play ($onComplete: FairyGUI.PlayCompleteCallback) : void
            public Play ($times: number, $delay: number, $onComplete: FairyGUI.PlayCompleteCallback) : void
            public Play ($times: number, $delay: number, $startTime: number, $endTime: number, $onComplete: FairyGUI.PlayCompleteCallback) : void
            public PlayReverse () : void
            public PlayReverse ($onComplete: FairyGUI.PlayCompleteCallback) : void
            public PlayReverse ($times: number, $delay: number, $onComplete: FairyGUI.PlayCompleteCallback) : void
            public ChangePlayTimes ($value: number) : void
            public SetAutoPlay ($autoPlay: boolean, $times: number, $delay: number) : void
            public Stop () : void
            public Stop ($setToComplete: boolean, $processCallback: boolean) : void
            public SetPaused ($paused: boolean) : void
            public Dispose () : void
            public SetValue ($label: string, ...aParams: any[]) : void
            public SetHook ($label: string, $callback: FairyGUI.TransitionHook) : void
            public ClearHooks () : void
            public SetTarget ($label: string, $newTarget: FairyGUI.GObject) : void
            public SetDuration ($label: string, $value: number) : void
            public GetLabelTime ($label: string) : number
            public OnTweenStart ($tweener: FairyGUI.GTweener) : void
            public OnTweenUpdate ($tweener: FairyGUI.GTweener) : void
            public OnTweenComplete ($tweener: FairyGUI.GTweener) : void
            public Setup ($buffer: FairyGUI.Utils.ByteBuffer) : void
            public constructor ($owner: FairyGUI.GComponent)
            public constructor ()
        }
        class GearAnimation extends FairyGUI.GearBase
        {
            public AddExtStatus ($pageId: string, $buffer: FairyGUI.Utils.ByteBuffer) : void
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearBase extends System.Object
        {
            public static disableAllTweenEffect : boolean
            public get controller(): FairyGUI.Controller;
            public set controller(value: FairyGUI.Controller);
            public get tweenConfig(): FairyGUI.GearTweenConfig;
            public Dispose () : void
            public Setup ($buffer: FairyGUI.Utils.ByteBuffer) : void
            public UpdateFromRelations ($dx: number, $dy: number) : void
            public Apply () : void
            public UpdateState () : void
        }
        class GearTweenConfig extends System.Object
        {
            public tween : boolean
            public easeType : FairyGUI.EaseType
            public customEase : FairyGUI.CustomEase
            public duration : number
            public delay : number
            public constructor ()
        }
        class GearColor extends FairyGUI.GearBase implements FairyGUI.ITweenListener
        {
            public OnTweenStart ($tweener: FairyGUI.GTweener) : void
            public OnTweenUpdate ($tweener: FairyGUI.GTweener) : void
            public OnTweenComplete ($tweener: FairyGUI.GTweener) : void
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearDisplay extends FairyGUI.GearBase
        {
            public get pages(): System.Array$1<string>;
            public set pages(value: System.Array$1<string>);
            public get connected(): boolean;
            public AddLock () : number
            public ReleaseLock ($token: number) : void
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearDisplay2 extends FairyGUI.GearBase
        {
            public condition : number
            public get pages(): System.Array$1<string>;
            public set pages(value: System.Array$1<string>);
            public Evaluate ($connected: boolean) : boolean
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearFontSize extends FairyGUI.GearBase
        {
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearIcon extends FairyGUI.GearBase
        {
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearLook extends FairyGUI.GearBase implements FairyGUI.ITweenListener
        {
            public OnTweenStart ($tweener: FairyGUI.GTweener) : void
            public OnTweenUpdate ($tweener: FairyGUI.GTweener) : void
            public OnTweenComplete ($tweener: FairyGUI.GTweener) : void
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearSize extends FairyGUI.GearBase implements FairyGUI.ITweenListener
        {
            public OnTweenStart ($tweener: FairyGUI.GTweener) : void
            public OnTweenUpdate ($tweener: FairyGUI.GTweener) : void
            public OnTweenComplete ($tweener: FairyGUI.GTweener) : void
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearText extends FairyGUI.GearBase
        {
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GearXY extends FairyGUI.GearBase implements FairyGUI.ITweenListener
        {
            public positionsInPercent : boolean
            public AddExtStatus ($pageId: string, $buffer: FairyGUI.Utils.ByteBuffer) : void
            public OnTweenStart ($tweener: FairyGUI.GTweener) : void
            public OnTweenUpdate ($tweener: FairyGUI.GTweener) : void
            public OnTweenComplete ($tweener: FairyGUI.GTweener) : void
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        class GGraph extends FairyGUI.GObject implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get shape(): FairyGUI.Shape;
            public ReplaceMe ($target: FairyGUI.GObject) : void
            public AddBeforeMe ($target: FairyGUI.GObject) : void
            public AddAfterMe ($target: FairyGUI.GObject) : void
            public SetNativeObject ($obj: FairyGUI.DisplayObject) : void
            public DrawRect ($aWidth: number, $aHeight: number, $lineSize: number, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color) : void
            public DrawRoundRect ($aWidth: number, $aHeight: number, $fillColor: UnityEngine.Color, $corner: System.Array$1<number>) : void
            public DrawEllipse ($aWidth: number, $aHeight: number, $fillColor: UnityEngine.Color) : void
            public DrawPolygon ($aWidth: number, $aHeight: number, $points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color) : void
            public DrawPolygon ($aWidth: number, $aHeight: number, $points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color, $lineSize: number, $lineColor: UnityEngine.Color) : void
            public constructor ()
        }
        class GImage extends FairyGUI.GObject implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get flip(): FairyGUI.FlipType;
            public set flip(value: FairyGUI.FlipType);
            public get fillMethod(): FairyGUI.FillMethod;
            public set fillMethod(value: FairyGUI.FillMethod);
            public get fillOrigin(): number;
            public set fillOrigin(value: number);
            public get fillClockwise(): boolean;
            public set fillClockwise(value: boolean);
            public get fillAmount(): number;
            public set fillAmount(value: number);
            public get texture(): FairyGUI.NTexture;
            public set texture(value: FairyGUI.NTexture);
            public get material(): UnityEngine.Material;
            public set material(value: UnityEngine.Material);
            public get shader(): string;
            public set shader(value: string);
            public constructor ()
        }
        enum ListSelectionMode
        { Single = 0, Multiple = 1, Multiple_SingleClick = 2, None = 3 }
        interface ListItemRenderer
        { (index: number, item: FairyGUI.GObject) : void; }
        var ListItemRenderer: { new (func: (index: number, item: FairyGUI.GObject) => void): ListItemRenderer; }
        interface ListItemProvider
        { (index: number) : string; }
        var ListItemProvider: { new (func: (index: number) => string): ListItemProvider; }
        enum ListLayoutType
        { SingleColumn = 0, SingleRow = 1, FlowHorizontal = 2, FlowVertical = 3, Pagination = 4 }
        class GObjectPool extends System.Object
        {
            public initCallback : FairyGUI.GObjectPool.InitCallbackDelegate
            public get count(): number;
            public Clear () : void
            public GetObject ($url: string) : FairyGUI.GObject
            public ReturnObject ($obj: FairyGUI.GObject) : void
            public constructor ($manager: UnityEngine.Transform)
            public constructor ()
        }
        class GMovieClip extends FairyGUI.GObject implements FairyGUI.IEventDispatcher, FairyGUI.IAnimationGear, FairyGUI.IColorGear
        {
            public get onPlayEnd(): FairyGUI.EventListener;
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get flip(): FairyGUI.FlipType;
            public set flip(value: FairyGUI.FlipType);
            public get material(): UnityEngine.Material;
            public set material(value: UnityEngine.Material);
            public get shader(): string;
            public set shader(value: string);
            public get timeScale(): number;
            public set timeScale(value: number);
            public get ignoreEngineTimeScale(): boolean;
            public set ignoreEngineTimeScale(value: boolean);
            public Rewind () : void
            public SyncStatus ($anotherMc: FairyGUI.GMovieClip) : void
            public Advance ($time: number) : void
            public SetPlaySettings ($start: number, $end: number, $times: number, $endAt: number) : void
            public constructor ()
        }
        class Relations extends System.Object
        {
            public handling : FairyGUI.GObject
            public get isEmpty(): boolean;
            public Add ($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType) : void
            public Add ($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType, $usePercent: boolean) : void
            public Remove ($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType) : void
            public Contains ($target: FairyGUI.GObject) : boolean
            public ClearFor ($target: FairyGUI.GObject) : void
            public ClearAll () : void
            public CopyFrom ($source: FairyGUI.Relations) : void
            public Dispose () : void
            public OnOwnerSizeChanged ($dWidth: number, $dHeight: number, $applyPivot: boolean) : void
            public Setup ($buffer: FairyGUI.Utils.ByteBuffer, $parentToChild: boolean) : void
            public constructor ($owner: FairyGUI.GObject)
            public constructor ()
        }
        enum RelationType
        { Left_Left = 0, Left_Center = 1, Left_Right = 2, Center_Center = 3, Right_Left = 4, Right_Center = 5, Right_Right = 6, Top_Top = 7, Top_Middle = 8, Top_Bottom = 9, Middle_Middle = 10, Bottom_Top = 11, Bottom_Middle = 12, Bottom_Bottom = 13, Width = 14, Height = 15, LeftExt_Left = 16, LeftExt_Right = 17, RightExt_Left = 18, RightExt_Right = 19, TopExt_Top = 20, TopExt_Bottom = 21, BottomExt_Top = 22, BottomExt_Bottom = 23, Size = 24 }
        class GProgressBar extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public get titleType(): FairyGUI.ProgressTitleType;
            public set titleType(value: FairyGUI.ProgressTitleType);
            public get min(): number;
            public set min(value: number);
            public get max(): number;
            public set max(value: number);
            public get value(): number;
            public set value(value: number);
            public get reverse(): boolean;
            public set reverse(value: boolean);
            public TweenValue ($value: number, $duration: number) : FairyGUI.GTweener
            public Update ($newValue: number) : void
            public constructor ()
        }
        class GSlider extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public changeOnClick : boolean
            public canDrag : boolean
            public get onChanged(): FairyGUI.EventListener;
            public get onGripTouchEnd(): FairyGUI.EventListener;
            public get titleType(): FairyGUI.ProgressTitleType;
            public set titleType(value: FairyGUI.ProgressTitleType);
            public get min(): number;
            public set min(value: number);
            public get max(): number;
            public set max(value: number);
            public get value(): number;
            public set value(value: number);
            public get wholeNumbers(): boolean;
            public set wholeNumbers(value: boolean);
            public constructor ()
        }
        class GRichTextField extends FairyGUI.GTextField implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear, FairyGUI.ITextColorGear
        {
            public get richTextField(): FairyGUI.RichTextField;
            public get emojies(): System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>;
            public set emojies(value: System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>);
            public constructor ()
        }
        class GTextInput extends FairyGUI.GTextField implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear, FairyGUI.ITextColorGear
        {
            public get inputTextField(): FairyGUI.InputTextField;
            public get onChanged(): FairyGUI.EventListener;
            public get onSubmit(): FairyGUI.EventListener;
            public get editable(): boolean;
            public set editable(value: boolean);
            public get hideInput(): boolean;
            public set hideInput(value: boolean);
            public get maxLength(): number;
            public set maxLength(value: number);
            public get restrict(): string;
            public set restrict(value: string);
            public get displayAsPassword(): boolean;
            public set displayAsPassword(value: boolean);
            public get caretPosition(): number;
            public set caretPosition(value: number);
            public get promptText(): string;
            public set promptText(value: string);
            public get keyboardInput(): boolean;
            public set keyboardInput(value: boolean);
            public get keyboardType(): number;
            public set keyboardType(value: number);
            public get disableIME(): boolean;
            public set disableIME(value: boolean);
            public get emojies(): System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>;
            public set emojies(value: System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>);
            public get border(): number;
            public set border(value: number);
            public get corner(): number;
            public set corner(value: number);
            public get borderColor(): UnityEngine.Color;
            public set borderColor(value: UnityEngine.Color);
            public get backgroundColor(): UnityEngine.Color;
            public set backgroundColor(value: UnityEngine.Color);
            public get mouseWheelEnabled(): boolean;
            public set mouseWheelEnabled(value: boolean);
            public SetSelection ($start: number, $length: number) : void
            public ReplaceSelection ($value: string) : void
            public constructor ()
        }
        enum ProgressTitleType
        { Percent = 0, ValueAndMax = 1, Value = 2, Max = 3 }
        class GScrollBar extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public get minSize(): number;
            public get gripDragging(): boolean;
            public SetScrollPane ($target: FairyGUI.ScrollPane, $vertical: boolean) : void
            public SetDisplayPerc ($value: number) : void
            public setScrollPerc ($value: number) : void
            public constructor ()
        }
        interface IUISource
        {
            fileName : string
            loaded : boolean
            Load ($callback: FairyGUI.UILoadCallback) : void
            Cancel () : void
        }
        interface UILoadCallback
        { () : void; }
        var UILoadCallback: { new (func: () => void): UILoadCallback; }
        class UIPackage extends System.Object
        {
            public static unloadBundleByFGUI : boolean
            public static URL_PREFIX : string
            public get id(): string;
            public get name(): string;
            public static get branch(): string;
            public static set branch(value: string);
            public get assetPath(): string;
            public get customId(): string;
            public set customId(value: string);
            public get resBundle(): UnityEngine.AssetBundle;
            public get dependencies(): System.Array$1<System.Collections.Generic.Dictionary$2<string, string>>;
            public static add_onReleaseResource ($value: System.Action$1<FairyGUI.PackageItem>) : void
            public static remove_onReleaseResource ($value: System.Action$1<FairyGUI.PackageItem>) : void
            public static GetVar ($key: string) : string
            public static SetVar ($key: string, $value: string) : void
            public static GetById ($id: string) : FairyGUI.UIPackage
            public static GetByName ($name: string) : FairyGUI.UIPackage
            public static AddPackage ($bundle: UnityEngine.AssetBundle) : FairyGUI.UIPackage
            public static AddPackage ($desc: UnityEngine.AssetBundle, $res: UnityEngine.AssetBundle) : FairyGUI.UIPackage
            public static AddPackage ($desc: UnityEngine.AssetBundle, $res: UnityEngine.AssetBundle, $mainAssetName: string) : FairyGUI.UIPackage
            public static AddPackage ($descFilePath: string) : FairyGUI.UIPackage
            public static AddPackage ($assetPath: string, $loadFunc: FairyGUI.UIPackage.LoadResource) : FairyGUI.UIPackage
            public static AddPackage ($descData: System.Array$1<number>, $assetNamePrefix: string, $loadFunc: FairyGUI.UIPackage.LoadResource) : FairyGUI.UIPackage
            public static AddPackage ($descData: System.Array$1<number>, $assetNamePrefix: string, $loadFunc: FairyGUI.UIPackage.LoadResourceAsync) : FairyGUI.UIPackage
            public static RemovePackage ($packageIdOrName: string) : void
            public static RemoveAllPackages () : void
            public static GetPackages () : System.Collections.Generic.List$1<FairyGUI.UIPackage>
            public static CreateObject ($pkgName: string, $resName: string) : FairyGUI.GObject
            public static CreateObject ($pkgName: string, $resName: string, $userClass: System.Type) : FairyGUI.GObject
            public static CreateObjectFromURL ($url: string) : FairyGUI.GObject
            public static CreateObjectFromURL ($url: string, $userClass: System.Type) : FairyGUI.GObject
            public static CreateObjectAsync ($pkgName: string, $resName: string, $callback: FairyGUI.UIPackage.CreateObjectCallback) : void
            public static CreateObjectFromURL ($url: string, $callback: FairyGUI.UIPackage.CreateObjectCallback) : void
            public static GetItemAsset ($pkgName: string, $resName: string) : any
            public static GetItemAssetByURL ($url: string) : any
            public static GetItemURL ($pkgName: string, $resName: string) : string
            public static GetItemByURL ($url: string) : FairyGUI.PackageItem
            public static NormalizeURL ($url: string) : string
            public static SetStringsSource ($source: FairyGUI.Utils.XML) : void
            public LoadAllAssets () : void
            public UnloadAssets () : void
            public ReloadAssets () : void
            public ReloadAssets ($resBundle: UnityEngine.AssetBundle) : void
            public CreateObject ($resName: string) : FairyGUI.GObject
            public CreateObject ($resName: string, $userClass: System.Type) : FairyGUI.GObject
            public CreateObjectAsync ($resName: string, $callback: FairyGUI.UIPackage.CreateObjectCallback) : void
            public GetItemAsset ($resName: string) : any
            public GetItems () : System.Collections.Generic.List$1<FairyGUI.PackageItem>
            public GetItem ($itemId: string) : FairyGUI.PackageItem
            public GetItemByName ($itemName: string) : FairyGUI.PackageItem
            public GetItemAsset ($item: FairyGUI.PackageItem) : any
            public SetItemAsset ($item: FairyGUI.PackageItem, $asset: any, $destroyMethod: FairyGUI.DestroyMethod) : void
            public constructor ()
        }
        enum PackageItemType
        { Image = 0, MovieClip = 1, Sound = 2, Component = 3, Atlas = 4, Font = 5, Swf = 6, Misc = 7, Unknown = 8, Spine = 9, DragoneBones = 10 }
        enum ObjectType
        { Image = 0, MovieClip = 1, Swf = 2, Graph = 3, Loader = 4, Group = 5, Text = 6, RichText = 7, InputText = 8, Component = 9, List = 10, Label = 11, Button = 12, ComboBox = 13, ProgressBar = 14, Slider = 15, ScrollBar = 16, Tree = 17, Loader3D = 18 }
        interface PlayCompleteCallback
        { () : void; }
        var PlayCompleteCallback: { new (func: () => void): PlayCompleteCallback; }
        interface TransitionHook
        { () : void; }
        var TransitionHook: { new (func: () => void): TransitionHook; }
        class TranslationHelper extends System.Object
        {
            public static strings : System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, string>>
            public static LoadFromXML ($source: FairyGUI.Utils.XML) : void
            public static TranslateComponent ($item: FairyGUI.PackageItem) : void
            public constructor ()
        }
        class TreeNode extends System.Object
        {
            public data : any
            public get parent(): FairyGUI.TreeNode;
            public get tree(): FairyGUI.TreeView;
            public get cell(): FairyGUI.GComponent;
            public get level(): number;
            public get expanded(): boolean;
            public set expanded(value: boolean);
            public get isFolder(): boolean;
            public get text(): string;
            public get numChildren(): number;
            public AddChild ($child: FairyGUI.TreeNode) : FairyGUI.TreeNode
            public AddChildAt ($child: FairyGUI.TreeNode, $index: number) : FairyGUI.TreeNode
            public RemoveChild ($child: FairyGUI.TreeNode) : FairyGUI.TreeNode
            public RemoveChildAt ($index: number) : FairyGUI.TreeNode
            public RemoveChildren ($beginIndex?: number, $endIndex?: number) : void
            public GetChildAt ($index: number) : FairyGUI.TreeNode
            public GetChildIndex ($child: FairyGUI.TreeNode) : number
            public GetPrevSibling () : FairyGUI.TreeNode
            public GetNextSibling () : FairyGUI.TreeNode
            public SetChildIndex ($child: FairyGUI.TreeNode, $index: number) : void
            public SwapChildren ($child1: FairyGUI.TreeNode, $child2: FairyGUI.TreeNode) : void
            public SwapChildrenAt ($index1: number, $index2: number) : void
            public constructor ($hasChild: boolean)
            public constructor ()
        }
        class TreeView extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public indent : number
            public treeNodeCreateCell : FairyGUI.TreeView.TreeNodeCreateCellDelegate
            public treeNodeRender : FairyGUI.TreeView.TreeNodeRenderDelegate
            public treeNodeWillExpand : FairyGUI.TreeView.TreeNodeWillExpandDelegate
            public get list(): FairyGUI.GList;
            public get root(): FairyGUI.TreeNode;
            public get onClickNode(): FairyGUI.EventListener;
            public get onRightClickNode(): FairyGUI.EventListener;
            public GetSelectedNode () : FairyGUI.TreeNode
            public GetSelection () : System.Collections.Generic.List$1<FairyGUI.TreeNode>
            public AddSelection ($node: FairyGUI.TreeNode, $scrollItToView?: boolean) : void
            public RemoveSelection ($node: FairyGUI.TreeNode) : void
            public ClearSelection () : void
            public GetNodeIndex ($node: FairyGUI.TreeNode) : number
            public UpdateNode ($node: FairyGUI.TreeNode) : void
            public UpdateNodes ($nodes: System.Collections.Generic.List$1<FairyGUI.TreeNode>) : void
            public ExpandAll ($folderNode: FairyGUI.TreeNode) : void
            public CollapseAll ($folderNode: FairyGUI.TreeNode) : void
            public constructor ($list: FairyGUI.GList)
            public constructor ()
        }
        class UIConfig extends UnityEngine.MonoBehaviour
        {
            public static defaultFont : string
            public static windowModalWaiting : string
            public static globalModalWaiting : string
            public static modalLayerColor : UnityEngine.Color
            public static buttonSound : FairyGUI.NAudioClip
            public static buttonSoundVolumeScale : number
            public static horizontalScrollBar : string
            public static verticalScrollBar : string
            public static defaultScrollStep : number
            public static defaultScrollDecelerationRate : number
            public static defaultScrollBarDisplay : FairyGUI.ScrollBarDisplayType
            public static defaultScrollTouchEffect : boolean
            public static defaultScrollBounceEffect : boolean
            public static defaultScrollSnappingThreshold : number
            public static defaultScrollPagingThreshold : number
            public static popupMenu : string
            public static popupMenu_seperator : string
            public static loaderErrorSign : string
            public static tooltipsWin : string
            public static defaultComboBoxVisibleItemCount : number
            public static touchScrollSensitivity : number
            public static touchDragSensitivity : number
            public static clickDragSensitivity : number
            public static allowSoftnessOnTopOrLeftSide : boolean
            public static bringWindowToFrontOnClick : boolean
            public static inputCaretSize : number
            public static inputHighlightColor : UnityEngine.Color
            public static frameTimeForAsyncUIConstruction : number
            public static depthSupportForPaintingMode : boolean
            public static enhancedTextOutlineEffect : boolean
            public static makePixelPerfect : boolean
            public Items : System.Collections.Generic.List$1<FairyGUI.UIConfig.ConfigValue>
            public PreloadPackages : System.Collections.Generic.List$1<string>
            public static soundLoader : FairyGUI.UIConfig.SoundLoader
            public Load () : void
            public static SetDefaultValue ($key: FairyGUI.UIConfig.ConfigKey, $value: FairyGUI.UIConfig.ConfigValue) : void
            public static ClearResourceRefs () : void
            public ApplyModifiedProperties () : void
            public constructor ()
        }
        enum ScrollBarDisplayType
        { Default = 0, Visible = 1, Auto = 2, Hidden = 3 }
        class UIContentScaler extends UnityEngine.MonoBehaviour
        {
            public scaleMode : FairyGUI.UIContentScaler.ScaleMode
            public screenMatchMode : FairyGUI.UIContentScaler.ScreenMatchMode
            public designResolutionX : number
            public designResolutionY : number
            public fallbackScreenDPI : number
            public defaultSpriteDPI : number
            public constantScaleFactor : number
            public ignoreOrientation : boolean
            public static scaleFactor : number
            public static scaleLevel : number
            public ApplyModifiedProperties () : void
            public ApplyChange () : void
            public constructor ()
        }
        class UIObjectFactory extends System.Object
        {
            public static SetPackageItemExtension ($url: string, $type: System.Type) : void
            public static SetPackageItemExtension ($url: string, $creator: FairyGUI.UIObjectFactory.GComponentCreator) : void
            public static SetLoaderExtension ($type: System.Type) : void
            public static SetLoaderExtension ($creator: FairyGUI.UIObjectFactory.GLoaderCreator) : void
            public static Clear () : void
            public static NewObject ($pi: FairyGUI.PackageItem, $userClass?: System.Type) : FairyGUI.GObject
            public static NewObject ($type: FairyGUI.ObjectType) : FairyGUI.GObject
            public constructor ()
        }
        class UIPainter extends UnityEngine.MonoBehaviour implements FairyGUI.EMRenderTarget
        {
            public packageName : string
            public componentName : string
            public sortingOrder : number
            public get container(): FairyGUI.Container;
            public get ui(): FairyGUI.GComponent;
            public get EM_sortingOrder(): number;
            public SetSortingOrder ($value: number, $apply: boolean) : void
            public CreateUI () : void
            public ApplyModifiedProperties ($sortingOrderChanged: boolean) : void
            public OnUpdateSource ($data: System.Array$1<any>) : void
            public EM_BeforeUpdate () : void
            public EM_Update ($context: FairyGUI.UpdateContext) : void
            public EM_Reload () : void
            public constructor ()
        }
        class UIPanel extends UnityEngine.MonoBehaviour implements FairyGUI.EMRenderTarget
        {
            public packageName : string
            public componentName : string
            public fitScreen : FairyGUI.FitScreen
            public sortingOrder : number
            public get container(): FairyGUI.Container;
            public get ui(): FairyGUI.GComponent;
            public get EM_sortingOrder(): number;
            public CreateUI () : void
            public SetSortingOrder ($value: number, $apply: boolean) : void
            public SetHitTestMode ($value: FairyGUI.HitTestMode) : void
            public CacheNativeChildrenRenderers () : void
            public ApplyModifiedProperties ($sortingOrderChanged: boolean, $fitScreenChanged: boolean) : void
            public MoveUI ($delta: UnityEngine.Vector3) : void
            public GetUIWorldPosition () : UnityEngine.Vector3
            public EM_BeforeUpdate () : void
            public EM_Update ($context: FairyGUI.UpdateContext) : void
            public EM_Reload () : void
            public constructor ()
        }
        enum FitScreen
        { None = 0, FitSize = 1, FitWidthAndSetMiddle = 2, FitHeightAndSetCenter = 3 }
        enum HitTestMode
        { Default = 0, Raycast = 1 }
        class Timers extends System.Object
        {
            public static repeat : number
            public static time : number
            public static catchCallbackExceptions : boolean
            public static get inst(): FairyGUI.Timers;
            public Add ($interval: number, $repeat: number, $callback: FairyGUI.TimerCallback) : void
            public Add ($interval: number, $repeat: number, $callback: FairyGUI.TimerCallback, $callbackParam: any) : void
            public CallLater ($callback: FairyGUI.TimerCallback) : void
            public CallLater ($callback: FairyGUI.TimerCallback, $callbackParam: any) : void
            public AddUpdate ($callback: FairyGUI.TimerCallback) : void
            public AddUpdate ($callback: FairyGUI.TimerCallback, $callbackParam: any) : void
            public StartCoroutine ($routine: System.Collections.IEnumerator) : void
            public Exists ($callback: FairyGUI.TimerCallback) : boolean
            public Remove ($callback: FairyGUI.TimerCallback) : void
            public Update () : void
            public constructor ()
        }
        interface TimerCallback
        { (param: any) : void; }
        var TimerCallback: { new (func: (param: any) => void): TimerCallback; }
    }
    namespace FairyEditor {
        class App extends System.Object
        {
            public static isMacOS : boolean
            public static language : string
            public static batchMode : boolean
            public static preferences : FairyEditor.Preferences
            public static localStore : FairyEditor.LocalStore
            public static hotkeyManager : FairyEditor.HotkeyManager
            public static externalImagePool : ExternalImagePool
            public static get groot(): FairyGUI.GRoot;
            public static get project(): FairyEditor.FProject;
            public static get workspaceSettings(): FairyEditor.WorkspaceSettings;
            public static get mainView(): FairyEditor.View.MainView;
            public static get docView(): FairyEditor.View.DocumentView;
            public static get libView(): FairyEditor.View.LibraryView;
            public static get inspectorView(): FairyEditor.View.InspectorView;
            public static get testView(): FairyEditor.View.TestView;
            public static get timelineView(): FairyEditor.View.TimelineView;
            public static get consoleView(): FairyEditor.View.ConsoleView;
            public static get menu(): FairyEditor.Component.IMenu;
            public static get viewManager(): FairyEditor.ViewManager;
            public static get dragManager(): FairyEditor.DragDropManager;
            public static get pluginManager(): FairyEditor.PluginManager;
            public static get docFactory(): FairyEditor.View.DocumentFactory;
            public static get activeDoc(): FairyEditor.View.Document;
            public static get preferenceFolder(): string;
            public static get isActive(): boolean;
            public static add_onProjectOpened ($value: System.Action) : void
            public static remove_onProjectOpened ($value: System.Action) : void
            public static add_onProjectClosed ($value: System.Action) : void
            public static remove_onProjectClosed ($value: System.Action) : void
            public static add_onUpdate ($value: System.Action) : void
            public static remove_onUpdate ($value: System.Action) : void
            public static add_onLateUpdate ($value: System.Action) : void
            public static remove_onLateUpdate ($value: System.Action) : void
            public static add_onValidate ($value: System.Action) : void
            public static remove_onValidate ($value: System.Action) : void
            public static GetString ($index: number) : string
            public static GetString ($index: string) : string
            public static GetIcon ($key: string) : string
            public static GetIcon ($key: string, $big: boolean) : string
            public static StartBackgroundJob () : void
            public static EndBackgroundJob () : void
            public static SetFrameRateFactor ($factor: FairyEditor.App.FrameRateFactor, $enabled: boolean) : void
            public static OpenProject ($path: string) : void
            public static CloseProject () : void
            public static RefreshProject () : void
            public static ShowPreview ($pi: FairyEditor.FPackageItem) : void
            public static FindReference ($source: string) : void
            public static GetActiveFolder () : FairyEditor.FPackageItem
            public static QueryToClose ($restart: boolean) : void
            public static Close () : void
            public static Alert ($msg: string) : void
            public static Alert ($msg: string, $err: System.Exception) : void
            public static Alert ($msg: string, $err: System.Exception, $callback: System.Action) : void
            public static Confirm ($msg: string, $callback: System.Action$1<string>) : void
            public static Input ($msg: string, $text: string, $callback: System.Action$1<string>) : void
            public static SetWaitCursor ($value: boolean) : void
            public static ShowWaiting () : void
            public static ShowWaiting ($msg: string) : void
            public static ShowWaiting ($msg: string, $cancelCallback: System.Action) : void
            public static CloseWaiting () : void
            public static SetVar ($key: string, $value: any) : void
            public static On ($eventType: string, $callback: FairyGUI.EventCallback1) : void
            public static Off ($eventType: string, $callback: FairyGUI.EventCallback1) : void
            public static Dispatch ($eventType: string, $eventData?: any) : void
            public static ChangeColorSapce ($colorSpace: UnityEngine.ColorSpace) : void
            public constructor ()
        }
        class Preferences extends System.Object
        {
            public language : string
            public checkNewVersion : string
            public genComPreview : boolean
            public meaningfullChildName : boolean
            public hideInvisibleChild : boolean
            public publishAction : string
            public saveBeforePublish : boolean
            public PNGCompressionToolPath : string
            public editorFont : string
            public hotkeys : System.Collections.Generic.Dictionary$2<string, string>
            public Load () : void
            public Save () : void
            public constructor ()
        }
        class LocalStore extends System.Object
        {
            public Set ($key: string, $value: any) : void
            public Load () : void
            public Save () : void
            public constructor ()
        }
        class HotkeyManager extends System.Object
        {
            public get functions(): System.Collections.Generic.List$1<FairyEditor.HotkeyManager.FunctionDef>;
            public Init () : void
            public SetHotkey ($funcId: string, $hotkey: string) : void
            public ResetHotkey ($funcId: string) : void
            public ResetAll () : void
            public CaptureHotkey ($receiver: FairyGUI.GObject) : void
            public GetFunctionDef ($funcId: string) : FairyEditor.HotkeyManager.FunctionDef
            public GetFunction ($evt: FairyGUI.InputEvent, $code: $Ref<number>) : string
            public TranslateKey ($hotkey: string) : number
            public constructor ()
        }
        class FProject extends System.Object
        {
            public isMain : boolean
            public _globalFontVersion : number
            public static FILE_EXT : string
            public static ASSETS_PATH : string
            public static SETTINGS_PATH : string
            public static OBJS_PATH : string
            public get versionCode(): number;
            public get serialNumberSeed(): string;
            public get lastChanged(): number;
            public get opened(): boolean;
            public get id(): string;
            public get name(): string;
            public get type(): string;
            public set type(value: string);
            public get supportAtlas(): boolean;
            public get isH5(): boolean;
            public get supportExtractAlpha(): boolean;
            public get supportAlphaMask(): boolean;
            public get zipFormatOption(): boolean;
            public get binaryFormatOption(): boolean;
            public get supportCustomFileExtension(): boolean;
            public get basePath(): string;
            public get assetsPath(): string;
            public get objsPath(): string;
            public get settingsPath(): string;
            public get activeBranch(): string;
            public set activeBranch(value: string);
            public get allPackages(): System.Collections.Generic.List$1<FairyEditor.FPackage>;
            public get allBranches(): System.Collections.Generic.List$1<string>;
            public SetChanged () : void
            public static CreateNew ($projectPath: string, $name: string, $type: string, $pkgName?: string) : void
            public Open ($projectDescFile: string) : void
            public ScanBranches () : boolean
            public Dispose () : void
            public GetSettings ($name: string) : FairyEditor.SettingsBase
            public LoadAllSettings () : void
            public GetDefaultFont () : string
            public Rename ($newName: string) : void
            public GetPackage ($packageId: string) : FairyEditor.FPackage
            public GetPackageByName ($packageName: string) : FairyEditor.FPackage
            public CreatePackage ($newName: string) : FairyEditor.FPackage
            public AddPackage ($folder: string) : FairyEditor.FPackage
            public DeletePackage ($packageId: string) : void
            public Save () : void
            public GetItemByURL ($url: string) : FairyEditor.FPackageItem
            public GetItem ($pkgId: string, $itemId: string) : FairyEditor.FPackageItem
            public FindItemByFile ($file: string) : FairyEditor.FPackageItem
            public GetItemNameByURL ($url: string) : string
            public CreateBranch ($branchName: string) : void
            public RenameBranch ($oldName: string, $newName: string) : void
            public RemoveBranch ($branchName: string) : void
            public RegisterComExtension ($name: string, $className: string, $superClassName: string) : void
            public GetComExtension ($className: string) : FairyEditor.ComExtensionDef
            public GetComExtensionNames () : System.Collections.Generic.List$1<string>
            public ClearComExtensions () : void
            public static ValidateName ($newName: string) : string
            public constructor ($main?: boolean)
            public constructor ()
        }
        class WorkspaceSettings extends System.Object
        {
            public Set ($key: string, $value: any) : void
            public Load () : void
            public Save () : void
            public constructor ()
        }
        class ViewManager extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public get playMode(): boolean;
            public set playMode(value: boolean);
            public get viewIds(): System.Collections.Generic.List$1<string>;
            public get lastFocusedView(): FairyGUI.GComponent;
            public AddView ($view: FairyGUI.GComponent, $viewId: string, $options: FairyEditor.ViewOptions) : FairyGUI.GComponent
            public RemoveView ($viewId: string) : void
            public GetView ($viewId: string) : FairyGUI.GComponent
            public IsViewShowing ($viewId: string) : boolean
            public SetViewTitle ($viewId: string, $title: string) : void
            public ShowView ($viewId: string) : void
            public HideView ($viewId: string) : void
            public LoadLayout () : void
            public ResetLayout () : void
            public SaveLayout () : void
            public ShowTabMenu ($view: FairyGUI.GComponent) : void
            public OnDragGridStart ($grid: FairyEditor.Component.ViewGrid, $tabButton: FairyGUI.GObject) : void
            public constructor ()
        }
        class DragDropManager extends System.Object
        {
            public get agent(): FairyGUI.GObject;
            public get dragging(): boolean;
            public StartDrag ($source?: FairyGUI.GObject, $sourceData?: any, $icon?: any, $cursor?: string, $onComplete?: System.Action$2<FairyGUI.GObject, any>, $onCancel?: System.Action$2<FairyGUI.GObject, any>, $onMove?: System.Action$3<FairyGUI.GObject, any, FairyGUI.EventContext>) : void
            public Cancel () : void
            public constructor ()
        }
        class PluginManager extends System.Object
        {
            public allPlugins : System.Collections.Generic.List$1<FairyEditor.PluginManager.PluginInfo>
            public get userPluginFolder(): string;
            public get projectPluginFolder(): string;
            public get basePath(): string;
            public Dispose () : void
            public Load () : void
            public LoadUIPackage ($filePath: string) : void
            public SetHotkey ($hotkey: string, $callback: System.Action) : void
            public HandleHotkey ($keyCode: number) : boolean
            public CreateNewPlugin ($name: string, $displayName: string, $icon: string, $desc: string, $template: string) : void
            public constructor ()
        }
        class FPackageItem extends System.Object
        {
            public exported : boolean
            public favorite : boolean
            public isError : boolean
            public get owner(): FairyEditor.FPackage;
            public get parent(): FairyEditor.FPackageItem;
            public get type(): string;
            public get id(): string;
            public set id(value: string);
            public get path(): string;
            public get branch(): string;
            public get isRoot(): boolean;
            public get isBranchRoot(): boolean;
            public get name(): string;
            public get file(): string;
            public get fileName(): string;
            public get modificationTime(): Date;
            public get sortKey(): string;
            public get version(): number;
            public get width(): number;
            public set width(value: number);
            public get height(): number;
            public set height(value: number);
            public get thumbnail(): FairyGUI.NTexture;
            public get children(): System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public get folderAtlas(): string;
            public set folderAtlas(value: string);
            public get supportAtlas(): boolean;
            public get supportResolution(): boolean;
            public get title(): string;
            public get contentHash(): string;
            public get isDisposed(): boolean;
            public add_onChanged ($value: System.Action$1<FairyEditor.FPackageItem>) : void
            public remove_onChanged ($value: System.Action$1<FairyEditor.FPackageItem>) : void
            public add_onAlternativeAdded ($value: System.Action$1<FairyEditor.FPackageItem>) : void
            public remove_onAlternativeAdded ($value: System.Action$1<FairyEditor.FPackageItem>) : void
            public MatchName ($key: string) : boolean
            public GetURL () : string
            public GetIcon ($opened?: boolean, $big?: boolean, $thumbnail?: boolean) : string
            public CopySettings ($source: FairyEditor.FPackageItem) : void
            public SetFile ($path: string, $fileName: string, $checkStatus?: boolean) : void
            public SetChanged () : void
            public Touch () : void
            public SetUptoDate () : void
            public FileExists () : boolean
            public GetAsset () : FairyEditor.AssetBase
            public ReadAssetSettings ($xml: FairyGUI.Utils.XML) : void
            public OpenWithDefaultApplication () : void
            public GetBranch ($branchName: string) : FairyEditor.FPackageItem
            public GetTrunk () : FairyEditor.FPackageItem
            public GetHighResolution ($scaleLevel: number) : FairyEditor.FPackageItem
            public GetStdResolution () : FairyEditor.FPackageItem
            public GetAtlasIndex () : number
            public SetVar ($key: string, $value: any) : void
            public AddRef () : void
            public ReleaseRef () : void
            public UnloadAsset ($timestamp?: number) : void
            public Dispose () : void
            public Serialize ($forPublish?: boolean) : FairyGUI.Utils.XML
            public constructor ($owner: FairyEditor.FPackage, $type: string, $id: string)
            public constructor ()
        }
        class Bootstrap extends UnityEngine.MonoBehaviour
        {
            public constructor ()
        }
        class LoaderExtension extends FairyGUI.GLoader implements FairyGUI.IEventDispatcher, FairyGUI.IAnimationGear, FairyGUI.IColorGear
        {
            public constructor ()
        }
        class AniSprite extends FairyGUI.Image implements FairyGUI.IEventDispatcher, FairyGUI.IMeshFactory
        {
            public get onPlayEnd(): FairyGUI.EventListener;
            public get animation(): FairyEditor.AniData;
            public set animation(value: FairyEditor.AniData);
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public get frameCount(): number;
            public Rewind () : void
            public Advance ($time: number) : void
            public SetPlaySettings () : void
            public SetPlaySettings ($start: number, $end: number, $times: number, $endAt: number) : void
            public StepNext () : void
            public StepPrev () : void
            public constructor ()
        }
        class AniData extends System.Object
        {
            public version : number
            public boundsRect : UnityEngine.Rect
            public fps : number
            public speed : number
            public repeatDelay : number
            public swing : boolean
            public frameList : System.Collections.Generic.List$1<FairyEditor.AniData.Frame>
            public spriteList : System.Collections.Generic.List$1<FairyEditor.AniData.FrameSprite>
            public static FILE_MARK : string
            public get frameCount(): number;
            public Load ($file: string) : void
            public Load ($ba: FairyGUI.Utils.ByteBuffer) : void
            public Save ($file: string) : void
            public Save () : System.Array$1<number>
            public CalculateBoundsRect () : void
            public CopySettings ($source: FairyEditor.AniData) : void
            public CopyFrom ($source: FairyEditor.AniData) : void
            public Reset ($ownsTexture?: boolean) : void
            public ImportImages ($images: System.Collections.Generic.IList$1<string>, $CompressPng: boolean) : void
            public constructor ()
        }
        class AniAsset extends FairyEditor.AssetBase
        {
            public smoothing : boolean
            public atlas : string
            public get animation(): FairyEditor.AniData;
            public Load () : System.Threading.Tasks.Task
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class AssetBase extends System.Object
        {
            public get isLoading(): boolean;
            public get isLoaded(): boolean;
            public ReadSettings ($xml: FairyGUI.Utils.XML) : void
            public WriteSettings ($xml: FairyGUI.Utils.XML, $forPublish: boolean) : void
            public LoadMeta () : void
            public Unload () : void
            public Dispose () : void
            public GetThumbnail () : FairyGUI.NTexture
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class BmFontData extends System.Object
        {
            public face : string
            public xadvance : number
            public canTint : boolean
            public resizable : boolean
            public fontSize : number
            public lineHeight : number
            public atlasFile : string
            public pages : number
            public hasChannel : boolean
            public baseline : number
            public packed : number
            public alphaChnl : number
            public redChnl : number
            public greenChnl : number
            public blueChnl : number
            public glyphs : System.Collections.Generic.List$1<FairyEditor.BmFontData.Glyph>
            public Load ($content: string, $lazyLoadChars?: boolean) : void
            public LoadChars () : void
            public Build () : string
            public constructor ()
        }
        class ComponentAsset extends FairyEditor.AssetBase
        {
            public get extension(): string;
            public get xml(): FairyGUI.Utils.XML;
            public get displayList(): System.Collections.Generic.List$1<FairyEditor.ComponentAsset.DisplayListItem>;
            public GetCustomProperties () : System.Collections.Generic.IList$1<FairyEditor.ComProperty>
            public GetControllerPages ($name: string, $pageNames: System.Collections.Generic.List$1<string>, $pageIds: System.Collections.Generic.List$1<string>) : void
            public CreateObject ($item: FairyEditor.FPackageItem, $flags?: number) : System.Threading.Tasks.Task$1<FairyEditor.FComponent>
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class ComProperty extends System.Object
        {
            public target : string
            public propertyId : number
            public label : string
            public value : any
            public CopyFrom ($source: FairyEditor.ComProperty) : void
            public constructor ()
        }
        class FComponent extends FairyEditor.FObject implements FairyGUI.IEventDispatcher
        {
            public customExtentionId : string
            public initName : string
            public designImage : string
            public designImageOffsetX : number
            public designImageOffsetY : number
            public designImageAlpha : number
            public designImageLayer : number
            public designImageForTest : boolean
            public bgColor : UnityEngine.Color
            public bgColorEnabled : boolean
            public hitTestSource : FairyEditor.FObject
            public mask : FairyEditor.FObject
            public reversedMask : boolean
            public remark : string
            public headerRes : string
            public footerRes : string
            public showSound : string
            public hideSound : string
            public get numChildren(): number;
            public get children(): System.Collections.Generic.List$1<FairyEditor.FObject>;
            public get controllers(): System.Collections.Generic.List$1<FairyEditor.FController>;
            public get transitions(): FairyEditor.FTransitions;
            public get customProperties(): System.Collections.Generic.List$1<FairyEditor.ComProperty>;
            public set customProperties(value: System.Collections.Generic.List$1<FairyEditor.ComProperty>);
            public get bounds(): UnityEngine.Rect;
            public get extention(): FairyEditor.ComExtention;
            public get extentionId(): string;
            public set extentionId(value: string);
            public get scrollPane(): FairyEditor.FScrollPane;
            public get overflow(): string;
            public set overflow(value: string);
            public get overflow2(): string;
            public set overflow2(value: string);
            public get scroll(): string;
            public set scroll(value: string);
            public get scrollBarFlags(): number;
            public set scrollBarFlags(value: number);
            public get scrollBarDisplay(): string;
            public set scrollBarDisplay(value: string);
            public get margin(): FairyEditor.FMargin;
            public get marginStr(): string;
            public set marginStr(value: string);
            public get scrollBarMargin(): FairyEditor.FMargin;
            public get scrollBarMarginStr(): string;
            public set scrollBarMarginStr(value: string);
            public get hzScrollBarRes(): string;
            public set hzScrollBarRes(value: string);
            public get vtScrollBarRes(): string;
            public set vtScrollBarRes(value: string);
            public get clipSoftnessX(): number;
            public set clipSoftnessX(value: number);
            public get clipSoftnessY(): number;
            public set clipSoftnessY(value: number);
            public get viewWidth(): number;
            public set viewWidth(value: number);
            public get viewHeight(): number;
            public set viewHeight(value: number);
            public get opaque(): boolean;
            public set opaque(value: boolean);
            public get text(): string;
            public set text(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get childrenRenderOrder(): string;
            public set childrenRenderOrder(value: string);
            public get apexIndex(): number;
            public set apexIndex(value: number);
            public get pageController(): string;
            public set pageController(value: string);
            public get pageControllerObj(): FairyEditor.FController;
            public get scriptData(): FairyGUI.Utils.XML;
            public AddChild ($child: FairyEditor.FObject) : FairyEditor.FObject
            public AddChildAt ($child: FairyEditor.FObject, $index: number) : FairyEditor.FObject
            public RemoveChild ($child: FairyEditor.FObject, $dispose?: boolean) : FairyEditor.FObject
            public RemoveChildAt ($index: number, $dispose?: boolean) : FairyEditor.FObject
            public RemoveChildren ($beginIndex?: number, $endIndex?: number, $dispose?: boolean) : void
            public GetChildAt ($index: number) : FairyEditor.FObject
            public GetChild ($name: string) : FairyEditor.FObject
            public GetChildByPath ($path: string) : FairyEditor.FObject
            public GetChildById ($id: string) : FairyEditor.FObject
            public GetChildIndex ($child: FairyEditor.FObject) : number
            public SetChildIndex ($child: FairyEditor.FObject, $index: number) : void
            public SwapChildren ($child1: FairyEditor.FObject, $child2: FairyEditor.FObject) : void
            public SwapChildrenAt ($index1: number, $index2: number) : void
            public AddController ($controller: FairyEditor.FController, $applyNow?: boolean) : void
            public GetController ($name: string) : FairyEditor.FController
            public RemoveController ($c: FairyEditor.FController) : void
            public UpdateChildrenVisible () : void
            public UpdateDisplayList ($immediatelly?: boolean) : void
            public GetSnappingPosition ($xValue: number, $yValue: number) : UnityEngine.Vector2
            public EnsureBoundsCorrect () : void
            public SetBoundsChangedFlag () : void
            public GetBounds () : UnityEngine.Rect
            public SetBounds ($ax: number, $ay: number, $aw: number, $ah: number) : void
            public ApplyController ($c: FairyEditor.FController) : void
            public ApplyAllControllers () : void
            public AdjustRadioGroupDepth ($obj: FairyEditor.FObject, $c: FairyEditor.FController) : void
            public GetCustomProperty ($target: string, $propertyId: number) : FairyEditor.ComProperty
            public ApplyCustomProperty ($cp: FairyEditor.ComProperty) : void
            public UpdateOverflow () : void
            public Write_editMode () : FairyGUI.Utils.XML
            public ValidateChildren ($checkOnly?: boolean) : boolean
            public CreateChild ($xml: FairyGUI.Utils.XML) : FairyEditor.FObject
            public GetChildrenInfo () : string
            public GetNextId () : string
            public IsIdInUse ($val: string) : boolean
            public ContainsComponent ($pi: FairyEditor.FPackageItem) : boolean
            public NotifyChildReplaced ($source: FairyEditor.FObject, $target: FairyEditor.FObject) : void
            public constructor ($flags: number)
        }
        class FObject extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public _parent : FairyEditor.FComponent
            public _id : string
            public _width : number
            public _height : number
            public _rawWidth : number
            public _rawHeight : number
            public _widthEnabled : boolean
            public _heightEnabled : boolean
            public _renderDepth : number
            public _outlineVersion : number
            public _opened : boolean
            public _group : FairyEditor.FGroup
            public _sizePercentInGroup : number
            public _gearLocked : boolean
            public _internalVisible : boolean
            public _hasSnapshot : boolean
            public _treeNode : FairyEditor.FTreeNode
            public _pivotFromSource : boolean
            public _pkg : FairyEditor.FPackage
            public _res : FairyEditor.ResourceRef
            public _objectType : string
            public _docElement : FairyEditor.View.DocElement
            public _flags : number
            public _underConstruct : boolean
            public sourceWidth : number
            public sourceHeight : number
            public initWidth : number
            public initHeight : number
            public customData : string
            public static loadingSnapshot : boolean
            public static MAX_GEAR_INDEX : number
            public get id(): string;
            public get name(): string;
            public set name(value: string);
            public get objectType(): string;
            public get pkg(): FairyEditor.FPackage;
            public get docElement(): FairyEditor.View.DocElement;
            public get touchable(): boolean;
            public set touchable(value: boolean);
            public get touchDisabled(): boolean;
            public get grayed(): boolean;
            public set grayed(value: boolean);
            public get enabled(): boolean;
            public set enabled(value: boolean);
            public get resourceURL(): string;
            public get x(): number;
            public set x(value: number);
            public get y(): number;
            public set y(value: number);
            public get xy(): UnityEngine.Vector2;
            public set xy(value: UnityEngine.Vector2);
            public get xMin(): number;
            public set xMin(value: number);
            public get xMax(): number;
            public set xMax(value: number);
            public get yMin(): number;
            public set yMin(value: number);
            public get yMax(): number;
            public set yMax(value: number);
            public get height(): number;
            public set height(value: number);
            public get width(): number;
            public set width(value: number);
            public get size(): UnityEngine.Vector2;
            public get minWidth(): number;
            public set minWidth(value: number);
            public get minHeight(): number;
            public set minHeight(value: number);
            public get maxWidth(): number;
            public set maxWidth(value: number);
            public get maxHeight(): number;
            public set maxHeight(value: number);
            public get actualWidth(): number;
            public get actualHeight(): number;
            public get scaleX(): number;
            public set scaleX(value: number);
            public get scaleY(): number;
            public set scaleY(value: number);
            public get aspectLocked(): boolean;
            public set aspectLocked(value: boolean);
            public get aspectRatio(): number;
            public get skewX(): number;
            public set skewX(value: number);
            public get skewY(): number;
            public set skewY(value: number);
            public get pivotX(): number;
            public set pivotX(value: number);
            public get pivotY(): number;
            public set pivotY(value: number);
            public get anchor(): boolean;
            public set anchor(value: boolean);
            public get locked(): boolean;
            public set locked(value: boolean);
            public get hideByEditor(): boolean;
            public set hideByEditor(value: boolean);
            public get useSourceSize(): boolean;
            public set useSourceSize(value: boolean);
            public get rotation(): number;
            public set rotation(value: number);
            public get alpha(): number;
            public set alpha(value: number);
            public get visible(): boolean;
            public set visible(value: boolean);
            public get internalVisible(): boolean;
            public get internalVisible2(): boolean;
            public get internalVisible3(): boolean;
            public get groupId(): string;
            public set groupId(value: string);
            public get tooltips(): string;
            public set tooltips(value: string);
            public get filterData(): FairyEditor.FilterData;
            public set filterData(value: FairyEditor.FilterData);
            public get filter(): string;
            public set filter(value: string);
            public get blendMode(): string;
            public set blendMode(value: string);
            public get relations(): FairyEditor.FRelations;
            public get displayObject(): FairyEditor.FDisplayObject;
            public get parent(): FairyEditor.FComponent;
            public get text(): string;
            public set text(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get errorStatus(): boolean;
            public set errorStatus(value: boolean);
            public get topmost(): FairyEditor.FComponent;
            public SetXY ($xv: number, $yv: number) : void
            public SetTopLeft ($xv: number, $yv: number) : void
            public SetSize ($wv: number, $hv: number, $ignorePivot?: boolean, $dontCheckLock?: boolean) : void
            public SetScale ($sx: number, $sy: number) : void
            public SetSkew ($xv: number, $yv: number) : void
            public SetPivot ($xv: number, $yv: number, $asAnchor: boolean) : void
            public InGroup ($group: FairyEditor.FGroup) : boolean
            public GetGear ($index: number, $createIfNull?: boolean) : FairyEditor.Framework.Gears.IGear
            public UpdateGear ($index: number) : void
            public UpdateGearFromRelations ($index: number, $dx: number, $dy: number) : void
            public SupportGear ($index: number) : boolean
            public ValidateGears () : void
            public HasGears () : boolean
            public CheckGearController ($index: number, $c: FairyEditor.FController) : boolean
            public CheckGearsController ($c: FairyEditor.FController) : boolean
            public AddDisplayLock () : number
            public ReleaseDisplayLock ($token: number) : void
            public CheckGearDisplay () : void
            public RemoveFromParent () : void
            public LocalToGlobal ($pt: UnityEngine.Vector2) : UnityEngine.Vector2
            public GlobalToLocal ($pt: UnityEngine.Vector2) : UnityEngine.Vector2
            public static cast ($obj: FairyGUI.DisplayObject) : FairyEditor.FObject
            public HandleXYChanged () : void
            public HandleSizeChanged () : void
            public HandleGrayedChanged () : void
            public HandleAlphaChanged () : void
            public HandleVisibleChanged () : void
            public HandleControllerChanged ($c: FairyEditor.FController) : void
            public GetProperty ($propName: string) : any
            public SetProperty ($propName: string, $value: any) : void
            public GetProp ($index: FairyEditor.ObjectPropID) : any
            public SetProp ($index: FairyEditor.ObjectPropID, $value: any) : void
            public IsObsolete () : boolean
            public Validate ($checkOnly?: boolean) : boolean
            public GetDetailString () : string
            public Create () : void
            public Dispose () : void
            public Recreate () : void
            public Read_beforeAdd ($xml: FairyGUI.Utils.XML, $strings: System.Collections.Generic.Dictionary$2<string, string>) : void
            public Read_afterAdd ($xml: FairyGUI.Utils.XML, $strings: System.Collections.Generic.Dictionary$2<string, string>) : void
            public Write () : FairyGUI.Utils.XML
            public ReadGears ($xml: FairyGUI.Utils.XML) : void
            public WriteGears ($xml?: FairyGUI.Utils.XML) : FairyGUI.Utils.XML
            public TakeSnapshot ($ss: FairyEditor.ObjectSnapshot) : void
            public ReadSnapshot ($ss: FairyEditor.ObjectSnapshot) : void
            public constructor ($flags: number)
            public constructor ()
        }
        class DragonBonesAsset extends FairyEditor.SkeletonAsset
        {
            public static ParseBounds ($sourceFile: string) : UnityEngine.Rect
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class SkeletonAsset extends FairyEditor.AssetBase
        {
            public files : System.Array$1<string>
            public atlasNames : System.Array$1<string>
            public anchorX : number
            public anchorY : number
            public shader : string
            public pma : boolean
            public get data(): FairyEditor.ISkeletonDataAsset;
            public get animations(): System.Collections.Generic.List$1<string>;
            public get skins(): System.Collections.Generic.List$1<string>;
            public Load () : System.Threading.Tasks.Task
        }
        class FBitmapFont extends FairyGUI.BitmapFont
        {
            public get fontData(): FairyEditor.BmFontData;
            public get usingAtlas(): boolean;
            public get branch(): string;
            public GetSubFont ($branch: string, $scaleLevel: number) : FairyEditor.FBitmapFont
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($parent: FairyEditor.FBitmapFont, $branch: string, $scaleLevel: number)
            public constructor ()
        }
        class FontAsset extends FairyEditor.AssetBase
        {
            public texture : string
            public samplePointSize : number
            public renderMode : string
            public italicStyle : number
            public boldWeight : number
            public atlasPadding : number
            public static DefaultItalicStyle : number
            public static DefaultBoldWeight : number
            public static DefaultAtlasPadding : number
            public get fontType(): FairyEditor.FontAsset.FontType;
            public static IsTTF ($file: string) : boolean
            public GetFont ($flags: number) : FairyGUI.BaseFont
            public GetFont ($branch: string, $scaleLevel: number) : FairyGUI.BaseFont
            public static ParseRenderMode ($str: string) : UnityEngine.TextCore.LowLevel.GlyphRenderMode
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class ImageAsset extends FairyEditor.AssetBase
        {
            public scale9Grid : UnityEngine.Rect
            public scaleOption : string
            public qualityOption : string
            public quality : number
            public smoothing : boolean
            public gridTile : number
            public atlas : string
            public duplicatePadding : boolean
            public disableTrim : boolean
            public svgWidth : number
            public svgHeight : number
            public static QUALITY_DEFAULT : string
            public static QUALITY_SOURCE : string
            public static QUALITY_CUSTOM : string
            public static SCALE_9GRID : string
            public static SCALE_TILE : string
            public get texture(): FairyGUI.NTexture;
            public get converting(): boolean;
            public get format(): string;
            public get targetQuality(): number;
            public get file(): string;
            public LoadTexture () : System.Threading.Tasks.Task
            public LoadForPublish ($trim: boolean) : System.Threading.Tasks.Task
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        interface ISkeletonDataAsset
        {
            GetAnimations () : System.Collections.Generic.List$1<string>
            GetSkins () : System.Collections.Generic.List$1<string>
            CreateComponent () : FairyEditor.ISkeletonAnimationComponent
            Dispose () : void
        }
        class SoundAsset extends FairyEditor.AssetBase
        {
            public get audio(): UnityEngine.AudioClip;
            public Play ($volumeScale?: number) : void
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class SpineAsset extends FairyEditor.SkeletonAsset
        {
            public static ParseBounds ($sourceFile: string) : UnityEngine.Rect
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class SwfAsset extends FairyEditor.AssetBase
        {
            public constructor ($packageItem: FairyEditor.FPackageItem)
            public constructor ($item: FairyEditor.FPackageItem)
            public constructor ()
        }
        class ComExtensionDef extends System.Object
        {
            public name : string
            public className : string
            public superClassName : string
            public constructor ()
        }
        class ComExtention extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public _type : string
            public get owner(): FairyEditor.FComponent;
            public set owner(value: FairyEditor.FComponent);
            public get text(): string;
            public set text(value: string);
            public get icon(): string;
            public set icon(value: string);
            public Create () : void
            public Dispose () : void
            public Read_editMode ($xml: FairyGUI.Utils.XML) : void
            public Write_editMode () : FairyGUI.Utils.XML
            public Read ($xml: FairyGUI.Utils.XML, $strings: System.Collections.Generic.Dictionary$2<string, string>) : void
            public Write () : FairyGUI.Utils.XML
            public HandleControllerChanged ($c: FairyEditor.FController) : void
            public GetProp ($index: FairyEditor.ObjectPropID) : any
            public SetProp ($index: FairyEditor.ObjectPropID, $value: any) : void
            public GetProperty ($propName: string) : any
            public SetProperty ($propName: string, $value: any) : void
            public constructor ()
        }
        class FController extends FairyGUI.EventDispatcher implements FairyGUI.IEventDispatcher
        {
            public name : string
            public autoRadioGroupDepth : boolean
            public exported : boolean
            public alias : string
            public homePageType : string
            public homePage : string
            public parent : FairyEditor.FComponent
            public changing : boolean
            public get selectedIndex(): number;
            public set selectedIndex(value: number);
            public get previsousIndex(): number;
            public get selectedPage(): string;
            public set selectedPage(value: string);
            public get selectedPageId(): string;
            public set selectedPageId(value: string);
            public set oppositePageId(value: string);
            public get previousPage(): string;
            public get previousPageId(): string;
            public get pageCount(): number;
            public SetSelectedIndex ($value: number) : void
            public GetPages () : System.Collections.Generic.List$1<FairyEditor.FControllerPage>
            public GetPageIds ($ret?: System.Collections.Generic.List$1<string>) : System.Collections.Generic.List$1<string>
            public GetPageNames ($ret?: System.Collections.Generic.List$1<string>) : System.Collections.Generic.List$1<string>
            public HasPageId ($value: string) : boolean
            public HasPageName ($value: string) : boolean
            public GetNameById ($id: string, $emptyMsg: string) : string
            public GetNamesByIds ($ids: System.Collections.IList, $emptyMsg: string) : string
            public AddPage ($name: string) : FairyEditor.FControllerPage
            public AddPageAt ($name: string, $index: number) : FairyEditor.FControllerPage
            public RemovePageAt ($index: number) : void
            public SetPages ($pages: System.Collections.Generic.IList$1<string>) : void
            public SwapPage ($index1: number, $index2: number) : void
            public GetActions () : System.Collections.Generic.List$1<FairyEditor.FControllerAction>
            public AddAction ($type: string) : FairyEditor.FControllerAction
            public RemoveAction ($action: FairyEditor.FControllerAction) : void
            public SwapAction ($index1: number, $index2: number) : void
            public RunActions () : void
            public Read ($xml: FairyGUI.Utils.XML) : void
            public Write () : FairyGUI.Utils.XML
            public Reset () : void
            public constructor ()
        }
        enum ObjectPropID
        { Text = 0, Icon = 1, Color = 2, OutlineColor = 3, Playing = 4, Frame = 5, DeltaTime = 6, TimeScale = 7, FontSize = 8, Selected = 9, AnimationName = 10, SkinName = 11 }
        class FEvents extends System.Object
        {
            public static POS_CHANGED : string
            public static SIZE_CHANGED : string
            public static CHANGED : string
            public static PLAY_END : string
            public static SUBMIT : string
            public static ADDED : string
            public static REMOVED : string
            public static CLICK_ITEM : string
        }
        class AlignConst extends System.Object
        {
            public static LEFT : string
            public static CENTER : string
            public static RIGHT : string
            public static Parse ($str: string) : FairyGUI.AlignType
            public static ToString ($type: FairyGUI.AlignType) : string
            public ToString () : string
        }
        class VerticalAlignConst extends System.Object
        {
            public static TOP : string
            public static MIDDLE : string
            public static BOTTOM : string
            public static Parse ($str: string) : FairyGUI.VertAlignType
            public static ToString ($type: FairyGUI.VertAlignType) : string
            public ToString () : string
        }
        class AutoSizeConst extends System.Object
        {
            public static NONE : string
            public static HEIGHT : string
            public static BOTH : string
            public static SHRINK : string
            public static ELLIPSIS : string
            public static Parse ($str: string) : FairyGUI.AutoSizeType
            public static ToString ($type: FairyGUI.AutoSizeType) : string
            public ToString () : string
        }
        class OverflowConst extends System.Object
        {
            public static VISIBLE : string
            public static HIDDEN : string
            public static SCROLL : string
        }
        class ScrollBarDisplayConst extends System.Object
        {
            public static DEFAULT : string
            public static VISIBLE : string
            public static AUTO : string
            public static HIDDEN : string
        }
        class ScrollConst extends System.Object
        {
            public static HORIZONTAL : string
            public static VERTICAL : string
            public static BOTH : string
        }
        class FlipConst extends System.Object
        {
            public static NONE : string
            public static HZ : string
            public static VT : string
            public static BOTH : string
            public static Parse ($str: string) : FairyGUI.FlipType
        }
        class LoaderFillConst extends System.Object
        {
            public static NONE : string
            public static SCALE_SHOW_ALL : string
            public static SCALE_NO_BORDER : string
            public static SCALE_MATCH_HEIGHT : string
            public static SCALE_MATCH_WIDTH : string
            public static SCALE_FREE : string
        }
        class FillMethodConst extends System.Object
        {
            public static Parse ($str: string) : FairyGUI.FillMethod
        }
        class EaseTypeConst extends System.Object
        {
            public static easeType : System.Array$1<string>
            public static easeInOutType : System.Array$1<string>
            public static Parse ($value: string) : FairyGUI.EaseType
        }
        class FButton extends FairyEditor.ComExtention implements FairyGUI.IEventDispatcher
        {
            public changeStageOnClick : boolean
            public static COMMON : string
            public static CHECK : string
            public static RADIO : string
            public static UP : string
            public static DOWN : string
            public static OVER : string
            public static SELECTED_OVER : string
            public static DISABLED : string
            public static SELECTED_DISABLED : string
            public get icon(): string;
            public set icon(value: string);
            public get selectedIcon(): string;
            public set selectedIcon(value: string);
            public get title(): string;
            public set title(value: string);
            public get text(): string;
            public set text(value: string);
            public get selectedTitle(): string;
            public set selectedTitle(value: string);
            public get titleColor(): UnityEngine.Color;
            public set titleColor(value: UnityEngine.Color);
            public get titleColorSet(): boolean;
            public set titleColorSet(value: boolean);
            public get titleFontSize(): number;
            public set titleFontSize(value: number);
            public get titleFontSizeSet(): boolean;
            public set titleFontSizeSet(value: boolean);
            public get sound(): string;
            public set sound(value: string);
            public get volume(): number;
            public set volume(value: number);
            public get baseSound(): string;
            public set baseSound(value: string);
            public get baseVolume(): number;
            public set baseVolume(value: number);
            public get soundSet(): boolean;
            public set soundSet(value: boolean);
            public get downEffect(): string;
            public set downEffect(value: string);
            public get downEffectValue(): number;
            public set downEffectValue(value: number);
            public get selected(): boolean;
            public set selected(value: boolean);
            public get mode(): string;
            public set mode(value: string);
            public get controller(): string;
            public set controller(value: string);
            public get controllerObj(): FairyEditor.FController;
            public get page(): string;
            public set page(value: string);
            public GetTextField () : FairyEditor.FTextField
            public HandleGrayChanged () : boolean
            public constructor ()
        }
        class FTextField extends FairyEditor.FObject implements FairyGUI.IEventDispatcher
        {
            public clearOnPublish : boolean
            public get text(): string;
            public set text(value: string);
            public get textFormat(): FairyGUI.TextFormat;
            public get supportProEffect(): boolean;
            public get font(): string;
            public set font(value: string);
            public get fontSize(): number;
            public set fontSize(value: number);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get align(): string;
            public set align(value: string);
            public get verticalAlign(): string;
            public set verticalAlign(value: string);
            public get leading(): number;
            public set leading(value: number);
            public get letterSpacing(): number;
            public set letterSpacing(value: number);
            public get underline(): boolean;
            public set underline(value: boolean);
            public get bold(): boolean;
            public set bold(value: boolean);
            public get italic(): boolean;
            public set italic(value: boolean);
            public get strike(): boolean;
            public set strike(value: boolean);
            public get stroke(): boolean;
            public set stroke(value: boolean);
            public get strokeColor(): UnityEngine.Color;
            public set strokeColor(value: UnityEngine.Color);
            public get strokeSize(): number;
            public set strokeSize(value: number);
            public get shadowY(): number;
            public set shadowY(value: number);
            public get shadowX(): number;
            public set shadowX(value: number);
            public get shadow(): boolean;
            public set shadow(value: boolean);
            public get shadowColor(): UnityEngine.Color;
            public set shadowColor(value: UnityEngine.Color);
            public get outlineSoftness(): number;
            public set outlineSoftness(value: number);
            public get underlaySoftness(): number;
            public set underlaySoftness(value: number);
            public get faceDilate(): number;
            public set faceDilate(value: number);
            public get ubbEnabled(): boolean;
            public set ubbEnabled(value: boolean);
            public get varsEnabled(): boolean;
            public set varsEnabled(value: boolean);
            public get autoSize(): string;
            public set autoSize(value: string);
            public get singleLine(): boolean;
            public set singleLine(value: boolean);
            public InitFrom ($other: FairyEditor.FTextField) : void
            public CopyTextFormat ($source: FairyEditor.FTextField) : void
            public constructor ($flags: number)
        }
        class FComboBox extends FairyEditor.ComExtention implements FairyGUI.IEventDispatcher
        {
            public clearOnPublish : boolean
            public get title(): string;
            public set title(value: string);
            public get text(): string;
            public set text(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get titleColor(): UnityEngine.Color;
            public set titleColor(value: UnityEngine.Color);
            public get titleColorSet(): boolean;
            public set titleColorSet(value: boolean);
            public get dropdown(): string;
            public set dropdown(value: string);
            public get visibleItemCount(): number;
            public set visibleItemCount(value: number);
            public get direction(): string;
            public set direction(value: string);
            public get items(): System.Array$1<System.Array$1<string>>;
            public set items(value: System.Array$1<System.Array$1<string>>);
            public set selectedIndex(value: number);
            public get selectionController(): string;
            public set selectionController(value: string);
            public get selectionControllerObj(): FairyEditor.FController;
            public get sound(): string;
            public set sound(value: string);
            public get volume(): number;
            public set volume(value: number);
            public GetTextField () : FairyEditor.FTextField
            public constructor ()
        }
        class FTransitions extends System.Object
        {
            public _loadingSnapshot : boolean
            public get items(): System.Collections.Generic.List$1<FairyEditor.FTransition>;
            public get isEmpty(): boolean;
            public AddItem ($name?: string) : FairyEditor.FTransition
            public RemoveItem ($item: FairyEditor.FTransition) : void
            public GetItem ($name: string) : FairyEditor.FTransition
            public Read ($xml: FairyGUI.Utils.XML) : void
            public Write ($xml?: FairyGUI.Utils.XML) : FairyGUI.Utils.XML
            public Dispose () : void
            public ClearSnapshot () : void
            public TakeSnapshot () : void
            public ReadSnapshot ($readController?: boolean) : void
            public OnOwnerAddedToStage () : void
            public OnOwnerRemovedFromStage () : void
            public constructor ($owner: FairyEditor.FComponent)
            public constructor ()
        }
        class FScrollPane extends System.Object
        {
            public static DISPLAY_ON_LEFT : number
            public static SNAP_TO_ITEM : number
            public static DISPLAY_IN_DEMAND : number
            public static PAGE_MODE : number
            public static TOUCH_EFFECT_ON : number
            public static TOUCH_EFFECT_OFF : number
            public static BOUNCE_BACK_EFFECT_ON : number
            public static BOUNCE_BACK_EFFECT_OFF : number
            public static INERTIA_DISABLED : number
            public static MASK_DISABLED : number
            public static FLOATING : number
            public static DONT_CLIP_MARGIN : number
            public get vtScrollBar(): FairyEditor.FScrollBar;
            public get hzScrollBar(): FairyEditor.FScrollBar;
            public get owner(): FairyEditor.FComponent;
            public get percX(): number;
            public set percX(value: number);
            public get percY(): number;
            public set percY(value: number);
            public get posX(): number;
            public set posX(value: number);
            public get posY(): number;
            public set posY(value: number);
            public get contentWidth(): number;
            public get contentHeight(): number;
            public get viewWidth(): number;
            public set viewWidth(value: number);
            public get viewHeight(): number;
            public set viewHeight(value: number);
            public get pageX(): number;
            public set pageX(value: number);
            public get pageY(): number;
            public set pageY(value: number);
            public Dispose () : void
            public Install () : void
            public Uninstall () : void
            public SetPercX ($value: number, $ani?: boolean) : void
            public SetPercY ($value: number, $ani?: boolean) : void
            public SetPosX ($value: number, $ani?: boolean) : void
            public SetPosY ($value: number, $ani?: boolean) : void
            public SetPageX ($value: number, $ani?: boolean) : void
            public SetPageY ($value: number, $ani?: boolean) : void
            public ScrollTop ($ani?: boolean) : void
            public ScrollBottom ($ani?: boolean) : void
            public ScrollUp ($ratio?: number, $ani?: boolean) : void
            public ScrollDown ($ratio?: number, $ani?: boolean) : void
            public ScrollLeft ($ratio?: number, $ani?: boolean) : void
            public ScrollRight ($ratio?: number, $ani?: boolean) : void
            public ScrollToView ($obj: FairyEditor.FObject, $ani?: boolean, $setFirst?: boolean) : void
            public ScrollToView ($rect: UnityEngine.Rect, $ani?: boolean, $setFirst?: boolean) : void
            public OnOwnerSizeChanged () : void
            public OnFlagsChanged ($forceReceate?: boolean) : void
            public Validate ($checkOnly?: boolean) : boolean
            public UpdateScrollRect () : void
            public SetContentSize ($aWidth: number, $aHeight: number) : void
            public HandleControllerChanged ($c: FairyEditor.FController) : void
            public UpdateScrollBarVisible () : void
            public constructor ($owner: FairyEditor.FComponent)
            public constructor ()
        }
        class FMargin extends System.Object
        {
            public left : number
            public right : number
            public top : number
            public bottom : number
            public get empty(): boolean;
            public Parse ($str: string) : void
            public Reset () : void
            public Copy ($source: FairyEditor.FMargin) : void
            public constructor ()
        }
        class FControllerPage extends System.Object
        {
            public id : string
            public name : string
            public remark : string
            public constructor ()
        }
        class FControllerAction extends System.Object
        {
            public type : string
            public fromPage : System.Array$1<string>
            public toPage : System.Array$1<string>
            public transitionName : string
            public repeat : number
            public delay : number
            public stopOnExit : boolean
            public objectId : string
            public controllerName : string
            public targetPage : string
            public Run ($controller: FairyEditor.FController, $prevPage: string, $curPage: string) : void
            public Reset () : void
            public GetFullControllerName ($gcom: FairyEditor.FComponent) : string
            public GetControllerObj ($gcom: FairyEditor.FComponent) : FairyEditor.FController
            public Read ($xml: FairyGUI.Utils.XML) : void
            public Write () : FairyGUI.Utils.XML
            public constructor ()
        }
        class FCustomEase extends FairyGUI.CustomEase
        {
            public points : System.Collections.Generic.List$1<FairyGUI.GPathPoint>
            public Update () : void
            public constructor ()
        }
        class FDisplayObject extends FairyGUI.Container implements FairyGUI.IEventDispatcher
        {
            public get owner(): FairyEditor.FObject;
            public get container(): FairyGUI.Container;
            public get content(): FairyGUI.DisplayObject;
            public set content(value: FairyGUI.DisplayObject);
            public get errorStatus(): boolean;
            public set errorStatus(value: boolean);
            public Reset () : void
            public HandleSizeChanged () : void
            public SetLoading ($value: boolean) : void
            public ApplyBlendMode () : void
            public ApplyFilter () : void
            public constructor ($owner: FairyEditor.FObject)
            public constructor ()
            public constructor ($gameObjectName: string)
            public constructor ($attachTarget: UnityEngine.GameObject)
        }
        class FGraph extends FairyEditor.FObject implements FairyGUI.IEventDispatcher, FairyGUI.IHitTest
        {
            public static EMPTY : string
            public static RECT : string
            public static ELLIPSE : string
            public static POLYGON : string
            public static REGULAR_POLYGON : string
            public get type(): string;
            public set type(value: string);
            public get isVerticesEditable(): boolean;
            public get shapeLocked(): boolean;
            public set shapeLocked(value: boolean);
            public get cornerRadius(): string;
            public set cornerRadius(value: string);
            public get lineColor(): UnityEngine.Color;
            public set lineColor(value: UnityEngine.Color);
            public get lineSize(): number;
            public set lineSize(value: number);
            public get fillColor(): UnityEngine.Color;
            public set fillColor(value: UnityEngine.Color);
            public get polygonPoints(): System.Collections.Generic.List$1<UnityEngine.Vector2>;
            public get verticesDistance(): System.Collections.Generic.List$1<number>;
            public get sides(): number;
            public set sides(value: number);
            public get startAngle(): number;
            public set startAngle(value: number);
            public get polygonData(): any;
            public set polygonData(value: any);
            public AddVertex ($vx: number, $vy: number, $near: boolean) : void
            public RemoveVertex ($index: number) : void
            public UpdateVertex ($index: number, $xv: number, $yv: number) : void
            public UpdateVertexDistance ($index: number, $value: number) : void
            public CalculatePolygonBounds () : UnityEngine.Rect
            public UpdateGraph () : void
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
            public constructor ($flags: number)
        }
        class FGroup extends FairyEditor.FObject implements FairyGUI.IEventDispatcher
        {
            public _updating : number
            public _childrenDirty : boolean
            public static HORIZONTAL : string
            public static VERTICAL : string
            public get advanced(): boolean;
            public set advanced(value: boolean);
            public get excludeInvisibles(): boolean;
            public set excludeInvisibles(value: boolean);
            public get autoSizeDisabled(): boolean;
            public set autoSizeDisabled(value: boolean);
            public get mainGridMinSize(): number;
            public set mainGridMinSize(value: number);
            public get mainGridIndex(): number;
            public set mainGridIndex(value: number);
            public get hasMainGrid(): boolean;
            public set hasMainGrid(value: boolean);
            public get collapsed(): boolean;
            public set collapsed(value: boolean);
            public get layout(): string;
            public set layout(value: string);
            public get lineGap(): number;
            public set lineGap(value: number);
            public get columnGap(): number;
            public set columnGap(value: number);
            public get boundsChanged(): boolean;
            public get children(): System.Collections.Generic.List$1<FairyEditor.FObject>;
            public get empty(): boolean;
            public Refresh ($positionChangedOnly?: boolean) : void
            public FreeChildrenArray () : void
            public GetStartIndex () : number
            public UpdateImmdediately ($param?: any) : void
            public MoveChildren ($dx: number, $dy: number) : void
            public ResizeChildren ($dw: number, $dh: number) : void
            public constructor ($flags: number)
        }
        class FilterData extends System.Object
        {
            public type : string
            public brightness : number
            public contrast : number
            public saturation : number
            public hue : number
            public Read ($xml: FairyGUI.Utils.XML) : void
            public Write ($xml: FairyGUI.Utils.XML) : void
            public CopyFrom ($source: FairyEditor.FilterData) : void
            public Clone () : FairyEditor.FilterData
            public constructor ()
        }
        class FImage extends FairyEditor.FObject implements FairyGUI.IEventDispatcher, FairyGUI.IHitTest
        {
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get flip(): string;
            public set flip(value: string);
            public get fillOrigin(): number;
            public set fillOrigin(value: number);
            public get fillClockwise(): boolean;
            public set fillClockwise(value: boolean);
            public get fillMethod(): string;
            public set fillMethod(value: string);
            public get fillAmount(): number;
            public set fillAmount(value: number);
            public get bitmap(): FairyGUI.Image;
            public HitTest ($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2) : boolean
            public constructor ($flags: number)
        }
        class FLabel extends FairyEditor.ComExtention implements FairyGUI.IEventDispatcher
        {
            public restrict : string
            public maxLength : number
            public keyboardType : number
            public get icon(): string;
            public set icon(value: string);
            public get title(): string;
            public set title(value: string);
            public get text(): string;
            public set text(value: string);
            public get titleColor(): UnityEngine.Color;
            public set titleColor(value: UnityEngine.Color);
            public get titleColorSet(): boolean;
            public set titleColorSet(value: boolean);
            public get titleFontSize(): number;
            public set titleFontSize(value: number);
            public get titleFontSizeSet(): boolean;
            public set titleFontSizeSet(value: boolean);
            public get input(): boolean;
            public get password(): boolean;
            public set password(value: boolean);
            public get promptText(): string;
            public set promptText(value: string);
            public get sound(): string;
            public set sound(value: string);
            public get volume(): number;
            public set volume(value: number);
            public GetTextField () : FairyEditor.FTextField
            public constructor ()
        }
        class FList extends FairyEditor.FComponent implements FairyGUI.IEventDispatcher
        {
            public clearOnPublish : boolean
            public scrollItemToViewOnClick : boolean
            public foldInvisibleItems : boolean
            public clickToExpand : number
            public static SINGLE_COLUMN : string
            public static SINGLE_ROW : string
            public static FLOW_HZ : string
            public static FLOW_VT : string
            public static PAGINATION : string
            public get layout(): string;
            public set layout(value: string);
            public get selectionMode(): string;
            public set selectionMode(value: string);
            public get lineGap(): number;
            public set lineGap(value: number);
            public get columnGap(): number;
            public set columnGap(value: number);
            public get repeatX(): number;
            public set repeatX(value: number);
            public get repeatY(): number;
            public set repeatY(value: number);
            public get defaultItem(): string;
            public set defaultItem(value: string);
            public get autoResizeItem(): boolean;
            public set autoResizeItem(value: boolean);
            public get autoResizeItem1(): boolean;
            public set autoResizeItem1(value: boolean);
            public get autoResizeItem2(): boolean;
            public set autoResizeItem2(value: boolean);
            public get treeViewEnabled(): boolean;
            public set treeViewEnabled(value: boolean);
            public get indent(): number;
            public set indent(value: number);
            public get items(): System.Collections.Generic.List$1<FairyEditor.ListItemData>;
            public set items(value: System.Collections.Generic.List$1<FairyEditor.ListItemData>);
            public get align(): string;
            public set align(value: string);
            public get verticalAlign(): string;
            public set verticalAlign(value: string);
            public get selectionController(): string;
            public set selectionController(value: string);
            public get selectionControllerObj(): FairyEditor.FController;
            public get selectedIndex(): number;
            public set selectedIndex(value: number);
            public GetSelection ($result?: System.Collections.Generic.List$1<number>) : System.Collections.Generic.List$1<number>
            public AddSelection ($index: number, $scrollItToView?: boolean) : void
            public RemoveSelection ($index: number) : void
            public ClearSelection () : void
            public AddItem ($url: string) : FairyEditor.FObject
            public AddItemAt ($url: string, $index: number) : FairyEditor.FObject
            public ResizeToFit ($itemCount?: number, $minSize?: number) : void
            public constructor ($flags: number)
        }
        class ListItemData extends System.Object
        {
            public url : string
            public name : string
            public title : string
            public icon : string
            public selectedTitle : string
            public selectedIcon : string
            public level : number
            public get properties(): System.Collections.Generic.List$1<FairyEditor.ComProperty>;
            public CopyFrom ($source: FairyEditor.ListItemData) : void
            public constructor ()
        }
        class FLoader extends FairyEditor.FObject implements FairyGUI.IEventDispatcher
        {
            public clearOnPublish : boolean
            public get url(): string;
            public set url(value: string);
            public get texture(): FairyGUI.NTexture;
            public set texture(value: FairyGUI.NTexture);
            public get icon(): string;
            public set icon(value: string);
            public get align(): string;
            public set align(value: string);
            public get verticalAlign(): string;
            public set verticalAlign(value: string);
            public get fill(): string;
            public set fill(value: string);
            public get shrinkOnly(): boolean;
            public set shrinkOnly(value: boolean);
            public get autoSize(): boolean;
            public set autoSize(value: boolean);
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public get showErrorSign(): boolean;
            public set showErrorSign(value: boolean);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get fillOrigin(): number;
            public set fillOrigin(value: number);
            public get fillClockwise(): boolean;
            public set fillClockwise(value: boolean);
            public get fillMethod(): string;
            public set fillMethod(value: string);
            public get fillAmount(): number;
            public set fillAmount(value: number);
            public get contentRes(): FairyEditor.ResourceRef;
            public constructor ($flags: number)
        }
        class ResourceRef extends System.Object
        {
            public get packageItem(): FairyEditor.FPackageItem;
            public get displayItem(): FairyEditor.FPackageItem;
            public get displayFont(): FairyGUI.BaseFont;
            public get name(): string;
            public get desc(): string;
            public get isMissing(): boolean;
            public get missingInfo(): FairyEditor.MissingInfo;
            public get sourceWidth(): number;
            public get sourceHeight(): number;
            public SetPackageItem ($res: FairyEditor.FPackageItem, $ownerFlags?: number) : void
            public IsObsolete () : boolean
            public GetURL () : string
            public Update () : void
            public Release () : void
            public constructor ($pi?: FairyEditor.FPackageItem, $missingInfo?: FairyEditor.MissingInfo, $ownerFlags?: number)
            public constructor ()
        }
        class FLoader3D extends FairyEditor.FObject implements FairyGUI.IEventDispatcher
        {
            public clearOnPublish : boolean
            public get url(): string;
            public set url(value: string);
            public get icon(): string;
            public set icon(value: string);
            public get autoSize(): boolean;
            public set autoSize(value: boolean);
            public get fill(): string;
            public set fill(value: string);
            public get shrinkOnly(): boolean;
            public set shrinkOnly(value: boolean);
            public get align(): string;
            public set align(value: string);
            public get verticalAlign(): string;
            public set verticalAlign(value: string);
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public get animationName(): string;
            public set animationName(value: string);
            public get skinName(): string;
            public set skinName(value: string);
            public get loop(): boolean;
            public set loop(value: boolean);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public get contentRes(): FairyEditor.ResourceRef;
            public constructor ($flags: number)
        }
        class FMovieClip extends FairyEditor.FObject implements FairyGUI.IEventDispatcher
        {
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public get color(): UnityEngine.Color;
            public set color(value: UnityEngine.Color);
            public Advance ($time: number) : void
            public constructor ($flags: number)
        }
        class FTreeNode extends System.Object
        {
            public get expanded(): boolean;
            public set expanded(value: boolean);
            public get isFolder(): boolean;
            public get parent(): FairyEditor.FTreeNode;
            public get data(): any;
            public set data(value: any);
            public get text(): string;
            public get cell(): FairyEditor.FComponent;
            public get level(): number;
            public get numChildren(): number;
            public get tree(): FairyEditor.FTree;
            public AddChild ($child: FairyEditor.FTreeNode) : FairyEditor.FTreeNode
            public AddChildAt ($child: FairyEditor.FTreeNode, $index: number) : FairyEditor.FTreeNode
            public RemoveChild ($child: FairyEditor.FTreeNode) : FairyEditor.FTreeNode
            public RemoveChildAt ($index: number) : FairyEditor.FTreeNode
            public RemoveChildren ($beginIndex?: number, $endIndex?: number) : void
            public GetChildAt ($index: number) : FairyEditor.FTreeNode
            public GetChildIndex ($child: FairyEditor.FTreeNode) : number
            public GetPrevSibling () : FairyEditor.FTreeNode
            public GetNextSibling () : FairyEditor.FTreeNode
            public SetChildIndex ($child: FairyEditor.FTreeNode, $index: number) : void
            public SwapChildren ($child1: FairyEditor.FTreeNode, $child2: FairyEditor.FTreeNode) : void
            public SwapChildrenAt ($index1: number, $index2: number) : void
            public ExpandToRoot () : void
            public constructor ($hasChild: boolean, $resURL?: string)
            public constructor ()
        }
        class FPackage extends System.Object
        {
            public get opened(): boolean;
            public get project(): FairyEditor.FProject;
            public get id(): string;
            public get name(): string;
            public get basePath(): string;
            public get cacheFolder(): string;
            public get metaFolder(): string;
            public get items(): System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public get publishSettings(): FairyEditor.PublishSettings;
            public get rootItem(): FairyEditor.FPackageItem;
            public get strings(): System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, string>>;
            public set strings(value: System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, string>>);
            public GetBranchRootItem ($branch: string) : FairyEditor.FPackageItem
            public BeginBatch () : void
            public EndBatch () : void
            public Open () : void
            public Save () : void
            public SetChanged () : void
            public Touch () : void
            public Dispose () : void
            public EnsureOpen () : void
            public FreeUnusedResources ($ignoreTimeStamp: boolean) : void
            public GetNextId () : string
            public GetSequenceName ($resName: string) : string
            public GetUniqueName ($folder: FairyEditor.FPackageItem, $fileName: string) : string
            public GetItemListing ($folder: FairyEditor.FPackageItem, $filters?: System.Array$1<string>, $sorted?: boolean, $recursive?: boolean, $result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : System.Collections.Generic.List$1<FairyEditor.FPackageItem>
            public GetFavoriteItems ($result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : System.Collections.Generic.List$1<FairyEditor.FPackageItem>
            public GetItem ($itemId: string) : FairyEditor.FPackageItem
            public FindItemByName ($itemName: string) : FairyEditor.FPackageItem
            public GetItemByPath ($path: string) : FairyEditor.FPackageItem
            public GetItemByName ($folder: FairyEditor.FPackageItem, $itemName: string) : FairyEditor.FPackageItem
            public GetItemByFileName ($folder: FairyEditor.FPackageItem, $fileName: string) : FairyEditor.FPackageItem
            public GetItemPath ($pi: FairyEditor.FPackageItem, $result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : System.Collections.Generic.List$1<FairyEditor.FPackageItem>
            public AddItem ($pi: FairyEditor.FPackageItem) : void
            public RenameItem ($pi: FairyEditor.FPackageItem, $newName: string) : void
            public MoveItem ($pi: FairyEditor.FPackageItem, $path: string) : void
            public DeleteItem ($pi: FairyEditor.FPackageItem) : number
            public DuplicateItem ($pi: FairyEditor.FPackageItem, $newName: string) : FairyEditor.FPackageItem
            public EnsurePathExists ($path: string, $allowCreateDirectory: boolean) : FairyEditor.FPackageItem
            public GetBranchPath ($branch: string) : string
            public CreateBranch ($branch: string) : void
            public CreateFolder ($cname: string, $path?: string, $autoRename?: boolean) : FairyEditor.FPackageItem
            public CreatePath ($path: string) : FairyEditor.FPackageItem
            public CreateComponentItem ($cname: string, $width: number, $height: number, $path?: string, $extentionId?: string, $exported?: boolean, $autoRename?: boolean) : FairyEditor.FPackageItem
            public CreateFontItem ($cname: string, $path?: string, $autoRename?: boolean) : FairyEditor.FPackageItem
            public CreateMovieClipItem ($cname: string, $path?: string, $autoRename?: boolean) : FairyEditor.FPackageItem
            public ImportResource ($sourceFile: string, $toPath: string, $resName: string) : System.Threading.Tasks.Task$1<FairyEditor.FPackageItem>
            public UpdateResource ($pi: FairyEditor.FPackageItem, $sourceFile: string) : System.Threading.Tasks.Task
            public constructor ($project: FairyEditor.FProject, $folder: string)
            public constructor ()
        }
        class FRelations extends System.Object
        {
            public handling : FairyEditor.FObject
            public get widthLocked(): boolean;
            public get heightLocked(): boolean;
            public get items(): System.Collections.Generic.List$1<FairyEditor.FRelationItem>;
            public get isEmpty(): boolean;
            public AddItem ($target: FairyEditor.FObject, $type: number, $usePercent?: boolean) : FairyEditor.FRelationItem
            public AddItem ($target: FairyEditor.FObject, $sidePair: string) : FairyEditor.FRelationItem
            public RemoveItem ($item: FairyEditor.FRelationItem) : void
            public ReplaceItem ($index: number, $target: FairyEditor.FObject, $sidePair: string) : void
            public GetItem ($target: FairyEditor.FObject) : FairyEditor.FRelationItem
            public HasTarget ($target: FairyEditor.FObject) : boolean
            public RemoveTarget ($target: FairyEditor.FObject) : void
            public ReplaceTarget ($originTarget: FairyEditor.FObject, $newTarget: FairyEditor.FObject) : void
            public OnOwnerSizeChanged ($dWidth: number, $dHeight: number, $applyPivot: boolean) : void
            public Reset () : void
            public Read ($xml: FairyGUI.Utils.XML, $inSource?: boolean) : void
            public Write ($xml?: FairyGUI.Utils.XML) : FairyGUI.Utils.XML
            public constructor ($owner: FairyEditor.FObject)
            public constructor ()
        }
        class ObjectSnapshot extends System.Object
        {
            public x : number
            public y : number
            public width : number
            public height : number
            public scaleX : number
            public scaleY : number
            public skewX : number
            public skewY : number
            public pivotX : number
            public pivotY : number
            public anchor : boolean
            public alpha : number
            public rotation : number
            public color : UnityEngine.Color
            public playing : boolean
            public frame : number
            public visible : boolean
            public filterData : FairyEditor.FilterData
            public text : string
            public icon : string
            public animationName : string
            public skinName : string
            public static GetFromPool ($obj: FairyEditor.FObject) : FairyEditor.ObjectSnapshot
            public static ReturnToPool ($col: System.Collections.Generic.List$1<FairyEditor.ObjectSnapshot>) : void
            public Take () : void
            public Load () : void
            public constructor ()
        }
        class FObjectFactory extends System.Object
        {
            public static constructingDepth : number
            public static CreateObject ($pi: FairyEditor.FPackageItem, $flags?: number) : FairyEditor.FObject
            public static CreateObject ($pkg: FairyEditor.FPackage, $type: string, $missingInfo?: FairyEditor.MissingInfo, $flags?: number) : FairyEditor.FObject
            public static CreateObject ($di: FairyEditor.ComponentAsset.DisplayListItem, $flags?: number) : FairyEditor.FObject
            public static NewObject ($pi: FairyEditor.FPackageItem, $flags?: number) : FairyEditor.FObject
            public static NewObject ($pkg: FairyEditor.FPackage, $type: string, $missingInfo?: FairyEditor.MissingInfo, $flags?: number) : FairyEditor.FObject
            public static NewObject ($di: FairyEditor.ComponentAsset.DisplayListItem, $flags?: number) : FairyEditor.FObject
            public static NewExtention ($pkg: FairyEditor.FPackage, $type: string) : FairyEditor.ComExtention
            public static GetClassByType ($type: string) : System.Type
            public constructor ()
        }
        class MissingInfo extends System.Object
        {
            public pkgName : string
            public pkgId : string
            public itemId : string
            public fileName : string
            public constructor ($pkgId: string, $itemId: string, $fileName: string)
            public constructor ($url: string)
            public constructor ()
        }
        class FObjectFlags extends System.Object
        {
            public static IN_DOC : number
            public static IN_TEST : number
            public static IN_PREVIEW : number
            public static INSPECTING : number
            public static ROOT : number
            public static IsDocRoot ($flags: number) : boolean
            public static GetScaleLevel ($flags: number) : number
            public constructor ()
        }
        class FObjectType extends System.Object
        {
            public static PACKAGE : string
            public static FOLDER : string
            public static IMAGE : string
            public static GRAPH : string
            public static LIST : string
            public static LOADER : string
            public static TEXT : string
            public static RICHTEXT : string
            public static INPUTTEXT : string
            public static GROUP : string
            public static SWF : string
            public static MOVIECLIP : string
            public static COMPONENT : string
            public static Loader3D : string
            public static EXT_BUTTON : string
            public static EXT_LABEL : string
            public static EXT_COMBOBOX : string
            public static EXT_PROGRESS_BAR : string
            public static EXT_SLIDER : string
            public static EXT_SCROLLBAR : string
            public static NAME_PREFIX : System.Collections.Generic.Dictionary$2<string, string>
            public constructor ()
        }
        class PublishSettings extends System.Object
        {
            public path : string
            public fileName : string
            public packageCount : number
            public genCode : boolean
            public codePath : string
            public branchPath : string
            public useGlobalAtlasSettings : boolean
            public atlasList : System.Collections.Generic.List$1<FairyEditor.AtlasSettings>
            public excludedList : System.Collections.Generic.List$1<string>
            public FillCombo ($cb: FairyGUI.GComboBox) : void
            public constructor ()
        }
        class FPackageItemType extends System.Object
        {
            public static FOLDER : string
            public static IMAGE : string
            public static SWF : string
            public static MOVIECLIP : string
            public static SOUND : string
            public static COMPONENT : string
            public static FONT : string
            public static MISC : string
            public static ATLAS : string
            public static SPINE : string
            public static DRAGONBONES : string
            public static fileExtensionMap : System.Collections.Generic.Dictionary$2<string, string>
            public static GetFileType ($file: string) : string
            public constructor ()
        }
        class FProgressBar extends FairyEditor.ComExtention implements FairyGUI.IEventDispatcher
        {
            public static TITLE_PERCENT : string
            public static TITLE_VALUE_AND_MAX : string
            public static TITLE_VALUE_ONLY : string
            public static TITLE_MAX_ONLY : string
            public get titleType(): string;
            public set titleType(value: string);
            public get min(): number;
            public set min(value: number);
            public get max(): number;
            public set max(value: number);
            public get value(): number;
            public set value(value: number);
            public get reverse(): boolean;
            public set reverse(value: boolean);
            public get sound(): string;
            public set sound(value: string);
            public get volume(): number;
            public set volume(value: number);
            public Update () : void
            public constructor ()
        }
        class SettingsBase extends System.Object
        {
            public get fileName(): string;
            public Touch ($forced?: boolean) : void
            public Save () : void
        }
        class FRelationDef extends System.Object
        {
            public affectBySelfSizeChanged : boolean
            public percent : boolean
            public type : number
            public constructor ()
        }
        class FRelationItem extends System.Object
        {
            public get owner(): FairyEditor.FObject;
            public get readOnly(): boolean;
            public get target(): FairyEditor.FObject;
            public set target(value: FairyEditor.FObject);
            public get containsWidthRelated(): boolean;
            public get containsHeightRelated(): boolean;
            public get defs(): System.Collections.Generic.List$1<FairyEditor.FRelationDef>;
            public get desc(): string;
            public set desc(value: string);
            public Set ($target: FairyEditor.FObject, $sidePairs: string, $readOnly?: boolean) : void
            public Dispose () : void
            public AddDef ($type: number, $usePercent?: boolean, $checkDuplicated?: boolean) : void
            public AddDefs ($sidePairs: string, $checkDuplicated?: boolean) : void
            public HasDef ($type: number) : boolean
            public ApplySelfSizeChanged ($dWidth: number, $dHeight: number, $applyPivot: boolean) : void
            public constructor ($owner: FairyEditor.FObject)
            public constructor ()
        }
        class FRelationType extends System.Object
        {
            public static Left_Left : number
            public static Left_Center : number
            public static Left_Right : number
            public static Center_Center : number
            public static Right_Left : number
            public static Right_Center : number
            public static Right_Right : number
            public static LeftExt_Left : number
            public static LeftExt_Right : number
            public static RightExt_Left : number
            public static RightExt_Right : number
            public static Width : number
            public static Top_Top : number
            public static Top_Middle : number
            public static Top_Bottom : number
            public static Middle_Middle : number
            public static Bottom_Top : number
            public static Bottom_Middle : number
            public static Bottom_Bottom : number
            public static TopExt_Top : number
            public static TopExt_Bottom : number
            public static BottomExt_Top : number
            public static BottomExt_Bottom : number
            public static Height : number
            public static Size : number
            public static Names : System.Array$1<string>
            public constructor ()
        }
        class FRichTextField extends FairyEditor.FTextField implements FairyGUI.IEventDispatcher
        {
            public constructor ($flags: number)
        }
        class FScrollBar extends FairyEditor.ComExtention implements FairyGUI.IEventDispatcher
        {
            public get minSize(): number;
            public get fixedGripSize(): boolean;
            public set fixedGripSize(value: boolean);
            public get gripDragging(): boolean;
            public SetScrollPane ($scrollPane: FairyEditor.FScrollPane, $vertical: boolean) : void
            public SetDisplayPerc ($value: number) : void
            public SetScrollPerc ($val: number) : void
            public constructor ()
        }
        class FSlider extends FairyEditor.ComExtention implements FairyGUI.IEventDispatcher
        {
            public changeOnClick : boolean
            public static TITLE_PERCENT : string
            public static TITLE_VALUE_AND_MAX : string
            public static TITLE_VALUE_ONLY : string
            public static TITLE_MAX_ONLY : string
            public get titleType(): string;
            public set titleType(value: string);
            public get min(): number;
            public set min(value: number);
            public get max(): number;
            public set max(value: number);
            public get value(): number;
            public set value(value: number);
            public get reverse(): boolean;
            public set reverse(value: boolean);
            public get wholeNumbers(): boolean;
            public set wholeNumbers(value: boolean);
            public Update () : void
            public constructor ()
        }
        class FSwfObject extends FairyEditor.FObject implements FairyGUI.IEventDispatcher
        {
            public get playing(): boolean;
            public set playing(value: boolean);
            public get frame(): number;
            public set frame(value: number);
            public Advance ($timeInMiniseconds: number) : void
            public constructor ($flags: number)
        }
        class FTextInput extends FairyEditor.FTextField implements FairyGUI.IEventDispatcher
        {
            public get password(): boolean;
            public set password(value: boolean);
            public get keyboardType(): number;
            public set keyboardType(value: number);
            public get maxLength(): number;
            public set maxLength(value: number);
            public get restrict(): string;
            public set restrict(value: string);
            public get promptText(): string;
            public set promptText(value: string);
            public constructor ($flags: number)
        }
        class FTransition extends System.Object
        {
            public static OPTION_IGNORE_DISPLAY_CONTROLLER : number
            public static OPTION_AUTO_STOP_DISABLED : number
            public static OPTION_AUTO_STOP_AT_END : number
            public get owner(): FairyEditor.FComponent;
            public set owner(value: FairyEditor.FComponent);
            public get name(): string;
            public set name(value: string);
            public get options(): number;
            public set options(value: number);
            public get autoPlay(): boolean;
            public set autoPlay(value: boolean);
            public get autoPlayDelay(): number;
            public set autoPlayDelay(value: number);
            public get autoPlayRepeat(): number;
            public set autoPlayRepeat(value: number);
            public get frameRate(): number;
            public set frameRate(value: number);
            public get items(): System.Collections.Generic.List$1<FairyEditor.FTransitionItem>;
            public get maxFrame(): number;
            public get playing(): boolean;
            public get playTimes(): number;
            public set playTimes(value: number);
            public Dispose () : void
            public CreateItem ($targetId: string, $type: string, $frame: number) : FairyEditor.FTransitionItem
            public FindItem ($frame: number, $targetId: string, $type: string) : FairyEditor.FTransitionItem
            public FindItems ($frameStart: number, $frameEnd: number, $targetId: string, $type: string, $result: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>) : void
            public GetItemWithPath ($frame: number, $targetId: string) : FairyEditor.FTransitionItem
            public AddItem ($transItem: FairyEditor.FTransitionItem) : void
            public AddItems ($items: System.Collections.Generic.IEnumerable$1<FairyEditor.FTransitionItem>) : void
            public DeleteItem ($item: FairyEditor.FTransitionItem) : void
            public DeleteItems ($targetId: string, $type: string) : System.Array$1<FairyEditor.FTransitionItem>
            public CopyItems ($targetId: string, $type: string) : FairyGUI.Utils.XML
            public CopyItems ($items: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>) : FairyGUI.Utils.XML
            public static GetAllowType ($obj: FairyEditor.FObject, $type: string) : boolean
            public static SupportTween ($type: string) : boolean
            public UpdateFromRelations ($targetId: string, $dx: number, $dy: number) : void
            public Validate () : void
            public Read ($xml: FairyGUI.Utils.XML) : void
            public Write ($forSaving: boolean) : FairyGUI.Utils.XML
            public OnExit () : void
            public OnOwnerAddedToStage () : void
            public OnOwnerRemovedFromStage () : void
            public Play ($onComplete?: System.Action, $times?: number, $delay?: number, $startFrame?: number, $endFrame?: number, $editMode?: boolean) : void
            public Stop ($setToComplete?: boolean, $processCallback?: boolean) : void
            public GetProperty ($propName: string) : any
            public SetProperty ($propName: string, $value: any) : void
            public static ReadItems ($owner: FairyEditor.FTransition, $col: System.Collections.Generic.List$1<FairyGUI.Utils.XML>, $result: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>) : void
            public static WriteItems ($items: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>, $xml: FairyGUI.Utils.XML, $forSaving: boolean) : void
            public constructor ($owner: FairyEditor.FComponent)
            public constructor ()
        }
        class FTransitionItem extends System.Object
        {
            public easeType : string
            public easeInOutType : string
            public repeat : number
            public yoyo : boolean
            public label : string
            public value : FairyEditor.FTransitionValue
            public tweenValue : FairyEditor.FTransitionValue
            public pathOffsetX : number
            public pathOffsetY : number
            public target : FairyEditor.FObject
            public owner : FairyEditor.FTransition
            public tweener : FairyGUI.GTweener
            public innerTrans : FairyEditor.FTransition
            public nextItem : FairyEditor.FTransitionItem
            public prevItem : FairyEditor.FTransitionItem
            public displayLockToken : number
            public get type(): string;
            public set type(value: string);
            public get targetId(): string;
            public set targetId(value: string);
            public get frame(): number;
            public set frame(value: number);
            public get tween(): boolean;
            public set tween(value: boolean);
            public get easeName(): string;
            public get usePath(): boolean;
            public set usePath(value: boolean);
            public get path(): FairyEditor.GPathExt;
            public get pathPoints(): System.Collections.Generic.List$1<FairyGUI.GPathPoint>;
            public set pathPoints(value: System.Collections.Generic.List$1<FairyGUI.GPathPoint>);
            public get customEase(): FairyEditor.FCustomEase;
            public get pathData(): string;
            public set pathData(value: string);
            public get customEaseData(): string;
            public set customEaseData(value: string);
            public get encodedValue(): string;
            public set encodedValue(value: string);
            public SetPathToTweener () : void
            public AddPathPoint ($px: number, $py: number, $near: boolean) : void
            public RemovePathPoint ($pointIndex: number) : void
            public UpdatePathPoint ($pointIndex: number, $x: number, $y: number) : void
            public UpdateControlPoint ($pointIndex: number, $controlIndex: number, $x: number, $y: number) : void
            public GetProperty ($propName: string) : any
            public SetProperty ($propName: string, $value: any) : void
            public constructor ($owner: FairyEditor.FTransition)
            public constructor ()
        }
        class FTransitionValue extends System.Object
        {
            public f1 : number
            public f2 : number
            public f3 : number
            public f4 : number
            public iu : UnityEngine.Color
            public i : number
            public s : string
            public s2 : string
            public b1 : boolean
            public b2 : boolean
            public b3 : boolean
            public b4 : boolean
            public aniHandledFlag : boolean
            public CopyFrom ($source: FairyEditor.FTransitionValue) : void
            public Reset () : void
            public Equals ($other: FairyEditor.FTransitionValue) : boolean
            public Decode ($type: string, $str: string) : void
            public Encode ($type: string) : string
            public constructor ()
            public Equals ($obj: any) : boolean
            public static Equals ($objA: any, $objB: any) : boolean
        }
        class GPathExt extends FairyGUI.GPath
        {
            public points : System.Collections.Generic.List$1<FairyGUI.GPathPoint>
            public Update () : void
            public GetSegmentType ($segmentIndex: number) : FairyGUI.GPathPoint.CurveType
            public GetAnchorsInSegment ($segmentIndex: number, $result?: System.Collections.Generic.List$1<UnityEngine.Vector2>) : System.Collections.Generic.List$1<UnityEngine.Vector2>
            public FindSegmentNear ($pt: UnityEngine.Vector3) : number
            public static PointLineDistance ($pointX: number, $pointY: number, $startX: number, $startY: number, $endX: number, $endY: number, $isSegment: boolean) : number
            public constructor ()
        }
        class FTree extends FairyEditor.FTreeNode
        {
            public treeNodeRender : FairyEditor.FTree.TreeNodeRenderDelegate
            public treeNodeWillExpand : FairyEditor.FTree.TreeNodeWillExpandDelegate
            public get indent(): number;
            public set indent(value: number);
            public GetSelectedNode () : FairyEditor.FTreeNode
            public GetSelectedNodes ($result?: System.Collections.Generic.List$1<FairyEditor.FTreeNode>) : System.Collections.Generic.List$1<FairyEditor.FTreeNode>
            public SelectNode ($node: FairyEditor.FTreeNode, $scrollItToView?: boolean) : void
            public UnselectNode ($node: FairyEditor.FTreeNode) : void
            public GetNodeIndex ($node: FairyEditor.FTreeNode) : number
            public UpdateNode ($node: FairyEditor.FTreeNode) : void
            public UpdateNodes ($nodes: System.Collections.Generic.List$1<FairyEditor.FTreeNode>) : void
            public ExpandAll ($folderNode?: FairyEditor.FTreeNode) : void
            public CollapseAll ($folderNode?: FairyEditor.FTreeNode) : void
            public CreateCell ($node: FairyEditor.FTreeNode) : void
            public constructor ($list: FairyEditor.FList)
            public constructor ($hasChild: boolean, $resURL?: string)
            public constructor ()
        }
        class FHtmlImage extends System.Object implements FairyGUI.Utils.IHtmlObject
        {
            public get loader(): FairyEditor.FLoader;
            public get displayObject(): FairyGUI.DisplayObject;
            public get element(): FairyGUI.Utils.HtmlElement;
            public get width(): number;
            public get height(): number;
            public Create ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : void
            public SetPosition ($x: number, $y: number) : void
            public Add () : void
            public Remove () : void
            public Release () : void
            public Dispose () : void
            public constructor ()
        }
        class FHtmlPageContext extends System.Object implements FairyGUI.Utils.IHtmlPageContext
        {
            public static inst : FairyGUI.Utils.HtmlPageContext
            public CreateObject ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : FairyGUI.Utils.IHtmlObject
            public FreeObject ($obj: FairyGUI.Utils.IHtmlObject) : void
            public GetImageTexture ($image: FairyGUI.Utils.HtmlImage) : FairyGUI.NTexture
            public FreeImageTexture ($image: FairyGUI.Utils.HtmlImage, $texture: FairyGUI.NTexture) : void
            public constructor ()
        }
        class ProjectType extends System.Object
        {
            public static Flash : string
            public static Starling : string
            public static Unity : string
            public static Egret : string
            public static Layabox : string
            public static Haxe : string
            public static PIXI : string
            public static Cocos2dx : string
            public static CryEngine : string
            public static Vision : string
            public static MonoGame : string
            public static CocosCreator : string
            public static LibGDX : string
            public static Unreal : string
            public static Corona : string
            public static ThreeJS : string
            public static CreateJS : string
            public static DOM : string
            public static IDs : System.Array$1<string>
            public static Names : System.Array$1<string>
            public constructor ()
        }
        interface ISkeletonAnimationComponent
        {
            gameObject : UnityEngine.GameObject
            SetColor ($c: UnityEngine.Color) : void
            SetAlpha ($alpha: number) : void
            UpdateAnimation ($animationName: string, $loop: boolean, $playing: boolean, $frame: number, $skinName: string) : void
            Advance ($time: number) : void
            Dispose () : void
        }
        class SpineCompatibilityHelper extends System.Object
        {
            public static GetFunction ($info: Spine.Unity.SkeletonDataCompatibility.VersionInfo) : FairyEditor.SpineCompatibilityHelper.Delegates
            public static GetVersionInfo ($descAsset: UnityEngine.TextAsset, $problemDescription: $Ref<string>) : Spine.Unity.SkeletonDataCompatibility.VersionInfo
        }
        class PublishHandler extends System.Object
        {
            public static CODE_FILE_MARK : string
            public genCodeHandler : System.Action$1<FairyEditor.PublishHandler>
            public get pkg(): FairyEditor.FPackage;
            public get project(): FairyEditor.FProject;
            public get isSuccess(): boolean;
            public get publishDescOnly(): boolean;
            public set publishDescOnly(value: boolean);
            public get exportPath(): string;
            public set exportPath(value: string);
            public get exportCodePath(): string;
            public set exportCodePath(value: string);
            public get useAtlas(): boolean;
            public get fileName(): string;
            public get fileExtension(): string;
            public get genCode(): boolean;
            public set genCode(value: boolean);
            public get items(): System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public get paused(): boolean;
            public set paused(value: boolean);
            public ExportBinaryDesc ($descFile: string) : void
            public ExportDescZip ($zipArchive: System.IO.Compression.ZipStorer) : void
            public ExportResZip ($zipArchive: System.IO.Compression.ZipStorer, $compress: boolean) : void
            public ExportResFiles ($resPrefix: string) : System.Threading.Tasks.Task
            public ClearOldResFiles ($folder: string) : void
            public CollectClasses ($stripMember: boolean, $stripClass: boolean, $fguiNamespace: string) : System.Collections.Generic.List$1<FairyEditor.PublishHandler.ClassInfo>
            public SetupCodeFolder ($path: string, $codeFileExtensions: string) : void
            public SetupCodeFolder ($path: string, $codeFileExtensions: string, $fileMark: string) : void
            public ToFilename ($source: string) : string
            public add_onComplete ($value: System.Action) : void
            public remove_onComplete ($value: System.Action) : void
            public IsInList ($item: FairyEditor.FPackageItem) : boolean
            public GetItemDesc ($item: FairyEditor.FPackageItem) : any
            public GetScriptData ($item: FairyEditor.FPackageItem) : FairyGUI.Utils.XML
            public Run () : System.Threading.Tasks.Task
            public constructor ($pkg: FairyEditor.FPackage, $branch: string)
            public constructor ()
        }
        class Clipboard extends System.Object
        {
            public static TEXT : string
            public static OBJECT_KEY : string
            public static ITEM_KEY : string
            public static TIMELINE_KEY : string
            public static CONTROLLER_KEY : string
            public static RELATION_PROPS_KEY : string
            public static GEAR_PROPS_KEY : string
            public static SetText ($value: string) : void
            public static GetText () : string
            public static GetValue ($key: string) : any
            public static SetValue ($key: string, $value: any) : void
            public static HasFormat ($key: string) : boolean
        }
        class ComponentTemplates extends System.Object
        {
            public CreateLabelItem ($cname: string, $width: number, $height: number, $path: string) : FairyEditor.FPackageItem
            public CreateButtonItem ($cname: string, $extentionId: string, $mode: string, $images: System.Array$1<string>, $width: number, $height: number, $asListItem: boolean, $createRelations: boolean, $createText: boolean, $createIcon: boolean, $exported: boolean, $path: string) : FairyEditor.FPackageItem
            public CreateComboBoxItem ($cname: string, $buttonImages: System.Array$1<string>, $bgImage: string, $itemImages: System.Array$1<string>, $path: string) : FairyEditor.FPackageItem
            public CreateScrollBarItem ($cname: string, $type: number, $createArrows: boolean, $arrow1Images: System.Array$1<string>, $arrow2Images: System.Array$1<string>, $bgImage: string, $gripImages: System.Array$1<string>, $path: string) : FairyEditor.FPackageItem
            public CreateProgressBarItem ($cname: string, $bgImage: string, $barImage: string, $titleType: string, $reverse: boolean, $path: string) : FairyEditor.FPackageItem
            public CreateSliderItem ($cname: string, $type: number, $bgImage: string, $barImage: string, $gripImages: System.Array$1<string>, $titleType: string, $path: string) : FairyEditor.FPackageItem
            public CreatePopupMenuItem ($cname: string, $bgImage: string, $itemImages: System.Array$1<string>, $path: string) : FairyEditor.FPackageItem
            public CreateWindowFrameItem ($cname: string, $bgImage: string, $closeButton: string, $createTitle: boolean, $createIcon: boolean, $path: string) : FairyEditor.FPackageItem
            public constructor ($pkg: FairyEditor.FPackage)
            public constructor ()
        }
        class CopyHandler extends System.Object
        {
            public get resultList(): System.Collections.Generic.List$1<FairyEditor.DepItem>;
            public get existsItemCount(): number;
            public InitWithItems ($items: System.Collections.Generic.IList$1<FairyEditor.FPackageItem>, $targetPkg: FairyEditor.FPackage, $targetPath: string, $seekLevel: FairyEditor.DependencyQuery.SeekLevel) : void
            public InitWithObject ($sourcePkg: FairyEditor.FPackage, $xml: FairyGUI.Utils.XML, $targetPkg: FairyEditor.FPackage, $targetPath: string, $ignoreExported?: boolean) : void
            public Copy ($targetPkg: FairyEditor.FPackage, $overrideOption: FairyEditor.CopyHandler.OverrideOption, $isMove?: boolean) : void
            public constructor ()
        }
        class DepItem extends System.Object
        {
            public item : FairyEditor.FPackageItem
            public content : any
            public isSource : boolean
            public analysed : boolean
            public targetPath : string
            public refCount : number
            public constructor ()
        }
        class CursorType extends System.Object
        {
            public static H_RESIZE : string
            public static V_RESIZE : string
            public static TL_RESIZE : string
            public static TR_RESIZE : string
            public static BL_RESIZE : string
            public static BR_RESIZE : string
            public static SELECT : string
            public static HAND : string
            public static DRAG : string
            public static ADJUST : string
            public static FINGER : string
            public static COLOR_PICKER : string
            public static WAIT : string
            public constructor ()
        }
        class DependencyQuery extends System.Object
        {
            public get resultList(): System.Collections.Generic.List$1<FairyEditor.DepItem>;
            public get references(): System.Collections.Generic.List$1<FairyEditor.ReferenceInfo>;
            public QueryDependencies ($items: System.Collections.Generic.IList$1<FairyEditor.FPackageItem>, $seekLevel: FairyEditor.DependencyQuery.SeekLevel) : void
            public QueryDependencies ($project: FairyEditor.FProject, $url: string, $seekLevel: FairyEditor.DependencyQuery.SeekLevel) : void
            public QueryDependencies ($pkg: FairyEditor.FPackage, $xml: FairyGUI.Utils.XML, $seekLevel: FairyEditor.DependencyQuery.SeekLevel) : void
            public QueryReferences ($project: FairyEditor.FProject, $url: string) : void
            public ReplaceReferences ($newItem: FairyEditor.FPackageItem) : void
            public constructor ()
        }
        class ReferenceInfo extends System.Object
        {
            public ownerPkg : FairyEditor.FPackage
            public pkgId : string
            public itemId : string
            public content : any
            public propKey : string
            public arrayIndex : number
            public valueType : FairyEditor.ReferenceInfo.ValueType
            public Update ($newItem: FairyEditor.FPackageItem) : boolean
            public constructor ()
        }
        class EditorEvents extends System.Object
        {
            public static SelectionChanged : string
            public static DocumentActivated : string
            public static DocumentDeactivated : string
            public static TestStart : string
            public static TestStop : string
            public static PackageListChanged : string
            public static PackageReloaded : string
            public static PackageTreeChanged : string
            public static PackageItemChanged : string
            public static HierarchyChanged : string
            public static ProjectRefreshStart : string
            public static ProjectRefreshEnd : string
            public static BackgroundChanged : string
            public static PluginListChanged : string
            public constructor ()
        }
        class ExportStringsHandler extends System.Object
        {
            public Parse ($pkgs: System.Collections.Generic.IList$1<FairyEditor.FPackage>, $ignoreDiscarded?: boolean) : void
            public Export ($file: string, $merge: boolean) : void
            public constructor ()
        }
        class FindDuplicateResource extends System.Object
        {
            public get result(): System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public GetGroup ($index: number, $result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : void
            public Start ($pkgs: System.Collections.Generic.List$1<FairyEditor.FPackage>, $onProgress: System.Action$1<number>, $onComplete: System.Action) : void
            public Cancel () : void
            public constructor ()
        }
        class FindUnusedResource extends System.Object
        {
            public get result(): System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public Start ($pkgs: System.Collections.Generic.List$1<FairyEditor.FPackage>, $onProgress: System.Action$1<number>, $onComplete: System.Action) : void
            public Cancel () : void
            public constructor ()
        }
        class FullSearch extends System.Object
        {
            public get result(): System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public Start ($strName: string, $strTypes: string, $includeBrances: boolean) : void
            public constructor ()
        }
        class ImportStringsHandler extends System.Object
        {
            public get strings(): System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, string>>>;
            public Parse ($file: string) : void
            public Import () : void
            public constructor ()
        }
        class ProjectRefreshHandler extends System.Object
        {
            public Dispose () : void
            public Run () : void
            public constructor ()
        }
        class ResourceImportQueue extends System.Object
        {
            public static Create ($toPkg: FairyEditor.FPackage) : FairyEditor.ResourceImportQueue
            public Add ($file: string, $targetPath?: string, $resName?: string) : FairyEditor.ResourceImportQueue
            public AddRelative ($file: string, $targetPath?: string, $basePath?: string, $resName?: string) : FairyEditor.ResourceImportQueue
            public Process ($callback?: System.Action$1<System.Collections.Generic.IList$1<FairyEditor.FPackageItem>>, $dropToDocument?: boolean, $dropPos?: System.Nullable$1<UnityEngine.Vector2>) : void
        }
        class ViewOptions extends System.Object
        {
            public title : string
            public icon : string
            public hResizePriority : number
            public vResizePriority : number
            public location : string
            public constructor ()
        }
        class AdaptationSettings extends FairyEditor.SettingsBase
        {
            public scaleMode : string
            public screenMathMode : string
            public designResolutionX : number
            public designResolutionY : number
            public devices : System.Collections.Generic.List$1<FairyEditor.AdaptationSettings.DeviceInfo>
            public defaultDevices : System.Collections.Generic.List$1<FairyEditor.AdaptationSettings.DeviceInfo>
            public GetDeviceResolution ($device: string) : FairyEditor.AdaptationSettings.DeviceInfo
            public FillCombo ($cb: FairyGUI.GComboBox) : void
            public constructor ($project: FairyEditor.FProject)
            public constructor ()
        }
        class AtlasSettings extends System.Object
        {
            public name : string
            public compression : boolean
            public extractAlpha : boolean
            public packSettings : FairyEditor.PackSettings
            public CopyFrom ($source: FairyEditor.AtlasSettings) : void
            public constructor ()
        }
        class PackSettings extends System.Object
        {
            public pot : boolean
            public mof : boolean
            public padding : number
            public rotation : boolean
            public minWidth : number
            public minHeight : number
            public maxWidth : number
            public maxHeight : number
            public square : boolean
            public fast : boolean
            public edgePadding : boolean
            public duplicatePadding : boolean
            public multiPage : boolean
            public CopyFrom ($source: FairyEditor.PackSettings) : void
            public constructor ()
        }
        class CommonSettings extends FairyEditor.SettingsBase
        {
            public font : string
            public fontSize : number
            public textColor : UnityEngine.Color
            public fontAdjustment : boolean
            public colorScheme : System.Collections.Generic.List$1<string>
            public fontSizeScheme : System.Collections.Generic.List$1<string>
            public fontScheme : System.Collections.Generic.List$1<string>
            public scrollBars : FairyEditor.CommonSettings.ScrollBarConfig
            public tipsRes : string
            public buttonClickSound : string
            public pivot : string
            public constructor ($project: FairyEditor.FProject)
            public constructor ()
        }
        class CustomProps extends FairyEditor.SettingsBase
        {
            public elements : System.Collections.Generic.Dictionary$2<string, string>
            public FillCombo ($cb: FairyGUI.GComboBox) : void
            public constructor ($project: FairyEditor.FProject)
            public constructor ()
        }
        class GlobalPublishSettings extends FairyEditor.SettingsBase
        {
            public path : string
            public branchPath : string
            public fileExtension : string
            public packageCount : number
            public compressDesc : boolean
            public binaryFormat : boolean
            public jpegQuality : number
            public compressPNG : boolean
            public codeGeneration : FairyEditor.GlobalPublishSettings.CodeGenerationConfig
            public includeHighResolution : number
            public branchProcessing : number
            public seperatedAtlasForBranch : boolean
            public atlasSetting : FairyEditor.GlobalPublishSettings.AtlasSetting
            public get include2x(): boolean;
            public set include2x(value: boolean);
            public get include3x(): boolean;
            public set include3x(value: boolean);
            public get include4x(): boolean;
            public set include4x(value: boolean);
            public constructor ($project: FairyEditor.FProject)
            public constructor ()
        }
        class I18nSettings extends FairyEditor.SettingsBase
        {
            public langFiles : System.Collections.Generic.List$1<FairyEditor.I18nSettings.LanguageFile>
            public get lang(): FairyEditor.I18nSettings.LanguageFile;
            public SetLangByName ($langName: string) : void
            public LoadStrings () : void
            public FillCombo ($cb: FairyGUI.GComboBox) : void
            public constructor ($project: FairyEditor.FProject)
            public constructor ()
        }
        interface ILicenseManager
        {
            clientId : string
            isPro : boolean
            isExpired : boolean
            expireDateString : string
            keyHash : string
            Init () : void
            SetKey ($key: string) : void
            Revoke () : void
        }
        class PackageGroupSettings extends FairyEditor.SettingsBase
        {
            public groups : System.Collections.Generic.List$1<FairyEditor.PackageGroupSettings.PackageGroup>
            public GetGroup ($name: string) : FairyEditor.PackageGroupSettings.PackageGroup
            public constructor ($project: FairyEditor.FProject)
            public constructor ()
        }
        class ArrowKeyHelper extends System.Object
        {
            public static direction : number
            public static shift : boolean
            public static ctrlOrCmd : boolean
            public static OnKeyDown ($evt: FairyGUI.InputEvent) : void
            public static OnKeyUp ($evt: FairyGUI.InputEvent) : void
            public static Reset () : void
        }
        class AssetSizeUtil extends System.Object
        {
            public static GetSize ($file: string) : FairyEditor.AssetSizeUtil.Result
        }
        class BuilderUtil extends System.Object
        {
            public static TimeBase : Date
            public static GenerateUID () : string
            public static GenDevCode () : string
            public static ToStringBase36 ($num: bigint) : string
            public static ToNumberBase36 ($str: string) : number
            public static Encrypt_MD5 ($input: string, $encode?: System.Text.Encoding) : string
            public static GetMD5HashFromFile ($filePath: string) : string
            public static Decrypt_DES16 ($base64String: string, $key: string) : string
            public static Encrypt_DES16 ($source: string, $key: string) : string
            public static Union ($rect1: UnityEngine.Rect, $rect2: UnityEngine.Rect) : UnityEngine.Rect
            public static GetNameFromId ($aId: string) : string
            public static GetFileExtension ($fileName: string, $keepCase?: boolean) : string
            public static PointLineDistance ($pointX: number, $pointY: number, $startX: number, $startY: number, $endX: number, $endY: number, $isSegment: boolean) : number
            public static GetSizeName ($val: number, $digits?: number) : string
            public static OpenURL ($url: string) : void
            public static OpenWithDefaultApp ($file: string) : void
            public static RevealInExplorer ($file: string) : void
            public static ToUnixTimestamp ($dateTime: Date) : bigint
            public static WaitForNextFrame () : System.Threading.Tasks.Task
            public static CreateZip ($zipFile: string, $dir: string) : void
            public static Unzip ($zipFile: string, $dir: string) : void
        }
        class BytesWriter extends System.Object
        {
            public littleEndian : boolean
            public get length(): number;
            public get position(): number;
            public set position(value: number);
            public ReadByte ($pos: number) : number
            public WriteByte ($value: number) : void
            public WriteBoolean ($value: boolean) : void
            public WriteShort ($value: number) : void
            public WriteInt ($value: number) : void
            public WriteFloat ($value: number) : void
            public WriteUTF ($str: string) : void
            public WriteUTFBytes ($str: string) : void
            public WriteBytes ($bytes: System.Array$1<number>) : void
            public WriteBytes ($ba: FairyEditor.BytesWriter) : void
            public WriteColor ($c: UnityEngine.Color32) : void
            public ToBytes () : System.Array$1<number>
            public constructor ()
        }
        class ColorUtil extends System.Object
        {
            public static ToHexString ($color: UnityEngine.Color, $includeAlpha?: boolean) : string
            public static FromHexString ($str: string, $hasAlpha?: boolean) : UnityEngine.Color
            public static FromARGB ($argb: number) : UnityEngine.Color
            public static FromRGB ($rgb: number) : UnityEngine.Color
            public static ToRGB ($color: UnityEngine.Color) : number
            public static ToARGB ($color: UnityEngine.Color) : number
        }
        class FontUtil extends System.Object
        {
            public static GetOSInstalledFontNames ($forceRefresh: boolean) : System.Collections.Generic.List$1<FairyEditor.FontUtil.FontInfo>
            public static RequestFont ($family: string) : void
            public static GetFontName ($fontFile: string) : FairyEditor.FontUtil.FontName
            public static GetPreviewTexture ($fontInfo: FairyEditor.FontUtil.FontInfo) : FairyGUI.NTexture
        }
        class ImageUtil extends System.Object
        {
            public static get ToolAvailable(): boolean;
            public static Init () : void
            public static Quantize ($image: FairyEditor.VImage) : System.Array$1<number>
            public static Quantize ($pngFile: string, $targetFile: string) : boolean
            public static Quantize ($pngFile: string) : string
        }
        class VImage extends System.Object implements System.IDisposable
        {
            public get width(): number;
            public get height(): number;
            public get transparent(): boolean;
            public get bandCount(): number;
            public static New ($width: number, $height: number, $transparent: boolean) : FairyEditor.VImage
            public static New ($width: number, $height: number, $transparent: boolean, $fillColor: UnityEngine.Color) : FairyEditor.VImage
            public static New ($file: string) : FairyEditor.VImage
            public static New ($data: System.Array$1<number>) : FairyEditor.VImage
            public static New ($file: string, $width: number, $height: number) : FairyEditor.VImage
            public static Thumbnail ($file: string, $width: number) : FairyEditor.VImage
            public static GetImageSize ($file: string, $width: $Ref<number>, $height: $Ref<number>) : boolean
            public Dispose () : void
            public Resize ($width: number, $height: number, $kernel?: FairyEditor.VImage.Kernel) : void
            public ResizeBy ($hscale: number, $vscale: number, $kernel?: FairyEditor.VImage.Kernel) : void
            public Rotate ($angle: number) : void
            public FindTrim () : UnityEngine.Rect
            public Crop ($rect: UnityEngine.Rect) : void
            public Embed ($x: number, $y: number, $width: number, $height: number, $extend: FairyEditor.VImage.Extend, $background: UnityEngine.Color) : void
            public AlphaBlend ($another: FairyEditor.VImage, $x: number, $y: number) : void
            public CopyPixels ($another: FairyEditor.VImage, $x: number, $y: number) : void
            public CopyPixels ($another: FairyEditor.VImage, $sourceRect: UnityEngine.Rect, $destPoint: UnityEngine.Vector2) : void
            public Composite ($another: FairyEditor.VImage, $x: number, $y: number, $blendMode: FairyEditor.VImage.BlendMode, $premultiplied: boolean) : void
            public Composite ($images: System.Collections.Generic.IList$1<FairyEditor.VImage>, $pos: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $blendMode: FairyEditor.VImage.BlendMode, $premultiplied: boolean) : void
            public ExtractAlpha ($returnAlpha: boolean) : FairyEditor.VImage
            public Clear ($color: UnityEngine.Color) : void
            public DrawRect ($x: number, $y: number, $width: number, $height: number, $color: UnityEngine.Color, $fill: boolean) : void
            public GetRawData () : System.IntPtr
            public GetRawDataSize () : number
            public GetPixels () : System.Array$1<number>
            public ToTexture ($smoothing: boolean, $makeNoLongerReadable: boolean) : UnityEngine.Texture2D
            public GetAnimation () : FairyEditor.VImage.Animation
            public Save ($file: string) : void
            public Save ($file: string, $format: string) : void
            public Save ($file: string, $format: string, $quality: number) : void
            public Clone () : FairyEditor.VImage
            public static InitLibrary () : void
        }
        class IOUtil extends System.Object
        {
            public static DeleteFile ($file: string, $toTrash?: boolean) : void
            public static CopyFile ($sourceFile: string, $destFile: string) : void
            public static BrowseForDirectory ($title: string, $callback: System.Action$1<string>) : void
            public static BrowseForOpen ($title: string, $directory: string, $extensions: System.Array$1<SFB.ExtensionFilter>, $callback: System.Action$1<string>) : void
            public static BrowseForOpenMultiple ($title: string, $directory: string, $extensions: System.Array$1<SFB.ExtensionFilter>, $callback: System.Action$1<System.Array$1<string>>) : void
            public static BrowseForSave ($title: string, $directory: string, $extension: SFB.ExtensionFilter, $callback: System.Action$1<string>) : void
            public static BrowseForSave ($title: string, $directory: string, $defaultName: string, $defaultExt: string, $callback: System.Action$1<string>) : void
        }
        class JsonUtil extends System.Object
        {
            public static ColorHexFormat : boolean
            public static DecodeJson ($content: string) : any
            public static EncodeJson ($obj: any) : string
            public static EncodeJson ($obj: any, $indent: boolean) : string
        }
        class NativeDragDrop extends System.Object
        {
            public static Init () : void
            public static Dispose () : void
        }
        class UserActionException extends System.Exception implements System.Runtime.InteropServices._Exception, System.Runtime.Serialization.ISerializable
        {
            public constructor ($message: string)
            public constructor ()
        }
        class PathPointsUtil extends System.Object
        {
            public static InsertPoint ($pos: UnityEngine.Vector3, $index: number, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>) : void
            public static RemovePoint ($index: number, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>) : void
            public static UpdatePoint ($index: number, $pos: UnityEngine.Vector3, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>) : void
            public static UpdateControlPoint ($pointIndex: number, $controlIndex: number, $pos: UnityEngine.Vector3, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>) : void
            public static SerializeFrom ($source: string, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>) : void
            public static SerializeTo ($points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>) : string
        }
        class PlistElement extends System.Object
        {
            public AsString () : string
            public AsInteger () : number
            public AsBoolean () : boolean
            public AsArray () : FairyEditor.PlistElementArray
            public AsDict () : FairyEditor.PlistElementDict
            public AsReal () : number
            public AsDate () : Date
            public get_Item ($key: string) : FairyEditor.PlistElement
            public set_Item ($key: string, $value: FairyEditor.PlistElement) : void
        }
        class PlistElementArray extends FairyEditor.PlistElement
        {
            public values : System.Collections.Generic.List$1<FairyEditor.PlistElement>
            public AddString ($val: string) : void
            public AddInteger ($val: number) : void
            public AddBoolean ($val: boolean) : void
            public AddDate ($val: Date) : void
            public AddReal ($val: number) : void
            public AddArray () : FairyEditor.PlistElementArray
            public AddDict () : FairyEditor.PlistElementDict
            public constructor ()
        }
        class PlistElementDict extends FairyEditor.PlistElement
        {
            public get values(): System.Collections.Generic.IDictionary$2<string, FairyEditor.PlistElement>;
            public get_Item ($key: string) : FairyEditor.PlistElement
            public set_Item ($key: string, $value: FairyEditor.PlistElement) : void
            public SetInteger ($key: string, $val: number) : void
            public SetString ($key: string, $val: string) : void
            public SetBoolean ($key: string, $val: boolean) : void
            public SetDate ($key: string, $val: Date) : void
            public SetReal ($key: string, $val: number) : void
            public CreateArray ($key: string) : FairyEditor.PlistElementArray
            public CreateDict ($key: string) : FairyEditor.PlistElementDict
            public constructor ()
        }
        class PlistElementString extends FairyEditor.PlistElement
        {
            public value : string
            public constructor ($v: string)
            public constructor ()
        }
        class PlistElementInteger extends FairyEditor.PlistElement
        {
            public value : number
            public constructor ($v: number)
            public constructor ()
        }
        class PlistElementReal extends FairyEditor.PlistElement
        {
            public value : number
            public constructor ($v: number)
            public constructor ()
        }
        class PlistElementBoolean extends FairyEditor.PlistElement
        {
            public value : boolean
            public constructor ($v: boolean)
            public constructor ()
        }
        class PlistElementDate extends FairyEditor.PlistElement
        {
            public value : Date
            public constructor ($date: Date)
            public constructor ()
        }
        class PlistDocument extends System.Object
        {
            public root : FairyEditor.PlistElementDict
            public version : string
            public Create () : void
            public ReadFromFile ($path: string) : void
            public ReadFromStream ($tr: System.IO.TextReader) : void
            public ReadFromString ($text: string) : void
            public WriteToFile ($path: string) : void
            public WriteToStream ($tw: System.IO.TextWriter) : void
            public WriteToString () : string
            public constructor ()
        }
        class PrimitiveExtension extends System.Object
        {
            public static FormattedString ($value: number, $fractionDigits?: number) : string
        }
        class ProcessUtil extends System.Object
        {
            public static LaunchApp () : void
            public static Start ($path: string, $args: System.Array$1<string>, $dir: string, $waitUntilExit: boolean) : string
            public static GetOpenFilename () : string
            public static GetUUID () : string
            public static GetAppVersion () : string
        }
        class ReflectionUtil extends System.Object
        {
            public static GetInfo ($type: System.Type, $propName: string) : any
            public static GetProperty ($obj: any, $propName: string) : any
            public static SetProperty ($obj: any, $propName: string, $value: any) : void
        }
        class WindowUtil extends System.Object
        {
            public static ChangeTitle ($title: string) : void
            public static ChangeIcon ($icon: string) : void
            public static GetScaleFactor () : number
            public static BringToFront () : void
        }
        class XMLExtension extends System.Object
        {
            public static Load ($file: string) : FairyGUI.Utils.XML
            public static LoadXMLBrief ($file: string) : FairyGUI.Utils.XML
            public static GetAttributeArray ($xml: FairyGUI.Utils.XML, $attrName: string, $i1: $Ref<number>, $i2: $Ref<number>) : boolean
            public static GetAttributeArray ($xml: FairyGUI.Utils.XML, $attrName: string, $i1: $Ref<number>, $i2: $Ref<number>, $i3: $Ref<number>, $i4: $Ref<number>) : boolean
            public static GetAttributeArray ($xml: FairyGUI.Utils.XML, $attrName: string, $f1: $Ref<number>, $f2: $Ref<number>, $f3: $Ref<number>, $f4: $Ref<number>) : boolean
            public static GetAttributeArray ($xml: FairyGUI.Utils.XML, $attrName: string, $f1: $Ref<number>, $f2: $Ref<number>) : boolean
            public static GetAttributeArray ($xml: FairyGUI.Utils.XML, $attrName: string, $s1: $Ref<string>, $s2: $Ref<string>) : boolean
        }
    }
        class ExternalImagePool extends System.Object
        {
        }
        namespace FairyEditor.View {
        class MainView extends System.Object
        {
            public get panel(): FairyGUI.GComponent;
            public get toolbar(): FairyGUI.GComponent;
            public UpdateUserInfo () : void
            public ShowNewVersionPrompt ($versionName: string) : void
            public ShowRestartPrompt () : void
            public ShowAlreadyUpdatedPrompt () : void
            public ShowStartScene () : void
            public HandleGlobalHotkey ($funcId: string) : boolean
            public FillLanguages () : void
            public DropFiles ($mousePos: UnityEngine.Vector2, $arrFiles: System.Array$1<string>) : void
            public constructor ()
        }
        class DocumentView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public get docContainer(): FairyGUI.GComponent;
            public get activeDoc(): FairyEditor.View.IDocument;
            public set activeDoc(value: FairyEditor.View.IDocument);
            public get viewScale(): number;
            public set viewScale(value: number);
            public AddFactory ($factory: FairyEditor.View.IDocumentFactory) : void
            public RemoveFactory ($factory: FairyEditor.View.IDocumentFactory) : void
            public FindDocument ($docURL: string) : FairyEditor.View.IDocument
            public CloseDocuments ($pkg: FairyEditor.FPackage) : void
            public OpenDocument ($url: string, $activateIt?: boolean) : FairyEditor.View.IDocument
            public SaveDocument ($doc?: FairyEditor.View.IDocument) : void
            public SaveAllDocuments () : void
            public CloseDocument ($doc?: FairyEditor.View.IDocument) : void
            public CloseAllDocuments () : void
            public QueryToCloseDocument ($doc?: FairyEditor.View.IDocument) : void
            public QueryToCloseOtherDocuments () : void
            public QueryToCloseAllDocuments () : void
            public QueryToSaveAllDocuments ($callback: System.Action$1<string>) : void
            public HasUnsavedDocuments () : boolean
            public UpdateTab ($doc: FairyEditor.View.IDocument) : void
            public HandleHotkey ($context: FairyGUI.EventContext) : void
            public constructor ()
        }
        class LibraryView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public get contextMenu(): FairyEditor.Component.IMenu;
            public get contextMenu_Folder(): FairyEditor.Component.IMenu;
            public get contextMenu_Package(): FairyEditor.Component.IMenu;
            public get currentGroup(): string;
            public GetFolderUnderPoint ($globalPos: UnityEngine.Vector2, $touchTarget: FairyGUI.GObject) : FairyEditor.FPackageItem
            public GetSelectedResource () : FairyEditor.FPackageItem
            public GetSelectedResources ($includeChildren: boolean) : System.Collections.Generic.List$1<FairyEditor.FPackageItem>
            public GetSelectedFolder () : FairyEditor.FPackageItem
            public Highlight ($pi: FairyEditor.FPackageItem, $setFocus?: boolean) : void
            public MoveResources ($dropTarget: FairyEditor.FPackageItem, $items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : void
            public DeleteResources ($items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : void
            public SetResourcesExported ($items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>, $value: boolean) : void
            public SetResourcesFavorite ($items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>, $value: boolean) : void
            public OpenResource ($pi: FairyEditor.FPackageItem) : void
            public OpenResources ($pis: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : void
            public RevealInExplorer ($pi: FairyEditor.FPackageItem) : void
            public ShowUpdateResourceDialog ($pi: FairyEditor.FPackageItem) : void
            public ShowImportResourcesDialog ($pkg?: FairyEditor.FPackage, $toPath?: string) : void
            public AddPackageToGroup ($pkg: FairyEditor.FPackage) : void
            public GetPackagesInGroup ($group: string, $result: System.Collections.Generic.List$1<FairyEditor.FPackage>) : System.Collections.Generic.List$1<FairyEditor.FPackage>
            public constructor ()
        }
        class InspectorView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public get visibleInspectors(): System.Collections.Generic.List$1<FairyEditor.View.IInspector>;
            public get scrollPos(): number;
            public set scrollPos(value: number);
            public GetInspector ($name: string) : FairyEditor.View.IInspector
            public AddInspector ($type: System.Type, $name: string, $title: string) : void
            public AddInspector ($luaTable: XLua.LuaTable, $name: string, $title: string) : void
            public AddInspector ($factoryMethod: System.Func$1<FairyEditor.View.PluginInspector>, $name: string, $title: string) : void
            public RemoveInspector ($name: string) : void
            public RemoveAllPluginInspectors () : void
            public SetTitle ($name: string, $title: string) : void
            public Show ($name: string) : void
            public Show ($names: System.Array$1<string>) : void
            public Show ($names: System.Collections.Generic.List$1<string>) : void
            public UpdateInspector ($inst: FairyEditor.View.IInspector) : void
            public ShowPopup ($name: string, $target: FairyGUI.GObject, $dir?: FairyGUI.PopupDirection, $closeUntilMouseUp?: boolean) : void
            public Refresh ($name: string) : void
            public Clear () : void
            public constructor ()
        }
        class TestView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public get running(): boolean;
            public Start () : void
            public Reload () : void
            public Stop () : void
            public PlayTransition ($name: string) : void
            public TogglePopup ($popup: FairyEditor.FObject, $target?: FairyEditor.FObject, $direction?: string) : void
            public ShowPopup ($popup: FairyEditor.FObject, $target?: FairyEditor.FObject, $direction?: string) : void
            public HidePopup () : void
            public ShowTooltips ($msg: string) : void
            public HideTooltips () : void
            public constructor ()
        }
        class TimelineView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public Refresh ($transItem?: FairyEditor.FTransitionItem) : void
            public SelectKeyFrame ($transItem: FairyEditor.FTransitionItem) : void
            public GetSelection () : FairyEditor.FTransitionItem
            public GetSelections ($result: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>) : void
            public constructor ()
        }
        class ConsoleView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public Log ($msg: string) : void
            public Log ($logType: UnityEngine.LogType, $msg: string) : void
            public LogError ($msg: string) : void
            public LogError ($msg: string, $err?: System.Exception) : void
            public LogWarning ($msg: string) : void
            public Clear () : void
            public constructor ()
        }
        class DocumentFactory extends System.Object implements FairyEditor.View.IDocumentFactory
        {
            public get contextMenu(): FairyEditor.Component.IMenu;
            public CreateDocument ($docURL: string) : FairyEditor.View.IDocument
            public InvokeDocumentMethod ($methodName: string, $args?: System.Array$1<any>) : void
            public ConnectInspector ($inspectorName: string) : void
            public ConnectInspector ($inspectorName: string, $forObjectType: string, $forEmptySelection: boolean, $forTimelineMode: boolean) : void
            public disconnectInspector ($inspectorName: string) : void
            public constructor ()
            public CreateDocument ($url: string) : FairyEditor.View.IDocument
        }
        interface IDocumentFactory
        {
            CreateDocument ($url: string) : FairyEditor.View.IDocument
        }
        class Document extends System.Object implements FairyEditor.View.IDocument
        {
            public get panel(): FairyGUI.GComponent;
            public get selectionLayer(): FairyGUI.Container;
            public get inspectingTarget(): FairyEditor.FObject;
            public get inspectingTargets(): System.Collections.Generic.IList$1<FairyEditor.FObject>;
            public get inspectingObjectType(): string;
            public get packageItem(): FairyEditor.FPackageItem;
            public get content(): FairyEditor.FComponent;
            public get displayTitle(): string;
            public get displayIcon(): string;
            public get history(): FairyEditor.View.ActionHistory;
            public get docURL(): string;
            public get isModified(): boolean;
            public get savedVersion(): number;
            public get openedGroup(): FairyEditor.FObject;
            public get isPickingObject(): boolean;
            public get timelineMode(): boolean;
            public get editingTransition(): FairyEditor.FTransition;
            public get head(): number;
            public set head(value: number);
            public Open ($pi: FairyEditor.FPackageItem) : void
            public DiscardChanges () : void
            public OnEnable () : void
            public OnDisable () : void
            public OnDestroy () : void
            public OnValidate () : void
            public SetMeta ($key: string, $value: any) : void
            public OnUpdate () : void
            public GetInspectingTargetCount ($objectType: string) : number
            public SetModified ($value?: boolean) : void
            public Serialize () : FairyGUI.Utils.XML
            public Save () : void
            public OnViewSizeChanged () : void
            public OnViewScaleChanged () : void
            public OnViewBackgroundChanged () : void
            public SelectObject ($obj: FairyEditor.FObject, $scrollToView?: boolean, $allowOpenGroups?: boolean) : void
            public SelectAll () : void
            public GetSelection () : System.Collections.Generic.IList$1<FairyEditor.FObject>
            public UnselectObject ($obj: FairyEditor.FObject) : void
            public UnselectAll () : void
            public SetSelection ($obj: FairyEditor.FObject) : void
            public SetSelection ($objs: System.Collections.Generic.IList$1<FairyEditor.FObject>) : void
            public CopySelection () : void
            public DeleteSelection () : void
            public DeleteGroupContent ($group: FairyEditor.FGroup) : void
            public MoveSelection ($dx: number, $dy: number) : void
            public GlobalToCanvas ($stagePos: UnityEngine.Vector2) : UnityEngine.Vector2
            public GetCenterPos () : UnityEngine.Vector2
            public Paste ($pos?: System.Nullable$1<UnityEngine.Vector2>, $pasteToCenter?: boolean) : void
            public ReplaceSelection ($url: string) : void
            public OpenChild ($target: FairyEditor.FObject) : void
            public StartInlineEdit ($target: FairyEditor.FTextField) : void
            public ShowContextMenu () : void
            public UpdateEditMenu ($editMeu: FairyEditor.Component.IMenu) : void
            public InsertObject ($url: string, $pos?: System.Nullable$1<UnityEngine.Vector2>, $insertIndex?: number) : FairyEditor.FObject
            public RemoveObject ($obj: FairyEditor.FObject) : void
            public AdjustDepth ($index: number) : void
            public CreateGroup () : void
            public DestroyGroup () : void
            public OpenGroup ($group: FairyEditor.FObject) : void
            public CloseGroup ($depth?: number) : void
            public NotifyGroupRemoved ($group: FairyEditor.FGroup) : void
            public HandleHotkey ($hotkeyId: string) : void
            public PickObject ($initValue: FairyEditor.FObject, $callback: System.Action$1<FairyEditor.FObject>, $validator?: System.Func$2<FairyEditor.FObject, boolean>, $cancelCallback?: System.Action) : void
            public CancelPickObject () : void
            public EnterTimelineMode ($name: string) : void
            public ExitTimelineMode () : void
            public RefreshTransition () : void
            public RefreshInspectors ($flag?: number) : void
            public GetOutlineLocks ($obj: FairyEditor.FObject) : number
            public SetTransitionProperty ($trans: FairyEditor.FTransition, $propName: string, $propValue: any) : void
            public SetKeyFrameProperty ($item: FairyEditor.FTransitionItem, $propName: string, $propValue: any) : void
            public SetKeyFrameValue ($item: FairyEditor.FTransitionItem, ...values: any[]) : void
            public SetKeyFramePathPos ($item: FairyEditor.FTransitionItem, $pointIndex: number, $x: number, $y: number) : void
            public SetKeyFrameControlPointPos ($item: FairyEditor.FTransitionItem, $pointIndex: number, $controlIndex: number, $x: number, $y: number) : void
            public SetKeyFrameControlPointSmooth ($item: FairyEditor.FTransitionItem, $pointIndex: number, $smooth: boolean) : void
            public SetKeyFrame ($targetId: string, $type: string, $frame: number) : void
            public AddKeyFrames ($keyFrames: System.Collections.Generic.IEnumerable$1<FairyEditor.FTransitionItem>) : void
            public CreateKeyFrame ($transType: string) : void
            public CreateKeyFrame ($child: FairyEditor.FObject, $type: string) : FairyEditor.FTransitionItem
            public AddKeyFrame ($item: FairyEditor.FTransitionItem) : void
            public AddKeyFrames ($items: System.Array$1<FairyEditor.FTransitionItem>) : void
            public RemoveKeyFrame ($item: FairyEditor.FTransitionItem) : void
            public RemoveKeyFrames ($targetId: string, $type: string) : void
            public UpdateTransition ($xml: FairyGUI.Utils.XML) : void
            public AddTransition ($name?: string) : FairyEditor.FTransition
            public RemoveTransition ($name: string) : void
            public DuplicateTransition ($name: string, $newInstanceName?: string) : FairyEditor.FTransition
            public UpdateTransitions ($data: FairyGUI.Utils.XML) : void
            public AddController ($data: FairyGUI.Utils.XML) : void
            public UpdateController ($controllerName: string, $data: FairyGUI.Utils.XML) : void
            public RemoveController ($controllerName: string) : void
            public SwitchPage ($controllerName: string, $index: number) : number
            public constructor ()
            public UpdateEditMenu ($editMenu: FairyEditor.Component.IMenu) : void
        }
        interface IDocument
        {
            panel : FairyGUI.GComponent
            packageItem : FairyEditor.FPackageItem
            docURL : string
            displayTitle : string
            displayIcon : string
            isModified : boolean
            Save () : void
            DiscardChanges () : void
            UpdateEditMenu ($editMenu: FairyEditor.Component.IMenu) : void
            HandleHotkey ($hotkeyId: string) : void
            OnEnable () : void
            OnDisable () : void
            OnValidate () : void
            OnUpdate () : void
            OnDestroy () : void
            OnViewSizeChanged () : void
            OnViewScaleChanged () : void
            OnViewBackgroundChanged () : void
        }
        class DocElement extends System.Object
        {
            public get owner(): FairyEditor.View.Document;
            public get isRoot(): boolean;
            public get isValid(): boolean;
            public get relationsDisabled(): boolean;
            public get displayIcon(): string;
            public get selected(): boolean;
            public set selected(value: boolean);
            public get gizmo(): FairyEditor.View.Gizmo;
            public SetProperty ($propName: string, $propValue: any) : void
            public SetGearProperty ($gearIndex: number, $propName: string, $propValue: any) : void
            public UpdateGears ($data: FairyGUI.Utils.XML) : void
            public SetRelation ($target: FairyEditor.FObject, $desc: string) : void
            public RemoveRelation ($target: FairyEditor.FObject) : void
            public UpdateRelations ($data: FairyGUI.Utils.XML) : void
            public SetExtensionProperty ($propName: string, $propValue: any) : void
            public SetChildProperty ($target: string, $propertyId: number, $propValue: any) : void
            public SetVertexPosition ($pointIndex: number, $x: number, $y: number) : void
            public SetVertexDistance ($pointIndex: number, $distance: number) : void
            public SetScriptData ($name: string, $value: string) : void
            public constructor ($doc: FairyEditor.View.Document, $obj: FairyEditor.FObject, $isRoot?: boolean)
            public constructor ()
        }
        interface IActionHistoryItem
        {
            isPersists : boolean
            Process ($owner: FairyEditor.View.Document) : boolean
        }
        class ActionHistory extends System.Object
        {
            public get processing(): boolean;
            public CanUndo () : boolean
            public CanRedo () : boolean
            public Add ($item: FairyEditor.View.IActionHistoryItem) : void
            public GetPendingList () : System.Collections.Generic.List$1<FairyEditor.View.IActionHistoryItem>
            public Reset () : void
            public PushHistory () : void
            public PopHistory () : void
            public Undo () : boolean
            public Redo () : boolean
            public constructor ($doc: FairyEditor.View.Document)
            public constructor ()
        }
        class Gizmo extends FairyGUI.Container implements FairyGUI.IEventDispatcher
        {
            public static RESIZE_HANDLE : number
            public static VERTEX_HANDLE : number
            public static PATH_HANDLE : number
            public static CONTROL_HANDLE : number
            public static HANDLE_SIZE : number
            public static OUTLINE_COLOR : UnityEngine.Color
            public static OUTLINE_COLOR_COM : UnityEngine.Color
            public static OUTLINE_COLOR_GROUP : UnityEngine.Color
            public static PATH_COLOR : UnityEngine.Color
            public static TANGENT_COLOR : UnityEngine.Color
            public static VERTEX_HANDLE_COLOR : UnityEngine.Color
            public static PATH_HANDLE_COLOR : UnityEngine.Color
            public static CONTROLL_HANDLE_COLOR : UnityEngine.Color
            public get owner(): FairyEditor.FObject;
            public get activeHandleIndex(): number;
            public get activeHandleType(): number;
            public get verticesEditing(): boolean;
            public get keyFrame(): FairyEditor.FTransitionItem;
            public get activeHandle(): FairyEditor.View.GizmoHandle;
            public set activeHandle(value: FairyEditor.View.GizmoHandle);
            public EditVertices () : void
            public EditPath ($keyFrame: FairyEditor.FTransitionItem) : void
            public EditComplete () : void
            public Refresh ($immediately?: boolean) : void
            public ShowDecorations ($visible: boolean) : void
            public SetSelected ($value: boolean) : void
            public OnUpdate () : void
            public OnDragStart ($context: FairyGUI.EventContext) : void
            public OnDragMove ($context: FairyGUI.EventContext) : void
            public OnDragEnd ($context: FairyGUI.EventContext) : void
            public constructor ($doc: FairyEditor.View.Document, $owner: FairyEditor.FObject)
            public constructor ()
            public constructor ($gameObjectName: string)
            public constructor ($attachTarget: UnityEngine.GameObject)
        }
        class GizmoHandle extends FairyGUI.Shape implements FairyGUI.IEventDispatcher
        {
            public index : number
            public get type(): number;
            public get selected(): boolean;
            public set selected(value: boolean);
            public constructor ($type: number, $color: UnityEngine.Color, $shape?: number)
            public constructor ()
        }
        class GizmoHandleSet extends System.Object
        {
            public ResetIndex () : void
            public GetHandle () : FairyEditor.View.GizmoHandle
            public RemoveSpares () : void
            public constructor ($manager: FairyGUI.DisplayObject, $type: number, $color: UnityEngine.Color, $shape?: number)
            public constructor ()
        }
        class GridMesh extends System.Object implements FairyGUI.IMeshFactory
        {
            public gridSize : number
            public offset : UnityEngine.Vector2
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
        }
        class InspectorUpdateFlags extends System.Object
        {
            public static COMMON : number
            public static TRANSFORM : number
            public static GEAR : number
            public static RELATION : number
            public static GIZMO : number
            public static FlagsByName : System.Collections.Generic.Dictionary$2<string, number>
        }
        class PathLineMesh extends System.Object implements FairyGUI.IMeshFactory
        {
            public pathLine : FairyGUI.LineMesh
            public controlLines : System.Collections.Generic.List$1<FairyGUI.StraightLineMesh>
            public controlLineCount : number
            public GetControlLine () : FairyGUI.StraightLineMesh
            public OnPopulateMesh ($vb: FairyGUI.VertexBuffer) : void
            public constructor ()
        }
        class DocCamera extends UnityEngine.MonoBehaviour
        {
            public cachedTransform : UnityEngine.Transform
            public cachedCamera : UnityEngine.Camera
            public owner : FairyGUI.GComponent
            public constructor ()
        }
        class FavoritesView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public constructor ()
        }
        class HierarchyView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public constructor ()
        }
        interface IInspector
        {
            panel : FairyGUI.GComponent
            UpdateUI () : boolean
            Dispose () : void
        }
        class PluginInspector extends System.Object implements FairyEditor.View.IInspector
        {
            public updateAction : System.Func$1<boolean>
            public disposeAction : System.Action
            public get panel(): FairyGUI.GComponent;
            public set panel(value: FairyGUI.GComponent);
            public UpdateUI () : boolean
            public Dispose () : void
            public constructor ()
        }
        class MainMenu extends System.Object
        {
            public get root(): FairyEditor.Component.IMenu;
            public AddStartSceneMenu () : void
            public AddProjectMenu () : void
            public constructor ($root: FairyEditor.Component.IMenu)
            public constructor ()
        }
        class PlugInView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public constructor ()
        }
        class PreviewView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public Show ($pi?: FairyEditor.FPackageItem) : void
            public constructor ()
        }
        class ProjectView extends System.Object
        {
            public onContextMenu : FairyEditor.View.ProjectView.OnContextMenuDelegate
            public onGetItemListing : FairyEditor.View.ProjectView.OnGetItemListingDelegate
            public allowDrag : boolean
            public get project(): FairyEditor.FProject;
            public set project(value: FairyEditor.FProject);
            public get treeView(): FairyGUI.GTree;
            public get listView(): FairyGUI.GList;
            public set showListView(value: boolean);
            public SetChanged ($pi: FairyEditor.FPackageItem) : boolean
            public SetTreeChanged ($pi: FairyEditor.FPackageItem, $recursive?: boolean, $applyImmediately?: boolean) : void
            public GetSelectedPackage () : FairyEditor.FPackage
            public GetSelectedFolder () : FairyEditor.FPackageItem
            public GetSelectedResource () : FairyEditor.FPackageItem
            public GetSelectedResources ($result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : System.Collections.Generic.List$1<FairyEditor.FPackageItem>
            public GetFolderUnderPoint ($globalPos: UnityEngine.Vector2, $touchTarget: FairyGUI.GObject) : FairyEditor.FPackageItem
            public GetExpandedFolders ($parentNode?: FairyGUI.GTreeNode, $result?: System.Collections.Generic.List$1<string>) : System.Collections.Generic.List$1<string>
            public SetExpanedFolders ($arr: System.Collections.IList) : void
            public IsInView ($pi: FairyEditor.FPackageItem) : boolean
            public Select ($pi: FairyEditor.FPackageItem) : boolean
            public SelectNextTo ($pi: FairyEditor.FPackageItem) : void
            public Expand ($pi: FairyEditor.FPackageItem) : void
            public Rename ($pi?: FairyEditor.FPackageItem) : void
            public Open () : void
            public ChangeIconSize ($scale: number) : void
            public constructor ($proj: FairyEditor.FProject, $tree: FairyGUI.GTree, $sep?: FairyGUI.GObject, $list?: FairyGUI.GList)
            public constructor ()
        }
        class ReferenceView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public Open ($source: string) : void
            public FillMenuTargets () : void
            public constructor ()
        }
        class ResourceMenu extends System.Object
        {
            public get realMenu(): FairyEditor.Component.IMenu;
            public get targetItems(): System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public Show () : void
            public constructor ()
        }
        class SearchView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public FillMenuTargets () : void
            public constructor ()
        }
        class TransitionListView extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public Refresh () : void
            public constructor ()
        }
    }
    namespace FairyEditor.Component {
        interface IMenu
        {
            AddItem ($caption: string, $name: string, $selectCallback: System.Action$1<string>) : void
            AddItem ($caption: string, $name: string, $atIndex: number, $isSubMenu: boolean, $selectCallback: System.Action$1<string>) : void
            AddSeperator () : void
            AddSeperator ($atIndex: number) : void
            RemoveItem ($name: string) : void
            SetItemEnabled ($name: string, $enabled: boolean) : void
            SetItemChecked ($name: string, $checked: boolean) : void
            IsItemChecked ($name: string) : boolean
            SetItemText ($name: string, $text: string) : void
            ClearItems () : void
            GetSubMenu ($name: string) : FairyEditor.Component.IMenu
            Invoke ($name: string) : void
            Dispose () : void
        }
        class ViewGrid extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public uid : string
            public get showTabs(): boolean;
            public set showTabs(value: boolean);
            public get numViews(): number;
            public get selectedIndex(): number;
            public set selectedIndex(value: number);
            public get selectedView(): FairyGUI.GComponent;
            public set selectedView(value: FairyGUI.GComponent);
            public GetViewAt ($index: number) : FairyGUI.GComponent
            public AddView ($view: FairyGUI.GComponent) : void
            public AddViewAt ($view: FairyGUI.GComponent, $index: number) : void
            public RemoveView ($view: FairyGUI.GComponent) : void
            public RemoveViewAt ($index: number) : void
            public SetViewIndex ($view: FairyGUI.GComponent, $index: number) : void
            public GetViewIndex ($view: FairyGUI.GComponent) : number
            public GetViewIndexById ($viewId: string) : number
            public ContainsView ($ids: System.Array$1<string>) : boolean
            public MoveViews ($anotherGrid: FairyEditor.Component.ViewGrid) : void
            public Clear () : void
            public Refresh () : void
            public SetViewTitle ($index: number, $title: string) : void
            public constructor ()
        }
        class ChildObjectInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public typeFilter : System.Array$1<string>
            public get value(): FairyEditor.FObject;
            public set value(value: FairyEditor.FObject);
            public Start () : void
            public constructor ()
        }
        class ColorInput extends FairyGUI.GButton implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public showAlpha : boolean
            public get colorValue(): UnityEngine.Color;
            public set colorValue(value: UnityEngine.Color);
            public constructor ()
        }
        class ColorPicker extends System.Object
        {
            public get isShowing(): boolean;
            public Show ($input: FairyEditor.Component.ColorInput, $popupTarget: FairyGUI.GObject, $color: UnityEngine.Color, $showAlpha: boolean) : void
            public Hide () : void
            public constructor ()
        }
        class ComPropertyInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public get value(): any;
            public Update ($cp: FairyEditor.ComProperty, $pagesSupplier: any) : void
            public constructor ()
        }
        class ControllerInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public prompt : string
            public includeChildren : boolean
            public get owner(): FairyEditor.FComponent;
            public set owner(value: FairyEditor.FComponent);
            public get value(): string;
            public set value(value: string);
            public constructor ()
        }
        class ControllerMultiPageInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public prompt : string
            public get controller(): FairyEditor.FController;
            public set controller(value: FairyEditor.FController);
            public get value(): System.Array$1<string>;
            public set value(value: System.Array$1<string>);
            public constructor ()
        }
        class ControllerPageInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public prompt : string
            public nullPageOption : boolean
            public additionalOptions : boolean
            public get controller(): FairyEditor.FController;
            public set controller(value: FairyEditor.FController);
            public get value(): string;
            public set value(value: string);
            public constructor ()
        }
        class EditableListItem extends FairyEditor.Component.ListItem implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public sign : FairyGUI.GLoader
            public get editable(): boolean;
            public set editable(value: boolean);
            public get toggleClickCount(): number;
            public set toggleClickCount(value: number);
            public StartEditing ($text?: string) : void
            public constructor ()
        }
        class ListItem extends FairyGUI.GButton implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public titleObj : FairyGUI.GTextField
            public iconObj : FairyGUI.GLoader
            public constructor ()
        }
        class EditableTreeItem extends FairyGUI.GButton implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public toggleClickCount : number
            public get editable(): boolean;
            public set editable(value: boolean);
            public StartEditing ($text?: string) : void
            public constructor ()
        }
        class FontInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public constructor ()
        }
        class FontSizeInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public get value(): number;
            public set value(value: number);
            public get max(): number;
            public set max(value: number);
            public constructor ()
        }
        class InputElement extends System.ValueType
        {
            public name : string
            public type : string
            public prop : string
            public dummy : boolean
            public extData : any
            public min : FairyEditor.Component.InputElement.OptionalValue$1<number>
            public max : FairyEditor.Component.InputElement.OptionalValue$1<number>
            public step : FairyEditor.Component.InputElement.OptionalValue$1<number>
            public precision : FairyEditor.Component.InputElement.OptionalValue$1<number>
            public items : System.Array$1<string>
            public values : System.Array$1<string>
            public visibleItemCount : FairyEditor.Component.InputElement.OptionalValue$1<number>
            public valueName : string
            public inverted : boolean
            public showAlpha : boolean
            public filter : System.Array$1<string>
            public pages : string
            public includeChildren : boolean
            public prompt : string
            public readonly : boolean
            public disableIME : boolean
            public trim : boolean
        }
        class FormHelper extends System.Object
        {
            public onPropChanged : FairyEditor.Component.FormHelper.OnPropChangedDelegate
            public get owner(): FairyGUI.GComponent;
            public BindControls ($data: System.Collections.Generic.IList$1<FairyEditor.Component.InputElement>) : void
            public GetControl ($controlName: string) : FairyGUI.GObject
            public UpdateValuesFrom ($obj: any, $controlNames?: System.Collections.IList) : void
            public SetValue ($controlName: string, $value: any) : void
            public GetValue ($controlName: string) : any
            public UpdateUI () : void
            public constructor ($owner: FairyGUI.GComponent)
            public constructor ()
        }
        class InlineSearchBar extends FairyGUI.GButton implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public get pattern(): System.Text.RegularExpressions.Regex;
            public Reset () : void
            public HandleKeyEvent ($evt: FairyGUI.InputEvent) : boolean
            public constructor ()
        }
        class LinkButton extends FairyGUI.GButton implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public constructor ()
        }
        class ListHelper extends System.Object
        {
            public onInsert : System.Action$2<number, FairyGUI.GComponent>
            public onRemove : System.Action$1<number>
            public onSwap : System.Action$2<number, number>
            public Add ($context?: FairyGUI.EventContext) : void
            public Insert ($context?: FairyGUI.EventContext) : void
            public Remove ($context?: FairyGUI.EventContext) : void
            public MoveUp ($context?: FairyGUI.EventContext) : void
            public MoveDown ($context?: FairyGUI.EventContext) : void
            public constructor ($list: FairyGUI.GList, $indexColumn?: string)
            public constructor ()
        }
        class ListItemInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public toggleClickCount : number
            public get editable(): boolean;
            public set editable(value: boolean);
            public StartEditing ($text?: string) : void
            public constructor ()
        }
        class ListItemResourceInput extends FairyEditor.Component.ResourceInput implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public toggleClickCount : number
            public StartEditing () : void
            public constructor ()
        }
        class ResourceInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public promptText : string
            public isFontInput : boolean
            public set text(value: string);
            public constructor ()
        }
        class MenuBar extends System.Object implements FairyEditor.Component.IMenu
        {
            public Dispose () : void
            public AddItem ($caption: string, $name: string, $selectCallback: System.Action$1<string>) : void
            public AddItem ($caption: string, $name: string, $atIndex: number, $isSubMenu: boolean, $selectCallback: System.Action$1<string>) : void
            public GetSubMenu ($name: string) : FairyEditor.Component.IMenu
            public RemoveItem ($name: string) : void
            public AddSeperator () : void
            public AddSeperator ($atIndex: number) : void
            public SetItemEnabled ($name: string, $enabled: boolean) : void
            public SetItemChecked ($name: string, $checked: boolean) : void
            public IsItemChecked ($name: string) : boolean
            public SetItemText ($name: string, $text: string) : void
            public ClearItems () : void
            public Invoke ($name: string) : void
            public constructor ($panel: FairyGUI.GComponent)
            public constructor ()
        }
        class NativeMenu extends System.Object implements FairyEditor.Component.IMenu
        {
            public static applicationMenu : FairyEditor.Component.NativeMenu
            public static dockIconMenu : FairyEditor.Component.NativeMenu
            public Dispose () : void
            public AddItem ($caption: string, $name: string, $selectCallback: System.Action$1<string>) : void
            public AddItem ($caption: string, $name: string, $atIndex: number, $isSubMenu: boolean, $selectCallback: System.Action$1<string>) : void
            public AddSeperator () : void
            public AddSeperator ($atIndex: number) : void
            public SetItemEnabled ($name: string, $enabled: boolean) : void
            public SetItemChecked ($name: string, $checked: boolean) : void
            public IsItemChecked ($name: string) : boolean
            public SetItemText ($name: string, $text: string) : void
            public GetSubMenu ($name: string) : FairyEditor.Component.IMenu
            public RemoveItem ($name: string) : void
            public ClearItems () : void
            public Invoke ($name: string) : void
        }
        class NumericInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public get max(): number;
            public set max(value: number);
            public get min(): number;
            public set min(value: number);
            public get value(): number;
            public set value(value: number);
            public get step(): number;
            public set step(value: number);
            public get fractionDigits(): number;
            public set fractionDigits(value: number);
            public set text(value: string);
            public constructor ()
        }
        class SelectAnimationMenu extends System.Object
        {
            public static GetInstance () : FairyEditor.Component.SelectAnimationMenu
            public Show ($input: FairyGUI.GObject, $target: FairyEditor.FLoader3D, $popupTarget?: FairyGUI.GObject) : void
            public constructor ()
        }
        class SelectDesignImageMenu extends System.Object
        {
            public static GetInstance () : FairyEditor.Component.SelectDesignImageMenu
            public Show ($input: FairyGUI.GObject, $popupTarget?: FairyGUI.GObject) : void
            public constructor ()
        }
        class SelectPivotMenu extends System.Object
        {
            public static GetInstance () : FairyEditor.Component.SelectPivotMenu
            public Show ($input1: FairyGUI.GObject, $input2: FairyGUI.GObject, $popupTarget?: FairyGUI.GObject) : void
            public constructor ()
        }
        class SelectSkinMenu extends System.Object
        {
            public static GetInstance () : FairyEditor.Component.SelectSkinMenu
            public Show ($input: FairyGUI.GObject, $target: FairyEditor.FLoader3D, $popupTarget?: FairyGUI.GObject) : void
            public constructor ()
        }
        class TextArea extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public constructor ()
        }
        class TextInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public set text(value: string);
            public constructor ()
        }
        class TransitionInput extends FairyGUI.GLabel implements FairyGUI.IEventDispatcher, FairyGUI.IColorGear
        {
            public prompt : string
            public includeChildren : boolean
            public get owner(): FairyEditor.FComponent;
            public set owner(value: FairyEditor.FComponent);
            public get value(): string;
            public set value(value: string);
            public constructor ()
        }
        class ViewGridGroup extends FairyGUI.GComponent implements FairyGUI.IEventDispatcher
        {
            public uid : string
            public get layout(): FairyGUI.GroupLayoutType;
            public get numGrids(): number;
            public AddGrid ($child: FairyGUI.GObject) : void
            public AddGridAt ($child: FairyGUI.GObject, $index: number) : void
            public ResetChildrenSize () : void
            public RemoveGrid ($child: FairyGUI.GObject, $dispose?: boolean) : void
            public ReplaceGrid ($oldChild: FairyGUI.GObject, $newChild: FairyGUI.GObject) : void
            public MoveGrids ($anotherGroup: FairyEditor.Component.ViewGridGroup, $index: number) : void
            public GetGridAt ($index: number) : FairyGUI.GObject
            public GetGridIndex ($grid: FairyGUI.GObject) : number
            public FindGrid ($view: FairyGUI.GComponent, $recursive?: boolean) : FairyEditor.Component.ViewGrid
            public FindGridById ($id: string, $recursive?: boolean) : FairyEditor.Component.ViewGrid
            public FindGridByIds ($ids: System.Array$1<string>, $recursive?: boolean) : FairyEditor.Component.ViewGrid
            public FindGroup ($id: string) : FairyEditor.Component.ViewGridGroup
            public static EachGrid ($grp: FairyEditor.Component.ViewGridGroup, $recursive: boolean, $callback: FairyEditor.Component.ViewGridGroup.EachGridCallback) : FairyEditor.Component.ViewGrid
            public constructor ($layout: FairyGUI.GroupLayoutType)
            public constructor ()
        }
    }
    namespace FairyEditor.App {
        enum FrameRateFactor
        { BackgroundJob = 1, NativeDragDrop = 2, DraggingObject = 256, Testing = 512 }
    }
    namespace FairyGUI.Utils {
        class XML extends System.Object
        {
            public name : string
            public text : string
            public get attributes(): System.Collections.Generic.Dictionary$2<string, string>;
            public get elements(): FairyGUI.Utils.XMLList;
            public static Create ($tag: string) : FairyGUI.Utils.XML
            public HasAttribute ($attrName: string) : boolean
            public GetAttribute ($attrName: string) : string
            public GetAttribute ($attrName: string, $defValue: string) : string
            public GetAttributeInt ($attrName: string) : number
            public GetAttributeInt ($attrName: string, $defValue: number) : number
            public GetAttributeFloat ($attrName: string) : number
            public GetAttributeFloat ($attrName: string, $defValue: number) : number
            public GetAttributeBool ($attrName: string) : boolean
            public GetAttributeBool ($attrName: string, $defValue: boolean) : boolean
            public GetAttributeArray ($attrName: string) : System.Array$1<string>
            public GetAttributeArray ($attrName: string, $seperator: number) : System.Array$1<string>
            public GetAttributeColor ($attrName: string, $defValue: UnityEngine.Color) : UnityEngine.Color
            public GetAttributeVector ($attrName: string) : UnityEngine.Vector2
            public SetAttribute ($attrName: string, $attrValue: string) : void
            public SetAttribute ($attrName: string, $attrValue: boolean) : void
            public SetAttribute ($attrName: string, $attrValue: number) : void
            public RemoveAttribute ($attrName: string) : void
            public GetNode ($selector: string) : FairyGUI.Utils.XML
            public Elements () : FairyGUI.Utils.XMLList
            public Elements ($selector: string) : FairyGUI.Utils.XMLList
            public GetEnumerator () : FairyGUI.Utils.XMLList.Enumerator
            public GetEnumerator ($selector: string) : FairyGUI.Utils.XMLList.Enumerator
            public AppendChild ($child: FairyGUI.Utils.XML) : void
            public RemoveChild ($child: FairyGUI.Utils.XML) : void
            public RemoveChildren ($selector: string) : void
            public Parse ($aSource: string) : void
            public Reset () : void
            public ToXMLString ($includeHeader: boolean) : string
            public constructor ($XmlString: string)
            public constructor ()
        }
        interface XML {
            GetAttributeArray ($attrName: string, $i1: $Ref<number>, $i2: $Ref<number>) : boolean;
            GetAttributeArray ($attrName: string, $i1: $Ref<number>, $i2: $Ref<number>, $i3: $Ref<number>, $i4: $Ref<number>) : boolean;
            GetAttributeArray ($attrName: string, $f1: $Ref<number>, $f2: $Ref<number>, $f3: $Ref<number>, $f4: $Ref<number>) : boolean;
            GetAttributeArray ($attrName: string, $f1: $Ref<number>, $f2: $Ref<number>) : boolean;
            GetAttributeArray ($attrName: string, $s1: $Ref<string>, $s2: $Ref<string>) : boolean;
        }
        class ByteBuffer extends System.Object
        {
            public littleEndian : boolean
            public stringTable : System.Array$1<string>
            public version : number
            public get position(): number;
            public set position(value: number);
            public get length(): number;
            public get bytesAvailable(): boolean;
            public get buffer(): System.Array$1<number>;
            public set buffer(value: System.Array$1<number>);
            public Skip ($count: number) : number
            public ReadByte () : number
            public ReadBytes ($output: System.Array$1<number>, $destIndex: number, $count: number) : System.Array$1<number>
            public ReadBytes ($count: number) : System.Array$1<number>
            public ReadBuffer () : FairyGUI.Utils.ByteBuffer
            public ReadChar () : number
            public ReadBool () : boolean
            public ReadShort () : number
            public ReadUshort () : number
            public ReadInt () : number
            public ReadUint () : number
            public ReadFloat () : number
            public ReadLong () : bigint
            public ReadDouble () : number
            public ReadString () : string
            public ReadString ($len: number) : string
            public ReadS () : string
            public ReadSArray ($cnt: number) : System.Array$1<string>
            public ReadPath () : System.Collections.Generic.List$1<FairyGUI.GPathPoint>
            public WriteS ($value: string) : void
            public ReadColor () : UnityEngine.Color
            public Seek ($indexTablePos: number, $blockIndex: number) : boolean
            public constructor ($data: System.Array$1<number>, $offset?: number, $length?: number)
            public constructor ()
        }
        interface IHtmlObject
        {
            width : number
            height : number
            displayObject : FairyGUI.DisplayObject
            element : FairyGUI.Utils.HtmlElement
            Create ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : void
            SetPosition ($x: number, $y: number) : void
            Add () : void
            Remove () : void
            Release () : void
            Dispose () : void
        }
        class HtmlElement extends System.Object
        {
            public type : FairyGUI.Utils.HtmlElementType
            public name : string
            public text : string
            public format : FairyGUI.TextFormat
            public charIndex : number
            public htmlObject : FairyGUI.Utils.IHtmlObject
            public status : number
            public space : number
            public position : UnityEngine.Vector2
            public get isEntity(): boolean;
            public Get ($attrName: string) : any
            public Set ($attrName: string, $attrValue: any) : void
            public GetString ($attrName: string) : string
            public GetString ($attrName: string, $defValue: string) : string
            public GetInt ($attrName: string) : number
            public GetInt ($attrName: string, $defValue: number) : number
            public GetFloat ($attrName: string) : number
            public GetFloat ($attrName: string, $defValue: number) : number
            public GetBool ($attrName: string) : boolean
            public GetBool ($attrName: string, $defValue: boolean) : boolean
            public GetColor ($attrName: string, $defValue: UnityEngine.Color) : UnityEngine.Color
            public FetchAttributes () : void
            public static GetElement ($type: FairyGUI.Utils.HtmlElementType) : FairyGUI.Utils.HtmlElement
            public static ReturnElement ($element: FairyGUI.Utils.HtmlElement) : void
            public static ReturnElements ($elements: System.Collections.Generic.List$1<FairyGUI.Utils.HtmlElement>) : void
            public constructor ()
        }
        interface IHtmlPageContext
        {
            CreateObject ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : FairyGUI.Utils.IHtmlObject
            FreeObject ($obj: FairyGUI.Utils.IHtmlObject) : void
            GetImageTexture ($image: FairyGUI.Utils.HtmlImage) : FairyGUI.NTexture
            FreeImageTexture ($image: FairyGUI.Utils.HtmlImage, $texture: FairyGUI.NTexture) : void
        }
        class HtmlPageContext extends System.Object implements FairyGUI.Utils.IHtmlPageContext
        {
            public static inst : FairyGUI.Utils.HtmlPageContext
            public CreateObject ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : FairyGUI.Utils.IHtmlObject
            public FreeObject ($obj: FairyGUI.Utils.IHtmlObject) : void
            public GetImageTexture ($image: FairyGUI.Utils.HtmlImage) : FairyGUI.NTexture
            public FreeImageTexture ($image: FairyGUI.Utils.HtmlImage, $texture: FairyGUI.NTexture) : void
            public constructor ()
        }
        class HtmlImage extends System.Object implements FairyGUI.Utils.IHtmlObject
        {
            public get loader(): FairyGUI.GLoader;
            public get displayObject(): FairyGUI.DisplayObject;
            public get element(): FairyGUI.Utils.HtmlElement;
            public get width(): number;
            public get height(): number;
            public Create ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : void
            public SetPosition ($x: number, $y: number) : void
            public Add () : void
            public Remove () : void
            public Release () : void
            public Dispose () : void
            public constructor ()
        }
        class HtmlParseOptions extends System.Object
        {
            public linkUnderline : boolean
            public linkColor : UnityEngine.Color
            public linkBgColor : UnityEngine.Color
            public linkHoverBgColor : UnityEngine.Color
            public ignoreWhiteSpace : boolean
            public static DefaultLinkUnderline : boolean
            public static DefaultLinkColor : UnityEngine.Color
            public static DefaultLinkBgColor : UnityEngine.Color
            public static DefaultLinkHoverBgColor : UnityEngine.Color
            public constructor ()
        }
        class HtmlButton extends System.Object implements FairyGUI.Utils.IHtmlObject
        {
            public static CLICK_EVENT : string
            public static resource : string
            public get button(): FairyGUI.GComponent;
            public get displayObject(): FairyGUI.DisplayObject;
            public get element(): FairyGUI.Utils.HtmlElement;
            public get width(): number;
            public get height(): number;
            public Create ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : void
            public SetPosition ($x: number, $y: number) : void
            public Add () : void
            public Remove () : void
            public Release () : void
            public Dispose () : void
            public constructor ()
        }
        enum HtmlElementType
        { Text = 0, Link = 1, Image = 2, Input = 3, Select = 4, Object = 5, LinkEnd = 6 }
        class HtmlInput extends System.Object implements FairyGUI.Utils.IHtmlObject
        {
            public static defaultBorderSize : number
            public static defaultBorderColor : UnityEngine.Color
            public static defaultBackgroundColor : UnityEngine.Color
            public get textInput(): FairyGUI.GTextInput;
            public get displayObject(): FairyGUI.DisplayObject;
            public get element(): FairyGUI.Utils.HtmlElement;
            public get width(): number;
            public get height(): number;
            public Create ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : void
            public SetPosition ($x: number, $y: number) : void
            public Add () : void
            public Remove () : void
            public Release () : void
            public Dispose () : void
            public constructor ()
        }
        class HtmlLink extends System.Object implements FairyGUI.Utils.IHtmlObject
        {
            public get displayObject(): FairyGUI.DisplayObject;
            public get element(): FairyGUI.Utils.HtmlElement;
            public get width(): number;
            public get height(): number;
            public Create ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : void
            public SetArea ($startLine: number, $startCharX: number, $endLine: number, $endCharX: number) : void
            public SetPosition ($x: number, $y: number) : void
            public Add () : void
            public Remove () : void
            public Release () : void
            public Dispose () : void
            public constructor ()
        }
        class HtmlParser extends System.Object
        {
            public static inst : FairyGUI.Utils.HtmlParser
            public Parse ($aSource: string, $defaultFormat: FairyGUI.TextFormat, $elements: System.Collections.Generic.List$1<FairyGUI.Utils.HtmlElement>, $parseOptions: FairyGUI.Utils.HtmlParseOptions) : void
            public constructor ()
        }
        class HtmlSelect extends System.Object implements FairyGUI.Utils.IHtmlObject
        {
            public static CHANGED_EVENT : string
            public static resource : string
            public get comboBox(): FairyGUI.GComboBox;
            public get displayObject(): FairyGUI.DisplayObject;
            public get element(): FairyGUI.Utils.HtmlElement;
            public get width(): number;
            public get height(): number;
            public Create ($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement) : void
            public SetPosition ($x: number, $y: number) : void
            public Add () : void
            public Remove () : void
            public Release () : void
            public Dispose () : void
            public constructor ()
        }
        class ToolSet extends System.Object
        {
            public static ConvertFromHtmlColor ($str: string) : UnityEngine.Color
            public static ColorFromRGB ($value: number) : UnityEngine.Color
            public static ColorFromRGBA ($value: number) : UnityEngine.Color
            public static CharToHex ($c: number) : number
            public static Intersection ($rect1: $Ref<UnityEngine.Rect>, $rect2: $Ref<UnityEngine.Rect>) : UnityEngine.Rect
            public static Union ($rect1: $Ref<UnityEngine.Rect>, $rect2: $Ref<UnityEngine.Rect>) : UnityEngine.Rect
            public static SkewMatrix ($matrix: $Ref<UnityEngine.Matrix4x4>, $skewX: number, $skewY: number) : void
            public static RotateUV ($uv: System.Array$1<UnityEngine.Vector2>, $baseUVRect: $Ref<UnityEngine.Rect>) : void
        }
        class UBBParser extends System.Object
        {
            public static inst : FairyGUI.Utils.UBBParser
            public defaultTagHandler : FairyGUI.Utils.UBBParser.TagHandler
            public handlers : System.Collections.Generic.Dictionary$2<string, FairyGUI.Utils.UBBParser.TagHandler>
            public defaultImgWidth : number
            public defaultImgHeight : number
            public GetTagText ($remove: boolean) : string
            public Parse ($text: string) : string
            public constructor ()
        }
        class XMLList extends System.Object
        {
            public rawList : System.Collections.Generic.List$1<FairyGUI.Utils.XML>
            public get Count(): number;
            public Add ($xml: FairyGUI.Utils.XML) : void
            public Clear () : void
            public get_Item ($index: number) : FairyGUI.Utils.XML
            public GetEnumerator () : FairyGUI.Utils.XMLList.Enumerator
            public GetEnumerator ($selector: string) : FairyGUI.Utils.XMLList.Enumerator
            public Filter ($selector: string) : FairyGUI.Utils.XMLList
            public Find ($selector: string) : FairyGUI.Utils.XML
            public RemoveAll ($selector: string) : void
            public constructor ()
            public constructor ($list: System.Collections.Generic.List$1<FairyGUI.Utils.XML>)
        }
        class XMLIterator extends System.Object
        {
            public static tagName : string
            public static tagType : FairyGUI.Utils.XMLTagType
            public static lastTagName : string
            public static Begin ($source: string, $lowerCaseName?: boolean) : void
            public static NextTag () : boolean
            public static GetTagSource () : string
            public static GetRawText ($trim?: boolean) : string
            public static GetText ($trim?: boolean) : string
            public static HasAttribute ($attrName: string) : boolean
            public static GetAttribute ($attrName: string) : string
            public static GetAttribute ($attrName: string, $defValue: string) : string
            public static GetAttributeInt ($attrName: string) : number
            public static GetAttributeInt ($attrName: string, $defValue: number) : number
            public static GetAttributeFloat ($attrName: string) : number
            public static GetAttributeFloat ($attrName: string, $defValue: number) : number
            public static GetAttributeBool ($attrName: string) : boolean
            public static GetAttributeBool ($attrName: string, $defValue: boolean) : boolean
            public static GetAttributes ($result: System.Collections.Generic.Dictionary$2<string, string>) : System.Collections.Generic.Dictionary$2<string, string>
            public static GetAttributes ($result: System.Collections.Hashtable) : System.Collections.Hashtable
            public constructor ()
        }
        enum XMLTagType
        { Start = 0, End = 1, Void = 2, CDATA = 3, Comment = 4, Instruction = 5 }
        class XMLUtils extends System.Object
        {
            public static DecodeString ($aSource: string) : string
            public static EncodeString ($sb: System.Text.StringBuilder, $start: number, $isAttribute?: boolean) : void
            public static EncodeString ($str: string, $isAttribute?: boolean) : string
            public constructor ()
        }
        class ZipReader extends System.Object
        {
            public get entryCount(): number;
            public GetNextEntry ($entry: FairyGUI.Utils.ZipReader.ZipEntry) : boolean
            public GetEntryData ($entry: FairyGUI.Utils.ZipReader.ZipEntry) : System.Array$1<number>
            public constructor ($data: System.Array$1<number>)
            public constructor ()
        }
    }
    namespace System.Threading.Tasks {
        class Task extends System.Object implements System.IAsyncResult, System.Threading.IThreadPoolWorkItem, System.IDisposable
        {
        }
        class Task$1<TResult> extends System.Threading.Tasks.Task implements System.IAsyncResult, System.Threading.IThreadPoolWorkItem, System.IDisposable
        {
        }
    }
    namespace System.Threading {
        interface IThreadPoolWorkItem
        {
        }
    }
    namespace FairyEditor.AniData {
        class Frame extends System.Object
        {
            public rect : UnityEngine.Rect
            public spriteIndex : number
            public delay : number
            public constructor ()
        }
        class FrameSprite extends System.Object
        {
            public texture : FairyGUI.NTexture
            public frameIndex : number
            public raw : System.Array$1<number>
            public constructor ()
        }
    }
    namespace FairyEditor.BmFontData {
        class Glyph extends System.ValueType
        {
            public id : number
            public x : number
            public y : number
            public xoffset : number
            public yoffset : number
            public width : number
            public height : number
            public xadvance : number
            public img : string
            public channel : number
        }
    }
    namespace FairyEditor.ComponentAsset {
        class DisplayListItem extends System.Object
        {
            public packageItem : FairyEditor.FPackageItem
            public pkg : FairyEditor.FPackage
            public type : string
            public desc : FairyGUI.Utils.XML
            public missingInfo : FairyEditor.MissingInfo
            public existingInstance : FairyEditor.FObject
            public constructor ()
        }
    }
    namespace FairyEditor.FontAsset {
        enum FontType
        { Sprites = 0, Fnt = 1, TTF = 2 }
    }
    namespace UnityEngine.TextCore.LowLevel {
        /** The rendering modes used by the Font Engine to render glyphs. */
        enum GlyphRenderMode
        { SMOOTH_HINTED = 4121, SMOOTH = 4117, RASTER_HINTED = 4122, RASTER = 4118, SDF = 4134, SDF8 = 8230, SDF16 = 16422, SDF32 = 32806, SDFAA_HINTED = 4169, SDFAA = 4165 }
    }
    namespace FairyEditor.Framework.Gears {
        interface IGear
        {
        }
    }
    namespace FairyEditor.FTree {
        interface TreeNodeRenderDelegate
        { (node: FairyEditor.FTreeNode, obj: FairyEditor.FComponent) : void; }
        var TreeNodeRenderDelegate: { new (func: (node: FairyEditor.FTreeNode, obj: FairyEditor.FComponent) => void): TreeNodeRenderDelegate; }
        interface TreeNodeWillExpandDelegate
        { (node: FairyEditor.FTreeNode, expand: boolean) : void; }
        var TreeNodeWillExpandDelegate: { new (func: (node: FairyEditor.FTreeNode, expand: boolean) => void): TreeNodeWillExpandDelegate; }
    }
    namespace FairyGUI.GPathPoint {
        enum CurveType
        { CRSpline = 0, Bezier = 1, CubicBezier = 2, Straight = 3 }
    }
    namespace FairyEditor.SpineCompatibilityHelper {
        class Delegates extends System.ValueType
        {
            public CreateRuntimeInstance : FairyEditor.SpineCompatibilityHelper.CreateRuntimeInstanceDelegate
            public ParseBounds : FairyEditor.SpineCompatibilityHelper.ParseBoundsDelegate
        }
        interface CreateRuntimeInstanceDelegate
        { (descAsset: UnityEngine.TextAsset, atlasTextAsset: UnityEngine.TextAsset, textures: System.Array$1<UnityEngine.Texture2D>, materialPropertySource: UnityEngine.Material, initialize: boolean) : FairyEditor.ISkeletonDataAsset; }
        var CreateRuntimeInstanceDelegate: { new (func: (descAsset: UnityEngine.TextAsset, atlasTextAsset: UnityEngine.TextAsset, textures: System.Array$1<UnityEngine.Texture2D>, materialPropertySource: UnityEngine.Material, initialize: boolean) => FairyEditor.ISkeletonDataAsset): CreateRuntimeInstanceDelegate; }
        interface ParseBoundsDelegate
        { (sourceFile: string) : UnityEngine.Rect; }
        var ParseBoundsDelegate: { new (func: (sourceFile: string) => UnityEngine.Rect): ParseBoundsDelegate; }
    }
    namespace Spine.Unity.SkeletonDataCompatibility {
        class VersionInfo extends System.Object
        {
        }
    }
    namespace System.IO.Compression {
        class ZipStorer extends System.Object implements System.IDisposable
        {
        }
    }
    namespace FairyEditor.PublishHandler {
        class ClassInfo extends System.Object
        {
            public className : string
            public superClassName : string
            public resId : string
            public resName : string
            public res : FairyEditor.FPackageItem
            public members : System.Collections.Generic.List$1<FairyEditor.PublishHandler.MemberInfo>
            public references : System.Collections.Generic.List$1<string>
            public constructor ()
        }
        class MemberInfo extends System.Object
        {
            public name : string
            public varName : string
            public type : string
            public index : number
            public group : number
            public res : FairyEditor.FPackageItem
            public constructor ()
        }
    }
    namespace FairyEditor.DependencyQuery {
        enum SeekLevel
        { SELECTION = 0, SAME_PACKAGE_BUT_NOT_EXPORTED = 1, SAME_PACKAGE = 2, ALL = 3 }
    }
    namespace FairyEditor.CopyHandler {
        enum OverrideOption
        { RENAME = 0, REPLACE = 1, SKIP = 2 }
    }
    namespace FairyEditor.HotkeyManager {
        class FunctionDef extends System.Object
        {
            public id : string
            public hotkey : string
            public desc : string
            public get isCustomized(): boolean;
            public constructor ($id: string, $hotkey: string, $desc: string)
            public constructor ()
        }
    }
    namespace FairyEditor.PluginManager {
        class PluginInfo extends System.Object
        {
            public name : string
            public displayName : string
            public description : string
            public version : string
            public author : FairyEditor.PluginManager.PluginInfo.Author
            public icon : string
            public main : string
            public onPublish : System.Action$1<FairyEditor.PublishHandler>
            public onDestroy : System.Action
            public constructor ()
        }
    }
    namespace FairyEditor.ReferenceInfo {
        enum ValueType
        { ID = 0, URL = 1, URL_COMPLEX = 2, CHAR_IMG = 3, ASSET_PROP = 4 }
    }
    namespace FairyEditor.AdaptationSettings {
        class DeviceInfo extends System.ValueType
        {
            public name : string
            public resolutionX : number
            public resolutionY : number
        }
    }
    namespace FairyEditor.CommonSettings {
        class ScrollBarConfig extends System.Object
        {
            public horizontal : string
            public vertical : string
            public defaultDisplay : string
            public constructor ()
        }
    }
    namespace FairyEditor.GlobalPublishSettings {
        class CodeGenerationConfig extends System.Object
        {
            public allowGenCode : boolean
            public codePath : string
            public classNamePrefix : string
            public memberNamePrefix : string
            public packageName : string
            public ignoreNoname : boolean
            public getMemberByName : boolean
            public codeType : string
            public constructor ()
        }
        class AtlasSetting extends System.Object
        {
            public maxSize : number
            public paging : boolean
            public sizeOption : string
            public forceSquare : boolean
            public allowRotation : boolean
            public trimImage : boolean
            public constructor ()
        }
    }
    namespace FairyEditor.I18nSettings {
        class LanguageFile extends System.Object
        {
            public name : string
            public path : string
            public fontName : string
            public modificationDate : Date
            public constructor ()
        }
    }
    namespace FairyEditor.PackageGroupSettings {
        class PackageGroup extends System.Object
        {
            public name : string
            public pkgs : System.Collections.Generic.List$1<string>
            public constructor ()
        }
    }
    namespace FairyEditor.AssetSizeUtil {
        class Result extends System.ValueType
        {
            public width : number
            public height : number
            public type : string
            public bitDepth : number
            public colorType : number
        }
    }
    namespace FairyEditor.FontUtil {
        class FontInfo extends System.Object
        {
            public family : string
            public localeFamily : string
            public file : string
            public externalLoad : boolean
            public constructor ()
        }
        class FontName extends System.Object
        {
            public family : string
            public localeFamily : string
            public constructor ()
        }
    }
    namespace SFB {
        class ExtensionFilter extends System.ValueType
        {
        }
    }
    namespace FairyEditor.VImage {
        enum Kernel
        { NEAREST = 0, LINEAR = 1, CUBIC = 2, MITCHELL = 3, LANCZOS2 = 4, LANCZOS3 = 5, LAST = 6 }
        enum Extend
        { BLACK = 0, COPY = 1, REPEAT = 2, MIRROR = 3, WHITE = 4, BACKGROUND = 5, LAST = 6 }
        enum BlendMode
        { CLEAR = 0, SOURCE = 1, OVER = 2, IN = 3, OUT = 4, ATOP = 5, DEST = 6, DEST_OVER = 7, DEST_IN = 8, DEST_OUT = 9, DEST_ATOP = 10, XOR = 11, ADD = 12, SATURATE = 13, MULTIPLY = 14, SCREEN = 15, OVERLAY = 16, DARKEN = 17, LIGHTEN = 18, COLOUR_DODGE = 19, COLOUR_BURN = 20, HARD_LIGHT = 21, SOFT_LIGHT = 22, DIFFERENCE = 23, EXCLUSION = 24, LAST = 25 }
        class Animation extends System.ValueType
        {
            public frames : System.Array$1<FairyEditor.VImage>
            public frameDelays : System.Array$1<number>
            public loopDelay : number
        }
    }
    namespace XLua {
        class LuaTable extends XLua.LuaBase implements System.IDisposable
        {
        }
        class LuaBase extends System.Object implements System.IDisposable
        {
        }
    }
    namespace FairyEditor.View.ProjectView {
        interface OnContextMenuDelegate
        { (pi: FairyEditor.FPackageItem, context: FairyGUI.EventContext) : void; }
        var OnContextMenuDelegate: { new (func: (pi: FairyEditor.FPackageItem, context: FairyGUI.EventContext) => void): OnContextMenuDelegate; }
        interface OnGetItemListingDelegate
        { (folder: FairyEditor.FPackageItem, filters: System.Array$1<string>, result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) : System.Collections.Generic.List$1<FairyEditor.FPackageItem>; }
        var OnGetItemListingDelegate: { new (func: (folder: FairyEditor.FPackageItem, filters: System.Array$1<string>, result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) => System.Collections.Generic.List$1<FairyEditor.FPackageItem>): OnGetItemListingDelegate; }
    }
    namespace FairyEditor.Component.InputElement {
        class OptionalValue$1<T> extends System.ValueType
        {
        }
    }
    namespace FairyEditor.Component.FormHelper {
        interface OnPropChangedDelegate
        { (propName: string, propValue: any, extData: any) : boolean; }
        var OnPropChangedDelegate: { new (func: (propName: string, propValue: any, extData: any) => boolean): OnPropChangedDelegate; }
    }
    namespace System.Text.RegularExpressions {
        class Regex extends System.Object implements System.Runtime.Serialization.ISerializable
        {
        }
    }
    namespace FairyEditor.Component.ViewGridGroup {
        interface EachGridCallback
        { (grid: FairyEditor.Component.ViewGrid) : boolean; }
        var EachGridCallback: { new (func: (grid: FairyEditor.Component.ViewGrid) => boolean): EachGridCallback; }
    }
    namespace FairyEditor.PluginManager.PluginInfo {
        class Author extends System.Object
        {
            public name : string
            public constructor ()
        }
    }
    namespace FairyGUI.BlendModeUtils {
        class BlendFactor extends System.Object
        {
            public srcFactor : UnityEngine.Rendering.BlendMode
            public dstFactor : UnityEngine.Rendering.BlendMode
            public pma : boolean
            public constructor ($srcFactor: UnityEngine.Rendering.BlendMode, $dstFactor: UnityEngine.Rendering.BlendMode, $pma?: boolean)
            public constructor ()
        }
    }
    namespace FairyGUI.MovieClip {
        class Frame extends System.Object
        {
            public texture : FairyGUI.NTexture
            public addDelay : number
            public constructor ()
        }
    }
    namespace FairyGUI.NGraphics {
        class VertexMatrix extends System.Object
        {
            public cameraPos : UnityEngine.Vector3
            public matrix : UnityEngine.Matrix4x4
            public constructor ()
        }
    }
    namespace FairyGUI.ShaderConfig {
        interface GetFunction
        { (name: string) : UnityEngine.Shader; }
        var GetFunction: { new (func: (name: string) => UnityEngine.Shader): GetFunction; }
    }
    namespace FairyGUI.BitmapFont {
        class BMGlyph extends System.Object
        {
            public x : number
            public y : number
            public width : number
            public height : number
            public advance : number
            public lineHeight : number
            public uv : System.Array$1<UnityEngine.Vector2>
            public channel : number
            public constructor ()
        }
    }
    namespace FairyGUI.RTLSupport {
        enum DirectionType
        { UNKNOW = 0, LTR = 1, RTL = 2, NEUTRAL = 3 }
    }
    namespace FairyGUI.TextField {
        class LineInfo extends System.Object
        {
            public width : number
            public height : number
            public baseline : number
            public charIndex : number
            public charCount : number
            public y : number
            public static Borrow () : FairyGUI.TextField.LineInfo
            public static Return ($value: FairyGUI.TextField.LineInfo) : void
            public static Return ($values: System.Collections.Generic.List$1<FairyGUI.TextField.LineInfo>) : void
            public constructor ()
        }
        class CharPosition extends System.ValueType
        {
            public charIndex : number
            public lineIndex : number
            public offsetX : number
            public vertCount : number
            public width : number
            public imgIndex : number
        }
        class LineCharInfo extends System.ValueType
        {
            public width : number
            public height : number
            public baseline : number
        }
    }
    namespace FairyGUI.TextFormat {
        enum SpecialStyle
        { None = 0, Superscript = 1, Subscript = 2 }
    }
    namespace FairyGUI.UpdateContext {
        class ClipInfo extends System.ValueType
        {
            public rect : UnityEngine.Rect
            public clipBox : UnityEngine.Vector4
            public soft : boolean
            public softness : UnityEngine.Vector4
            public clipId : number
            public rectMaskDepth : number
            public referenceValue : number
            public reversed : boolean
        }
    }
    namespace DragonBones {
        class UnityArmatureComponent extends DragonBones.DragonBoneEventDispatcher implements DragonBones.IEventDispatcher$1<DragonBones.EventObject>, DragonBones.IArmatureProxy
        {
        }
        class DragonBoneEventDispatcher extends DragonBones.UnityEventDispatcher$1<DragonBones.EventObject> implements DragonBones.IEventDispatcher$1<DragonBones.EventObject>
        {
        }
        class EventObject extends DragonBones.BaseObject
        {
        }
        class BaseObject extends System.Object
        {
        }
        class UnityEventDispatcher$1<T> extends UnityEngine.MonoBehaviour
        {
        }
        interface IEventDispatcher$1<T>
        {
        }
        interface IArmatureProxy extends DragonBones.IEventDispatcher$1<DragonBones.EventObject>
        {
        }
        class DragonBonesData extends DragonBones.BaseObject
        {
        }
    }
    namespace Spine.Unity {
        class SkeletonAnimation extends Spine.Unity.SkeletonRenderer implements Spine.Unity.ISkeletonAnimation, Spine.Unity.IHasSkeletonDataAsset, Spine.Unity.ISkeletonComponent, Spine.Unity.IAnimationStateComponent
        {
        }
        class SkeletonRenderer extends UnityEngine.MonoBehaviour implements Spine.Unity.IHasSkeletonDataAsset, Spine.Unity.ISkeletonComponent
        {
        }
        interface IHasSkeletonDataAsset
        {
        }
        interface ISkeletonComponent
        {
        }
        interface ISkeletonAnimation
        {
        }
        interface IAnimationStateComponent
        {
        }
        class SkeletonDataAsset extends UnityEngine.ScriptableObject
        {
        }
    }
    namespace TMPro {
        class TMP_FontAsset extends TMPro.TMP_Asset
        {
        }
        class TMP_Asset extends UnityEngine.ScriptableObject
        {
        }
        enum FontWeight
        { Thin = 100, ExtraLight = 200, Light = 300, Regular = 400, Medium = 500, SemiBold = 600, Bold = 700, Heavy = 800, Black = 900 }
    }
    namespace FairyGUI.ControllerAction {
        enum ActionType
        { PlayTransition = 0, ChangePage = 1 }
    }
    namespace FairyGUI.UIPackage {
        interface CreateObjectCallback
        { (result: FairyGUI.GObject) : void; }
        var CreateObjectCallback: { new (func: (result: FairyGUI.GObject) => void): CreateObjectCallback; }
        interface LoadResource
        { (name: string, extension: string, type: System.Type, destroyMethod: $Ref<FairyGUI.DestroyMethod>) : any; }
        var LoadResource: { new (func: (name: string, extension: string, type: System.Type, destroyMethod: $Ref<FairyGUI.DestroyMethod>) => any): LoadResource; }
        interface LoadResourceAsync
        { (name: string, extension: string, type: System.Type, item: FairyGUI.PackageItem) : void; }
        var LoadResourceAsync: { new (func: (name: string, extension: string, type: System.Type, item: FairyGUI.PackageItem) => void): LoadResourceAsync; }
    }
    namespace FairyGUI.GObjectPool {
        interface InitCallbackDelegate
        { (obj: FairyGUI.GObject) : void; }
        var InitCallbackDelegate: { new (func: (obj: FairyGUI.GObject) => void): InitCallbackDelegate; }
    }
    namespace FairyGUI.UIContentScaler {
        enum ScreenMatchMode
        { MatchWidthOrHeight = 0, MatchWidth = 1, MatchHeight = 2 }
        enum ScaleMode
        { ConstantPixelSize = 0, ScaleWithScreenSize = 1, ConstantPhysicalSize = 2 }
    }
    namespace FairyGUI.GTree {
        interface TreeNodeRenderDelegate
        { (node: FairyGUI.GTreeNode, obj: FairyGUI.GComponent) : void; }
        var TreeNodeRenderDelegate: { new (func: (node: FairyGUI.GTreeNode, obj: FairyGUI.GComponent) => void): TreeNodeRenderDelegate; }
        interface TreeNodeWillExpandDelegate
        { (node: FairyGUI.GTreeNode, expand: boolean) : void; }
        var TreeNodeWillExpandDelegate: { new (func: (node: FairyGUI.GTreeNode, expand: boolean) => void): TreeNodeWillExpandDelegate; }
    }
    namespace FairyGUI.UIObjectFactory {
        interface GComponentCreator
        { () : FairyGUI.GComponent; }
        var GComponentCreator: { new (func: () => FairyGUI.GComponent): GComponentCreator; }
        interface GLoaderCreator
        { () : FairyGUI.GLoader; }
        var GLoaderCreator: { new (func: () => FairyGUI.GLoader): GLoaderCreator; }
    }
    namespace FairyGUI.TreeView {
        interface TreeNodeCreateCellDelegate
        { (node: FairyGUI.TreeNode) : FairyGUI.GComponent; }
        var TreeNodeCreateCellDelegate: { new (func: (node: FairyGUI.TreeNode) => FairyGUI.GComponent): TreeNodeCreateCellDelegate; }
        interface TreeNodeRenderDelegate
        { (node: FairyGUI.TreeNode) : void; }
        var TreeNodeRenderDelegate: { new (func: (node: FairyGUI.TreeNode) => void): TreeNodeRenderDelegate; }
        interface TreeNodeWillExpandDelegate
        { (node: FairyGUI.TreeNode, expand: boolean) : void; }
        var TreeNodeWillExpandDelegate: { new (func: (node: FairyGUI.TreeNode, expand: boolean) => void): TreeNodeWillExpandDelegate; }
    }
    namespace FairyGUI.UIConfig {
        class ConfigValue extends System.Object
        {
            public valid : boolean
            public s : string
            public i : number
            public f : number
            public b : boolean
            public c : UnityEngine.Color
            public Reset () : void
            public constructor ()
        }
        interface SoundLoader
        { (url: string) : FairyGUI.NAudioClip; }
        var SoundLoader: { new (func: (url: string) => FairyGUI.NAudioClip): SoundLoader; }
        enum ConfigKey
        { DefaultFont = 0, ButtonSound = 1, ButtonSoundVolumeScale = 2, HorizontalScrollBar = 3, VerticalScrollBar = 4, DefaultScrollStep = 5, DefaultScrollBarDisplay = 6, DefaultScrollTouchEffect = 7, DefaultScrollBounceEffect = 8, TouchScrollSensitivity = 9, WindowModalWaiting = 10, GlobalModalWaiting = 11, PopupMenu = 12, PopupMenu_seperator = 13, LoaderErrorSign = 14, TooltipsWin = 15, DefaultComboBoxVisibleItemCount = 16, TouchDragSensitivity = 17, ClickDragSensitivity = 18, ModalLayerColor = 19, RenderingTextBrighterOnDesktop = 20, AllowSoftnessOnTopOrLeftSide = 21, InputCaretSize = 22, InputHighlightColor = 23, EnhancedTextOutlineEffect = 24, DepthSupportForPaintingMode = 25, RichTextRowVerticalAlign = 26, Branch = 27, PleaseSelect = 100 }
    }
    namespace FairyGUI.Utils.UBBParser {
        interface TagHandler
        { (tagName: string, end: boolean, attr: string) : string; }
        var TagHandler: { new (func: (tagName: string, end: boolean, attr: string) => string): TagHandler; }
    }
    namespace FairyGUI.Utils.XMLList {
        class Enumerator extends System.ValueType
        {
            public get Current(): FairyGUI.Utils.XML;
            public MoveNext () : boolean
            public Erase () : void
            public Reset () : void
            public constructor ($source: System.Collections.Generic.List$1<FairyGUI.Utils.XML>, $selector: string)
            public constructor ()
        }
    }
    namespace FairyGUI.Utils.ZipReader {
        class ZipEntry extends System.Object
        {
            public name : string
            public compress : number
            public crc : number
            public size : number
            public sourceSize : number
            public offset : number
            public isDirectory : boolean
            public constructor ()
        }
    }
}
