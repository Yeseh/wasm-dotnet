package wit_demo;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;

import org.teavm.interop.Memory;
import org.teavm.interop.Address;
import org.teavm.interop.Import;
import org.teavm.interop.Export;
import org.teavm.interop.CustomSection;

public final class DemoWorld {
    private DemoWorld() {}
    
    @CustomSection(name = "component-type:Demo")
    private static final String __WIT_BINDGEN_COMPONENT_TYPE = "02000464656d6f0464656d6f0464656d6f0061736d0b00010007ce010141040142020140000073040c63757272656e742d75736572000100040a686f73742d66756e637314706b673a2f64656d6f2f686f73742d66756e637305000141040142020140000073040c63757272656e742d757365720001000304686f737414706b673a2f64656d6f2f686f73742d66756e63730500014205017202046e616d657303616765790406706572736f6e00030000016b010140010377686f020001040568656c6c6f000103040670656f706c65000501040464656d6f0e706b673a2f64656d6f2f64656d6f04010b12010464656d6f09706b673a2f64656d6f0300";
    
    public static final int RETURN_AREA = Memory.malloc(12, 4).toInt();
}
