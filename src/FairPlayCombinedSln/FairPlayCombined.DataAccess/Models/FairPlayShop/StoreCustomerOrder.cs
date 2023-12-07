﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.DataAccess.Models.FairPlayShopSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;


namespace FairPlayCombined.DataAccess.Models.FairPlayShopSchema;

[Table("StoreCustomerOrder", Schema = "FairPlayShop")]
public partial class StoreCustomerOrder
{
    [Key]
    public long StoreCustomerOrderId { get; set; }

    public long StoreCustomerId { get; set; }

    public DateTimeOffset OrderDateTime { get; set; }

    [Column(TypeName = "money")]
    public decimal OrderSubTotal { get; set; }

    [Column(TypeName = "money")]
    public decimal TaxTotal { get; set; }

    [Column(TypeName = "money")]
    public decimal OrderTotal { get; set; }

    [ForeignKey("StoreCustomerId")]
    [InverseProperty("StoreCustomerOrder")]
    public virtual StoreCustomer StoreCustomer { get; set; }

    [InverseProperty("StoreCustomerOrder")]
    public virtual ICollection<StoreCustomerOrderDetail> StoreCustomerOrderDetail { get; set; } = new List<StoreCustomerOrderDetail>();
}