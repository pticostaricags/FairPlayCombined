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


namespace FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;

[Table("VideoInfographic", Schema = "FairPlayTube")]
[Index("VideoInfoId", "PhotoId", Name = "UI_VideoInfographic_VideoPhoto", IsUnique = true)]
public partial class VideoInfographic
{
    [Key]
    public long VideoInfographicId { get; set; }

    public long VideoInfoId { get; set; }

    public long PhotoId { get; set; }

    [ForeignKey("PhotoId")]
    [InverseProperty("VideoInfographic")]
    public virtual Photo Photo { get; set; }

    [ForeignKey("VideoInfoId")]
    [InverseProperty("VideoInfographic")]
    public virtual VideoInfo VideoInfo { get; set; }
}