using Lab9.Interfaces;

namespace Lab9.Classes;

public class Register : Element, IShiftable
{
    private Memory[] triggers;
    private int resetState;
    private int setState;

    public Register(int size) : base("10-bit Register", size, size)
    {
        triggers = new Memory[size];
        for (int i = 0; i < size; i++)
        {
            triggers[i] = new Memory();
        }
    }

    public override void SetInputs(int[] inputs)
    {
        if (inputs.Length != InputCount)
            throw new ArgumentException($"Expected {InputCount} inputs.");

        for (int i = 0; i < inputs.Length; i++)
        {
            triggers[i].SetInputs(new int[] { inputs[i] });
        }
    }

    public override int ComputeOutput()
    {
        // Пример вычисления: возвращает количество активных (1) выходов
        return triggers.Count(t => t.ComputeOutput() == 1);
    }

    public override void Invert()
    {
        foreach (var trigger in triggers)
        {
            trigger.Invert();
        }
    }

    public void Shift(int bits)
    {
        if (bits < 0)
        {
            throw new ArgumentException("Bit shift count cannot be negative.");
        }

        for (int i = 0; i < bits; i++)
        {
            for (int j = triggers.Length - 1; j > 0; j--)
            {
                triggers[j].SetInputs(new int[] { triggers[j - 1].ComputeOutput() });
            }
            triggers[0].SetInputs(new int[] { 0 });
        }
    }
}
