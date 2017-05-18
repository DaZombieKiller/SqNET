using System;
using System.Runtime.InteropServices;

namespace SqDotNet
{
    #region Enums
    /// <summary>The execution state of a VM.</summary>
    public enum VMState
    {
        /// <summary></summary>
        Idle,

        /// <summary></summary>
        Running,

        /// <summary></summary>
        Suspended
    }

    enum ObjectFlags
    {
        CanBeFalse = 0x01000000,
        Delegable = 0x02000000,
        Numeric = 0x04000000,
        RefCounted = 0x08000000,
    }

    enum RawTypes
    {
        Null = 0x00000001,
        Integer = 0x00000002,
        Float = 0x00000004,
        Bool = 0x00000008,
        String = 0x00000010,
        Table = 0x00000020,
        Array = 0x00000040,
        UserData = 0x00000080,
        Closure = 0x00000100,
        NativeClosure = 0x00000200,
        Generator = 0x00000400,
        UserPointer = 0x00000800,
        Thread = 0x00001000,
        FuncProto = 0x00002000,
        Class = 0x00004000,
        Instance = 0x00008000,
        WeakRef = 0x00010000,
        Outer = 0x00020000,
    }

    /// <summary></summary>
    public enum ObjectType
    {
        /// <summary></summary>
        Null = (RawTypes.Null|ObjectFlags.CanBeFalse),

        /// <summary></summary>
        Integer = (RawTypes.Integer|ObjectFlags.Numeric|ObjectFlags.CanBeFalse),

        /// <summary></summary>
        Float = (RawTypes.Float|ObjectFlags.Numeric|ObjectFlags.CanBeFalse),

        /// <summary></summary>
        Bool = (RawTypes.Bool|ObjectFlags.CanBeFalse),

        /// <summary></summary>
        String = (RawTypes.String|ObjectFlags.RefCounted),

        /// <summary></summary>
        Table = (RawTypes.Table|ObjectFlags.RefCounted|ObjectFlags.Delegable),

        /// <summary></summary>
        Array = (RawTypes.Array|ObjectFlags.RefCounted),

        /// <summary></summary>
        UserData = (RawTypes.UserData|ObjectFlags.RefCounted|ObjectFlags.Delegable),

        /// <summary></summary>
        Closure = (RawTypes.Closure|ObjectFlags.RefCounted),

        /// <summary></summary>
        NativeClosure = (RawTypes.NativeClosure|ObjectFlags.RefCounted),

        /// <summary></summary>
        Generator = (RawTypes.Generator|ObjectFlags.RefCounted),

        /// <summary></summary>
        UserPointer = RawTypes.UserPointer,

        /// <summary></summary>
        Thread = (RawTypes.Thread|ObjectFlags.RefCounted),

        /// <summary>Internal usage only</summary>
        FuncProto = (RawTypes.FuncProto|ObjectFlags.RefCounted),

        /// <summary></summary>
        Class = (RawTypes.Class|ObjectFlags.RefCounted),

        /// <summary></summary>
        Instance = (RawTypes.Instance|ObjectFlags.RefCounted|ObjectFlags.Delegable),

        /// <summary></summary>
        WeakRef = (RawTypes.WeakRef|ObjectFlags.RefCounted),

        /// <summary>Internal usage only</summary>
        Outer = (RawTypes.Outer|ObjectFlags.RefCounted),
    }
    #endregion

    #region Structs
    /// <summary></summary>
    public struct FunctionInfo
    {
        /// <summary></summary>
        public IntPtr FuncId;

        /// <summary></summary>
        public string Name;

        /// <summary></summary>
        public string Source;
    }

    /// <summary></summary>
    public struct StackInfo
    {
        /// <summary></summary>
        public string FuncName;

        /// <summary></summary>
        public string Source;

        /// <summary></summary>
        public int Line;
    }

    /// <summary>A Squirrel object</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Object
    {
        // Must be 16 bytes
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] data;

        public bool ToBool()
        {
            return Squirrel.Unmanaged.ObjToBool(out this);
        }

