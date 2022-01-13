using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolManagement_DBEntities db = new SchoolManagement_DBEntities();

        // GET: Enrollments
        public async Task<ActionResult> Index()
        {
            var enrollment = db.Enrollment.Include(e => e.Course).Include(e => e.Student).Include(e => e.Lecturers);
            return View(await enrollment.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "Title");
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "LastName");
            ViewBag.LecturerId = new SelectList(db.Lecturers, "LecturerId", "FirstName");
            return View();
        }

        // POST: Enrollments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnrollmentId,Grade,CourseId,StudentId,LecturerId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollment.Add(enrollment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "LastName", enrollment.StudentId);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "LecturerId", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }
        [HttpPost]
        public async Task<JsonResult> AddStudent([Bind(Include = "CourseId,StudentId")] Enrollment enrollment)
        {
            try
            {
                var isEnrolled = db.Enrollment.Any(q => q.CourseId == enrollment.CourseId && q.StudentId == enrollment.StudentId);
                if (ModelState.IsValid && !isEnrolled)
                {
                    db.Enrollment.Add(enrollment);
                    await db.SaveChangesAsync();
                    return Json(new { IsSuccess = true, Message = "Student Added Successfully" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = false, Message = "Student Is Already Enrolled" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Please contact your administrator" }, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: Enrollments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "LastName", enrollment.StudentId);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "LecturerId", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnrollmentId,Grade,CourseId,StudentId,LecturerId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Course, "CourseId", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(db.Student, "StudentId", "LastName", enrollment.StudentId);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "LecturerId", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enrollment enrollment = await db.Enrollment.FindAsync(id);
            db.Enrollment.Remove(enrollment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult GetStudents(string term)
        {
            var student = db.Student.Select(q => new { Name = q.FirstName + " " + q.LastName, Id = q.StudentId }).Where(q=>q.Name.Contains(term));
            return Json(student,JsonRequestBehavior.AllowGet);
        }
    }
}
