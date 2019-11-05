using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monster.API.Models
{
    public class MonsterContext : DbContext
    {
        public DbSet<MonsterModel> Monsters { get; set; }

        public MonsterContext(DbContextOptions<MonsterContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MonsterModel>().ToTable("Monsters");

            builder.Entity<MonsterModel>().HasIndex(monster => monster.MonsterId).IsUnique();

            builder.Entity<MonsterModel>().Property(monster => monster.MonsterId).ValueGeneratedOnAdd();
            builder.Entity<MonsterModel>().Property(monster => monster.Type).HasDefaultValue(1);
            builder.Entity<MonsterModel>().Property(monster => monster.MaxHP).HasDefaultValue(1);
            builder.Entity<MonsterModel>().Property(monster => monster.CurrentHP).HasDefaultValue(1);
        }
    }
}
