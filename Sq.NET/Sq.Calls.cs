using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_call", CallingConvention = CallConv)]
        extern public static int Call(IntPtr v, int nparams, bool retval, bool raiseerror);

        [DllImport(DllName, EntryPoint = "sq_getcallee", CallingConvention = CallConv)]
        extern public static int GetCallee(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_getlasterror", CallingConvention = CallConv)]
        extern public static int GetLastError(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_getlocal", CallingConvention = CallConv)]
        extern private static IntPtr NativeGetLocal(IntPtr v, uint level, uint nseq);
        public static string GetLocal(IntPtr v, uint level, uint nseq)
            => Marshal.PtrToStringUni(NativeGetLocal(v, level, nseq));

        [DllImport(DllName, EntryPoint = "sq_reseterror", CallingConvention = CallConv)]
        extern public static void ResetError(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_resume", CallingConvention = CallConv)]
        extern public static int Resume(IntPtr v, bool retval, bool raiseerror);

        [DllImport(DllName, EntryPoint = "sq_throwerror", CallingConvention = CallConv)]
        extern public static int ThrowError(IntPtr v, [MarshalAs(StringType)] string err);

        [DllImport(DllName, EntryPoint = "sq_throwobject", CallingConvention = CallConv)]
        extern public static int ThrowObject(IntPtr v);
    }
}
