using BusinessLayer.Entities;
using BusinessLayer.Enums;
using DataLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
    [Authorize]
    public class RepairTicketsController : Controller
    {
        private readonly IRepairTicketService _ticketService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;

        public RepairTicketsController(
            IRepairTicketService ticketService,
            ICategoryService categoryService,
            UserManager<AppUser> userManager)
        {
            _ticketService = ticketService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var allTickets = await _ticketService.GetAllAsync();
                return View(allTickets);
            }

            var userId = _userManager.GetUserId(User);
            var userTickets = await _ticketService.GetByUserIdAsync(userId!);
            return View(userTickets);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && ticket.ClientId != userId)
            {
                return Forbid();
            }

            return View(ticket);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairTicket ticket)
        {
            var userId = _userManager.GetUserId(User);
            ticket.ClientId = userId!;
            ticket.Status = RepairStatus.Pending;

            //Removing navigation properties
            ModelState.Remove("ClientId");
            ModelState.Remove("Client");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                await _ticketService.CreateAsync(ticket);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", ticket.CategoryId);
            return View(ticket);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && ticket.ClientId != userId)
            {
                return Forbid();
            }

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", ticket.CategoryId);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RepairTicket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            var existingTicket = await _ticketService.GetByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && existingTicket.ClientId != userId)
            {
                return Forbid();
            }

            ModelState.Remove("ClientId");
            ModelState.Remove("Client");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                existingTicket.DeviceModel = ticket.DeviceModel;
                existingTicket.IssueDescription = ticket.IssueDescription;
                existingTicket.CategoryId = ticket.CategoryId;

                //Only the admin must and can change the status and price!!!
                if (User.IsInRole("Admin"))
                {
                    existingTicket.Status = ticket.Status;
                    existingTicket.EstimatedPrice = ticket.EstimatedPrice;
                }

                await _ticketService.UpdateAsync(existingTicket);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", ticket.CategoryId);
            return View(ticket);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && ticket.ClientId != userId)
            {
                return Forbid();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && ticket.ClientId != userId)
            {
                return Forbid();
            }

            await _ticketService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
