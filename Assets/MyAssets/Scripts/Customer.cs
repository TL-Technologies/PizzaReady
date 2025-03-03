using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using DG.Tweening;

public class Customer : MonoBehaviour
{
    public Transform[] foodPoses;
    private List<Food>collectedFoods = new List<Food>();
    private int buyFoodCapacity, foodPrice, foodPoseCount = 0;
    private bool goToBillingCounter, goingToChair, canCollect = true;
    [HideInInspector]
    public bool counterLook;
    private BillingDesk billingDesk;
    [HideInInspector]
    public Transform exitTransform, area, target;
    public GameObject moneyPrefab, garbagePrefab;
    public SkinnedMeshRenderer skin;
    [HideInInspector]
    public NavMeshAgent agent;
    public Animator anim;
    Vector3 targetShelfPos;
    public float eatingTime;
    private Chair chair;
    private GameManager gameManager;
    public List<GameObject> hats;
    private List<Chair> chairs = new List<Chair>();

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetFoodPrice();

        skin.material.color = gameManager.customerColors[Random.Range(0, gameManager.customerColors.Length)];
        hats[Random.Range(0, hats.Count)].SetActive(true);

        foreach (BillingDesk _billingDesk in area.GetComponentsInChildren<BillingDesk>())
        {
            if(!_billingDesk.isCarBillingDesk)
                billingDesk = _billingDesk;
        }

        buyFoodCapacity = Random.Range(1, 5);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;

        GoToBillingCounter();
        anim.Play("Walk");
    }

    public void SetFoodPrice()
    {
        foodPrice = gameManager.foodPrice;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            other.GetComponentInParent<CustomerSpawner>().SpawnCustomer();
            Destroy(gameObject);
        }
    }

    public void GoToBillingCounter()
    {
        goToBillingCounter = true;
        agent.updateRotation = true;
        agent.isStopped = false;
        billingDesk.customersForBilling.Add(this);
        billingDesk.ArrangeCustomersInQue();
    }

    public void PayMoney()
    {
        int val = buyFoodCapacity * foodPrice;
        billingDesk.GetComponent<AudioSource>().Play();

        for (int i = 0; i < val; i++) 
        {
            int index = billingDesk.moneyPosCount;

            GameObject money = Instantiate(moneyPrefab, transform.position, moneyPrefab.transform.rotation);

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

    private void OnTriggerStay(Collider other)
    {
        if (billingDesk.isPlayerOnCounter && other.CompareTag("CustomerCollider") && ReachedDestinationOrGaveUp())
        {
            FoodPlaceManager shelf = other.GetComponentInParent<FoodPlaceManager>();

            if (shelf.collectedFoods.Count > 0 && canCollect)
            {
                Food food = shelf.collectedFoods[shelf.collectedFoods.Count - 1];
                shelf.collectedFoods.Remove(food);
                shelf.MoveShelfTopTransform();

                food.GotoCustomer(foodPoses[foodPoseCount], this);
                collectedFoods.Add(food);

                foodPoseCount++;
                canCollect = false;
            }
        }
    }

    public void FoodColected()
    {
        if (collectedFoods.Count == buyFoodCapacity)
        {
            PayMoney();
            GotoChair();
            return;
        }
        canCollect = true;       
    }

    private void GotoChair()
    {
        chairs.AddRange(area.GetComponentsInChildren<Chair>());

        foreach (Chair _chair in chairs)
        {
            if (!_chair.isFull)
            {
                _chair.isFull = true;
                anim.Play("CarryWalk");
                agent.SetDestination(_chair.transform.position);
                chair = _chair;
                billingDesk.customersForBilling.Remove(this);
                billingDesk.ArrangeCustomersInQue();
                goingToChair = true;               
                return;
            }
        }

        print("Test");
        Invoke(nameof(GotoChair), 2);
    }
    public void Eat()
    {
        transform.rotation = chair.transform.rotation;
        anim.Play("SitAndEat");

        foreach (Food food in collectedFoods)
        {
            food.transform.DOJump(chair.foodPos.position, 4, 1, .4f);
        }

        Invoke("EatingTime", eatingTime);
    }

    private void EatingTime()
    {
        foreach (Food food in collectedFoods)
        {
            Destroy(food.gameObject);
        }

        Instantiate(garbagePrefab, chair.foodPos.position, Quaternion.identity, chair.transform);
        Invoke("GoToExit", 2);
    }

    public void GoToExit()
    {
        anim.Play("Walk");
        agent.SetDestination(exitTransform.position);
    }

    private void Update()
    {
        if (goingToChair && ReachedDestinationOrGaveUp())
        {
            goingToChair = false;
            Eat();
        }

        if (counterLook)
        {
            if (ReachedDestinationOrGaveUp())
            {
                anim.Play("Idle");
                transform.rotation = target.rotation;
                counterLook = false;
            }
        }
    }

    private bool ReachedDestinationOrGaveUp()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    return true;
            }
        }

        return false;
    }
}
