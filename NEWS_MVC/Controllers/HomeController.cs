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
        public ActionResult search()//搜索功能
        {
            string input = Request["search_text"];
            NEWS NEWSDB = new NEWS();
            List<article> news = NEWSDB.article.Where(s => s.name.Contains(input)).Select(m => m).ToList();
            news = news.OrderByDescending(m => m.newstime).ToList();
            return View(news);
        }
        // 关闭数据验证，不然传html的值会报错

        public string Base64ToImage(string base64Str, string path, string imgName)//传输图片功能
        {
            string filename = "";//声明一个string类型的相对路径
            //取图片的后缀格式
            string hz = base64Str.Split(',')[0].Split(';')[0].Split('/')[1];
            string[] str = base64Str.Split(',');  //base64Str为base64完整的字符串，先处理一下得到我们所需要的字符串
            byte[] imageBytes = Convert.FromBase64String(str[1]);
            //读入MemoryStream对象
            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
            memoryStream.Write(imageBytes, 0, imageBytes.Length);
            filename = path + imgName + "." + hz;//所要保存的相对路径及名字
            string tmpRootDir = Server.MapPath(path); //获取程序根目录 
            if (!Directory.Exists(tmpRootDir))
            {
                Directory.CreateDirectory(tmpRootDir);
            }
            string imagesurl2 = tmpRootDir + imgName + "." + hz; //转换成绝对路径 
            //  转成图片
            Image image = Image.FromStream(memoryStream);
            //   图片名称
            string iname = DateTime.Now.ToString("yyMMddhhmmss");
            image.Save(imagesurl2);  // 将图片存到本地Server.MapPath("pic\\") + iname + "." + hz
            return filename;
        }

        [ValidateInput(false)]
        public JsonResult Insert()
        {
            string html = Request["html"];
            string name = Request["biaoti"];
            string userid = Request["userid"];
            string type = Request["type"];
            string priority = Request["priority"];
            string fengmian = Request["fengmian"];
            string username = Request["username"];

            NEWS NEWSDB = new NEWS();
            article news = new article();
            news.name = name;
            news.newstime = DateTime.Now.ToLocalTime();
            var userid_int = Convert.ToInt32(userid);
            news.userid = userid_int;
            news.type = type;
            news.priority = priority[0] - '0';
            var now = NEWSDB.article.Select(m => m).ToList<article>();
            var q = (NEWSDB.article.Select(e => e.articleid).Max() + 1).ToString();
            if (!(fengmian[0] == 'h' && fengmian[1] == 't' && fengmian[2] == 't' && fengmian[3] == 'p'))
            {
                fengmian = Base64ToImage(fengmian, "../Articles/", q);
                //return Json(fengmian);
            }
            news.fengmian = fengmian;
            news.username = username;
            NEWSDB.article.Add(news);
            NEWSDB.SaveChanges();
            //return Json(fengmian);
            FileStream fs = new FileStream(Server.MapPath("../Articles/") + q + ".html", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(html);//按行写
            sw.Close();//关闭
            return Json(q);
        }
        public ActionResult page()
        {
            string articleid = Request["articleid"];
            ViewData["id"] = articleid;
            return View();
        }
        public ActionResult userpage()
        {
            NEWS NEWSDB = new NEWS();
            string userid = Request["userid"];
            int userid_int = Convert.ToInt32(userid);
            List<article> userartircle = NEWSDB.article.Where(u => u.userid == userid_int).Select(m => m).ToList();

            return View(userartircle);
        }
        public ActionResult logout()
        {
            HttpCookie cok = Request.Cookies["name"];
            HttpCookie cok2 = Request.Cookies["userid"];
            if (cok != null)
            {
                TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
                Response.AppendCookie(cok);
            }
            if (cok2 != null)
            {
                TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                cok2.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
                Response.AppendCookie(cok2);
            }
            return Content("<script>location.href='Index'</script>");
        }
        public string changePW()//修改用户密码
        {
            NEWS NEWSDB = new NEWS();
            int userid = Convert.ToInt32(Request["userid"]);
            var users = NEWSDB.user.Where(u => u.userid == userid).SingleOrDefault();

            if (users == null)
            {
                return "0";

            }
            else
            {
                users.password = Request["password"];
                NEWSDB.SaveChanges();
                return "1";
                //重置成功
            }
        }
        public string changeemail()//修改邮箱
        {
            NEWS NEWSDB = new NEWS();
            int userid = Convert.ToInt32(Request["userid"]);
            var users = NEWSDB.user.Where(u => u.userid == userid).SingleOrDefault();

            if (users == null)
            {
                return "0";

            }
            else
            {
                users.email = Request["email"];
                NEWSDB.SaveChanges();
                return "1";
                //重置成功
            }
        }
    }
}