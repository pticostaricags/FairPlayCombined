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


namespace FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;

[Table("PostVisibility", Schema = "FairPlaySocial")]
[Index("Name", Name = "UI_PostVisibility_Name", IsUnique = true)]
public partial class PostVisibility
{
    [Key]
    public short PostVisibilityId { get; set; }

    [Required]
    [StringLength(11)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Description { get; set; }

    [InverseProperty("PostVisibility")]
    public virtual ICollection<Post> Post { get; set; } = new List<Post>();
}