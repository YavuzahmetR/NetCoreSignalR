using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR_Sample_Project.Web.Models;
using SignalR_Sample_Project.Web.Models.ViewModels;
using SignalR_Sample_Project.Web.Services;
using System.Diagnostics;

namespace SignalR_Sample_Project.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        FileService fileService,
        AppDbContext appDbContext) : Controller
    {
        
        
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if(!ModelState.IsValid) return View(signUpViewModel);

            var newUser = new IdentityUser()
            {
                UserName = signUpViewModel.Email,
                Email = signUpViewModel.Email
            };

            var result = await userManager.CreateAsync(newUser,signUpViewModel.PasswordConfirm);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction(nameof(SignIn));

        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (!ModelState.IsValid) return View(signInViewModel);

            var hasUser = await userManager.FindByEmailAsync(signInViewModel.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password Wrong");
            }

            var result = await signInManager.PasswordSignInAsync(hasUser!,signInViewModel.Password,true,false);

            if (!result.Succeeded) { ModelState.AddModelError(string.Empty, "Email or Password Wrong"); }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ProductList()
        {
            var user = await userManager.FindByEmailAsync("ayavuzisik@gmail.com");
            if(appDbContext.Products.Any(x => x.UserId == user!.Id))
            {
                var products = appDbContext.Products.Where(x=> x.UserId == user!.Id).ToList();
                return View(products);
            }

            var productList = new List<Product>() 
            {
                new Product() { Name = "Pen 1", Description = "Desc 1", Price = 100, UserId = user!.Id },
                new Product() { Name = "Pen 2", Description = "Desc 1", Price = 100, UserId = user!.Id },
                new Product() { Name = "Pen 3", Description = "Desc 1", Price = 100, UserId = user!.Id },
                new Product() { Name = "Pen 4", Description = "Desc 1", Price = 100, UserId = user!.Id },
                new Product() { Name = "Pen 5", Description = "Desc 1", Price = 100, UserId = user!.Id },
            };

            appDbContext.Products.AddRange(productList);
            await appDbContext.SaveChangesAsync();
            return View(productList);




        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateExcel()
        {
            var response = new
            {
                Status = await fileService.AddMessageToQueue()
            };

            return Json(response);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
