using DG.Tweening;
using UnityEngine;

public class DoorSliding : MonoBehaviour
{
    public Transform doorR, doorL;
    public float duration;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Customer") || other.CompareTag("Player"))
        {
            if(doorR)
                doorR.DOLocalRotate(new Vector3(0, 90, 0), duration);
            if (doorL)
                doorL.DOLocalRotate(new Vector3(0, -90, 0), duration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Customer") || other.CompareTag("Player"))
        {
            if (doorR)
                doorR.DOLocalRotate(new Vector3(0, 0, 0), duration);
            if (doorL)
                doorL.DOLocalRotate(new Vector3(0, 0, 0), duration);
        }
    }
}
