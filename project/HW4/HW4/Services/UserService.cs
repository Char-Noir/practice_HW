using System.ComponentModel.DataAnnotations;
using HW4.DTO;
using HW4.Models;
using Microsoft.EntityFrameworkCore;

namespace HW4.Services;

public class UserService: IUserService
{
    private readonly Hw4BuriakContext _context;

    public UserService(Hw4BuriakContext context)
    {
        _context = context;
    }

    public async Task<UserResponseDto> AddUserAsync(UserRequestDto user)
    {
        var newUser = new User(){UserName = user.UserName, UserEmail = user.UserEmail};
        if (!ValidateEmail(newUser))
        {
            return new UserResponseDto(null, user.UserEmail, user.UserName, false,"Invalid email address.");
        }

        if (user.UserName.Length <= 0 || user.UserName.Length > 60)
        {
            return new UserResponseDto(null, user.UserEmail, user.UserName, false,"Name length must be between 3 and 60.");
        }

        if (_context.Users.Any(x => user.UserEmail == x.UserEmail))
        {
            return new UserResponseDto(null, user.UserEmail, user.UserName, false,"This email is already registered.");
        }

        try
        {

            var role = await _context.Roles.Where(x => x.RoleName == "Developer").FirstOrDefaultAsync();
            if (role != null)
            {
                newUser.UserRole = role.RoleId;
            }
            else
            {
                return new UserResponseDto(null, user.UserName, user.UserEmail, true, "Something went wrong. Please contact the administrator.");
            }
            await _context.Users.AddAsync(newUser);
            _context.SaveChanges();

            return new UserResponseDto(newUser.UserId, newUser.UserName, newUser.UserEmail, true, string.Empty);
        }
        catch
        {
            return new UserResponseDto(null, user.UserName, user.UserEmail, true, "Something went wrong. Please contact the administrator.");
        }
       
        
    }

    private static bool ValidateEmail(User user)
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateProperty(user.UserEmail, new ValidationContext(user, null, null) { MemberName = "UserEmail" }, validationResults);

        return isValid;
    }

    public async Task<UserResponseDto> GetUserAsync(int id)
    {
        if (id <= 0)
        {
            return new UserResponseDto(null, string.Empty, string.Empty, false,"Invalid Id"); 
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return new UserResponseDto(null, string.Empty, string.Empty, false,"User not found!"); 
        }
        return new UserResponseDto(user.UserId, user.UserName, user.UserEmail, true, string.Empty);
    }
}