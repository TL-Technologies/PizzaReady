using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float distance;
    public float height;
    public float smoothness;

    public Transform camTarget;
    public Vector3 offset;

    Vector3 velocity;

    void LateUpdate()
    {
        if (!camTarget)
            return;

        Vector3 pos = Vector3.zero;
        pos.x = camTarget.position.x;
        pos.y = camTarget.position.y + height;
        pos.z = camTarget.position.z - distance;

        transform.position = Vector3.SmoothDamp(transform.position, pos+offset, ref velocity, smoothness);
    }
}
