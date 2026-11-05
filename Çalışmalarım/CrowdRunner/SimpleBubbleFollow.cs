using UnityEngine;

public class SimpleBubbleFollow : MonoBehaviour
{
    public Transform target; // takip edilecek nesne
    public float offsetZ = 0f; // isteğe bağlı z offset

    private Vector3 fixedXY;

    void Start()
    {
        fixedXY = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Update()
    {
        if (target == null) return;
        transform.position = new Vector3(fixedXY.x, fixedXY.y, target.position.z + offsetZ);
    }
}