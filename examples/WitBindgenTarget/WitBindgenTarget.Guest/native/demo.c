#include <mono-wasi/driver.h>
#include <assert.h>
#include <string.h>

// Imports
__attribute__((import_module("host"), import_name("current-user")))
void __wasm_import_host_current_user(int32_t);

// Exports
MonoMethod* method_Hello;
__attribute__((export_name("people#hello")))
MonoObject* __wasm_export_people_hello(int32_t arg, int32_t arg0, int32_t arg1, int32_t arg2) {
    if (!method_Hello) {
        method_Hello = lookup_dotnet_method("wit_demo.dll", "wit_demo", "people", "WasmExportPostReturn", -1);
        assert(method_Hello);
    }

    MonoObject *exception;
    void* method_params[] = { arg, arg0, arg1, arg2 };

    MonoObject* result_ptr = mono_wasm_invoke_method(method_Hello, NULL, method_params, &exception);

    assert(!exception);

    return result_ptr;
}

MonoMethod* method_HelloPostReturn;
__attribute__((weak, export_name("cabi_post_people#hello")))
void __wasm_export_people_hello_post_return(int32_t arg) {
    if (!method_HelloPostReturn) {
        method_Hello = lookup_dotnet_method("wit_demo.dll", "wit_demo", "people", "WasmExportPostReturn", -1);
        assert(method_Hello);
    }

    MonoObject *exception;
    void* method_params[] = { arg };

    mono_wasm_invoke_method(method_Hello, NULL, method_params, &exception);

    assert(!exception);
}

// cabi exports
__attribute__((weak, export_name("cabi_realloc")))
void *cabi_realloc(void *ptr, size_t orig_size, size_t org_align, size_t new_size) {
  void *ret = realloc(ptr, new_size);
  if (!ret) abort();
  return ret;
}

// Attach imports to the dotnet runtime
void demo_attach_internal_calls() {
    mono_add_internal_call("wit_demo.Host::WasmImportCurrentUser", __wasm_import_host_current_user);
}

extern void __component_type_object_force_link_demo(void);
void __component_type_object_force_link_demo_public_use_in_this_compilation_unit(void) {
  __component_type_object_force_link_demo();
}
