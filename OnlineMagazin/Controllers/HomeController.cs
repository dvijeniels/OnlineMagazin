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
using Microsoft.AspNetCore.Identity;
using OnlineMagazin.Areas.Identity.Data;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineMagazin.Service;
using Newtonsoft.Json.Schema;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using AspNetCore.SEOHelper.Sitemap;
using Microsoft.Extensions.Hosting;

namespace OnlineMagazin.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly UserManager<OnlineMagazinUser> _userManager;
        private readonly SignInManager<OnlineMagazinUser> _signInManager;
        public HomeController(OnlineMagazinContext context, ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, 
            UserManager<OnlineMagazinUser> userManager, SignInManager<OnlineMagazinUser> signInManager)
        {
            _context = context;
            _logger = logger;
            this._hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public string CreateSitemapInRootDirectory()
        {
            var list = new List<SitemapNode>();
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/HomeIndex/", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/GetProducts", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.7, Url = "https://pskanker.ru/Home/About", Frequency = SitemapFrequency.Always });

            new SitemapDocument().CreateSitemapXML(list, _hostEnvironment.ContentRootPath);
            return "sitemap.xml";
        }
        [AllowAnonymous]
        public async Task<IActionResult> HomeIndex()
        {
            //List<Message> mesaj = _context.Message.Where(x => x.Read == false).ToList();
            //HttpContext.Session.SetInt32("mesajSay", (mesaj.Count));
            var products = await _context.Products.Include(a=>a.TypeProduct).ToListAsync();
            if (products.Count==0)
            {
                return View();
            }
            ViewBag.UpToCategory = _context.Category.ToList();
            return View(products);
        }
        [AllowAnonymous]
        public IActionResult ModalView(int productID)
        {
            var response = _context.Products.Where(a => a.ProductId == productID).FirstOrDefault();
            return PartialView("_ModalView", response);
        }

        [AllowAnonymous]
        public IActionResult Delivery()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Payment()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult RegisterConfirmationSend()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult ForgetPasswordConfirmationSend()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(string userId, string code)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(code))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"Не удалось загрузить пользователя с таким ID '{userId}'.");
                }

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                }
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult WishList()
        {
            var like = SessionHelper.GetObjectFromJson<List<Liked>>(HttpContext.Session, "liked");
            if (like != null && like.Count != 0)
            {
                ViewBag.liked = like;
            }
            else
            {
                ViewBag.WishListControl = "Ваш список понравившихся пуст";
            }
            return View(like);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ProductsFromCategory(int id, int? page, int? PageSize)
        {
            if (id != 0)
            {
                HttpContext.Session.SetInt32("id", id);
            }
            var productsFromCat = await _context.Products.Where(x => x.CategoryId == HttpContext.Session.GetInt32("id")).ToListAsync();
            ViewBag.CategoryId = _context.ProductFeatures.Where(a => a.CategoryId == HttpContext.Session.GetInt32("id")).ToList();
            Category cat = _context.Category.Where(s => s.CategoryId == HttpContext.Session.GetInt32("id")).SingleOrDefault();
            if (cat == null)
            {
                return RedirectToAction("HomeIndex");
            }
            ViewBag.CategoryName = cat.CategoriName;
            if (productsFromCat.Count == 0)
            {
                ViewBag.kayityok = "Нету товаров относящихся к этому категорию";
            }
            ViewBag.Temp = _context.ProductFeatures.Where(a => a.CategoryId == HttpContext.Session.GetInt32("id")).Select(a => a.CategoryFeature.FeatureName).Distinct().ToList();
            var list = _context.ProductFeatures.Where(a=>a.CategoryId== HttpContext.Session.GetInt32("id")).Select(a => new { a.CategoryFeature.FeatureName, a.Value }).ToList();
            var q = from x in list
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            ViewBag.Value = q.ToList();
            var productsCount = _context.Products.Where(x => x.CategoryId == HttpContext.Session.GetInt32("id")).OrderBy(e => e.ProductId).Count();
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="16", Text= "16" },
                new SelectListItem() { Value="32", Text= "32" },
                new SelectListItem() { Value="64", Text= "64" },
                new SelectListItem() { Value="128", Text= "128" },
            };
            var pageNumber = (page ?? 1);
            int pagesize = (PageSize ?? 16);
            ViewBag.psize = pagesize;
            ViewBag.Count = productsCount;
            return View(productsFromCat.ToPagedList(pageNumber, pagesize));
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Commenting(int productID, string UserName, string Contents, int Score)
        {
            Comments newComment = new Comments();
            if (Contents != null)
            {
                newComment.ProductId = productID;
                newComment.Contents = Contents;
                newComment.UserName = UserName; 
                newComment.Score = Score;
                newComment.Date = DateTime.Today;
                _context.Comments.Add(newComment);
                _context.SaveChanges();
            }
            return Json(newComment);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult DeleteComment(int commentId)
        {
            Comments comm = _context.Comments.Where(s => s.CommentId == commentId).SingleOrDefault();
            if (comm != null)
            {
                _context.Comments.Remove(comm);
                _context.SaveChanges();
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        //[AllowAnonymous]
        //public async Task<IActionResult> GetProductsFeatures(string value, string FeatureName)
        //{
        //    ViewData["value"] =value;
        //    ViewData["FeatureName"] =FeatureName;
        //    ViewBag.PageSize = new List<SelectListItem>()
        //    {
        //        new SelectListItem() { Value="8", Text= "8" },
        //        new SelectListItem() { Value="16", Text= "16" },
        //        new SelectListItem() { Value="32", Text= "32" },
        //        new SelectListItem() { Value="64", Text= "64" },
        //        new SelectListItem() { Value="128", Text= "128" },
        //    };
        //    var result= await _context.ProductFeatures.Where(x => x.Value ==value).FirstOrDefaultAsync();
        //    ViewBag.ValueName = FeatureName+" " + result.Value;
        //    var productsFromFeatures = await _context.Products.Where(products => products.ProductFeatures.Any(a=>a.Value==value)).ToListAsync();
        //    return View(productsFromFeatures.ToPagedList(1, 8));
        //}
        [AllowAnonymous]
        public IActionResult GetProductsFeatures(string value, string FeatureName, int? page, int? PageSize)
        {
            if(value!=null&& FeatureName!=null)
            {
                HttpContext.Session.SetString("value", value);
                HttpContext.Session.SetString("FeatureName", FeatureName);
            }
            ViewBag.ValueName = HttpContext.Session.GetString("FeatureName")+" "+ HttpContext.Session.GetString("value");
            var productsCount = _context.Products.Where(products => products.ProductFeatures.Any(a => a.Value == HttpContext.Session.GetString("value"))).OrderBy(e => e.ProductId).Count();
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="16", Text= "16" },
                new SelectListItem() { Value="32", Text= "32" },
                new SelectListItem() { Value="64", Text= "64" },
                new SelectListItem() { Value="128", Text= "128" },
            };
            var pageNumber = (page ?? 1);
            int pagesize = (PageSize ?? 16);
            ViewBag.psize = pagesize;
            ViewBag.Count = productsCount;
            var productsFromFeatures = _context.Products.Where(products => products.ProductFeatures.Any(a => a.Value == HttpContext.Session.GetString("value"))).ToList().ToPagedList(pageNumber, pagesize);
            return View(productsFromFeatures);
        }
        [AllowAnonymous]
        public IActionResult GetProducts(int? page, int? PageSize)
        {
            var productsCount = _context.Products.OrderBy(e => e.ProductId).Count();
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() { Value="16", Text= "16" },
                new SelectListItem() { Value="32", Text= "32" },
                new SelectListItem() { Value="64", Text= "64" },
                new SelectListItem() { Value="128", Text= "128" },
            };
            var pageNumber = (page ?? 1);
            int pagesize = (PageSize ?? 16);
            ViewBag.psize = pagesize;
            ViewBag.Count = productsCount;
            var products =_context.Products.ToList().ToPagedList(pageNumber, pagesize);
            return View(products);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Subcribe(string Mail)
        {
            Mails newMail = new Mails();
            if (Mail!=null)
            {
                var result = _context.Mails.Where(mail => mail.Mail == Mail).Count();
                if (result > 0)
                {
                    return Json("Error");
                }
                newMail.Mail = Mail;
                newMail.Status = true;
                _context.Mails.Add(newMail);
                _context.SaveChanges();
            }
            return Json(newMail);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UnSubcribe(string mail)
        {
            var unsubcribe=_context.Mails.Where(a=>a.Mail == mail).FirstOrDefault();
            if (unsubcribe != null)
            {
                unsubcribe.Status = false;
                _context.Update(unsubcribe);
                await _context.SaveChangesAsync();
            }
            else { return NotFound(); }
            return View(unsubcribe);
        }

        [AllowAnonymous]
        public async Task<IActionResult> OrderCompleted()
        {
            ViewData["NumberOrder"]= ViewData["NumberOrder"];
            var order = await _context.Orders.ToListAsync();
            return View(order);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ProductDetail(int? id)
        {
            ViewData["Comments"]= _context.Comments.Where(x=>x.Status==true && x.ProductId==id).Count();
            var onlineMagazinContext = _context.Products.Include(p => p.Category);
            ViewProductCart viewProduct = new ViewProductCart();
            viewProduct.products = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(com => com.ProductId == id);
            return View(viewProduct);
        }
        public async Task<IActionResult> Account(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> AccountEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Uyes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountEdit(string id, [Bind("Email,PhoneNumber,Region,City,Description,BirthDate,Address,Address2,FirstAndLastName,ResimFile")] OnlineMagazinUser onlineMagazinUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != userId)
            {
                return NotFound();
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (ModelState.IsValid)
            {
                if (onlineMagazinUser.ResimFile != null)
                {
                    //Save image wwwRoow/allimage
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(onlineMagazinUser.ResimFile.FileName);
                    string extension = Path.GetExtension(onlineMagazinUser.ResimFile.FileName);
                    user.Foto = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await onlineMagazinUser.ResimFile.CopyToAsync(fileStream);
                    }
                    //Save image wwwRoow/allimage
                }
                try
                {
                    user.FirstAndLastName = onlineMagazinUser.FirstAndLastName;
                    user.Email = onlineMagazinUser.Email;
                    user.PhoneNumber = onlineMagazinUser.PhoneNumber;
                    user.Address = onlineMagazinUser.Address;
                    user.Address2 = onlineMagazinUser.Address2;
                    user.Address = onlineMagazinUser.Address;
                    user.Region = onlineMagazinUser.Region;
                    user.City = onlineMagazinUser.City;
                    user.Description = onlineMagazinUser.Description;
                    user.BirthDate = onlineMagazinUser.BirthDate;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UyeExists(userId))
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
            return View(onlineMagazinUser);
        }
        
        private bool UyeExists(string id)
        {
            return _context.UserClaims.Any(e => e.UserId == id);
        }

        //public void GetCarts()
        //{
        //    var cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "Carts");
        //    //var carts = (Carts)Session["Carts"];
        //    if (cart == null)
        //    {
        //        //carts = new Carts();
        //        SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session.Set, "Carts");
        //        HttpContext.Session.SetString("Carts", carts);
        //    }
        //}
        [AllowAnonymous]
        public IActionResult Cart()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
            if (cart != null && cart.Count!=0)
            {
                ViewBag.cart = cart;
                ViewBag.total = cart.Sum(item => item.Products.Price * item.qty);
            }
            else
            {
                ViewBag.CartControl = "Ваша корзина пуста";
            }
            return View(cart);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddProductToLiked(int productID)
        {
            var prID = _context.Products.Where(a => a.ProductId == productID).FirstOrDefault();
            var likeds = SessionHelper.GetObjectFromJson<List<Liked>>(HttpContext.Session, "liked");
            if (likeds == null)
            {
                List<Liked> liked = new List<Liked>();
                liked.Add(new Liked { Products = prID });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "liked", liked);
            }
            else
            {
                int index = isExistWish(productID);
                if (index == -1)
                {
                    likeds.Add(new Liked { Products = prID });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "liked", likeds);
            }
            return Json(likeds);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddProductToCart(int productID, ushort qty)
        {
            var prID = _context.Products.Where(a => a.ProductId == productID).FirstOrDefault();
            var carts = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
            if (carts == null)
            {
                List<Carts> cart = new List<Carts>();
                cart.Add(new Carts { Products = prID, qty = qty });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Carts> cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
                int index = isExist(productID);
                if (index != -1)
                {
                    cart[index].qty++;
                }
                else
                {
                    cart.Add(new Carts { Products = prID, qty = qty });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return Json(carts);
        }
        [HttpPost]
        private int isExist(int id)
        {
            List<Carts> cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Products.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult DeleteFromWishList(int id)
        {
            List<Liked> liked = SessionHelper.GetObjectFromJson<List<Liked>>(HttpContext.Session, "liked");
            int index = isExistWish(id);
            liked.RemoveAt(index);
            liked.Count();
            if (liked.Count() == 0)
            {
                HttpContext.Session.Remove("liked");
                ViewBag.WishListControl = "Ваш список понравившихся пуст";
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "liked", liked);
            return RedirectToAction(nameof(WishList));
        }
        [HttpPost]
        private int isExistWish(int id)
        {
            List<Liked> liked = SessionHelper.GetObjectFromJson<List<Liked>>(HttpContext.Session, "liked");
            for (int i = 0; i < liked.Count; i++)
            {
                if (liked[i].Products.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        [AllowAnonymous]
        public IActionResult GetMyViewComponent()
        {
            return ViewComponent("CartView");//it will call Follower.cs InvokeAsync, and pass id to it.
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult DeleteFromCart(int id)
        {
            List<Carts> cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            cart.Count();
            if (cart.Count()==0)
            {
                HttpContext.Session.Remove("cart");
                ViewBag.CartControl = "Ваша корзина пуста";
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction(nameof(Cart));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult UpdateCart(int id, ushort qty)
        {
            var prdID = _context.Products.Find(id);

            if (prdID != null)
            {
                if (SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart") == null)
                {
                    List<Carts> cart = new List<Carts>();
                    cart.Add(new Carts { Products = prdID, qty = 1 });
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    List<Carts> cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
                    int index = isExist(id);
                    if (index != -1)
                    {
                        cart[index].qty= qty;
                    }
                    else
                    {
                        cart.Add(new Carts { Products = prdID, qty = 1 });
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
            }

            return RedirectToAction(nameof(Cart));
        }

        [AllowAnonymous]
        public ActionResult Categories()
        {
            List<Category> Categoriler = _context.Category.ToList();
            return PartialView(Categoriler);
        }
        [AllowAnonymous]
        public ActionResult News()
        {
            List<Duyrular> Duyurular = _context.Duyrular.ToList();
            return PartialView(Duyurular);
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            List<About> metin = _context.About.ToList();
            return View(metin);
        }
        [AllowAnonymous]
        public ActionResult Message(string send)
        {
            ViewBag.Hata = send;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
        
        public IActionResult OrderListUser(int Id)
        {
            var order = _context.Orders.Where(u => u.OrderId == Id).Select(i=>new OrderDetails() {
                OrderId = i.OrderId,
                OrderNumber=i.OrderNumber,
                FirstAndLastName=i.FirstAndLastName,
                BuyingType=i.BuyingType,
                Comment=i.Comment,
                Address = i.Address,
                Address2 = i.Address2,
                Region = i.Region,
                City=i.City,
                PhoneNumber=i.PhoneNumber,
                Status=i.Status,
                OrderDate=i.OrderDate,
                CreatedDate=i.CreatedDate,
                OrderDetailLines = i.OrderLines.Select(a=>new OrderDetailLines()
                { 
                    ProductId=a.ProductId,
                    ProductName=a.Product.Baslik,
                    Foto=a.Product.Foto,
                    Foto2=a.Product.Foto2,
                    qty=a.qty,
                    Price=a.Price,
                }).ToList()
            }).FirstOrDefault();
            ViewBag.totalProductPrice = order.OrderDetailLines.Sum(a => a.Price * a.qty);
            //_context.Orders.Where(u => u.OrderId == Id).Sum(item => item.Price * item.qty);
            if (order != null)
            {
                return View(order);
            }
            return RedirectToAction(nameof(HomeIndex));
        }
        [AllowAnonymous]
        public IActionResult OrderCreate(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null) 
            {
                ViewBag.AdSoyad = user.FirstAndLastName;
                ViewBag.Address = user.Address;
                ViewBag.Address2 = user.Address2;
                ViewBag.PhoneNumber = user.PhoneNumber;
                ViewBag.Region = user.Region;
                ViewBag.City = user.City;
            }
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> OrderCreate([Bind("OrderId,FirstAndLastName,Address,Address2,PhoneNumber,Region,City,BuyingType,Comment")] Orders orders)
        {
            var carts = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                orders.UserId = userId;
                orders.OrderNumber="ZK"+new Random().Next(11111111,99999999).ToString();
                HttpContext.Session.SetString("NumberOrder", orders.OrderNumber);
                orders.CreatedDate= DateTime.Now;
                orders.OrderDate= DateTime.Now;
                orders.InOrder = false;
                orders.Status = "Заказ в обработке";
                orders.OrderLines = new List<OrderLines>();
                foreach (var item in carts)
                {
                    var orderLines = new OrderLines();
                    orderLines.qty = item.qty;
                    orderLines.ProductId = item.Products.ProductId;
                    orderLines.Price = carts.Sum(item => item.Products.Price * item.qty); ;
                    orders.OrderLines.Add(orderLines);
                }
                _context.Add(orders);
                await _context.SaveChangesAsync();
                var cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
                HttpContext.Session.Remove("cart");
                return RedirectToAction(nameof(OrderCompleted));
            }
            return View(orders);
        }
        [AllowAnonymous]
        public IActionResult SearchAllProduct(string searchString, Category category,int? page)
        {
            // Use LINQ to get list of genres.
            //var genreQuery = from m in _context.Products where m.CategoryId == categoryId select m;
            var PageNumber = page ?? 1;
            var products =from m in _context.Products
                           select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Baslik.Contains(searchString));
            }

            if (category.CategoryId != 0)
            {
                products = products.Where(x => x.CategoryId == category.CategoryId);
            }
            if (products.Count() == 0)
            {
                ViewBag.ProductNotHave = "Ничего не нашлось";
            }

            return View(products.ToList().ToPagedList(PageNumber, 8));
        }
        bool TestForNullOrEmpty(string s)
        {
            bool result;
            result = s == null || s == string.Empty;
            return result;
        }

    }
}
