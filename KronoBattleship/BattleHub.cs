using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace KronoBattleship
{
    public class BattleHub : Hub
    {
        public void Send(string user, string battleId, string message)
        {
            Clients.Group(battleId).broadcastMessage(user, message);
        }

        //TODO check if the callback can be sent from javascript
        public void CallFunction(string enemyName, string functionName)
        {
            Clients.Group(enemyName).callFunction(functionName);
        }

        public void Attack(string enemyName, string functionName, int attack, bool over)
        {
            Clients.Group(enemyName).attack(functionName, attack, over);
        }

        public void FinishGame(string enemyName, bool winner)
        {
            Clients.Group(enemyName).gameOver(winner);
        }

        public Task Join(string battleId)
        {
            return Groups.Add(Context.ConnectionId, battleId);
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;         
            Groups.Add(Context.ConnectionId, name);
            return base.OnConnected();
        }
    }
}