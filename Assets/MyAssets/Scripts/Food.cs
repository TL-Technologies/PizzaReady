using UnityEngine;
using RDG;
using DG.Tweening;

public class Food : MonoBehaviour
{
    private Transform foodCollectPos, targetPose;
    private float foodCollectPlayerYVal = .2f;
    private bool goToPlayer = true;
    public float speed, jumpPower;
    public string foodName;
    [HideInInspector]
    public bool isGarbageItem, goToCustomer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (goToPlayer)
            {
                if (other.gameObject.GetComponent<PlayerManager>())
                {
                    PlayerManager _PlayerManager = other.GetComponent<PlayerManager>();

                    if (_PlayerManager.isGarbageCollected && !isGarbageItem)
                        return;
                    if (isGarbageItem)
                    {
                        if(_PlayerManager.collectedFood.Count > 0)
                        {
                            if (!_PlayerManager.collectedFood[0].isGarbageItem)
                            {
                                return;
                            }
                        }
                    }

                    if (other.GetComponent<Helper>() && isGarbageItem)
                    {
                        return;
                    }

                    if (_PlayerManager.collectedFood.Count < _PlayerManager.maxFoodPlayerCarry)
                    {
                        if (!isGarbageItem)
                            transform.GetComponentInParent<FoodSpawner>().Spawn();
                        else
                        {
                            _PlayerManager.isGarbageCollected = true;
                            transform.GetComponentInParent<Chair>().isFull = false;
                        }

                        Vibration.Vibrate(30);
                        if(other.GetComponent<PlayerController>())
                           AudioManager.Instance.Play("FoodCollect");

                        transform.parent = other.transform;
                        _PlayerManager.collectedFood.Add(this);
                    }
                    else
                        return;

                }

                foodCollectPos = other.transform.GetChild(1).transform;
                targetPose = foodCollectPos;

                transform.DOLocalJump(targetPose.localPosition, jumpPower, 1, speed)
                .OnComplete(delegate ()
                {
                    this.transform.localPosition = foodCollectPos.localPosition;
                    foodCollectPos.position = new Vector3(foodCollectPos.transform.position.x, foodCollectPos.transform.position.y + foodCollectPlayerYVal, foodCollectPos.transform.position.z);
                });

                goToPlayer = false;
            }
        }
    }

    public void PlaceFood(Transform targetPos)
    {
        if(transform.parent)
        transform.parent = null;
        targetPose = targetPos;
        transform.DOJump(targetPose.position, 4, 1, .4f);
        foodCollectPos.position = new Vector3(foodCollectPos.transform.position.x, foodCollectPos.transform.position.y - foodCollectPlayerYVal, foodCollectPos.transform.position.z);
    }

    public void GotoCustomer(Transform target, Customer customer)
    {
        transform.DOJump(target.position, 4, 1, .4f)
        .OnComplete(delegate ()
        {
            goToCustomer = false;
            transform.parent = target;
            transform.position = target.position;
            customer.FoodColected();
        });
    }

    public void GotoCar(Transform target, Car car)
    {
        transform.DOJump(target.position, 4, 1, .4f)
        .OnComplete(delegate ()
        {
            goToCustomer = false;
            transform.parent = target;
            transform.position = target.position;
            car.FoodColected();
        });
    }

    public void GotoTrashBin(Transform target)
    {
        transform.DOJump(target.position, 4, 1, .4f)
        .OnComplete(delegate ()
        {
            Destroy(this.gameObject);
        });
    }
}
