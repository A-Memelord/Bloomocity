using UnityEngine;
using System.Collections;

public class plant1 : MonoBehaviour
{
    public GameObject plantpart;
    public GameObject one;
    public GameObject two;
    public GameObject three;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(grow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator grow()
    {
        two.transform.Rotate(new Vector3(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45)));

        float difference = Random.Range(1f, 2f);
        for (float t = 0f; t < 1; t += Time.deltaTime * difference)
        {
            one.transform.localScale = new Vector3(1, Mathf.Lerp(one.transform.localScale.y, difference, t), 1);
            yield return new WaitForEndOfFrame();
        }
        one.transform.localScale = new Vector3(1, difference, 3);

        float difference2 = Random.Range(1f, 2f);
        for (float t = 0f; t < 1; t += Time.deltaTime * difference2)
        {
            two.transform.localScale = new Vector3(1, Mathf.Lerp(two.transform.localScale.y, difference2, t), 1);
            yield return new WaitForEndOfFrame();
        }
        //two.transform.localScale = new Vector3(1, final_two_scale, 1);

        int stems = Random.Range(1, 3);
        while (stems > 0)
        {
            stems--;
            GameObject new_plant_part = Instantiate(plantpart, three.transform);
            new_plant_part.transform.SetParent(null);
            new_plant_part.transform.localScale = new Vector3(1, 1, 1);
            new_plant_part.transform.rotation = three.transform.rotation;
            new_plant_part.transform.position = three.transform.position;
        }
    }
}
