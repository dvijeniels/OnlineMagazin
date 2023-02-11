using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly OnlineMagazinContext _context;

        public OrdersController(OnlineMagazinContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var onlineMagazinContext = _context.Orders.Include(o => o.OnlineMagazinUser);
            return View(await onlineMagazinContext.OrderByDescending(a=>a.CreatedDate).ToListAsync());
        }

        public IActionResult Details(int id)
        {
            if (id == 0 || _context.Orders == null)
            {
                return NotFound();
            }

            var order = OrdersOperation(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        public object OrdersOperation(int Id)
        {
            var order = _context.Orders.Where(u => u.OrderId == Id).Select(i => new OrderDetails()
            {
                OrderId = i.OrderId,
                OrderNumber = i.OrderNumber,
                FirstAndLastName = i.FirstAndLastName,
                BuyingType = i.BuyingType,
                Comment = i.Comment,
                Address = i.Address,
                Address2 = i.Address2,
                Region = i.Region,
                City = i.City,
                PhoneNumber = i.PhoneNumber,
                Status = i.Status,
                OrderDate = i.OrderDate,
                CreatedDate = i.CreatedDate,
                OrderDetailLines = i.OrderLines.Select(a => new OrderDetailLines()
                {
                    ProductId = a.ProductId,
                    ProductName = a.Product.Baslik,
                    Foto = a.Product.Foto,
                    Foto2 = a.Product.Foto2,
                    qty = a.qty,
                    Price = a.Price,
                }).ToList()

            }).FirstOrDefault();
            ViewBag.totalProductPrice = order.OrderDetailLines.Sum(a => a.Price * a.qty);
            return order;
        }
        // GET: Orders1/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0 || _context.Orders == null)
            {
                return NotFound();
            }
            var order= OrdersOperation(id);
            
            var orders = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderNumber,UserId,FirstAndLastName,Address,Address2,PhoneNumber,Region,City,Status,BuyingType,Comment,CreatedDate,OrderDate,InOrder")] OrderDetails orders)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var employee = new Orders();
                employee.OrderId = orders.OrderId;
                employee.UserId = userId;
                employee.OrderNumber = orders.OrderNumber;
                employee.FirstAndLastName = orders.FirstAndLastName;
                employee.Address = orders.Address;
                employee.Address2 = orders.Address2;
                employee.PhoneNumber = orders.PhoneNumber;
                employee.Region = orders.Region;
                employee.City = orders.City;
                employee.Status = orders.Status;
                employee.CreatedDate= orders.CreatedDate;
                employee.OrderDate = orders.OrderDate;
                employee.BuyingType = orders.BuyingType;
                employee.Comment = orders.Comment;
                employee.InOrder = orders.InOrder;
                if (orders.InOrder)
                {
                    employee.OrderDate = DateTime.Now;
                }

                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.OrderId))
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
            return View(orders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFromOrderLines(int Id,int productId)
        {
            int index = isExist(Id,productId);
            var orderLines = _context.OrderLines.Where(u => u.OrderLineId == index).ToList();
            foreach (var item in orderLines)
            {
                _context.OrderLines.Remove(item);
            }
            _context.SaveChanges();
            return RedirectToAction("Edit", new { id = Id });
        }
        private int isExist(int Id,int productId)
        {
            var orderLines = _context.OrderLines.Where(u => u.OrderId == Id).ToList();
            for (int i = 0; i < orderLines.Count; i++)
            {
                if (orderLines[i].ProductId.Equals(productId))
                {
                    return orderLines[i].OrderLineId;
                }
            }
            return -1;
        }
        // GET: Orders1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.OnlineMagazinUser)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Набор сущностей имеет значение null.");
            }
            var orders = await _context.Orders.FindAsync(id);
            if (orders != null)
            {
                _context.Orders.Remove(orders);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
          return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
