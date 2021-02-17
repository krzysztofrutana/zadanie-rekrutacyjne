using Microsoft.AspNetCore.Mvc;
using MyWebPage.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zadanie_rekrutacyjne.Database.Models;
using zadanie_rekrutacyjne.Helpers.Interfaces;
using zadanie_rekrutacyjne.ViewModels;

namespace zadanie_rekrutacyjne.ViewComponents
{
    [ViewComponent(Name = "Node")]
    public class NodeViewComponent : ViewComponent
    {
        public ICategoryRepository _categoryRepository;
        private readonly IShownPanelHelpers _shownPanelHelpers;

        public NodeViewComponent(ICategoryRepository categoryRepository, IShownPanelHelpers shownPanelHelpers)
        {
            _categoryRepository = categoryRepository;
            _shownPanelHelpers = shownPanelHelpers;
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<CategoryViewModel> categories, bool withOptions)
        {
            List<int> shownPanel = _shownPanelHelpers.GetIDs(); //get shown panels iDs

            // get all information from ViewData
            string searchText = ViewData["SearchText"]?.ToString();
            string sortOrder = ViewData["SortOrder"]?.ToString();
            string nodeID = ViewData["NodeID"]?.ToString();

            // model of this view component is list, so every categoryViewModel must be check
            foreach (CategoryViewModel categoryViewModel in categories)
            {
                // map category to category view model with cech if current category is show or not
                var categoryChildren = await _categoryRepository.GetChildrenAsync(categoryViewModel.ID);
                List<CategoryViewModel> categoryChildreViewModels = new List<CategoryViewModel>();
                foreach(Category category in categoryChildren)
                {
                    CategoryViewModel categoryChildrenViewModel = new CategoryViewModel(category);
                    categoryChildrenViewModel.Shown = shownPanel.Contains(category.ID) ? true : false;
                    categoryChildreViewModels.Add(categoryChildrenViewModel);
                }
                categoryViewModel.CategoryChildren = categoryChildreViewModels;
                
                // if current category must be sort
                if (!String.IsNullOrWhiteSpace(sortOrder) && !String.IsNullOrWhiteSpace(nodeID))
                {
                    if (categoryViewModel.ID == int.Parse(nodeID))
                    {
                        if (sortOrder.Equals("desc"))
                        {
                            categoryViewModel.CategoryChildren = categoryViewModel.CategoryChildren.OrderBy(c => c.Name).ToList();
                        }
                        else if (sortOrder.Equals("asc"))
                        {
                            categoryViewModel.CategoryChildren = categoryViewModel.CategoryChildren.OrderByDescending(c => c.Name).ToList();
                        }
                    }
                    else
                    {   // if id is different than current category sort all children by name
                        categoryViewModel.CategoryChildren = categoryViewModel.CategoryChildren.OrderBy(c => c.Name).ToList();
                    }
                }
                else
                {   // normal sort for all categories
                    categoryViewModel.CategoryChildren = categoryViewModel.CategoryChildren.OrderBy(c => c.Name).ToList();
                }
            }
            // if all main categories must be sort (nodeID == -1 means main)
            if(!String.IsNullOrWhiteSpace(nodeID) && int.Parse(nodeID) == -1)
            {
                if (sortOrder.Equals("desc"))
                {
                    categories = categories.OrderBy(c => c.Name).ToList();
                }
                else if (sortOrder.Equals("asc"))
                {
                    categories = categories.OrderByDescending(c => c.Name).ToList();
                }
            }

            ViewBag.WithOptions = withOptions; //options mean all edit, delete, details and sort button

            return View(categories);
        }
    }
}
