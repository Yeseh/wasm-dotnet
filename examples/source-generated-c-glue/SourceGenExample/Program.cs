﻿using System.Runtime.CompilerServices;
using Wasi.SourceGenerator;

namespace ConsoleApp;

partial class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello");
    }

    [WasiExport("bla", "dotnet", "hello_from")]
    public static int HelloFrom()
    {
        Console.WriteLine("Hello from WASI");
        return 1;
    }
}

public static class Interop
{

    [MethodImpl(MethodImplOptions.InternalCall)]
    [WasiImport("bla", "env", "hello")]
    public static extern void Hello();
}
