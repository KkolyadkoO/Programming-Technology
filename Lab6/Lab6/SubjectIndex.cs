namespace Lab6;

public class SubjectIndex : IComparable<SubjectIndex>
{
    private string _word;
    private List<int> _pageNumbers;

    public string Word
    {
        get => _word;
        set => _word = value;
    }

    public List<int> PageNumbers
    {
        get => _pageNumbers;
        set
        {
            if (value.Count > 10 || value.Count < 1)
                throw new ArgumentException("Количество страниц должно быть от 1 до 10.");
            _pageNumbers = value;
        }
    }

    public SubjectIndex(string word, List<int> pages)
    {
        Word = word;
        PageNumbers = pages;
    }

    public override string ToString()
    {
        return $"Слово: {Word}, Страницы: {string.Join(", ", PageNumbers)}";
    }

    public int CompareTo(SubjectIndex other)
    {
        return string.Compare(this.Word, other.Word, StringComparison.Ordinal);
    }

    public static bool operator ==(SubjectIndex a, SubjectIndex b)
    {
        return a.PageNumbers.Count == b.PageNumbers.Count;
    }

    public static bool operator !=(SubjectIndex a, SubjectIndex b)
    {
        return a.PageNumbers.Count != b.PageNumbers.Count;
    }

    public static bool operator >(SubjectIndex a, SubjectIndex b)
    {
        return a.PageNumbers.Count > b.PageNumbers.Count;
    }

    public static bool operator <(SubjectIndex a, SubjectIndex b)
    {
        return a.PageNumbers.Count < b.PageNumbers.Count;
    }

    public override bool Equals(object obj)
    {
        if (obj is SubjectIndex other)
        {
            return this.PageNumbers.Count == other.PageNumbers.Count;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return PageNumbers.Count.GetHashCode();
    }
}