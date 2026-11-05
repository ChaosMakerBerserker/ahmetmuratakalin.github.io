using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 size = new Vector3(10, 10, 30);

    [Header("Gizmo")]
    public GameObject gizmoCubePrefab;
    private GameObject gizmoInstance;

    void Start()
    {
        if (gizmoCubePrefab != null)
        {
            gizmoInstance = Instantiate(gizmoCubePrefab, transform);
            gizmoInstance.transform.localPosition = new Vector3(0, 0, size.z / 2f); // Z ekseni boyunca taşı
            gizmoInstance.transform.localScale = size;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 topPosition = transform.position + new Vector3(0, 0, size.z / 2f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(topPosition, size);
    }
}