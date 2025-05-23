using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandstormObstacle : MonoBehaviour
{
    [Header("Sandstorm Settings")]
    public float pullForce = 5f;           // How strong the pull towards center is
    public float speedReduction = 0.5f;    // Multiplier for player speed (0.5 = half speed)
    public float detectionRadius = 10f;    // How far the sandstorm affects the player
    public float damageRadius = 3f;        // How close player needs to be to take damage
    public float escapeSpeedMultiplier = 1.5f; // Sprint speed multiplier to escape

    [Header("Damage Settings")]
    public int damagePerSecond = 10;       // Damage dealt per second
    public float damageInterval = 1f;      // How often to deal damage (in seconds)

    private PlayerMovement playerMovement;
    private playerHealth playerHealth;
    private bool playerInStorm = false;
    private bool playerInDamageZone = false;
    private float originalSpeed;
    private float originalSprintSpeed;
    private float damageTimer = 0f;

    void Start()
    {
        // Find the player components
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerHealth = FindObjectOfType<playerHealth>();

        if (playerMovement != null)
        {
            // Store original speeds
            originalSpeed = playerMovement.speed;
            originalSprintSpeed = playerMovement.sprintSpeed;
        }

        // Set up the sphere collider as trigger
        SphereCollider col = GetComponent<SphereCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
        }
        col.isTrigger = true;
        col.radius = detectionRadius;
    }

    void Update()
    {
        if (playerInStorm && playerMovement != null)
        {
            ApplySandstormEffect();
            CheckDamageZone();

            if (playerInDamageZone)
            {
                HandleDamage();
            }
        }
    }

    void ApplySandstormEffect()
    {
        GameObject player = playerMovement.gameObject;
        Vector3 directionToStorm = (transform.position - player.transform.position).normalized;
        float distanceToStorm = Vector3.Distance(transform.position, player.transform.position);

        // Calculate pull force based on distance (stronger when closer)
        float pullStrength = pullForce * (1f - (distanceToStorm / detectionRadius));

        // Apply pull force towards sandstorm center
        Vector3 pullVector = directionToStorm * pullStrength * Time.deltaTime;

        // Check if player is sprinting to resist the pull
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (isSprinting)
        {
            // Player can escape when sprinting - reduce pull or even reverse it
            pullVector *= -escapeSpeedMultiplier; // Negative to help escape
        }

        // Apply the pull force
        playerMovement.controller.Move(pullVector);
    }

    void CheckDamageZone()
    {
        GameObject player = playerMovement.gameObject;
        float distanceToStorm = Vector3.Distance(transform.position, player.transform.position);

        bool wasInDamageZone = playerInDamageZone;
        playerInDamageZone = distanceToStorm <= damageRadius;

        // Reset damage timer when entering damage zone
        if (playerInDamageZone && !wasInDamageZone)
        {
            damageTimer = 0f;
            Debug.Log("Player entered damage zone!");
        }
        else if (!playerInDamageZone && wasInDamageZone)
        {
            Debug.Log("Player left damage zone!");
        }
    }

    void HandleDamage()
    {
        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond);
            }
            damageTimer = 0f; // Reset timer
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInStorm = true;

            // Reduce player movement speed
            if (playerMovement != null)
            {
                playerMovement.speed = originalSpeed * speedReduction;
                playerMovement.sprintSpeed = originalSprintSpeed * speedReduction;
            }

            Debug.Log("Player entered sandstorm effect zone!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInStorm = false;
            playerInDamageZone = false; // Also exit damage zone

            // Restore original player speeds
            if (playerMovement != null)
            {
                playerMovement.speed = originalSpeed;
                playerMovement.sprintSpeed = originalSprintSpeed;
            }

            Debug.Log("Player escaped sandstorm!");
        }
    }

    // Visual helper to see the detection radius in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
