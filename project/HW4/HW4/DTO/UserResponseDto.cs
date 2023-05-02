namespace HW4.DTO;

public record UserResponseDto(int? Id, string UserName, string UserEmail, bool Success, string Error);