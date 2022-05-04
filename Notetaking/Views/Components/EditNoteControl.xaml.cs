using System;
using System.Collections.Generic;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EditNoteControl : UserControl
    {
        public Note ObjectNote { get; private set; }
        public bool IsDirty { get; private set; }

        public EditNoteControl()
        {
            InitializeComponent();
        }

        public void SetNote(Note note)
        {
            ObjectNote = note;
            NoteTitle.Text = ObjectNote.Title;
            Note.Text = ObjectNote.Body;
            IsDirty = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button) e.Source).Name)
            {
                case "SaveNoteBtn":
                    HandleSaveBtnClicked();
                    break;
                case "DeleteNoteBtn":
                    HandleDeleteBtnClicked();
                    break;
            }
        }

        private void HandleSaveBtnClicked()
        {
            ObjectNote.Title = NoteTitle.Text;
            ObjectNote.Body = Note.Text;
            App.NoteController.UpdateNote(ObjectNote);
            IsDirty = false;
            NoteSavedText.Visibility = Visibility.Visible;
        }

        private void HandleDeleteBtnClicked()
        {
            App.NoteController.DeleteNote(ObjectNote);
        }

        private void Note_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.IsDirty = true;
            NoteSavedText.Visibility = Visibility.Hidden;
        }

        private void NoteTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.IsDirty = true;
            NoteSavedText.Visibility = Visibility.Hidden;
        }
    }
}
