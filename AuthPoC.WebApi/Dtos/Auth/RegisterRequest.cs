namespace AuthPoC.WebApi.Dtos.Auth;

public record RegisterRequest(string Username, string Password, string Role);
