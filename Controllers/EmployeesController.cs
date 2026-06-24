
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DapperDemo.Models;
using DapperDemo.Data;
using DapperDemo.Repository;

public class EmployeesController : Controller
{
    private readonly ICompanyRepository _compRepo;
    private readonly IEmployeeRepository _empRepo;
    [BindProperty]
    public Employee Employee { get; set; }

    public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo)
    {
        _compRepo = compRepo;
        _empRepo = empRepo;
    }

    // GET: EMPLOYEES           
    public async Task<IActionResult> Index()
    {
        return View(_empRepo.FindAll());
    }

    // GET: EMPLOYEES/Create
    public IActionResult Create()
    {
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
