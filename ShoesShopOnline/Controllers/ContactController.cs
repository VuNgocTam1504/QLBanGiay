using ShoesShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoesShopOnline.Controllers
{
    public class ContactController : Controller
    {
        private Shoes db = new Shoes();
        // GET: Contact
        [HttpPost]
        public ActionResult AddContact([Bind(Include = "ContactID ,CustomerName,Content,Email,Phone,DateContact")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateContact = DateTime.Now;
                db.Contacts.Add(contact);
                db.SaveChanges();
                TempData["message"] = "Thanks for your information!";
                return RedirectToAction(nameof(Index));
            }
            TempData["message"] = "Oop! try again";
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Index()
        {
            ViewBag.message = TempData["message"];
            return View();
        }
    }
}