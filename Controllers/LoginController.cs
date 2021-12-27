using DataAccessLayer.Concreate;
using EntityLayer.Concreate;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous] //Proje seviyesinde tanımladığım tüm kurallardan muaf olması için bu attribute kullanılır.
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(Writer p)
        {
            //    Context c = new Context();
            //    var datavalue = c.Writers.FirstOrDefault(x => x.WriterMail == p.WriterMail && x.WriterPassword == p.WriterPassword);
            Context c = new Context();
            var datavalue = c.Writers.FirstOrDefault(x => x.WriterMail == p.WriterMail && x.WriterPassword == p.WriterPassword);
            if (datavalue != null)
            {
                //CookieOptions cookies = new CookieOptions();
                //cookies.Expires = DateTime.Now.AddDays(21);
                //Response.Cookies.Append("kullaniciAdi", datavalue.WriterName);
                //int denemeInt = datavalue.WriterId;

                //Response.Cookies.Append("kullaniciId", denemeInt.ToString());
               

                //adminse
                if (datavalue.WriterId == 15)
                {
                    //talep oluşturuldu.
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,p.WriterMail)
                        };
                    //useridentityde deger gondermemiz gereken yer de virgulden sonraki kisim startupda
                    //belirledigimiz authenticationsheme almasi gerekiyor yani cookie yetkilendirmesini yapmamizi sagliyor
                    var useridentity = new ClaimsIdentity(claims, "a");
                    ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                    await HttpContext.SignInAsync(principal); // gelen değeri şifreli bir şekilde cookie oluşturması için.
                    return RedirectToAction("Index", "Widget", new { area = "Admin" });
                }
                else //admin değilse
                {
                    //talep oluşturuldu.
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,p.WriterMail)
                        };
                    //useridentityde deger gondermemiz gereken yer de virgulden sonraki kisim startupda
                    //belirledigimiz authenticationsheme almasi gerekiyor yani cookie yetkilendirmesini yapmamizi sagliyor
                    var useridentity = new ClaimsIdentity(claims, "a");
                    ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                    await HttpContext.SignInAsync(principal); // gelen değeri şifreli bir şekilde cookie oluşturması için.
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return View();
            }
        
                


             
        }
        
        public async Task<IActionResult> WriterLogout()
        {

            await HttpContext.SignOutAsync();
        
            return RedirectToAction("Index", "Login");
        }

        //public IActionResult WriterLogout()
        //{
        //    HttpContext.Session.Remove("principal");
        //    return RedirectToAction("Index", "Login");
        //}
    }
}
////Session Komutları var.
//Context c = new Context();
//var datavalue = c.Writers.FirstOrDefault(x => x.WriterMail == p.WriterMail && x.WriterPassword == p.WriterPassword);
////FirstOrDefault twk değer getirmek için ya da tek değer üzerinden sorgulama yapmak için kullanılır.
//if (datavalue != null)
//{
//    HttpContext.Session.SetString("username", p.WriterMail);
//    return RedirectToAction("Index", "Writer");
//}
//else
//{
//    return View();
//}
