namespace AuthPoC.WebApi.Dtos.Auth;

public record LoginResponse(string Token, string Username, string Role);
