package wit_demo;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;

import org.teavm.interop.Memory;
import org.teavm.interop.Address;
import org.teavm.interop.Import;
import org.teavm.interop.Export;

public final class People {
    private People() {}
    
    public static final class Parent {
        public final String name;
        public final boolean dead;
        
        public Parent(String name, boolean dead) {
            this.name = name;
            this.dead = dead;
        }
    }
    
    public static final class Person {
        public final String name;
        public final int age;
        public final Parent parent;
        
        public Person(String name, int age, Parent parent) {
            this.name = name;
            this.age = age;
            this.parent = parent;
        }
    }
    
    @Export(name = "people#hello")
    private static int wasmExportHello(int p0, int p1, int p2, int p3, int p4, int p5, int p6) {
        
        Person lifted;
        
        switch (p0) {
            case 0: {
                lifted = null;
                break;
            }
            
            case 1: {
                
                byte[] bytes = new byte[p2];
                Memory.getBytes(Address.fromInt(p1), bytes, 0, p2);
                
                byte[] bytes2 = new byte[p5];
                Memory.getBytes(Address.fromInt(p4), bytes2, 0, p5);
                
                lifted = new Person(new String(bytes, StandardCharsets.UTF_8), p3, new Parent(new String(bytes2, StandardCharsets.UTF_8), (p6 != 0)));
                break;
            }
            
            default: throw new AssertionError("invalid discriminant: " + (p0));
        }
        
        Person result = PeopleImpl.hello(lifted);
        
        byte[] bytes3 = ((result).name).getBytes(StandardCharsets.UTF_8);
        
        Address address = Memory.malloc(bytes3.length, 1);
        Memory.putBytes(address, bytes3, 0, bytes3.length);
        Address.fromInt((DemoWorld.RETURN_AREA) + 4).putInt(bytes3.length);
        Address.fromInt((DemoWorld.RETURN_AREA) + 0).putInt(address.toInt());
        Address.fromInt((DemoWorld.RETURN_AREA) + 8).putInt((result).age);
        byte[] bytes4 = (((result).parent).name).getBytes(StandardCharsets.UTF_8);
        
        Address address5 = Memory.malloc(bytes4.length, 1);
        Memory.putBytes(address5, bytes4, 0, bytes4.length);
        Address.fromInt((DemoWorld.RETURN_AREA) + 16).putInt(bytes4.length);
        Address.fromInt((DemoWorld.RETURN_AREA) + 12).putInt(address5.toInt());
        Address.fromInt((DemoWorld.RETURN_AREA) + 20).putByte((byte) ((((result).parent).dead ? 1 : 0)));
        return DemoWorld.RETURN_AREA;
        
    }
    
    @Export(name = "cabi_post_people#hello")
    private static void wasmExportHelloPostReturn(int p0) {
        Memory.free(Address.fromInt(Address.fromInt((p0) + 0).getInt()), Address.fromInt((p0) + 4).getInt(), 1);
        Memory.free(Address.fromInt(Address.fromInt((p0) + 12).getInt()), Address.fromInt((p0) + 16).getInt(), 1);
        
    }
    
}

