using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PacientuKonsultavimoSistema.Models;

namespace PacientuKonsultavimoSistema.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var currentUser = await _userManager.GetUserAsync(User);

            ViewBag.CurrentUserName = currentUser.FirstName +" " +currentUser.LastName;
            var messages = await _context.Messages.ToListAsync();

            return View(messages);
        }

        public async Task<IActionResult> Create(Message message)
        {
            if (ModelState.IsValid)
            {
                
                              
                var sender = await _userManager.GetUserAsync(User);
                message.UserName = sender.FirstName + " " + sender.LastName;
                message.UserID = sender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return View();
        }
    }
}
