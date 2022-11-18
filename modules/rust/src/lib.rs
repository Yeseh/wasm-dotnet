#[no_mangle]
pub extern "C" fn hello() {
    let dotnet_version = std::env::var("DOTNET_VERSION")
        .unwrap_or(String::from("NOT FOUND"));

    println!("
        Heyyy Dotnet, 

        I see you are using version {}
        And I just wanted to say hello :)

        xoxo Rust", dotnet_version
    );
}