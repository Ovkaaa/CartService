namespace CartService.API.Auth;

public class AuthOptions
{
    public const string SectionName = "Authentication";

    public string Authority { get; set; } = string.Empty;
    public bool RequireHttpsMetadata { get; set; }
    public string Audience { get; set; } = string.Empty;
}
