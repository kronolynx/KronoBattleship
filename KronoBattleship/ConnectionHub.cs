using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using KronoBattleship.Models;
using System.Threading.Tasks;
using KronoBattleship.Datalayer;

namespace KronoBattleship
{
    public class ConnectionHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        //private ApplicationDbContext db = new ApplicationDbContext();


        public void requestPrompt(string text)
        {
            Clients.Others.testPrompt(text);
        }

        public void answerPrompt(string answer)
        {
            Clients.Others.answer(answer);
        }

        public override Task OnConnected()
        {
            // this will only work if the user is logged in
            string name = Context.User.Identity.Name;
            //var user = db.Users.First(u => u.UserName == name);
            //user.Online = "online";
            //db.SaveChanges();
            _connections.Add(name, Context.ConnectionId);
            Clients.Others.login(name);
            Clients.Caller.displayOnline(_connections.UsersOnline());
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            //var user = db.Users.First(u => u.UserName == name);
            //user.Online = "offline";
            //db.SaveChanges();
            _connections.Remove(name, Context.ConnectionId);
            Clients.Others.logout(name);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }
            Clients.Others.login(name + " re");
            return base.OnReconnected();
        }
    }
}