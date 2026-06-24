using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDbConnection db;

        public EmployeeRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public Employee Add(Employee employee)
        {
            var sql = "INSERT INTO Employees (Name, Email, Phone, Title, CompanyId) VALUES(@Name, @Email, @Phone, @Title, @CompanyId);"
                        + "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = db.Query<int>(sql, employee).Single();
            employee.EmployeeId = id;
            return employee;
        }
        public Employee Find(int id)
        {
            var sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            return db.Query<Employee>(sql, new { @Id = id }).Single();
        }
        public List<Employee> FindAll()
        {
            var sql = "SELECT * FROM Employees";
            return db.Query<Employee>(sql).ToList();
        }
        public void Delete(int id)
        {
            var sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            db.Execute(sql, new { Id = id });
            return;
        }
        public Employee Update(Employee employee)
        {
            var sql = "UPDATE Employees SET Name = @Name, Email = @Email, Phone = @Phone, " +
                     "Title = @Title, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";
            db.Execute(sql, employee);
            return employee;
        }
    }
}
