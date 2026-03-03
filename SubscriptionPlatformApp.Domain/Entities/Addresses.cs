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

        [MaxLength(256)]
        [Column("Address1")]
        public string Address1 { get; set; } = null!;

        [MaxLength(256)]
        [Column("Address2")]
        public string? Address2 { get; set; }

        [MaxLength(128)]
        [Column("District")]
        public string District { get; set; } = null!;

        [MaxLength(128)]
        [Column("SubDistrict")]
        public string SubDistrict { get; set; } = null!;

        [MaxLength(128)]
        [Column("Province")]
        public string Province { get; set; } = null!;

        [MaxLength(64)]
        [Column("Zipcode")]
        public string Zipcode { get; set; } = null!;

        [Column("AddressType")]
        public string AddressType { get; set; } = null!;
    }
}
