using System.ComponentModel.DataAnnotations;

namespace M326.Models
{
    public class Entry
    {
        public int ID { get; set; }

        [Required]
        //Gibt an um welche Art von Versäumnisses es sich handelt
        public string type { get; set; }

        [Required]
        //Wurde das Versäumniss Entschuldigt?
        public Boolean Excused { get; set; }

        [Required]
        //Zeit des Versäumisses
        public DateTime TimeOfAction { get; set; }

        [Required]
        //Schüler ID
        public int student_ID { get; set; }
    }
}
