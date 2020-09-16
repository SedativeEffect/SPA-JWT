using System;

namespace SPA.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
    [Flags]
    public enum Roles
    {
        Admin = 1,
        User = 2,
        Customer = 4,
        Editor = 8
    }
}