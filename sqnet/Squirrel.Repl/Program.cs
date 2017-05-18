using System;
using System.Linq;
using SqDotNet;

internal class Program
{
    static void Main(string[] args)
    {
        using (var v = new Squirrel())
        {
            v.SetPrintFunc(PrintFunc, ErrorFunc);
            v.PushRootTable();
            v.RegisterBlobLib();
            v.RegisterIOLib();
            v.RegisterSystemLib();
            v.RegisterMathLib();
            v.RegisterStringLib();

            // aux library
            // sets error handlers
            v.SetErrorHandlers();

            bool justCompile = false;
            string outFile = string.Empty;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-") &&  args[i].Length <= 1)
                    switch (args[i][1])
                {
                    case 'd': // DEBUG (debug infos)
                        v.EnableDebugInfo(true);
                        break;
                    case 'c':
                        justCompile = true;
                        break;
                    case 'o':
                        if (i < args.Length)
                            outFile = args[++i];
                        break;
                    case 'v':
                        PrintVersionInfo();
                        return;
                    case 'h':
                        PrintVersionInfo();
                        PrintUsage();
                        return;
                    default:
                        PrintVersionInfo();
                        Console.WriteLine("unknown parameter '-{0}'", args[i][1]);
                        PrintUsage();
                        return;
                }
                else
                {
                    string inFile = args[i++];

                    if (justCompile)
                    {
                        if (v.LoadFile(inFile, true) >= 0)
                        {
                            if (v.WriteClosureToFile(string.IsNullOrEmpty(outFile) ? "out.cnut" : outFile) >= 0)
                                return;
                        }
                    }
                    else
                    {
                        if (v.LoadFile(inFile, true) >= 0)
                        {
                            int callArgs = 1;
                            v.PushRootTable();

                            for (int j = i; j < args.Length; j++, callArgs++)
                                v.PushString(args[j], -1);

                            if (v.Call(callArgs, true, true) >= 0)
                            {
                                ObjectType type = v.GetType(-1);
                                if (type == ObjectType.Integer)
                                {
                                    int result = (int)type;
                                    v.GetInteger(-1, out result);
                                    Environment.Exit(result);
                                }
                            }
                        }
                    }

                    return;
                }
            }

            Interactive(v);
        }
    }

    static void Interactive(Squirrel v)
    {
        // print version information
        PrintVersionInfo();

        // add "quit()" function
        v.PushRootTable();
        v.PushString("quit", -1);
        v.NewClosure(Quit, 0);
        v.SetParamsCheck(1, null);
        v.NewSlot(-3, false);

        for (;;)
        {
            int blocks = 0;
            bool str = false;
            bool retVal = false;
            bool execute = false;
            string buffer = string.Empty;
            Console.Write("\nsq>");

            while (!execute)
            {
                var c = (char)Console.Read();

                switch (c)
                {
                    case '\r': continue;
                    case '\n':
                        if (buffer.Last() == '\\')
                        {
                            buffer = buffer.Remove(buffer.Length - 1);
                            continue;
                        }
                        if (blocks == 0)
                            execute = true;
                        break;
                    case '}':
                        if (!str)
                            blocks--;
                        break;
                    case '{':
                        if (!str)
                            blocks++;
                        break;
                    case '"':
                    case '\'':
                        str = !str;
                        break;
                }

                buffer += c;
            }

            if (!string.IsNullOrEmpty(buffer))
            {
                if (buffer.StartsWith("="))
                {
                    buffer = $"return ({buffer.Substring(1)})";
                    retVal = true;
                }

                int oldtop = v.GetTop();
                if (v.CompileBuffer(buffer, buffer.Length, "interactive console", true) >= 0)
                {
                    v.PushRootTable();
                    if (v.Call(1, retVal, true) >= 0 && retVal != false)
                    {
                        Console.WriteLine();
                        v.PushRootTable();
                        v.PushString("print", -1);
                        v.Get(-2);
                        v.PushRootTable();
                        v.Push(-4);
                        v.Call(2, false, true);
                        retVal = false;
                        Console.WriteLine();
                    }
                }

                v.SetTop(oldtop);
            }
        }
    }

    static void PrintFunc(Squirrel v, string message)
    {
        Console.Write(message);
    }

    static void ErrorFunc(Squirrel v, string message)
    {
        Console.Error.Write(message);
    }

    static void PrintVersionInfo()
    {
        Console.WriteLine("Squirrel.NET (x{0}) Copyright (C) 2017 Benjamin Moir", Environment.Is64BitProcess ? 64 : 86);
        Console.WriteLine("Squirrel Copyright (C) 2003-2016 Alberto Demichelis");
    }

    static void PrintUsage()
    {
        Console.WriteLine("usage: squirrel.repl <options> <scriptpath [args]>.");
        Console.WriteLine("Available options are:");
        Console.WriteLine("   -c              compiles the file to bytecode(default output 'out.cnut')");
        Console.WriteLine("   -o              specifies output file for the -c option");
        Console.WriteLine("   -c              compiles only");
        Console.WriteLine("   -d              generates debug infos");
        Console.WriteLine("   -v              displays version infos");
        Console.WriteLine("   -h              prints help");
    }

    static int Quit(Squirrel v)
    {
        Environment.Exit(0);
        return 0;
    }
}
