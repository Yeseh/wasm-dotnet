#include <mono-wasi/driver.h>
#include <assert.h>
#include <string.h>

MonoMethod* method_GuestHello;

__attribute__((__import_module__("host"), __import_name__("host_hello")))
extern void host_hello ();

__attribute__((export_name("guest_hello")))
void guest_hello() {
    if (!method_GuestHello) {
        method_GuestHello = lookup_dotnet_method("function-imports-exports.dll", "functions_imports_exports", "Program", "GuestHello", -1);
        assert(method_GuestHello);
    }

    void* method_params[] = {};

    MonoObject *exception;
    mono_wasm_invoke_method(method_GuestHello, NULL, method_params, &exception);
    assert(!exception);
}

void attach_internal_calls() {
    // This links the C function guest_hello to GuestHello in Interop.cs
    // as long as the C# code uses the [MethodImpl.InternalCall] attribute
    mono_add_internal_call("function_imports_exports.Interop::HostHello", host_hello);
}