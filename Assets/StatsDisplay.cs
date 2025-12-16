using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    public TMP_Text moneyText;

    void Update()
    {
        if (moneyText.text != SaveDataController.Instance.CurrentData.Money.ToString())
        {
            moneyText.text = "Money: $" + SaveDataController.Instance.CurrentData.Money.ToString();
        }
    }
}
