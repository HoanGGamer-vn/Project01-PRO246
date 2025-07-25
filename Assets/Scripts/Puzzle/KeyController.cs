using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public Data data;
    private Rigidbody2D rb;
    private Rigidbody2D blue;
    private Rigidbody2D red;
    private string keyPicker;
    // Start is called before the first frame update
    void Start()
    {
        data.hasKey = false;
        keyPicker = "";
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        blue = GameObject.FindGameObjectWithTag("Blue").GetComponent<Rigidbody2D>();
        red = GameObject.FindGameObjectWithTag("Red").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keyPicker == "Blue")
        {
            transform.position = new Vector3(blue.position.x, blue.position.y + 1f, transform.position.z);
        }
        else if (keyPicker == "Red")
        {
            transform.position = new Vector3(red.position.x, red.position.y + 1f, transform.position.z);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Blue") || other.CompareTag("Red"))
        {
            data.hasKey = true;
            keyPicker = other.tag;

            rb.rotation = 90f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
