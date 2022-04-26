using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

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

        public void AddNote(string title, string body)
        {
            Notes.Add(new Note() { Title = title, Body = body});
            App.MainWindow.SetNotes(Notes);
            Trace.WriteLine($"Total number of notes: {Notes.Count}");
        }
    }

    public class Note
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
