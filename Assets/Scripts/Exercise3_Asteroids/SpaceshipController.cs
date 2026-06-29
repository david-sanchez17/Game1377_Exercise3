/*
 * Assignment: AsteroidsGame - SpaceshipController Script - PART 1 & 2
 * 
 * Objective:
 * Implement a player controller for a spaceship in an Asteroids prototype. The player should be able to rotate the ship,
 * move forward, wrap around the screen, and shoot bullets. 
 * 
 * Requirements:
 * PART 1: Player Movement
 * 1. The player should be able to rotate the ship left and right using A/D keys from an input axis.
 *      This movement should be done with Transform based movement. 
 * 2. The player should be able to thrust forward using only the W key from an input axis
 *      This movement should be done with physics applied to a RigidBody2D. 
 * 3. The player should be able to wrap around the screen when they go off one edge and come back on the other side.
 * 4. The player should be able to teleport to a random location on the screen using left shift in an input button. You 
 *      do not need to check if there is an asteroid there. 
 *      Hint: For determining the random location, you can use the ScreenBounds class (see ScreenWrap.cs for how to use)
 *      
 * PART 2: Shooting
 * 1. The player should be able to shoot bullets using the space key in an input button
 *      Bullets should only go in the direction the ship is facing and bullet speed should be controlled by the Bullet.cs
 
 */

using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 500f;

    private float rotationInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");

        HandleRotation();
        HandleFire();
        HandleHyperspace();
        HandleScreenWrap();
    }

    void FixedUpdate()
    {
        HandleThrust();
    }

    private void HandleRotation()
    {
        transform.Rotate(0f, 0f, -rotationInput * rotationSpeed * Time.deltaTime);
    }

    private void HandleThrust()
    {
   
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.up * thrustForce * Time.fixedDeltaTime);
        }
    }

    private void HandleFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }
    }
    private void FireBullet()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Bullet Prefab is missing");
            return;
        }

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    private void HandleHyperspace()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TeleportToRandomLocation();
        }
    }

    private void TeleportToRandomLocation()
    {
        float randomX = Random.Range(ScreenBounds.ScreenLeft, ScreenBounds.ScreenRight);
        float randomY = Random.Range(ScreenBounds.ScreenBottom, ScreenBounds.ScreenTop);

        transform.position = new Vector2(randomX, randomY);
    }
    private void HandleScreenWrap()
    {
        Vector2 position = transform.position;

        if (position.x > ScreenBounds.ScreenRight)
        {
            position.x = ScreenBounds.ScreenLeft;
        }
        else if (position.x < ScreenBounds.ScreenLeft)
        {
            position.x = ScreenBounds.ScreenRight;
        }

        if (position.y > ScreenBounds.ScreenTop)
        {
            position.y = ScreenBounds.ScreenBottom;
        }
        else if (position.y < ScreenBounds.ScreenBottom)
        {
            position.y = ScreenBounds.ScreenTop;
        }

        transform.position = position;
    }
}