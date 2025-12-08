using UnityEngine;

[CreateAssetMenu(fileName = "PlantShopObject", menuName = "Scriptable Objects/PlantShopObject")]
public class PlantShopObject : ScriptableObject
{
    public string plantName;
    public double plantCost;
    public GameObject plantPrefab;
}
