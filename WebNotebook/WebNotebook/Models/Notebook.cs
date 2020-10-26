using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotebook.Models
{
    public class Notebook
    {
        public int Id { set; get; }
        public int CreatorId { set; get; }
        public string Name { set; get; }
        public string Cover { set; get; }
        public DateTime Created { set; get; }
        public DateTime Modified { set; get; }
        public int IsDefault { set; get; }
        public int? DefaultImage { set; get; }
    }
}
