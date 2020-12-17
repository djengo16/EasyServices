namespace EasyServices.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using EasyServices.Common;
    using EasyServices.Services.Data;
    using EasyServices.Services.Messaging;
    using EasyServices.Web.ViewModels;
    using EasyServices.Web.ViewModels.Announcements;
    using EasyServices.Web.ViewModels.Home;
    using EasyServices.Web.ViewModels.Mail;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IAnnouncementsService announcementsService;
        private readonly ICitiesService citiesService;
        private readonly ICategoriesService categoriesService;
        private readonly IUsersService usersService;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IEmailSender emailSender;

        public HomeController(
            IAnnouncementsService announcementsService,
            ICitiesService citiesService,
            ICategoriesService categoriesService,
            IUsersService usersService,
            ISubCategoriesService subCategoriesService,
            IEmailSender emailSender)
        {
            this.announcementsService = announcementsService;
            this.citiesService = citiesService;
            this.categoriesService = categoriesService;
            this.usersService = usersService;
            this.subCategoriesService = subCategoriesService;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                UsersCount = this.usersService.GetCount(),
                AnnouncementsCount = this.announcementsService.GetCount(),
                SubCategoriesCount = this.subCategoriesService.GetCount(),
                Announcements = await this.announcementsService.GetLastAsync<AnnouncementViewModel>(8),
                CitiesItems = this.citiesService.GetAllAsKeyValuePairs(),
                Categories = await this.categoriesService.GetCategoriesAndSubCategoriesAsync<AnnouncementCategoryInputModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Contacts()
        {
            var viewModel = new SendMailInputModel();

            viewModel.SenderMailAddress = this.User.Identity.Name;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Contacts(SendMailInputModel inpumodel)
        {
            var html = "<p>" + inpumodel.Content + "</p>";

            await this.emailSender.SendEmailAsync(
                GlobalConstants.SingleSender,
                inpumodel.SenderMailAddress,
                GlobalConstants.SystemMail,
                inpumodel.Title,
                html);

            this.TempData["Message"] = SuccesMessages.SuccessfulySendMail;

            return this.RedirectToAction();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
