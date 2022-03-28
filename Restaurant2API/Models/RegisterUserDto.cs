﻿using System.ComponentModel.DataAnnotations;

namespace Restaurant2API.Models
{
    public class RegisterUserDto
    {
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        public int RoleId { get; set; } = 1;
    }
}
