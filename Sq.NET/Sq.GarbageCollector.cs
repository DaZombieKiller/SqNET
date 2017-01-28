using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_collectgarbage", CallingConvention = CallingConvention.StdCall)]
        extern public static int CollectGarbage(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_resurrectunreachable", CallingConvention = CallingConvention.StdCall)]
        extern public static int ResurrectUnreachable(IntPtr v);
    }
}
