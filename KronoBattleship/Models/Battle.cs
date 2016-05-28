using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoBattleship.Models
{
    public class Battle
    {
        public Battle() { }
        public Battle(User player, User enemy)
        {
            if (player.UserName.CompareTo(enemy.UserName) < 0)
            {
                Player = player;
                Enemy = enemy;
            } else
            {
                Player = enemy;
                Enemy = player;
            }
            PlayerName = Player.UserName;
            EnemyName = Enemy.UserName;
            //PlayerBoard = new String('x', 100);
            PlayerBoard = EnemyBoard = "";
        }
        public int BattleId { get; set; }

        public string PlayerName { get; set; }
        public virtual User Player { get; set; }
        public string EnemyName { get; set; }
        public virtual User Enemy { get; set; }
        public string PlayerBoard { get; set; }
        public string EnemyBoard { get; set; }
        public string ActivePlayer { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
