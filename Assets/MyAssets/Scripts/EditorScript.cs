#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class EditorScript : EditorWindow
{

    public GameObject prefab;

    [MenuItem("Window/Prefab Preview")]
    public static void ShowWindow()
    {
        GetWindow<EditorScript>("Prefab Preview");
    }


    private void OnGUI()
    {
        if (GUILayout.Button("BuyPoint"))
        {
            prefab = Resources.Load<GameObject>("BuyPoint");
            InstantiatePrefab(prefab);
        }

        if (GUILayout.Button("Chair"))
        {
            prefab = Resources.Load<GameObject>("Chair");
            InstantiatePrefab(prefab);
        }

        if (GUILayout.Button("Sofa"))
        {
            prefab = Resources.Load<GameObject>("Sofa");
            InstantiatePrefab(prefab);
        }

        if (GUILayout.Button("DesebleBuyPointsObjects"))
        {
            DisebleObjects();
        }
        
        
        if (GUILayout.Button("EnableBuyPointsObjects"))
        {
            EnableObjects();
        }

        if (GUILayout.Button("SetBuyPointsSrNo"))
        {
            SetBuyPointsSrNo();
        }
    }

    int i = 0;
    private void SetBuyPointsSrNo()
    {
        foreach (BuyPoint buyPoint in FindObjectsOfType<BuyPoint>(true))
        {
            buyPoint.srNo = i;
            i++;
            EditorUtility.SetDirty(buyPoint);
        }
    }

    private void DisebleObjects()
    {
        foreach (BuyPoint buyPoint in FindObjectsOfType<BuyPoint>())
        {
            buyPoint.objectToUnlock.SetActive(false);

            if(buyPoint.destroyObj)
                buyPoint.destroyObj.SetActive(true);

            buyPoint.gameObject.SetActive(false);

            EditorUtility.SetDirty(buyPoint.objectToUnlock);
            EditorUtility.SetDirty(buyPoint);
        }
    }

    private void EnableObjects()
    {
        foreach (BuyPoint buyPoint in FindObjectsOfType<BuyPoint>(true))
        {
            buyPoint.objectToUnlock.SetActive(true);

            if (buyPoint.destroyObj)
                buyPoint.destroyObj.SetActive(false);

            buyPoint.gameObject.SetActive(true);

            EditorUtility.SetDirty(buyPoint.objectToUnlock);
            EditorUtility.SetDirty(buyPoint);
        }
    }

    private void InstantiatePrefab(GameObject prefab)
    {
        // Get the currently selected GameObject in the hierarchy
        GameObject selectedGameObject = Selection.activeGameObject;

        // Check if the prefab and selected GameObject are valid
        if (prefab != null && selectedGameObject != null)
        {
            Debug.Log(prefab.name);
            Debug.Log(prefab.name);

            // Instantiate the prefab at the selected GameObject's position and rotation and parent it
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab, selectedGameObject.transform.parent) as GameObject;
            if (instance == null)
            {
                Debug.LogError("Failed to instantiate prefab. Ensure the prefab is valid.");
                return;
            }

            Vector3 vec = selectedGameObject.transform.localPosition;
            Quaternion rot = selectedGameObject.transform.rotation;

            instance.transform.SetLocalPositionAndRotation(vec, rot);
            instance.name = prefab.name;

            DestroyImmediate(selectedGameObject);
            EditorUtility.SetDirty(instance);
        }
        else
        {
            Debug.LogWarning("Please select a valid prefab and a GameObject in the hierarchy.");
        }
    }
}
#endif