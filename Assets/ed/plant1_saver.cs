using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Data
{
    public Vector3 stem_1_pos;
    public Vector3 stem_1_rot;
    public Vector3 stem_1_scale;
    public Vector3 stem_2_rot;
    public Vector3 stem_2_scale;
    public Vector3 stem_2_endpos;
}

public class plant1_saver : MonoBehaviour
{
    public List<Data> plant_data;
    public List<Vector3> plant_detail_data;
    public GameObject plant1;
    public GameObject plant_detail1;
    public GameObject root;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("plant1_saver started");

        if (plant_data.Count == 0)
        {
            InvokeRepeating(nameof(save_plant), 10f, 10f);
        }
        else
        {
            load_plant(plant_data, plant_detail_data);
        }
        root.GetComponent<plant1>().grow_count = 5 - plant_data.Count;
        root.GetComponent<plant1>().StartCoroutine(root.GetComponent<plant1>().Grow());
        // StartCoroutine(save_plant());
        //if (plant_data != null)
        //{
        //   GameObject new_plant1 = Instantiate(plant1, plant_data[0], Quaternion.Euler(plant_data[1]), transform);
        //}
    }

    // Update is called once per frame
    void OnDestroy()
    {
        save_plant();

        SaveDataController.Instance.CurrentData.plantedPlants.Add(new()
        {
            plantType = 1,
            pos = transform.position,
            rot = transform.rotation,
            rootPlantData = plant_data,
            plantDetailData = plant_detail_data
        });
    }

    public void save_plant()
    {
        plant_data.Clear();
        plant_detail_data.Clear();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<plant1>())
            {
                Data data = new()
                {
                    stem_1_pos = child.GetComponent<plant1>().transform.position,
                    stem_1_rot = child.GetComponent<plant1>().transform.localEulerAngles,
                    stem_1_scale = new Vector3(1, child.GetComponent<plant1>().length1, 1),
                    stem_2_rot = child.GetComponent<plant1>().two.transform.eulerAngles,
                    stem_2_scale = new Vector3(1, child.GetComponent<plant1>().length2, 1),
                    stem_2_endpos = child.GetComponent<plant1>().three.transform.position
                };

                plant_data.Add(data);
            }
            if (child.GetComponent<plant_detail1>())
            {
                plant_detail_data.Add(child.GetComponent<plant_detail1>().transform.position);
            }
        }
    }

    public void load_plant(List<Data> data, List<Vector3> detail)
    {
        plant_data = data;
        plant_detail_data = detail;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);

        }
        for (int i = 0; i < plant_data.Count; i++)
        {
            GameObject new_plant1 = Instantiate(plant1, plant_data[i].stem_1_pos, Quaternion.Euler(plant_data[i].stem_1_rot), transform);
            new_plant1.GetComponent<plant1>().one.transform.localScale = new Vector3(1, plant_data[i].stem_1_scale.y, 1);
            new_plant1.GetComponent<plant1>().two.transform.eulerAngles = plant_data[i].stem_2_rot;
            new_plant1.GetComponent<plant1>().two.transform.localScale = new Vector3(1, plant_data[i].stem_2_scale.y, 1);
        }

        for (int i = 0; i < plant_detail_data.Count; i++)
        {
            GameObject new_plant_detail1 = Instantiate(plant_detail1, plant_detail_data[i], Quaternion.Euler(plant_detail_data[i]), transform);
        }

        if (plant_data.Count != 0)
        {
            GameObject continued_plant = Instantiate(plant1, plant_data[^1].stem_2_endpos, Quaternion.Euler(plant_data[^1].stem_2_rot), transform);
            continued_plant.GetComponent<plant1>().grow_count = plant_data.Count;
            continued_plant.GetComponent<plant1>().root = continued_plant;
            continued_plant.GetComponent<plant1>().is_root = true;
            continued_plant.GetComponent<plant1>().holder = this.gameObject;
            continued_plant.GetComponent<plant1>().StartCoroutine(continued_plant.GetComponent<plant1>().Grow());
        }
    }

    /*
    private void OnDestroy()
    {
        SaveDataController.Instance.CurrentData.plantedPlants.Add(new PlantSaveData
        {
            plantType = this.plantType,
            seed = this.seed,
            pos = this.transform.position,
            rot = this.transform.rotation,
            scale = this.transform.localScale,
            lifeTime = this.lifeTime
        });
    }
    */
}
