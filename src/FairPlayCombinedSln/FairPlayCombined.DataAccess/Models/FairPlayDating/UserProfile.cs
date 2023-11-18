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

[Table("UserProfile", Schema = "FairPlayDating")]
[Index("ApplicationUserId", Name = "UI_UserProfile_ApplicationUserId", IsUnique = true)]
public partial class UserProfile
{
    [Key]
    public long UserProfileId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    [Required]
    [StringLength(100)]
    public string About { get; set; }

    public short HairColorId { get; set; }

    public short PreferredHairColorId { get; set; }

    public short EyesColorId { get; set; }

    public short PreferredEyesColorId { get; set; }

    public short BiologicalGenderId { get; set; }

    public short CurrentDateObjectiveId { get; set; }

    public short ReligionId { get; set; }

    public short PreferredReligionId { get; set; }

    public double CurrentLatitude { get; set; }

    public double CurrentLongitude { get; set; }

    public long ProfilePhotoId { get; set; }

    public short KidStatusId { get; set; }

    public short PreferredKidStatusId { get; set; }

    public short TattooStatusId { get; set; }

    public short PreferredTattooStatusId { get; set; }

    public DateTimeOffset BirthDate { get; set; }

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("UserProfile")]
    public virtual AspNetUsers ApplicationUser { get; set; }

    [ForeignKey("BiologicalGenderId")]
    [InverseProperty("UserProfile")]
    public virtual Gender BiologicalGender { get; set; }

    [ForeignKey("CurrentDateObjectiveId")]
    [InverseProperty("UserProfile")]
    public virtual DateObjective CurrentDateObjective { get; set; }

    [ForeignKey("EyesColorId")]
    [InverseProperty("UserProfileEyesColor")]
    public virtual EyesColor EyesColor { get; set; }

    [ForeignKey("HairColorId")]
    [InverseProperty("UserProfileHairColor")]
    public virtual HairColor HairColor { get; set; }

    [ForeignKey("KidStatusId")]
    [InverseProperty("UserProfileKidStatus")]
    public virtual KidStatus KidStatus { get; set; }

    [ForeignKey("PreferredEyesColorId")]
    [InverseProperty("UserProfilePreferredEyesColor")]
    public virtual EyesColor PreferredEyesColor { get; set; }

    [ForeignKey("PreferredHairColorId")]
    [InverseProperty("UserProfilePreferredHairColor")]
    public virtual HairColor PreferredHairColor { get; set; }

    [ForeignKey("PreferredKidStatusId")]
    [InverseProperty("UserProfilePreferredKidStatus")]
    public virtual KidStatus PreferredKidStatus { get; set; }

    [ForeignKey("PreferredReligionId")]
    [InverseProperty("UserProfilePreferredReligion")]
    public virtual Religion PreferredReligion { get; set; }

    [ForeignKey("PreferredTattooStatusId")]
    [InverseProperty("UserProfilePreferredTattooStatus")]
    public virtual TattooStatus PreferredTattooStatus { get; set; }

    [ForeignKey("ProfilePhotoId")]
    [InverseProperty("UserProfile")]
    public virtual Photo ProfilePhoto { get; set; }

    [ForeignKey("ReligionId")]
    [InverseProperty("UserProfileReligion")]
    public virtual Religion Religion { get; set; }

    [ForeignKey("TattooStatusId")]
    [InverseProperty("UserProfileTattooStatus")]
    public virtual TattooStatus TattooStatus { get; set; }
}