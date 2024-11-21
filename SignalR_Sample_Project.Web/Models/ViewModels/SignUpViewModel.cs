using System.ComponentModel.DataAnnotations;

namespace SignalR_Sample_Project.Web.Models.ViewModels
{
    public record SignUpViewModel([Required] string Email, [Required] string Password, [Required] string PasswordConfirm);
}
