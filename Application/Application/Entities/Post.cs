using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApplication.Enums;

namespace SocialApplication.Entities
{
    internal class Post
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserId")]
        public long UserId { get; set; }

        [ForeignKey("GroupId")]
        public long GroupId { get; set; }

        public PostVisibility Visibility { get; set; }

        public PostTag Tag { get; set; }
    }
}
