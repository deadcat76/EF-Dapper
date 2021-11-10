using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Employee : IDomainObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public int? DepartmentId { get; set; }
    }
    public class Department : IDomainObject
    {
            public int ID { get; set; }
            public string Title { get; set; }
            public List<Employee> Employees { get; set; }
    }
}
