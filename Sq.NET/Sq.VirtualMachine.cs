using System;
using System.Runtime.InteropServices;

namespace Squirrel
{
    public static partial class Sq
    {
        /// <summary>
        /// The execution state of a VM.
        /// </summary>
        public enum VMState
        {
            Idle,
            Running,
            Suspended
        }

        /// <summary>
        /// Releases a squirrel VM and all related friend VMs.
        /// </summary>
        /// <param name="v">The target VM</param>
        [DllImport(DllName, EntryPoint = "sq_close", CallingConvention = CallingConvention.StdCall)]
        extern public static void Close(IntPtr v);

        /// <summary>
        /// Returns the current error function of the given VM. (see <see cref="Sq.SetPrintFunc"/>)
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <returns>A pointer to a SQPrintFunction, or null if no function has been set.</returns>
        //[DllImport(DllName, EntryPoint = "sq_geterrorfunc", CallingConvention = CallingConvention.StdCall)]
        public static SqPrintFunction GetErrorFunc(IntPtr v)
            => _errorFunctions[v];

        /// <summary>
        /// Returns the foreign pointer of a VM instance.
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <returns>The current VM's foreign pointer</returns>
        [DllImport(DllName, EntryPoint = "sq_getforeignptr", CallingConvention = CallingConvention.StdCall)]
        extern public static IntPtr GetForeignPtr(IntPtr v);

        /// <summary>
        /// Returns the current print function of the given VM. (see <see cref="Sq.SetPrintFunc"/>)
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <returns>A pointer to a SQPrintFunction, or null if no function has been set.</returns>
        //[DllImport(DllName, EntryPoint = "sq_getprintfunc", CallingConvention = CallingConvention.StdCall)]
        public static SqPrintFunction GetPrintFunc(IntPtr v)
            => _printFunctions[v];

        /// <summary>
        /// Returns the version number of the VM.
        /// </summary>
        /// <returns>Version number of the VM (as in SQUIRREL_VERSION_NUMBER)</returns>
        [DllImport(DllName, EntryPoint = "sq_getversion", CallingConvention = CallingConvention.StdCall)]
        extern public static int GetVersion();

        /// <summary>
        /// Returns the execution state of a VM.
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <returns>The state of the VM encoded as an integer value. The following constants are defined: Idle, Running, Suspended</returns>
        [DllImport(DllName, EntryPoint = "sq_getvmstate", CallingConvention = CallingConvention.StdCall)]
        extern public static VMState GetVMState(IntPtr v);

        /// <summary>
        /// Pushes the object at the position 'idx' of the source VM stack in the destination VM stack.
        /// </summary>
        /// <param name="dest">The destination VM</param>
        /// <param name="src">The source VM</param>
        /// <param name="idx">The index in the source stack of the value that has to be moved</param>
        [DllImport(DllName, EntryPoint = "sq_move", CallingConvention = CallingConvention.StdCall)]
        extern public static void Move(IntPtr dest, IntPtr src, int idx);

        /// <summary>
        /// Creates a new VM FriendVM of the one passed as first parameter and pushes it in its stack as a "thread" object.
        /// </summary>
        /// <param name="friendvm">A friend VM</param>
        /// <param name="initialstacksize">The size of the stack in slots (number of objects)</param>
        /// <returns>A pointer to the new VM</returns>
        /// <remarks>By default the roottable is shared with the VM passed as first parameter. The new VM lifetime is bound to the "thread" object pushed in the stack and behave like a normal squirrel object.</remarks>
        [DllImport(DllName, EntryPoint = "sq_newthread", CallingConvention = CallingConvention.StdCall)]
        extern public static IntPtr NewThread(IntPtr friendvm, int initialstacksize);

        /// <summary>
        /// creates a new instance of a squirrel VM that consists in a new execution stack.
        /// </summary>
        /// <param name="initialstacksize">the size of the stack in slots (number of objects)</param>
        /// <returns>A handle to a squirrel vm</returns>
        /// <remarks>The returned VM has to be released with <see cref="Sq.Close(IntPtr)"/></remarks>
        [DllImport(DllName, EntryPoint = "sq_open", CallingConvention = CallingConvention.StdCall)]
        extern public static IntPtr Open(int initialstacksize);

        /// <summary>
        /// Pushes the current const table in the stack.
        /// </summary>
        /// <param name="v">The target VM</param>
        [DllImport(DllName, EntryPoint = "sq_pushconsttable", CallingConvention = CallingConvention.StdCall)]
        extern public static void PushConstTable(IntPtr v);

