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

[Table("VideoDigitalMarketingPlan", Schema = "FairPlayTube")]
[Index("VideoInfoId", "SocialNetworkName", Name = "IX_VideoDigitalMarketingPlan_SocialNetworkPlan")]
public partial class VideoDigitalMarketingPlan
{
    [Key]
    public long VideoDigitalMarketingPlanId { get; set; }

    public long VideoInfoId { get; set; }

    [Required]
    [StringLength(50)]
    public string SocialNetworkName { get; set; }

    [Required]
    public string HtmlDigitalMarketingPlan { get; set; }

    [Column("OpenAIPromptId")]
    public long OpenAipromptId { get; set; }

    [ForeignKey("OpenAipromptId")]
    [InverseProperty("VideoDigitalMarketingPlan")]
    public virtual OpenAiprompt OpenAiprompt { get; set; }

    [ForeignKey("VideoInfoId")]
    [InverseProperty("VideoDigitalMarketingPlan")]
    public virtual VideoInfo VideoInfo { get; set; }
}