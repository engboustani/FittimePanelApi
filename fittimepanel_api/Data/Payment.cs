using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FittimePanelApi.Data
{
    public enum PaymentStatus : int
    {
        Created = 0,
        GoesToGetway = 1,
        Successful = 2,
        Failed = 3,
    }

    public class Payment : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public User User { get; set; }
        public Exercise Exercise { get; set; }
        public PaymentGetway PaymentGetway { get; set; }
        public PaymentStatus Status { get; set; }
    }

    public class PaymentGetway : BaseEntity
    {
        public PaymentGetway()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }

}
