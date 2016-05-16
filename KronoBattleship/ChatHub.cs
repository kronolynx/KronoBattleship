using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace KronoBattleship
{
    public class ChatHub : Hub
    {
        public void Send(string userName, string message)
        {
            Clients.All.broadcastMessage(userName, message);
        }
    }
}