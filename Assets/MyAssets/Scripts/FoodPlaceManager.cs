using System.Collections.Generic;
using UnityEngine;

public class FoodPlaceManager : MonoBehaviour
{
    public Transform HelperPos;
    public int collectFoodCapacity;

    public string shelfFoodName;
    public Transform shelfTopTransform;

    [HideInInspector]
    public List<Food> collectedFoods;
    public List<Transform> shelfPos;

    public void MoveShelfTopTransform()
    {
        if(collectedFoods.Count < collectFoodCapacity)
            shelfTopTransform.position = shelfPos[collectedFoods.Count].position;
    }
}
