using Lab9.Interfaces;
using System.IO;

namespace Lab9.Classes;
[Serializable]

public class Memory : Element
{
    private int[] inputValues;
    private int directOutput;
    private int inverseOutput;

    public Memory() : base("D-Trigger", 2, 2)
    {
        inputValues = new int[InputCount];
        Reset();
    }
    public void SetInputsNoState(int[] inputs)
    {
        inputValues[0] = inputs[0];
        inputValues[1] = inputs[1];
    }
    public int getState()
    {
        return inputValues[1];
    }

    public override void SetInputs(int[] inputs)
    {
        if (inputs.Length != InputCount)
            throw new ArgumentException($"Expected {InputCount} inputs.");
        if (inputs[1] == 0)
        {
            inputValues[1] = 0;
        }
        else
        {
            inputValues = inputs;
        }
        SetOutputs();
    }
    public int[] getInputValues() {
        return [inputValues[0], inputValues[1]]; 
    }
    private void SetOutputs()
    {
        directOutput = inputValues[0];
        inverseOutput = inputValues[0] == 0 ? 1 : 0;
    }

    public override int ComputeOutput()
    {
        return directOutput;
    }
    public void SetState(int state)
    {
        if(state>=0 && state <= 1)
        {
            inputValues[1] = state;
        }
        else
        {
            inputValues[1] = 0;
        }
    }

    public override void Invert()
    {
        if (inputValues[1] == 1)
        {
            inputValues[0] = inputValues[0] == 0 ? 1 : 0;
            directOutput = directOutput == 0 ? 1 : 0;
            inverseOutput = inverseOutput == 0 ? 1 : 0;
        }  
    }

    private void Reset()
    {
        directOutput = 0;
        inverseOutput = 1;
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

            writer.Write(directOutput);
            writer.Write(inverseOutput);

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

            directOutput = reader.ReadInt32();
            inverseOutput = reader.ReadInt32();
        }
    }
    public override string ToString()
    {
        return "["+inputValues[0]+ ", "+ inputValues[1]+"]";
    }
}

