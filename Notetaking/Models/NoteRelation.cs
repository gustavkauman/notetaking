using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notetaking.Models
{
    public class NoteRelation
    {
        public int FromNoteId { get; set; }
        public int ToNoteId { get; set; }

        public NoteRelation()
        {
        }

        public NoteRelation(Note FromNote, Note ToNote)
        {
            this.FromNoteId = FromNote.NoteId;
            this.ToNoteId = ToNote.NoteId;
        }
    }
}
