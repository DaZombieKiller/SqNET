using System;

namespace Squirrel
{
    public static partial class Sq
    {
        public static void Move(VM dest, VM source, int idx)
            => Move(dest.Pointer, source.Pointer, idx);

        public static VM NewThread(VM friendvm, int initialstacksize)
            => new VM(NewThread(friendvm.Pointer, initialstacksize));

        public class VM : IDisposable
        {
            private bool _isDisposed;
            private bool _isWrapping;
            private IntPtr _vm;

            public VM(int initialstacksize = 1024)
            {
                _vm = Sq.Open(initialstacksize);
            }

            public VM(IntPtr vm, bool allowDispose = false)
            {
                _vm = vm;
                _isWrapping = !allowDispose;
            }

            public void Dispose()
            {
                if (_isDisposed || _isWrapping)
                    return;

                Sq.Close(_vm);
                _isDisposed = true;
            }

            // The internal pointer
            public IntPtr Pointer => _vm;

            // SqVirtualMachine
            public void Close() => Dispose();
            public SqPrintFunction GetErrorFunc() => Sq.GetErrorFunc(_vm);
            public IntPtr GetForeignPtr() => Sq.GetForeignPtr(_vm);
            public SqPrintFunction GetPrintFunc() => Sq.GetPrintFunc(_vm);
            public VMState GetVMState() => Sq.GetVMState(_vm);
            public void PushConstTable() => Sq.PushConstTable(_vm);
            public void PushRegistryTable() => Sq.PushRegistryTable(_vm);
            public void PushRootTable() => Sq.PushRootTable(_vm);
            public void SetConstTable() => Sq.SetConstTable(_vm);
            public void SetErrorHandler() => Sq.SetErrorHandler(_vm);
            public void SetForeignPtr(IntPtr p) => Sq.SetForeignPtr(_vm, p);
            public void SetPrintFunc(SqPrintFunction printfunc, SqPrintFunction errorfunc) => Sq.SetPrintFunc(_vm, printfunc, errorfunc);
            public void SetRootTable() => Sq.SetRootTable(_vm);
            public int SuspendVM() => Sq.SuspendVM(_vm);
            public int WakeUpVM(bool resumedret, bool retval, bool raiseerror, bool throwerror) => Sq.WakeUpVM(_vm, resumedret, retval, raiseerror, throwerror);

            // SqCompiler
            public int Compile(IntPtr read, IntPtr p, string sourcename, bool raiseerror) => Sq.Compile(_vm, read, p, sourcename, raiseerror);
            public int CompileBuffer(string s, int size, string sourcename, bool raiseerror) => Sq.CompileBuffer(_vm, s, size, sourcename, raiseerror);
            public void EnableDebugInfo(bool enable) => Sq.EnableDebugInfo(_vm, enable);
            public void NotifyAllExceptions(bool enable) => Sq.NotifyAllExceptions(_vm, enable);
            public void SetCompilerErrorHandler(SqCompilerError func) => Sq.SetCompilerErrorHandler(_vm, func);

            // SqStackOperations
            public int Cmp() => Sq.Cmp(_vm);
            public int GetTop() => Sq.GetTop(_vm);
            public void Pop(int nelementstopop) => Sq.Pop(_vm, nelementstopop);
            public void PopTop() => Sq.PopTop(_vm);
            public void Push(int idx) => Sq.Push(_vm, idx);
            public void Remove(int idx) => Sq.Remove(_vm, idx);
            public void ReserveStack(int nsize) => Sq.ReserveStack(_vm, nsize);
            public void SetTop(int idx) => Sq.SetTop(_vm, idx);

