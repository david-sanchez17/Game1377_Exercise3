using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        ExtraLife, 
        SpeedBoost,
        BigBullets
    }

    [Header("Power Up")]
    [SerializeField] private PowerUpType powerUpType;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatHeight;
    }

   
}
