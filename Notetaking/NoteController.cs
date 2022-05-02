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
        private ObservableCollection<Note> Notes;

        private NoteController()
        {
            Notes = new ObservableCollection<Note>();
        }

        public static NoteController GetInstance()
        {
            if (Instance == null)
                Instance = new NoteController();

            return Instance;
        }

        public void AddAllNotes(IList<Note> notes)
        {
            foreach (var note in notes)
            {
                Notes.Add(note);
                Trace.WriteLine($"Added note with id: {note.NoteId}");
            }

            App.MainWindow.SetNotes(Notes);
        }

        public void AddNote(string title, string body)
        {
            using (var db = App.DBContext)
            {
                var note = new Note() { Title = title, Body = body };
                db.Notes.Add(note);
                db.SaveChanges();

                Notes.Add(note);
                App.MainWindow.SetNotes(Notes);
                Trace.WriteLine($"Total number of notes: {Notes.Count}");
            }
        }
    }

    public class Note
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
