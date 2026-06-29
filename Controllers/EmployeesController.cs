
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DapperDemo.Models;
using DapperDemo.Data;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

public class EmployeesController : Controller
{
    private readonly ICompanyRepository _compRepo;
    private readonly IEmployeeRepository _empRepo;
    private readonly IBonusRepository _bonusRepo;
    [BindProperty]
    public Employee Employee { get; set; }

    public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo, IBonusRepository bonusRepo)
    {
        _compRepo = compRepo;
        _empRepo = empRepo;
        _bonusRepo = bonusRepo;
    }

    // GET: EMPLOYEES           
    public async Task<IActionResult> Index(int companyId=0)
    {
        //List<Employee> employees = _empRepo.FindAll();
        //foreach (var obj in employees)
        //{
        //    obj.Company = _compRepo.Find(obj.CompanyId);
        //}

        List<Employee> employees = _bonusRepo.GetEmployeeWithCompany(companyId);
        return View(employees);
    }

    // GET: EMPLOYEES/Create
    public IActionResult Create()
    {
        IEnumerable<SelectListItem> companyList = _compRepo.FindAll().Select(c => new SelectListItem
        {
            Value = c.CompanyId.ToString(),
            Text = c.Name
        });
        ViewBag.CompanyList = companyList;
        return View();
    }

    // POST: EMPLOYEES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Create")]
    public async Task<IActionResult> CreatePOST()
    {
        if (ModelState.IsValid)
        {
            _empRepo.Add(Employee);
            return RedirectToAction(nameof(Index));
        }
        return View(Employee);
    }

    // GET: EMPLOYEES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var Employee = _empRepo.Find(id.GetValueOrDefault());
        IEnumerable<SelectListItem> companyList = _compRepo.FindAll().Select(c => new SelectListItem
        {
            Value = c.CompanyId.ToString(),
            Text = c.Name
        });
        ViewBag.CompanyList = companyList;
        if (Employee == null)
        {
            return NotFound();
        }
        return View(Employee);
    }

    // POST: EMPLOYEES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        if (id != Employee.EmployeeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _empRepo.Update(Employee);
            return RedirectToAction(nameof(Index));
        }
        return View(Employee);
    }

    // GET: EMPLOYEES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        _empRepo.Delete(id.GetValueOrDefault());
        return RedirectToAction(nameof(Index));
    }
}
