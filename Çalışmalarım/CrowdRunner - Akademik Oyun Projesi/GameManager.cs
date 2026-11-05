using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    public TextMeshProUGUI levelText;
    
    public GameObject DoorParent;
    public Transform DoorParentTransform;
    private int _doorCount = 1;
    
    public CrowdSystem crowdSystem;

    public GameObject FinishLineObject;

    public GameObject EnemyGameObject;
    
    public int levelID = 2;
    
    public bool gameEnded = true;
    public PlayerController playerController;
    public Doors[] doorsList = new Doors[0];

    private void Awake()
    {
        gameEnded = true;
    }

    void Start()
    {
        LoadLevel();
        
        gameEnded = false;
        if (GameOverPanel != null)
        {
            GameWinPanel.SetActive(false);
            GameOverPanel.SetActive(false);
        }
    }
    
    // PlayerPrefs ile level yükleme eğer yoksa 1. leveli yükle
    private void LoadLevel()
    {
        levelID = PlayerPrefs.GetInt("LevelID", 1);
        _doorCount = levelID + 1;
        print("Yüklenen Level: " + levelID + " | Kapı Sayısı: " + _doorCount);
        if (levelText != null)
        {
            levelText.text = "Level: " + levelID;
        }

        CreateDoors();
    }
    
    private void CreateEnemies(float finislineZPos)
    {
        // Finishline dan önce yan yana en fazla 8 kişi olacak şekilde düşman oluşturulacak. 8 sıra dolunca alt satıra geçilecek. 
        // levelID 3 ten sonra düşman oluşturulabilir. o level başlangıcında %70 ihtimalle düşman oluşturulacak, %30 ihtimalle oluşturulmayacak.
        // Düşmanlar FinishLine dan 20 birim önce başlayacak şekilde oluşturulacak. bu yüzden eğer düşman oluşacaksa Finishline 20 birim öncesinden başlamalı.
        // eğer düşman oluşturulmayacaksa FinishLine aynı kalacak.
        // Düşman sayısı levelID ye bağlı olarak artacak, levelID ile 1 arasında rastgele düşman oluşturulacak. 
        if (levelID < 3) return;
        
        // önceki enemeyler temizlenecek. EnemyGameObject hariç
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy != null && enemy != EnemyGameObject)
                Destroy(enemy);
        }
        
        int enemyCount = Random.Range(1, levelID * 2); // 1 ile levelID arasında rastgele düşman sayısı
        print("Oluşturulan Düşman Sayısı: " + enemyCount);
        float finishLineZ = FinishLineObject.transform.position.z;
        float enemyStartZ = finishLineZ - 20f; // Düşmanlar FinishLine dan 20 birim önce başlayacak
        float spacing = 1.5f; // Düşmanlar arasındaki boşluk
        int enemiesPerRow = 8; // Her satırda en fazla 8 düşman
        int currentRow = 0;
        int currentCol = 0;
        for (int i = 0; i < enemyCount; i++)
        {
            // düşmanlar arası hem z hem z ekesininde spacing kadar boşluk olacak şekilde yerleştirilecek.
            float xPosition = (currentCol - (enemiesPerRow / 2f - 0.5f)) * spacing; // Ortalamak için (enemiesPerRow/2 - 0.5)
            float zPosition = enemyStartZ - (currentRow * spacing);
            Vector3 enemyPosition = new Vector3(xPosition, 1, zPosition);
            Instantiate(EnemyGameObject, enemyPosition, EnemyGameObject.transform.rotation);
            currentCol++;
            if (currentCol >= enemiesPerRow)
            {
                currentCol = 0;
                currentRow++;
            }
        }
    }
    
    private void CreateDoors()
    {
        if (DoorParent == null) return;
        
        // Önce var olan kapıları temizle
        foreach (var door in doorsList)
        {
            if (door != null)
                Destroy(door.gameObject);
        }

        foreach (var door in DoorParentTransform)
        {
            Destroy(((Transform)door).gameObject);
        }
        
        // Doors Objelerinin Parentı gameobject olan DoorParent leri oluşturulacak. İçerisinden Doors componentleri alınacak ve doorsList e atanacak.
        // Her DoorParent arasında z ekseninde 20 birim boşluk olacak. İlk DoorObject z=40 da olacak.
        
        doorsList = new Doors[_doorCount * 2];
        
        print("Kapı Sayısı: " + doorsList.Length + " | Kapı Parent Sayısı: " + _doorCount);
        
        for (int i = 0; i < _doorCount; i++)
        {
            GameObject doorObj = Instantiate(DoorParent, new Vector3(0, 0, 40 + i * 20), Quaternion.identity);
            doorObj.transform.SetParent(DoorParentTransform);
            // hem sağ hem sol kapı için Doors componenti alınacak.
            Doors[] doorComponents = doorObj.GetComponentsInChildren<Doors>();
            
            if (doorComponents.Length >= 2)
            {
                doorsList[i * 2] = doorComponents[0]; // Sol kapı
                doorsList[i * 2 + 1] = doorComponents[1]; // Sağ kapı
                
                // İlk kapı setrandoms da farkı türü atamayacak.
                if(i == 0)
                {
                    doorsList[i * 2].isFirstDoor = true;
                    doorsList[i * 2 + 1].isFirstDoor = true;
                }
            }
        }
        
        // FinishLine ın pozisyonu son kapının biraz ilerisinde olacak. burada CreateEnemies i çağırabiliriz çünkü düşmanlar finishline dan önce oluşturulacak.
        // FinishLine 20 birim sonra başlayacak şekilde ayarlanacak. Eğer düşman oluşturulacaksa finishline 20 birim daha geride olacak.
        // %70 ihtimalle düşman oluşturulacak, %30 ihtimalle oluşturulmayacak.
        
        if (Random.value <= 0.7f)
        {
            FinishLineObject.transform.position = new Vector3(0, 0, 40 + (_doorCount + 2) * 20); // Son kapının 40 birim ilerisinde
            CreateEnemies(FinishLineObject.transform.position.z);
        }
        else
        {
            FinishLineObject.transform.position = new Vector3(0, 0, 40 + (_doorCount + 1) * 20);
        }
    }

    public void LoseGame()
    {
        gameEnded = true;
        Debug.Log("Oyun bitti!");
        if (GameOverPanel != null)
        {
            GameWinPanel.SetActive(false);
            GameOverPanel.SetActive(true);
        }
    }
    
    public void WinGame()
    {
        gameEnded = true;
        Debug.Log("Oyunu kazandınız!");
        if (GameWinPanel != null)
        {
            GameOverPanel.SetActive(false);
            GameWinPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        
        crowdSystem.RemoveCrowdList();
        crowdSystem.crowdCount = 0;
        crowdSystem.crowdCount = levelID;
        
        gameEnded = false;
        Debug.Log("Oyun yeniden başladı!");
        GameOverPanel.SetActive(false);
        playerController.ResetPosition();
        foreach (var door in doorsList)
        {
            door.SetRandoms();
        }
    }

    public void NextLevel()
    {
        crowdSystem.RemoveCrowdList();
        crowdSystem.crowdCount = 0;
        crowdSystem.crowdCount = levelID + 1;
        
        levelID++;
        PlayerPrefs.SetInt("LevelID", levelID);
        if (levelText != null)
            levelText.text = "Level: " + levelID;

        playerController.ResetPosition();
        
        _doorCount = levelID + 1;
        CreateDoors();
        
        foreach (var door in doorsList)
        {
            door.SetRandoms();
        }
        gameEnded = false;
        Debug.Log("Sonraki seviyeye geçildi!");
        if (GameWinPanel != null)
            GameWinPanel.SetActive(false);
    }
}