        /// <summary>
        /// Pushes the registry table in the stack.
        /// </summary>
        /// <param name="v">The target VM</param>
        [DllImport(DllName, EntryPoint = "sq_pushregistrytable", CallingConvention = CallingConvention.StdCall)]
        extern public static void PushRegistryTable(IntPtr v);

        /// <summary>
        /// Pushes the current root table in the stack.
        /// </summary>
        /// <param name="v">The target VM</param>
        [DllImport(DllName, EntryPoint = "sq_pushroottable", CallingConvention = CallingConvention.StdCall)]
        extern public static void PushRootTable(IntPtr v);

        /// <summary>
        /// Pops a table from the stack and sets it as the const table.
        /// </summary>
        /// <param name="v">The target VM</param>
        [DllImport(DllName, EntryPoint = "sq_setconsttable", CallingConvention = CallingConvention.StdCall)]
        extern public static void SetConstTable(IntPtr v);

        /// <summary>
        /// Pops from the stack a closure or native closure and sets it as the runtime-error handler.
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <remarks>The error handler is shared by friend VMs.</remarks>
        [DllImport(DllName, EntryPoint = "sq_seterrorhandler", CallingConvention = CallingConvention.StdCall)]
        extern public static void SetErrorHandler(IntPtr v);

        /// <summary>
        /// Sets the foreign pointer of a certain VM instance. The foreign pointer is an arbitrary user defined pointer associated to a VM (by default is value id 0). This pointer is ignored by the VM.
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <param name="p">The pointer that has to be set</param>
        [DllImport(DllName, EntryPoint = "sq_setforeignptr", CallingConvention = CallingConvention.StdCall)]
        extern public static void SetForeignPtr(IntPtr v, IntPtr p);

        [DllImport(DllName, EntryPoint = "sq_setprintfunc", CallingConvention = CallingConvention.StdCall)]
        extern private static void NativeSetPrintFunc(IntPtr v, ISqPrintFunction pf, ISqPrintFunction ef);

        /// <summary>
        /// Sets the print function of the virtual machine. This function is used by the built-in function '::print()' to output text.
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <param name="printfunc">A pointer to the print func or null to disable the output</param>
        /// <param name="errorfunc">A pointer to the error func or null to disable the output</param>
        public static void SetPrintFunc(IntPtr v, SqPrintFunction printfunc, SqPrintFunction errorfunc)
        {
            NativeSetPrintFunc(v, (vm, fmt, argc, variant) =>
            {
                object[] args = argc > 0 ? GetObjectsForSqVariants(variant, argc) : new object[0];
                printfunc(new VM(v), fmt, args);
            }, (vm, fmt, argc, variant) =>
            {
                object[] args = argc > 0 ? GetObjectsForSqVariants(variant, argc) : new object[0];
                errorfunc(new VM(v), fmt, args);
            });

            _printFunctions[v] = printfunc;
            _errorFunctions[v] = errorfunc;
        }

        /// <summary>
        /// Pops a table from the stack and sets it as the root table.
        /// </summary>
        /// <param name="v">The target VM</param>
        [DllImport(DllName, EntryPoint = "sq_setroottable", CallingConvention = CallingConvention.StdCall)]
        extern public static void SetRootTable(IntPtr v);

        /// <summary>
        /// Suspends the execution of the specified VM.
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <returns>An SQRESULT (that has to be returned by a C function)</returns>
        /// <remarks>sq_result can only be called as return expression of a C function. The function will fail if the suspension is done through more C calls or in a metamethod.</remarks>
        /// <example>
        /// int SuspendVMExample(IntPtr v)
        /// {
        ///     return Sq.SuspendVM(v);
        /// }
        /// </example>
        [DllImport(DllName, EntryPoint = "sq_suspendvm", CallingConvention = CallingConvention.StdCall)]
        extern public static int SuspendVM(IntPtr v);

        /// <summary>
        /// Wake up the execution of a previously suspended VM.
        /// </summary>
        /// <param name="v">The target VM</param>
        /// <param name="resumedret">If true the function will pop a value from the stack and use it as a return value for the function that has previously suspended the VM</param>
        /// <param name="retval">If true the function will push the return value of the function that suspended the execution or the main function one</param>
        /// <param name="raiseerror">If true, if a runtime error occurs during the execution of the call, the VM will invoke the error handler</param>
        /// <param name="throwerror">If true, the VM will throw and exception as soon as it is resumed. The exception payload must be set beforehand invoking Sq.ThrowError</param>
        /// <returns>A HRESULT</returns>
        [DllImport(DllName, EntryPoint = "sq_wakeupvm", CallingConvention = CallingConvention.StdCall)]
        extern public static int WakeUpVM(IntPtr v, bool resumedret, bool retval, bool raiseerror, bool throwerror);
    }
}
