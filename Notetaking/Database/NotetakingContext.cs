using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notetaking
{
    public class NotetakingContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public string DBPath { get; }

        public NotetakingContext()
        {
            var appData = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notetaking");
            Directory.CreateDirectory(appData);
            DBPath = Path.Join(appData, "notetaking.db");

            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DBPath}");
    }
}
