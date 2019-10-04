using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glava2_DBFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (userstoredbEntities db = new userstoredbEntities())
            {
                var users = db.Users;
                foreach (User u in users)
                    Console.WriteLine("{0}.{1} - {2}", u.Id, u.Name, u.Age);
            }
            Console.Read();
        }
    }
}
