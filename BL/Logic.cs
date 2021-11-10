using DataAccessLayer;
using Domain;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Logic : INotifyPropertyChanged
    {
        IRepository<Employee> repository = new EntityRepository<Employee>();
        IRepository<Department> repository2 = new EntityRepository<Department>();
        //IRepository<Employee> repository = new DapperRepository<Employee>();
        public Logic()
        {
            AddCommand = new RelayCommand(add, true);
            AVGAgeCommand = new RelayCommand(AvgAge, true);
            AVGSalaryCommand = new RelayCommand(AvgSalary, true);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Employee> Emps { get; set; } = new ObservableCollection<Employee> { new Employee { Name = "Пахомова К.И", Age = 30, Salary = 50000 }, };
        public RelayCommand AddCommand { get; set; }
        public RelayCommand AVGAgeCommand { get; set; }
        public RelayCommand AVGSalaryCommand { get; set; }
        public Employee NewEmployee { get; set; } = new Employee();
        private double _averageAge;
        public double AverageAge
        {
            get => _averageAge;
            set
            {
                _averageAge = value;
                OnPropertyChanged(nameof(AverageAge));
            }
        }
        private double _averageSalary;
        public double AverageSalary
        {
            get => _averageSalary;
            set
            {
                _averageSalary = value;
                OnPropertyChanged(nameof(AverageSalary));
            }
        }

        public void AvgSalary()
        {
            AverageSalary = Emps.Average(employee => employee.Salary);
        }
        public void AvgAge()
        {
            AverageAge = Emps.Average(employee => employee.Age);
        }
        public void add()
        {
            if (NewEmployee.Name != null)
            {
                Emps.Add(new Employee { ID = NewEmployee.ID, Name = NewEmployee.Name, Age = NewEmployee.Age, Salary = NewEmployee.Salary });
            }
        }
        public void AddEmployee(string name, int age, int salary, int? department_id)
        {
            Employee employee = new Employee()
            {
                Name = name,
                Age = age,
                Salary = salary,
                DepartmentId = department_id
            };
            Emps.Add(employee);
            repository.Add(employee);
            repository.Save();
        }
        public List<(int, string, int, int, int?)> GetEmployees()
        {
            var Result = new List<(int, string, int, int, int?)>();
            var employees = repository.GetAll().ToList();
            foreach (Employee employee in employees)
            {
                Result.Add((employee.ID, employee.Name, employee.Age, employee.Salary, employee.DepartmentId));
            }
            return Result;
        }
        //public (int, string, int, int) GetEmployee(int ID)
        //{
        //    var employee = repository.GetById(ID);
        //    return (employee.ID, employee.Name, employee.Age, employee.Salary);
        //}
        public void DeleteEmployee(int ID)
        {
            var employee = repository.GetById(ID);
            if (employee != null)
                repository.Delete(employee);
            repository.Save();
        }
        public void UpdateEmployee((int, string, int, int) employee)
        {
            var empl1 = repository.GetById(employee.Item1);
            if (!string.IsNullOrEmpty(employee.Item2)) { empl1.Name = employee.Item2; }
            if (employee.Item3 != 0) { empl1.Age = employee.Item3; }
            if (employee.Item4 != 0) { empl1.Salary = employee.Item4; };
            repository.Update(empl1);
            repository.Save();
        }
        public List<(int, string, List<(int, string, int, int)>)> GetDepartaments()
        {
            var Result = new List<(int, string, List<(int, string, int, int)>)>();
            var Departments = repository2.GetAll("Employees");
            foreach (Department department in Departments)
            {
                var employees = new List<(int, string, int, int)>();
                if (department.Employees != null)
                {
                    foreach (Employee employee in department.Employees)
                    {
                        if (employee != null)
                        { employees.Add((employee.ID, employee.Name, employee.Age, employee.Salary)); }
                    }
                }
                Result.Add((department.ID, department.Title, employees));
            }
            return Result;
        }
        public void AddDepartment(string title, int[] EmployeeIDs)
        {
            Department department = new Department()
            {
                Title = title,
            };

            repository2.Add(department);

            repository2.Save();
            repository.Save();
            foreach (int ID in EmployeeIDs)
            {
                var e = repository.GetById(ID);
                if (e != null)
                {
                    e.DepartmentId = department.ID;
                    repository.Save();
                    repository2.Save();
                }

            }
        }
        public void DeleteDepartment(int ID)
        {
            var department = repository2.GetById(ID);
            if (department != null)
                repository2.Delete(department);
            repository2.Save();
        }
    }
}
