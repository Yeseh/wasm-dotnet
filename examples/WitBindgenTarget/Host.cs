using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime.InteropServices;

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

        var len = BitConverter.ToInt32(lenSpan);
        GCHandle.

        return Encoding.UTF8.GetString(addrSpan, len);
    }
}