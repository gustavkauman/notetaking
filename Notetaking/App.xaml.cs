using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Notetaking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static NoteController NoteController { get; set; }
        internal static MainWindow MainWindow { get; set; }
        internal static NotetakingContext DBContext { get; set; }

        private void AppStartup(object sender, StartupEventArgs args)
        {
            NoteController = NoteController.GetInstance();
            DBContext = new NotetakingContext();
            MainWindow = new MainWindow();
            NoteController.AddAllNotes(DBContext.Notes.ToList());
            MainWindow.Show();
        }
    }
}
