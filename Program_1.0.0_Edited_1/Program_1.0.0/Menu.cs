using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_1._0._0 {
    class Menu
    {
        public List<Company> Companies;
        MyDbContext db = new MyDbContext();
        // конструктор класса Menu
        public Menu()
        {
            // инициализацию всех самых значимых данных класса нужно осуществлять в конструкторе
            Companies = new List<Company>();
        }

        public void Show()
        {
            Console.WriteLine("Программа 'Program 1.0.0' приветствует Вас!!! ");
            
            while (true)
            {
                Console.WriteLine("\n1. Зарегистрировать компанию");
                Console.WriteLine("2. Зарегистрировать сотрудника"); 
                Console.WriteLine("3. Вывести список компаний"); 
                Console.WriteLine("4. Вывести список сотрудников"); 
                Console.WriteLine("5. Вывести список зарегистрированных компаний с сотрудниками"); 
                Console.WriteLine("6. Удалить компанию"); 

                //Console.WriteLine("7. Редактировать список сотрудников компании"); 
                //Console.WriteLine("8. Удалить компанию из БД");  
                
                Console.WriteLine("0. Выход");
                Console.WriteLine();

                Console.Write("\nВыберите действие, которое необходимо выполнить:");

                string userChoise = Console.ReadLine();
                switch (userChoise) 
                {
                    case "1":
                        RegisterCompany();
                        break;

                    case "2":
                        RegisterEmployee();
                        break;

                    case "3":
                        DisplayOnlyCompanies();
                        break;

                    case "4":
                        DisplayOnlyEmployee();
                        break;

                    case "5":
                        DisplayCompanies();
                        break;

                    case "6":
                        DeleteCompany();
                        break;

                    case "22":
                        // отображаем список имеющихся компаний для того, 
                        // чтобы пользователь мог указать в какую именно компанию трудоустроить сотрудника
                        // если ни одной компании еще не зарегистрированно, то нужно уведомить об этом пользователя 
                        // и предложить ему сперва создать компанию, а уже потом зарегистрировать сотрудника
                        if (Companies.Count == 0)
                        {
                            Console.WriteLine("Сперва нужно зарегистрировать компанию, потом можно будет отредактировать список сотрудников");
                            Console.WriteLine("Зарегистрировать компанию: да, нет? ");
                            userChoise = Console.ReadLine();

                            if(String.Equals(userChoise, "да", StringComparison.InvariantCulture))
                            {
                                RegisterCompany();
                                break;
                            }
                        }


                        DisplayCompanies();
                        Console.Write("Выберите компанию и введите ее номер для редактирования списка сотрудников: ");
                        userChoise = Console.ReadLine();

                        int companyId = -1;
                        if (int.TryParse(userChoise, out companyId) && companyId < Companies.Count)
                        {
                            var company = Companies[companyId];
                            company.ShowEmployeeList();

                            Console.Write("\nВведите имя сотрудника: ");
                            var firstName = Console.ReadLine();

                            Console.Write("Введите фамилию сотрудника: ");
                            var lastName = Console.ReadLine();

                            Console.Write("Введите отчество сотрудника: ");
                            var middleName = Console.ReadLine();

                            company.AddEmployee(firstName, lastName, middleName);
                        }
                        break;

                    case "8":
                        DisplayCompanies();
                        if(Companies.Count <= 0)
                        {
                            continue;
                        }

                        while (true)
                        {
                            Console.Write("\nВведите номер удаляемой компании: ");
                            userChoise = Console.ReadLine();

                            // Проверим не выходит ли введенный номер за пределы допустимых значений в списке
                            int removedCompanyId = -1;
                            if (int.TryParse(userChoise, out removedCompanyId) == false || removedCompanyId >= Companies.Count) {
                                Console.WriteLine("Компании с указанным номером не существует. Проверьте правильность вводимых данных и повторите операцию снова");
                                continue;
                            }

                            // удаляем компанию из списка
                            Companies.RemoveAt(removedCompanyId);
                            Console.WriteLine("Операция успешно завершена\n");
                            break;
                        }
                        break;

                    case "0":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Введенная команда не распознана. Повторите ввод снова");
                        break;
                }
            }


        }

        private void RegisterCompany()
        {
            // Считываем имя новой компании, 
            // создаем экземпляр класса Company 
            // и добавляем его в список компаний, зарегистрированных в нашем приложении

            using (MyDbContext context = new MyDbContext())
            {
                Console.Write("\nВведите название компании: ");

                string companyName = Console.ReadLine();

                Company company = new Company { Name = companyName};

                context.CompaniesDb.Add(company);

                context.SaveChanges();

                Console.WriteLine("\nКомпания успешно зарегистрирована! ");
                Console.WriteLine("\nВыберите дальнейшие действия ");

            }
        }

        private void RegisterEmployee()
        {
            // Считываем имя новой компании, 
            // создаем экземпляр класса Company 
            // и добавляем его в список компаний, зарегистрированных в нашем приложении

            using (MyDbContext context = new MyDbContext())
            {
                while (true)
                {
                    Console.Write("\nВведите имя сотрудника: ");

                    string eFirstName = Console.ReadLine();
                    
                    Console.Write("\nВведите фамилию сотрудника: ");

                    string eLastName = Console.ReadLine();

                    Console.Write("\nВведите отчество сотрудника: ");

                    string eMiddleName = Console.ReadLine();

                    Employee employee = new Employee { FirstName = eFirstName, LastName = eLastName, MiddleName = eMiddleName, DateOfBirth = DateTime.Now };
                    context.EmployeeDb.Add(employee);

                    Console.WriteLine("Зарегистрировать еще сотрудника? да/нет");
                    string newEmployee = Console.ReadLine();

                    if (newEmployee == "нет")
                    {
                        context.SaveChanges();
                        Console.WriteLine("Успех! ");
                        break;
                    }
                }
            }
        }


        private void DisplayCompanies()
        {
            using (MyDbContext context = new MyDbContext())
            {
                var companiesDisplay = context.CompaniesDb.ToList();

                foreach (var company in companiesDisplay)
                {
                    Console.WriteLine("Company name - {0}, Id - {1}", company.Name, company.Id);

                    var employeeDisplay = context.EmployeeDb.Where(empl => empl.Id == company.Id).ToList();

                    foreach (var employee in employeeDisplay)
                    {
                        Console.WriteLine("{0} {1} {2}, Id - {3}", employee.LastName, employee.FirstName,  employee.MiddleName, employee.Id);
                    }
                }
            }
        }

        public void DisplayOnlyCompanies()
        {
            using (MyDbContext context = new MyDbContext())
            {
                var companyDisplay = context.CompaniesDb.ToList();

                foreach (var company in companyDisplay)
                {
                    Console.WriteLine("Company ID - {0}, Name - {1}", company.Id, company.Name);

                }
            }
        }

        public void DisplayOnlyEmployee()
        {
            using (MyDbContext context = new MyDbContext())
            {
                var employeeDisplay = context.EmployeeDb.ToList();

                foreach (var employee in employeeDisplay)
                {
                    Console.WriteLine("{0} {1} {2}, Id - {3}", employee.LastName,  employee.FirstName,  employee.MiddleName, employee.Id);

                }
            }
        }

        public void DeleteCompany()
        {
            MyDbContext context = new MyDbContext();

            Console.WriteLine("Введите название компании: ");

            var unnecessaryCompany = Console.ReadLine();

            Company company = context.CompaniesDb.Where(o => o.Name == unnecessaryCompany).FirstOrDefault();
            try
            {
                context.CompaniesDb.Remove(company);
                context.SaveChanges();
                Console.WriteLine("Компания успешно удалена!");
            }

            catch
            {
                Console.WriteLine("Упс! В названии компании имеется ошибка! ");
            }
            

        }

    }
}
