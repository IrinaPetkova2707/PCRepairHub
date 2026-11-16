using BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public interface IRepairTicketService
    {
        Task<IEnumerable<RepairTicket>> GetAllAsync();
        Task<IEnumerable<RepairTicket>> GetByUserIdAsync(string userId);
        Task<RepairTicket?> GetByIdAsync(int id);
        Task CreateAsync(RepairTicket ticket);
        Task UpdateAsync(RepairTicket ticket);
        Task DeleteAsync(int id);
    }
}
