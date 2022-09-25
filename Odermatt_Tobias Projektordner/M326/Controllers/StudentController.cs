using M326.Data;
using M326.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M326.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public StudentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //Schüler Seite
        public IActionResult Index()
        {
            //Schüler werden dem ViewBag übergeen
            var Students = dbContext.Students.ToList();
            ViewBag.Students = Students;
            //Das Dicitionary wird der View hinzugefügt, damit die Klassen ID zu dem Namen aufgelöst werden kann.
            ViewBag.ClassDict = generateDictionaryforTable(Students);
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            Student newStudent = new Student();
            ViewBag.ClassDict = generateClassDictionaryforForm();
            return PartialView("_StudentModalPartial", newStudent);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var newStudent = dbContext.Students.Find(id);
            ViewBag.ClassDict = generateClassDictionaryforForm();
            return PartialView("_EditStudentModelPartial", newStudent);
        }


        [HttpPost]
        public IActionResult manageStudent(Student _student)
        {
            //Falls eine Klasse erstellt wird ist die = 0
            if (_student.ID == 0)
            {
                //Wurde  ein leerzeichen in einer dieser Felder eingegeben passiert nichts.
                if (_student.firstname != null && _student.lastname != null && _student.city != null && _student.street != null && _student.plz != null)
                {
                    //Fügt der Datenbank die Klasse hinzu
                    dbContext.Students.Add(_student);
                }
            }
            else
            {
                //Aktuallisiert die Daten in der DB
                dbContext.Students.Update(_student);
            }
            //Änderungen werden gespeichert
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        //Ruft das Lösch Modal auf und, übergibt das Model
        public IActionResult deleteStudent(int id)
        {
            var deleteStudent = dbContext.Students.Find(id);
            ViewBag.Student = deleteStudent;
            return PartialView("_DeleteStudentModelPartial", deleteStudent);
        }

        //Löscht einen Schüler
        [HttpGet]
        public IActionResult deleteStudentConfirmed(int id)
        {
            //Löscht die Klasse
            var deleteStudent = dbContext.Students.Find(id);
            if(deleteStudent != null)
            {
                dbContext.Students.Remove(deleteStudent);
                dbContext.SaveChanges();
            }
            else { return NotFound(); }
   
            return RedirectToAction("Index");
        }

        //Ein Dictionary wird generiert, dies ist für auflösung der Klassen ID zu Klassen Namen notwendig
        public IDictionary<int, String> generateDictionaryforTable(List<Student> Students)
        {
            IDictionary<int, String> ClassDict = new Dictionary<int, String>();

            foreach (var Student in Students)
            {
                var studentClassID = dbContext.Classes.Find(Student.class_ID);
                if (studentClassID != null)
                {
                    ClassDict[Student.class_ID] = studentClassID.name;
                }
                //Wurde keine Klasse unter diesem Schüler gefunden, wird keine Klasse ausegeben
                else
                {
                    ClassDict[Student.class_ID] = "Keine Klasse";
                }
            }
            return ClassDict;
        }


        //Dictionary das die ganzen Klassen ID und Klassen Namen beinhaltet
        public IDictionary<int, String> generateClassDictionaryforForm()
        {
            IDictionary<int, String> ClassDict = new Dictionary<int, String>();
            //Der Dictionary wird dann in den Viewbag geschoben
            if (dbContext.Classes != null)
            {
                foreach (var _class in dbContext.Classes)
                {
                    ClassDict[_class.ID] = _class.name;
                }
            }
            return ClassDict;
        }

        [HttpGet]
        public IActionResult details(int studentID)
        {
            var Student = dbContext.Students.Find(studentID);
            if (Student == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Student = Student;
                return PartialView("_DetailStudentModal");
            }
        }
    }
}
