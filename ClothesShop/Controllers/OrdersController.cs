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
    public class OrdersController : Controller
    {
        private readonly ClothesShopContext _context;
        private readonly OrderService _service;
        private readonly int _pageSize;

        public OrdersController(ClothesShopContext context)
        {
            _context = context;
            _service = new OrderService();
            _pageSize = 8;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string selectedName, int? page, OrderService.SortState? sortState)
        {
            if (!User.IsInRole(Areas.Identity.Roles.User) && !User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return Redirect("~/Identity/Account/Login");
            }
            bool isFromFilter = HttpContext.Request.Query["isFromFilter"] == "true";

            _service.GetSortPagingCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter,
                ref page, ref sortState);
            _service.GetFilterCookiesForUserIfNull(Request.Cookies, User.Identity.Name, isFromFilter);
            _service.SetDefaultValuesIfNull(ref page, ref sortState);
            _service.SetCookies(Response.Cookies, User.Identity.Name, page, sortState);

            var orders = _context.Orders.AsQueryable();

            orders = _service.Filter(orders);

            var count = await orders.CountAsync();

            orders = _service.Sort(orders, (OrderService.SortState)sortState);
            orders = _service.Paging(orders, isFromFilter, (int)page, _pageSize);

            ViewModels.Order.IndexOrderViewModel model = new ViewModels.Order.IndexOrderViewModel
            {
                Orders = await orders.ToListAsync(),
                PageViewModel = new ViewModels.PageViewModel(count, (int)page, _pageSize)
            };

            return View(model);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.User) && !User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return Redirect("~/Identity/Account/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,MyProperty,CustomerName,CustomerPhone,CustomerAddress,Date,IsPaid,IsSent,Id")] Order order)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,MyProperty,CustomerName,CustomerPhone,CustomerAddress,Date,IsPaid,IsSent,Id")] Order order)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole(Areas.Identity.Roles.Admin))
            {
                return RedirectToAction("Index", "Home");
            }
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
