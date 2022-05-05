using Notetaking.Models;
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

            UpdateLists();
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
            MarkDirty();
        }

        private void NoteTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            MarkDirty();
        }

        private void SaveRelationsBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Note relatedNote in PossibleRelatedNotesListBox.SelectedItems)
            {
                App.NoteController.AddRelation(ObjectNote.NoteId, relatedNote.NoteId);
            }

            UpdateLists();
            MarkClean();
        }

        private void PossibleRelatedNotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MarkDirty();
        }

        private void RelatedNotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MarkDirty();
        }

        private void RemoveRelationsBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (RelatedNotesListItem item in RelatedNotesListBox.Items)
            {
                if (item.IsSelected)
                {
                    App.NoteController.RemoveRelation(ObjectNote.NoteId, item.NoteId);
                }
            }

            UpdateLists();
            MarkClean();
        }

        #region Aux
        private void UpdateLists()
        {
            var db = App.DBContext;
            var possibleNotes = db.Notes.Where(n => n.NoteId != ObjectNote.NoteId).ToDictionary(note => note.NoteId, note => note);
            var query = from noteRelation in db.Set<NoteRelation>().Where(nr => nr.FromNoteId == ObjectNote.NoteId)
                        join dbNote in db.Set<Note>()
                            on noteRelation.ToNoteId equals dbNote.NoteId
                        select new { noteRelation, dbNote };
            var relatedNotesResult = query.ToList();
            var relatedNotesList = new List<RelatedNotesListItem>();

            foreach (var relatedNote in relatedNotesResult)
            {
                if (possibleNotes.ContainsKey(relatedNote.noteRelation.ToNoteId))
                {
                    possibleNotes.Remove(relatedNote.noteRelation.ToNoteId);
                }
                relatedNotesList.Add(new RelatedNotesListItem() { NoteId = relatedNote.noteRelation.ToNoteId, Text = relatedNote.dbNote.Title });
            }

            PossibleRelatedNotesListBox.ItemsSource = possibleNotes.Values.ToList();
            RelatedNotesListBox.ItemsSource = relatedNotesList;
        }

        private void MarkDirty()
        {
            this.IsDirty = true;
            NoteSavedText.Visibility = Visibility.Hidden;
        }

        private void MarkClean()
        {
            this.IsDirty = false;
            NoteSavedText.Visibility = Visibility.Visible;
        }

        #endregion
    }

    public class RelatedNotesListItem
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
