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

[Index("NormalizedEmail", Name = "EmailIndex")]
public partial class AspNetUsers
{
    [Key]
    public string Id { get; set; }

    [StringLength(256)]
    public string UserName { get; set; }

    [StringLength(256)]
    public string NormalizedUserName { get; set; }

    [StringLength(256)]
    public string Email { get; set; }

    [StringLength(256)]
    public string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Lastname { get; set; }

    [InverseProperty("FromApplicationUser")]
    public virtual ICollection<ApplicationUserVouch> ApplicationUserVouchFromApplicationUser { get; set; } = new List<ApplicationUserVouch>();

    [InverseProperty("ToApplicationUser")]
    public virtual ICollection<ApplicationUserVouch> ApplicationUserVouchToApplicationUser { get; set; } = new List<ApplicationUserVouch>();

    [InverseProperty("User")]
    public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; } = new List<AspNetUserClaims>();

    [InverseProperty("User")]
    public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; } = new List<AspNetUserLogins>();

    [InverseProperty("User")]
    public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; } = new List<AspNetUserTokens>();

    [InverseProperty("OwnerApplicationUser")]
    public virtual ICollection<Company> Company { get; set; } = new List<Company>();

    [InverseProperty("OwnerApplicationUser")]
    public virtual ICollection<Contact> Contact { get; set; } = new List<Contact>();

    [InverseProperty("Owner")]
    public virtual ICollection<Expense> Expense { get; set; } = new List<Expense>();

    [InverseProperty("OwnerApplicationUser")]
    public virtual ICollection<Group> Group { get; set; } = new List<Group>();

    [InverseProperty("Owner")]
    public virtual ICollection<Income> Income { get; set; } = new List<Income>();

    [InverseProperty("LikedApplicationUser")]
    public virtual ICollection<LikedUserProfile> LikedUserProfileLikedApplicationUser { get; set; } = new List<LikedUserProfile>();

    [InverseProperty("LikingApplicationUser")]
    public virtual ICollection<LikedUserProfile> LikedUserProfileLikingApplicationUser { get; set; } = new List<LikedUserProfile>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<LinkedInConnection> LinkedInConnection { get; set; } = new List<LinkedInConnection>();

    [InverseProperty("Owner")]
    public virtual ICollection<MonthlyBudgetInfo> MonthlyBudgetInfo { get; set; } = new List<MonthlyBudgetInfo>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<NewVideoRecommendation> NewVideoRecommendation { get; set; } = new List<NewVideoRecommendation>();

    [InverseProperty("NotLikedApplicationUser")]
    public virtual ICollection<NotLikedUserProfile> NotLikedUserProfileNotLikedApplicationUser { get; set; } = new List<NotLikedUserProfile>();

    [InverseProperty("NotLikingApplicationUser")]
    public virtual ICollection<NotLikedUserProfile> NotLikedUserProfileNotLikingApplicationUser { get; set; } = new List<NotLikedUserProfile>();

    [InverseProperty("OwnerApplicationUser")]
    public virtual ICollection<OpenAiprompt> OpenAiprompt { get; set; } = new List<OpenAiprompt>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<PaypalTransaction> PaypalTransaction { get; set; } = new List<PaypalTransaction>();

    [InverseProperty("OwnerApplicationUser")]
    public virtual ICollection<Post> Post { get; set; } = new List<Post>();

    [InverseProperty("Owner")]
    public virtual ICollection<Product> Product { get; set; } = new List<Product>();

    [InverseProperty("Owner")]
    public virtual ICollection<Store> Store { get; set; } = new List<Store>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<UserActivity> UserActivity { get; set; } = new List<UserActivity>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<UserDataExportQueue> UserDataExportQueue { get; set; } = new List<UserDataExportQueue>();

    [InverseProperty("ApplicationUser")]
    public virtual UserFunds UserFunds { get; set; }

    [InverseProperty("ClaimedByApplicationUser")]
    public virtual ICollection<UserFundsUniqueCodes> UserFundsUniqueCodes { get; set; } = new List<UserFundsUniqueCodes>();

    [InverseProperty("FromApplicationUser")]
    public virtual ICollection<UserMessage> UserMessageFromApplicationUser { get; set; } = new List<UserMessage>();

    [InverseProperty("ToApplicationUser")]
    public virtual ICollection<UserMessage> UserMessageToApplicationUser { get; set; } = new List<UserMessage>();

    [InverseProperty("ApplicationUser")]
    public virtual UserMonetizationProfile UserMonetizationProfile { get; set; }

    [InverseProperty("ApplicationUser")]
    public virtual UserProfile UserProfile { get; set; }

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<VideoComment> VideoComment { get; set; } = new List<VideoComment>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<VideoInfo> VideoInfo { get; set; } = new List<VideoInfo>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<VideoPlan> VideoPlan { get; set; } = new List<VideoPlan>();

    [InverseProperty("WatchedByApplicationUser")]
    public virtual ICollection<VideoWatchTime> VideoWatchTime { get; set; } = new List<VideoWatchTime>();

    [InverseProperty("ApplicationUser")]
    public virtual ICollection<VisitorTracking> VisitorTracking { get; set; } = new List<VisitorTracking>();

    [ForeignKey("UserId")]
    [InverseProperty("User")]
    public virtual ICollection<AspNetRoles> Role { get; set; } = new List<AspNetRoles>();
}