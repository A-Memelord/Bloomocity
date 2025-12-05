using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TMP_Text ItemCount;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = string.Empty;
    }
}
