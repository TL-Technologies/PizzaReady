using System.Collections.Generic;
using UnityEngine;

public class BillingDesk : MonoBehaviour
{
    public Transform moneyPosParent;
    [HideInInspector]
    public List<Customer> customersForBilling;
    [HideInInspector]
    public List<Car> carsForBilling;
    public Transform[] billingQue;
    [HideInInspector]
    public List<GameObject> money;
    public List<Transform> moneyPos;
    [HideInInspector]
    public int moneyPosCount;
    [HideInInspector]
    public bool isPlayerOnCounter;
    public bool isCarBillingDesk;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerOnCounter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerOnCounter = false;
    }

    public void ArrangeCustomersInQue()
    {
        for (int i = 0; i < customersForBilling.Count; i++)
        {
            if (customersForBilling[i].target == null || customersForBilling[i].target != billingQue[i])
            {
                customersForBilling[i].agent.SetDestination(billingQue[i].position);
                customersForBilling[i].target = billingQue[i];
                customersForBilling[i].counterLook = true;
            }
        }
    }

    public void ArrangeCarsInQue()
    {
        for (int i = 0; i < carsForBilling.Count; i++)
        {
            if (carsForBilling[i].target == null || carsForBilling[i].target != billingQue[i])
            {
                carsForBilling[i].Goto(billingQue[i].position);
                carsForBilling[i].target = billingQue[i];
            }
        }
    }
}