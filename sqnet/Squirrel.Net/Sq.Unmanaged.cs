using System;
using System.Runtime.InteropServices;

namespace SqDotNet
{
    public partial class Squirrel
    {
        /// <summary></summary>
        public static class Unmanaged
        {
            #region Constants
            /// <summary>The type of string used by Squirrel</summary>
#if SQUNICODE
            public const UnmanagedType SqString = UnmanagedType.LPWStr;
#else
            public const UnmanagedType SqString = UnmanagedType.LPStr;
#endif

            /// <summary>Path to the main Squirrel library</summary>
            public const string SqLib = "squirrel";

            /// <summary>Path to the Squirrel standard library</summary>
            public const string SqStdLib = "sqstdlib";

            /// <summary>Path to the Squirrel.NET helper library</summary>
            public const string SqNetLib = "sqnetlib";
            #endregion

            #region Delegates
            /// <summary></summary>
            /// <param name="p"></param>
            /// <param name="size"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int SqReleaseHook(IntPtr p, int size);

            /// <summary></summary>
            /// <param name="file"></param>
            /// <param name="buf"></param>
            /// <param name="size"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int SqReadFunc(IntPtr file, IntPtr buf, int size);

            /// <summary></summary>
            /// <param name="file"></param>
            /// <param name="p"></param>
            /// <param name="size"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int SqWriteFunc(IntPtr file, IntPtr p, int size);

            /// <summary></summary>
            /// <param name="file"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int SqLexReadFunc(IntPtr file);

            /// <summary>A Squirrel function</summary>
            /// <param name="v"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int SqFunction(IntPtr v);

            /// <summary></summary>
            /// <param name="v"></param>
            /// <param name="desc"></param>
            /// <param name="source"></param>
            /// <param name="line"></param>
            /// <param name="column"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void SqCompilerError(IntPtr v, [MarshalAs(SqString)] string desc, [MarshalAs(SqString)] string source, int line, int column);

            /// <summary></summary>
            /// <param name="v"></param>
            /// <param name="message"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void SqPrintFunction(IntPtr v, [MarshalAs(SqString)] string message);

            /// <summary></summary>
            /// <param name="v"></param>
            /// <param name="type"></param>
            /// <param name="sourcename"></param>
            /// <param name="line"></param>
            /// <param name="funcname"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate void SqDebugHook(IntPtr v, int type, [MarshalAs(SqString)] string sourcename, int line, [MarshalAs(SqString)] string funcname);
            #endregion

            #region Structs
            [StructLayout(LayoutKind.Sequential)]
            private struct IFunctionInfo
            {
                public IntPtr FuncId;
                public IntPtr Name;
                public IntPtr Source;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct IStackInfo
            {
                public IntPtr FuncName;
                public IntPtr Source;
                public int Line;
            }
            #endregion

            #region Virtual Machine
            [DllImport(SqLib, EntryPoint = "sq_close", CallingConvention = CallingConvention.Cdecl)]
            extern public static void Close(IntPtr v);

