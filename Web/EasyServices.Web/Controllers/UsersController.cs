namespace EasyServices.Web.Controllers
{
    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Profile(string id)
        {
            var viewModel = this.usersService.GetUserById<UserProfileViewModel>(id);

            return this.View(viewModel);
        }
    }
}
