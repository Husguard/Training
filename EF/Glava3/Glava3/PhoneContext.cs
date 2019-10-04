using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;

namespace Glava3
{
    public class PhoneContext : DbContext
    {
        public PhoneContext() : base("DefaultConnection")
        { }

        public DbSet<Phone> Phones { get; set; }
    }
}
