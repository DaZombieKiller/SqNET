using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_collectgarbage", CallingConvention = CallConv)]
        extern public static int CollectGarbage(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_resurrectunreachable", CallingConvention = CallConv)]
        extern public static int ResurrectUnreachable(IntPtr v);
    }
}
