using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeManager : MonoBehaviour
{
    private GameManager _GameManager;
    public Text capacityBuyValText, speedBuyValText, profitBuyValText;
    public GameObject capacityFullText, speedFullText, profitFullText;
    public Button playerCapacityBtn, profitsUpBtn, playerSpeedBtn;
    private int capacityBuyVal, speedBuyValue, profitBuyValue;
    public int moneyIncreaseVal, increaseCapacityVal, increaseSpeedVal, increaseFoodPriceVal;
    public PlayerManager playerManager;
    public ParticleSystem upgradeParticle;
    public PlayerController playerController;
    public float maxSpeed, maxCapacity, maxFoodPrice;

    private void OnEnable()
    {
        _GameManager = FindObjectOfType<GameManager>();
        UpdateBuyAmountsText();
        CheckButtonsActive();
    }

    private void UpdateBuyAmountsText()
    {     
        capacityBuyVal = PlayerPrefs.GetInt("PlayerCapacityBuyVal", moneyIncreaseVal);
        capacityBuyValText.text = capacityBuyVal.ToString();

        speedBuyValue = PlayerPrefs.GetInt("PlayerSpeedBuyVal", moneyIncreaseVal);
        speedBuyValText.text = speedBuyValue.ToString();

        profitBuyValue = PlayerPrefs.GetInt("ProfitBuyVal", moneyIncreaseVal);
        profitBuyValText.text = profitBuyValue.ToString();
    }

    private void CheckButtonsActive()
    {       
        if (capacityBuyVal <= _GameManager.collectedMoney)
            playerCapacityBtn.interactable = true;
        else
            playerCapacityBtn.interactable = false;


        if (speedBuyValue <= _GameManager.collectedMoney)
            playerSpeedBtn.interactable = true;
        else
            playerSpeedBtn.interactable = false;


        if (profitBuyValue <= _GameManager.collectedMoney)
            profitsUpBtn.interactable = true;
        else
            profitsUpBtn.interactable = false;


        if (PlayerPrefs.HasKey("PlayerCapacityFull"))
        {
            capacityFullText.SetActive(true);
            playerCapacityBtn.interactable = false;
        }

        if (PlayerPrefs.HasKey("PlayerSpeedFull"))
        {
            speedFullText.SetActive(true);
            playerSpeedBtn.interactable = false;
        }

        if (PlayerPrefs.HasKey("ProfitFull"))
        {
            profitFullText.SetActive(true);
            profitsUpBtn.interactable = false;
        }
    }

    public void BuyCapacity()
    {
        MyAdManager.Instance.ShowInterstitialAd();
        AudioManager.Instance.Play("Upgrade");
        _GameManager.LessMoneyinBulk(capacityBuyVal);

        capacityBuyVal += moneyIncreaseVal;
        PlayerPrefs.SetInt("PlayerCapacityBuyVal", capacityBuyVal);

        UpdateBuyAmountsText();
        FindObjectOfType<Player>().IncreasePlayerCapacity(increaseCapacityVal);

        if (playerManager.maxFoodPlayerCarry == maxCapacity)
            PlayerPrefs.SetString("PlayerCapacityFull", "");

        CheckButtonsActive();
        upgradeParticle.Play();
    }

    public void BuySpeed()
    {
        MyAdManager.Instance.ShowInterstitialAd();
        AudioManager.Instance.Play("Upgrade");
        _GameManager.LessMoneyinBulk(speedBuyValue);
        speedBuyValue += moneyIncreaseVal;
        PlayerPrefs.SetInt("PlayerSpeedBuyVal", speedBuyValue);

        UpdateBuyAmountsText();
        playerController.IncreaseSpeed(increaseSpeedVal);

        if (playerController.speed == maxSpeed)
            PlayerPrefs.SetString("PlayerSpeedFull", "");

        CheckButtonsActive();
        upgradeParticle.Play();
    }

    public void ProfitsUp()
    {
        MyAdManager.Instance.ShowInterstitialAd();
        AudioManager.Instance.Play("Upgrade");
        _GameManager.LessMoneyinBulk(profitBuyValue);
        profitBuyValue += moneyIncreaseVal;
        PlayerPrefs.SetInt("ProfitBuyVal", profitBuyValue);

        UpdateBuyAmountsText();

        _GameManager.IncreaseFoodPrice(increaseFoodPriceVal);

        foreach (Customer customer in FindObjectsOfType<Customer>())
        {
            customer.SetFoodPrice();
        }

        foreach (Car car in FindObjectsOfType<Car>())
        {
            car.SetFoodPrice();
        }

        if (_GameManager.foodPrice == maxFoodPrice)
            PlayerPrefs.SetString("ProfitFull", "");

        CheckButtonsActive();
        upgradeParticle.Play();
    }
}
