using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Dapper;
using System.Configuration;

namespace DataAccessLayer
{
    public class Context : DbContext
    {
        public Context() : base("DBConnection") { }

        public DbSet<Employee> Employee1 { set; get; }
        public DbSet<Department> Department { set; get; }
    }
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        Context db = new Context();
        public IEnumerable<T> GetAll(string table = null)
        {
            if (String.IsNullOrEmpty(table))
            { return db.Set<T>(); }

            return db.Set<T>().Include(table);
        }
        public void Delete(T obj)
        {
            db.Set<T>().Remove(obj);
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public void Add(T obj)
        {
            db.Set<T>().Add(obj);
        }
        public T GetById(int ID)
        {
            return db.Set<T>().Find(ID);
        }
        public void Update(T obj)
        {
            var obj1 = GetById(obj.ID);
            obj = obj1;
        }
    }
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {

        //static string connectionString =  ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        static string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = DBConnection; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public void Add(T t)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Employees (Name, Age, Salary, Department_ID) VALUES(@Name, @Age, @Salary); SELECT CAST(SCOPE_IDENTITY() as int)";
                int employeeId = db.Query<int>(sqlQuery, t).FirstOrDefault();
                t.ID = employeeId;
            }
        }
        public void Delete(T ID)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Employees WHERE ID = @ID";
                db.Execute(sqlQuery, new { ID });
            }
        }

        public IEnumerable<T> GetAll(string table = null)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<T>("SELECT * FROM Employees").ToList();
            }

        }

        public T GetById(int ID)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<T>("SELECT *  FROM Employees WHERE Id = @ID", new { ID }).FirstOrDefault();
            }
        }

        public void Save()
        {
        }

        public void Update(T obj)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Employees SET Name = @Name,Age = @Age, Salary = @Salary WHERE Id = @ID";
                db.Execute(sqlQuery, obj);
            }
        }
    }
}
