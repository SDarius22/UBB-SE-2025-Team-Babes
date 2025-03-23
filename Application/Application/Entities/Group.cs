using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApplication.Entities
{
    internal class Group
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long AdminId { get; set; }
    }
}
