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


namespace FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;

[Table("VideoInfo", Schema = "FairPlayTube")]
public partial class VideoInfo
{
    [Key]
    public long VideoInfoId { get; set; }

    public Guid AccountId { get; set; }

    [StringLength(50)]
    public string VideoId { get; set; }

    [Required]
    [StringLength(50)]
    public string Location { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [StringLength(50)]
    public string FileName { get; set; }

    [StringLength(500)]
    public string VideoBloblUrl { get; set; }

    [StringLength(500)]
    public string IndexedVideoUrl { get; set; }

    /// <summary>
    /// Video Owner Id
    /// </summary>
    [Required]
    [StringLength(450)]
    public string ApplicationUserId { get; set; }

    public short VideoIndexStatusId { get; set; }

    public double VideoDurationInSeconds { get; set; }

    [StringLength(500)]
    public string VideoIndexSourceClass { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string ExternalVideoSourceUrl { get; set; }

    [StringLength(10)]
    public string VideoLanguageCode { get; set; }

    public short VideoVisibilityId { get; set; }

    [StringLength(500)]
    public string ThumbnailUrl { get; set; }

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

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("VideoInfo")]
    public virtual AspNetUsers ApplicationUser { get; set; }

    [ForeignKey("VideoIndexStatusId")]
    [InverseProperty("VideoInfo")]
    public virtual VideoIndexStatus VideoIndexStatus { get; set; }

    [InverseProperty("VideoInfo")]
    public virtual ICollection<VideoIndexingTransaction> VideoIndexingTransaction { get; set; } = new List<VideoIndexingTransaction>();

    [InverseProperty("VideoInfo")]
    public virtual ICollection<VideoJob> VideoJob { get; set; } = new List<VideoJob>();

    [ForeignKey("VideoVisibilityId")]
    [InverseProperty("VideoInfo")]
    public virtual VideoVisibility VideoVisibility { get; set; }
}