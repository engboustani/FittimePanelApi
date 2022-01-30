using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models
{
    public class CreatePaymentDTO
    {
        public Guid ExerciseId { get; set; }
        public int PaymentGetwayId { get; set; }
    }

    public class PaymentDTO : CreatePaymentDTO
    {
        public Guid Id { get; set; }
        public ExerciseDTO Exercise { get; set; }
        public PaymentGetawayDTO PaymentGetway { get; set; }
    }

    public class PaymentDetailDTO : PaymentDTO
    {
        public int Status { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }


    public class PaymentGetawayDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public ICollection<PaymentDTO> Payments { get; set; }
    }

    public class PaymentLinkDTO
    {
        public string Link { get; set; }
    }
}
