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

        //Relación 1-N State a Country
        public ICollection<State>? States { get; set;}

        [Display(Name = "Estados/Departamentos")]
        //Esto es una popiedad de lectura que me sirve para contar los estados de un país
        public int StateNumber => States == null ? 0 : States.Count; //Recuerden que esto es un if ternario
    }
}
