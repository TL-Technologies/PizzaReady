using DG.Tweening;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform helperPos;
    public FoodPlaceManager yourAreaFoodPlaceManager;
    public Animator pizzaMachineAnim;
    public bool isCoffeeSpawner;

    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (isCoffeeSpawner)
            Invoke(nameof(SpawnCoffee), .5f);
        else
        {
            pizzaMachineAnim.Play("On");
            Invoke(nameof(SpawnPizza), .5f);
        }
    }

    private void SpawnPizza()
    {
        GameObject foodObj = Instantiate(foodPrefab, transform.position, transform.rotation, transform);
        foodObj.transform.DOLocalMove(new Vector3(1.39f, 0, 0), .4f).OnComplete(() =>
        {
            foodObj.GetComponent<BoxCollider>().enabled = true;
        });
    }

    private void SpawnCoffee()
    {
        Instantiate(foodPrefab, transform.position, transform.rotation, transform);
    }
}
