using System.Linq;
using System.Web.Mvc;
using TicketTrace.Models;

namespace TicketTrace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["RoleName"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult Ticket()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                using (TicketEntities db = new TicketEntities())
                {
                    var obj = db.User
                        .Join(db.Role, u => u.RID, r => r.RID, (u, r) => new { u = u, r = r })
                        .Where(a => a.u.Account.Equals(objUser.Account) && a.u.Password.Equals(objUser.Password)).FirstOrDefault();
                    
                    if (obj != null)
                    {
                        Session["UserID"] = obj.u.UID.ToString();
                        Session["UserName"] = obj.u.Name.ToString();
                        Session["RoleName"] = obj.r.RoleName.ToString();
                        return RedirectToAction("Index", "TicketMains");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}