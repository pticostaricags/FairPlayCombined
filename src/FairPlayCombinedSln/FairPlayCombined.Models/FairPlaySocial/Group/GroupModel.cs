using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlaySocial.Group
{
    public class GroupModel : IListModel
    {
        public long GroupId { get; set; }
        public string? OwnerApplicationUserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? TopicTag { get; set; }
    }
}
