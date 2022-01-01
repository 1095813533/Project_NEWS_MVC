using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWS_MVC.Models;
using System.IO;
using System.Drawing;

namespace NEWS_MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            NEWS NEWSDB = new NEWS();
            var news = NEWSDB.article.Select(m => m).ToList<article>();
            news = news.OrderByDescending(m => m.newstime).ToList();
            return View(news);
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult userlogin(string name, string password)
        {
            NEWS NEWSDB = new NEWS();
            var user = NEWSDB.user.Where(u => u.name == name && u.password == password).SingleOrDefault();
            if (user == null)
            {
                return Content("<script>alert('输入有误');location.href='Login'</script>");

            }
            else
            {
                List<user> usercookie = NEWSDB.user.Where(u => u.name == name).Select(m => m).ToList();

                HttpCookie cookieemail = new HttpCookie("email");
                cookieemail.Value = usercookie[0].email;
                Response.AppendCookie(cookieemail);
                HttpCookie cookieid = new HttpCookie("userid");
                cookieid.Value = usercookie[0].userid.ToString();
                Response.AppendCookie(cookieid);
                HttpCookie cookiename = new HttpCookie("name");
                cookiename.Value = usercookie[0].name;
                Response.AppendCookie(cookiename);
                HttpCookie cookiepassword = new HttpCookie("password");
                cookiepassword.Value = usercookie[0].password;
                Response.AppendCookie(cookiepassword);
                HttpCookie cookietype = new HttpCookie("type");
                cookietype.Value = usercookie[0].type.ToString();
                Response.AppendCookie(cookietype);
                return Content("<script>location.href='Index'</script>");
                //登录成功跳转
            }
        }
        public ActionResult userreg(string name, string email, string password, string confirmPassword)//注册帐号
        {
            NEWS NEWSDB = new NEWS();
            if (password != confirmPassword)
            {
                return Content("<script>alert('两次密码输入不一致');location.href='Login'</script>");
            }
            user users = new user();
            users.name = name;
            users.password = password;
            users.email = email;
            users.type = 2;
            users.regtime = DateTime.Now.ToLocalTime();
            var user = NEWSDB.user.Where(u => u.name == name && u.password == password).SingleOrDefault();
            if (user == null)
            {
                NEWSDB.user.Add(users);
                try
                {
                    NEWSDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content("<script>alert('该用户名已经存在');location.href='Login'</script>");
                }
            }
            else
            {
                return Content("<script>alert('该用户名已经存在');location.href='Login'</script>");
            }
            return Content("<script>location.href='Login'</script>");
        }

        public ActionResult userreset(string email, string name)//重置密码
        {
            NEWS NEWSDB = new NEWS();
            var users = NEWSDB.user.Where(u => u.email == email && u.name == name).SingleOrDefault();

            if (users == null)
            {
                return Content("<script>alert('输入有误');location.href='Login'</script>");

            }
            else
            {
                users.password = "123456";
                NEWSDB.SaveChanges();
                return Content("<script>alert('重置成功');location.href='Login'</script>");
                //重置成功
            }
            return Content("<script>alert('咋回事');location.href='Login'</script>");
        }
        public ActionResult wangeditor()
        {
            return View();
        }
    }
}