            // SqObjectHandling
            public int BindEnv(int idx) => Sq.BindEnv(_vm, idx);
            public int CreateInstance(int idx) => Sq.CreateInstance(_vm, idx);
            public int GetBool(int idx, out bool b) => Sq.GetBool(_vm, idx, out b);
            public int GetByHandle(int idx, IntPtr handle) => Sq.GetByHandle(_vm, idx, handle);
            public int GetClosureInfo(int idx, out uint nparams, out uint nfreevars) => Sq.GetClosureInfo(_vm, idx, out nparams, out nfreevars);
            public int GetClosureName(int idx) => Sq.GetClosureName(_vm, idx);
#if SQUSEDOUBLE
            public int GetFloat(int idx, out double f) => Sq.GetFloat(_vm, idx, out f);
            public int GetDouble(int idx, out double f) => Sq.GetDouble(_vm, idx, out f);
#else
            public int GetFloat(int idx, out float f) => Sq.GetFloat(_vm, idx, out f);
            public int GetDouble(int idx, out float f) => Sq.GetDouble(_vm, idx, out f);
#endif
            public uint GetHash(int idx) => Sq.GetHash(_vm, idx);
            public int GetInstanceUserPointer(int idx, out IntPtr up, IntPtr typetag) => Sq.GetInstanceUserPointer(_vm, idx, out up, typetag);
            public int GetInteger(int idx, out int i) => Sq.GetInteger(_vm, idx, out i);
            public int GetMemberHandle(int idx, out IntPtr handle) => Sq.GetMemberHandle(_vm, idx, out handle);
            public string GetScratchPad(int minsize) => Sq.GetScratchPad(_vm, minsize);
            public int GetSize(int idx) => Sq.GetSize(_vm, idx);
            public int GetString(int idx, out string c) => Sq.GetString(_vm, idx, out c);
            public int GetThread(int idx, out IntPtr thread) => Sq.GetThread(_vm, idx, out thread);
            public ObjectType GetType(int idx) => Sq.GetType(_vm, idx);
            public int GetTypeTag(int idx, out IntPtr typetag) => Sq.GetTypeTag(_vm, idx, out typetag);
            public int GetUserData(int idx, out IntPtr p, out IntPtr typetag) => Sq.GetUserData(_vm, idx, out p, out typetag);
            public int GetUserPointer(int idx, out IntPtr p) => Sq.GetUserPointer(_vm, idx, out p);
            public void NewArray(int size) => Sq.NewArray(_vm, size);
            public int NewClass(bool hasbase) => Sq.NewClass(_vm, hasbase);
            public void NewClosure(SqFunction func, int nfreevars) => Sq.NewClosure(_vm, func, nfreevars);
            public void NewTable() => Sq.NewTable(_vm);
            public void NewTableEx(int initialcapacity) => Sq.NewTableEx(_vm, initialcapacity);
            public IntPtr NewUserData(uint size) => Sq.NewUserData(_vm, size);
            public void PushBool(bool b) => Sq.PushBool(_vm, b);
#if SQUSEDOUBLE
            public void PushFloat(double f) => Sq.PushFloat(_vm, f);
            public void PushDouble(double f) => Sq.PushDouble(_vm, f);
#else
            public void PushFloat(float f) => Sq.PushFloat(_vm, f);
            public void PushDouble(float f) => Sq.PushDouble(_vm, f);
#endif
            public void PushInteger(int n) => Sq.PushInteger(_vm, n);
            public void PushNull() => Sq.PushNull(_vm);
            public void PushString(string s, int len) => Sq.PushString(_vm, s, len);
            public void PushUserPointer(IntPtr p) => Sq.PushUserPointer(_vm, p);
            public int SetByHandle(int idx, IntPtr handle) => Sq.SetByHandle(_vm, idx, handle);
            public int SetClassUserDataSize(int idx, int udsize) => Sq.SetClassUserDataSize(_vm, idx, udsize);
            public int SetInstanceUserPointer(int idx, IntPtr up) => Sq.SetInstanceUserPointer(_vm, idx, up);
            public int SetNativeClosureName(int idx, string name) => Sq.SetNativeClosureName(_vm, idx, name);
            public int SetParamsCheck(int nparamscheck, string typemask) => Sq.SetParamsCheck(_vm, nparamscheck, typemask);
            public void SetReleaseHook(int idx, SqReleaseHook hook) => Sq.SetReleaseHook(_vm, idx, hook);
            public int SetTypeTag(int idx, IntPtr typetag) => Sq.SetTypeTag(_vm, idx, typetag);
            public void ToBool(int idx, out bool b) => Sq.ToBool(_vm, idx, out b);
            public void ToString(int idx) => Sq.ToString(_vm, idx);
            public ObjectType TypeOf(int idx) => Sq.TypeOf(_vm, idx);

            // SqCalls
            public int Call(int nparams, bool retval, bool raiseerror) => Sq.Call(_vm, nparams, retval, raiseerror);
            public int GetCallee() => Sq.GetCallee(_vm);
            public int GetLastError() => Sq.GetLastError(_vm);
            public string GetLocal(uint level, uint nseq) => Sq.GetLocal(_vm, level, nseq);
            public void ResetError() => Sq.ResetError(_vm);
            public int Resume(bool retval, bool raiseerror) => Sq.Resume(_vm, retval, raiseerror);
            public int ThrowError(string err) => Sq.ThrowError(_vm, err);
            public int ThrowObject() => Sq.ThrowObject(_vm);

