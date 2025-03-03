using UnityEngine;
using RDG;
using TMPro;
using DG.Tweening;

public class BuyPoint : MonoBehaviour
{
    public int srNo, purchaseAmount;
    private GameManager gameManager;
    private float countAnimSpeed = 0.1f;
    private float animDuration = 0.5f;
    private TextMeshPro moneyAmountText;
    public GameObject objectToUnlock;
    public bool destroyWall;
    public GameObject destroyObj;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(srNo + "Unlocked"))
            UnlockObject();

        gameManager = FindObjectOfType<GameManager>();
        purchaseAmount = PlayerPrefs.GetInt(srNo+"PurchaseAmount", purchaseAmount);
        moneyAmountText = GetComponentInChildren<TextMeshPro>();

        ShowPurchaseAmount();
    }

    private void ShowPurchaseAmount()
    {
        moneyAmountText.text = purchaseAmount.ToString();
    }

    public void StartSpend()
    {
        countAnimSpeed = 0.08f;
        InvokeRepeating("Spend", countAnimSpeed, countAnimSpeed);
    }

    private void Spend()
    {
        if (gameManager.collectedMoney > 0)
        {       
            AudioManager.Instance.Play("BuyPoint");

            Vibration.Vibrate(20);
            purchaseAmount--;
            PlayerPrefs.SetInt(srNo + "PurchaseAmount", purchaseAmount);

            gameManager.LessMoney();
            ShowPurchaseAmount();

            if (purchaseAmount == 0)
            {
                PlayerPrefs.SetString(srNo + "Unlocked", "True");

                objectToUnlock.transform.DOPunchScale(new Vector3(0.1f, 1, 0.1f), animDuration, 7).OnComplete(() => Destroy(this.gameObject)); ;
                
                UnlockObject();

                if (gameObject.name != "WorkerBuyPoint")
                    FindObjectOfType<BuyPointsManager>().UnlockNextBuyPoint();

                CustomerSpawner customerSpawner = GetComponentInParent<Area>().GetComponentInChildren<CustomerSpawner>();
               
                    if(customerSpawner)
                    customerSpawner.SpawnCustomer();
              
                AudioManager.Instance.Play("Unlock");
                ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
                particle.transform.parent = null;
                particle.Play();
            }
        }
        else
        {
            CancelInvoke("Spend");
        }
    }

    private void UnlockObject()
    {
        if (destroyWall)
            Destroy(destroyObj);

        objectToUnlock.SetActive(true);
        DOTween.Kill(this.gameObject);      
        Destroy(this.gameObject);
    }

    public void StopSpend()
    {
        CancelInvoke("Spend");
    }
}
