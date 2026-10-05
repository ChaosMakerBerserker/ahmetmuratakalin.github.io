using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}