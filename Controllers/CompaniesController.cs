
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DapperDemo.Models;
using DapperDemo.Data;
using DapperDemo.Repository;

public class CompaniesController : Controller
{
    private readonly ICompanyRepository _compRepo;
    private readonly IEmployeeRepository _empRepo;
    private readonly IBonusRepository _bonusRepo;
    private readonly IDapperSprocRepo _dapperSprocRepo;

    public CompaniesController(ICompanyRepository compRepo, IEmployeeRepository empRepo, IBonusRepository bonusRepo, IDapperSprocRepo dapperSprocRepo)
    {
        _compRepo = compRepo;
        _empRepo = empRepo;
        _bonusRepo = bonusRepo;
        _dapperSprocRepo = dapperSprocRepo;
    }

    // GET: COMPANYS
    public async Task<IActionResult> Index()
    {
        return View(_compRepo.FindAll());
        //return View(_dapperSprocRepo.List<Company>("usp_GetALLCompany"));
    }

    // GET: COMPANYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = _bonusRepo.GetCompanyWithCompanies(id.GetValueOrDefault());
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // GET: COMPANYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: COMPANYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
    {
        if (ModelState.IsValid)
        {
            _compRepo.Add(company);
            return RedirectToAction(nameof(Index));
        }
        return View(company);
    }

    // GET: COMPANYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = _compRepo.Find(id.GetValueOrDefault());
        //var company = _dapperSprocRepo.Single<Company>("usp_GetCompany", new { CompanyId = id.GetValueOrDefault() });
        if (company == null)
        {
            return NotFound();
        }
        return View(company);
    }

    // POST: COMPANYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? companyid, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
    {
        if (companyid != company.CompanyId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _compRepo.Update(company);
            return RedirectToAction(nameof(Index));
        }
        return View(company);
    }

    // GET: COMPANYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        _compRepo.Delete(id.GetValueOrDefault());
        return RedirectToAction(nameof(Index));
    }
}
