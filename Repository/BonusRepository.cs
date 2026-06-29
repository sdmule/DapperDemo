using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DapperDemo.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection db;

        public BonusRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public void AddTestCompanyWithEmployees(Company objComp)
        {
            var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                        + "SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = db.Query<int>(sql, objComp).Single();
            objComp.CompanyId = id;

            foreach (var employee in objComp.Employees)
            {
                employee.CompanyId = objComp.CompanyId;
                var sql1 = "INSERT INTO Employees (Name, Title, Phone, Email, CompanyId) VALUES(@Name, @Title, @Phone, @Email, @CompanyId);"
                            + "SELECT CAST(SCOPE_IDENTITY() as int);";
                db.Query<int>(sql1, employee).Single();
            }
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

        public void RemoveRange(int[] companyIds)
        {
            db.Query("DELETE FROM Companies WHERE CompanyId IN @companyIds", new { companyIds });
        }

        public List<Company> FilterCompanyByName(string name)
        {
            return db.Query<Company>("SELECT * FROM Companies WHERE Name LIKE '%'+ @name + '%' ", new { name}).ToList();
        }
    }
}
