using Abharna.Data;
using Abharna.Helpers;
using Abharna.Models;
using Microsoft.AspNetCore.Mvc;

namespace Abharna.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // ---------------- Login ----------------

        [HttpGet]
        public IActionResult Login()
        {
            // COOKIE: read the e-mail saved from the last login
            ViewBag.LastEmail = Request.Cookies["lastEmail"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password, bool remember)
        {
            email = (email ?? "").Trim();

            // retrieve the user from the database
            User? user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || user.Password != PasswordHelper.Hash(password ?? ""))
            {
                ViewBag.Error = "Wrong email or password.";
                ViewBag.LastEmail = Request.Cookies["lastEmail"];
                return View();
            }

            // SESSION: remember who is signed in
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.AccountType);

            // COOKIE: remember the e-mail for 7 days (or delete it)
            if (remember)
            {
                Response.Cookies.Append("lastEmail", user.Email, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true
                });
            }
            else
            {
                Response.Cookies.Delete("lastEmail");
            }

            return RedirectToAction("Welcome");
        }

        // ---------------- Welcome (needs a session) ----------------

        public IActionResult Welcome()
        {
            string? name = HttpContext.Session.GetString("UserName");
            if (name == null)
            {
                return RedirectToAction("Login");
            }

            string role = HttpContext.Session.GetString("UserRole") ?? "";
            ViewBag.UserName = name;
            ViewBag.UserRole = role;
            ViewBag.Tools = GetTools(role);
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private List<string> GetTools(string role)
        {
            switch (role)
            {
                case "Port Staff":
                    return new List<string> { "Register ships and berths", "Register containers", "Update ship and container status" };
                case "Port Manager":
                    return new List<string> { "See vessels and containers", "Follow container distribution", "Watch customs holds" };
                case "Customs Broker":
                    return new List<string> { "Open the clearance queue", "Update clearance", "Release or hold containers" };
                case "Shipping Rep":
                    return new List<string> { "Follow vessels", "See containers", "Check berths" };
                default:
                    return new List<string> { "Track your containers", "Read notifications", "See the estimated arrival time" };
            }
        }

        // ---------------- Sign up ----------------

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(string fullname, string email, string phone, string password, string accountType, string contact)
        {
            fullname = (fullname ?? "").Trim();
            email = (email ?? "").Trim();
            phone = (phone ?? "").Trim();
            password = password ?? "";
            accountType = accountType ?? "";

            // validation on the server
            if (fullname.Length < 3)
            {
                ViewBag.Error = "The name must have 3 characters or more.";
                return View();
            }
            if (phone.Length != 10 || !phone.StartsWith("05") || !phone.All(char.IsDigit))
            {
                ViewBag.Error = "The phone must have 10 digits and start with 05.";
                return View();
            }
            if (password.Length < 8)
            {
                ViewBag.Error = "The password must have 8 characters or more.";
                return View();
            }
            if (accountType == "")
            {
                ViewBag.Error = "Please choose an account type.";
                return View();
            }
            if (_db.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "This email is already registered.";
                return View();
            }

            // insert the new user into the database
            User user = new User
            {
                FullName = fullname,
                Email = email,
                Phone = phone,
                Password = PasswordHelper.Hash(password),
                AccountType = accountType,
                ContactBy = contact ?? "Email"
            };
            _db.Users.Add(user);
            _db.SaveChanges();

            TempData["Message"] = "Account created. You can sign in now.";
            return RedirectToAction("Login");
        }
    }
}
