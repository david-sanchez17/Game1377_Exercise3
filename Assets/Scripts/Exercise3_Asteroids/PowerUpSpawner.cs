using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power Ups")]
    [SerializeField] private GameObject[] powerUpPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private int maxPowerUps = 3;

    private int currentPowerUps;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), spawnInterval, spawnInterval);
    }

    private void SpawnPowerUp()
    {
        if (currentPowerUps >= maxPowerUps)
            return;
        float randomX = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomY = Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop);

        Vector2 spawnPosition = new Vector2(randomX, randomY);
        GameObject powerUp = Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)], spawnPosition, Quaternion.identity);

        currentPowerUps++;
        Destroy(powerUp, 10f);
    }

    public void PowerUpDestroyed()
    {
        currentPowerUps--;
    }
}
