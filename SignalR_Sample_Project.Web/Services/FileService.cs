using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR_Sample_Project.Web.Models;
using System.Threading.Channels;

namespace SignalR_Sample_Project.Web.Services
{
    public class FileService(AppDbContext appDbContext, UserManager<IdentityUser> userManager, 
        IHttpContextAccessor httpContextAccessor, Channel<(string userId, List<Product> products)> channel)
    {

        public async Task<bool> AddMessageToQueue()
        {
            var userId = userManager.GetUserId(httpContextAccessor.HttpContext!.User);

            var products = await appDbContext.Products.Where(x=> x.UserId == userId).ToListAsync();

            return channel.Writer.TryWrite((userId!,products));
        }
    }
}
