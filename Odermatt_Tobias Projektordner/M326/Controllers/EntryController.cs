using M326.Data;
using M326.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M326.Controllers
{
    [Authorize]
    public class EntryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EntryController(Data.ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        //Löscht einen bestimmten Eintrag
        public IActionResult deleteEntry(int id)
        {
            var deleteEntry = dbContext.Entry.Find(id);
            if(deleteEntry != null)
            {
                var studentID = deleteEntry.student_ID;
                dbContext.Entry.Remove(deleteEntry);
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { studentID = studentID });
            }
            return NotFound();
        }

        [HttpGet]
        //Entschuldigt einen bestimmten Eintrag
        public IActionResult excuseEntry(int id)
        {
            var excuseEntry = dbContext.Entry.Find(id);
            if (excuseEntry != null)
            {
                var studentID = excuseEntry.student_ID;
                excuseEntry.Excused = true;
                dbContext.Entry.Update(excuseEntry);
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { studentID = studentID });
            }
            return NotFound();
        }



        [HttpGet]
        //Erstellt ein Schüler Model und übergibt die ID
        public IActionResult Create(int StudentID)
        {
            ViewBag.StudentID = StudentID;
            Entry newEntry = new Entry();
            return PartialView("_EntryModalPartial", newEntry);
        }

        [HttpGet]
        //Ruft das bearbeiten Modal auf, fall die angegebene ID nicht existiert wird 404 ausgeben
        public IActionResult Edit(int id)
        {
            var newEntry = dbContext.Entry.Find(id);
            if(newEntry == null) { return NotFound(); }
            ViewBag.StudentID = newEntry.student_ID;
            return PartialView("_EditEntryModelPartial", newEntry);
        }

        [HttpPost]
        public IActionResult manageEntry(Entry _entry)
        {
            //Falls Eintragswert ungültig ist = 404
            if (_entry.type == "Absenz" || _entry.type == "Verspätung")
            {
                //Falls ein Eintrag erstellt wird ist die ID = 0
                if (_entry.ID == 0)
                {
                    //Fügt der Datenbank die Klasse hinzu
                    dbContext.Entry.Add(_entry);
                }
                else
                {
                    //Aktuallisiert die Daten in der DB
                    dbContext.Entry.Update(_entry);
                }
                //Änderungen werden gespeichert
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { studentID = _entry.student_ID });
            }
            else {return NotFound(); }
        }


        [HttpGet]
        //Zeigt die Einträge eines Schülers, mit geliferte Paramter 
        public IActionResult Index(int studentID)
        {
            var entrysOfStudent = new List<M326.Models.Entry>();
            var Student = dbContext.Students.Find(studentID);
            var entrys = dbContext.Entry.ToList();

            //Wurde ein Schüler gefunden, werden die Einträge des entsprechenden Schülers ausgegeben
            if (Student != null)
            {
                int ExcusedDelayCount = 0;
                int ExcusedAbsenceCount = 0;
                int UnexcusedDelayCount = 0;
                int UnexcusedAbsenceCount = 0;

                if (entrys != null)
                {
                    foreach (var entry in entrys)
                    {
                        if (entry.student_ID == studentID) {
                            entrysOfStudent.Add(entry);

                            //Hier werden die Absenzen/Verspätungen gezählt, das ganze ist unterteilt zusätzlich mit Entschuldigt, Unentschuldigt
                            ExcusedDelayCount += entry.type == "Verspätung" && entry.Excused ? 1 : 0;
                            ExcusedAbsenceCount += entry.type == "Absenz" && entry.Excused ? 1 : 0;
                            UnexcusedDelayCount += entry.type == "Verspätung" && entry.Excused  == false ? 1 : 0;
                            UnexcusedAbsenceCount += entry.type == "Absenz" && entry.Excused == false ? 1 : 0;
                        }
                    }
                }
                ViewBag.ExcusedDelayCount = ExcusedDelayCount;
                ViewBag.ExcusedAbsenceCount = ExcusedAbsenceCount;
                ViewBag.UnexcusedDelayCount = UnexcusedDelayCount;
                ViewBag.UnexcusedAbsenceCount = UnexcusedAbsenceCount;
                ViewBag.Student = Student;
                ViewBag.Entry = entrysOfStudent;
                return View("Index");
         
            }
            //Existiert die ID nicht wird 404 ausgegeben
            else { return NotFound(); }
        }


    }
}
