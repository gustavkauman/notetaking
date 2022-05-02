using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System;

namespace Notetaking
{
    internal class NoteController
    {
        public static NoteController Instance;

        private NoteController()
        {
        }

        public static NoteController GetInstance()
        {
            if (Instance == null)
                Instance = new NoteController();

            return Instance;
        }

        public void AddAllNotes(IList<Note> notes)
        {
            App.MainWindow.SetNotes(new ObservableCollection<Note>(notes));
        }

        public void AddNote(string title, string body)
        {
            var db = App.DBContext;
            var note = new Note() { Title = title, Body = body };
            db.Notes.Add(note);
            db.SaveChanges();
            App.MainWindow.RefreshNotes();
        }

        public void UpdateNote(Note note)
        {
            if (note.NoteId <= 0)
            {
                // Method was incorrectly called
                AddNote(note.Title, note.Body);
            }

            var db = App.DBContext;
            var dbNote = db.Notes.Find(note.NoteId);
            if (dbNote == null)
            {
                return;
            }

            db.Entry(dbNote).CurrentValues.SetValues(note);
            db.SaveChanges();

            App.MainWindow.RefreshNotes();
        }

        public void DeleteNote(Note note)
        {
            if (note.NoteId <= 0)
            {
                // Method was incorrectly called
                return;
            }

            var db = App.DBContext;
            db.Entry(note).State = EntityState.Deleted;
            db.SaveChanges();
            App.MainWindow.RefreshNotes();
            App.MainWindow.ResetContent();
        }
    }

    public class Note
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
