using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Glava_1._1
{
    class UserContext : DbContext
    {
        public UserContext() : base("UserContext")
        { }
        public DbSet<User> Users { get; set; }
    }
}
``