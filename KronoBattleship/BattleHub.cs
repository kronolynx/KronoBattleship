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