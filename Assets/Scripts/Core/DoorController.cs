using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private int count;
    public Data data;
    private SpriteRenderer door;
    public Sprite openDoorSprite;
    public Sprite closedDoorSprite;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        door = GetComponent<SpriteRenderer>();
        door.sprite = closedDoorSprite;
        data.hasKey = false;
        data.isCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (data.hasKey && count == 2)
        {
            door.sprite = openDoorSprite;
            data.isCompleted = true;
        }
        else
        {
            door.sprite = closedDoorSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Blue") || other.CompareTag("Red"))
        {
            count++;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Blue") || other.CompareTag("Red"))
        {
            count--;
        }
    }
}
