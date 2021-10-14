using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FittimePanelApi.Data
{
    public class Ticket : BaseEntity
    {
        public Ticket()
        {
            TicketMessages = new HashSet<TicketMessage>();
            TicketStatuses = new HashSet<TicketStatus>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public virtual User UserCreated { get; set; }
        public virtual User UserAnswered { get; set; }
        public virtual ICollection<TicketMessage> TicketMessages { get; set; }
        public virtual ICollection<TicketStatus> TicketStatuses { get; set; }
    }

    public class TicketMessage : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual User User { get; set; }
        public string Text { get; set; }
    }

    public class TicketStatus : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int Status { get; set; }
        public string Text { get; set; }
    }
}
