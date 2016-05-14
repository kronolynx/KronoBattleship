using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoBattleship.Models
{
    public class Battle
    {
        public int BattleId { get; set; }
        public virtual User Player { get; set; }
        public int PlayerId { get; set; }
        public virtual User Enemy { get; set; }
        public int EnemyId { get; set; }
        public string PlayerBoard { get; set; }
        public string EnemyBoard { get; set; }
        public int ActivePlayer { get; set; }
        // Child 
        public virtual ICollection<Message> Messages { get; set; }
    }
}
