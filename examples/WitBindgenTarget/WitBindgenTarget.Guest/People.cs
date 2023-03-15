namespace wit_demo;

using System.Runtime.InteropServices;
using System.Text;

public static partial class People 
{
    public sealed class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
    }
    
    // NOTE: Probably 'partial' in the actual implementation
    // User should implement this themselves, fof testing purposes just return something
    public static string Hello(Person? person)
        => person is null ? "Hey!" : $"Hello, {person.Name}";

    // NOTE: P/Invoke takes care of casting char* to string?
    private unsafe static IntPtr wasmExportHello(
        int arg0, 
        int arg1,
        int arg2,
        int arg3 
    )
    {
        Person? lifted;

        switch (arg0)
        {
            case 0: {
                lifted = null;
                break;
            }
            case 1: {
                var name = Encoding.UTF8.GetString(new ReadOnlySpan<byte>((void*) arg1, arg2));
                lifted = new(name, arg3);
                break;
            }

            default: throw new Exception($"invalid discriminant: {arg0}");
        }

        var resultBytes = Encoding.UTF8.GetBytes(People.Hello(lifted));
        var resultHandle = GCHandle.Alloc(resultBytes)
            .AddrOfPinnedObject()
            .ToInt32();
        
        var lenBytes = BitConverter.GetBytes(resultBytes.Length);
        Marshal.Copy(lenBytes, 0, DemoWorld.RETURN_AREA, lenBytes.Length);

        var addrBytes = BitConverter.GetBytes(resultHandle);
        Marshal.Copy(addrBytes, 0, DemoWorld.RETURN_AREA + lenBytes.Length, addrBytes.Length);

        return DemoWorld.RETURN_AREA;
    }

    // NOTE: Used to free 'resultHandle' allocated to a GCHandle in above function
    private static void wasmExportHelloPostReturn(IntPtr ptr)
    {
        if (ptr != IntPtr.Zero)
        {
            GCHandle.FromIntPtr(ptr).Free();
        }
    }
}
