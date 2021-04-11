using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContractMaster.models;

namespace ContractMaster.Controllers
{
    public class MainCategoryController : Controller
    {
        private readonly ContractMasterContext _context;

        public MainCategoryController(ContractMasterContext context)
        {
            _context = context;
        }

        // GET: MainCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.MainCategory.ToListAsync());
        }

        // GET: MainCategory/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainCategory = await _context.MainCategory
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (mainCategory == null)
            {
                return NotFound();
            }

            return View(mainCategory);
        }

        // GET: MainCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] MainCategory mainCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mainCategory);
        }

        // GET: MainCategory/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainCategory = await _context.MainCategory.FindAsync(id);
            if (mainCategory == null)
            {
                return NotFound();
            }
            return View(mainCategory);
        }

        // POST: MainCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CategoryId,CategoryName")] MainCategory mainCategory)
        {
            if (id != mainCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainCategoryExists(mainCategory.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mainCategory);
        }

        // GET: MainCategory/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainCategory = await _context.MainCategory
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (mainCategory == null)
            {
                return NotFound();
            }

            return View(mainCategory);
        }

        // POST: MainCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mainCategory = await _context.MainCategory.FindAsync(id);
            _context.MainCategory.Remove(mainCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainCategoryExists(string id)
        {
            return _context.MainCategory.Any(e => e.CategoryId == id);
        }
    }
}
