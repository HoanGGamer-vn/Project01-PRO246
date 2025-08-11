using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position + new Vector3(0, 4f, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LowLimit"))
        {
            transform.position = StartPos;
        }
    }
}
