using Lab9.Interfaces;
using System.IO;

namespace Lab9.Classes;
[Serializable]
public abstract class Element : IInvertible
{
    private string name;
    private int inputCount;
    private int outputCount;

    public Element() { }

    public Element(string name, int inputCount = 1, int outputCount = 1)
    {
        this.name = name;
        this.inputCount = inputCount;
        this.outputCount = outputCount;
    }

    public string Name => name;
    public int InputCount => inputCount;
    public int OutputCount => outputCount;

    public virtual void SetInputs(int[] inputs)
    {
        throw new NotImplementedException("This element type does not support setting inputs.");
    }

    public abstract int ComputeOutput();

    // Реализация инверсии для базового класса
    public virtual void Invert()
    {
        throw new NotImplementedException("Invert not implemented for this element.");
    }
    public virtual string ToBinaryString()
    {
        throw new NotImplementedException();
    }
    public virtual void FromBinaryString(string dataString)
    {
        throw new NotImplementedException();
    }

    
}
