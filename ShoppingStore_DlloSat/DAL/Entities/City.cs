using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ShoppingStore_DlloSat.DAL.Entities
{
    public class City : Entity
    {
        [Display(Name = "Categoría")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")] //El campo "país" debe tener máximo "50" caracteres.
        [Required(ErrorMessage = "¡El campo {0} es requerido!")]
        public string Name { get; set; }

        //Relación con State
        public State? State { get; set; }
    }
}
