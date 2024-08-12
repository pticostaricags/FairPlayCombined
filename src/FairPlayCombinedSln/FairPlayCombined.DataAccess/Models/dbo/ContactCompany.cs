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


namespace FairPlayCombined.DataAccess.Models.dboSchema;

public partial class ContactCompany
{
    [Key]
    public long ContactCompanyId { get; set; }

    public long ContactId { get; set; }

    public long CompanyId { get; set; }

    [Required]
    [StringLength(50)]
    public string JobTitle { get; set; }

    [ForeignKey("CompanyId")]
    [InverseProperty("ContactCompany")]
    public virtual Company Company { get; set; }

    [ForeignKey("ContactId")]
    [InverseProperty("ContactCompany")]
    public virtual Contact Contact { get; set; }
}