            // SqObjectsManipulation
            public int ArrayAppend(int idx) => Sq.ArrayAppend(_vm, idx);
            public int ArrayInsert(int idx, int destpos) => Sq.ArrayInsert(_vm, idx, destpos);
            public int ArrayPop(int idx) => Sq.ArrayPop(_vm, idx);
            public int ArrayRemove(int idx, int itemidx) => Sq.ArrayRemove(_vm, idx, itemidx);
            public int ArrayResize(int idx, int newsize) => Sq.ArrayResize(_vm, idx, newsize);
            public int ArrayReverse(int idx) => Sq.ArrayReverse(_vm, idx);
            public int Clear(int idx) => Sq.Clear(_vm, idx);
            public int Clone(int idx) => Sq.Clone(_vm, idx);
            public int CreateSlot(int idx) => Sq.CreateSlot(_vm, idx);
            public int DeleteSlot(int idx, bool pushval) => Sq.DeleteSlot(_vm, idx, pushval);
            public int Get(int idx) => Sq.Get(_vm, idx);
            public int GetAttributes(int idx) => Sq.GetAttributes(_vm, idx);
            public int GetClass(int idx) => Sq.GetClass(_vm, idx);
            public int GetDelegate(int idx) => Sq.GetDelegate(_vm, idx);
            public string GetFreeVariable(int idx, int nval) => Sq.GetFreeVariable(_vm, idx, nval);
            public int GetWeakRefVal(int idx) => Sq.GetWeakRefVal(_vm, idx);
            public bool InstanceOf() => Sq.InstanceOf(_vm);
            public int NewMember(int idx, bool bstatic) => Sq.NewMember(_vm, idx, bstatic);
            public int NewSlot(int idx, bool bstatic) => Sq.NewSlot(_vm, idx, bstatic);
            public int Next(int idx) => Sq.Next(_vm, idx);
            public int RawDeleteSlot(int idx, bool pushval) => Sq.RawDeleteSlot(_vm, idx, pushval);
            public int RawGet(int idx) => Sq.RawGet(_vm, idx);
            public int RawNewMember(int idx, bool bstatic) => Sq.RawNewMember(_vm, idx, bstatic);
            public int RawSet(int idx) => Sq.RawSet(_vm, idx);
            public int Set(int idx) => Sq.Set(_vm, idx);
            public int SetAttributes(int idx) => Sq.SetAttributes(_vm, idx);
            public int SetDelegate(int idx) => Sq.SetDelegate(_vm, idx);
            public int SetFreeVariable(int idx, int nval) => Sq.SetFreeVariable(_vm, idx, nval);
            public void WeakRef(int idx) => Sq.WeakRef(_vm, idx);

            // SqBytecodeSerialization
            public int ReadClosure(SqReadFunc readf, IntPtr up) => Sq.ReadClosure(_vm, readf, up);
            public int WriteClosure(SqWriteFunc writef, IntPtr up) => Sq.WriteClosure(_vm, writef, up);

            // SqRawObjectHandling
            public void AddRef(out Object po) => Sq.AddRef(_vm, out po);
            public int GetObjTypeTag(out IntPtr typetag) => Sq.GetObjTypeTag(_vm, out typetag);
            public uint GetRefCount(out Object po) => Sq.GetRefCount(_vm, out po);
            public int GetStackObj(int idx, out Object po) => Sq.GetStackObj(_vm, idx, out po);
            public void PushObject(Object po) => Sq.PushObject(_vm, po);
            public bool Release(out Object po) => Sq.Release(_vm, out po);

            // SqGarbageCollector
            public int CollectGarbage() => Sq.CollectGarbage(_vm);
            public int ResurrectUnreachable() => Sq.ResurrectUnreachable(_vm);

            // SqDebugInterface
            public int GetFunctionInfo(int level, out FunctionInfo fi) => Sq.GetFunctionInfo(_vm, level, out fi);
            public void SetDebugHook() => Sq.SetDebugHook(_vm);
            public void SetNativeDebugHook(SqDebugHook hook) => Sq.SetNativeDebugHook(_vm, hook);
            public int GetStackInfo(int level, out StackInfo si) => Sq.GetStackInfo(_vm, level, out si);
        }
    }
}
