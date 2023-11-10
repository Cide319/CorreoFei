using System.ComponentModel.DataAnnotations;

namespace CorreoFei.Models
{
    public class ContactoViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre de Contacto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo electrónico válida")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }
    }
}
