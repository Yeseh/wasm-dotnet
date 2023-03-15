// <auto-generated>

#include <mono-wasi/driver.h>
#include <assert.h>
#include <string.h>

MonoMethod* method_HelloFrom;

__attribute__((__import_module__("env"), __import_name__("hello")))
void wasm_import_hello();


__attribute__((export_name("hello_from")))\n
void wasm_export_hello_from() {
    if(!method_HelloFrom) {
        method_HelloFrom = lookup_dotnet_method("SourceGenExample.dll", "ConsoleApp", "Program", "HelloFrom", -1);
        assert(method_HelloFrom);
    }
    MonoObject* exception;
    void* method_params[] = {};
    mono_wasm_invoke_method(method_HelloFrom, NULL, method_params, &exception);
    assert(!exception);
}

void attach_internal_calls() {
    mono_add_internal_call("ConsoleApp.Interop::Hello", wasm_import_hello);
}

