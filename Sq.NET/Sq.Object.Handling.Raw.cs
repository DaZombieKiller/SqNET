using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        [DllImport(DllName, EntryPoint = "sq_addout", CallingConvention = CallConv)]
        extern public static void AddRef(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_getobjtypetag", CallingConvention = CallConv)]
        extern public static int GetObjTypeTag(IntPtr v, out IntPtr typetag);

        [DllImport(DllName, EntryPoint = "sq_getoutcount", CallingConvention = CallConv)]
        extern public static uint GetRefCount(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_getstackobj", CallingConvention = CallConv)]
        extern public static int GetStackObj(IntPtr v, int idx, out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtobool", CallingConvention = CallConv)]
        extern public static bool ObjToBool(out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtofloat", CallingConvention = CallConv)]
#if SQUSEDOUBLE
        extern public static double ObjToFloat(out Object po);
        public static double ObjToDouble(out Object po) => ObjToFloat(out po);
#else
        extern public static float ObjToFloat(out Object po);
        public static float ObjToDouble(out Object po) => ObjToFloat(out po);
#endif

        [DllImport(DllName, EntryPoint = "sq_objtointeger", CallingConvention = CallConv)]
        extern public static int ObjToInteger(out Object po);

        [DllImport(DllName, EntryPoint = "sq_objtostring", CallingConvention = CallConv)]
        extern private static IntPtr NativeObjToString(out Object po);
        public static string ObjToString(out Object po)
            => Marshal.PtrToStringUni(NativeObjToString(out po));

        [DllImport(DllName, EntryPoint = "sq_objtouserpointer", CallingConvention = CallConv)]
        extern public static IntPtr ObjToUserPointer(out Object po);

        [DllImport(DllName, EntryPoint = "sq_pushobject", CallingConvention = CallConv)]
        extern public static void PushObject(IntPtr v, Object po);

        [DllImport(DllName, EntryPoint = "sq_release", CallingConvention = CallConv)]
        extern public static bool Release(IntPtr v, out Object po);

        [DllImport(DllName, EntryPoint = "sq_resetobject", CallingConvention = CallConv)]
        extern public static void ResetObject(out Object po);
    }
}
