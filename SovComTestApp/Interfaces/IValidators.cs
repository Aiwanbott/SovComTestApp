namespace SovComTestApp.Interfaces;

public interface IValidators
{
    bool ValidatePhoneNumbers(IList<string> phoneNumbers);
    bool ValidateDuplicates(IList<string> phoneNumbers);
    bool ValidateGSM(string message);
    bool ValidateCyrillic(string message);
    bool ValidateLength(string message);
}

