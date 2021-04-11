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
    public class BuyerController : Controller
    {
        private readonly ContractMasterContext _context;

        public BuyerController(ContractMasterContext context)
        {
            _context = context;
        }

        // GET: Buyer
        public async Task<IActionResult> Index()
        {
            var contractMasterContext = _context.Buyer.Include(b => b.Client);
            return View(await contractMasterContext.ToListAsync());
        }

        // GET: Buyer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyer
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.BuyerId == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // GET: Buyer/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "ClientId");
            return View();
        }

        // POST: Buyer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuyerId,BuyerName,ClientId,BuyerEmail")] Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buyer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "ClientId", buyer.ClientId);
            return View(buyer);
        }

        // GET: Buyer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyer.FindAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "ClientId", buyer.ClientId);
            return View(buyer);
        }

        // POST: Buyer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BuyerId,BuyerName,ClientId,BuyerEmail")] Buyer buyer)
        {
            if (id != buyer.BuyerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buyer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyerExists(buyer.BuyerId))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "ClientId", "ClientId", buyer.ClientId);
            return View(buyer);
        }

        // GET: Buyer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyer = await _context.Buyer
                .Include(b => b.Client)
                .FirstOrDefaultAsync(m => m.BuyerId == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // POST: Buyer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var buyer = await _context.Buyer.FindAsync(id);
            _context.Buyer.Remove(buyer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuyerExists(string id)
        {
            return _context.Buyer.Any(e => e.BuyerId == id);
        }
    }
}
