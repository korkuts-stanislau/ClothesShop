using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClothesShop.Data;
using ClothesShop.Models;

namespace ClothesShop.Controllers
{
    public class ClothingItemTypesController : Controller
    {
        private readonly ClothesShopContext _context;

        public ClothingItemTypesController(ClothesShopContext context)
        {
            _context = context;
        }

        // GET: ClothingItemTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClothingItemTypes.ToListAsync());
        }

        // GET: ClothingItemTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItemType = await _context.ClothingItemTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItemType == null)
            {
                return NotFound();
            }

            return View(clothingItemType);
        }

        // GET: ClothingItemTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClothingItemTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Id")] ClothingItemType clothingItemType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clothingItemType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clothingItemType);
        }

        // GET: ClothingItemTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItemType = await _context.ClothingItemTypes.FindAsync(id);
            if (clothingItemType == null)
            {
                return NotFound();
            }
            return View(clothingItemType);
        }

        // POST: ClothingItemTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id")] ClothingItemType clothingItemType)
        {
            if (id != clothingItemType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clothingItemType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClothingItemTypeExists(clothingItemType.Id))
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
            return View(clothingItemType);
        }

        // GET: ClothingItemTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItemType = await _context.ClothingItemTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItemType == null)
            {
                return NotFound();
            }

            return View(clothingItemType);
        }

        // POST: ClothingItemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clothingItemType = await _context.ClothingItemTypes.FindAsync(id);
            _context.ClothingItemTypes.Remove(clothingItemType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClothingItemTypeExists(int id)
        {
            return _context.ClothingItemTypes.Any(e => e.Id == id);
        }
    }
}
