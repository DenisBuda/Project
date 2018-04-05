using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_1._0._0 {
    class Menu {
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
                Console.WriteLine("2. Редактировать список сотрудников компании");
                Console.WriteLine("3. Удалить компанию из БД");
                Console.WriteLine("4. Вывести список зарегистрированных компаний с сотрудниками");
                Console.WriteLine("5. Вывести список компаний отдельно");
                Console.WriteLine("6. Зарегистрировать сотрудника");
                Console.WriteLine("0. Выход");
                Console.WriteLine();

                Console.Write("\nВыберите действие, которое необходимо выполнить:");

                string userChoise = Console.ReadLine();
                switch (userChoise) {
                    case "1":
                        RegisterCompany();
                        break;

                    case "2":
                        // отображаем список имеющихся компаний для того, 
                        // чтобы пользователь мог указать в какую именно компанию трудоустроить сотрудника
                        // если ни одной компании еще не зарегистрированно, то нужно уведомить об этом пользователя 
                        // и предложить ему сперва создать компанию, а уже потом зарегистрировать сотрудника
                        if (Companies.Count == 0)
                        {
                            Console.WriteLine("Сперва нужно зарегистрировать компанию, потом можно будет отредактировать список сотрудников");
                            Console.WriteLine("Зарегистрировать компанию: да, нет? ");
                            userChoise = Console.ReadLine();

                            if(String.Equals(userChoise, "да", StringComparison.InvariantCulture)) {
                                RegisterCompany();
                                break;
                            }
                        }


                        DisplayCompanies();
                        Console.Write("Выберите компанию и введите ее номер для редактирования списка сотрудников: ");
                        userChoise = Console.ReadLine();

                        int companyId = -1;
                        if (int.TryParse(userChoise, out companyId) && companyId < Companies.Count) {
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


                    case "3":
                        DisplayCompanies();
                        if(Companies.Count <= 0) {
                            continue;
                        }

                        while (true) {
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

                    case "4":
                        DisplayCompanies();
                        break;

                    case "5":
                        DisplayOnlyCompanies();
                        break;

                    case "6":
                        RegisterEmployee();
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

            using (MyDbContext db = new MyDbContext())
            {
                Console.Write("\nВведите название компании: ");

                string companyName = Console.ReadLine();

                Console.Write("\nВведите Id компании: ");

                int companyId = int.Parse(Console.ReadLine());

                Company comp = new Company { Name = companyName, Id = companyId };


                db.CompaniesC.Add(comp);

                db.SaveChanges();

                Console.WriteLine("Компания успешно зарегистрирована! ");
                Console.WriteLine("Выберите дальнейшие действия ");

            }

            /*
            Console.Write("\nВведите название компании: ");
            string companyName = Console.ReadLine();

            var company = new Company(companyName);
            Companies.Add(company);

            Console.WriteLine("Компания успешно зарегистрированна\n");
            */
        }

        private void RegisterEmployee()
        {
            // Считываем имя новой компании, 
            // создаем экземпляр класса Company 
            // и добавляем его в список компаний, зарегистрированных в нашем приложении

            using (MyDbContext db = new MyDbContext())
            {
                Console.Write("\nВведите id сотрудника: ");

                int eId = int.Parse(Console.ReadLine());

                Console.Write("\nВведите имя сотрудника: ");

                string eFirstName = Console.ReadLine();

                Console.Write("\nВведите фамилию сотрудника: ");

                string eLastName = Console.ReadLine();

                Console.Write("\nВведите отчество сотрудника: ");

                string eMiddleName = Console.ReadLine();

                Employee emp = new Employee { FirstName = eFirstName, LastName = eLastName, MiddleName = eMiddleName, DateOfBirth = DateTime.Now, Id = eId  };


                db.EmployeeC.Add(emp);

                db.SaveChanges();

                Console.WriteLine("Сотрудник успешно зарегистрирован! ");

            }

            /*
            Console.Write("\nВведите название компании: ");
            string companyName = Console.ReadLine();

            var company = new Company(companyName);
            Companies.Add(company);

            Console.WriteLine("Компания успешно зарегистрированна\n");
            */
        }


        private void DisplayCompanies()
        {
            using (MyDbContext ee = new MyDbContext())
            {

                var compan = ee.CompaniesC.ToList();

                foreach (var cmp in compan)
                {
                    Console.WriteLine("Company name = {0}", cmp.Name);

                    var employee = ee.EmployeeC.Where(emp => emp.Id == cmp.Id).ToList();

                    foreach (var empl in employee)
                    {
                        Console.WriteLine(empl.FirstName);
                    }
                }
            }



            /*
            if (Companies == null || Companies.Count == 0) {
                Console.WriteLine("БД компаний пуста");
                return;
            }

            for (int idx = 0; idx < Companies.Count; idx++) {
                Console.WriteLine($"{idx}. {Companies[idx].Name} (Дата основания - ) {Companies[idx].DateOfFoundation.ToShortDateString()}");
            }*/
        }

        public void DisplayOnlyCompanies()
        {
            using (MyDbContext ee = new MyDbContext())
            {
                var compan = ee.CompaniesC.ToList();

                foreach (var cmp in compan)
                {
                    Console.WriteLine("Company name = {0}", cmp.Name);

                }
            }
        }

    }
}
