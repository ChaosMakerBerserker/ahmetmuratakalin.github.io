using UnityEngine;
using TMPro;

public class CrowdCounter : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshPro crowdCounterText;
    [SerializeField] private CrowdSystem crowdSystem; // CrowdSystem referansı

    private void Start()
    {
        if (crowdSystem == null)
            Debug.LogError("CrowdSystem reference is missing!");
    }

    private void Update()
    {
        if (crowdSystem != null && crowdCounterText != null)
        {
            // TMP text’i CrowdSystem’in crowdCount değerine göre güncelle
            crowdCounterText.text = crowdSystem.GetCrowdCount().ToString();
        }
    }
}