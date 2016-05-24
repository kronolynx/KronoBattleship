using KronoBattleship.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoBattleship.Datalayer
{
    class BattleConfiguration : EntityTypeConfiguration<Battle>
    {
        public BattleConfiguration()
        {
            //Property(n => n.EnemyId)
            //    .IsRequired();
            //Property(n => n.PlayerId)
            //    .IsRequired();
            Property(n => n.PlayerBoard)
                .HasMaxLength(100);
            Property(n => n.EnemyBoard)
                .HasMaxLength(100);
            Property(p => p.RowVersion)
                .IsConcurrencyToken();

            //HasMany(n => n.Messages)
            //    .WithOptional()
            //    .HasForeignKey(n => n.BattleId)
            //    .WillCascadeOnDelete(true);

            //HasKey(n => new { n.PlayerId, n.EnemyId});
        }
    }
}
