using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Random = System.Random;

public class plant1 : MonoBehaviour
{
    public GameObject model;
    public GameObject armature;
    public GameObject plantpart;
    public GameObject one;
    public GameObject two;
    public GameObject three;

    public GameObject root;
    public int grow_count = 5;

    public int plantType = 1;
    public int seed;

    public bool is_root = true;

    public List<Vector3> root_plant_data;
    public Random random;

    private void OnDestroy()
    {
        if (is_root)
        {
            SaveDataController.Instance.CurrentData.plantedPlants.Add(new PlantSaveData
            {
                plantType = this.plantType,
                seed = this.seed,
                pos = this.transform.position,
                rot = this.transform.rotation,
                scale = this.transform.localScale,
                rootPlantData = root_plant_data
            });
        }
    }

    void Start()
    {
        if (is_root)
        {
            root = this.gameObject;
            add_data(transform.position);
        }
        StartCoroutine(Grow());
    }

    public IEnumerator Grow()
    {
        if (root.GetComponent<plant1>().grow_count != 0)
        {
            root.GetComponent<plant1>().grow_count -= 1;
            float difference = random.NextFloat(0.1f, 0.2f);
            float growth = random.NextFloat(0.5f, 0.8f);

            for (float t = 0f; t < 1; t += Time.deltaTime * difference)
            {
                one.transform.localScale = new Vector3(1, Mathf.Lerp(0, growth, t), 1);
                yield return new WaitForEndOfFrame();
            }
            root.GetComponent<plant1>().add_data(one.transform.position);

            two.transform.rotation = Quaternion.Euler(random.NextFloat(-45, 45), random.NextFloat(-45, 45), random.NextFloat(-45, 45));
            root.GetComponent<plant1>().add_data(two.transform.eulerAngles);
            float difference2 = random.NextFloat(0.1f, 0.2f);
            float growth2 = random.NextFloat(0.5f, 0.8f);
            bool wall = false;
            while (wall == true)
            {
                wall = false;
                if (Physics.Raycast(two.transform.position, transform.forward * growth2, out RaycastHit hitInfo, growth2 * 2))
                {
                    wall = true;
                    two.transform.rotation = Quaternion.Euler(random.NextFloat(-45, 45), random.NextFloat(-45, 45), random.NextFloat(-45, 45));
                }
            }


            for (float t = 0f; t < 1; t += Time.deltaTime * difference2)
            {
                two.transform.localScale = new Vector3(1, Mathf.Lerp(0, growth2, t), 1);
                yield return new WaitForEndOfFrame();
            }
            root.GetComponent<plant1>().add_data(one.transform.position);

            if (root.GetComponent<plant1>().grow_count != 0)
            {
                int stems = random.Next(1, 3);
                while (stems > 0)
                {
                    stems--;
                    GameObject new_plant_part = Instantiate(plantpart, three.transform);
                    new_plant_part.GetComponent<plant1>().is_root = false;
                    new_plant_part.GetComponent<plant1>().root = root;
                    new_plant_part.transform.SetParent(plantpart.transform);
                    new_plant_part.GetComponent<plant1>().model.transform.localScale = Vector3.one;
                    new_plant_part.transform.localScale = Vector3.one;
                    new_plant_part.transform.localPosition = three.transform.localPosition;
                    new_plant_part.transform.rotation = three.transform.rotation;
                    new_plant_part.transform.position = three.transform.position;
                    new_plant_part.GetComponent<plant1>().one.transform.localScale = new Vector3(1, 0.01f, 1);
                    new_plant_part.GetComponent<plant1>().two.transform.localScale = new Vector3(1, 0f, 1);
                }
            }
        }
    }

    public void LoadRootData(List<Vector3> data)
    {
        // TODO: Implement loading of root data to reconstruct the plant structure
    }

    public void add_data(Vector3 data)
    {
        root_plant_data.Add(data);
    }
}
