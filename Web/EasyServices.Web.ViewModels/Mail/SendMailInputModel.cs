namespace EasyServices.Web.ViewModels.Mail
{
    using System.ComponentModel.DataAnnotations;

    using EasyServices.Common;

    public class SendMailInputModel
    {
        [MaxLength(100, ErrorMessage = ErrorMessages.TooBigMailTitle)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredField)]
        [EmailAddress(ErrorMessage = ErrorMessages.InvalidMail)]
        public string SenderMailAddress { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredField)]
        [MaxLength(1000, ErrorMessage = ErrorMessages.TooBigMailContent)]
        public string Content { get; set; }
    }
}
