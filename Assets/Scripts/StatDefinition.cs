[System.Serializable]
public class StatDefinition : System.Object {

    public string name;
    public string description;
    public int baseValue;
    public int currentValue;

    public StatDefinition(string name, string description, int baseValue) {
        this.name = name;
        this.description = description;
        this.baseValue = baseValue;
    }
}
