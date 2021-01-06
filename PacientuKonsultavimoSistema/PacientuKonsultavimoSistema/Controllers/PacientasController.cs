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
    // Si controller gali pasiekti tik pacientai
    [Authorize(Roles = "Pacientas")]
    public class PacientasController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public PacientasController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet] // Display visi gydytojai
       
        public IActionResult ListGydytojai()
        {
            var gydytojai = _context.Gydytojai;
            return View(gydytojai);
        }

        
        public IActionResult ListGydytojaiPac(string inputSpeciality, string institution)
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


        [HttpGet]
        public IActionResult RegistruotisPasGydytoja()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistruotisPasGydytoja(string id)
        {

            return RedirectToAction("ListGydGrafikas", id);
        }

        
        public async Task<IActionResult> ListGydGrafikas(string id, DateTime data, string sortOrder)
        {
            ViewBag.SortParam = String.IsNullOrEmpty(sortOrder) ? "data_desc" : "";
            DateTime now = DateTime.Now;
            //int IntId = Convert.ToInt32(id);

            var graf = _context.Grafikas.ToList();
            List<Grafikas> grafNew = new List<Grafikas>();
            if (graf != null)
            {
                foreach (var g in graf)
                {
                    if (g.GydytojasId == id && g.Start.Date>= DateTime.Now.Date && g.Statusas == false && data == DateTime.MinValue)
                    {
                        grafNew.Add(g); 
                    }
                    else if (g.GydytojasId == id && g.Start.Date >= DateTime.Now.Date && g.Statusas == false && data != DateTime.MinValue)
                    {
                        if(g.Start.Date == data.Date)
                        {
                            grafNew.Add(g);
                        }
                    }
                }


                //NewGrafikas.Sort();
                switch (sortOrder)
                {
                    case "data_desc":
                        grafNew.Sort(delegate (Grafikas x, Grafikas y)
                        {
                            if (x.Start == null && y.Start == null) return 0;
                            else if (x.Start == null) return -1;
                            else if (y.Start == null) return 1;
                            else return y.Start.Date.CompareTo(x.Start.Date);
                        });
                        break;
                    default:
                        grafNew.Sort(delegate (Grafikas x, Grafikas y)
                        {
                            if (x.Start == null && y.Start == null) return 0;
                            else if (x.Start == null) return -1;
                            else if (y.Start == null) return 1;
                            else return x.Start.Date.CompareTo(y.Start.Date);
                        });
                        break;

                }

                return View(grafNew);
            }

            return View();


        }

        public async Task<IActionResult> Registracija(string id)
        {
            int ID = Convert.ToInt32(id);
            if (id == null)
            {
                return RedirectToAction("Error", "Administration");
            }

            Grafikas graf = _context.Grafikas.Find(ID);
           

            if (graf == null)
            {
                return RedirectToAction("Error", "Administration");
            }
            else
            {
                var pacientas = await _userManager.GetUserAsync(User);
                graf.Statusas = true;
                var Pac = _context.Pacientai.Find(pacientas.Id);
                graf.PacientasId = Pac.asmKodas;
                graf.PacVardas = Pac.vardas;
                graf.PacPavarde = Pac.pavarde;

                // _context.Database.("update dbo.Blogs set Name = 'My Boring Blog' where Id = 1");
                //_context.Gydytojai.Remove(personalDetail);
                _context.Update(graf);
                _context.SaveChanges();
                return RedirectToAction("ListGydytojaiPac", "Pacientas");
            }

            //return RedirectToAction("ListGydytojaiPac", "Pacientas");
        }

        
        
        public async Task<IActionResult> Apsilankymai(string sortOrder, DateTime data)
        {
            ViewBag.SortParam = String.IsNullOrEmpty(sortOrder) ? "data_desc" : "";
            var pacientas = await _userManager.GetUserAsync(User);
            List<Grafikas> grafikas = _context.Grafikas.ToList();
            List<Grafikas> NewGrafikas = new List<Grafikas>();

            foreach (var graf in grafikas)
            {
                if (graf.PacientasId == Convert.ToString(pacientas.Id) && data==DateTime.MinValue)
                {
                    NewGrafikas.Add(graf);
                }
                else if (graf.PacientasId == Convert.ToString(pacientas.Id) && data != DateTime.MinValue)
                {
                    if (graf.Start.Date == data)
                    {
                        NewGrafikas.Add(graf);
                    }
                }
            }
            //NewGrafikas.Sort();
            switch(sortOrder)
            {
                case "data_desc":
                    NewGrafikas.Sort(delegate (Grafikas x, Grafikas y)
                    {
                        if (x.Start == null && y.Start == null) return 0;
                        else if (x.Start == null) return -1;
                        else if (y.Start == null) return 1;
                        else return y.Start.Date.CompareTo(x.Start.Date);
                    });
                    break;
                        default:
                    NewGrafikas.Sort(delegate (Grafikas x, Grafikas y)
                    {
                        if (x.Start == null && y.Start == null) return 0;
                        else if (x.Start == null) return -1;
                        else if (y.Start == null) return 1;
                        else return x.Start.Date.CompareTo(y.Start.Date);
                    });
                    break;

            }
            

             return View(NewGrafikas);
        }

        [HttpGet]
        
        public async Task<IActionResult> SortData(string data)
        {
            var pacientas = await _userManager.GetUserAsync(User);
            List<Grafikas> grafikas = _context.Grafikas.ToList();
            List<Grafikas> NewGrafikas = new List<Grafikas>();

            foreach (var graf in grafikas)
            {
                if (graf.PacientasId == Convert.ToString(pacientas.Id) && graf.Start.ToString() == data)
                {
                    NewGrafikas.Add(graf);
                }
            }

            


            return View(NewGrafikas);
        }

        
        
        public async Task<IActionResult> LigosIstorija(DateTime data)
        {
            var pacientas = await _userManager.GetUserAsync(User);

            
            List<LigosIstorija> istorija = _context.LigosIstorija.ToList();
            List<LigosIstorija> NewIstorija = new List<LigosIstorija>();

            foreach (var ist in istorija)
            {
                if (ist.PacientasId == pacientas.Id && data == DateTime.MinValue)
                {
                    NewIstorija.Add(ist);
                }
                else if(ist.PacientasId == pacientas.Id && data != DateTime.MinValue)
                {
                        if (ist.Data.Date == data.Date)
                        {
                            NewIstorija.Add(ist);
                        }
                    }

                }

            if (NewIstorija != null)
            {
                return View(NewIstorija);
            }
            else
                return View();
        }

    }
}
