using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public List<GameObject> plantPrefabs;

    public Dictionary<string, GameObject> prefabLookup = new ();

    void Awake()
    {
        foreach (var p in plantPrefabs)
        {
            prefabLookup[p.name] = p;
        }

    }


    private void Start()
    {
        foreach (var plantData in SaveDataController.Instance.CurrentData.plantedPlants)
        {
            GameObject plantInstance = Instantiate(plantPrefabs[plantData.plantType], plantData.pos, plantData.rot);
            plantInstance.transform.localScale = plantData.scale;
            //plantInstance.GetComponent<plant1>().lifeTime = plantData.lifeTime;
            //plantInstance.GetComponent<plant1>().GrowInstant(plantData.lifeTime);
            plantInstance.GetComponent<plant1>().seed = plantData.seed;
            plantInstance.GetComponent<plant1>().random = new System.Random(plantData.seed);
        }
    }
}
