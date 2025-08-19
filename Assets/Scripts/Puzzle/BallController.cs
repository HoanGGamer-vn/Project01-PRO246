using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Vector3 spawnPoint;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private PhysicsMaterial2D physicsMaterial2D;

    private AudioSource audioSource;
    public AudioClip bounceSound;      
    public float bounceThreshold = 2f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        physicsMaterial2D = new PhysicsMaterial2D();
        physicsMaterial2D.bounciness = 0.9f;
        physicsMaterial2D.friction = 0.1f;

        col.sharedMaterial = physicsMaterial2D;
        spawnPoint = transform.position;

        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LowLimit") || other.CompareTag("Trap"))
        {
            transform.position = spawnPoint;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (other.CompareTag("Button"))
        {
            physicsMaterial2D.bounciness = 0.3f;
        }
        if (other.CompareTag("Goal"))
        {
            physicsMaterial2D.bounciness = 0.08f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Button"))
        {
            physicsMaterial2D.bounciness = 0.9f;
        }
    }

 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (bounceSound != null && audioSource != null)
        {
            float impactForce = collision.relativeVelocity.magnitude;
            if (impactForce > bounceThreshold)
            {
                audioSource.PlayOneShot(bounceSound);
            }
        }
    }
}
