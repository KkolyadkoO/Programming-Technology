namespace Lab8;

public struct NOTE
{
    public string FullName  { get; set; }
    public string PhoneNumber { get; set; }
    public int[] BirthDate { get; set; } 

    public NOTE(string fullName, string phoneNumber, int[] birthDate)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
    }

    public override string ToString()
    {
        return $"{FullName}, {PhoneNumber}, {BirthDate[0]}/{BirthDate[1]}/{BirthDate[2]}";
    }
}