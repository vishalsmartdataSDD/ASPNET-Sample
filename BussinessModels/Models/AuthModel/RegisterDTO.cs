﻿namespace BussinessModels.Models.AuthModel
{
    public class RegisterDTO
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
