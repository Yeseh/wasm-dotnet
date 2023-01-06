#include <mono-wasi/driver.h>
#include <assert.h>
#include <string.h>

MonoMethod* method_GuestHello;

__attribute__((__import_module__("host"), __import_name__("host_hello")))
extern void host_hello();

__attribute__((export_name("guest_hello")))
void guest_hello() {
    if (!method_GuestHello) {
        method_GuestHello = lookup_dotnet_method("guest_c_interop.dll", "guest_c_interop", "Program", "GuestHello", -1);
        assert(method_GuestHello);
    }

    MonoObject *exception;
    void* method_params[] = {};

    mono_wasm_invoke_method(method_GuestHello, NULL, method_params, &exception);

    assert(!exception);
}

void attach_internal_calls() {
    // This links the C function host_hello to HostHello in Interop.cs
    // as long as the C# code uses the [MetodImpl(MethodImplOptions.InternalCall)] attribute
    mono_add_internal_call("guest_c_interop.Interop::HostHello", host_hello);
}