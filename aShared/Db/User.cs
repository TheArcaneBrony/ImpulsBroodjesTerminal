using System;
using System.Collections.Generic;

#nullable disable

namespace ThumbnailGenerator
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserFirstname { get; set; }
        public string UserLastname { get; set; }
        public string UserPassword { get; set; }
        public sbyte? UserEnabled { get; set; }
        public sbyte? UserIsAdmin { get; set; }
        public sbyte? UserCanViewOrders { get; set; }
    }
}
