namespace EventFlow.Common.Infrastructure.Authentication;

public static class CustomClaims
{
    // JWT subject claim that stores the application's user ID.
    public const string Sub = "sub";

    // Custom claim used to store user permissions.
    public const string Permission = "permission";
}
