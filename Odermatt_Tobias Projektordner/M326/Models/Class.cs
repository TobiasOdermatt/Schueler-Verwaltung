using System.ComponentModel.DataAnnotations;

namespace M326.Models
{
    public class Class
    {
        //Klassen ID
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        //Klassennamen
        public string name { get; set; }

        [Required]
        [StringLength(25)]
        //Name des Klassenlehrers
        public string teacherName{ get; set;}

    }
}
