using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_collectgarbage")]
        extern public static int CollectGarbage(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_resurrectunreachable")]
        extern public static int ResurrectUnreachable(IntPtr v);
    }
}
