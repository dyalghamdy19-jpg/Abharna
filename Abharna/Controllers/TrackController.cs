using Abharna.Data;
using Abharna.Models;
using Microsoft.AspNetCore.Mvc;

namespace Abharna.Controllers
{
    public class TrackController : Controller
    {
        private readonly AppDbContext _db;

        public TrackController(AppDbContext db)
        {
            _db = db;
        }

        // only the Port Staff can add, edit and delete (SESSION)
        private bool IsStaff()
        {
            return HttpContext.Session.GetString("UserRole") == "Port Staff";
        }

        // ---------------- SEARCH one container (retrieve / search) ----------------

        public IActionResult Index(string? container)
        {
            ViewBag.Samples = _db.Containers.OrderBy(c => c.ContainerNumber).ToList();

            if (string.IsNullOrWhiteSpace(container))
            {
                return View();
            }

            string no = container.Trim().ToUpper();
            Container? found = _db.Containers.FirstOrDefault(c => c.ContainerNumber == no);
            if (found == null)
            {
                ViewBag.Error = "Container " + no + " was not found. Try one of the samples below.";
            }
            return View(found);
        }

        // ---------------- LIST + SEARCH all containers ----------------

        public IActionResult Manage(string? q)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var list = _db.Containers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                string s = q.Trim();
                list = list.Where(c => c.ContainerNumber.Contains(s) || c.Cargo.Contains(s)
                                    || c.Location.Contains(s) || c.Vessel.Contains(s));
            }

            ViewBag.Query = q;
            ViewBag.CanEdit = IsStaff();
            return View(list.OrderBy(c => c.ContainerNumber).ToList());
        }

        // ---------------- INSERT ----------------

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsStaff())
            {
                return RedirectToAction("Manage");
            }
            return View(new Container { Stage = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Container model)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Manage");
            }

            model.ContainerNumber = (model.ContainerNumber ?? "").Trim().ToUpper();
            if (_db.Containers.Any(c => c.ContainerNumber == model.ContainerNumber))
            {
                ModelState.AddModelError("ContainerNumber", "This container number already exists.");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _db.Containers.Add(model);
            _db.SaveChanges();
            TempData["Message"] = "Container " + model.ContainerNumber + " was added.";
            return RedirectToAction("Manage");
        }

        // ---------------- UPDATE ----------------

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Manage");
            }
            Container? c = _db.Containers.Find(id);
            if (c == null)
            {
                return RedirectToAction("Manage");
            }
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Container model)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Manage");
            }
            Container? c = _db.Containers.Find(model.Id);
            if (c == null)
            {
                return RedirectToAction("Manage");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            c.Cargo = model.Cargo;
            c.Stage = model.Stage;
            c.Location = model.Location;
            c.Vessel = model.Vessel;
            c.Eta = model.Eta;
            _db.SaveChanges();

            TempData["Message"] = "Container " + c.ContainerNumber + " was updated.";
            return RedirectToAction("Manage");
        }

        // ---------------- DELETE ----------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!IsStaff())
            {
                return RedirectToAction("Manage");
            }
            Container? c = _db.Containers.Find(id);
            if (c != null)
            {
                _db.Containers.Remove(c);
                _db.SaveChanges();
                TempData["Message"] = "Container " + c.ContainerNumber + " was deleted.";
            }
            return RedirectToAction("Manage");
        }
    }
}
