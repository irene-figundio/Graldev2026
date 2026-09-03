using System.ComponentModel.DataAnnotations;

namespace Graldev.Web.Models
{
    public class ContactViewModel
    {
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio. / Last name is required.")]
        [Display(Name = "Cognome")]
        public string Cognome { get; set; } = "";

        [Required(ErrorMessage = "L'email aziendale è obbligatoria. / Business email is required.")]
        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido. / Enter a valid email address.")]
        [Display(Name = "Email Aziendale")]
        public string EmailAziendale { get; set; } = "";

        [Required(ErrorMessage = "Il nome dell'azienda è obbligatorio. / Company name is required.")]
        [Display(Name = "Azienda")]
        public string Azienda { get; set; } = "";

        [Required(ErrorMessage = "Il ruolo è obbligatorio. / Job title is required.")]
        [Display(Name = "Ruolo")]
        public string Ruolo { get; set; } = "";

        [Required(ErrorMessage = "L'area di interesse è obbligatoria. / Area of interest is required.")]
        [Display(Name = "Area di interesse")]
        public string AreaInteresse { get; set; } = "";

        [Required(ErrorMessage = "Il messaggio è obbligatorio. / Message is required.")]
        [MinLength(10, ErrorMessage = "Il messaggio deve essere lungo almeno 10 caratteri. / Message must be at least 10 characters.")]
        [Display(Name = "Messaggio")]
        public string Messaggio { get; set; } = "";

        // Google reCAPTCHA v3 token
        public string? RecaptchaToken { get; set; }

        // Honeypot field - must be empty
        public string? Website { get; set; }
    }
}
