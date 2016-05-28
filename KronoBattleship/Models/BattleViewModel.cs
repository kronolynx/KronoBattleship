using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KronoBattleship.Models
{
    public class BattleViewModel
    {
        public BattleViewModel(Battle battle, string currentUsername)
        {
            if (battle.PlayerName.Equals(currentUsername))
            {
                PlayerName = battle.PlayerName;
                EnemyName = battle.EnemyName;
                PlayerBoard = battle.PlayerBoard;
                EnemyBoard = battle.EnemyBoard;
                PlayerPicture = battle.Player.Picture;
                EnemyPicture = battle.Enemy.Picture;
                
            }else
            {
                PlayerName = battle.EnemyName;
                EnemyName = battle.PlayerName;
                PlayerBoard = battle.EnemyBoard;
                EnemyBoard = battle.PlayerBoard;
                PlayerPicture = battle.Enemy.Picture;
                EnemyPicture = battle.Player.Picture;
            }
            BattleId = battle.BattleId;
            ActivePlayer = battle.ActivePlayer;
        }
        public int BattleId { get; set; }
        public string PlayerName { get; set; }
        public string EnemyName { get; set; }
        public string PlayerBoard { get; set; }
        public string EnemyBoard { get; set; }
        public string ActivePlayer { get; set; }
        public string PlayerPicture { get; set; }
        public string EnemyPicture { get; set; }
    }
}