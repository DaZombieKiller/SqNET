using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_bindenv", CallingConvention = CallConv)]
        extern public static int BindEnv(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_createinstance", CallingConvention = CallConv)]
        extern public static int CreateInstance(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getbool", CallingConvention = CallConv)]
        extern public static int GetBool(IntPtr v, int idx, out bool b);

        [DllImport(DllName, EntryPoint = "sq_getbyhandle", CallingConvention = CallConv)]
        extern public static int GetByHandle(IntPtr v, int idx, IntPtr handle);

        [DllImport(DllName, EntryPoint = "sq_getclosureinfo", CallingConvention = CallConv)]
        extern public static int GetClosureInfo(IntPtr v, int idx, out uint nparams, out uint nfreevars);

        [DllImport(DllName, EntryPoint = "sq_getclosurename", CallingConvention = CallConv)]
        extern public static int GetClosureName(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getfloat", CallingConvention = CallConv)]
#if SQUSEDOUBLE
        extern public static int GetFloat(IntPtr v, int idx, out double f);
        public static int GetDouble(IntPtr v, int idx, out double f) => GetFloat(v, idx, out f);
#else
        extern public static int GetFloat(IntPtr v, int idx, out float f);
        public static int GetDouble(IntPtr v, int idx, out float f) => GetFloat(v, idx, out f);
#endif

        [DllImport(DllName, EntryPoint = "sq_gethash", CallingConvention = CallConv)]
        extern public static uint GetHash(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getinstanceup", CallingConvention = CallConv)]
        extern public static int GetInstanceUserPointer(IntPtr v, int idx, out IntPtr up, IntPtr typetag);

        [DllImport(DllName, EntryPoint = "sq_getinteger", CallingConvention = CallConv)]
        extern public static int GetInteger(IntPtr v, int idx, out int i);

        [DllImport(DllName, EntryPoint = "sq_getmemberhandle", CallingConvention = CallConv)]
        extern public static int GetMemberHandle(IntPtr v, int idx, out IntPtr handle);

        [DllImport(DllName, EntryPoint = "sq_getscratchpad", CallingConvention = CallConv)]
        extern private static IntPtr NativeGetScratchPad(IntPtr v, int minsize);
        public static string GetScratchPad(IntPtr v, int minsize)
            => Marshal.PtrToStringUni(NativeGetScratchPad(v, minsize));

        [DllImport(DllName, EntryPoint = "sq_getsize", CallingConvention = CallConv)]
        extern public static int GetSize(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getstring", CallingConvention = CallConv)]
        extern private static int NativeGetString(IntPtr v, int idx, out IntPtr c);

        public static int GetString(IntPtr v, int idx, out string c)
        {
            IntPtr ptr = IntPtr.Zero;
            int ret = NativeGetString(v, idx, out ptr);
            c = Marshal.PtrToStringUni(ptr);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "sq_getthread", CallingConvention = CallConv)]
        extern public static int GetThread(IntPtr v, int idx, out IntPtr thread);

        [DllImport(DllName, EntryPoint = "sq_gettype", CallingConvention = CallConv)]
        extern public static ObjectType GetType(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_gettypetag", CallingConvention = CallConv)]
        extern public static int GetTypeTag(IntPtr v, int idx, out IntPtr typetag);

        [DllImport(DllName, EntryPoint = "sq_getuserdata", CallingConvention = CallConv)]
        extern public static int GetUserData(IntPtr v, int idx, out IntPtr p, out IntPtr typetag);

        [DllImport(DllName, EntryPoint = "sq_getuserpointer", CallingConvention = CallConv)]
        extern public static int GetUserPointer(IntPtr v, int idx, out IntPtr p);

        [DllImport(DllName, EntryPoint = "sq_newarray", CallingConvention = CallConv)]
        extern public static void NewArray(IntPtr v, int size);

        [DllImport(DllName, EntryPoint = "sq_newclass", CallingConvention = CallConv)]
        extern public static int NewClass(IntPtr v, bool hasbase);

        [DllImport(DllName, EntryPoint = "sq_newclosure", CallingConvention = CallConv)]
        extern private static void NativeNewClosure(IntPtr v, ISqFunction func, int nfreevars);
        public static void NewClosure(IntPtr v, SqFunction func, int nfreevars)
        {
            NativeNewClosure(v, (vm) =>
            {
                return func(new VM(vm));
            }, nfreevars);
        }

        [DllImport(DllName, EntryPoint = "sq_newtable", CallingConvention = CallConv)]
        extern public static void NewTable(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_newtableex", CallingConvention = CallConv)]
        extern public static void NewTableEx(IntPtr v, int initialcapacity);

        [DllImport(DllName, EntryPoint = "sq_newuserdata", CallingConvention = CallConv)]
        extern public static IntPtr NewUserData(IntPtr v, uint size);

        [DllImport(DllName, EntryPoint = "sq_pushbool", CallingConvention = CallConv)]
        extern public static void PushBool(IntPtr v, bool b);

        [DllImport(DllName, EntryPoint = "sq_pushfloat", CallingConvention = CallConv)]
#if SQUSEDOUBLE
        extern public static void PushFloat(IntPtr v, double f);
        public static void PushDouble(IntPtr v, double f) => PushFloat(v, f);
#else
        extern public static void PushFloat(IntPtr v, float f);
        public static void PushDouble(IntPtr v, float f) => PushFloat(v, f);
#endif

        [DllImport(DllName, EntryPoint = "sq_pushinteger", CallingConvention = CallConv)]
        extern public static void PushInteger(IntPtr v, int n);

        [DllImport(DllName, EntryPoint = "sq_pushnull", CallingConvention = CallConv)]
        extern public static void PushNull(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_pushstring", CallingConvention = CallConv)]
        extern public static void PushString(IntPtr v, [MarshalAs(StringType)] string s, int len);

        [DllImport(DllName, EntryPoint = "sq_pushuserpointer", CallingConvention = CallConv)]
        extern public static void PushUserPointer(IntPtr v, IntPtr p);

        [DllImport(DllName, EntryPoint = "sq_setbyhandle", CallingConvention = CallConv)]
        extern public static int SetByHandle(IntPtr v, int idx, IntPtr handle);

        [DllImport(DllName, EntryPoint = "sq_setclassudsize", CallingConvention = CallConv)]
        extern public static int SetClassUserDataSize(IntPtr v, int idx, int udsize);

        [DllImport(DllName, EntryPoint = "sq_setinstanceup", CallingConvention = CallConv)]
        extern public static int SetInstanceUserPointer(IntPtr v, int idx, IntPtr up);

        [DllImport(DllName, EntryPoint = "sq_setnativeclosurename", CallingConvention = CallConv)]
        extern public static int SetNativeClosureName(IntPtr v, int idx, [MarshalAs(StringType)] string name);

        [DllImport(DllName, EntryPoint = "sq_setparamscheck", CallingConvention = CallConv)]
        extern public static int SetParamsCheck(IntPtr v, int nparamscheck, [MarshalAs(StringType)] string typemask);

        [DllImport(DllName, EntryPoint = "sq_setreleasehook", CallingConvention = CallConv)]
        extern public static void SetReleaseHook(IntPtr v, int idx, SqReleaseHook hook);

        [DllImport(DllName, EntryPoint = "sq_settypetag", CallingConvention = CallConv)]
        extern public static int SetTypeTag(IntPtr v, int idx, IntPtr typetag);

        [DllImport(DllName, EntryPoint = "sq_tobool", CallingConvention = CallConv)]
        extern public static void ToBool(IntPtr v, int idx, out bool b);

        [DllImport(DllName, EntryPoint = "sq_tostring", CallingConvention = CallConv)]
        extern public static void ToString(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_typeof", CallingConvention = CallConv)]
        extern public static ObjectType TypeOf(IntPtr v, int idx);
    }
}
