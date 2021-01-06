using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PacientuKonsultavimoSistema.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacientuKonsultavimoSistema.Models;

namespace PacientuKonsultavimoSistema.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext _context;

        public AdministrationController(UserManager<ApplicationUser> userManager, AppDbContext Db, 
                                        RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _context = Db;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        public IActionResult ListGydytojaiAdmin(string inputSpeciality, string institution)
        {
            var gydytojai = _context.Gydytojai;
            List<Gydytojas> gyd1 = new List<Gydytojas>();
            List<Gydytojas> gyd2 = new List<Gydytojas>();
            List<Gydytojas> gyd3 = new List<Gydytojas>();
            foreach (var g in gydytojai)
            {

                if (!String.IsNullOrEmpty(inputSpeciality) && !String.IsNullOrEmpty(institution))
                {
                    if (g.specialybe == inputSpeciality && g.istaiga == institution)
                    {
                        gyd1.Add(g);
                    }
                }
                else if (!String.IsNullOrEmpty(inputSpeciality) && String.IsNullOrEmpty(institution))
                {
                    if (g.specialybe == inputSpeciality)
                    {
                        gyd2.Add(g);
                    }
                }
                else if (String.IsNullOrEmpty(inputSpeciality) && !String.IsNullOrEmpty(institution))
                {
                    if (g.istaiga == institution)
                    {
                        gyd3.Add(g);
                    }
                }

            }

            if (!String.IsNullOrEmpty(inputSpeciality) && !String.IsNullOrEmpty(institution))
            {
                return View(gyd1);
            }
            else if (!String.IsNullOrEmpty(inputSpeciality) && String.IsNullOrEmpty(institution))
            {
                return View(gyd2);
            }
            else if (String.IsNullOrEmpty(inputSpeciality) && !String.IsNullOrEmpty(institution))
            {
                return View(gyd3);
            }


            return View(gydytojai);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
         
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }



        
        public async Task<IActionResult> GydytojoDelete(string id)
        {

            if (id == null)
            {
                return RedirectToAction("Error", "Administration");
            }

            Gydytojas personalDetail = _context.Gydytojai.Find(id);

            var user =  await userManager.FindByEmailAsync(personalDetail.ePastas);

            if (personalDetail == null || user == null)
            {
                return RedirectToAction("Error", "Administration");
            }
            else
            {
               //_context.Gydytojai.Remove(personalDetail);
                 await userManager.DeleteAsync(user);
               // _context.SaveChanges();
            }
            
            return RedirectToAction("ListGydytojaiAdmin", "Administration");
        }


        public async Task<IActionResult> GydytojoGrafikoKurimas(GydytojasGrafikasViewModel model, string id)
        {

            if (ModelState.IsValid)
            {
                var Gydytojas = _context.Gydytojai.Find(id);
                var esamasGrafikas = _context.Grafikas.Where(a => a.GydytojasId == id);
                int i = 0;
                int kiekis = 0;
                string laisvos = model.Laisvos;
                int[] dd = new int[31];
                if (model.Laisvos != null)
                {
                    string[] dienos = laisvos.Split(',');
                    foreach(string diena in dienos)
                    {
                        dd[i] = Convert.ToInt32(diena);
                        i++;
                    }
                }
                foreach (DateTime day in EachDay(model.AppointmentStart, model.AppointmentEnd))
                {
                    foreach(var graf in esamasGrafikas)
                    {
                        if(graf.Start.Date == day.Date)
                        {
                            kiekis++;
                        }
                    }
                    if (!dd.Contains(day.Day) && day.Date > DateTime.Now && kiekis == 0)
                    {
                        for (int j = 9; j < 18; j++)
                        {
                            DateTime data = new DateTime(day.Year, day.Month, day.Day, j, 0, 0);
                            DateTime data2 = new DateTime(day.Year, day.Month, day.Day, j, 45, 0);
                            var grafikas = new Grafikas
                            {

                                // Sukurti random Id arba autoincrement Id
                                // Paskirstyti Appointment Start ir End


                                Start = data,
                                End = data2,
                                GydytojasId = id,
                                Statusas = false,
                                GydVardas = Gydytojas.vardas,
                                GydPavarde = Gydytojas.pavarde

                            };
                            if (!_context.Grafikas.Contains(grafikas))
                            {
                                _context.Grafikas.Add(grafikas);
                                _context.SaveChanges();
                            }

                        }
                    }

                }

               // return RedirectToAction("ListGydytojaiAdmin", "Administration");
            }

            return View(model);
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
