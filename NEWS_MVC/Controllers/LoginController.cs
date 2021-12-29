using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWS_MVC.Models;

namespace NEWS_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        NEWS NEWSDB = new NEWS();
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult userlogin(string email, string password)
        {
            var user = NEWSDB.user.Where(u => u.email == email && u.password == password).SingleOrDefault();
            if(user == null)
                {
                return Content("<script>alert('输入有误');location.href='Demo404'</script>");

            }
                else
                {
                return Content("<script>location.href='Index'</script>");
                //登录成功跳转
            }
        }
        [HttpPost]
        public ActionResult Login(user users)
        {
            if(ModelState.IsValid)
            {
                NEWSDB.user.Add(users);
                NEWSDB.SaveChanges();
                return Content("登录成功");
            }
            return View();
        }
    }
}