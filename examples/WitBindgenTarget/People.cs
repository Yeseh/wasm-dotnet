public static partial class People 
{
    public sealed class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
    }
    
    public static partial string Hello();

    private unsafe static string wasmExportHello(
        int person_option_discriminant,
        IntPtr person_name_ptr,
        int person_name_len,
        int person_age
    )
    {
        Person? lifted;

        switch (person_option_discriminant)
        {
            case 0: {
                lifted = null;
                break;
            }
            case 1: {
                var bytes = stackalloc byte[person_name_len];

                break;
            }
        }

        return "";
    }
}
