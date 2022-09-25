using M326.Data;
using M326.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M326.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ClassController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Klassenstartseite
        public IActionResult Index()
        {
          //Die Daten der Klassen werden dem ViewBag übergeben
            ViewBag.Classes = dbContext.Classes.ToList();
            return View();
        }


        [HttpGet]
        //Wird der Create Class Button gedrück wird ein Partial geladen.
        public IActionResult Create()
        {
            //Neues Klassenmodel wird erstellt
            Class newClass = new Class();
            return PartialView("_ClassModalPartial", newClass);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //Die zu bearbeitende Klasse wird aufgerufen
            var newClass = dbContext.Classes.Find(id);
            return PartialView("_EditClassModelPartial", newClass);
        }


        [HttpPost]
        //Macht die Datenbank einträge für die Klasse, hier werden Klassen erstellt oder bearbeitet
        public IActionResult manageClass(Class _class)
        {
            //Falls eine Klasse erstellt wird ist die = 0
            if (_class.ID == 0)
            {
                //Falls Klasse oder Lehrername leer sind passiert nichts.
                if(_class.name != null && _class.teacherName != null)
                {
                    //Fügt der Datenbank die Klasse hinzu
                    dbContext.Classes.Add(_class);
                }
            }
            else
            {
                //Aktuallisiert die Daten in der DB
                dbContext.Classes.Update(_class);
            }
            //Änderungen werden gespeichert
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        //Ruft das Lösch Modal auf und, übergibt das Model
        public IActionResult deleteClass(int id)
        {
            var newClass = dbContext.Classes.Find(id);
            return PartialView("_DeleteClassModelPartial", newClass);
        }

        //Löscht einen Schüler
        [HttpPost]
        public IActionResult deleteClass(Class _class)
        {
            //Löscht die Klasse
            dbContext.Classes.Remove(_class);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        //Sollte die viewClass Seite auferufen werden.
        public IActionResult viewClass(int id)
        {
            var classToView = dbContext.Classes.Find(id);
            //Überprüft ob die Klasse existiert
            if (classToView != null)
            {
                //Klassennamen wird hier aufgelöst
                ViewBag.className = classToView.name;

                //Klassenlehrernamen wird hier aufgelöst
                ViewBag.teacherName = classToView.teacherName;


                //Erstellt eine Schüler Liste
                var StudentInClass = new List<Student>();

                //Jeder Schüler der in dieser Klasse ist wird in der Liste aufgenommen
                foreach (var student in dbContext.Students.ToList())
                {
                    if (student.class_ID == id) { StudentInClass.Add(student); }
                }
                //Falls keine Schüler existieren wird diese Variable auf true gesetzt
                //und die Liste wird nicht hinzugefügt
                ViewBag.emptyClass = StudentInClass.Count == 0 ? true : false;
                if (!ViewBag.emptyClass)
                {
                    ViewBag.studentsInClass = StudentInClass;
                }
                //Seite wird zurückgegeben
                return View("viewClass");
            }
            //Falls die Klasse nicht existiert wird ein Fehler zurückgegeben.
            else { return NotFound(); }
        }
    }

}
