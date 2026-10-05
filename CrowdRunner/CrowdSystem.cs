using System;
using UnityEngine;

public class CrowdSystem : MonoBehaviour
{
    [Header("Settings")]
    public GameObject runnerPrefab;
    public Transform runnerParent;
    public Transform target;
    public GameManager gameManager;

    int runnersPerRow = 8;

    [Header("Crowd Settings")]
    public int crowdCount = 2; // Başlangıçta 2 runner
    public float spacing = 1.5f;

    private void Awake()
    {
        crowdCount = PlayerPrefs.GetInt("LevelID", 1);
        
        UpdateRunners();
    }

    void Start()
    {
    }

    public void AddCrowd(int amount)
    {
        crowdCount += amount;
        
        if (crowdCount <= 0)
        {
            gameManager.LoseGame();
            return;
        }
        
        UpdateRunners();
    }

    private void UpdateRunners()
    {
        print("Crowd Güncelleniyor. Yeni Crowd Sayısı: " + crowdCount);
        if (runnerParent == null || runnerPrefab == null) return;

        // Önce var olan runner’ları temizle
        foreach (Transform child in runnerParent)
        {
            Destroy(child.gameObject);
        }

        // Yeni runner’ları oluştur
        for (int i = 0; i < crowdCount; i++)
        {
            int row = i / runnersPerRow;
            int col = i % runnersPerRow;

            float centerOffset = (crowdCount < runnersPerRow) ? (crowdCount - 1) / 2f : 3.5f;
            float xOffset = (col - centerOffset) * spacing;
            Vector3 positionOffset = new Vector3(xOffset, 0, -row * spacing);

            GameObject runnerObj = Instantiate(runnerPrefab, runnerParent);
            runnerObj.SetActive(false); // Pozisyonu ayarlamadan önce kapalı tut
            runnerObj.transform.localPosition = positionOffset;
            runnerObj.SetActive(true);

            RunnerFollow followScript = runnerObj.GetComponent<RunnerFollow>();
            if (followScript != null)
            {
                followScript.target = target;
                followScript.gameManager = gameManager;
            }
        }
    }

    public void RemoveCrowdList()
    {
        if (runnerParent == null) return;

        // Var olan runner’ları temizle
        foreach (Transform child in runnerParent)
        {
            Destroy(child.gameObject);
        }
    }

    public Vector3 GetTargetPosition()
    {
        return target != null ? target.position : runnerParent.position;
    }

    public int GetCrowdCount()
    {
        return crowdCount;
    }
}
