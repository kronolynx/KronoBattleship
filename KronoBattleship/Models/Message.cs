using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoBattleship.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Body { get; set; }
        public virtual User Player { get; set; }
        public virtual Battle Battle { get; set; }
        public DateTime DateSent { get; set; }
    }
}
