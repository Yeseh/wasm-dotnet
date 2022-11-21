use std::{slice, str};

type Fd = u32;
type Size = usize;
type Errno = i32;
type Rval = u32;
type Ptr = *const u8;

#[repr(C)]
struct Ciovec {
    buf: *const u8,
    buf_len: Size,
}

#[link(wasm_import_module = "wasi_snapshot_preview1")]
extern "C" {
    fn fd_write(fd: Fd, iovs_ptr: *const Ciovec, iovs_len: Size, nwritten: *mut Size) -> Errno;
    fn proc_exit(rval: Rval);
}

#[link(wasm_import_module = "dotnet")]
extern "C" {
    fn dotnet_hello();
    fn dotnet_hello_params(int: i32, float: f32);
    fn mem_ptr() -> i32;
    // fn dotnet_hello_struct(strct: DotnetInput);
}

#[no_mangle]
pub extern "C" fn hello() {
    let dotnet_version = std::env::var("DOTNET_VERSION").unwrap_or(String::from("NOT FOUND"));
    println!("Hello Dotnet {}, from WASI!", dotnet_version);
}

#[no_mangle]
pub unsafe extern "C" fn host_functions() {
    dotnet_hello();
    dotnet_hello_params(5, 7.5);
}

#[no_mangle]
pub unsafe extern "C" fn read_struct_raw() -> f32 {
    let start_ptr = mem_ptr() as i32;
    println!("Rust ptr: {:?}", start_ptr as i32);
    // We know the C# Input struct has a size of 8 bytes
    // With the first 4 being an integer, last 4 a float
    let int_slice = slice::from_raw_parts(start_ptr as *const u8, 4);
    // let float_slice = slice::from_raw_parts(start_ptr.add(4), 4);

    println!("{:?}", int_slice);
    // println!("{:?}", float_slice);

    // WASM uses little endian
    // let input_int = i32::from_le_bytes(int_slice);
    // let input_float = f32::from_le_bytes(float_slice);

    // println!("WASI got i: {}, f: {}", input_int, input_float);

    // input_int as f32 * input_float
    0 as f32
}
