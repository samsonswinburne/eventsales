namespace EventSalesBackend.Data;

public class RegexPatterns
{
    public const string NamePattern = @"^[\p{L}\p{N}\u200D\uFE0F .,!?'""\-()]+$";

    public const string NamePatternValidationMessage =
        "Only letters, numbers, spaces, common punctuation, and emojis are allowed.";
}