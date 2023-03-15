namespace wit_demo;

using System.Runtime.CompilerServices;
using System.Text;

public sealed class Host 
{
    private Host() {}

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void WasmImportCurrentUser(IntPtr ptr);

    public unsafe static string CurrentUser() 
    {
        WasmImportCurrentUser(DemoWorld.RETURN_AREA);

        var addrSpan = new ReadOnlySpan<byte>((void*)DemoWorld.RETURN_AREA, sizeof(int));
        var addr = BitConverter.ToInt32(addrSpan);

        var lenSpan = new ReadOnlySpan<byte>((void*)(DemoWorld.RETURN_AREA + sizeof(int)), sizeof(int));
        var len = BitConverter.ToInt32(lenSpan);

        return Encoding.UTF8.GetString((byte*)addr, len);
    }
}