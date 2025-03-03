using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameManager _GameManager;
    private bool removedAnyFood;
    public PlayerManager _PlayerManager;

    private void Start()
    { 
        _PlayerManager.maxFoodPlayerCarry = PlayerPrefs.GetInt("PlayerCapacityVal", _PlayerManager.maxFoodPlayerCarry);
        _GameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FoodPlaceCollider") && !_PlayerManager.isGarbageCollected)
        {
            FoodPlaceManager shelf = other.GetComponentInParent<FoodPlaceManager>();

            if (shelf.collectedFoods.Count < shelf.collectFoodCapacity)
            {
                int collectedFoodCount = _PlayerManager.collectedFood.Count - 1;

                if (collectedFoodCount >= 0)
                {
                    for (int i = _PlayerManager.collectedFood.Count - 1; i >= 0; i--)
                    {
                        if (_PlayerManager.collectedFood[i].foodName == shelf.shelfFoodName)
                        {
                            removedAnyFood = true;
                            _PlayerManager.collectedFood[i].PlaceFood(shelf.shelfTopTransform);
                            AudioManager.Instance.Play("FoodPlace");

                            shelf.collectedFoods.Add(_PlayerManager.collectedFood[i]);
                            _PlayerManager.collectedFood[i].transform.parent = shelf.transform;
                            shelf.MoveShelfTopTransform();

                            _PlayerManager.collectedFood[i].goToCustomer = true;
                            _PlayerManager.collectedFood.Remove(_PlayerManager.collectedFood[i]);
                            break;
                        }
                    }

                    if (removedAnyFood)
                    {
                        Transform foodCollectPos = _PlayerManager.foodCollectPos;

                        foodCollectPos.localPosition = _PlayerManager.initialFoodCollectPos;

                        foreach (Food food in _PlayerManager.collectedFood)
                        {
                            food.transform.localPosition = foodCollectPos.localPosition;
                            foodCollectPos.localPosition = new Vector3(foodCollectPos.transform.localPosition.x, foodCollectPos.transform.localPosition.y + .2f, foodCollectPos.transform.localPosition.z);
                        }

                        removedAnyFood = false;
                    }
                }
            }
        }

        if (other.CompareTag("MoneyCollider"))
        {
            BillingDesk billingDesk = other.transform.parent.GetComponentInChildren<BillingDesk>();

            if (billingDesk.money.Count > 0)
            {
                foreach (GameObject money in billingDesk.money)
                {
                    money.transform.DOJump(transform.position, 4, 1, .4f)
                    .OnComplete(delegate ()
                    {
                        _GameManager.AddMoney(5);
                        AudioManager.Instance.Play("MoneyCollect");
                        Destroy(money);
                    });
                }

                billingDesk.money = new List<GameObject>();
                billingDesk.moneyPosCount = 0;

                Vector3 vec = billingDesk.moneyPosParent.position;
                vec.y = 0;
                billingDesk.moneyPosParent.position = vec;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BuyPoint"))
            other.GetComponent<BuyPoint>().StartSpend();

        if (other.CompareTag("HelperSpawner"))
            other.GetComponent<HelperUpgradePoint>().OpenWindow();

        if (other.CompareTag("PlayerUpgradePoint"))
            other.GetComponent<PlayUpgradePoint>().OpenWindow();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BuyPoint"))
            other.GetComponent<BuyPoint>().StopSpend();

        if (other.CompareTag("HelperSpawner"))
            other.GetComponent<HelperUpgradePoint>().CloseWindow();

        if (other.CompareTag("PlayerUpgradePoint"))
            other.GetComponent<PlayUpgradePoint>().CloseWindow();
    }

    public void IncreasePlayerCapacity(int increaseVal)
    {
        _PlayerManager.maxFoodPlayerCarry += increaseVal;
        PlayerPrefs.SetInt("PlayerCapacityVal", _PlayerManager.maxFoodPlayerCarry);
    }
}
