using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;

        public BonusRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
        public List<Employee> GetEmployeeWithCompany()
        {
            var sql = "select e.*, c.* from Employees e inner join Companies c on c.CompanyId = e.CompanyId";

            var employee = db.Query< Employee, Company, Employee> (sql, (e, c) =>
            {
                e.Company = c;
                return e;
            }, splitOn: "CompanyId");

            return employee.ToList();
        }
    }
}
