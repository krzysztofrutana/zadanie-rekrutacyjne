using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using zadanie_rekrutacyjne.Database.Models;

namespace zadanie_rekrutacyjne.ViewModels
{
    public class CategoryViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Nazwa:")]
        [Required(ErrorMessage = "Nazwa nie może być pusta.")]
        public String Name { get; set; }

        [Display(Name = "Kategoria nadrzędna:")]
        public int? ParentID { get; set; }

        public CategoryViewModel Parent { get; set; }
        public ICollection<CategoryViewModel> CategoryChildren { get; set; }

        public bool Shown { get; set; } = false;

        // constructor for map existing category to view model
        public CategoryViewModel(Category category)
        {
            ID = category.ID;
            Name = category.Name;
            ParentID = category.Parent?.ID;
            Parent = (category.Parent != null) ? new CategoryViewModel(category.Parent) : null;
        }

        // constructor for create new category
        public CategoryViewModel() { }

    }
}
