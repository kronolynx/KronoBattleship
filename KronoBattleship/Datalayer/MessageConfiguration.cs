using KronoBattleship.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoBattleship.Datalayer
{
    class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            //Property(n => n.BattleId)
            //    .IsRequired();
            //Property(n => n.PlayerId)
            //    .IsRequired();


        }
    }
}
