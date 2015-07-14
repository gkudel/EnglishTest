using EnglishTest.DAL;
using EnglishTest.Models;
using EnglishTest.ModelView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EnglishTest.Controllers
{
    public class TestController : Controller
    {
        private ETDbContext db = new ETDbContext();

        //
        // GET: /Test/
        public ActionResult Index()
        {
            return View(db.Tests);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Date,Finished")] Test test)
        {
            try
            {
                test.Start = DateTime.Now;
                test.Finished = false;
                if (ModelState.IsValid)
                {
                    db.Tests.Add(test);
                    db.SaveChanges();
                    return RedirectToAction("Play", new { Id = test.Id });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", " Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(test);
        }


        // GET: /Admin/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // POST: /Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Test test = db.Tests.Find(id);
                db.Tests.Remove(test);
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        private ActionResult Next(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            Step step = new Step() { TestId = test.Id };
            Word word = DrawNextWord(test);
            if (word != null)
            {
                step.PolishWord = word.Polish;
                step.EnglishWord = "";
                return View("Play", step);
            }
            else
            {
                try
                {
                    test.Finished = true;
                    test.Finish = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.Entry(test).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Done", new { Id = id });
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", " Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        private Word DrawNextWord(Test test)
        {
            List<Word> list = db.Words
                .Where(w => !db.Questions.Any(q => q.TestId == test.Id && q.WordId == w.Id)).ToList();

            int c = list.Count;
            if (c > 0)
            {
                Random r = new Random();
                return list[r.Next(0, c)];
            }
            return null;
        }

        public ActionResult Play(int? id)
        {
            return Next(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Play([Bind(Include="TestId, PolishWord, EnglishhWord")] Step step)
        {
            if (step == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Word w = db.Words.SingleOrDefault(word => word.Polish == step.PolishWord);
            if (w == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                if (w.English.ToUpper() != step.EnglishWord.ToUpper())
                {
                    this.ModelState.AddModelError("EnglishhWord", "Please try to one more time");
                    return View(step);
                }
                else
                {                   
                    try
                    {
                        Question q = new Question() { Pass = true, WordId = w.Id, TestId = step.TestId };                        
                        db.Questions.Add(q);
                        db.SaveChanges();
                        return RedirectToAction("Play", new { Id = step.TestId});
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", " Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            return View(step);
        }


        public ActionResult Finish(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        [HttpPost, ActionName("Finish")]
        [ValidateAntiForgeryToken]
        public ActionResult FinishPost(int id)
        {
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            try
            {
                test.Finished = true;
                test.Finish = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Entry(test).State = EntityState.Modified; 
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", " Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(test);
        }

        public ActionResult Done(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            Result r = new Result() { TestId = test.Id };
            var query = db.Questions.Where(q => q.TestId == test.Id);
            r.ResultMessage = "Good answers " + query.Where(q => q.Pass).Count() + " of " + query.Count();
            return View(r);
        }

        public JsonResult Help(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                Word w = db.Words.SingleOrDefault(word => word.Polish.ToUpper() == id.ToUpper());
                if (w != null)
                {
                    var result = new { EnglishhWord = w.English };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}