using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zadanie_rekrutacyjne.Database.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Display(Name = "Nazwa:")]
        public String Name { get; set; }
        public Category Parent { get; set; }
        public ICollection<Category> CategoryChildren { get; set; }
    }
}
