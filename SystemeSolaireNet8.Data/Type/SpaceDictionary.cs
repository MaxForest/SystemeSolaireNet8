namespace SystemeSolaireNet8.Data.Type
{
  public class SpaceDictionary : Dictionary<string, decimal>
  {
    public override string ToString()
    {
      string value = "{ ";

      foreach (var pair in this)
      {
        value += $" {pair.Key} : {pair.Value};";
      }

      return value + " }";
    }
  }
}
