﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayBlogsSchema;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.DataAccess.Models.FairPlayShopSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;


namespace FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;

[Table("Income", Schema = "FairPlayBudget")]
public partial class Income
{
    [Key]
    public long IncomeId { get; set; }

    public DateTimeOffset IncomeDateTime { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(450)]
    public string OwnerId { get; set; }

    public long MonthlyBudgetInfoId { get; set; }

    public int CurrencyId { get; set; }

    [ForeignKey("CurrencyId")]
    [InverseProperty("Income")]
    public virtual Currency Currency { get; set; }

    [ForeignKey("MonthlyBudgetInfoId")]
    [InverseProperty("Income")]
    public virtual MonthlyBudgetInfo MonthlyBudgetInfo { get; set; }

    [ForeignKey("OwnerId")]
    [InverseProperty("Income")]
    public virtual AspNetUsers Owner { get; set; }
}