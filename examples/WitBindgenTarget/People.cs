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
    
    // NOTE: User of the guest should implement this
    public static partial string Hello(Person? person);

    // NOTE: P/Invoke takes care of casting char* to string?
    private unsafe static IntPtr wasmExportHello(
        int person_option_discriminant, 
        string person_name, 
        int person_age
    )
    {
        Person? lifted;

        switch (person_option_discriminant)
        {
            case 0: {
                lifted = null;
                break;
            }
            case 1: {
                lifted = new(person_name, person_age);
                break;
            }

            default: throw new Exception($"invalid discriminant: {person_option_discriminant}");
        }

        var result = People.Hello(lifted);
        var resultBytes = Encoding.UTF8.GetBytes(result);
        var resultHandle = GCHandle.Alloc(resultBytes)
            .AddrOfPinnedObject()
            .ToInt32();
        
        var writeByte = (byte*)DemoWorld.RETURN_AREA;
        var lenBytes = BitConverter.GetBytes(resultBytes.Length);
        var addrBytes = BitConverter.GetBytes(resultHandle);

        Marshal.Copy(lenBytes, 0, DemoWorld.RETURN_AREA, lenBytes.Length);
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
