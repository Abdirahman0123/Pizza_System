//using Foolproof;
using FoolProof.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Pizza_System.Model
{

    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        //[IgnoreDataMember]
        [JsonIgnore]
        //[IgnoreDataMember]
        public string? UserId { get; set; }  
        public int MenuId { get; set; }

        [IgnoreDataMember]
        internal User User { get; set; }

        [IgnoreDataMember]
        internal Menu Menu { get; set; }

        [Required]
        [RegularExpression("Collection|Delivery" )]
        public string DeliveryOption { get; set; }

        //[IgnoreDataMember]
        [Required(ErrorMessage = "Quantity is required")]
        
        [Range(1, 20, ErrorMessage ="Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }

        [RequiredIf(nameof(DeliveryOption), "Delivery")]
        public string Address1 { get; set; }

        [RequiredIf(nameof(DeliveryOption), "Delivery")]
        public string Address2 { get; set; }

        [RequiredIf(nameof(DeliveryOption), "Delivery")]
        public string Street { get; set; }
        [RequiredIf(nameof(DeliveryOption), "Delivery")]
        public string PostCode { get; set; }

        public long Total { get; set; }
    }
}
