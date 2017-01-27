using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
#if SQUNICODE
        public const UnmanagedType StringType = UnmanagedType.LPWStr;
#else
        // not supported, will not currently compile on the C++ side
        // not very difficult to fix, but not worth the effort overall
        public const UnmanagedType StringType = UnmanagedType.LPStr;
#endif

        // dll path
        public const string DllName = "squirrel.dll";

        // success/failure funcs
        public static bool Failed(int v) => v < 0;
        public static bool Succeeded(int v) => v >= 0;

        // delegates
        public delegate int SqFunction(VM v);
        public delegate void SqCompilerError(VM v, string desc, string source, int line, int column);
        public delegate void SqPrintFunction(VM v, string format, params object[] args);
        public delegate int SqReleaseHook(IntPtr p, int size);
        public delegate int SqReadFunc(IntPtr file, IntPtr buf, int size);
        public delegate int SqWriteFunc(IntPtr file, IntPtr p, int size);
        public delegate void SqDebugHook(VM v, int type, string sourcename, int line, string funcname);

        // internal delegates
        private delegate int ISqFunction(IntPtr v);
        private delegate void ISqCompilerError(IntPtr v, [MarshalAs(StringType)] string desc, [MarshalAs(StringType)] string source, int line, int column);
        private delegate void ISqPrintFunction(IntPtr v, [MarshalAs(StringType)] string format, int argc, IntPtr variant);
        private delegate void ISqDebugHook(IntPtr v, int type, [MarshalAs(StringType)] string sourcename, int line, [MarshalAs(StringType)] string funcname);

        // print functions
        private static Dictionary<IntPtr, SqPrintFunction> _printFunctions
            = new Dictionary<IntPtr, SqPrintFunction>();

        private static Dictionary<IntPtr, SqPrintFunction> _errorFunctions
            = new Dictionary<IntPtr, SqPrintFunction>();

        // user pointer
        private static object _userPointer;

        // structs
        public struct Object
        {
            // Object just needs to be a 16-byte struct
            // would it even be worth it to expose the actual members?
            // A struct with [StructLayout(LayoutKind.Explicit)] would be
            // needed to emulate the SQObjectValue union
            Int64 a;
            Int64 b;
        }

        public struct FunctionInfo
        {
            public IntPtr funcid;
            public string name;
            public string source;
        }

        public struct StackInfo
        {
            public string funcname;
            public string source;
            public int line;
        }

        // internal structs
        [StructLayout(LayoutKind.Sequential)]
        private struct IFunctionInfo
        {
            public IntPtr funcid;
            public IntPtr name;
            public IntPtr source;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IStackInfo
        {
            public IntPtr funcname;
            public IntPtr source;
            public int line;
        }

        // constants
        const int SQOBJECT_REF_COUNTED = 0x08000000;
        const int SQOBJECT_NUMERIC = 0x04000000;
        const int SQOBJECT_DELEGABLE = 0x02000000;
        const int SQOBJECT_CANBEFALSE = 0x01000000;

        const int _RT_NULL = 0x00000001;
        const int _RT_INTEGER = 0x00000002;
        const int _RT_FLOAT = 0x00000004;
        const int _RT_BOOL = 0x00000008;
        const int _RT_STRING = 0x00000010;
        const int _RT_TABLE = 0x00000020;
        const int _RT_ARRAY = 0x00000040;
        const int _RT_USERDATA = 0x00000080;
        const int _RT_CLOSURE = 0x00000100;
        const int _RT_NATIVECLOSURE = 0x00000200;
        const int _RT_GENERATOR = 0x00000400;
        const int _RT_USERPOINTER = 0x00000800;
        const int _RT_THREAD = 0x00001000;
        const int _RT_FUNCPROTO = 0x00002000;
        const int _RT_CLASS = 0x00004000;
        const int _RT_INSTANCE = 0x00008000;
        const int _RT_WEAKREF = 0x00010000;
        const int _RT_OUTER = 0x00020000;

        // enums
        public enum ObjectType
        {
            OT_NULL = (_RT_NULL | SQOBJECT_CANBEFALSE),
            OT_INTEGER = (_RT_INTEGER | SQOBJECT_NUMERIC | SQOBJECT_CANBEFALSE),
            OT_FLOAT = (_RT_FLOAT | SQOBJECT_NUMERIC | SQOBJECT_CANBEFALSE),
            OT_BOOL = (_RT_BOOL | SQOBJECT_CANBEFALSE),
            OT_STRING = (_RT_STRING | SQOBJECT_REF_COUNTED),
            OT_TABLE = (_RT_TABLE | SQOBJECT_REF_COUNTED | SQOBJECT_DELEGABLE),
            OT_ARRAY = (_RT_ARRAY | SQOBJECT_REF_COUNTED),
            OT_USERDATA = (_RT_USERDATA | SQOBJECT_REF_COUNTED | SQOBJECT_DELEGABLE),
            OT_CLOSURE = (_RT_CLOSURE | SQOBJECT_REF_COUNTED),
            OT_NATIVECLOSURE = (_RT_NATIVECLOSURE | SQOBJECT_REF_COUNTED),
            OT_GENERATOR = (_RT_GENERATOR | SQOBJECT_REF_COUNTED),
            OT_USERPOINTER = _RT_USERPOINTER,
            OT_THREAD = (_RT_THREAD | SQOBJECT_REF_COUNTED),
            OT_FUNCPROTO = (_RT_FUNCPROTO | SQOBJECT_REF_COUNTED), // internal usage only
            OT_CLASS = (_RT_CLASS | SQOBJECT_REF_COUNTED),
            OT_INSTANCE = (_RT_INSTANCE | SQOBJECT_REF_COUNTED | SQOBJECT_DELEGABLE),
            OT_WEAKREF = (_RT_WEAKREF | SQOBJECT_REF_COUNTED),
            OT_OUTER = (_RT_OUTER | SQOBJECT_REF_COUNTED) // internal usage only
        }
    }
}
