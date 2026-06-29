using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeeWithCompany(int id);
    }
}
