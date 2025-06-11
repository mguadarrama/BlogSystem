using System;
using System.Linq;

namespace BlogCore.Domain;

public class Author
{
    internal Author() { } // For EF Core and testing

    public Author(string name, string surname, string socialSecurityNumber)
    {
        ValidateName(name);
        ValidateSurname(surname);
        ValidateSocialSecurityNumber(socialSecurityNumber);

        Name = name;
        Surname = surname;
        SocialSecurityNumber = socialSecurityNumber;
    }

    public int Id { get; internal set; }
    public string Name { get; internal set; }
    public string Surname { get; internal set; }
    public string SocialSecurityNumber { get; internal set; }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty");

        if (name.Length > 100)
            throw new DomainException("Name cannot be longer than 100 characters");
    }

    private void ValidateSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
            throw new DomainException("Surname cannot be empty");

        if (surname.Length > 100)
            throw new DomainException("Surname cannot be longer than 100 characters");
    }

    private void ValidateSocialSecurityNumber(string ssn)
    {
        if (string.IsNullOrWhiteSpace(ssn))
            throw new DomainException("Social security number cannot be empty");

        if (ssn.Length != 11)
            throw new DomainException("Social security number must be exactly 11 digits long");

        // Choose one of the following by commenting/uncommenting:

        if (!IsDigitsOnly_Linq(ssn)) // LINQ approach
        // if (!IsDigitsOnly_ForLoop(ssn)) // Manual for-loop approach
            throw new DomainException("Social security number must contain only digits");
    }

    // LINQ-style validation
    private bool IsDigitsOnly_Linq(string input)
    {
        return input.All(char.IsDigit);
    }

    // Manual for-loop validation
    private bool IsDigitsOnly_ForLoop(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (!char.IsDigit(input[i]))
                return false;
        }
        return true;
    }
}
