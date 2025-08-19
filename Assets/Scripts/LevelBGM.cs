using UnityEngine;

public class LevelBGM : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void OnDestroy()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
