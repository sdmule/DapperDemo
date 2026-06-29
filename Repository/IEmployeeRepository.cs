using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface IEmployeeRepository
    {
        Employee Find(int id);
        List<Employee> FindAll();
        Employee Add(Employee employee);
        Task<Employee> AddAsync(Employee employee);
        Employee Update(Employee employee);
        void Delete(int id);
    }
}
