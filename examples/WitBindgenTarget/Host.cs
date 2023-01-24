using System.Runtime.CompilerServices;
using System.Text;

public sealed class Host 
{
    private Host() {}

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void wasmImportCurrentUser(IntPtr ptr);

    public unsafe static string CurrentUser() 
    {
        wasmImportCurrentUser(DemoWorld.RETURN_AREA);

        // Generate readonly spans for returned values
        // Convert returned values to their original types
        var addrSpan = new ReadOnlySpan<byte>((void*)DemoWorld.RETURN_AREA, sizeof(int));
        var lenSpan = new ReadOnlySpan<byte>((void*)(DemoWorld.RETURN_AREA + sizeof(int)), sizeof(int));

        var addr = BitConverter.ToInt32(addrSpan);
        var len = BitConverter.ToInt32(lenSpan);

        return Encoding.UTF8.GetString((byte*)addr, len);
    }
}