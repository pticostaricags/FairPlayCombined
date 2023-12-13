﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using FairPlayCombined.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.DataAccess.Models.FairPlayShopSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;

namespace FairPlayCombined.DataAccess.Data;

public partial class FairPlayCombinedDbContext : DbContext
{
    public FairPlayCombinedDbContext(DbContextOptions<FairPlayCombinedDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activity { get; set; }

    public virtual DbSet<ApplicationUserVouch> ApplicationUserVouch { get; set; }

    public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }

    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

    public virtual DbSet<City> City { get; set; }

    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<Culture> Culture { get; set; }

    public virtual DbSet<Currency> Currency { get; set; }

    public virtual DbSet<DateObjective> DateObjective { get; set; }

    public virtual DbSet<ErrorLog> ErrorLog { get; set; }

    public virtual DbSet<Expense> Expense { get; set; }

    public virtual DbSet<EyesColor> EyesColor { get; set; }

    public virtual DbSet<Frequency> Frequency { get; set; }

    public virtual DbSet<Gender> Gender { get; set; }

    public virtual DbSet<Group> Group { get; set; }

    public virtual DbSet<HairColor> HairColor { get; set; }

    public virtual DbSet<Income> Income { get; set; }

    public virtual DbSet<KidStatus> KidStatus { get; set; }

    public virtual DbSet<LikedUserProfile> LikedUserProfile { get; set; }

    public virtual DbSet<MonthlyBudgetInfo> MonthlyBudgetInfo { get; set; }

    public virtual DbSet<NotLikedUserProfile> NotLikedUserProfile { get; set; }

    public virtual DbSet<OpenAiprompt> OpenAiprompt { get; set; }

    public virtual DbSet<Photo> Photo { get; set; }

    public virtual DbSet<Post> Post { get; set; }

    public virtual DbSet<PostType> PostType { get; set; }

    public virtual DbSet<PostVisibility> PostVisibility { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductStatus> ProductStatus { get; set; }

    public virtual DbSet<Religion> Religion { get; set; }

    public virtual DbSet<Resource> Resource { get; set; }

    public virtual DbSet<StateOrProvince> StateOrProvince { get; set; }

    public virtual DbSet<Store> Store { get; set; }

    public virtual DbSet<StoreCustomer> StoreCustomer { get; set; }

    public virtual DbSet<StoreCustomerAddress> StoreCustomerAddress { get; set; }

    public virtual DbSet<StoreCustomerOrder> StoreCustomerOrder { get; set; }

    public virtual DbSet<StoreCustomerOrderDetail> StoreCustomerOrderDetail { get; set; }

    public virtual DbSet<TattooStatus> TattooStatus { get; set; }

    public virtual DbSet<UserActivity> UserActivity { get; set; }

    public virtual DbSet<UserMessage> UserMessage { get; set; }

    public virtual DbSet<UserProfile> UserProfile { get; set; }

    public virtual DbSet<VideoCaptions> VideoCaptions { get; set; }

    public virtual DbSet<VideoDigitalMarketingDailyPosts> VideoDigitalMarketingDailyPosts { get; set; }

    public virtual DbSet<VideoDigitalMarketingPlan> VideoDigitalMarketingPlan { get; set; }

    public virtual DbSet<VideoIndexStatus> VideoIndexStatus { get; set; }

    public virtual DbSet<VideoIndexingCost> VideoIndexingCost { get; set; }

    public virtual DbSet<VideoIndexingMargin> VideoIndexingMargin { get; set; }

    public virtual DbSet<VideoIndexingTransaction> VideoIndexingTransaction { get; set; }

    public virtual DbSet<VideoInfo> VideoInfo { get; set; }

    public virtual DbSet<VideoJob> VideoJob { get; set; }

    public virtual DbSet<VideoJobApplicationStatus> VideoJobApplicationStatus { get; set; }

    public virtual DbSet<VideoVisibility> VideoVisibility { get; set; }

    public virtual DbSet<VwBalance> VwBalance { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUserVouch>(entity =>
        {
            entity.HasOne(d => d.FromApplicationUser).WithMany(p => p.ApplicationUserVouchFromApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationUserVouch_FromUser");

            entity.HasOne(d => d.ToApplicationUser).WithMany(p => p.ApplicationUserVouchToApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationUserVouch_ToUser");
        });

        modelBuilder.Entity<AspNetRoles>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<AspNetUsers>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.HasMany(d => d.Role).WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRoles",
                    r => r.HasOne<AspNetRoles>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUsers>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasOne(d => d.StateOrProvince).WithMany(p => p.City)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_StateOrProvince");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasOne(d => d.Currency).WithMany(p => p.Expense)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_Currency");

            entity.HasOne(d => d.MonthlyBudgetInfo).WithMany(p => p.Expense)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_MonthlyBudgetInfo");

            entity.HasOne(d => d.Owner).WithMany(p => p.Expense)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expense_AspNetUsers");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasOne(d => d.OwnerApplicationUser).WithMany(p => p.Group)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_ApplicationUser");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasOne(d => d.Currency).WithMany(p => p.Income)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Income_Currency");

            entity.HasOne(d => d.MonthlyBudgetInfo).WithMany(p => p.Income)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Income_MonthlyBudgetInfo");

            entity.HasOne(d => d.Owner).WithMany(p => p.Income)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Income_AspNetUsers");
        });

