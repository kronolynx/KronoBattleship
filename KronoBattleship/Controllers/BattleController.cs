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
            var currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if(currentUser.UserName.Equals(battle.PlayerName) || currentUser.UserName.Equals(battle.EnemyName))
            {
                ViewBag.CurrentUser = currentUser;
                ViewBag.Enemy = battle.PlayerName.Equals(ViewBag.CurrentUser.UserName) ? battle.Enemy : battle.Player;
                return View(battle);
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
            if(battle == null)
            {
                User player = db.Users.Where(n => n.UserName.Equals(playerName)).First();
                User enemy = db.Users.Where(n => n.UserName.Equals(enemyName)).First();
                battle = new Battle(player, enemy);
                db.Battles.Add(battle);
                db.SaveChanges();
            }
            var context = GlobalHost.ConnectionManager.GetHubContext<ConnectionHub>();
            context.Clients.Group(getEnemyName(battle)).answer(User.Identity.Name, battle.BattleId);
            //context.Clients.All.test("hello");
            return RedirectToAction("Index", new { battleId = battle.BattleId});      
        }

        // GET: Battle/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Battle/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Battle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Battle/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private string getEnemyName(Battle battle)
        {
             return battle.PlayerName.Equals(User.Identity.Name) ? battle.EnemyName : battle.PlayerName ;
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
    }
}
