using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApplication.Enums;

namespace SocialApplication.Entities
{
    internal class Reaction
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
        public ReactionType Type { get; set; }
    }
}
