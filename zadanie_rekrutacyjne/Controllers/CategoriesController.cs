using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebPage.Repositories.Interfaces;
using Newtonsoft.Json;
using zadanie_rekrutacyjne.Database.Data;
using zadanie_rekrutacyjne.Database.Models;
using zadanie_rekrutacyjne.Helpers.Interfaces;
using zadanie_rekrutacyjne.Models;
using zadanie_rekrutacyjne.ViewModels;

namespace zadanie_rekrutacyjne.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IShownPanelHelpers _shownPanelHelpers;

        public CategoriesController(ICategoryRepository categoryRepository, IShownPanelHelpers shownPanelHelpers)
        {
            _categoryRepository = categoryRepository;
            _shownPanelHelpers = shownPanelHelpers;
        }

        public IActionResult Index()
        {
            // for now only redirect to tree page
            return RedirectToAction(nameof(Tree));
        }


        // table page
        public async Task<IActionResult> Table(string nameSortOrder, string parentCategorySortOrder, string searchText)
        {
            var categories = await _categoryRepository.GetAllWithParentsAsync();

            // is search text exist if parent is null it is necessary to add a new category object with name to filter by paren name
            if (!String.IsNullOrWhiteSpace(searchText))
            {
                foreach (Category category in categories)
                {
                    if (category.Parent == null)
                    {
                        category.Parent = new Category()
                        {
                            Name = "Kategoria główna"
                        };
                    }
                }
                ViewData["SearchText"] = searchText;
                categories = categories.Where(c => (c.Name.Contains(searchText) || c.Parent.Name.Contains(searchText))).ToList();
            }

            // sort by name column
            if (!String.IsNullOrWhiteSpace(nameSortOrder))
            {
                if (nameSortOrder.Equals("asc"))
                {
                    categories = categories.OrderByDescending(c => c.Name).ToList();
                }
                else if (nameSortOrder.Equals("desc"))
                {
                    categories = categories.OrderBy(c => c.Name).ToList();
                }
                ViewData["NameSortOrder"] = nameSortOrder;

            }

            // sort by category parent name column
            else if (!String.IsNullOrWhiteSpace(parentCategorySortOrder))
            {
                if (parentCategorySortOrder.Equals("asc"))
                {
                    categories = categories.OrderByDescending(c => c.Parent?.Name).ToList();
                }
                else if (parentCategorySortOrder.Equals("desc"))
                {
                    categories = categories.OrderBy(c => c.Parent?.Name).ToList();
                }
                ViewData["ParentCategorySortOrder"] = parentCategorySortOrder;
            }

            return View("Table", categories);
        }



        public async Task<IActionResult> Tree(string searchText, string sortOrder, string nodeID)
        {
            // to prepare tree neccesarry is get all shown panel first 
            List<int> shownPanel = _shownPanelHelpers.GetIDs();

            if (!String.IsNullOrWhiteSpace(searchText))
            {
                var allCategories = await _categoryRepository.GetAllWithChildrenAsync();

                // map category object to category view model object with check if panel was shown before page refresh
                List<CategoryViewModel> allCategoryViewModels = new List<CategoryViewModel>();
                foreach (Category category in allCategories)
                {
                    CategoryViewModel categoryViewModel = new CategoryViewModel(category);
                    categoryViewModel.Shown = shownPanel.Contains(category.ID) ? true : false;
                    allCategoryViewModels.Add(categoryViewModel);
                }

                
                allCategoryViewModels = allCategoryViewModels.Where(c => (c.Name.Contains(searchText))).ToList(); // filter all categories by search text

                List<CategoryViewModel> readyToShowCategoriesViewModel = new List<CategoryViewModel>();
                readyToShowCategoriesViewModel.AddRange(allCategoryViewModels);

                foreach(CategoryViewModel categoryViewModelCheckDuplicates in allCategoryViewModels)
                {
                    // check if in filtered allCategoryViewModels exist duplicates (to avoid the situation when 
                    // in result of search will be parent with current category and this category at the same time

                    var checkIfContains = allCategoryViewModels.Where(c => c.ID == categoryViewModelCheckDuplicates.Parent?.ID).ToList();
                    if (checkIfContains != null && checkIfContains.Count > 0) 
                    {
                        readyToShowCategoriesViewModel.Remove(categoryViewModelCheckDuplicates);
                    }
                }
                // at the end sort readyToShowCategoriesViewModel by name
                readyToShowCategoriesViewModel = readyToShowCategoriesViewModel.OrderBy(c => c.Name).ToList();

                // save arguments for use in view and view component invoke method
                ViewData["SearchText"] = searchText;
                ViewData["SortOrder"] = sortOrder ?? "";
                ViewData["NodeID"] = nodeID ?? ""; // save nodeId which will be sort

                return View("Tree", readyToShowCategoriesViewModel);
            }
            else
            {
                // simillary method as above without filter by search text
                var categories = await _categoryRepository.GetMainAsync();
                List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
                foreach (Category category in categories)
                {
                    CategoryViewModel categoryViewModel = new CategoryViewModel(category);
                    categoryViewModel.Shown = shownPanel.Contains(category.ID) ? true : false;
                    categoryViewModels.Add(categoryViewModel);
                }
                

                if (String.IsNullOrWhiteSpace(sortOrder) && String.IsNullOrWhiteSpace(nodeID))
                {
                    categoryViewModels = categoryViewModels.OrderBy(c => c.Name).ToList();
                }

                ViewData["SortOrder"] = sortOrder ?? "";
                ViewData["NodeID"] = nodeID ?? "";

                return View("Tree", categoryViewModels);
            }

        }

        // GET: Categories/Details/5
        // standard Detail method with map object to view object 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindWithChildrenAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel categoryViewModel = new CategoryViewModel(category);
            List<CategoryViewModel> categoryChildreViewModels = new List<CategoryViewModel>();
            if (category.CategoryChildren != null)
            {
                foreach (Category categoryChildren in category.CategoryChildren)
                {
                    CategoryViewModel categoryChildrenViewModel = new CategoryViewModel(categoryChildren);
                    categoryChildreViewModels.Add(categoryChildrenViewModel);
                }
            }
            categoryViewModel.CategoryChildren = categoryChildreViewModels;

            return PartialView("_DetailsModalPartial", categoryViewModel);
        }



        // GET: Categories/Create
        // standard create method 
        public async Task<IActionResult> Create()
        {
            var categoryList = await _categoryRepository.GetAllAsync();

            // prepare select list for drop down menu. Use all categories because when category is created can be added to any categories (and to Kategoria główna which is null)
            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Kategoria główna", Value = "" }
            };
            foreach (Category categoryItem in categoryList)
            {
                selectListItems.Add(new SelectListItem() { Text = categoryItem.Name, Value = categoryItem.ID.ToString() });
            }
            ViewBag.CategoriesList = new SelectList(selectListItems, "Value", "Text");


            return PartialView("_CreateModalPartial", new CategoryViewModel());
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            // after AJAX request, the result of the post method, go back to the Ajax function so it is necessary to create a category list for the drop-down menu a second time, whether the model is correct
                        var categoryList = await _categoryRepository.GetAllAsync();
            List<SelectListItem> selectListItems = new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Kategoria główna", Value = "" }
                };
            foreach (Category categoryItem in categoryList)
            {
                selectListItems.Add(new SelectListItem() { Text = categoryItem.Name, Value = categoryItem.ID.ToString() });
            }

            if (ModelState.IsValid)
            {
                // search choosen parent by id, create new object category, add name and parent to object (parent can be null) and save to database
                var parent = await _categoryRepository.FindAsync(categoryViewModel.ParentID);

                Category category = new Category()
                {
                    Name = categoryViewModel.Name,
                    Parent = parent
                };
                _categoryRepository.Add(category);
                await _categoryRepository.SaveChangesAsync();

                ViewBag.CategoriesList = new SelectList(selectListItems, "Value", "Text");

                return PartialView("_CreateModalPartial", new CategoryViewModel());
            }
            else
            {

                ViewBag.CategoriesList = new SelectList(selectListItems, "Value", "Text");

                return PartialView("_CreateModalPartial", new CategoryViewModel());
            }

        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel categoryViewModel = new CategoryViewModel(category); // map category to category view model

            var categoryList = await _categoryRepository.GetAllAsync(); // get all categories 
            var categoryChildrenWithParentOnCurrentNode = await _categoryRepository.GetAllNodeChildrenAsync(category.ID); // get all categories in current node start from current category
            categoryChildrenWithParentOnCurrentNode.Add(category.Parent); // category cannot be move to yourself
            categoryList = categoryList.Except(categoryChildrenWithParentOnCurrentNode).ToList(); // remove from all categories list all category from current category node

            // create selectlist with all left categories and main category
            List<SelectListItem> selectListItems = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Kategoria główna", Value = "" }
            };
            foreach (Category categoryItem in categoryList)
            {
                if (categoryItem.ID == category.ID)
                    continue;
                selectListItems.Add(new SelectListItem() { Text = categoryItem.Name, Value = categoryItem.ID.ToString() });
            }

            ViewBag.CategoriesList = new SelectList(selectListItems, "Value", "Text");

            return PartialView("_EditModalPartial", categoryViewModel);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel categoryViewModel)
        {
            var categoryList = await _categoryRepository.GetAllAsync();
            var categoryChildren = await _categoryRepository.GetAllNodeChildrenAsync(id);
            categoryList = categoryList.Except(categoryChildren).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Category categoryItem in categoryList)
            {
                if (categoryItem.ID == id)
                    continue;
                selectListItems.Add(new SelectListItem() { Text = categoryItem.Name, Value = categoryItem.ID.ToString() });
            }

            // very similar situation as in create method
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = await _categoryRepository.FindAsync(id);
                    category.Name = categoryViewModel.Name;
                    if (categoryViewModel.ParentID.Equals(""))
                    {
                        category.Parent = null;
                    }
                    else
                    {
                        category.Parent = await _categoryRepository.FindAsync(categoryViewModel.ParentID);
                    }
                    _categoryRepository.Update(category);
                    await _categoryRepository.SaveChangesAsync();

                    
                    ViewBag.CategoriesList = new SelectList(selectListItems, "Value", "Text");
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!CategoryExists(categoryViewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }

                return PartialView("_EditModalPartial", categoryViewModel);
            }
            else
            {
            
                ViewBag.CategoriesList = new SelectList(selectListItems, "Value", "Text");
                return PartialView("_EditModalPartial", categoryViewModel);
            }


        }

        // GET: Categories/Delete/5
        // standard delete get method return current category
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteModalPartial", category);
        }

        // POST: Categories/Delete/5
        // standard delete post method, if category had children delete all children also
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryRepository.FindWithChildrenAsync(id);
                if (category.CategoryChildren != null && category.CategoryChildren.Count > 0)
                {
                    await _categoryRepository.DeleteWithChildrenAsync(category);
                }
                else
                {
                    _categoryRepository.DeleteAsync(category);
                }
                await _categoryRepository.SaveChangesAsync();
                return PartialView("_DeleteModalPartial", category);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        private bool CategoryExists(int id)
        {
            return _categoryRepository.CategoryExist(id);
        }

        // save infomation about shown panels id
        [HttpPost]
        public IActionResult SetShownPanel(string JsonLocalStorageObj)
        {
            JsonLocalStorageModel panels = JsonConvert.DeserializeObject<JsonLocalStorageModel>(JsonLocalStorageObj);
            _shownPanelHelpers.SetIDs(panels.GetIDs());

            return new EmptyResult();
        }
    }
}
