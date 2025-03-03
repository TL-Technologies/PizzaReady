using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HelperSpawner : MonoBehaviour
{
    public Button helperCapacityBtn, helperBuybtn, helperSpeedBtn;
    public Text capacityBuyValText, speedBuyValText, helperBuyValText;
    public int moneyIncreaseVal, increaseCapacityVal, increaseSpeedVal;
    private int capacityBuyVal, speedBuyValue, helperBuyValue;
    private GameManager _GameManager;
    public GameObject capacityFullText, speedFullText;
    [HideInInspector]
    public Helper helper;
    public GameObject helperPrefab;
    public Transform helperSpawnPoint;
    public float maxSpeed, maxCapacity;

    private void OnEnable()
    {
        _GameManager = FindObjectOfType<GameManager>();
        UpdateBuyAmountsText();
        CheckButtonsActive();
    }

    public void BuyHelper()
    {

        AudioManager.Instance.Play("Upgrade");
        _GameManager.LessMoneyinBulk(helperBuyValue);

        helper = Instantiate(helperPrefab, helperSpawnPoint.position, helperSpawnPoint.rotation).GetComponent<Helper>();

        PlayerPrefs.SetString("Helper", "");

        helperBuybtn.transform.parent.gameObject.SetActive(false);
        helperCapacityBtn.transform.parent.gameObject.SetActive(true);
        helperSpeedBtn.transform.parent.gameObject.SetActive(true);
    }

    public void BuyCapacity()
    {
        AudioManager.Instance.Play("Upgrade");
        _GameManager.LessMoneyinBulk(capacityBuyVal);

        capacityBuyVal += moneyIncreaseVal;
        PlayerPrefs.SetInt("HelperCapacityBuyVal", capacityBuyVal);

        UpdateBuyAmountsText();
        helper.IncreaseCapacity(increaseCapacityVal);

        if (helper._PlayerManager.maxFoodPlayerCarry == maxCapacity)
            PlayerPrefs.SetString("HelperCapacityFull", "");

        CheckButtonsActive();
        helper.upgradeParticle.Play();
    }

    public void BuySpeed()
    {
      

        AudioManager.Instance.Play("Upgrade");

        _GameManager.LessMoneyinBulk(speedBuyValue);

        speedBuyValue += moneyIncreaseVal;
        PlayerPrefs.SetInt("HelperSpeedBuyVal", speedBuyValue);

        UpdateBuyAmountsText();
        helper.IncreaseSpeed(increaseSpeedVal);

        if (helper.gameObject.GetComponent<NavMeshAgent>().speed == maxSpeed)
            PlayerPrefs.SetString("HelperSpeedFull", "");

        CheckButtonsActive();
        helper.upgradeParticle.Play();
    }

    private void UpdateBuyAmountsText()
    {       
        helperBuyValue = moneyIncreaseVal;
        helperBuyValText.text = moneyIncreaseVal.ToString();

        capacityBuyVal = PlayerPrefs.GetInt("HelperCapacityBuyVal", moneyIncreaseVal);
        capacityBuyValText.text = capacityBuyVal.ToString();

        speedBuyValue = PlayerPrefs.GetInt("HelperSpeedBuyVal", moneyIncreaseVal);
        speedBuyValText.text = speedBuyValue.ToString();
    }

    private void CheckButtonsActive()
    {      
        if (PlayerPrefs.HasKey("Helper"))
            helperBuybtn.transform.parent.gameObject.SetActive(false);
        else
        {
            if (helperBuyValue <= _GameManager.collectedMoney)
                helperBuybtn.interactable = true;
            else
                helperBuybtn.interactable = false;

            helperCapacityBtn.transform.parent.gameObject.SetActive(false);
            helperSpeedBtn.transform.parent.gameObject.SetActive(false);
        }

        if (capacityBuyVal <= _GameManager.collectedMoney)
            helperCapacityBtn.interactable = true;
        else
            helperCapacityBtn.interactable = false;


        if (speedBuyValue <= _GameManager.collectedMoney)
            helperSpeedBtn.interactable = true;
        else
            helperSpeedBtn.interactable = false;


        if (PlayerPrefs.HasKey("HelperCapacityFull"))
        {
            capacityFullText.SetActive(true);
            helperCapacityBtn.interactable = false;
        }

        if (PlayerPrefs.HasKey("HelperSpeedFull"))
        {
            speedFullText.SetActive(true);
            helperSpeedBtn.interactable = false;
        }
    }
}
