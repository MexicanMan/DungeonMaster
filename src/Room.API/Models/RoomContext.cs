using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Room.API.Models
{
    public class RoomContext : DbContext
    {
        public DbSet<RoomModel> Rooms { get; set; }

        public RoomContext(DbContextOptions<RoomContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RoomModel>().ToTable("Rooms");

            builder.Entity<RoomModel>().HasIndex(room => room.RoomId).IsUnique();

            builder.Entity<RoomModel>().Property(room => room.RoomId).ValueGeneratedOnAdd();
        }
    }
}
