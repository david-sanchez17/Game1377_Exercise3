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

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidsPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float thrustForce = 500f;

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireCooldown = 0.25f;

    [Header("Lives")]
    [SerializeField] private int lives = 2;
    [SerializeField] private float respawnInvincibility = 3f;

    [Header("Power Up Durations")]
    [SerializeField] private float speedBoostDuration = 5f;
    [SerializeField] private float bigBulletDuration = 5f;

    [Header("Effects")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hyperspaceSound;

    private AudioSource audioSource;

    private float rotationInput;
    private float nextFireTime;
 
    private bool isInvincible = false;

    private float normalRotationSpeed;
    private float normalThrustForce;

    private float bulletScale = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        normalRotationSpeed = rotationSpeed;
        normalThrustForce = thrustForce;
    }

    void Update()
    {
        rotationInput = Input.GetAxis("Horizontal");

        HandleRotation();
        HandleFire();
        HandleHyperspace();
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
   
        if (Input.GetButtonDown("Thrust"))
        {
            rb.AddForce(transform.up * thrustForce * Time.fixedDeltaTime);
            if (animator != null)
                animator.SetBool("Thrust", true);
            if (!audioSource.isPlaying && thrustSound != null)
                audioSource.PlayOneShot(thrustSound);
        }
        else
        {
            if (animator != null)
                animator.SetBool("Thrust", false);
        }
    }

    private void HandleFire()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            FireBullet();
        }
    }
    private void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.localScale *= bulletScale;

        if (animator != null)
            animator.SetTrigger("Fire");
        if (fireSound != null)
            audioSource.PlayOneShot(fireSound);
    }
    
    private void HandleHyperspace()
    {
        if (Input.GetButtonDown("Hyperspace"))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("YOU DIED");
        //Destroys player
        Destroy(gameObject);
    }

    private void LoseLife()
    {
        lives--;
            if (lives > 0)
        {
            transform.position = Vector3.zero;
            StartCoroutine(RespawnInvincibility());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator RespawnInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(respawnInvincibility);
        isInvincible = false;
    }
    
}