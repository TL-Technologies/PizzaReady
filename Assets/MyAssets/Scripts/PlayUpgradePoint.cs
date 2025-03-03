using UnityEngine;

public class PlayUpgradePoint : MonoBehaviour
{
    public GameObject PlayerUpgradePanel;

    public void OpenWindow()
    {
        PlayerUpgradePanel.SetActive(true);
    }

    public void CloseWindow()
    {
        PlayerUpgradePanel.SetActive(false);
    }
}
