using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant1_saver : MonoBehaviour
{
    public List<Vector3> plant_data;
    public GameObject plant1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(save_plant());
        //if (plant_data != null)
        //{
        //   GameObject new_plant1 = Instantiate(plant1, plant_data[0], Quaternion.Euler(plant_data[1]), transform);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator save_plant()
    {
        yield return new WaitForSeconds(10f);
        plant_data.Clear();
        foreach (Transform child in transform)
        {
            plant_data.Add(child.GetComponent<plant1>().transform.position);
            plant_data.Add(child.GetComponent<plant1>().transform.localEulerAngles);
            plant_data.Add(new Vector3(child.GetComponent<plant1>().length1, 0, 0));
            plant_data.Add(child.GetComponent<plant1>().two.transform.eulerAngles);
            plant_data.Add(new Vector3(child.GetComponent<plant1>().length2, 0, 0));
        }
        StartCoroutine(save_plant());
    }

    public void load_plant()
    {
        print(plant_data.Count / 5);
        for (int i = 0; i < plant_data.Count / 5; i++)
        {
            GameObject new_plant1 = Instantiate(plant1, plant_data[i * 5], Quaternion.Euler(plant_data[i * 5 + 1]), transform);
            new_plant1.GetComponent<plant1>().length1 = plant_data[i * 5 + 2].x;
            new_plant1.GetComponent<plant1>().two.transform.eulerAngles = plant_data[i * 5 + 3];
            new_plant1.GetComponent<plant1>().length2 = plant_data[i * 5 + 4].x;
        }
    }
}
