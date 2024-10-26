using System;
using System.Collections.Generic;

namespace Crud.Models
{
    public partial class AccountUser
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public int? LoginId { get; set; }
    }
}
