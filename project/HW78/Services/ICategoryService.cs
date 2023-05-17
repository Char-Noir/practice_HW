using HW78.DTO;
using HW78.Dto.Request;
using HW78.Dto.Response;

namespace HW78.Services
{
    public interface ICategoryService
    {
        Task<DtoResult<int>> CreateCategoryAsync(CategoryDtoRequest category);

        Task<DtoResult<CategoryDtoResponse>> GetCategoryAsync(int categoryId);
        Task<DtoResult<IEnumerable<CategoryDtoResponse>>> GetCategoriesAsync(Dictionary<string, string> parametrs);

        Task<DtoResult<bool>> UpdateCategoryAsync(int id, CategoryDtoExpandedRequest category);
        Task<DtoResult<bool>> ActivateCategoryAsync(int id);
        Task<DtoResult<bool>> DeactivateCategoryAsync(int id);
        Task<DtoResult<bool>> ShowCategoryAsync(int id);
        Task<DtoResult<bool>> HideCategoryAsync(int id);

        Task<DtoResult<bool>> DeleteCategoryAsync(int categoryId);

    }
}
