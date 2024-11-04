using Lab9.Interfaces;
using System.IO;

namespace Lab9.Classes;
[Serializable]

public class Register : Element, IShiftable
{
    private Memory[] triggers;
    private int setState = 0;

    public Register(int size) : base("10-bit Register", size, size)
    {
        triggers = new Memory[size];
        for (int i = 0; i < size; i++)
        {
            triggers[i] = new Memory();
        }
    }

    public int getCurrentState()
    {
        return triggers[0].getState();
    }
    public void SetSetState (int state)
    {
        if(state>=0 && state <= 1) { 
            setState = state;
            triggers[0].SetState(state);
        }
        else { setState = 0; }
       
    }

    public override void SetInputs(int[] inputs)
    {
        if (inputs.Length != InputCount)
            throw new ArgumentException($"Expected {InputCount} inputs.");

        for (int i = 0; i < inputs.Length; i++)
        {
            triggers[i].SetInputs(new int[] { inputs[i], setState });
        }
    }

    public override int ComputeOutput()
    {
        return triggers[0].ComputeOutput();
    }

    public override void Invert()
    {
        triggers[0].Invert();
    }

    public void Shift(int bits)
    {
        if (bits < 0)
        {
            throw new ArgumentException("Bit shift count cannot be negative.");
        }

        for (int i = 0; i < bits; i++)
        {
            var buff = triggers[triggers.Length - 1].getInputValues();
            for (int j = triggers.Length - 1; j > 0; j--)
            {
                triggers[j].SetInputsNoState(triggers[j-1].getInputValues());
            }
            triggers[0].SetInputsNoState(buff);
        }
        setState = triggers[0].getState();
    }
    

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < triggers.Length; i++)
        {
            result += triggers[i].ToString() + " ";
        }
        return result;
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
                triggers[i].FromBinaryString(reader.ReadString());
            }

            setState = reader.ReadInt32();
        }
    }
    public override string ToBinaryString()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(triggers.Length);
            foreach (var value in triggers)
            {
                writer.Write(value.ToBinaryString());
            }

            writer.Write(setState);

            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
