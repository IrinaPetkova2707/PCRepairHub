using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 60 characters.")]
        public string Name { get; set; } = null!;
        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string? Description { get; set; }
        public ICollection<RepairTicket> RepairTickets { get; set; } = new List<RepairTicket>();
    }
}
