using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System;
using Notetaking.Models;

namespace Notetaking
{
    public class Note
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
