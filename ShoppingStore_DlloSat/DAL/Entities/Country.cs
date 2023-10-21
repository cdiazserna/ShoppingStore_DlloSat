using System.ComponentModel.DataAnnotations;

namespace ShoppingStore_DlloSat.DAL.Entities
{
    //Nuestra primera entidad que se conertirá en tabla en la BD 
    public class Country : Entity
    {
        [Display(Name = "País")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")] //El campo "país" debe tener máximo "50" caracteres.
        [Required(ErrorMessage = "¡El campo {0} es requerido!")]
        public string Name { get; set; }
    }
}
