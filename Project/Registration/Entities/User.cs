﻿namespace Registration.Entities
{
    public  class User
    {
        public int Id { get; set; }
        //public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Email { get; set; }
        public string Address { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string role { get; set; }    
    }
}
