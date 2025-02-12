namespace CleanArchitecture.Shared.Models.Errors;

public class ErrorProperty
{
    public string Property { get; set; }
    public string Value { get; set; }
    public ErrorProperty()
    {

    }
    public ErrorProperty(string property, string value)
    {
        Property = property;
        Value = value;
    }
}
