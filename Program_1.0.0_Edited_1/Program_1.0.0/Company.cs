using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_1._0._0 {
    public class Company
    {
        public int Id { get; set; } //Для связи таблиц
        public string Name { get; set; } //Название компании

        public DateTime DateOfFoundation;

        public ICollection<Employee> employeesCol { get; set; } //EF

        public Company() //EF
        {
            employeesCol = new List<Employee>();
        }

        public int EmployeeCount {
            get {
                return Employees == null ? 0 : Employees.Count;
            }
        }

        public List<Employee> Employees { get; set; }


        public Company(string companyName) {
            // поскольку имя параметра является входным параметром (к тому же наиболее важным), 
            // то нужно проверить правильность вводимых данных
            if (String.IsNullOrWhiteSpace(companyName))
                throw new ArgumentNullException(nameof(companyName), "Наименование компании не может быть пустым или равным null");

            Name = companyName;
            DateOfFoundation = DateTime.Now;
            Employees = new List<Employee>();
        }

        public void AddEmployee(string firstName, string lastName, string middleName) {
            var employee = new Employee(firstName, lastName, middleName);
            Employees.Add(employee);
        }

        public void ShowEmployeeList() {
            if (Employees.Count == 0)
                Console.WriteLine("В компании не имеется ни одного сотрудника\n");

            foreach (var emp in Employees) {
                Console.WriteLine(emp.FirstName + " " + emp.LastName + " " + emp.MiddleName);
            }
        }
    }
}
