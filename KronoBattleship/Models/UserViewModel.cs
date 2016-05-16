using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KronoBattleship.Models
{
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Wins = user.Wins;
            Losses = user.Losses;
            Picture = user.Picture;

        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Picture { get; set; }

        public static List<UserViewModel> GetList(List<User> list)
        {
            return list.Select(u => new UserViewModel(u)).ToList();
        }
    }
}