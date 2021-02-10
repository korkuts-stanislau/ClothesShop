using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClothesShop.Data;
using ClothesShop.Models;
using ClothesShop.EntityServices;

namespace ClothesShop.Controllers
{
    public class BasketItemsController : Controller
    {
        private readonly ClothesShopContext _context;
        private readonly BasketItemService _service;
        private readonly int _pageSize;

        public BasketItemsController(ClothesShopContext context)
        {
            _context = context;
            _service = new BasketItemService();
            _pageSize = 5;
        }

        // GET: BasketItems
        public async Task<IActionResult> Index(string selectedName, int? page, BasketItemService.SortState? sortState)
        {
            if (!User.IsInRole(Areas.Identity.Roles.User) && !User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return Redirect("~/Identity/Account/Login");
            }
            bool isFromFilter = HttpContext.Request.Query["isFromFilter"] == "true";

            _service.GetSortPagingCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref page, ref sortState);
            _service.GetFilterCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref selectedName);
            _service.SetDefaultValuesIfNull(ref selectedName, ref page, ref sortState);
            _service.SetCookies(Response.Cookies, User.Identity.Name, selectedName, page, sortState);

            var basketItems = _context.BasketItems
                .Include(b => b.ClothingItem)
                .AsQueryable();

            basketItems = _service.Filter(basketItems, selectedName);

            var count = await basketItems.CountAsync();

            basketItems = _service.Sort(basketItems, (BasketItemService.SortState)sortState);
            basketItems = _service.Paging(basketItems, isFromFilter, (int)page, _pageSize);

            ViewModels.BasketItem.IndexBasketItemViewModel model = new ViewModels.BasketItem.IndexBasketItemViewModel
            {
                BasketItems = await basketItems.ToListAsync(),
                PageViewModel = new ViewModels.PageViewModel(count, (int)page, _pageSize),
                FilterBasketItemViewModel = new ViewModels.BasketItem.FilterBasketItemViewModel(selectedName),
                SortBasketItemViewModel = new ViewModels.BasketItem.SortBasketItemViewModel((BasketItemService.SortState)sortState),
            };

            return View(model);
        }

        // GET: BasketItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItems
                .Include(b => b.ClothingItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basketItem == null)
            {
                return NotFound();
            }

            return View(basketItem);
        }

        // GET: BasketItems/Create
        public IActionResult Create()
        {
            ViewData["ClothingItemId"] = new SelectList(_context.ClothingItems, "Id", "Id");
            return View();
        }

        // POST: BasketItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,ClothingItemId,Count,Id")] BasketItem basketItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basketItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClothingItemId"] = new SelectList(_context.ClothingItems, "Id", "Id", basketItem.ClothingItemId);
            return View(basketItem);
        }

        // GET: BasketItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItems.FindAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }
            ViewData["ClothingItemId"] = new SelectList(_context.ClothingItems, "Id", "Id", basketItem.ClothingItemId);
            return View(basketItem);
        }

        // POST: BasketItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ClothingItemId,Count,Id")] BasketItem basketItem)
        {
            if (id != basketItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basketItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketItemExists(basketItem.Id))
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
            ViewData["ClothingItemId"] = new SelectList(_context.ClothingItems, "Id", "Id", basketItem.ClothingItemId);
            return View(basketItem);
        }

        // GET: BasketItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItems
                .Include(b => b.ClothingItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basketItem == null)
            {
                return NotFound();
            }

            return View(basketItem);
        }

        // POST: BasketItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basketItem = await _context.BasketItems.FindAsync(id);
            _context.BasketItems.Remove(basketItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasketItemExists(int id)
        {
            return _context.BasketItems.Any(e => e.Id == id);
        }
    }
}
