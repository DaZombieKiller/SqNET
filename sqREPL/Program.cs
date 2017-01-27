using System;
using System.Text.RegularExpressions;
using Squirrel;

internal class Program
{
    static void Main(string[] args)
        => new Program(args);

    enum GetArgReturn
    {
        Interactive = 0,
        Done = 2,
        Error = 3,
    }

    void CFormatToCSharp(ref string fmt)
    {
        int n = 0;
        const string regex = @"(?<!%)(?:%%)*%([\-\+0\ \#])?(\d+|\*)?(\.\*|\.\d+)?([hLIw]|l{1,2}|I32|I64)?([cCdiouxXeEfgGaAnpsSZ])";
        fmt = fmt.Replace("{", "{{").Replace("}", "}}");
        fmt = Regex.Replace(fmt, regex, (match) => $"{{{n++}}}");
    }

    void PrintFunc(Sq.VM v, string fmt, params object[] args)
    {
        CFormatToCSharp(ref fmt);
        Console.WriteLine(fmt, args);
    }

    void ErrorFunc(Sq.VM v, string fmt, params object[] args)
    {
        CFormatToCSharp(ref fmt);
        Console.WriteLine(fmt, args);
    }

    void PrintVersionInfo()
    {
        Console.WriteLine("Sq.NET Stable ({0} bits) Copyright (C) 2017 Benjamin Moir", Environment.Is64BitProcess ? 64 : 32);
        Console.WriteLine("Squirrel Copyright (C) 2003-2016 Alberto Demichelis");
    }

    void PrintUsage()
    {
        Console.WriteLine("usage: sq <options> <scriptpath [args]>.");
        Console.WriteLine("Available options are:");
        Console.WriteLine("   -c              compiles the file to bytecode(default output 'out.cnut')");
        Console.WriteLine("   -o              specifies output file for the -c option");
        Console.WriteLine("   -c              compiles only");
        Console.WriteLine("   -d              generates debug infos");
        Console.WriteLine("   -v              displays version infos");
        Console.WriteLine("   -h              prints help");
    }

    GetArgReturn GetArgs(Sq.VM v, string[] args, out int retval)
    {
        retval = 0;
        bool compilesOnly = false;
        string output = string.Empty;

        if (args.Length > 0)
        {
            int arg = 0;
            bool exitLoop = false;
            while (arg < args.Length && !exitLoop)
            {
                if (args[arg][0] == '-')
                {
                    switch (args[arg][1])
                    {
                        case 'd': // DEBUG (debug infos)
                            v.EnableDebugInfo(true);
                            break;
                        case 'c':
                            compilesOnly = true;
                            break;
                        case 'o':
                            if (arg < args.Length)
                            {
                                arg++;
                                output = args[arg];
                            }
                            break;
                        case 'v':
                            PrintVersionInfo();
                            return GetArgReturn.Done;
                        case 'h':
                            PrintVersionInfo();
                            PrintUsage();
                            return GetArgReturn.Done;
                        default:
                            PrintVersionInfo();
                            Console.WriteLine("unknown parameter '-{0}'", args[arg][1]);
                            PrintUsage();
                            retval = -1;
                            return GetArgReturn.Error;
                    }
                }
                else
                    break;
                arg++;
            }

            // src file
            if (arg < args.Length)
            {
                string filename = string.Empty;
                filename = args[arg];
                arg++;

                if (compilesOnly)
                {
                    if (Sq.Succeeded(Sq.Std.LoadFile(v, filename, true)))
                    {
                        string outfile = output != string.Empty ? output : "out.cnut";
                        if (Sq.Succeeded(Sq.Std.WriteClosureToFile(v, outfile)))
                            return GetArgReturn.Done;
                    }
                }
                else
                {
                    if (Sq.Succeeded(Sq.Std.LoadFile(v, filename, true)))
                    {
                        int callargs = 1;
                        v.PushRootTable();
                        for (int i = arg; i < args.Length; i++)
                        {
                            string a = args[i];
                            v.PushString(a, -1);
                            callargs++;
                        }

                        if (Sq.Succeeded(v.Call(callargs, true, true)))
                        {
                            Sq.ObjectType type = v.GetType(-1);
                            if (type == Sq.ObjectType.OT_INTEGER)
                            {
                                retval = (int)type;
                                v.GetInteger(-1, out retval);
                            }
                            return GetArgReturn.Done;
                        }
                        else
                            return GetArgReturn.Error;
                    }
                }
            }
        }

        return GetArgReturn.Interactive;
    }

    static int done = 0;

    int Quit(Sq.VM v)
    {
        done = 1;
        return 0;
    }

    void Interactive(Sq.VM v)
    {
        const int maxInput = 1024;
        string buffer = string.Empty;

        int blocks = 0;
        bool str = false;
        int retval = 0;
        PrintVersionInfo();

        v.PushRootTable();
        v.PushString("quit", -1);
        v.NewClosure(Quit, 0);
        v.SetParamsCheck(1, null);
        v.NewSlot(-3, false);

        while (done == 0)
        {
            int i = 0;
            buffer = string.Empty;
            Console.Write("\nsq>");
            for (;;)
            {
                int c;
                if (done != 0) return;
                c = Console.Read();
                if (c == '\n')
                {
                    if (i > 0 && buffer[i - 1] == '\\')
                        buffer += '\n';
                    
                    else if (blocks == 0)
                        break;
                    buffer += '\n';
                }
                else if (c == '}')
                {
                    blocks--;
                    buffer += (char)c;
                }
                else if (c == '{' && !str)
                {
                    blocks++;
                    buffer += (char)c;
                }
                else if (c == '"' || c == '\'')
                {
                    str = !str;
                    buffer += (char)c;
                }
                else if (i > maxInput - 1)
                {
                    Console.WriteLine("sq : input line too long");
                    break;
                }
                else
                    buffer += (char)c;
            }

            if (buffer[0] == '=')
            {
                buffer = $"return ({buffer.Substring(1)})";
                retval = 1;
            }

            i = buffer.Length;

            if (i > 0)
            {
                int oldtop = v.GetTop();
                if (Sq.Succeeded(v.CompileBuffer(buffer, buffer.Length, "interactive console", true)))
                {
                    v.PushRootTable();
                    if (Sq.Succeeded(v.Call(1, retval != 0, true)) && retval != 0)
                    {
                        Console.WriteLine();
                        v.PushRootTable();
                        v.PushString("print", -1);
                        v.Get(-2);
                        v.PushRootTable();
                        v.Push(-4);
                        v.Call(2, false, true);
                        retval = 0;
                        Console.WriteLine();
                    }
                }

                v.SetTop(oldtop);
            }
        }
    }

    Program(string[] args)
    {
        int retval = 0;

        using (var v = new Sq.VM(1024))
        {
            v.SetPrintFunc(PrintFunc, ErrorFunc);

            v.PushRootTable();
            Sq.Std.RegisterBlobLib(v);
            Sq.Std.RegisterIOLib(v);
            Sq.Std.RegisterSystemLib(v);
            Sq.Std.RegisterMathLib(v);
            Sq.Std.RegisterStringLib(v);

            // aux library
            // sets error handlers
            Sq.Std.SetErrorHandlers(v);

            // gets arguments
            switch (GetArgs(v, args, out retval))
            {
                case GetArgReturn.Interactive:
                    Interactive(v);
                    break;
                case GetArgReturn.Done:
                case GetArgReturn.Error:
                default:
                    break;
            }
        }

        Environment.Exit(retval);
    }
}
