using Lab9.Interfaces;
using System.IO;

namespace Lab9.Classes;
[Serializable]
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

    public byte[] ToBinaryData()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(inputValues.Length);
            foreach (var value in inputValues)
            {
                writer.Write(value);
            }

            writer.Flush();
            return ms.ToArray();
        }
    }

    public static Combinational FromBinaryData(byte[] data)
    {
        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            int inputValuesLength = reader.ReadInt32();
            var combinational = new Combinational(inputValuesLength);
            for (int i = 0; i < inputValuesLength; i++)
            {
                combinational.inputValues[i] = reader.ReadInt32();
            }

            return combinational;
        }
    }
    public override string ToString()
    {
        string result = "[";
        for(int i = 0; i < inputValues.Length-1; i++)
        {
            result += inputValues[i] + ",";
        }
        result += inputValues[inputValues.Length-1] + "]";
        return result;
    }

    public override string ToBinaryString()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(inputValues.Length);
            foreach (var value in inputValues)
            {
                writer.Write(value);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
    }
    public override void FromBinaryString(string dataString)
    {
        var data = Convert.FromBase64String(dataString);
        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {


            int inputValuesLength = reader.ReadInt32();
            for (int i = 0; i < inputValuesLength; i++)
            {
                inputValues[i] = reader.ReadInt32();
            }
        }
    }
}


