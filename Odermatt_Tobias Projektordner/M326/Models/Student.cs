using System.ComponentModel.DataAnnotations;

namespace M326.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required]
        [StringLength(25)]
        public string firstname { get; set; }

        [Required]
        [StringLength(25)]
        public string lastname { get; set; }

        [Required]
        [StringLength(30)]
        public string street { get; set; }

        [Required]
        [StringLength(4)]
        public string plz { get; set; }

        [Required]
        [StringLength(50)]
        public string city { get; set; }

        //Damit das Datum in dem Form in diesem Format ist, und keine Minuten enthaltet
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        
        //Geburtsdatum eines Schülers/
        public DateTime birthday { get; set; }

        //Klassen ID zeigt in welcher Klasse ein Schüler ist
        public int class_ID { get; set; }
    }
}
