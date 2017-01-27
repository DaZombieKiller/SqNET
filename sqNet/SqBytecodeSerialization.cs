using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_readclosure")]
        extern public static int ReadClosure(IntPtr v, SqReadFunc readf, IntPtr up);

        [DllImport(DllName, EntryPoint = "sq_writeclosure")]
        extern public static int WriteClosure(IntPtr v, SqWriteFunc writef, IntPtr up);
    }
}
