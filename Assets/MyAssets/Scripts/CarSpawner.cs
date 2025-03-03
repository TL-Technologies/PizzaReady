using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform exitTransform;
    public Transform area;

    void Start()
    {
        Invoke(nameof(StartSpawning), 1);
    }

    private void StartSpawning()
    {
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        for (int i = 0; i < 5; i++)
        {
            int spawnTime = Random.Range(1, 4);
            SpawnCar();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SpawnCar()
    {
        GameObject carObj = Instantiate(carPrefab, transform.position, transform.rotation, area);
        Car car = carObj.GetComponent<Car>();

        car.exitTransform = exitTransform;
        car.area = area;
    }
}
