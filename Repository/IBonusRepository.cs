using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeeWithCompany(int id);
        Company GetCompanyWithCompanies(int id);

        List<Company> GetAllCompaniesWithEmployees();

        void AddTestCompanyWithEmployees(Company objComp);

        void RemoveRange(int[] companyIds);

        List<Company> FilterCompanyByName(string name);
    }
}
