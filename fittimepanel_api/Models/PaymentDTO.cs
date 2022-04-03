using FittimePanelApi.Repository;
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

    public class PaymentListDTO
    {
        public Guid Id { get; set; }
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

    public class DiscardInfoDTO
    {
        public int Status { get; set; }
        public Guid Id { get; set; }
        public int DiscountType { get; set; }
        public string? Error { get; set; }
    }

    public class CreatePaymentDiscountDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Enabled { get; set; }
        public int Percentage { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; }
        public bool Limited { get; set; }
        public int Limit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class PaymentDiscountDTO : CreatePaymentDiscountDTO
    {
        public Guid Id { get; set; }
        public IList<PaymentListDTO> Payments { get; set; }
    }

    public class PaymentDiscountListAllItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PaymentDiscountPageAllItemDTO
    {
        public PageInfo PageInfo { get; set; }
        public IList<PaymentDiscountListAllItemDTO> ItemsList { get; set; }
    }

}
