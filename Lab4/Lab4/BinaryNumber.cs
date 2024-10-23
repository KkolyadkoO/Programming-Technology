namespace Lab4;

public class BinaryNumber
{
    public string Value { get; private set; }

    public BinaryNumber(string binaryString)
    {
        if (IsBinary(binaryString))
        {
            Value = binaryString;
        }
        else
        {
            throw new ArgumentException("Недопустимое значение. Ожидалось двоичное число.");
        }
    }

    private static bool IsBinary(string value)
    {
        return !string.IsNullOrEmpty(value) && value.All(c => c is '0' or '1');
    }

    private int ToInt() => Convert.ToInt32(Value, 2);

    private static string ToBinaryString(int number) => Convert.ToString(number, 2);

    public static BinaryNumber operator +(BinaryNumber a, BinaryNumber b)
    {
        int result = a.ToInt() + b.ToInt();
        return new BinaryNumber(ToBinaryString(result));
    }

    public static BinaryNumber operator -(BinaryNumber a, BinaryNumber b)
    {
        int result = a.ToInt() - b.ToInt();
        if (result < 0)
            throw new InvalidOperationException("Результат операции вычитания меньше нуля.");

        return new BinaryNumber(ToBinaryString(result));
    }

    public static BinaryNumber operator *(BinaryNumber a, BinaryNumber b)
    {
        int result = a.ToInt() * b.ToInt();
        return new BinaryNumber(ToBinaryString(result));
    }

    public static BinaryNumber operator /(BinaryNumber a, BinaryNumber b)
    {
        if (b.ToInt() == 0)
            throw new DivideByZeroException("Деление на ноль.");
        
        int result = a.ToInt() / b.ToInt();
        return new BinaryNumber(ToBinaryString(result));
    }

    public override string ToString() => Value;
}