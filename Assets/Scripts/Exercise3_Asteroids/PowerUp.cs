using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        ExtraLife, 
        SpeedBoost,
        BigBullets
    }

    [SerializeField] private PowerUpType powerUpType;

    private void OnTrigger2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        AsteroidsPlayerController player = other.GetComponent<AsteroidsPlayerController>();

        if (player == null)
            return;

        switch (powerUpType)
        {
            case PowerUpType.ExtraLife:
                player.AddLife();
                break;

            case PowerUpType.SpeedBoost:
                player.AddLife();
                break;

            case PowerUpType.BigBullets:
                player.ActivateBigBullets();
                break;
        }
        Destroy(gameObject);
    }
}
