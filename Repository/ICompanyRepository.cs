using DapperDemo.Models;

namespace DapperDemo.Repository
{
    public interface ICompanyRepository
    {
        Company Find(int id);
        List<Company> FindAll();
        Company Add(Company company);
        Company Update(Company company);
        void Delete(int id);
    }
}
