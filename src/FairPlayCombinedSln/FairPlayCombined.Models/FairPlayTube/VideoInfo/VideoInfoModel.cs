﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayTube.VideoInfo
{
    public class VideoInfoModel : IListModel
    {
        public long VideoInfoId { get; set; }
        public Guid AccountId { get; set; }
        public string? VideoId { get; set; }
        public string? Location { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public string? VideoBloblUrl { get; set; }
        public string? IndexedVideoUrl { get; set; }
        public string? ApplicationUserId { get; set; }
        public short VideoIndexStatusId { get; set; }
        public double VideoDurationInSeconds { get; set; }
        public string? VideoIndexSourceClass { get; set; }
        public decimal Price { get; set; }
        public string? ExternalVideoSourceUrl { get; set; }
        public string? VideoLanguageCode { get; set; }
        public short VideoVisibilityId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? YouTubeVideoId { get; set; }
    }
}
