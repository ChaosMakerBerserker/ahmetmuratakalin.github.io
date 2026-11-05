using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour 
{
    [Header("Elements")]

    public DoorType doorType;

    public Color additionColor = Color.green;
    public Color differenceColor = Color.red;
    public Color multiplicationColor = Color.blue;
    public Color divisionColor = Color.yellow;

    public SpriteRenderer RightdoorSprite;
    public SpriteRenderer LeftdoorSprite;
    public TextMeshPro rightDoorText;
    public TextMeshPro leftDoorText;
    
    public bool isFirstDoor = false;

    [Header("Settings")]
    public BonusType rightDoorBonusType = BonusType.Addition;
    public BonusType leftDoorBonusType = BonusType.Addition;
    public int rightDoorBonusAmount = 10;
    public int leftDoorBonusAmount = 2;

    void Awake()
    {
        
    }

    void Start()
    {
        SetRandoms();
    }


    public void SetRandoms()
    {
        SetRandomBonuses();
        UpdateDoorTexts();
        UpdateDoorColors();
    }
    void SetRandomBonuses()
    {
        // Kapı bonus türlerini ve miktarlarını rastgele belirle
        if(isFirstDoor)
        {
            // Hint: Burada sağ ve sol kapıya rastgele bonus türleri atıyoruz ama Difference ya da Division türü atanmayacak!
            rightDoorBonusType = (BonusType)Random.Range(0, 2); // 0 veya 1 (Addition veya Multiplication)
            leftDoorBonusType = (BonusType)Random.Range(0, 2);  // 0 veya 1 (Addition veya Multiplication)
            rightDoorBonusAmount = GetRandomAmount(rightDoorBonusType);
            leftDoorBonusAmount = GetRandomAmount(leftDoorBonusType);
        }
        else
        {
            // LevelID değerine göre kapı bonus türlerini belirle. Ayrıca peş peşe aynı tür gelmemeli. En son Multiplication geldiyse sonrakinde gelmemeli.
            
            rightDoorBonusType = (BonusType)Random.Range(0, 4); // 0 ile 3 arasında (Addition, Difference, Multiplication, Division)
            leftDoorBonusType = (BonusType)Random.Range(0, 4);  // 0 ile 3 arasında (Addition, Difference, Multiplication, Division)
            rightDoorBonusAmount = GetRandomAmount(rightDoorBonusType);
            leftDoorBonusAmount = GetRandomAmount(leftDoorBonusType);
            
            // Aynı türün peş peşe gelmesini engelle
            if (rightDoorBonusType == leftDoorBonusType)
            {
                leftDoorBonusType = (BonusType)((((int)leftDoorBonusType) + 1) % 4); // Farklı bir tür yap
                leftDoorBonusAmount = GetRandomAmount(leftDoorBonusType);
            }
            if (rightDoorBonusType == BonusType.Multiplication)
            {
                leftDoorBonusType = (BonusType)Random.Range(0, 3); // 0 ile 2 arasında (Addition, Difference, Division)
                leftDoorBonusAmount = GetRandomAmount(leftDoorBonusType);
            }
            if (leftDoorBonusType == BonusType.Multiplication)
            {
                rightDoorBonusType = (BonusType)Random.Range(0, 3); // 0 ile 2 arasında (Addition, Difference, Division)
                rightDoorBonusAmount = GetRandomAmount(rightDoorBonusType);
            }
        }
    }

    void UpdateDoorColors()
    {
        if(doorType == DoorType.Right && RightdoorSprite != null)
        {
            switch (rightDoorBonusType)
            {
                case BonusType.Addition:
                    RightdoorSprite.color = additionColor;
                    break;
                case BonusType.Difference:
                    RightdoorSprite.color = differenceColor;
                    break;
                case BonusType.Multiplication:
                    RightdoorSprite.color = multiplicationColor;
                    break;
                case BonusType.Division:
                    RightdoorSprite.color = divisionColor;
                    break;
            }
        }
        if(doorType == DoorType.Left && LeftdoorSprite != null)
        {
            switch (leftDoorBonusType)
            {
                case BonusType.Addition:
                    LeftdoorSprite.color = additionColor;
                    break;
                case BonusType.Difference:
                    LeftdoorSprite.color = differenceColor;
                    break;
                case BonusType.Multiplication:
                    LeftdoorSprite.color = multiplicationColor;
                    break;
                case BonusType.Division:
                    LeftdoorSprite.color = divisionColor;
                    break;
            }
        }
    }

    int GetRandomAmount(BonusType type)
    {
        switch (type)
        {
            case BonusType.Addition:
            case BonusType.Difference:
                return Random.Range(1, 16); // 1 ile 15 arasında
            case BonusType.Multiplication:
            case BonusType.Division:
                return Random.Range(2, 4); // 2, 3
            default:
                return 1;
        }
    }

    private void UpdateDoorTexts()
    {
        if (doorType == DoorType.Right && rightDoorText != null)
        {
            rightDoorText.text = GetFormattedText(rightDoorBonusType, rightDoorBonusAmount);
        }
        if (doorType == DoorType.Left && leftDoorText != null)
        {
            leftDoorText.text = GetFormattedText(leftDoorBonusType, leftDoorBonusAmount);
        }
    }

    private string GetFormattedText(BonusType type, int amount)
    {
        switch (type)
        {
            case BonusType.Addition: return $"+{amount}";
            case BonusType.Difference: return $"-{amount}";
            case BonusType.Multiplication: return $"x{amount}";
            case BonusType.Division: return $"/{amount}";
            default: return amount.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("Player") == false) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null || player.crowdSystem == null) return;

        float playerX = other.transform.position.x;
        float middleX = transform.position.x;

        if (playerX >= middleX) // Sağ kapı
        {
            ApplyBonus(player, rightDoorBonusType, rightDoorBonusAmount);
            Debug.Log("Sağ kapı seçildi!");
        }
        else // Sol kapı
        {
            ApplyBonus(player, leftDoorBonusType, leftDoorBonusAmount);
            Debug.Log("Sol kapı seçildi!");
        }
    }

    private void ApplyBonus(PlayerController player, BonusType type, int amount)
    {
        switch (type)
        {
            case BonusType.Addition:
                player.crowdSystem.AddCrowd(amount);
                break;

            case BonusType.Multiplication:
                int current = player.crowdSystem.GetCrowdCount();
                player.crowdSystem.AddCrowd(current * (amount - 1));
                break;
            
            case BonusType.Difference:
                player.crowdSystem.AddCrowd(-amount);
                break;

            case BonusType.Division:
                int cur = player.crowdSystem.GetCrowdCount();
                if (amount != 0)
                {
                    // Tam bölme yaparak kalan kişiyi de ekleyelim. Örneğin 7 kişi / 2 = 3.5, yani 3 kişi kalmalı.
                    int newCount = cur / amount;
                    player.crowdSystem.AddCrowd(newCount - cur);
                }
                break;
        }
    }
}
