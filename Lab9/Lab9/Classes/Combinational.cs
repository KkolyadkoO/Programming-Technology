using Lab9.Interfaces;

namespace Lab9.Classes;
public class Combinational : Element
{
    private int[] inputValues;

    public Combinational(int inputCount) : base("MOD2", inputCount, 1)
    {
        inputValues = new int[inputCount];
    }

    public override void SetInputs(int[] inputs)
    {
        if (inputs.Length != InputCount)
            throw new ArgumentException($"Expected {InputCount} inputs.");
        
        inputValues = inputs;
    }

    public override int ComputeOutput()
    {
        return inputValues.Aggregate((a, b) => a ^ b);
    }

    public override void Invert()
    {
        inputValues = inputValues.Select(value => value == 0 ? 1 : 0).ToArray();
    }
}


