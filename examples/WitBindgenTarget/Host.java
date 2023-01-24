package wit_demo;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;

import org.teavm.interop.Memory;
import org.teavm.interop.Address;
import org.teavm.interop.Import;
import org.teavm.interop.Export;

public final class Host {
    private Host() {}
    
    @Import(name = "current-user", module = "host")
    private static native void wasmImportCurrentUser(int p0);
    
    public static String currentUser() {
        wasmImportCurrentUser(DemoWorld.RETURN_AREA);
        
        byte[] bytes = new byte[Address.fromInt((DemoWorld.RETURN_AREA) + 4).getInt()];
        Memory.getBytes(Address.fromInt(Address.fromInt((DemoWorld.RETURN_AREA) + 0).getInt()), bytes, 0, Address.fromInt((DemoWorld.RETURN_AREA) + 4).getInt());
        return new String(bytes, StandardCharsets.UTF_8);
        
    }
    
}

