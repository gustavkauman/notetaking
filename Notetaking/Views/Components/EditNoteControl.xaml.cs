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
            MarkClean();
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
            foreach (Note note in RelatedNotesListBox.SelectedItems)
            {
                App.NoteController.RemoveRelation(ObjectNote.NoteId, note.NoteId);
            }

            UpdateLists();
            MarkClean();
        }

        #region Aux
        private void UpdateLists()
        {
            PossibleRelatedNotesListBox.ItemsSource = App.NoteController.GetPossibleRelatedNotes(ObjectNote);
            RelatedNotesListBox.ItemsSource = App.NoteController.GetRelatedNotes(ObjectNote);
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
}
