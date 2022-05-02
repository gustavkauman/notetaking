using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notetaking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool WasForced { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public static void ShowMessage(string Message)
        {
            MessageBox.Show(Message);
        }

        public void SetNotes(ObservableCollection<Note> Notes)
        {
            NotesList.ItemsSource = Notes;
        }

        public void RefreshNotes()
        {
            var db = App.DBContext;
            this.SetNotes(new ObservableCollection<Note>(db.Notes.ToList()));
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var content = ContentControl.Content;
            if (content != null && 
                content is EditNoteControl && 
                e.RemovedItems.Count > 0 && 
                !WasForced)
            {
                // ContentControl is of type EditNoteControl AND we actually moved from one value to another
                if ((content as EditNoteControl).IsDirty)
                {
                    if (MessageBox.Show("Changing view will loose unsaved progress. Are you sure?", "Confirmation Dialog", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    {
                        WasForced = true;
                        NotesList.SelectedItem = e.RemovedItems[0];
                        return;
                    }
                }
            }

            if (WasForced)
            {
                WasForced = false;
                return;
            }

            var db = App.DBContext;
            var id = ((Note)NotesList.SelectedValue).NoteId;
            var dbNote = db.Notes.Find(id);
            if (dbNote == null)
            {
                return;
            }

            var control = new EditNoteControl();
            control.SetNote(dbNote);
            ContentControl.Content = control;
        }

        private void CreateNewNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new CreateNewNoteControl();
        }
    }
}
