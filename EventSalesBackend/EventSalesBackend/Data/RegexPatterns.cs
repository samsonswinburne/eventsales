namespace EventSalesBackend.Data;

public class RegexPatterns
{
    public const string NamePattern = @"^[\p{L}\p{N}\p{M}\p{S}\p{P}\p{Zs}]+$";
    public const string SlugPattern = @"^[a-z0-9-]+$";
    public const string AlNumPattern = @"^[a-zA-Z]+$";
}
public class RegexValidationMessages
{
    public const string NamePatternMessage =
        "Only letters, numbers, spaces, common punctuation, and emojis are allowed.";
    public const string SlugPatternMessage = "Only lowercase characters, numbers and the \"-\" character are allowed";
    public const string AlNumPatternMessage = "Only alphanumeric characters are allowed in a ticket key";
}