using Lab9.Interfaces;

namespace Lab9.Classes;

public class Memory : Element
{
    private int[] inputValues;
    private int directOutput;
    private int inverseOutput;

    public Memory() : base("D-Trigger", 1, 1)
    {
        inputValues = new int[InputCount];
        Reset();
    }

    public override void SetInputs(int[] inputs)
    {
        if (inputs.Length != InputCount)
            throw new ArgumentException($"Expected {InputCount} inputs.");

        inputValues = inputs;
    }

    public override int ComputeOutput()
    {
        return directOutput;
    }

    public override void Invert()
    {
        directOutput = directOutput == 0 ? 1 : 0;
        inverseOutput = inverseOutput == 0 ? 1 : 0;
    }

    private void Reset()
    {
        directOutput = 0;
        inverseOutput = 1;
    }
}

