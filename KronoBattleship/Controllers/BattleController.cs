using KronoBattleship.Datalayer;
using KronoBattleship.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KronoBattleship.Controllers
{
    [System.Web.Mvc.Authorize]
    public class BattleController : Controller
    {
        // GET: Battle/id
        public ActionResult Index(int battleId)
        {
            var db = new ApplicationDbContext();
            Battle battle = db.Battles.Find(battleId);
            var currentUserName = getCurrentUserName();
            if (battle != null && (currentUserName.Equals(battle.PlayerName) || currentUserName.Equals(battle.EnemyName)))
            {
                return View(new BattleViewModel(battle, currentUserName));
            }
            return Redirect("/Chat/Index");
        }

        // POST: Battle/Create
        [HttpPost]
        public ActionResult Create(string user1, string user2)
        {

            string playerName, enemyName;

            getPlayers(user1, user2, out playerName, out enemyName);
            var db = new ApplicationDbContext();
            Battle battle = db.Battles.Where(b => b.PlayerName.Equals(playerName) && b.EnemyName.Equals(enemyName)).FirstOrDefault();
            if (battle == null)
            {
                User player = db.Users.Where(n => n.UserName.Equals(playerName)).First();
                User enemy = db.Users.Where(n => n.UserName.Equals(enemyName)).First();
                battle = new Battle(player, enemy);
                db.Battles.Add(battle);
                db.SaveChanges();
            }
            var context = getContext();
            context.Clients.Group(getEnemyName(battle)).answer(User.Identity.Name, battle.BattleId);
            return RedirectToAction("Index", new { battleId = battle.BattleId });
        }


        // POST: Battle/Attack/5
        [HttpPost]
        public ActionResult Attack(int battleId, int attack)
        {
            bool hit;
            bool gameOver = false;
            string boardAfterAttack;
            var playerName = getCurrentUserName();
            var db = new ApplicationDbContext();
            Battle battle = db.Battles.Find(battleId);
            if (playerName.Equals(battle.PlayerName))
            {
                boardAfterAttack = battle.EnemyBoard;
                shipHit(attack, out hit, ref boardAfterAttack);
                battle.EnemyBoard = boardAfterAttack;
            }
            else
            {
                boardAfterAttack = battle.PlayerBoard;
                shipHit(attack, out hit, ref boardAfterAttack);
                battle.PlayerBoard = boardAfterAttack;
            }

            if (isTheGameOver(boardAfterAttack))
            {
                gameOver = true;
                battle.PlayerBoard = "";
                battle.EnemyBoard = "";
                battle.ActivePlayer = "";
                if (battle.PlayerName.Equals(playerName))
                {
                    battle.Player.Wins++;
                    battle.Enemy.Losses++;
                }
                else
                {
                    battle.Player.Losses++;
                    battle.Enemy.Wins++;
                }
            }
            else
            {
                battle.ActivePlayer = getEnemyName(battle);
            }
            db.SaveChanges();
            return Json(new { Hit = hit, GameOver = gameOver });

        }


        // GET: Battle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public void GameOver(int battleId)
        {
            var db = new ApplicationDbContext();
            Battle battle = db.Battles.Find(battleId);
            var currentUserName = getCurrentUserName();

            battle.PlayerBoard = "";
            battle.EnemyBoard = "";
            battle.ActivePlayer = "";
            if (battle.PlayerName.Equals(currentUserName))
            {
                battle.Player.Losses++;
                battle.Enemy.Wins++;
            }
            else
            {
                battle.Player.Wins++;
                battle.Enemy.Losses++;
            }
            db.SaveChanges();
        }


        [HttpPost]
        public ActionResult Ready(int battleId, string playerBoard)
        {
            var db = new ApplicationDbContext();
            Battle battle = db.Battles.Find(battleId);
            var currentUserName = getCurrentUserName();
            string enemyBoard;
            if (currentUserName.Equals(battle.PlayerName))
            {
                battle.PlayerBoard = playerBoard;
                enemyBoard = battle.EnemyBoard;
                if (battle.EnemyBoard == "")
                    battle.ActivePlayer = battle.PlayerName;
            }
            else
            {
                battle.EnemyBoard = playerBoard;
                enemyBoard = battle.PlayerBoard;
                if (battle.PlayerBoard == "")
                    battle.ActivePlayer = battle.EnemyName;
            }
            db.SaveChanges();
            return Json(new { EnemyBoard = enemyBoard });
        }


        private IHubContext getContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ConnectionHub>();
        }
        private string getCurrentUserName()
        {
            return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId()).UserName;
        }
        private string getEnemyName(Battle battle)
        {
            return battle.PlayerName.Equals(User.Identity.Name) ? battle.EnemyName : battle.PlayerName;
        }

        // Helpers
        private void getPlayers(string user1, string user2, out string player, out string enemy)
        {
            if (user1.CompareTo(user2) < 0)
            {
                player = user1;
                enemy = user2;
            }
            else
            {
                player = user2;
                enemy = user1;
            }
        }

        private void shipHit(int attack, out bool hit, ref string enemyBoard)
        {
            // if the enemyboard contains any letter that indicates is a ship
            hit = "acegikmoqs".Contains(enemyBoard[attack]);
            char[] tempBoard = enemyBoard.ToCharArray();
            // increase the letter at the possition of the attack
            tempBoard[attack] = (char)(enemyBoard[attack] + 1);
            enemyBoard = new string(tempBoard);
        }

        private bool isTheGameOver(string enemyboard)
        {
            // check if all the ships have been hit
            return enemyboard.FirstOrDefault(c => c != 'y' && c != 'z' && ("acegikmoqs".Contains(c)))
                             .Equals('\0');
        }
    }
}
