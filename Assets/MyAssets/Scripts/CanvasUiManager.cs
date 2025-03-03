using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasUiManager : MonoBehaviour
{
    public Text collectedMoney;
    public GameObject dragToMoveWindow;
    public GameObject settingsPanel;

    private void Update()
    {
        if (Input.GetMouseButton(0) && dragToMoveWindow)
        {
            AudioManager.Instance.Play("Click");
            PlayerPrefs.SetString("DragWindow","");
            Destroy(dragToMoveWindow);
        }
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey("DragWindow"))
            Destroy(dragToMoveWindow);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("DragWindow");
    }

    public void SetMoneyText(int amount)
    {
        collectedMoney.text = "$" + amount.ToString();
    }

    public void Reload()
    {
        AudioManager.Instance.Play("Click");
        SceneManager.LoadScene(0);
    }

    public void GetRewardCash()
    {
        AudioManager.Instance.Play("Click");
    }

    public void OpenSettingsWindow()
    {
        AudioManager.Instance.Play("Click");
        settingsPanel.SetActive(true);
    }

}
