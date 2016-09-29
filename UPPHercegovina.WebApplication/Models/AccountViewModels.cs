using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UPPHercegovina.WebApplication.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Zapamti browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Name; //ovo je za roles i nemam potrebe za ovim

        [Required(ErrorMessage = "Email je obavezno polje!")]
        [EmailAddress(ErrorMessage = "Loš format emaila!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezno polje !")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Lozinka i potvrda lozinke nisu iste!")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Općine")]
        public int TownshipId { get; set; }

        [Required(ErrorMessage = "Ime je obavezno polje !")]
        [Display(Name = "Ime")]
        [RegularExpression(@"^[a-zA-ZšćčđžŠČĆŽĐ\s]*$", ErrorMessage = "Brojevi i znakovi nisu dozvoljeni!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno polje !")]
        [Display(Name = "Prezime")]
        [RegularExpression(@"^[a-zA-ZšćčđžŠČĆŽĐ\s]*$", ErrorMessage = "Brojevi nisu dozvoljeni!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adresa je obavezno polje !")]
        [Display(Name = "Adresa")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefon je obavezno polje !")]
        [Display(Name = "Telefon")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{3,4}", ErrorMessage = "Unesite format 06X XXX XXX ili 06X XXX XXXX")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Matični broj je obavezno polje!")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Matični broj sadrži samo brojeve!")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Matični broj se sastoji od 13 brojeva!")]
        [Display(Name = "Matični broj")]
        public string IdentificationNumber { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrda lozinke")]
        [Compare("Password", ErrorMessage = "Lozinka i potvrda lozinke nisu iste.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ChangeProfileViewModel
    {
        //mislim da ovo uopste i ne koristim
        //onda ga nećemo ni uređivati :)
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Township { get; set; }
        public string PictureUrl { get; set; }
    }

    public class UsersViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Ime")]
        public string FirstName { get; set; }

        [Display(Name = "Prezime")]
        public string LastName { get; set; }

        [Display(Name = "Broj telefona")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Općina")]
        public string Township { get; set; }

        public static List<UsersViewModel> UsersToViewModel(bool status)
        {
            List<UsersViewModel> userViewList = new List<UsersViewModel>();

            List<ApplicationUser> user = new List<ApplicationUser>();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                user = context.Users.Where(u => u.Status != status)
                    .OrderBy(u => u.TownshipId).ThenBy(u => u.FirstName).ToList();

                user.ForEach(item => {
                    UsersViewModel userViewModel = new UsersViewModel()
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PhoneNumber = item.PhoneNumber,
                        Township = context.PlaceOfResidences.Find(item.TownshipId).Name,
                        Id = item.Id
                    };
                    userViewList.Add(userViewModel);         
                });
            }
            return userViewList;
        }
    }

    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Slika")]
        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "Ime je obavezno polje !")]
        [Display(Name = "Ime")]
        [RegularExpression(@"^[a-zA-ZšćčđžŠČĆŽĐ\s]*$", ErrorMessage = "Brojevi i znakovi nisu dozvoljeni!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno polje !")]
        [Display(Name = "Prezime")]
        [RegularExpression(@"^[a-zA-ZšćčđžŠČĆŽĐ\s]*$", ErrorMessage = "Brojevi nisu dozvoljeni!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Telefon je obavezno polje !")]
        [Display(Name = "Telefon")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{3,4}", ErrorMessage = "Unesite format 06X XXX XXX ili 06X XXX XXXX")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Općine")]
        public int TownshipId { get; set; }

        [Required(ErrorMessage = "Matični broj je obavezno polje!")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Matični broj sadrži samo brojeve!")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Matični broj se sastoji od 13 brojeva!")]
        [Display(Name = "Matični broj")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Adresa je obavezno polje !")]
        [Display(Name = "Adresa")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email je obavezno polje!")]
        [EmailAddress(ErrorMessage = "Loš format emaila!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Email potvrđen")]
        public bool EmailConfirmed { get; set; }
    }

}
