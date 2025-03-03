using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform exitTransform;
    public Transform area;

    void Start()
    {
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        for (int i = 0; i < 5; i++)
        {
            int spawnTime = Random.Range(1, 4);
            SpawnCustomer();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, transform.position, transform.rotation, area);
        customer.GetComponent<Customer>().exitTransform = exitTransform;
        customer.GetComponent<Customer>().area = area;
    }
}
