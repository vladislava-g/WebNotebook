using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotebook.Models
{
    public class Note
    {
        public int Id { set; get; }
        public int UserId { set; get; }
        public int NotebookId { set; get; }
        public int TypeId { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        public DateTime Created { set; get; }
        public DateTime Modified { set; get; }
        public string Hex { set; get; }
    }
}
