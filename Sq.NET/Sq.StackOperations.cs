using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_cmp", CallingConvention = CallingConvention.StdCall)]
        extern public static int Cmp(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_gettop", CallingConvention = CallingConvention.StdCall)]
        extern public static int GetTop(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_pop", CallingConvention = CallingConvention.StdCall)]
        extern public static void Pop(IntPtr v, int nelementstopop);

        [DllImport(DllName, EntryPoint = "sq_poptop", CallingConvention = CallingConvention.StdCall)]
        extern public static void PopTop(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_push", CallingConvention = CallingConvention.StdCall)]
        extern public static void Push(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_remove", CallingConvention = CallingConvention.StdCall)]
        extern public static void Remove(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_reservestack", CallingConvention = CallingConvention.StdCall)]
        extern public static void ReserveStack(IntPtr v, int nsize);

        [DllImport(DllName, EntryPoint = "sq_settop", CallingConvention = CallingConvention.StdCall)]
        extern public static void SetTop(IntPtr v, int idx);
    }
}
