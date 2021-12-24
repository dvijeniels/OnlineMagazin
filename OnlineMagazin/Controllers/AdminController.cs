﻿using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMagazin.Controllers
{
    public class AdminController : Controller
    {
        private readonly OnlineMagazinContext _context;
        public AdminController(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IActionResult AdminIndex()
        {
            List<Category> kategoriler = _context.Category.ToList();
            ViewBag.catSay = kategoriler.Count;
            List<Products> product = _context.Products.ToList();
            ViewBag.productSay = product.Count;
            List<Uye> uyeler = _context.Uye.ToList();
            ViewBag.uyeSay = uyeler.Count;
            List<Duyrular> duyurular = _context.Duyrular.ToList();
            ViewBag.duyuruSay = duyurular.Count;
            List<Message> mesaj = _context.Message.ToList();
            ViewBag.mesajSay = mesaj.Count;
            return View();
        }
    }
}