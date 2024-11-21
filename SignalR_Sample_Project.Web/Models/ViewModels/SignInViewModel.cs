using System.ComponentModel.DataAnnotations;

namespace SignalR_Sample_Project.Web.Models.ViewModels
{
    public record SignInViewModel([Required] string Email, [Required] string Password);


}
