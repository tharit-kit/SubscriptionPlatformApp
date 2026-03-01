using SubscriptionPlatformApp.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SubscriptionPlatformApp.Domain.Entities
{
    [Table("Addresses")]
    public class Addresses : BaseEntity
    {
        [Key]
        [Column("AddressId")]
        public Guid AddressId { get; set; }

        [Column("Address1")]
        public string Address1 { get; set; } = null!;

        [Column("Address2")]
        public string? Address2 { get; set; }

        [Column("District")]
        public string District { get; set; } = null!;

        [Column("SubDistrict")]
        public string SubDistrict { get; set; } = null!;

        [Column("Province")]
        public string Province { get; set; } = null!;

        [Column("Zipcode")]
        public string Zipcode { get; set; } = null!;

        public AddressType AddressType { get; set; } = null!;
    }
}
