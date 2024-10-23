namespace Lab6;

public class PageNumberComparer : IComparer<SubjectIndex>
{
    public int Compare(SubjectIndex x, SubjectIndex y)
    {
        return x.PageNumbers.Count.CompareTo(y.PageNumbers.Count);
    }
}