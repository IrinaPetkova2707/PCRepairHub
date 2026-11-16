using BusinessLayer.Entities;
using DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Services
{
    public class RepairTicketService : IRepairTicketService
    {
        private readonly AppDbContext _context;

        public RepairTicketService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RepairTicket>> GetAllAsync()
        {
            return await _context.RepairTickets
                .Include(t => t.Category)
                .Include(t => t.Client)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<RepairTicket>> GetByUserIdAsync(string userId)
        {
            return await _context.RepairTickets
                .Include(t => t.Category)
                .Where(t => t.ClientId == userId)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }

        public async Task<RepairTicket?> GetByIdAsync(int id)
        {
            return await _context.RepairTickets
                .Include(t => t.Category)
                .Include(t => t.Client)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task CreateAsync(RepairTicket ticket)
        {
            ticket.CreatedOn = DateTime.UtcNow;
            _context.RepairTickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RepairTicket ticket)
        {
            _context.RepairTickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.RepairTickets.FindAsync(id);
            if (ticket != null)
            {
                _context.RepairTickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
