using ShoppingStore_DlloSat.DAL.Entities;

namespace ShoppingStore_DlloSat.Models
{
    public class StateViewModel : State
    {
        public Guid CountryId { get; set; } //Este CountryId lo necesito para poder crear un estado.
    }
}
