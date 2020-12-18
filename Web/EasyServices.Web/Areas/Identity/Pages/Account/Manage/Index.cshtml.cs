namespace EasyServices.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using EasyServices.Data.Models;
    using EasyServices.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Cloudinary cloudinary;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            Cloudinary cloudinary)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.cloudinary = cloudinary;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Имена")]
            public string Name { get; set; }

            [Phone]
            [Display(Name = "Телефон")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Описание")]
            public string DescriptionInfo { get; set; }

            [Display(Name = "Уебсайт")]
            public string WebSite { get; set; }

            [Display(Name = "Профилна снимка")]

            public string ProfilePictureUrl { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            this.Input = new InputModel
            {
                Name = user.Name,
                PhoneNumber = phoneNumber,
                DescriptionInfo = user.Description,
                WebSite = user.WebSite,
                ProfilePictureUrl = user.ProfilePicture,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {

            ;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            if (this.Input.Name != user.Name)
            {
                user.Name = this.Input.Name;
            }

            if (this.Input.DescriptionInfo != user.Description)
            {
                user.Description = this.Input.DescriptionInfo;
            }

            if (this.Input.WebSite != user.WebSite)
            {
                user.WebSite = this.Input.WebSite;
            }

            if (file != null)
            {
                await CloudinaryHelper.RemoveFileAsync(this.cloudinary, user.ProfilePicture); // Deleting the old img

                string imageUrl = await CloudinaryHelper.UploadFileAsync(this.cloudinary, file, true);
                user.ProfilePicture = imageUrl;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Профила е обновен";
            return this.RedirectToPage();
        }
    }
}
