namespace EventSalesBackend.Data;

public class RegexPatterns
{
    public const string NamePattern = @"^[\p{L}\p{N}\p{M}\p{S}\p{P}\s]+$";


}
public class RegexValidationMessages
{
    public const string NamePatternMessage =
        "Only letters, numbers, spaces, common punctuation, and emojis are allowed.";
}