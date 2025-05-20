namespace UserService.Application.DTOs;

public record RegisterUserRequest(string Email, string FullName, string Password, string PhoneNumber);