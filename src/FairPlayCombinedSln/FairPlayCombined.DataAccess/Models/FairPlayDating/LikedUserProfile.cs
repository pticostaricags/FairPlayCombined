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


namespace FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;

[Table("LikedUserProfile", Schema = "FairPlayDating")]
[Index("LikingApplicationUserId", "LikedApplicationUserId", Name = "UI_LikedUserProfile_LikingApplicationUserId_LikedApplicationUserId", IsUnique = true)]
public partial class LikedUserProfile
{
    [Key]
    public long LikedUserProfileId { get; set; }

    [Required]
    public string LikingApplicationUserId { get; set; }

    [Required]
    public string LikedApplicationUserId { get; set; }

    public DateTimeOffset RowCreationDateTime { get; set; }

    [Required]
    [StringLength(256)]
    public string RowCreationUser { get; set; }

    [Required]
    [StringLength(250)]
    public string SourceApplication { get; set; }

    [Required]
    [Column("OriginatorIPAddress")]
    [StringLength(100)]
    public string OriginatorIpaddress { get; set; }

    [ForeignKey("LikedApplicationUserId")]
    [InverseProperty("LikedUserProfileLikedApplicationUser")]
    public virtual AspNetUsers LikedApplicationUser { get; set; }

    [ForeignKey("LikingApplicationUserId")]
    [InverseProperty("LikedUserProfileLikingApplicationUser")]
    public virtual AspNetUsers LikingApplicationUser { get; set; }
}