using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Enums;

namespace BusinessLayer.Entities
{
    public class RepairTicket
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Device model is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Device model must be between 2 and 100 characters.")]
        public string DeviceModel { get; set; } = null!;
        [Required(ErrorMessage = "Issue description is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Issue description must be between 10 and 1000 characters.")]
        public string IssueDescription { get; set; } = null!;
        [Required]
        [Range(0.00, 10000.00, ErrorMessage = "Estimated price must be between 0 and 10,000.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedPrice { get; set; }
        [Required]
        public RepairStatus Status { get; set; } = RepairStatus.Pending;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        //1:N relationship to Category
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        //1:N relationship to AppUser
        [Required]
        public string ClientId { get; set; } = null!;
        [ForeignKey(nameof(ClientId))]
        public AppUser Client { get; set; } = null!;
    }
}
