using UnityEngine;

public class PlayerDeathSound : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Sound Effect")]
    public AudioClip deathSound;   
    void Start()
    {
    
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap") || other.CompareTag("LowLimit"))
        {
            PlayDeathSound();
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
