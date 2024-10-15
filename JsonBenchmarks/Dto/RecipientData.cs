
namespace JsonBenchmarks.Dto;

public record RecipientData
{
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? CountryIsoCode { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyIdentification { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FaxNumber { get; set; }
    public string? Email { get; set; }
    public string? ERegisteredIdentity { get; set; }

    public int? ERegisteredLocale { get; set; }

    public int? ConsentFlag { get; set; }
    public int? PrimaryPreference { get; set; }
    public DateTime? MaterializedDate { get; set; }
}
