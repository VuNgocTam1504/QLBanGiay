using ShoesShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoesShopOnline.Controllers
{
    public class NewFeedController : Controller
    {
        // GET: NewFeed
        Shoes db = new Shoes();
        public ActionResult Index()
        {
            var list = (from tt in db.TinTucs select tt);
            ICollection<TinTuc> news = (from tt in db.TinTucs orderby tt.NgayDang select tt).ToList();
            TinTuc newsfisrt = (from tt in db.TinTucs orderby tt.NgayDang select tt).FirstOrDefault();
            ViewBag.news = news;
            return View(newsfisrt);
        }
        public ActionResult Single(int ma)
        {
            var newfeed = (from tt in db.TinTucs where tt.MaTin.Equals(ma) select tt).FirstOrDefault();
            return View(newfeed);
        }
    }
}