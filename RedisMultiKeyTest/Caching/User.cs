namespace RedisMultiKeyTest.Caching
{
    class User
    {
        public User(string? name, string? email, int age, string? city)
        {
            Name = name;
            Email = email;
            Age = age;
            City = city;
        }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public string? City{ get; set; }
    }
}
