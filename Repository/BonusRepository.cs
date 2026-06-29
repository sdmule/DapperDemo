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

        public List<Company> GetAllCompaniesWithEmployees()
        {
            var sql = "select c.*, e.* from Employees e inner join Companies c on c.CompanyId = e.CompanyId";

            var companyDict = new Dictionary<int, Company>();

            var company = db.Query<Company, Employee, Company>(sql, (c, e) =>
            {
                if (!companyDict.TryGetValue(c.CompanyId, out var currentCompany))
                {
                    currentCompany = c;
                    //currentCompany.Employees = new List<Employee>();
                    companyDict.Add(currentCompany.CompanyId, currentCompany);
                }
                currentCompany.Employees.Add(e);
                return currentCompany;
            }, splitOn: "EmployeeId");

            return company.Distinct().ToList();
        }

        public Company GetCompanyWithCompanies(int id)
        {
            var p = new
            {
                CompanyId = id,
            };

            var sql = "Select * from Companies where CompanyId = @CompanyId;"
                   + " Select * from Employees where CompanyId = @CompanyId;";

            Company company;

            using(var lists = db.QueryMultiple(sql, p))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }

            return company;
        }

        public List<Employee> GetEmployeeWithCompany(int id) 
        {
            var sql = "select e.*, c.* from Employees e inner join Companies c on c.CompanyId = e.CompanyId";
            if(id != 0)
            {
                sql += " where e.CompanyId = @Id";
            }

            var employee = db.Query< Employee, Company, Employee> (sql, (e, c) =>
            {
                e.Company = c;
                return e;
            }, new { id }, splitOn: "CompanyId");

            return employee.ToList();
        }
    }
}
