using System;
using System.Collections.Generic;

namespace Crud.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public int? CountryId { get; set; }
        public string? PictureProfileUrl { get; set; }
        public Guid? AccountNumber { get; set; }
        public int? BankId { get; set; }
        public long? InitialDeposit { get; set; }
    }
}
