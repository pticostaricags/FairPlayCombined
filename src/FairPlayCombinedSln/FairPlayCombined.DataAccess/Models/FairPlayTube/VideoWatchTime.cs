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

[Table("VideoWatchTime", Schema = "FairPlayTube")]
[Index("SessionId", Name = "UI_VideoWatchTime_SessionId", IsUnique = true)]
[Index("VideoInfoId", "SessionId", Name = "UI_VideoWatchTime_VideoInfoId_SessionId", IsUnique = true)]
public partial class VideoWatchTime
{
    [Key]
    public long VideoWatchTimeId { get; set; }

    public long VideoInfoId { get; set; }

    public Guid SessionId { get; set; }

    public DateTimeOffset SessionStartDatetime { get; set; }

    public int WatchTime { get; set; }

    [StringLength(450)]
    public string WatchedByApplicationUserId { get; set; }

    [ForeignKey("VideoInfoId")]
    [InverseProperty("VideoWatchTime")]
    public virtual VideoInfo VideoInfo { get; set; }

    [ForeignKey("WatchedByApplicationUserId")]
    [InverseProperty("VideoWatchTime")]
    public virtual AspNetUsers WatchedByApplicationUser { get; set; }
}