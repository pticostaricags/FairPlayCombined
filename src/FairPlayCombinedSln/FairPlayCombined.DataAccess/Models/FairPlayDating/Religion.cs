﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.DataAccess.Models.FairPlayShopSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;


namespace FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;

[Table("Religion", Schema = "FairPlayDating")]
public partial class Religion
{
    [Key]
    public short ReligionId { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; }

    [InverseProperty("PreferredReligion")]
    public virtual ICollection<UserProfile> UserProfilePreferredReligion { get; set; } = new List<UserProfile>();

    [InverseProperty("Religion")]
    public virtual ICollection<UserProfile> UserProfileReligion { get; set; } = new List<UserProfile>();
}