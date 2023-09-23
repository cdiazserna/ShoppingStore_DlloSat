using System.ComponentModel.DataAnnotations;

namespace ShoppingStore_DlloSat.DAL.Entities
{
    public class Entity
    {
        [Required] //DataAnnotations que significa Obligatorio
        public Guid Id { get; set; } // PK

        [Display(Name = "Fecha de Creación")] //Display es para mostrar el nombre que quiero pintar en las vistas
        public DateTime? CreatedDate { get; set; } // Signo Elvis (?)... significa que este campo es Nulleable

        [Display(Name = "Fecha de Modificación")]
        public DateTime? ModifiedDate { get; set; }
    }
}
