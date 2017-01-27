using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_cmp")]
        extern public static int Cmp(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_gettop")]
        extern public static int GetTop(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_pop")]
        extern public static void Pop(IntPtr v, int nelementstopop);

        [DllImport(DllName, EntryPoint = "sq_poptop")]
        extern public static void PopTop(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_push")]
        extern public static void Push(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_remove")]
        extern public static void Remove(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_reservestack")]
        extern public static void ReserveStack(IntPtr v, int nsize);

        [DllImport(DllName, EntryPoint = "sq_settop")]
        extern public static void SetTop(IntPtr v, int idx);
    }
}
