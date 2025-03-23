using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApplication.Entities
{
    internal class Comment
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("UserId")]
        public long UserId { get; set; }

        [ForeignKey("PostId")]
        public long PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
