using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContractMaster.models;
using Microsoft.AspNetCore.Http;

namespace ContractMaster.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ContractMasterContext _context;

        public CompanyController(ContractMasterContext context)
        {
            _context = context;
        }

        // GET: Company
        public async Task<IActionResult> Index(string companyId)
        {
            if (companyId != null)
            {
                HttpContext.Session.SetString(nameof(companyId), companyId);
            }
            else if (HttpContext.Session.GetString(nameof(companyId)) != null)
            {
                companyId = HttpContext.Session.GetString(nameof(companyId));
            }
            else if (Request.Cookies["memberId"] != null)
            {
                companyId = Request.Cookies["memberId"];
            }
            else
            {
                TempData["message"] = "Please select a company to see their details.";
                return Redirect("/Company/index");
            }

            var company = _context.Company.Include(a => a.Client).Where(a => a.CompanyId == companyId);

            return View(await company.ToListAsync());
        }

        // GET: Company/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                TempData["message"] = "Can not find company: ID is null";
                return Redirect("/Company/Index");
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                TempData["message"] = "Can not find company: No company match with ID.";
                return Redirect("/Company/Index");
            }

            return View(company);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,AccountStatus,Address,City,Province,PositalCode,ContactPerson")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                TempData["message"] = "Can not find company: ID is null";
                return Redirect("/Company/Index");
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                TempData["message"] = "Can not find company: No company match with ID.";
                return Redirect("/Company/Index");
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CompanyId,CompanyName,AccountStatus,Address,City,Province,PositalCode,ContactPerson")] Company company)
        {
            if (id != company.CompanyId)
            {
                TempData["message"] = "Can not edit company: ID is changed.";
                return Redirect($"/Company/Index/{id}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        TempData["message"] = "Can not edit company: Company is not exist.";
                        return Redirect("/Company/Index");
                    }
                    else
                    {
                        TempData["message"] = "Error editing company: " + ex.GetBaseException().Message;
                        return Redirect("/Company/Index");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                TempData["message"] = "Can not find company: ID is null";
                return Redirect("/Company/Index");
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                TempData["message"] = "Can not find company: No company match with ID.";
                return Redirect("/Company/Index");
            }

            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var company = await _context.Company.FindAsync(id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(string id)
        {
            return _context.Company.Any(e => e.CompanyId == id);
        }
    }
}
