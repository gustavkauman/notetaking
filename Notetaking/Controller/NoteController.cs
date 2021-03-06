using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System;
using Notetaking.Exceptions;
using Notetaking.Models;
using System.Linq;

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
                throw new ArgumentException($"The note id of {nameof(note)} must be greater than or equal to 0");
            }

            var db = App.DBContext;
            var dbNote = db.Notes.Find(note.NoteId);
            if (dbNote == null)
            {
                throw new DatabaseRecordMissingException("Cannot update note because note not found in database!");
            }

            db.Entry(dbNote).CurrentValues.SetValues(note);
            db.SaveChanges();

            App.MainWindow.RefreshNotes();
        }

        public void DeleteNote(Note note)
        {
            if (note.NoteId <= 0)
            {
                throw new ArgumentException($"The note id of {nameof(note)} cannot be less than or equal to 0");
            }

            var db = App.DBContext;
            db.Entry(note).State = EntityState.Deleted;
            db.SaveChanges();
            App.MainWindow.RefreshNotes();
            App.MainWindow.ResetContent();
        }

        public void AddRelation(int FromNoteId, int ToNoteId)
        {
            var db = App.DBContext;
            var exists = db.NoteRelation.Where(nr => nr.FromNoteId == FromNoteId && nr.ToNoteId == ToNoteId).ToList().Count > 0;
            if (exists)
                return;

            // Clear change tracker since it has tracked the entity during read
            db.ChangeTracker.Clear();

            db.NoteRelation.Add(new NoteRelation() { FromNoteId = FromNoteId, ToNoteId = ToNoteId });
            db.SaveChanges();
        }

        public void RemoveRelation(int FromNoteId, int ToNoteId)
        {
            var db = App.DBContext;
            var missing = db.NoteRelation.Where(nr => nr.FromNoteId == FromNoteId && nr.ToNoteId == ToNoteId).ToList().Count <= 0;
            if (missing)
                return;

            // Clear change tracker since it has tracked the entity during read
            db.ChangeTracker.Clear();

            db.NoteRelation.Remove(new NoteRelation() { FromNoteId = FromNoteId, ToNoteId = ToNoteId });
            db.SaveChanges();
        }

        public IList<Note> GetPossibleRelatedNotes(Note note)
        {
            var db = App.DBContext;
            var possibleNotes = db.Notes.Where(n => n.NoteId != note.NoteId).ToDictionary(note => note.NoteId, note => note);
            var query = from noteRelation in db.Set<NoteRelation>().Where(nr => nr.FromNoteId == note.NoteId)
                        join dbNote in db.Set<Note>()
                            on noteRelation.ToNoteId equals dbNote.NoteId
                        select new { noteRelation, dbNote };
            var relatedNotesResult = query.ToList();

            foreach (var relatedNote in relatedNotesResult)
            {
                if (possibleNotes.ContainsKey(relatedNote.noteRelation.ToNoteId))
                {
                    possibleNotes.Remove(relatedNote.noteRelation.ToNoteId);
                }
            }

            return possibleNotes.Values.ToList();
        }

        public IList<Note> GetRelatedNotes(Note note)
        {
            var db = App.DBContext;
            var result = new List<Note>();
            var relations = db.NoteRelation.Where(nr => nr.FromNoteId == note.NoteId).ToList();
            foreach (var rel in relations)
            {
                result.Add(db.Notes.Find(rel.ToNoteId));
            }
            return result;
        }
    }
}
