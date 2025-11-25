using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject creditsBackground;

    public GameObject[] creditPanels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCredits()
    {
        creditsBackground.SetActive(!creditsBackground.activeSelf);
    }

    public void SelectPanel(string name)
    {
        for (int i = 0; i < creditPanels.Length; i++)
        {
            print(creditPanels[i].name);
            print(name);
            if (creditPanels[i].name == name)
            {
                if (!creditPanels[i].activeSelf)
                {
                    creditPanels[i].SetActive(true);
                }
                else
                {
                    creditPanels[i].SetActive(false);
                }
                    
            }
            else
            {
                creditPanels[i].SetActive(false);
            }
        }
    }
}
