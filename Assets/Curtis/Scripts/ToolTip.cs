using Unity.VisualScripting;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public string message;

    private void OnMouseOver()
    {
        if (NPCSystem.current)
        {
            ToolTipManager.Instance.SetAndShowToolTip(message);
        }
        else
        {
            ToolTipManager.Instance.HideToolTip();
        }
    }
}
