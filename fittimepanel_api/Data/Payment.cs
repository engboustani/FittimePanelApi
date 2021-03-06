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

    public enum DiscountType : int
    {
        Discount = 0,
        Percentage = 1,
    }

    public class Payment : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public User User { get; set; }
        public double Amount { get; set; }
        public Guid ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
        public int PaymentGetwayId { get; set; }
        public virtual PaymentGetaway PaymentGetway { get; set; }
        public PaymentStatus Status { get; set; }
        public string Token { get; set; }
        public PaymentDiscount? Discount { get; set; }
    }

    public class PaymentGetaway : BaseEntity
    {
        public PaymentGetaway()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameFa { get; set; }
        public bool Enabled { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }

    public class PaymentDiscount : BaseEntity
    {
        public PaymentDiscount()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Enabled { get; set; }
        public int Percentage { get; set; }
        public double Discount { get; set; }
        public DiscountType DiscountType { get; set; }
        public bool Limited { get; set; }
        public int Limit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }


}
