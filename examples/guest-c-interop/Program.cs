namespace guest_c_interop;

using System.Runtime.CompilerServices;

public partial class Program
{
    public static int Main(string[] args)
    {
        Interop.HostHello();
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