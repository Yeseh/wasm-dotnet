package wit_demo;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;

import org.teavm.interop.Memory;
import org.teavm.interop.Address;
import org.teavm.interop.Import;
import org.teavm.interop.Export;

public final class People {
    private People() {}
    
    public static final class Person {
        public final String name;
        public final int age;
        
        public Person(String name, int age) {
            this.name = name;
            this.age = age;
        }
    }
    
    @Export(name = "people#hello")
    private static int wasmExportHello(int p0, int p1, int p2, int p3) {
        
        Person lifted;
        
        switch (p0) {
            case 0: {
                lifted = null;
                break;
            }
            
            case 1: {
                
                byte[] bytes = new byte[p2];
                Memory.getBytes(Address.fromInt(p1), bytes, 0, p2);
                
                lifted = new Person(new String(bytes, StandardCharsets.UTF_8), p3);
                break;
            }
            
            default: throw new AssertionError("invalid discriminant: " + (p0));
        }
        
        Person result = PeopleImpl.hello(lifted);
        
        byte[] bytes2 = ((result).name).getBytes(StandardCharsets.UTF_8);
        
        Address address = Memory.malloc(bytes2.length, 1);
        Memory.putBytes(address, bytes2, 0, bytes2.length);
        Address.fromInt((DemoWorld.RETURN_AREA) + 4).putInt(bytes2.length);
        Address.fromInt((DemoWorld.RETURN_AREA) + 0).putInt(address.toInt());
        Address.fromInt((DemoWorld.RETURN_AREA) + 8).putInt((result).age);
        return DemoWorld.RETURN_AREA;
        
    }
    
    @Export(name = "cabi_post_people#hello")
    private static void wasmExportHelloPostReturn(int p0) {
        Memory.free(
            Address.fromInt(Address.fromInt((p0) + 0).getInt()), 
            Address.fromInt((p0) + 4).getInt(), 1);
        
    }
    
}