            [DllImport(SqNetLib, EntryPoint = "sqnet_geterrorfunc", CallingConvention = CallingConvention.Cdecl)]
            extern public static SqPrintFunction GetErrorFunc(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_getforeignptr", CallingConvention = CallingConvention.Cdecl)]
            extern public static IntPtr GetForeignPtr(IntPtr v);

            [DllImport(SqNetLib, EntryPoint = "sqnet_getprintfunc", CallingConvention = CallingConvention.Cdecl)]
            extern public static SqPrintFunction GetPrintFunc(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_getversion", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetVersion();

            [DllImport(SqLib, EntryPoint = "sq_getvmstate", CallingConvention = CallingConvention.Cdecl)]
            extern public static VMState GetVMState(IntPtr v);

            /// <summary>
            /// Pushes the object at the position 'idx' of the source VM stack in the destination VM stack.
            /// </summary>
            /// <param name="dest">The destination VM</param>
            /// <param name="src">The source VM</param>
            /// <param name="idx">The index in the source stack of the value that has to be moved</param>
            [DllImport(SqLib, EntryPoint = "sq_move", CallingConvention = CallingConvention.Cdecl)]
            extern public static void Move(IntPtr dest, IntPtr src, int idx);

            /// <summary>
            /// Creates a new VM FriendVM of the one passed as first parameter and pushes it in its stack as a "thread" object.
            /// </summary>
            /// <param name="friendvm">A friend VM</param>
            /// <param name="initialstacksize">The size of the stack in slots (number of objects)</param>
            /// <returns>A pointer to the new VM</returns>
            /// <remarks>By default the roottable is shared with the VM passed as first parameter. The new VM lifetime is bound to the "thread" object pushed in the stack and behave like a normal squirrel object.</remarks>
            [DllImport(SqLib, EntryPoint = "sq_newthread", CallingConvention = CallingConvention.Cdecl)]
            extern public static IntPtr NewThread(IntPtr friendvm, int initialstacksize);

            [DllImport(SqLib, EntryPoint = "sq_open", CallingConvention = CallingConvention.Cdecl)]
            extern public static IntPtr Open(int initialstacksize);

            [DllImport(SqLib, EntryPoint = "sq_pushconsttable", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushConstTable(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_pushregistrytable", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushRegistryTable(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_pushroottable", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushRootTable(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_setconsttable", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetConstTable(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_seterrorhandler", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetErrorHandler(IntPtr v);

            /// <summary>
            /// Sets the foreign pointer of a certain VM instance. The foreign pointer is an arbitrary user defined pointer associated to a VM (by default is value id 0). This pointer is ignored by the VM.
            /// </summary>
            /// <param name="v">The target VM</param>
            /// <param name="p">The pointer that has to be set</param>
            [DllImport(SqLib, EntryPoint = "sq_setforeignptr", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetForeignPtr(IntPtr v, IntPtr p);

            [DllImport(SqNetLib, EntryPoint = "sqnet_setprintfunc", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetPrintFunc(IntPtr v, SqPrintFunction printfunc, SqPrintFunction errorfunc);

            [DllImport(SqLib, EntryPoint = "sq_setroottable", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetRootTable(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_suspendvm", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SuspendVM(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_wakeupvm", CallingConvention = CallingConvention.Cdecl)]
            extern public static int WakeUpVM(IntPtr v, bool resumedret, bool retval, bool raiseerror, bool throwerror);
            #endregion

            #region Compiler
            [DllImport(SqLib, EntryPoint = "sq_compile", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Compile(IntPtr v, SqLexReadFunc read, IntPtr p, [MarshalAs(SqString)] string sourcename, bool raiseerror);

            [DllImport(SqLib, EntryPoint = "sq_compilebuffer", CallingConvention = CallingConvention.Cdecl)]
            extern public static int CompileBuffer(IntPtr v, [MarshalAs(SqString)] string s, int size, [MarshalAs(SqString)] string sourcename, bool raiseerror);

            [DllImport(SqLib, EntryPoint = "sq_enabledebuginfo", CallingConvention = CallingConvention.Cdecl)]
            extern public static void EnableDebugInfo(IntPtr v, bool enable);

            [DllImport(SqLib, EntryPoint = "sq_notifyallexceptions", CallingConvention = CallingConvention.Cdecl)]
            extern public static void NotifyAllExceptions(IntPtr v, bool enable);

            [DllImport(SqLib, EntryPoint = "sq_setcompilererrorhandler", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetCompilerErrorHandler(IntPtr v, SqCompilerError errorfunc);
            #endregion

            #region Stack Operations
            [DllImport(SqLib, EntryPoint = "sq_cmp", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Cmp(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_gettop", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetTop(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_pop", CallingConvention = CallingConvention.Cdecl)]
            extern public static void Pop(IntPtr v, int nelementstopop);

            [DllImport(SqLib, EntryPoint = "sq_poptop", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PopTop(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_push", CallingConvention = CallingConvention.Cdecl)]
            extern public static void Push(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_remove", CallingConvention = CallingConvention.Cdecl)]
            extern public static void Remove(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_reservestack", CallingConvention = CallingConvention.Cdecl)]
            extern public static void ReserveStack(IntPtr v, int nsize);

            [DllImport(SqLib, EntryPoint = "sq_settop", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetTop(IntPtr v, int idx);
            #endregion

            #region Object Creation and Handling
            [DllImport(SqLib, EntryPoint = "sq_bindenv", CallingConvention = CallingConvention.Cdecl)]
            extern public static int BindEnv(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_createinstance", CallingConvention = CallingConvention.Cdecl)]
            extern public static int CreateInstance(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getbool", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetBool(IntPtr v, int idx, out bool b);

            [DllImport(SqLib, EntryPoint = "sq_getbyhandle", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetByHandle(IntPtr v, int idx, IntPtr handle);

            [DllImport(SqLib, EntryPoint = "sq_getclosureinfo", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetClosureInfo(IntPtr v, int idx, out uint nparams, out uint nfreevars);

            [DllImport(SqLib, EntryPoint = "sq_getclosurename", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetClosureName(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getfloat", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetFloat(IntPtr v, int idx, out float f);

            [DllImport(SqLib, EntryPoint = "sq_gethash", CallingConvention = CallingConvention.Cdecl)]
            extern public static uint GetHash(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getinstanceup", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetInstanceUserPointer(IntPtr v, int idx, out IntPtr up, IntPtr typetag);

            [DllImport(SqLib, EntryPoint = "sq_getinteger", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetInteger(IntPtr v, int idx, out int i);

            [DllImport(SqLib, EntryPoint = "sq_getmemberhandle", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetMemberHandle(IntPtr v, int idx, out IntPtr handle);

            [DllImport(SqLib, EntryPoint = "sq_getscratchpad", CallingConvention = CallingConvention.Cdecl)]
            extern private static IntPtr _GetScratchPad(IntPtr v, int minsize);
            public static string GetScratchPad(IntPtr v, int minsize)
            {
#if SQUNICODE
            return Marshal.PtrToStringUni(_GetScratchPad(v, minsize));
#else
                return Marshal.PtrToStringAnsi(_GetScratchPad(v, minsize));
#endif
            }

            [DllImport(SqLib, EntryPoint = "sq_getsize", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetSize(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getstring", CallingConvention = CallingConvention.Cdecl)]
            extern private static int _GetString(IntPtr v, int idx, out IntPtr c);
            public static int GetString(IntPtr v, int idx, out string c)
            {
                IntPtr ptr = IntPtr.Zero;
                int ret = _GetString(v, idx, out ptr);
#if SQUNICODE
            c = Marshal.PtrToStringUni(ptr);
#else
                c = Marshal.PtrToStringAnsi(ptr);
#endif
                return ret;
            }

            [DllImport(SqLib, EntryPoint = "sq_getthread", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetThread(IntPtr v, int idx, out IntPtr thread);

            [DllImport(SqLib, EntryPoint = "sq_gettype", CallingConvention = CallingConvention.Cdecl)]
            extern public static ObjectType GetType(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_gettypetag", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetTypeTag(IntPtr v, int idx, out IntPtr typetag);

            [DllImport(SqLib, EntryPoint = "sq_getuserdata", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetUserData(IntPtr v, int idx, out IntPtr p, out IntPtr typetag);

            [DllImport(SqLib, EntryPoint = "sq_getuserpointer", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetUserPointer(IntPtr v, int idx, out IntPtr p);

            [DllImport(SqLib, EntryPoint = "sq_newarray", CallingConvention = CallingConvention.Cdecl)]
            extern public static void NewArray(IntPtr v, int size);

            [DllImport(SqLib, EntryPoint = "sq_newclass", CallingConvention = CallingConvention.Cdecl)]
            extern public static int NewClass(IntPtr v, bool hasbase);

            [DllImport(SqLib, EntryPoint = "sq_newclosure", CallingConvention = CallingConvention.Cdecl)]
            extern public static void NewClosure(IntPtr v, SqFunction func, int nfreevars);

            [DllImport(SqLib, EntryPoint = "sq_newtable", CallingConvention = CallingConvention.Cdecl)]
            extern public static void NewTable(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_newtableex", CallingConvention = CallingConvention.Cdecl)]
            extern public static void NewTableEx(IntPtr v, int initialcapacity);

            [DllImport(SqLib, EntryPoint = "sq_newuserdata", CallingConvention = CallingConvention.Cdecl)]
            extern public static IntPtr NewUserData(IntPtr v, uint size);

            [DllImport(SqLib, EntryPoint = "sq_pushbool", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushBool(IntPtr v, bool b);

            [DllImport(SqLib, EntryPoint = "sq_pushfloat", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushFloat(IntPtr v, float f);

            [DllImport(SqLib, EntryPoint = "sq_pushinteger", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushInteger(IntPtr v, int n);

            [DllImport(SqLib, EntryPoint = "sq_pushnull", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushNull(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_pushstring", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushString(IntPtr v, [MarshalAs(SqString)] string s, int len);

            [DllImport(SqLib, EntryPoint = "sq_pushuserpointer", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushUserPointer(IntPtr v, IntPtr p);

            [DllImport(SqLib, EntryPoint = "sq_setbyhandle", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetByHandle(IntPtr v, int idx, IntPtr handle);

            [DllImport(SqLib, EntryPoint = "sq_setclassudsize", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetClassUserDataSize(IntPtr v, int idx, int udsize);

            [DllImport(SqLib, EntryPoint = "sq_setinstanceup", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetInstanceUserPointer(IntPtr v, int idx, IntPtr up);

            [DllImport(SqLib, EntryPoint = "sq_setnativeclosurename", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetNativeClosureName(IntPtr v, int idx, [MarshalAs(SqString)] string name);

            [DllImport(SqLib, EntryPoint = "sq_setparamscheck", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetParamsCheck(IntPtr v, int nparamscheck, [MarshalAs(SqString)] string typemask);

            [DllImport(SqLib, EntryPoint = "sq_setreleasehook", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetReleaseHook(IntPtr v, int idx, SqReleaseHook hook);

            [DllImport(SqLib, EntryPoint = "sq_settypetag", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetTypeTag(IntPtr v, int idx, IntPtr typetag);

            [DllImport(SqLib, EntryPoint = "sq_tobool", CallingConvention = CallingConvention.Cdecl)]
            extern public static void ToBool(IntPtr v, int idx, out bool b);

            [DllImport(SqLib, EntryPoint = "sq_tostring", CallingConvention = CallingConvention.Cdecl)]
            extern public static void ToString(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_typeof", CallingConvention = CallingConvention.Cdecl)]
            extern public static ObjectType TypeOf(IntPtr v, int idx);
            #endregion

            #region Calls
            [DllImport(SqLib, EntryPoint = "sq_call", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Call(IntPtr v, int nparams, bool retval, bool raiseerror);

            [DllImport(SqLib, EntryPoint = "sq_getcallee", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetCallee(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_getlasterror", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetLastError(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_getlocal", CallingConvention = CallingConvention.Cdecl)]
            extern private static IntPtr _GetLocal(IntPtr v, uint level, uint nseq);
            public static string GetLocal(IntPtr v, uint level, uint nseq)
            {
#if SQUNICODE
            return Marshal.PtrToStringUni(_GetLocal(v, level, nseq));
#else
                return Marshal.PtrToStringAnsi(_GetLocal(v, level, nseq));
#endif
            }

            [DllImport(SqLib, EntryPoint = "sq_reseterror", CallingConvention = CallingConvention.Cdecl)]
            extern public static void ResetError(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_resume", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Resume(IntPtr v, bool retval, bool raiseerror);

            [DllImport(SqLib, EntryPoint = "sq_throwerror", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ThrowError(IntPtr v, [MarshalAs(SqString)] string err);

            [DllImport(SqLib, EntryPoint = "sq_throwobject", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ThrowObject(IntPtr v);
            #endregion

            #region Object Manipulation
            [DllImport(SqLib, EntryPoint = "sq_arrayappend", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ArrayAppend(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_arrayinsert", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ArrayInsert(IntPtr v, int idx, int destpos);

            [DllImport(SqLib, EntryPoint = "sq_arraypop", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ArrayPop(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_arrayremove", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ArrayRemove(IntPtr v, int idx, int itemidx);

            [DllImport(SqLib, EntryPoint = "sq_arrayresize", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ArrayResize(IntPtr v, int idx, int newsize);

            [DllImport(SqLib, EntryPoint = "sq_arrayreverse", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ArrayReverse(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_clear", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Clear(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_clone", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Clone(IntPtr v, int idx);

            public static int CreateSlot(IntPtr v, int idx) { return NewSlot(v, idx, false); } // sq_createslot is a macro

            [DllImport(SqLib, EntryPoint = "sq_deleteslot", CallingConvention = CallingConvention.Cdecl)]
            extern public static int DeleteSlot(IntPtr v, int idx, bool pushval);

            [DllImport(SqLib, EntryPoint = "sq_get", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Get(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getattributes", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetAttributes(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getclass", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetClass(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getdelegate", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetDelegate(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_getfreevariable", CallingConvention = CallingConvention.Cdecl)]
            extern private static IntPtr _GetFreeVariable(IntPtr v, int idx, int nval);
            public static string GetFreeVariable(IntPtr v, int idx, int nval)
            {
#if SQUNICODE
            return Marshal.PtrToStringUni(_GetFreeVariable(v, idx, nval));
#else
                return Marshal.PtrToStringAnsi(_GetFreeVariable(v, idx, nval));
#endif
            }

            [DllImport(SqLib, EntryPoint = "sq_getweakrefval", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetWeakRefVal(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_instanceof", CallingConvention = CallingConvention.Cdecl)]
            extern public static bool InstanceOf(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_newmember", CallingConvention = CallingConvention.Cdecl)]
            extern public static int NewMember(IntPtr v, int idx, bool bstatic);

            [DllImport(SqLib, EntryPoint = "sq_newslot", CallingConvention = CallingConvention.Cdecl)]
            extern public static int NewSlot(IntPtr v, int idx, bool bstatic);

            [DllImport(SqLib, EntryPoint = "sq_next", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Next(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_rawdeleteslot", CallingConvention = CallingConvention.Cdecl)]
            extern public static int RawDeleteSlot(IntPtr v, int idx, bool pushval);

            [DllImport(SqLib, EntryPoint = "sq_rawget", CallingConvention = CallingConvention.Cdecl)]
            extern public static int RawGet(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_rawnewmember", CallingConvention = CallingConvention.Cdecl)]
            extern public static int RawNewMember(IntPtr v, int idx, bool bstatic);

            [DllImport(SqLib, EntryPoint = "sq_rawset", CallingConvention = CallingConvention.Cdecl)]
            extern public static int RawSet(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_set", CallingConvention = CallingConvention.Cdecl)]
            extern public static int Set(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_setattributes", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetAttributes(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_setdelegate", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetDelegate(IntPtr v, int idx);

            [DllImport(SqLib, EntryPoint = "sq_setfreevariable", CallingConvention = CallingConvention.Cdecl)]
            extern public static int SetFreeVariable(IntPtr v, int idx, int nval);

            [DllImport(SqLib, EntryPoint = "sq_weakref", CallingConvention = CallingConvention.Cdecl)]
            extern public static void WeakRef(IntPtr v, int idx);
            #endregion

            #region Bytecode Serialization
            [DllImport(SqLib, EntryPoint = "sq_readclosure", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ReadClosure(IntPtr v, SqReadFunc readf, IntPtr up);

            [DllImport(SqLib, EntryPoint = "sq_writeclosure", CallingConvention = CallingConvention.Cdecl)]
            extern public static int WriteClosure(IntPtr v, SqWriteFunc writef, IntPtr up);
            #endregion

            #region Raw Object Handling
            [DllImport(SqLib, EntryPoint = "sq_addout", CallingConvention = CallingConvention.Cdecl)]
            extern public static void AddRef(IntPtr v, out Object po);

            [DllImport(SqLib, EntryPoint = "sq_getobjtypetag", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetObjTypeTag(IntPtr v, out IntPtr typetag);

            [DllImport(SqLib, EntryPoint = "sq_getoutcount", CallingConvention = CallingConvention.Cdecl)]
            extern public static uint GetRefCount(IntPtr v, out Object po);

            [DllImport(SqLib, EntryPoint = "sq_getstackobj", CallingConvention = CallingConvention.Cdecl)]
            extern public static int GetStackObj(IntPtr v, int idx, out Object po);

            [DllImport(SqLib, EntryPoint = "sq_objtobool", CallingConvention = CallingConvention.Cdecl)]
            extern public static bool ObjToBool(out Object po);

            [DllImport(SqLib, EntryPoint = "sq_objtofloat", CallingConvention = CallingConvention.Cdecl)]
            extern public static float ObjToFloat(out Object po);

            [DllImport(SqLib, EntryPoint = "sq_objtointeger", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ObjToInteger(out Object po);

            [DllImport(SqLib, EntryPoint = "sq_objtostring", CallingConvention = CallingConvention.Cdecl)]
            extern private static IntPtr _ObjToString(out Object po);
            public static string ObjToString(out Object po)
            {
#if SQUNICODE
            return Marshal.PtrToStringUni(_ObjToString(out po));
#else
                return Marshal.PtrToStringAnsi(_ObjToString(out po));
#endif
            }

            [DllImport(SqLib, EntryPoint = "sq_objtouserpointer", CallingConvention = CallingConvention.Cdecl)]
            extern public static IntPtr ObjToUserPointer(out Object po);

            [DllImport(SqLib, EntryPoint = "sq_pushobject", CallingConvention = CallingConvention.Cdecl)]
            extern public static void PushObject(IntPtr v, Object po);

            [DllImport(SqLib, EntryPoint = "sq_release", CallingConvention = CallingConvention.Cdecl)]
            extern public static bool Release(IntPtr v, out Object po);

            [DllImport(SqLib, EntryPoint = "sq_resetobject", CallingConvention = CallingConvention.Cdecl)]
            extern public static void ResetObject(out Object po);
            #endregion

            #region Garbage Collector
            [DllImport(SqLib, EntryPoint = "sq_collectgarbage", CallingConvention = CallingConvention.Cdecl)]
            extern public static int CollectGarbage(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_resurrectunreachable", CallingConvention = CallingConvention.Cdecl)]
            extern public static int ResurrectUnreachable(IntPtr v);
            #endregion

            #region Debug Interface
            [DllImport(SqLib, EntryPoint = "sq_getfunctioninfo", CallingConvention = CallingConvention.Cdecl)]
            extern private static int _GetFunctionInfo(IntPtr v, int level, out IFunctionInfo fi);
            public static int GetFunctionInfo(IntPtr v, int level, out FunctionInfo fi)
            {
                var info = new IFunctionInfo { };
                int ret = _GetFunctionInfo(v, level, out info);

                fi = new FunctionInfo
                {
                    FuncId = info.FuncId,
#if SQUNICODE
                Name = Marshal.PtrToStringUni(info.Name),
                Source = Marshal.PtrToStringUni(info.Source),
#else
                    Name = Marshal.PtrToStringAnsi(info.Name),
                    Source = Marshal.PtrToStringAnsi(info.Source),
#endif
                };

                return ret;
            }

            [DllImport(SqLib, EntryPoint = "sq_setdebughook", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetDebugHook(IntPtr v);

            [DllImport(SqLib, EntryPoint = "sq_setnativedebughook", CallingConvention = CallingConvention.Cdecl)]
            extern public static void SetNativeDebugHook(IntPtr v, SqDebugHook hook);

            [DllImport(SqLib, EntryPoint = "sq_stackinfos", CallingConvention = CallingConvention.Cdecl)]
            extern private static int _StackInfos(IntPtr v, int level, out IStackInfo si);
            public static int StackInfos(IntPtr v, int level, out StackInfo si)
            {
                var info = new IStackInfo { };
                int ret = _StackInfos(v, level, out info);

                si = new StackInfo
                {
                    Line = info.Line,
#if SQUNICODE
                FuncName = Marshal.PtrToStringUni(info.FuncName),
                Source = Marshal.PtrToStringUni(info.Source),
#else
                    FuncName = Marshal.PtrToStringAnsi(info.FuncName),
                    Source = Marshal.PtrToStringAnsi(info.Source),
#endif
                };

                return ret;
            }
            #endregion

            #region Standard Library
            /// <summary>Squirrel Standard Library</summary>
            public static class Std
            {

                // SqStdAux
                [DllImport(SqStdLib, EntryPoint = "sqstd_seterrorhandlers", CallingConvention = CallingConvention.Cdecl)]
                extern public static void SetErrorHandlers(IntPtr v);

                [DllImport(SqStdLib, EntryPoint = "sqstd_printcallstack", CallingConvention = CallingConvention.Cdecl)]
                extern public static void PrintCallStack(IntPtr v);

                // SqStdBlob
                // sqstd_createblob
                // sqstd_getblob
                // sqstd_getblobsize

                [DllImport(SqStdLib, EntryPoint = "sqstd_register_bloblib", CallingConvention = CallingConvention.Cdecl)]
                extern public static int RegisterBlobLib(IntPtr v);

                // SqStdIO
                // sqstd_fopen
                // sqstd_fread
                // sqstd_fwrite
                // sqstd_fseek
                // sqstd_ftell
                // sqstd_fflush
                // sqstd_fclose
                // sqstd_feof
                // sqstd_createfile
                // sqstd_getfile

                [DllImport(SqStdLib, EntryPoint = "sqstd_loadfile", CallingConvention = CallingConvention.Cdecl)]
                extern public static int LoadFile(IntPtr v, [MarshalAs(SqString)] string filename, bool printerror);

                [DllImport(SqStdLib, EntryPoint = "sqstd_dofile", CallingConvention = CallingConvention.Cdecl)]
                extern public static int DoFile(IntPtr v, [MarshalAs(SqString)] string filename, bool retval, bool printerror);

                [DllImport(SqStdLib, EntryPoint = "sqstd_writeclosuretofile", CallingConvention = CallingConvention.Cdecl)]
                extern public static int WriteClosureToFile(IntPtr v, [MarshalAs(SqString)] string filename);

                [DllImport(SqStdLib, EntryPoint = "sqstd_register_iolib", CallingConvention = CallingConvention.Cdecl)]
                extern public static int RegisterIOLib(IntPtr v);

                // SqStdMath
                [DllImport(SqStdLib, EntryPoint = "sqstd_register_mathlib", CallingConvention = CallingConvention.Cdecl)]
                extern public static int RegisterMathLib(IntPtr v);

                // SqStdRex
                // sqstd_rex_compile
                // sqstd_rex_free
                // sqstd_rex_match
                // sqstd_rex_searchrange
                // sqstd_rex_search
                // sqstd_rex_getsubexpcount
                // sqstd_rex_getsubexp

                // SqStdString
                [DllImport(SqStdLib, EntryPoint = "sqstd_format", CallingConvention = CallingConvention.Cdecl)]
                extern public static int _Format(IntPtr v, int nformatstringidx, out int outlen, out IntPtr output);
                public static int Format(IntPtr v, int nformatstringidx, out int outlen, out string output)
                {
                    IntPtr ptr = IntPtr.Zero;
                    int ret = _Format(v, nformatstringidx, out outlen, out ptr);

#if SQUNICODE
                output = Marshal.PtrToStringUni(ptr);
#else
                    output = Marshal.PtrToStringAnsi(ptr);
#endif

                    return ret;
                }

                [DllImport(SqStdLib, EntryPoint = "sqstd_register_stringlib", CallingConvention = CallingConvention.Cdecl)]
                extern public static int RegisterStringLib(IntPtr v);

                // SqStdSystem
                [DllImport(SqStdLib, EntryPoint = "sqstd_register_systemlib", CallingConvention = CallingConvention.Cdecl)]
                extern public static int RegisterSystemLib(IntPtr v);
            }
            #endregion
        }
    }
}
