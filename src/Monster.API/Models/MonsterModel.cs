using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Models
{
    public class MonsterModel
    {
        [Key]
        public int MonsterId { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Type { get; set; }
    }
}