        modelBuilder.Entity<LikedUserProfile>(entity =>
        {
            entity.HasOne(d => d.LikedApplicationUser).WithMany(p => p.LikedUserProfileLikedApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedUserProfile_LikedApplicactionUser");

            entity.HasOne(d => d.LikingApplicationUser).WithMany(p => p.LikedUserProfileLikingApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedUserProfile_LikingApplicactionUser");
        });

        modelBuilder.Entity<MonthlyBudgetInfo>(entity =>
        {
            entity.HasOne(d => d.Owner).WithMany(p => p.MonthlyBudgetInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MonthlyBudgetInfo_AspNetUsers");
        });

        modelBuilder.Entity<NotLikedUserProfile>(entity =>
        {
            entity.HasOne(d => d.NotLikedApplicationUser).WithMany(p => p.NotLikedUserProfileNotLikedApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotLikedUserProfile_NotLikedApplicactionUser");

            entity.HasOne(d => d.NotLikingApplicationUser).WithMany(p => p.NotLikedUserProfileNotLikingApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotLikedUserProfile_NotLikingApplicactionUser");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("PostHistory", "FairPlaySocial");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.Property(e => e.PostTypeId).HasDefaultValueSql("1");
            entity.Property(e => e.PostVisibilityId).HasDefaultValueSql("1");

            entity.HasOne(d => d.CreatedFromPost).WithMany(p => p.InverseCreatedFromPost).HasConstraintName("FK_Post_Post");

            entity.HasOne(d => d.Group).WithMany(p => p.Post).HasConstraintName("FK_Post_Group");

            entity.HasOne(d => d.OwnerApplicationUser).WithMany(p => p.Post)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_ApplicationUser");

            entity.HasOne(d => d.Photo).WithMany(p => p.Post).HasConstraintName("FK_Post_Photo");

            entity.HasOne(d => d.PostType).WithMany(p => p.Post)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_PostType");

            entity.HasOne(d => d.PostVisibility).WithMany(p => p.Post)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_PostVisibility");

            entity.HasOne(d => d.RootPost).WithMany(p => p.InverseRootPost).HasConstraintName("FK_Post_Post_RootPost");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.Owner).WithMany(p => p.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_AspNetUsers");

            entity.HasOne(d => d.ProductStatus).WithMany(p => p.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductStatus");

            entity.HasOne(d => d.Store).WithMany(p => p.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Store");

            entity.HasOne(d => d.ThumbnailPhoto).WithMany(p => p.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Photo");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasOne(d => d.Culture).WithMany(p => p.Resource)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resource_Culture");
        });

        modelBuilder.Entity<StateOrProvince>(entity =>
        {
            entity.HasOne(d => d.Country).WithMany(p => p.StateOrProvince)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StateOrProvince_Country");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasOne(d => d.Owner).WithMany(p => p.Store)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Store_AspNetUsers");
        });

        modelBuilder.Entity<StoreCustomer>(entity =>
        {
            entity.HasOne(d => d.Store).WithMany(p => p.StoreCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreCustomer_Store");
        });

        modelBuilder.Entity<StoreCustomerAddress>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<StoreCustomerOrder>(entity =>
        {
            entity.HasOne(d => d.StoreCustomer).WithMany(p => p.StoreCustomerOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreCustomerOrder_StoreCustomer");
        });

        modelBuilder.Entity<StoreCustomerOrderDetail>(entity =>
        {
            entity.HasOne(d => d.Product).WithMany(p => p.StoreCustomerOrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreCustomerOrderDetail_Product");

            entity.HasOne(d => d.StoreCustomerOrder).WithMany(p => p.StoreCustomerOrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StoreCustomerOrderDetail_StoreCustomerOrder");
        });

        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.HasOne(d => d.Activity).WithMany(p => p.UserActivity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserActivity_Activity");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.UserActivity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserActivity_ApplicationUser");

            entity.HasOne(d => d.Frequency).WithMany(p => p.UserActivity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserActivity_Frequency");
        });

        modelBuilder.Entity<UserMessage>(entity =>
        {
            entity.HasOne(d => d.FromApplicationUser).WithMany(p => p.UserMessageFromApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FromApplicationUserId_ApplicationUser");

            entity.HasOne(d => d.ToApplicationUser).WithMany(p => p.UserMessageToApplicationUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ToApplicationUserId_ApplicationUser");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasOne(d => d.ApplicationUser).WithOne(p => p.UserProfile)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationUserId_UserProfile");

            entity.HasOne(d => d.BiologicalGender).WithMany(p => p.UserProfile)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_BiologicalGenderId");

            entity.HasOne(d => d.CurrentDateObjective).WithMany(p => p.UserProfile)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_CurrentDateObjectiveId");

            entity.HasOne(d => d.EyesColor).WithMany(p => p.UserProfileEyesColor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_EyesColor");

            entity.HasOne(d => d.HairColor).WithMany(p => p.UserProfileHairColor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_HairColor");

            entity.HasOne(d => d.KidStatus).WithMany(p => p.UserProfileKidStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_KidStatus");

            entity.HasOne(d => d.PreferredEyesColor).WithMany(p => p.UserProfilePreferredEyesColor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_PreferredEyesColor");

            entity.HasOne(d => d.PreferredHairColor).WithMany(p => p.UserProfilePreferredHairColor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_PreferredHairColor");

            entity.HasOne(d => d.PreferredKidStatus).WithMany(p => p.UserProfilePreferredKidStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_PreferredKidStatus");

            entity.HasOne(d => d.PreferredReligion).WithMany(p => p.UserProfilePreferredReligion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_PreferredReligionId");

            entity.HasOne(d => d.PreferredTattooStatus).WithMany(p => p.UserProfilePreferredTattooStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_PreferredTattooStatus");

            entity.HasOne(d => d.ProfilePhoto).WithMany(p => p.UserProfile)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_Photo");

            entity.HasOne(d => d.Religion).WithMany(p => p.UserProfileReligion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_ReligionId");

            entity.HasOne(d => d.TattooStatus).WithMany(p => p.UserProfileTattooStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserProfile_TattooStatus");
        });

        modelBuilder.Entity<VideoCaptions>(entity =>
        {
            entity.HasOne(d => d.VideoInfo).WithMany(p => p.VideoCaptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoCaptions_VideoInfo");
        });

        modelBuilder.Entity<VideoDigitalMarketingDailyPosts>(entity =>
        {
            entity.HasOne(d => d.VideoInfo).WithMany(p => p.VideoDigitalMarketingDailyPosts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoDigitalMarketingDailyPosts_VideoInfo");
        });

        modelBuilder.Entity<VideoDigitalMarketingPlan>(entity =>
        {
            entity.HasOne(d => d.VideoInfo).WithMany(p => p.VideoDigitalMarketingPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoDigitalMarketingPlan_VideoInfo");
        });

        modelBuilder.Entity<VideoIndexStatus>(entity =>
        {
            entity.Property(e => e.VideoIndexStatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<VideoIndexingTransaction>(entity =>
        {
            entity.HasOne(d => d.VideoInfo).WithMany(p => p.VideoIndexingTransaction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoIndexingTransaction_VideoInfo");
        });

        modelBuilder.Entity<VideoInfo>(entity =>
        {
            entity.Property(e => e.ApplicationUserId).HasComment("Video Owner Id");
            entity.Property(e => e.RowCreationDateTime).HasDefaultValueSql("getutcdate()");
            entity.Property(e => e.RowCreationUser).HasDefaultValueSql("'unknown'");
            entity.Property(e => e.SourceApplication).HasDefaultValueSql("'unknown'");
            entity.Property(e => e.VideoVisibilityId).HasDefaultValueSql("1");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.VideoInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoInfo_ApplicationUser");

            entity.HasOne(d => d.VideoIndexStatus).WithMany(p => p.VideoInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoInfo_VideoIndexStatus");

            entity.HasOne(d => d.VideoVisibility).WithMany(p => p.VideoInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoInfo_VideoVisibility");
        });

        modelBuilder.Entity<VideoJob>(entity =>
        {
            entity.HasOne(d => d.VideoInfo).WithMany(p => p.VideoJob)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VideoJob_VideoInfo");
        });

        modelBuilder.Entity<VideoJobApplicationStatus>(entity =>
        {
            entity.Property(e => e.VideoJobApplicationStatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<VideoVisibility>(entity =>
        {
            entity.Property(e => e.VideoVisibilityId).ValueGeneratedNever();
        });

        modelBuilder.Entity<VwBalance>(entity =>
        {
            entity.ToView("vwBalance", "FairPlayBudget");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}