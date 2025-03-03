using UnityEngine;

public class HelperUpgradePoint : MonoBehaviour
{
    public GameObject helperUpgradePanel;

    private void Start()
    {
        HelperSpawner helperSpawner = helperUpgradePanel.GetComponent<HelperSpawner>();
        GameObject helperPrefab = helperSpawner.helperPrefab;
        Transform helperSpawnPoint = helperSpawner.helperSpawnPoint;

        if (PlayerPrefs.HasKey("Helper"))
        {
            helperSpawner.helper = Instantiate(helperPrefab, helperSpawnPoint.position, helperSpawnPoint.rotation).GetComponent<Helper>();
        }
    }

    public void OpenWindow()
    {
        helperUpgradePanel.SetActive(true);
    }

    public void CloseWindow()
    {
        helperUpgradePanel.SetActive(false);
    }
}
