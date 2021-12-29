namespace NEWS_MVC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NEWS : DbContext
    {
        public NEWS()
            : base("name=NEWS3")
        {
        }

        public virtual DbSet<article> article { get; set; }
        public virtual DbSet<user> user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<article>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<article>()
                .Property(e => e.type)
                .IsFixedLength();

            modelBuilder.Entity<article>()
                .Property(e => e.fengmian)
                .IsFixedLength();

            modelBuilder.Entity<article>()
                .Property(e => e.username)
                .IsFixedLength();

            modelBuilder.Entity<user>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsFixedLength();

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsFixedLength();
        }
    }
}
