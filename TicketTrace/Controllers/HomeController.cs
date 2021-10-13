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
                    var obj = db.User.Where(a => a.Account.Equals(objUser.Account) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    
                    if (obj != null)
                    {
                        Session["UserID"] = obj.UID.ToString();
                        Session["UserName"] = obj.Name.ToString();
                        return RedirectToAction("UserDashBoard");
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