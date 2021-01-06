using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PacientuKonsultavimoSistema.Models;

namespace PacientuKonsultavimoSistema.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager, AppDbContext Db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = Db;
        }

        [HttpGet]
        public IActionResult Prisijungti()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Prisijungti(PrisijungtiViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Prisijungia

                var result = await signInManager.PasswordSignInAsync(model.email, model.password, model.prisimintiMane, false);

                // tikriname ar sekmingai sukurta vartotojas
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Netinkami prisijungimo duomenys");
                }

            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Atsijungti()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult PacientoRegistracija()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PacientoRegistracija(PacientasRegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = model.email,
                    PhoneNumber = model.number,
                    Email = model.email,
                    Id = model.idCode,
                    FirstName = model.firstName,
                    LastName = model.lastName,
                    Year = model.year,
                    Sex = model.sex,
                    Country = model.country,
                    LivingPlace = model.livingPlace
                };
                var pacientas = new Pacientas
                {
                    vardas = model.firstName,
                    pavarde = model.lastName,
                    ePastas = model.email,
                    metai = model.year,
                    asmKodas = model.idCode,
                    telNr = model.number,
                    lytis = model.sex,
                    gyvenviete = model.livingPlace,
                    salis = model.country,
                    UserId = model.idCode
                };
                var result = await userManager.CreateAsync(user, model.password);
                
                _context.Add<Pacientas>(pacientas);
                _context.SaveChanges();
                // tikriname ar sekmingai sukurta vartotojas
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Pacientas"); // Priskiriama paciento role
                    await signInManager.SignInAsync(user, isPersistent: false); // sesion cookie
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var message = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                    ModelState.AddModelError("", message);
                }

            }

            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GydytojoRegistracija()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GydytojoRegistracija(GydytojasRegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = model.email,
                    PhoneNumber = model.number,
                    Email = model.email,
                    Id = model.idCode,
                    FirstName = model.firstName,
                    LastName = model.lastName,
                    Year = model.year,
                    Sex = model.sex,
                    Country = model.country,
                    Speciality = model.speciality,
                    Institution = model.institution
                };
                var gydytojas = new Gydytojas
                {
                    vardas = model.firstName,
                    pavarde = model.lastName,
                    ePastas = model.email,
                    metai = model.year,
                    asmKodas = model.idCode,
                    telNr = model.number,
                    lytis = model.sex,
                    salis = model.country,
                    specialybe = model.speciality,
                    istaiga = model.institution,
                    UserId = model.idCode
                };
                var result = await userManager.CreateAsync(user, model.password);
                _context.Add<Gydytojas>(gydytojas);
                _context.SaveChanges();
                // tikriname ar sekmingai sukurta vartotojas
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Gydytojas"); // Priskiriama gydytojo role
                    //await signInManager.SignInAsync(user, isPersistent: false); // sesion cookie
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var message = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                    ModelState.AddModelError("", message);
                }

            }        

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                bool isGydytojas = currentUser.IsInRole("Gydytojas");
                bool isPacientas = currentUser.IsInRole("Pacientas");
                if (isGydytojas)
                {
                    var id = userManager.GetUserId(User);
                    ApplicationUser applicationUser = await userManager.GetUserAsync(User);
                    applicationUser.UserName = model.Email;
                    applicationUser.Email = model.Email;
                    var result = await userManager.UpdateAsync(applicationUser);
                    var entity = _context.Gydytojai.FirstOrDefault(gyd => gyd.asmKodas == id);
                    var updateMessages = _context.Messages.Where(i => i.UserID == id).ToList();

                    if(updateMessages != null)
                    {
                        foreach(var i in updateMessages)
                        {
                            i.UserName = model.Email;
                        }
                    }

                    if (entity != null && result.Succeeded)
                    {
                        entity.ePastas = model.Email;
                        _context.SaveChanges();
                        await signInManager.SignOutAsync();
                        return RedirectToAction("EditSuccess", "Account");
                    }

                }
                if (isPacientas)
                {
                    var id = userManager.GetUserId(User);
                    ApplicationUser applicationUser = await userManager.GetUserAsync(User);
                    applicationUser.UserName = model.Email;
                    applicationUser.Email = model.Email;

                    var result = await userManager.UpdateAsync(applicationUser);
                    var entity = _context.Pacientai.FirstOrDefault(pac => pac.asmKodas == id);
                    var updateMessages = _context.Messages.Where(i => i.UserID == id).ToList();

                    if (updateMessages != null)
                    {
                        foreach (var i in updateMessages)
                        {
                            i.UserName = model.Email;
                        }
                    }


                    if (entity != null && result.Succeeded)
                    {
                        entity.ePastas = model.Email;
                        _context.SaveChanges();
                        await signInManager.SignOutAsync();
                        return RedirectToAction("EditSuccess", "Account");
                    }

                }
            }
            
            return View();
        }

    }
}
