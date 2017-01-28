using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
	public static partial class Sq
	{
        public static partial class Std
        {
            // SqStdAux
            [DllImport(DllName, EntryPoint = "sqstd_seterrorhandlers", CallingConvention = CallingConvention.StdCall)]
            extern public static void SetErrorHandlers(IntPtr v);
            public static void SetErrorHandlers(VM v) => SetErrorHandlers(v.Pointer);

            [DllImport(DllName, EntryPoint = "sqstd_printcallstack", CallingConvention = CallingConvention.StdCall)]
            extern public static void PrintCallStack(IntPtr v);
            public static void PrintCallStack(VM v) => PrintCallStack(v.Pointer);

            // SqStdBlob
            // sqstd_createblob
            // sqstd_getblob
            // sqstd_getblobsize

            [DllImport(DllName, EntryPoint = "sqstd_register_bloblib", CallingConvention = CallingConvention.StdCall)]
            extern public static int RegisterBlobLib(IntPtr v);
            public static int RegisterBlobLib(VM v) => RegisterBlobLib(v.Pointer);

            // SqStdIO
            // sqstd_fopen
            // sqstd_fread
            // sqstd_fwrite
            // sqstd_fseek
            // sqstd_ftell
            // sqstd_fflush
            // sqstd_fclose
            // sqstd_feof
            // sqstd_createfile
            // sqstd_getfile

            [DllImport(DllName, EntryPoint = "sqstd_loadfile", CallingConvention = CallingConvention.StdCall)]
            extern public static int LoadFile(IntPtr v, [MarshalAs(StringType)] string filename, bool printerror);
            public static int LoadFile(VM v, string filename, bool printerror) => LoadFile(v.Pointer, filename, printerror);

            [DllImport(DllName, EntryPoint = "sqstd_dofile", CallingConvention = CallingConvention.StdCall)]
            extern public static int DoFile(IntPtr v, [MarshalAs(StringType)] string filename, bool retval, bool printerror);
            public static int DoFile(VM v, string filename, bool retval, bool printerror) => DoFile(v.Pointer, filename, retval, printerror);

            [DllImport(DllName, EntryPoint = "sqstd_writeclosuretofile", CallingConvention = CallingConvention.StdCall)]
            extern public static int WriteClosureToFile(IntPtr v, [MarshalAs(StringType)] string filename);
            public static int WriteClosureToFile(VM v, string filename) => WriteClosureToFile(v.Pointer, filename);

            [DllImport(DllName, EntryPoint = "sqstd_register_iolib", CallingConvention = CallingConvention.StdCall)]
            extern public static int RegisterIOLib(IntPtr v);
            public static int RegisterIOLib(VM v) => RegisterIOLib(v.Pointer);

            // SqStdMath
            [DllImport(DllName, EntryPoint = "sqstd_register_mathlib", CallingConvention = CallingConvention.StdCall)]
            extern public static int RegisterMathLib(IntPtr v);
            public static int RegisterMathLib(VM v) => RegisterMathLib(v.Pointer);

            // SqStdRex
            // sqstd_rex_compile
            // sqstd_rex_free
            // sqstd_rex_match
            // sqstd_rex_searchrange
            // sqstd_rex_search
            // sqstd_rex_getsubexpcount
            // sqstd_rex_getsubexp

            // SqStdString
            [DllImport(DllName, EntryPoint = "sqstd_format", CallingConvention = CallingConvention.StdCall)]
            extern private static int NativeFormat(IntPtr v, int nformatstringidx, out int outlen, out IntPtr output);
            public static int Format(VM v, int nformatstringidx, out int outlen, out string output) => Format(v.Pointer, nformatstringidx, out outlen, out output);
            public static int Format(IntPtr v, int nformatstringidx, out int outlen, out string output)
            {
                IntPtr ptr = IntPtr.Zero;
                int ret = NativeFormat(v, nformatstringidx, out outlen, out ptr);
                output = Marshal.PtrToStringUni(ptr);
                return ret;
            }

            [DllImport(DllName, EntryPoint = "sqstd_register_stringlib", CallingConvention = CallingConvention.StdCall)]
            extern public static int RegisterStringLib(IntPtr v);
            public static int RegisterStringLib(VM v) => RegisterStringLib(v.Pointer);

            // SqStdSystem
            [DllImport(DllName, EntryPoint = "sqstd_register_systemlib", CallingConvention = CallingConvention.StdCall)]
            extern public static int RegisterSystemLib(IntPtr v);
            public static int RegisterSystemLib(VM v) => RegisterSystemLib(v.Pointer);
        }
    }
}
