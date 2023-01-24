namespace guest_nativeaot;

using System;
using System.Runtime.InteropServices;

public partial class Program
{
    public static int Main(string[] args)
    {
        Interop.host_hello();
        return 0;
    }
    
    [UnmanagedCallersOnly( EntryPoint="guest_hello" )]
    public static void GuestHello()
    {
        Console.WriteLine("Hello from guest!");
    }
}

public static class Interop
{
    [DllImport("*")]
    public static extern void host_hello();
}