namespace SolarSystemN9.Data.Type;

public class SpaceDictionary : Dictionary<string, decimal>
{
    public override string ToString()
    {
        string value = "{ ";

        foreach (KeyValuePair<string, decimal> pair in this)
        {
            value += $" {pair.Key} : {pair.Value};";
        }

        return value + " }";
    }
}