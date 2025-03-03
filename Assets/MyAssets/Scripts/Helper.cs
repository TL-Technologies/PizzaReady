using UnityEngine;
using UnityEngine.AI;

public class Helper : MonoBehaviour
{
    public Transform foodCollectPos;
    private Vector3 initialFoodCollectPos;
    public Animator anim;
    private bool checkReachedFoodSpawner, checkReachedCounter, removedAnyFood;
    public NavMeshAgent agent;
    public ParticleSystem upgradeParticle;
    public PlayerManager _PlayerManager;
    private FoodSpawner foodSpawner;

    private void Start()
    {
        _PlayerManager.maxFoodPlayerCarry = PlayerPrefs.GetInt("CapacityVal", _PlayerManager.maxFoodPlayerCarry);
        agent.speed = PlayerPrefs.GetFloat("SpeedVal", agent.speed);
        initialFoodCollectPos = foodCollectPos.transform.localPosition;
        agent.updateRotation = true;
        FindFoodSpawner();
    }

    private void FindFoodSpawner()
    {
        FoodSpawner[] foodSpawners = FindObjectsOfType<FoodSpawner>();
        foodSpawner = foodSpawners[Random.Range(0, foodSpawners.Length)];

        Transform helperPos = foodSpawner.helperPos;
        agent.SetDestination(helperPos.position);
        checkReachedFoodSpawner = true;
    }

    private void GotoCounter()
    {
        Transform counterHelperPos = foodSpawner.yourAreaFoodPlaceManager.HelperPos;
        agent.SetDestination(counterHelperPos.position);
        checkReachedCounter = true;
    }

  
    private void Update()
    {    
        if (ReachedDestination() && checkReachedFoodSpawner)
        {
            if (_PlayerManager.collectedFood.Count == _PlayerManager.maxFoodPlayerCarry)
            {
                checkReachedFoodSpawner = false;
                GotoCounter();
            }
        }


        if (ReachedDestination() && checkReachedCounter)
        {
            if (_PlayerManager.collectedFood.Count == 0)
            {
                checkReachedCounter = false;
                FindFoodSpawner();
            }
        }

        if (_PlayerManager.collectedFood.Count > 0)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
                anim.Play("CarryIdle");
            else
                anim.Play("CarryRun");
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
                anim.Play("Idle");
            else
                anim.Play("Run");
        }
    }


    private bool ReachedDestination()
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("FoodPlaceCollider") && !_PlayerManager.isGarbageCollected)
        {
            FoodPlaceManager foodPlaceManager = other.GetComponentInParent<FoodPlaceManager>();

            if (foodPlaceManager.collectedFoods.Count < foodPlaceManager.collectFoodCapacity)
            {
                int collectedFoodCount = _PlayerManager.collectedFood.Count - 1;

                if (collectedFoodCount >= 0)
                {
                    for (int i = _PlayerManager.collectedFood.Count - 1; i >= 0; i--)
                    {
                        if (_PlayerManager.collectedFood[i].foodName == foodPlaceManager.shelfFoodName)
                        {
                            removedAnyFood = true;
                            _PlayerManager.collectedFood[i].PlaceFood(foodPlaceManager.shelfTopTransform);
                            AudioManager.Instance.Play("PlaceFood");

                            foodPlaceManager.collectedFoods.Add(_PlayerManager.collectedFood[i]);
                            _PlayerManager.collectedFood[i].transform.parent = foodPlaceManager.transform;
                            foodPlaceManager.MoveShelfTopTransform();

                            _PlayerManager.collectedFood[i].goToCustomer = true;
                            _PlayerManager.collectedFood.Remove(_PlayerManager.collectedFood[i]);
                            break;
                        }
                    }

                    if (removedAnyFood)
                    {
                        foodCollectPos.localPosition = initialFoodCollectPos;

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
    }

    public void IncreaseCapacity(int increaseVal)
    {
        _PlayerManager.maxFoodPlayerCarry += increaseVal;
        PlayerPrefs.SetInt("CapacityVal", _PlayerManager.maxFoodPlayerCarry);
    }

    public void IncreaseSpeed(int increaseVal)
    {
        agent.speed += increaseVal;
        PlayerPrefs.SetFloat("SpeedVal", agent.speed);
    }
}
