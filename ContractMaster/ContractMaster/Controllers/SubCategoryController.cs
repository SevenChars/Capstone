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
    public class SubCategoryController : Controller
    {
        private readonly ContractMasterContext _context;

        public SubCategoryController(ContractMasterContext context)
        {
            _context = context;
        }

        // GET: SubCategory
        public async Task<IActionResult> Index()
        {
            var contractMasterContext = _context.SubCategory.Include(s => s.Category);
            return View(await contractMasterContext.ToListAsync());
        }

        // GET: SubCategory/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategory
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubcategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: SubCategory/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.MainCategory, "CategoryId", "CategoryId");
            return View();
        }

        // POST: SubCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubcategoryId,CategoryId,SubcategoryName")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.MainCategory, "CategoryId", "CategoryId", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategory/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategory.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.MainCategory, "CategoryId", "CategoryId", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: SubCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SubcategoryId,CategoryId,SubcategoryName")] SubCategory subCategory)
        {
            if (id != subCategory.SubcategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.SubcategoryId))
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
            ViewData["CategoryId"] = new SelectList(_context.MainCategory, "CategoryId", "CategoryId", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategory/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategory
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubcategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var subCategory = await _context.SubCategory.FindAsync(id);
            _context.SubCategory.Remove(subCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(string id)
        {
            return _context.SubCategory.Any(e => e.SubcategoryId == id);
        }
    }
}
