using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_cmp", CallingConvention = CallConv)]
        extern public static int Cmp(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_gettop", CallingConvention = CallConv)]
        extern public static int GetTop(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_pop", CallingConvention = CallConv)]
        extern public static void Pop(IntPtr v, int nelementstopop);

        [DllImport(DllName, EntryPoint = "sq_poptop", CallingConvention = CallConv)]
        extern public static void PopTop(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_push", CallingConvention = CallConv)]
        extern public static void Push(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_remove", CallingConvention = CallConv)]
        extern public static void Remove(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_reservestack", CallingConvention = CallConv)]
        extern public static void ReserveStack(IntPtr v, int nsize);

        [DllImport(DllName, EntryPoint = "sq_settop", CallingConvention = CallConv)]
        extern public static void SetTop(IntPtr v, int idx);
    }
}
