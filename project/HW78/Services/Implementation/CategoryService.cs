using HW78.DTO;
using HW78.Dto.Request;
using HW78.Dto.Response;
using HW78.DAO;

namespace HW78.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _categoryDao;

        public CategoryService(ICategoryDao categoryDao)
        {
            _categoryDao = categoryDao;
        }

        public async Task<DtoResult<bool>> ActivateCategoryAsync(int id)
        {
            var category = await _categoryDao.GetCategoryAsync(id);
            if (!category.IsSuccessed){
                return DtoResult<bool>.Error(category.Messages[0]);
            }
            if (category.Data.IsActive)
            {
                return DtoResult<bool>.Error("Category activated already");
            }
            return await _categoryDao.UpdateCategoryAsync(id, new CategoryDtoExpandedRequest() { NameCategory = category.Data.NameCategory, IsVisible = category.Data.IsVisible, IsActive = true });
        }

        public async Task<DtoResult<int>> CreateCategoryAsync(CategoryDtoRequest category)
        {
            if(category.NameCategory.Length<=3 || category.NameCategory.Length > 64)
            {
                return DtoResult<int>.Error("Name lenght must be beetween 4 and 64 inclusivly.");
            }
            return await _categoryDao.CreateCategoryAsync(category);
        }

        public async Task<DtoResult<bool>> DeactivateCategoryAsync(int id)
        {
            var category = await _categoryDao.GetCategoryAsync(id);
            if (!category.IsSuccessed)
            {
                return DtoResult<bool>.Error(category.Messages[0]);
            }
            if (!category.Data.IsActive)
            {
                return DtoResult<bool>.Error("Category deactivated already");
            }
            return await _categoryDao.UpdateCategoryAsync(id, new CategoryDtoExpandedRequest() { NameCategory = category.Data.NameCategory, IsVisible = category.Data.IsVisible, IsActive = false });
        }

        public async Task<DtoResult<bool>> DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryDao.GetCategoryAsync(categoryId);
            if (!category.IsSuccessed)
            {
                return DtoResult<bool>.Error(category.Messages[0]);
            }
            if (category.Data.NumberOfExpenses > 0)
            {
                return DtoResult<bool>.Error("You cannot delete category with expenses.");
            }
            return  await _categoryDao.DeleteCategoryAsync(categoryId);
            
        }

        public async Task<DtoResult<CategoryDtoResponse>> GetCategoryAsync(int categoryId)
        {
            var result = await _categoryDao.CheckCategoryAsync(categoryId);
            if (result.IsSuccessed && result.Data)
            {
                return await _categoryDao.GetCategoryAsync(categoryId);
            }
            return DtoResult<CategoryDtoResponse>.Error("Category not found.");
        }

        public async Task<DtoResult<IEnumerable<CategoryDtoResponse>>> GetCategoriesAsync(Dictionary<string, string> parametrs)
        {
            return await _categoryDao.GetCategoriesAsync(parametrs);
        }

        public async Task<DtoResult<bool>> HideCategoryAsync(int id)
        {
            var category = await _categoryDao.GetCategoryAsync(id);
            if (!category.IsSuccessed)
            {
                return DtoResult<bool>.Error(category.Messages[0]);
            }
            if (!category.Data.IsVisible)
            {
                return DtoResult<bool>.Error("Category hided already");
            }
            return await _categoryDao.UpdateCategoryAsync(id, new CategoryDtoExpandedRequest() { NameCategory = category.Data.NameCategory, IsVisible = false, IsActive = category.Data.IsActive });
        }

        public async Task<DtoResult<bool>> ShowCategoryAsync(int id)
        {
            var category = await _categoryDao.GetCategoryAsync(id);
            if (!category.IsSuccessed)
            {
                return DtoResult<bool>.Error(category.Messages[0]);
            }
            if (category.Data.IsVisible)
            {
                return DtoResult<bool>.Error("Category visible already");
            }
            return await _categoryDao.UpdateCategoryAsync(id, new CategoryDtoExpandedRequest() { NameCategory = category.Data.NameCategory, IsVisible = true, IsActive = category.Data.IsActive });
        }

        public async Task<DtoResult<bool>> UpdateCategoryAsync(int id, CategoryDtoExpandedRequest category)
        {
            var category1 = await _categoryDao.CheckCategoryAsync(id);
            if (!category1.IsSuccessed || !category1.Data)
            {
                return DtoResult<bool>.Error(category1.Messages[0]);
            }
            return await _categoryDao.UpdateCategoryAsync(id, category);
        }
    }
}
