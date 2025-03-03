using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int collectedMoney;
    private CanvasUiManager _CanvasUiManager;
    public Color[] customerColors;
    public int foodPrice;


    [ContextMenu("sss")]
    public void Save()
    {
        PlayerPrefs.SetInt("MoneyAmount", 10000);
    } 
    
    [ContextMenu("ddd")]
    public void del()
    {
        PlayerPrefs.DeleteAll();
    }
    private void Awake()
    {
        foodPrice = PlayerPrefs.GetInt("FoodPrice", foodPrice);
    }

    private void Start()
    {
        _CanvasUiManager = FindObjectOfType<CanvasUiManager>();
        collectedMoney = PlayerPrefs.GetInt("MoneyAmount", 0);
        _CanvasUiManager.SetMoneyText(collectedMoney);

        if (!PlayerPrefs.HasKey("StartMoney"))
        {
            PlayerPrefs.SetString("StartMoney", "");
            AddMoney(500);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayerPrefs.DeleteAll();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.M))
            AddMoney(500);
    }

    public void AddMoney(int amount)
    {
        collectedMoney += amount;
        ShowAndSave();
    }

    public void LessMoney()
    {
        collectedMoney--;
        ShowAndSave();
    }

    public void LessMoneyinBulk(int amount)
    {
        collectedMoney -= amount;
        ShowAndSave();
    }

    public void ShowAndSave()
    {
        _CanvasUiManager.SetMoneyText(collectedMoney);
        PlayerPrefs.SetInt("MoneyAmount", collectedMoney);
    }

    public void IncreaseFoodPrice(int increaseVal)
    {
        foodPrice += increaseVal;
        PlayerPrefs.SetInt("FoodPrice", foodPrice);
    }
}