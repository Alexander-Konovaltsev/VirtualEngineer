namespace VirtualEngineer.Validation
{
    public interface IValidator
    {
        bool Validate(string value, out string error);
    }

}