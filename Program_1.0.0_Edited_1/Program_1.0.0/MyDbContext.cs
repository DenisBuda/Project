using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Program_1._0._0
{
    class MyDbContext : DbContext
    {
        public MyDbContext() : base("MyDbContext") { }

        public DbSet<Employee> EmployeeDb { get; set; }
        public DbSet<Company> CompaniesDb { get; set; }
    }
}
