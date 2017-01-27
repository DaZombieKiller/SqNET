using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_compile", CallingConvention = CallConv)]
        extern public static int Compile(IntPtr v, SqLexReadFunc read, IntPtr p, [MarshalAs(StringType)] string sourcename, bool raiseerror);

        [DllImport(DllName, EntryPoint = "sq_compilebuffer", CallingConvention = CallConv)]
        extern public static int CompileBuffer(IntPtr v, [MarshalAs(StringType)] string s, int size, [MarshalAs(StringType)] string sourcename, bool raiseerror);

        [DllImport(DllName, EntryPoint = "sq_enabledebuginfo", CallingConvention = CallConv)]
        extern public static void EnableDebugInfo(IntPtr v, bool enable);

        [DllImport(DllName, EntryPoint = "sq_notifyallexceptions", CallingConvention = CallConv)]
        extern public static void NotifyAllExceptions(IntPtr v, bool enable);

        [DllImport(DllName, EntryPoint = "sq_setcompilererrorhandler", CallingConvention = CallConv)]
        extern private static void NativeSetCompilerErrorHandler(IntPtr v, ISqCompilerError errorfunc);
        public static void SetCompilerErrorHandler(IntPtr v, SqCompilerError errorfunc)
        {
            NativeSetCompilerErrorHandler(v, (vm, desc, source, line, column) =>
            {
                errorfunc(new VM(vm), desc, source, line, column);
            });
        }
    }
}
