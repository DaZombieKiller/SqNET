using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_addout", CallingConvention = CallingConvention.StdCall)]
        extern public static void AddRef(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_getobjtypetag", CallingConvention = CallingConvention.StdCall)]
        extern public static int GetObjTypeTag(IntPtr v, out IntPtr typetag);

        [DllImport(DllName, EntryPoint = "sq_getoutcount", CallingConvention = CallingConvention.StdCall)]
        extern public static uint GetRefCount(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_getstackobj", CallingConvention = CallingConvention.StdCall)]
        extern public static int GetStackObj(IntPtr v, int idx, out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtobool", CallingConvention = CallingConvention.StdCall)]
        extern public static bool ObjToBool(out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtofloat", CallingConvention = CallingConvention.StdCall)]
#if SQUSEDOUBLE
        extern public static double ObjToFloat(out Object po);
        public static double ObjToDouble(out Object po) => ObjToFloat(out po);
#else
        extern public static float ObjToFloat(out Object po);
        public static float ObjToDouble(out Object po) => ObjToFloat(out po);
#endif

        [DllImport(DllName, EntryPoint = "sq_objtointeger", CallingConvention = CallingConvention.StdCall)]
        extern public static int ObjToInteger(out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtostring", CallingConvention = CallingConvention.StdCall)]
        extern private static IntPtr NativeObjToString(out Object po);
        public static string ObjToString(out Object po)
            => Marshal.PtrToStringUni(NativeObjToString(out po));

        [DllImport(DllName, EntryPoint = "sq_objtouserpointer", CallingConvention = CallingConvention.StdCall)]
        extern public static IntPtr ObjToUserPointer(out Object po);

        [DllImport(DllName, EntryPoint = "sq_pushobject", CallingConvention = CallingConvention.StdCall)]
        extern public static void PushObject(IntPtr v, Object po);

        [DllImport(DllName, EntryPoint = "sq_release", CallingConvention = CallingConvention.StdCall)]
        extern public static bool Release(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_resetobject", CallingConvention = CallingConvention.StdCall)]
        extern public static void ResetObject(out Object po);
    }
}
