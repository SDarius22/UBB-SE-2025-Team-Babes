using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Entities
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("UserId")]
        public long UserId { get; set; }
        //FK
        public long PostId { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
