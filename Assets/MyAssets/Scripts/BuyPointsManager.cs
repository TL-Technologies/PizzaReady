using System.Collections.Generic;
using UnityEngine;

public class BuyPointsManager : MonoBehaviour
{
    public List<BuyPoint> buyPoints;
    private int index = 0;

    private void Start()
    {
        index = PlayerPrefs.GetInt("BuyPointsActiveIndex", index);

        for (int i = 0; i < index; i++)
        {
            buyPoints[i].gameObject.SetActive(true);
        }
    }

    public void UnlockNextBuyPoint()
    {
        if (index == buyPoints.Count)
            return;

        buyPoints[index].gameObject.SetActive(true);
        index++;
        PlayerPrefs.SetInt("BuyPointsActiveIndex", index);
    }
}
