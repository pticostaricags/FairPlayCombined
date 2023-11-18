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

[Table("UserMessage", Schema = "FairPlayDating")]
public partial class UserMessage
{
    [Key]
    public long UserMessageId { get; set; }

    [Required]
    [StringLength(450)]
    public string FromApplicationUserId { get; set; }

    [Required]
    [StringLength(450)]
    public string ToApplicationUserId { get; set; }

    [Required]
    public string Message { get; set; }

    [ForeignKey("FromApplicationUserId")]
    [InverseProperty("UserMessageFromApplicationUser")]
    public virtual AspNetUsers FromApplicationUser { get; set; }

    [ForeignKey("ToApplicationUserId")]
    [InverseProperty("UserMessageToApplicationUser")]
    public virtual AspNetUsers ToApplicationUser { get; set; }
}