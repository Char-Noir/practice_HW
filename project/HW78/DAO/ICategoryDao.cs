using HW78.DTO;
using HW78.Dto.Request;
using HW78.Dto.Response;

namespace HW78.DAO
{
    public interface ICategoryDao
    {
        Task<DtoResult<int>> CreateCategoryAsync(CategoryDtoRequest category);

        Task<DtoResult<CategoryDtoResponse>> GetCategoryAsync(int categoryId);
        Task<DtoResult<IEnumerable<CategoryDtoResponse>>> GetCategoriesAsync(Dictionary<string, string> parametrs);

        Task<DtoResult<bool>> UpdateCategoryAsync(int id, CategoryDtoExpandedRequest category);

        Task<DtoResult<bool>> DeleteCategoryAsync(int categoryId);

        Task<DtoResult<bool>> CheckCategoryAsync(int categoryId);
    }
}
