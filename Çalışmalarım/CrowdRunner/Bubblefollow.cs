using UnityEngine;

public class BubbleFollow : MonoBehaviour
{
    [SerializeField] Transform target; // Balonun takip edeceği hedef
    private float fixedX;
    private float fixedY;

    void Start()
    {
        // Balonun x ve y pozisyonunu kaydet
        fixedX = transform.position.x;
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (target == null) return;

        // Balonun z pozisyonunu target ile aynı yap, x ve y sabit
        transform.position = new Vector3(fixedX, fixedY, target.position.z);
    }
}