using UnityEngine;

public class ObjectStatistics : MonoBehaviour {

    public StatDefinition[] statistic;
    public enum ObjectTypes { Weapon, Shield, Chest_Armor, Helmet, Pants, Boots, Food, Gold, Useless, Ingredients, Potions };
    public ObjectTypes objectType;
}
