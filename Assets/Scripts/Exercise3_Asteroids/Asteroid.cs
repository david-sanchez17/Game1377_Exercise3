/*
 * Assignment: Asteroids Game - Asteroid Script - PART 2
 * 
 * Objective: Create a functional asteroid script. This script will be responsible for the functionality of the asteroids.
 * this should include initial velocity, angular velocity, and breaking into smaller asteroids when destroyed.
 * Remember, asteroids should only spawn through the AsteroidSpawner script. 
 
* Requirements:
* 1. The asteroid should start with a constant speed but a random angular velocity. Both of these are set in the Rigidbody2D
*       The movement direction of the asteroid should not change. 
*       Hint: All movement for the asteroid should be done via a Rigidbody2D and should be able to be set at Start.
* 2. When the asteroid is destroyed, it should spawn two smaller asteroids if it is not already the smallest size. 
*       Hint: How can you use a function to set the AsteroidSpawner variable from a different script?
* 3. When the astroid hits the player, it should destroy the player. 
*/

using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum AsteroidSize { Small, Medium, Large }

    [Header("Asteroid")]
    [SerializeField] private AsteroidSize size;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minRotationSpeed = -180f;
    [SerializeField] private float maxRotationSpeed = 180f;

    [Header("Explosion")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float explosionLength = 0.5f;

    private Rigidbody2D rb;
    private Collider2D asteroidCollider;
    private Animator animator;
    private AudioSource audioSource;
    private AsteroidSpawner spawner;

    private bool isExploding = false;


    /// <summary>
    /// Initializes the asteroid by getting components, settings its movement speed and giving it a random rotation speed.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        asteroidCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spawner = FindAnyObjectByType<AsteroidSpawner>();

      
        rb.linearVelocity = transform.up * speed;

        rb.angularVelocity = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    /// <summary>
    /// Handles destroying asteroid by spawning smaller asteroids, plays explosion animation and sound, removes asteroid after anim finishes
    /// </summary>
    private void BreakAsteroid()
    {

        if (isExploding)
            return;
        isExploding = true;

   
        if (size == AsteroidSize.Large)
        {
            SpawnChildren(AsteroidSize.Medium);
        }
        else if (size == AsteroidSize.Medium)
        {
            SpawnChildren(AsteroidSize.Small);
        }

 
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;


        asteroidCollider.enabled = false;

 
        animator.SetTrigger("Destroy");

        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
  
        StartCoroutine(DestroyAfterAnimation());
    }
    
    /// <summary>
    /// Waits for explsion anim to finish before destroying the asteroid gameObject
    /// </summary>

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(explosionLength);
        Destroy(gameObject);
    }

    /// <summary>
    /// Spawns two smaller asteroids at the current asteroids position
    /// </summary>
    /// <param name="childSize"></param>

    private void SpawnChildren(AsteroidSize childSize)
    {
        if (spawner == null)
            return;
        spawner.SpawnAsteroid(transform.position, childSize);
        spawner.SpawnAsteroid(transform.position, childSize);
    }

    /// <summary>
    /// Detects collisions with bullets and breaks the asteroid when hit
    /// </summary>
    /// <param name="collision"></param>

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isExploding)
            return;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            BreakAsteroid();
        }
    }
}