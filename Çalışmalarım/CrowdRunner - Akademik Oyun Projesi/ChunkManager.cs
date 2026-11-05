using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject gizmoCubePrefab;
    public int chunkCount = 5;

    void Start()
    {
        for (int i = 0; i < chunkCount; i++)
        {
            Vector3 spawnPos = new Vector3(0, 0, i * 30f); // Z eksenine doğru spawn et

            Quaternion verticalRotation = Quaternion.identity; // Rotation gerek yok

            GameObject chunkInstance = Instantiate(chunkPrefab, spawnPos, verticalRotation, transform);

            Chunk chunkScript = chunkInstance.GetComponent<Chunk>();
            chunkScript.gizmoCubePrefab = gizmoCubePrefab;
        }
    }
}