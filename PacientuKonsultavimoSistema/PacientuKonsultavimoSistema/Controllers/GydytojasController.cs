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
    [Authorize(Roles = "Gydytojas")]
    public class GydytojasController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public GydytojasController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet] // Display visi gydytojai
        [AllowAnonymous]
        public IActionResult ListGydytojai(string inputSpeciality, string institution)
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


        public IActionResult ListGydytojaiPac(string inputSpeciality)
        {
            var gydytojai = _context.Gydytojai;
            List<Gydytojas> gyd = new List<Gydytojas>();
            foreach(var g in gydytojai)
            {
                if(g.specialybe == inputSpeciality)
                {
                    gyd.Add(g);
                }
            }

            if(!String.IsNullOrEmpty(inputSpeciality))
            {
                return View(gyd);
            }
     
                return View(gydytojai);
        }

        [HttpGet]
        
        public async Task<IActionResult> Dienotvarke(string sortOrder, DateTime data)
        {
            ViewBag.SortParam = String.IsNullOrEmpty(sortOrder) ? "data_desc" : "";
            var gydytojas = await _userManager.GetUserAsync(User);
            List<Grafikas> grafikas =  _context.Grafikas.ToList();
            List<Grafikas> NewGrafikas = new List<Grafikas>();

            foreach(var graf in grafikas)
            {
                if(graf.GydytojasId == Convert.ToString(gydytojas.Id) && graf.Start.Date>=DateTime.Now.Date && graf.Statusas && data == DateTime.MinValue)
                {
                    NewGrafikas.Add(graf);
                }
                else if (graf.GydytojasId == Convert.ToString(gydytojas.Id) && graf.Statusas && data != DateTime.MinValue)
                {
                    if (graf.Start.Date == data)
                    {
                        NewGrafikas.Add(graf);
                    }
                }
            }


            switch (sortOrder)
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
        
        public async Task<IActionResult> ManoPacientai(string Vardas, string vieta)
        {
            
            
            var gydytojas = await _userManager.GetUserAsync(User);
            List<Grafikas> grafikas = _context.Grafikas.ToList();
            _context.SaveChanges();
            List<Pacientas> pacientai = _context.Pacientai.ToList();
            List<Pacientas> NewPacientai = new List<Pacientas>();
            foreach (var graf in grafikas)
            {
                if(graf.GydytojasId == Convert.ToString(gydytojas.Id) && graf.Statusas)
                {
                    foreach(var pac in pacientai)
                    {
                        if (!NewPacientai.Contains(pac) && graf.PacientasId == pac.asmKodas && String.IsNullOrEmpty(Vardas) && vieta == null)
                        {
                            NewPacientai.Add(pac);
                        }
                        else if(!NewPacientai.Contains(pac) && graf.PacientasId == pac.asmKodas && String.IsNullOrEmpty(Vardas) && vieta != null)
                        {
                            if (pac.gyvenviete == vieta)
                            {
                                NewPacientai.Add(pac);
                            }
                        }
                        else if(!NewPacientai.Contains(pac) && graf.PacientasId == pac.asmKodas && !String.IsNullOrEmpty(Vardas) && vieta == null)
                        {
                            string Vardas2 = Vardas.ToLower();
                            if (pac.vardas.ToLower().Contains(Vardas2) || pac.pavarde.ToLower().Contains(Vardas2))
                            {
                                NewPacientai.Add(pac);
                            }
                        }
                        else if (!NewPacientai.Contains(pac) && graf.PacientasId == pac.asmKodas && !String.IsNullOrEmpty(Vardas) && vieta != null)
                        {
                            string Vardas2 = Vardas.ToLower();
                            if ((pac.vardas.ToLower().Contains(Vardas2) || pac.pavarde.ToLower().Contains(Vardas2)) && pac.gyvenviete == vieta)
                            {
                                NewPacientai.Add(pac);
                            }
                        }
                    }
                }
            }

            if (NewPacientai != null)
            {
                return View(NewPacientai);
            }
            else
                return View();
        }

      
       
        public async Task<IActionResult> PacientoLigosIstorija(string id, DateTime data)
        {

            var pacientas = _context.Pacientai.Find(id);
            ViewBag.Vardas = pacientas.vardas;
            ViewBag.Pavarde = pacientas.pavarde;
            ViewBag.Metai = pacientas.metai;
            ViewBag.Lytis= pacientas.lytis;
            ViewBag.Id2= id;

            List<LigosIstorija>  istorija = _context.LigosIstorija.ToList();
            List<LigosIstorija> NewIstorija = new List<LigosIstorija>();

            foreach (var ist in istorija)
            {
                if (ist.PacientasId == id && data == DateTime.MinValue)
                {
                    NewIstorija.Add(ist);
                }
                else if(ist.PacientasId == id && data != DateTime.MinValue)
                {
                    if(ist.Data.Date == data)
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



        [HttpPost]
        
        public async Task<IActionResult> LigosIstorija(LigosIstorijaViewModel model, string id)
        {

            if (ModelState.IsValid)
            {
                var gydytojas = await _userManager.GetUserAsync(User);
                var gyd = _context.Gydytojai.Find(gydytojas.Id);
                ViewBag.GydVardas = gyd.vardas;
                ViewBag.GydPav = gyd.pavarde;
                var istorija = new LigosIstorija
                {
                    PacientasId = id,
                    GydytojasId = gyd.asmKodas,
                    GydSpecialybe = gyd.specialybe,
                    Data = DateTime.Now,
                    Liga = model.Liga,
                    LigosAprasas = model.LigosAprasas,
                    GydVardas = gyd.vardas,
                    GydPavarde = gyd.pavarde
                };


                _context.Add<LigosIstorija>(istorija);
                _context.SaveChanges();
                return RedirectToAction("ManoPacientai", "Gydytojas");

            }
            else
                return View(model);
            
        }
    }
}

