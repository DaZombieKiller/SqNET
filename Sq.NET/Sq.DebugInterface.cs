using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_getfunctioninfo", CallingConvention = CallConv)]
        extern private static int NativeGetFunctionInfo(IntPtr v, int level, out IFunctionInfo fi);
        public static int GetFunctionInfo(IntPtr v, int level, out FunctionInfo fi)
        {
            IFunctionInfo ifi = new IFunctionInfo { };
            int ret = NativeGetFunctionInfo(v, level, out ifi);

            fi.funcid = ifi.funcid;
            fi.name = Marshal.PtrToStringUni(ifi.name);
            fi.source = Marshal.PtrToStringUni(ifi.source);

            return ret;
        }

        [DllImport(DllName, EntryPoint = "sq_setdebughook", CallingConvention = CallConv)]
        extern public static void SetDebugHook(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_setnativedebughook", CallingConvention = CallConv)]
        extern private static void NativeSetNativeDebugHook(IntPtr v, ISqDebugHook hook);
        public static void SetNativeDebugHook(IntPtr v, SqDebugHook hook)
        {
            NativeSetNativeDebugHook(v, (vm, type, sourcename, line, funcname) =>
            {
                hook(new VM(v), type, sourcename, line, funcname);
            });
        }

        [DllImport(DllName, EntryPoint = "sq_stackinfos", CallingConvention = CallConv)]
        extern private static int NativeStackInfos(IntPtr v, int level, out IStackInfo si);
        public static int GetStackInfo(IntPtr v, int level, out StackInfo si)
        {
            IStackInfo isi = new IStackInfo { };
            int ret = NativeStackInfos(v, level, out isi);

            si.funcname = Marshal.PtrToStringUni(isi.funcname);
            si.source = Marshal.PtrToStringUni(isi.source);
            si.line = isi.line;

            return ret;
        }
    }
}
