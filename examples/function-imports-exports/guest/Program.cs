using System.Runtime.CompilerServices;
using System;

public class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("Called guest entrypoint!");
        return 0;
    }
    
    public void GuestHello()
    {
        Console.WriteLine("Hello from guest!");
    }
}

public static class Interop
{
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void HostHello();
}