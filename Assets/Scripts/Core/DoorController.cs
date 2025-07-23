using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private int count;
    private SpriteRenderer door;
    public Sprite openDoorSprite;
    public Sprite closedDoorSprite;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        door = GetComponent<SpriteRenderer>();
        door.sprite = closedDoorSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Blue") || other.CompareTag("Red"))
        {
            count++;
        }
    }
}
