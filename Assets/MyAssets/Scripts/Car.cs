using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    private BillingDesk billingDesk;
    private GameManager gameManager;
    public Transform foodPos;
    private int buyFoodCapacity, foodPrice;
    private bool canCollect = true;
    private List<Food> collectedFoods = new List<Food>();
    public GameObject moneyPrefab;
    public GameObject[] models;

    public float speed; // Speed of movement
    public float rotationSpeed; // Speed of rotation

    [HideInInspector]
    public Transform target, exitTransform, area;
    public NavMeshAgent agent;

    private void Start()
    {
        ActiveRandomModel();
        gameManager = FindObjectOfType<GameManager>();
        SetFoodPrice();

        foreach (BillingDesk _billingDesk in area.GetComponentsInChildren<BillingDesk>())
        {
            if (_billingDesk.isCarBillingDesk)
                billingDesk = _billingDesk;
        }

        buyFoodCapacity = Random.Range(1, 5);

        billingDesk.carsForBilling.Add(this);
        billingDesk.ArrangeCarsInQue();
    }

    private void ActiveRandomModel()
    {
        int index = Random.Range(0, models.Length);
        models[index].SetActive(true);
    }

    public void Goto(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (billingDesk.isPlayerOnCounter && other.CompareTag("CustomerCollider"))
        {
            FoodPlaceManager shelf = other.GetComponentInParent<FoodPlaceManager>();

            if (shelf.collectedFoods.Count > 0 && canCollect)
            {
                Food food = shelf.collectedFoods[shelf.collectedFoods.Count - 1];
                shelf.collectedFoods.Remove(food);
                shelf.MoveShelfTopTransform();

                food.GotoCar(foodPos, this);
                collectedFoods.Add(food);

                canCollect = false;
            }
        }
    }

    public void FoodColected()
    {
        if (collectedFoods.Count == buyFoodCapacity)
        {
            PayMoney();

            foreach (Food food in collectedFoods)
            {
                Destroy(food.gameObject);
            }
            GotoExit();
            return;
        }
        canCollect = true;
    }

    public void PayMoney()
    {
        int val = buyFoodCapacity * foodPrice;
        billingDesk.GetComponent<AudioSource>().Play();

        for (int i = 0; i < val; i++)
        {
            int index = billingDesk.moneyPosCount;

            GameObject money = Instantiate(moneyPrefab, transform.position, Quaternion.Euler(0,0,0));

            money.transform.DOJump(billingDesk.moneyPos[index].position, 4, 1, .4f)
            .OnComplete(delegate ()
            {
                billingDesk.money.Add(money);
            });

            if (billingDesk.moneyPosCount == 9)
            {
                billingDesk.moneyPosCount = 0;

                Vector3 vec = billingDesk.moneyPosParent.position;
                vec.y += .2f;

                billingDesk.moneyPosParent.position = vec;
            }
            else
                billingDesk.moneyPosCount++;
        }
    }

    private void GotoExit()
    {
        StartCoroutine(MoveToTarget(exitTransform.position));
        Destroy(agent);

        billingDesk.carsForBilling.Remove(this);
        billingDesk.ArrangeCustomersInQue();
        exitTransform.GetComponentInParent<CarSpawner>().SpawnCar();
    }

    IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Calculate the direction to the target
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Calculate the rotation towards the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the player is exactly at the target position
        transform.position = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
            Destroy(gameObject);
    }

    public void SetFoodPrice()
    {
        foodPrice = gameManager.foodPrice;
    }
}