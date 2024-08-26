using System.Text.RegularExpressions;

namespace BuildingBlocks.ValueObjects;

public sealed partial class Email : ValueObject
{
    public const int MaxLength = 255;

    private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    private static readonly Lazy<Regex> EmailFormatRegex = new(IsValidEmail);

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(Email email)
        => email.Value;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(EmailErrors.NullOrEmpty);
        }

        if (email.Length > MaxLength)
        {
            return Result.Failure<Email>(EmailErrors.LongerThanAllowed);
        }

        if (!EmailFormatRegex.Value.IsMatch(email))
        {
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        }

        return Result.Create(new Email(email));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    [GeneratedRegex(EmailRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex IsValidEmail();
}
