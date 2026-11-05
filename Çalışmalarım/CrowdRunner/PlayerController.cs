using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CrowdSystem crowdSystem;   // CrowdSystem referansı

    public GameManager gameManager; // GameManager referansı

    [Header("Movement Settings")]
    public float speed = 10f;
    public float turnSpeed = 10f;
    public float slideSpeed = 5f;
    public float roadWidth = 10f;

    private Vector3 clickedScreenPosition;
    private Vector3 clickedPlayerPosition;
    private Vector3 targetPositionX;

    private Vector3 initialPosition;

    void Start()
    {
        targetPositionX = transform.position;
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (gameManager != null && gameManager.gameEnded) return;
        HandleSlide();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (crowdSystem == null) return;

        Vector3 targetPos = crowdSystem.GetTargetPosition();
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        float moveZ = speed * Time.deltaTime;
        float newZ = transform.position.z + moveZ;

        transform.position = new Vector3(targetPositionX.x, transform.position.y, newZ);
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        targetPositionX = initialPosition;
    }

    private void HandleSlide()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedScreenPosition = Input.mousePosition;
            clickedPlayerPosition = transform.position;
            targetPositionX = clickedPlayerPosition;
        }
        else if (Input.GetMouseButton(0))
        {
            float xDiff = (Input.mousePosition.x - clickedScreenPosition.x) / Screen.width * slideSpeed;
            targetPositionX.x = Mathf.Clamp(clickedPlayerPosition.x + xDiff, -roadWidth / 2, roadWidth / 2);
        }

        targetPositionX.x = Mathf.Lerp(transform.position.x, targetPositionX.x, 0.2f);
    }

    // Düşmanla çarpışma – düşmanı yok et (runner/enemy mantığına uyumlu)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player: Düşmana çarptı, düşman yok ediliyor!");
            Destroy(collision.gameObject); // Düşmanı yok et
        }
    }
}