using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ShoppingStore_DlloSat.DAL.Entities
{
    public class State : Entity
    {
        [Display(Name = "Estado/Departamento")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")] //El campo "país" debe tener máximo "50" caracteres.
        [Required(ErrorMessage = "¡El campo {0} es requerido!")]
        public string Name { get; set; }

        //Relación 1-N State a Country
        public Country Country { get; set; }

        //Relación con City
        public ICollection<City> Cities { get; set; }

        [Display(Name = "Ciudades")]
        //Esto es una popiedad de lectura que me sirve para contar las ciudades de un Estado
        public int CityNumber => (Cities == null ? 0 : Cities.Count); //Recuerden que esto es un if ternario
    }
}
