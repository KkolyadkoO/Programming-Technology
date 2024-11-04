namespace Lab6;

public class SubjectIndex : IComparable
{
    public string Word { get; set; }
    public List<int> PageNumbers { get; set; }

    public SubjectIndex(string word, List<int> pageNumbers)
    {
        Word = word;
        PageNumbers = pageNumbers;
    }

    public SubjectIndex() : this(string.Empty, new List<int>()) { }

    public int CompareTo(SubjectIndex? other)
    {
        return string.Compare(this.Word, other.Word, StringComparison.Ordinal);
    }

    public static bool operator >(SubjectIndex a, SubjectIndex b)
    {
        return a.CompareTo(b) > 0;
    }

    public static bool operator <(SubjectIndex a, SubjectIndex b)
    {
        return a.CompareTo(b) < 0;
    }

    public static bool operator ==(SubjectIndex a, SubjectIndex b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(SubjectIndex a, SubjectIndex b)
    {
        return !a.Equals(b);
    }

    public override bool Equals(object obj)
    {
        if (obj is SubjectIndex other)
        {
            return this.Word == other.Word;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Word.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Word}: {string.Join(", ", PageNumbers)}";
    }

    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }
}