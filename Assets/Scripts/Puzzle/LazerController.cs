using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour
{
    public GameObject lazerTail;
    void Start()
    {
        lazerTail.gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            lazerTail.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            lazerTail.gameObject.SetActive(true);
        }
    }
}
