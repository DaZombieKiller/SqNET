using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_arrayappend")]
        extern public static int ArrayAppend(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_arrayinsert")]
        extern public static int ArrayInsert(IntPtr v, int idx, int destpos);

        [DllImport(DllName, EntryPoint = "sq_arraypop")]
        extern public static int ArrayPop(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_arrayremove")]
        extern public static int ArrayRemove(IntPtr v, int idx, int itemidx);

        [DllImport(DllName, EntryPoint = "sq_arrayresize")]
        extern public static int ArrayResize(IntPtr v, int idx, int newsize);

        [DllImport(DllName, EntryPoint = "sq_arrayreverse")]
        extern public static int ArrayReverse(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_clear")]
        extern public static int Clear(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_clone")]
        extern public static int Clone(IntPtr v, int idx);

        // sq_createslot is a macro
        public static int CreateSlot(IntPtr v, int idx)
            => NewSlot(v, idx, false);

        [DllImport(DllName, EntryPoint = "sq_deleteslot")]
        extern public static int DeleteSlot(IntPtr v, int idx, bool pushval);

        [DllImport(DllName, EntryPoint = "sq_get")]
        extern public static int Get(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getattributes")]
        extern public static int GetAttributes(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getclass")]
        extern public static int GetClass(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getdelegate")]
        extern public static int GetDelegate(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_getfreevariable")]
        extern private static IntPtr NativeGetFreeVariable(IntPtr v, int idx, int nval);
        public static string GetFreeVariable(IntPtr v, int idx, int nval)
            => Marshal.PtrToStringUni(NativeGetFreeVariable(v, idx, nval));

        [DllImport(DllName, EntryPoint = "sq_getweakrefval")]
        extern public static int GetWeakRefVal(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_instanceof")]
        extern public static bool InstanceOf(IntPtr v);

        [DllImport(DllName, EntryPoint = "sq_newmember")]
        extern public static int NewMember(IntPtr v, int idx, bool bstatic);

        [DllImport(DllName, EntryPoint = "sq_newslot")]
        extern public static int NewSlot(IntPtr v, int idx, bool bstatic);

        [DllImport(DllName, EntryPoint = "sq_next")]
        extern public static int Next(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_rawdeleteslot")]
        extern public static int RawDeleteSlot(IntPtr v, int idx, bool pushval);

        [DllImport(DllName, EntryPoint = "sq_rawget")]
        extern public static int RawGet(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_rawnewmember")]
        extern public static int RawNewMember(IntPtr v, int idx, bool bstatic);

        [DllImport(DllName, EntryPoint = "sq_rawset")]
        extern public static int RawSet(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_set")]
        extern public static int Set(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_setattributes")]
        extern public static int SetAttributes(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_setdelegate")]
        extern public static int SetDelegate(IntPtr v, int idx);

        [DllImport(DllName, EntryPoint = "sq_setfreevariable")]
        extern public static int SetFreeVariable(IntPtr v, int idx, int nval);

        [DllImport(DllName, EntryPoint = "sq_weakref")]
        extern public static void WeakRef(IntPtr v, int idx);
    }
}
