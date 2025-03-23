using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Entities
{
    public class Group
    {
        [Key]
        public long Id { get; set; }
        public required string Name { get; set; }
        public long AdminId { get; set; }
    }
}
