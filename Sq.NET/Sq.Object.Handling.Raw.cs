using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_addout")]
        extern public static void AddRef(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_getobjtypetag")]
        extern public static int GetObjTypeTag(IntPtr v, out IntPtr typetag);

        [DllImport(DllName, EntryPoint = "sq_getoutcount")]
        extern public static uint GetRefCount(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_getstackobj")]
        extern public static int GetStackObj(IntPtr v, int idx, out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtobool")]
        extern public static bool ObjToBool(out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtofloat")]
#if SQUSEDOUBLE
        extern public static double ObjToFloat(out Object po);
        public static double ObjToDouble(out Object po) => ObjToFloat(out po);
#else
        extern public static float ObjToFloat(out Object po);
        public static float ObjToDouble(out Object po) => ObjToFloat(out po);
#endif

        [DllImport(DllName, EntryPoint = "sq_objtointeger")]
        extern public static int ObjToInteger(out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtostring")]
        extern private static IntPtr NativeObjToString(out Object po);
        public static string ObjToString(out Object po)
            => Marshal.PtrToStringUni(NativeObjToString(out po));

        [DllImport(DllName, EntryPoint = "sq_objtouserpointer")]
        extern public static IntPtr ObjToUserPointer(out Object po);

        [DllImport(DllName, EntryPoint = "sq_pushobject")]
        extern public static void PushObject(IntPtr v, Object po);

        [DllImport(DllName, EntryPoint = "sq_release")]
        extern public static bool Release(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_resetobject")]
        extern public static void ResetObject(out Object po);
    }
}