        public float ToFloat()
        {
            return Squirrel.Unmanaged.ObjToFloat(out this);
        }

        public int ToInteger()
        {
            return Squirrel.Unmanaged.ObjToInteger(out this);
        }

        public string ToString()
        {
            return Squirrel.Unmanaged.ObjToString(out this);
        }

        public IntPtr ToUserPointer()
        {
            return Squirrel.Unmanaged.ObjToUserPointer(out this);
        }

        public void Reset()
        {
            Squirrel.Unmanaged.ResetObject(out this);
        }
    }
    #endregion

    #region Delegates
    public delegate int Function(Squirrel v);
    public delegate void PrintFunction(Squirrel v, string message);
    #endregion

    /// <summary>A Squirrel virtual machine.</summary>
    public partial class Squirrel : IDisposable
    {
        #region Properties
        public IntPtr Pointer { get; private set; }
        #endregion

        #region Operators
        static public implicit operator Squirrel(IntPtr ptr)
        {
            return new Squirrel(ptr);
        }

        static public implicit operator IntPtr(Squirrel vm)
        {
            return vm.Pointer;
        }
        #endregion

        #region Virtual Machine
        /// <summary>Creates a new instance of a squirrel VM that consists in a new execution stack.</summary>
        /// <param name="stackSize">The size of the stack in slots (number of objects)</param>
        public Squirrel(int stackSize = 1024)
        {
            Pointer = Unmanaged.Open(stackSize);
        }

        private Squirrel(IntPtr vm)
        {
            Pointer = vm;
        }

        /// <summary></summary>
        public void Dispose()
        {
            Unmanaged.Close(Pointer);
        }

        /// <summary>Releases a Squirrel VM and all related friend VMs.</summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>Returns the current error function of the VM. (see <see cref="SetPrintFunc"/>)</summary>
        public PrintFunction GetErrorFunc()
        {
            Unmanaged.SqPrintFunction func = Unmanaged.GetErrorFunc(Pointer);
            return (Squirrel v, string message) => func(v.Pointer, message);
        }

        /// <summary>Returns the current print function of the VM. (see <see cref="SetPrintFunc"/>)</summary>
        public PrintFunction GetPrintFunc()
        {
            Unmanaged.SqPrintFunction func = Unmanaged.GetPrintFunc(Pointer);
            return (Squirrel v, string message) => func(v.Pointer, message);
        }

        /// <summary>Returns the version number of the VM.</summary>
        public int GetVersion()
        {
            return Unmanaged.GetVersion();
        }

        /// <summary>Returns the execution state of a VM.</summary>
        public VMState GetVMState()
        {
            return Unmanaged.GetVMState(Pointer);
        }

        /// <summary>Pushes the current const table in the stack.</summary>
        public void PushConstTable()
        {
            Unmanaged.PushConstTable(Pointer);
        }

        /// <summary>Pushes the registry table in the stack.</summary>
        public void PushRegistryTable()
        {
            Unmanaged.PushRegistryTable(Pointer);
        }

        /// <summary>Pushes the current root table in the stack.</summary>
        public void PushRootTable()
        {
            Unmanaged.PushRootTable(Pointer);
        }

        /// <summary>Pops a table from the stack and sets it as the const table.</summary>
        public void SetConstTable()
        {
            Unmanaged.SetConstTable(Pointer);
        }

        /// <summary>Pops from the stack a closure or native closure and sets it as the runtime-error handler.</summary>
        public void SetErrorHandler()
        {
            Unmanaged.SetErrorHandler(Pointer);
        }

        /// <summary>Sets the print function of the virtual machine. This function is used by the built-in function '::print()' to output text.</summary>
        public void SetPrintFunc(PrintFunction print, PrintFunction error)
        {
            Unmanaged.SetPrintFunc(Pointer,
                (IntPtr v, string message) => print(new Squirrel(v), message),
                (IntPtr v, string message) => error(new Squirrel(v), message));
        }

        /// <summary>Pops a table from the stack and sets it as the root table.</summary>
        public void SetRootTable()
        {
            Unmanaged.SetRootTable(Pointer);
        }

        /// <summary>Suspends the execution of the VM.</summary>
        public int Suspend()
        {
            return Unmanaged.SuspendVM(Pointer);
        }

        /// <summary>Wake up the execution of the VM.</summary>
        /// <param name="resumedRet">If true the function will pop a value from the stack and use it as a return value for the function that has previously suspended the VM</param>
        /// <param name="retVal">If true the function will push the return value of the function that suspended the execution or the main function one</param>
        /// <param name="raiseError">If true, if a runtime error occurs during the execution of the call, the VM will invoke the error handler</param>
        /// <param name="throwError">If true, the VM will throw and exception as soon as it is resumed. The exception payload must be set beforehand invoking Sq.ThrowError</param>
        public int WakeUp(bool resumedRet, bool retVal, bool raiseError, bool throwError)
        {
            return Unmanaged.WakeUpVM(Pointer, resumedRet, retVal, raiseError, throwError);
        }
        #endregion

        #region Compiler
        /// <summary></summary>
        /// <param name="read"></param>
        /// <param name="p"></param>
        /// <param name="sourceName"></param>
        /// <param name="raiseError"></param>
        public int Compile(Unmanaged.SqLexReadFunc read, IntPtr p, string sourceName, bool raiseError)
        {
            return Unmanaged.Compile(Pointer, read, p, sourceName, raiseError);
        }

        /// <summary></summary>
        /// <param name="s"></param>
        /// <param name="size"></param>
        /// <param name="sourceName"></param>
        /// <param name="raiseError"></param>
        public int CompileBuffer(string s, int size, string sourceName, bool raiseError)
        {
            return Unmanaged.CompileBuffer(Pointer, s, size, sourceName, raiseError);
        }

        /// <summary></summary>
        /// <param name="enable"></param>
        public void EnableDebugInfo(bool enable)
        {
            Unmanaged.EnableDebugInfo(Pointer, enable);
        }

        /// <summary></summary>
        /// <param name="enable"></param>
        public void NotifyAllExceptions(bool enable)
        {
            Unmanaged.NotifyAllExceptions(Pointer, enable);
        }

        /// <summary></summary>
        /// <param name="errorfunc"></param>
        public void SetCompilerErrorHandler(Unmanaged.SqCompilerError errorfunc)
        {
            Unmanaged.SetCompilerErrorHandler(Pointer, errorfunc);
        }
        #endregion

        #region Stack Operations
        /// <summary></summary>
        public int Cmp()
        {
            return Unmanaged.Cmp(Pointer);
        }

        public int GetTop()
        {
            return Unmanaged.GetTop(Pointer);
        }

        public void Pop(int numElementsToPop)
        {
            Unmanaged.Pop(Pointer, numElementsToPop);
        }

        public void PopTop()
        {
            Unmanaged.PopTop(Pointer);
        }

        public void Push(int idx)
        {
            Unmanaged.Push(Pointer, idx);
        }

        public void Remove(int idx)
        {
            Unmanaged.Remove(Pointer, idx);
        }

        public void ReserveStack(int nSize)
        {
            Unmanaged.ReserveStack(Pointer, nSize);
        }

        public void SetTop(int idx)
        {
            Unmanaged.SetTop(Pointer, idx);
        }
        #endregion

        #region Object Creation and Handling
        public int BindEnv(int idx)
        {
            return Unmanaged.BindEnv(Pointer, idx);
        }

        public int CreateInstance(int idx)
        {
            return Unmanaged.CreateInstance(Pointer, idx);
        }

        public int GetBool(int idx, out bool b)
        {
            return Unmanaged.GetBool(Pointer, idx, out b);
        }

        public int GetByHandle(int idx, IntPtr handle)
        {
            return Unmanaged.GetByHandle(Pointer, idx, handle);
        }

        public int GetClosureInfo(int idx, out uint nParams, out uint nFreeVars)
        {
            return Unmanaged.GetClosureInfo(Pointer, idx, out nParams, out nFreeVars);
        }

        public int GetClosureName(int idx)
        {
            return Unmanaged.GetClosureName(Pointer, idx);
        }

        public int GetFloat(int idx, out float f)
        {
            return Unmanaged.GetFloat(Pointer, idx, out f);
        }

        public uint GetHash(int idx)
        {
            return Unmanaged.GetHash(Pointer, idx);
        }

        public int GetInstanceUserPointer(int idx, out IntPtr userPointer, IntPtr typeTag)
        {
            return Unmanaged.GetInstanceUserPointer(Pointer, idx, out userPointer, typeTag);
        }

        public int GetInteger(int idx, out int i)
        {
            return Unmanaged.GetInteger(Pointer, idx, out i);
        }

        public int GetMemberHandle(int idx, out IntPtr handle)
        {
            return Unmanaged.GetMemberHandle(Pointer, idx, out handle);
        }

        public string GetScratchPad(int minSize)
        {
            return Unmanaged.GetScratchPad(Pointer, minSize);
        }

        public int GetSize(int idx)
        {
            return Unmanaged.GetSize(Pointer, idx);
        }

        public int GetString(int idx, out string c)
        {
            return Unmanaged.GetString(Pointer, idx, out c);
        }

        public int GetThread(int idx, out IntPtr thread)
        {
            return Unmanaged.GetThread(Pointer, idx, out thread);
        }

        public ObjectType GetType(int idx)
        {
            return Unmanaged.GetType(Pointer, idx);
        }

        public int GetTypeTag(int idx, out IntPtr typeTag)
        {
            return Unmanaged.GetTypeTag(Pointer, idx, out typeTag);
        }

        public int GetUserData(int idx, out IntPtr p, out IntPtr typeTag)
        {
            return Unmanaged.GetUserData(Pointer, idx, out p, out typeTag);
        }

        public int GetUserPointer(int idx, out IntPtr p)
        {
            return Unmanaged.GetUserPointer(Pointer, idx, out p);
        }

        public void NewArray(int size)
        {
            Unmanaged.NewArray(Pointer, size);
        }

        public int NewClass(bool hasBase)
        {
            return Unmanaged.NewClass(Pointer, hasBase);
        }

        public void NewClosure(Function func, int nFreeVars)
        {
            Unmanaged.NewClosure(Pointer, (v) => func(new Squirrel(v)), nFreeVars);
        }

        public void NewTable()
        {
            Unmanaged.NewTable(Pointer);
        }

        public void NewTableEx(int initialCapacity)
        {
            Unmanaged.NewTableEx(Pointer, initialCapacity);
        }

        public IntPtr NewUserData(uint size)
        {
            return Unmanaged.NewUserData(Pointer, size);
        }

        public void PushBool(bool b)
        {
            Unmanaged.PushBool(Pointer, b);
        }

        public void PushFloat(float f)
        {
            Unmanaged.PushFloat(Pointer, f);
        }

        public void PushInteger(int n)
        {
            Unmanaged.PushInteger(Pointer, n);
        }

        public void PushNull()
        {
            Unmanaged.PushNull(Pointer);
        }

        public void PushString(string s, int len)
        {
            Unmanaged.PushString(Pointer, s, len);
        }

        public void PushUserPointer(IntPtr p)
        {
            Unmanaged.PushUserPointer(Pointer, p);
        }

        public int SetByHandle(int idx, IntPtr handle)
        {
            return Unmanaged.SetByHandle(Pointer, idx, handle);
        }

        public int SetClassUserDataSize(int idx, int size)
        {
            return Unmanaged.SetClassUserDataSize(Pointer, idx, size);
        }

        public int SetInstanceUserPointer(int idx, IntPtr userPointer)
        {
            return Unmanaged.SetInstanceUserPointer(Pointer, idx, userPointer);
        }

        public int SetNativeClosureName(int idx, string name)
        {
            return Unmanaged.SetNativeClosureName(Pointer, idx, name);
        }

        public int SetParamsCheck(int nParamsCheck, string typeMask)
        {
            return Unmanaged.SetParamsCheck(Pointer, nParamsCheck, typeMask);
        }

        public void SetReleaseHook(int idx, Unmanaged.SqReleaseHook hook)
        {
            Unmanaged.SetReleaseHook(Pointer, idx, hook);
        }

        public int SetTypeTag(int idx, IntPtr typeTag)
        {
            return Unmanaged.SetTypeTag(Pointer, idx, typeTag);
        }

        public void ToBool(int idx, out bool b)
        {
            Unmanaged.ToBool(Pointer, idx, out b);
        }

        public void ToString(int idx)
        {
            Unmanaged.ToString(Pointer, idx);
        }

        public ObjectType TypeOf(int idx)
        {
            return Unmanaged.TypeOf(Pointer, idx);
        }
        #endregion

        #region Calls
        public int Call(int nParams, bool retVal, bool raiseError)
        {
            return Unmanaged.Call(Pointer, nParams, retVal, raiseError);
        }

        public int GetCallee()
        {
            return Unmanaged.GetCallee(Pointer);
        }

        public int GetLastError()
        {
            return Unmanaged.GetLastError(Pointer);
        }

        public string GetLocal(uint level, uint nseq)
        {
            return Unmanaged.GetLocal(Pointer, level, nseq);
        }

        public void ResetError()
        {
            Unmanaged.ResetError(Pointer);
        }

        public int Resume(bool retVal, bool raiseError)
        {
            return Unmanaged.Resume(Pointer, retVal, raiseError);
        }

        public int ThrowError(string err)
        {
            return Unmanaged.ThrowError(Pointer, err);
        }

        public int ThrowObject()
        {
            return Unmanaged.ThrowObject(Pointer);
        }
        #endregion

        #region Object Manipulation
        public int ArrayAppend(int idx)
        {
            return Unmanaged.ArrayAppend(Pointer, idx);
        }

        public int ArrayInsert(int idx, int destPos)
        {
            return Unmanaged.ArrayInsert(Pointer, idx, destPos);
        }

        public int ArrayPop(int idx)
        {
            return Unmanaged.ArrayPop(Pointer, idx);
        }

        public int ArrayRemove(int idx, int itemIdx)
        {
            return Unmanaged.ArrayRemove(Pointer, idx, itemIdx);
        }

        public int ArrayResize(int idx, int newSize)
        {
            return Unmanaged.ArrayResize(Pointer, idx, newSize);
        }

        public int ArrayReverse(int idx)
        {
            return Unmanaged.ArrayReverse(Pointer, idx);
        }

        public int Clear(int idx)
        {
            return Unmanaged.Clear(Pointer, idx);
        }

        public int Clone(int idx)
        {
            return Unmanaged.Clone(Pointer, idx);
        }

        public int CreateSlot(int idx)
        {
            return Unmanaged.CreateSlot(Pointer, idx);
        }

        public int DeleteSlot(int idx, bool pushVal)
        {
            return Unmanaged.DeleteSlot(Pointer, idx, pushVal);
        }

        public int Get(int idx)
        {
            return Unmanaged.Get(Pointer, idx);
        }

        public int GetAttributes(int idx)
        {
            return Unmanaged.Get(Pointer, idx);
        }

        public int GetClass(int idx)
        {
            return Unmanaged.GetClass(Pointer, idx);
        }

        public int GetDelegate(int idx)
        {
            return Unmanaged.GetDelegate(Pointer, idx);
        }

        public string GetFreeVariable(int idx, int nval)
        {
            return Unmanaged.GetFreeVariable(Pointer, idx, nval);
        }

        public int GetWeakRefVal(int idx)
        {
            return Unmanaged.GetWeakRefVal(Pointer, idx);
        }

        public bool InstanceOf()
        {
            return Unmanaged.InstanceOf(Pointer);
        }

        public int NewMember(int idx, bool bstatic)
        {
            return Unmanaged.NewMember(Pointer, idx, bstatic);
        }

        public int NewSlot(int idx, bool bstatic)
        {
            return Unmanaged.NewSlot(Pointer, idx, bstatic);
        }

        public int Next(int idx)
        {
            return Unmanaged.Next(Pointer, idx);
        }

        public int RawDeleteSlot(int idx, bool pushVal)
        {
            return Unmanaged.RawDeleteSlot(Pointer, idx, pushVal);
        }

        public int RawGet(int idx)
        {
            return Unmanaged.RawGet(Pointer, idx);
        }

        public int RawNewMember(int idx, bool bstatic)
        {
            return Unmanaged.RawNewMember(Pointer, idx, bstatic);
        }

        public int RawSet(int idx)
        {
            return Unmanaged.RawSet(Pointer, idx);
        }

        public int Set(int idx)
        {
            return Unmanaged.Set(Pointer, idx);
        }

        public int SetAttributes(int idx)
        {
            return Unmanaged.SetAttributes(Pointer, idx);
        }

        public int SetDelegate(int idx)
        {
            return Unmanaged.SetDelegate(Pointer, idx);
        }

        public int SetFreeVariable(int idx, int nval)
        {
            return Unmanaged.SetFreeVariable(Pointer, idx, nval);
        }

        public void WeakRef(int idx)
        {
            Unmanaged.WeakRef(Pointer, idx);
        }
        #endregion

        #region Bytecode Serialization
        public int ReadClosure(Unmanaged.SqReadFunc readf, IntPtr up)
        {
            return Unmanaged.ReadClosure(Pointer, readf, up);
        }

        public int WriteClosure(Unmanaged.SqWriteFunc writef, IntPtr up)
        {
            return Unmanaged.WriteClosure(Pointer, writef, up);
        }
        #endregion

        #region Raw Object Handling
        public void AddRef(out Object po)
        {
            Unmanaged.AddRef(Pointer, out po);
        }

        public int GetObjTypeTag(out IntPtr typeTag)
        {
            return Unmanaged.GetObjTypeTag(Pointer, out typeTag);
        }

        public uint GetRefCount(out Object po)
        {
            return Unmanaged.GetRefCount(Pointer, out po);
        }

        public int GetStackObj(int idx, out Object po)
        {
            return Unmanaged.GetStackObj(Pointer, idx, out po);
        }

        public void PushObject(Object po)
        {
            Unmanaged.PushObject(Pointer, po);
        }

        public bool Release(out Object po)
        {
            return Unmanaged.Release(Pointer, out po);
        }
        #endregion

        #region Garbage Collector
        #endregion

        #region Debug Interface
        #endregion

        #region Standard Library
        public void SetErrorHandlers()
        {
            Unmanaged.Std.SetErrorHandlers(Pointer);
        }

        public void PrintCallStack()
        {
            Unmanaged.Std.PrintCallStack(Pointer);
        }

        public int RegisterBlobLib()
        {
            return Unmanaged.Std.RegisterBlobLib(Pointer);
        }

        public int LoadFile(string filename, bool printError)
        {
            return Unmanaged.Std.LoadFile(Pointer, filename, printError);
        }

        public int DoFile(string filename, bool retVal, bool printError)
        {
            return Unmanaged.Std.DoFile(Pointer, filename, retVal, printError);
        }

        public int WriteClosureToFile(string filename)
        {
            return Unmanaged.Std.WriteClosureToFile(Pointer, filename);
        }

        public int RegisterIOLib()
        {
            return Unmanaged.Std.RegisterIOLib(Pointer);
        }

        public int RegisterMathLib()
        {
            return Unmanaged.Std.RegisterMathLib(Pointer);
        }

        public int RegisterStringLib()
        {
            return Unmanaged.Std.RegisterStringLib(Pointer);
        }

        public int RegisterSystemLib()
        {
            return Unmanaged.Std.RegisterSystemLib(Pointer);
        }
        #endregion
    }
}
