using UnityEngine;
using System.Collections;

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

    public bool is_root = true;
    void Start()
    {
        if (is_root)
        {
            root = this.gameObject;
        }
        StartCoroutine(grow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator grow()
    {
        if (root.GetComponent<plant1>().grow_count != 0)
        {
            root.GetComponent<plant1>().grow_count -= 1;
            float difference = Random.Range(0.1f, 0.2f);
            float growth = Random.Range(0.5f, 0.8f);

            for (float t = 0f; t < 1; t += Time.deltaTime * difference)
            {
                one.transform.localScale = new Vector3(1, Mathf.Lerp(0, growth, t), 1);
                Debug.Log(Time.deltaTime * difference);
                yield return new WaitForEndOfFrame();
            }

            two.transform.rotation = Quaternion.Euler(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45));
            float difference2 = Random.Range(0.1f, 0.2f);
            float growth2 = Random.Range(0.5f, 0.8f);
            bool wall = false;
            while (wall == true)
            {
                wall = false;
                if (Physics.Raycast(two.transform.position, transform.forward * growth2, out RaycastHit hitInfo, growth2 * 2))
                {
                    wall = true;
                    two.transform.rotation = Quaternion.Euler(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45));
                }
            }


            for (float t = 0f; t < 1; t += Time.deltaTime * difference2)
            {
                two.transform.localScale = new Vector3(1, Mathf.Lerp(0, growth2, t), 1);
                yield return new WaitForEndOfFrame();
            }

            if (root.GetComponent<plant1>().grow_count != 0)
            {
                int stems = Random.Range(1, 2);
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
}
