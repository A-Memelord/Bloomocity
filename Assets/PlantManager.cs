using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject plantPrefab;

    private void Start()
    {
        foreach (var plantData in SaveDataController.Instance.CurrentData.plantedPlants)
        {
            GameObject plantInstance = Instantiate(plantPrefab, plantData.pos, plantData.rot);
            // here

            //plantInstance.transform.localScale = plantData.scale;
            //plantInstance.GetComponent<plant1>().lifeTime = plantData.lifeTime;
            //plantInstance.GetComponent<plant1>().LoadRootData(plantData.lifeTime, plantData.rootPlantData);
            //plantInstance.GetComponent<plant1>().seed = plantData.seed;
            //plantInstance.GetComponent<plant1>().random = new System.Random(plantData.seed);

            Debug.Log("Loading plant at position: " + plantData.pos);

            plantInstance.GetComponent<plant1_saver>().load_plant(plantData.rootPlantData, plantData.plantDetailData, plantData.plantColor);

            Debug.Log("Plant loaded with root data count: " + plantData.rootPlantData.Count);
        }

        SaveDataController.Instance.CurrentData.plantedPlants.Clear();
    }
}
