using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LowLimitManager : MonoBehaviour
{
    public Rigidbody2D blue;
    public Rigidbody2D red;

    private int deadCount;

    void Start()
    {
        deadCount = 0;
    }

    private IEnumerator CountDead()
    {
        yield return new WaitForSeconds(2f);
        if (deadCount >= 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            deadCount = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Red") || other.CompareTag("Blue"))
        {
            deadCount++;
            StartCoroutine(CountDead());
        }
    }
}
