using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Random = System.Random;
using System.Security.Cryptography;

public class plant1 : MonoBehaviour
{
    public GameObject model;
    public GameObject armature;
    public GameObject plantpart;
    public GameObject stem;
    public GameObject one;
    public GameObject two;
    public GameObject three;

    public float length1;
    public float length2;

    public GameObject root;
    public GameObject holder;

    public int grow_count = 5;

    public int plantType = 1;
    public int seed;

    public bool is_root = true;

    public Random random = new();

    public float lifeTime;
    public GameObject plant_detail;

    public InventoryItemData plantValues;

    public GameObject MoneyPrefab;


    void Awake()
    {
        if (is_root)
        {
            root = this.gameObject;
            //add_data(transform.position);
        }
    }

    private void Update()
    {
       lifeTime += Time.deltaTime;
    }

    public IEnumerator Grow()
    {
        if (root.GetComponent<plant1>().grow_count != 0)
        {
            root.GetComponent<plant1>().grow_count -= 1;
            float difference = random.NextFloat(0.1f, 0.2f);
            float growth = random.NextFloat(0.5f, 0.8f);
            length1 = growth;

            for (float t = 0f; t < 1; t += Time.deltaTime * difference)
            {
                one.transform.localScale = new Vector3(1, Mathf.Lerp(0, growth, t), 1);
                yield return new WaitForEndOfFrame();
                if (root.GetComponent<plant1>().grow_count == 1 || root.GetComponent<plant1>().grow_count == 2)
                {
                    int rng = random.Next(1, 1000);
                    if (rng == 1)
                    {
                        GameObject new_plant_detail = Instantiate(plant_detail, one.transform.position, Quaternion.Euler(0, 0, 0), one.transform);
                        new_plant_detail.transform.SetParent(holder.transform);
                        new_plant_detail.GetComponent<plant_detail1>().stem.GetComponent<MeshRenderer>().material = stem.GetComponent<SkinnedMeshRenderer>().material;
                        new_plant_detail.transform.position = two.transform.position;
                        new_plant_detail.transform.localScale = Vector3.one;
                    }
                }
            }

            two.transform.rotation = Quaternion.Euler(random.NextFloat(-45, 45), random.NextFloat(-45, 45), random.NextFloat(-45, 45));
            float difference2 = random.NextFloat(0.1f, 0.2f);
            float growth2 = random.NextFloat(0.5f, 0.8f);
            length2 = growth2;
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
                if (root.GetComponent<plant1>().grow_count == 1 || root.GetComponent<plant1>().grow_count == 2)
                {
                    int rng = random.Next(1, 1000);
                    if (rng == 1)
                    {
                        GameObject new_plant_detail = Instantiate(plant_detail, one.transform.position, Quaternion.Euler(0, 0, 0), one.transform);
                        new_plant_detail.transform.SetParent(holder.transform);
                        new_plant_detail.GetComponent<plant_detail1>().stem.GetComponent<MeshRenderer>().material = stem.GetComponent<SkinnedMeshRenderer>().material;
                        new_plant_detail.transform.position = two.transform.position;
                        new_plant_detail.transform.localScale = Vector3.one;
                    }
                }
            }
            //root.GetComponent<plant1>().add_data(two.transform.position);

            if (root.GetComponent<plant1>().grow_count != 0)
            {
                int stems = random.Next(1, 5);
                while (stems > 0)
                {
                    stems--;
                    GameObject new_plant_part = Instantiate(plantpart, three.transform);
                    new_plant_part.GetComponent<plant1>().is_root = false;
                    new_plant_part.GetComponent<plant1>().root = root;
                    new_plant_part.GetComponent<plant1>().holder = holder;
                    new_plant_part.transform.SetParent(holder.transform);
                    new_plant_part.GetComponent<plant1>().model.transform.localScale = Vector3.one;
                    new_plant_part.GetComponent<plant1>().armature.transform.localScale = new Vector3(500f, 500f, 500f);
                    new_plant_part.transform.localScale = Vector3.one;
                    new_plant_part.transform.position = three.transform.position;
                    new_plant_part.transform.rotation = three.transform.rotation;
                    new_plant_part.GetComponent<plant1>().one.transform.localScale = new Vector3(1, 0.01f, 1);
                    new_plant_part.GetComponent<plant1>().two.transform.localScale = new Vector3(1, 0f, 1);
                    new_plant_part.GetComponent<plant1>().StartCoroutine(new_plant_part.GetComponent<plant1>().Grow());
                }
            }
            if (root.GetComponent<plant1>().grow_count == 0)
            {
                GameObject MoneyPre = Instantiate(MoneyPrefab, root.transform);
                MoneyPre.transform.GetComponent<MoneyPickup>().amount = (float)plantValues.value / 2f;
                print("Money spawned: " + MoneyPre.transform.GetComponent<MoneyPickup>().amount);
                MoneyPre.transform.SetParent(null);
            }
        }
    }

    // IF WE MAKE CHANGES TO THE ABOVE FUNCTION MAKE CHANGES TO THIS ONE TOO
    public void LoadRootData(float lifeTime, List<Vector3> data)
    {
        if (data[0] != null)
        {
            transform.position = data[0];
        }
        if (data[1] != null)
        {
            one.transform.localScale = new Vector3(1, data[1].x, 1);
        }
        if (data[2] != null)
        {
            two.transform.rotation = Quaternion.Euler(data[2]);
        }
        if (data[3] != null)
        {
            two.transform.localScale = new Vector3(1, data[3].x, 1);
        }
    }
}
