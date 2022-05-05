using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notetaking.Models;

namespace Notetaking
{
    public class NotetakingContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteRelation> NoteRelation { get; set; }
        public string DBPath { get; }

        public NotetakingContext()
        {
            var appData = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notetaking");
            Directory.CreateDirectory(appData);
            DBPath = Path.Join(appData, "notetaking.db");

            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DBPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NoteRelation>()
                .HasKey(nr => new { nr.FromNoteId, nr.ToNoteId });

            modelBuilder.Entity<NoteRelation>(model =>
            {
                model.HasKey(nr => new { nr.FromNoteId, nr.ToNoteId });
                model.HasOne<Note>()
                    .WithMany()
                    .HasForeignKey(nr => nr.FromNoteId);

                model.HasOne<Note>()
                    .WithMany()
                    .HasForeignKey(nr => nr.ToNoteId);
            });
        }
    }
}
