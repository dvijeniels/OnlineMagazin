using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.ViewModels;

namespace OnlineMagazin.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(OnlineMagazinContext context, ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _logger = logger;
            this._hostEnvironment = hostEnvironment;
        }
        [AllowAnonymous]
        public async Task<IActionResult> HomeIndex()
        {
            List<Message> mesaj = _context.Message.Where(x => x.Read == false).ToList();
            HttpContext.Session.SetInt32("mesajSay", (mesaj.Count));
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> LikeProducts()
        {
            List<Message> mesaj = _context.Message.Where(x => x.Read == false).ToList();
            HttpContext.Session.SetInt32("mesajSay", (mesaj.Count));
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
        
        public async Task<IActionResult> ProductsFromCategory(int id)
        {
            var productsFromCat = await _context.Products.Where(x => x.CategoryId == id).ToListAsync();
            Category cat = _context.Category.Where(s => s.CategoryId == id).SingleOrDefault();
            if (cat == null)
            {
                return RedirectToAction("homeIndex");
            }
            ViewBag.CategoryName = cat.CategoriName;
            if (productsFromCat.Count == 0)
            {
                ViewBag.kayityok = "Нету товаров относящихся к этому категорию";
            }
            return View(productsFromCat);
        }
        public JsonResult Commenting(string content, int MakaleId)
        {
            var uyeID = HttpContext.Session.GetInt32("uyeId");
            Comments newComment = new Comments();
            newComment.ProductsId = MakaleId;
            newComment.Contents = content;
            newComment.UyeId = uyeID;
            newComment.Date = DateTime.Now;
            _context.Comment.Add(newComment);
            _context.SaveChanges();
            return Json(newComment);
        }
        public JsonResult DeleteComment(int commentId)
        {
            Comments comm = _context.Comment.Where(s => s.CommentId == commentId).SingleOrDefault();
            if (comm != null)
            {
                _context.Comment.Remove(comm);
                _context.SaveChanges();
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        public async Task<IActionResult> GetProducts()
        {
                var products = await _context.Products.ToListAsync();
                return View(products);
        }
        public async Task<IActionResult> ProductDetail(int? id)
        {
            var uyeID = HttpContext.Session.GetInt32("uyeId");
            var cartID = HttpContext.Session.GetInt32("cartId");
            ViewProductUyeCart viewProductUye = new ViewProductUyeCart();
            viewProductUye.products = await _context.Products.FirstOrDefaultAsync(com => com.ProductId == id);
            var uye= await _context.Uye.FirstOrDefaultAsync(com => com.UyeId == uyeID);
            var cart = await _context.Carts.FirstOrDefaultAsync(com => com.CartId == cartID);
            viewProductUye.carts = cart;
            viewProductUye.uye = uye;
            return View(viewProductUye);
        }

        public async Task<IActionResult> Account(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Uye
                .FirstOrDefaultAsync(m => m.UyeId == id);
            if (uye == null)
            {
                return NotFound();
            }

            return View(uye);
        }
        public async Task<IActionResult> AccountEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Uye.FindAsync(id);
            if (uye == null)
            {
                return NotFound();
            }
            return View(uye);
        }

        // POST: Uyes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountEdit(int id, [Bind("UyeId,KullaniciAdi,Email,PhoneNumber,Region,City,Sifre,ConfirmPassword,Description,BirthDate,Address,Address2,AdSoyad,ResimFile")] Uye uye)
        {
            if (id != uye.UyeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (uye.ResimFile != null)
                {
                    //Save image wwwRoow/allimage
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(uye.ResimFile.FileName);
                    string extension = Path.GetExtension(uye.ResimFile.FileName);
                    uye.Foto = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await uye.ResimFile.CopyToAsync(fileStream);
                    }
                    //Save image wwwRoow/allimage
                }

                try
                {
                    _context.Update(uye);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UyeExists(uye.UyeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Account", new { id });
            }
            return View(uye);
        }
        
        private bool UyeExists(int id)
        {
            return _context.Uye.Any(e => e.UyeId == id);
        }
        
        [HttpPost]
        public JsonResult AddProductToCart(int productID, ushort qty, double ProductPrice, [Bind("CartId,ProductsId,UyeId,ProductPrice,FinalPrice,qty")] Carts carts)
        {
            var uyeID = HttpContext.Session.GetInt32("uyeId");
            var cartproductID = _context.Carts.Where(a => a.ProductsId == productID && a.UyeId == uyeID && a.InOrder == false).FirstOrDefault() ;
            if (uyeID != null)
            {
                if (ModelState.IsValid)
                {
                    if (cartproductID!=null && cartproductID.ProductsId == productID)
                    {
                        cartproductID.qty = (ushort)(cartproductID.qty+qty);
                        cartproductID.FinalPrice = ProductPrice * cartproductID.qty;
                        _context.Update(cartproductID);
                        ViewBag.Toplam = cartproductID.FinalPrice;
                        _context.SaveChangesAsync();
                    }
                    else
                    {
                        carts.UyeId = uyeID;
                        carts.ProductsId = productID;
                        carts.ProductPrice = ProductPrice;
                        carts.FinalPrice = ProductPrice * qty;
                        carts.qty = qty;
                        _context.Add(carts);
                        ViewBag.Toplam = carts.FinalPrice;
                        _context.SaveChangesAsync();
                    }  
                } 
            }  
            else
            {
                return Json(null);
            }
            return Json(carts);
        }
        public IActionResult GetMyViewComponent()
        {
            return ViewComponent("CartView");//it will call Follower.cs InvokeAsync, and pass id to it.
        }

        public async Task<IActionResult> Cart()
        {
            var uyeID = HttpContext.Session.GetInt32("uyeId");
            var onlineMagazinContext = _context.Carts.Where(a=>a.UyeId==uyeID && a.InOrder==false).Include(c => c.Products).Include(c => c.Uye);
            int ToplamRate = Convert.ToInt32(((from t in onlineMagazinContext select t.FinalPrice).Sum()));
            ViewBag.FinalPrice = ToplamRate;
            if (ToplamRate == 0)
            {
                ViewBag.CartControl = "Ваша корзина пуста";
            }
            return View(await onlineMagazinContext.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            var carts = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(carts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }
        [HttpPost]
        public JsonResult DeleteCart(int id)
        {
            var carts = _context.Carts.Find(id);
            _context.Carts.Remove(carts);
            _context.SaveChangesAsync();
            return Json(carts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCart(int id,ushort qty, [Bind("CartId,ProductsId,UyeId,ProductPrice,qty,FinalPrice")] Carts carts)
        {
            var cartID = _context.Carts.Find(id);
            if (cartID!=null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        cartID.qty = qty;
                        cartID.FinalPrice = cartID.ProductPrice * qty;
                        ViewBag.Toplam = cartID.FinalPrice;
                        _context.Update(cartID);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CartsExists(carts.CartId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    
                }
            }

            return RedirectToAction(nameof(Cart));
        }
        public IActionResult IndexUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexUser(Uye uye)
        {
            var datavalue = await _context.Uye.FirstOrDefaultAsync(x => x.KullaniciAdi == uye.KullaniciAdi && x.Sifre == uye.Sifre);
            //var cartsTemp = await _context.Carts.FirstOrDefaultAsync(x => x.UyeId == datavalue.UyeId);
            if (datavalue != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,uye.KullaniciAdi)
                };
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                HttpContext.Session.SetInt32("uyeId", (datavalue.UyeId));
                //HttpContext.Session.SetInt32("cartId", (cartsTemp.CartId));
                //if (cartsTemp!=null)
                //{
                //    carts.UyeId = datavalue.UyeId;
                //    _context.Add(carts);
                //}
                //else
                //{
                //    carts.UyeId = datavalue.UyeId;
                //    _context.Add(carts);
                //}
                //await _context.SaveChangesAsync();
                HttpContext.Session.SetInt32("Giris", Convert.ToInt32(datavalue.UyeId));
                return RedirectToAction("homeIndex");
            }
            ViewBag.Loged = true;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.SetInt32("Giris", 0);
            return RedirectToAction("homeIndex");
        }
        //public IActionResult IndexUser()
        //{
        //    return View();
        //}
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: Uyes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("UyeId,KullaniciAdi,Email,Sifre,ConfirmPassword,AdSoyad,ResimFile,Yetki")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,uye.KullaniciAdi)
                };
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                _context.Add(uye);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetInt32("Giris", Convert.ToInt32(uye.UyeId));
                return RedirectToAction("homeIndex");
            }
            return View(uye);
        }
        public ActionResult Categories()
        {
            List<Category> Categoriler = _context.Category.ToList();
            return PartialView(Categoriler);
        }
        public ActionResult News()
        {
            List<Duyrular> Duyurular = _context.Duyrular.ToList();
            return PartialView(Duyurular);
        }
        public ActionResult About()
        {
            List<About> metin = _context.About.ToList();
            return View(metin);
        }
        public ActionResult Message(string send)
        {
            ViewBag.Hata = send;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Message([Bind("MessageId,MessageGonderen,MessageBaslik,MessageMail,MessageDate,MessageIcerik")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.MessageDate = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Message), new { send = "succes" });
            }
            ViewBag.Hata = "error";
            return View(message);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private bool CartsExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}
