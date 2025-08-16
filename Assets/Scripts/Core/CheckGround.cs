using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public GameObject player;
    private string playerTag;
    public Data data;

    private bool isOnGround;
    private bool isOnElevator;
    private bool isOnBox;
    void Start()
    {
        playerTag = player.tag;
        SetCanJump(false);

        isOnBox = false;
        isOnGround = false;
        isOnElevator = false;
    }

    void Update()
    {
        transform.position = player.transform.position - new Vector3(0, 0.15f, 0);

        if(isOnGround || isOnElevator || isOnBox)
        {
            SetCanJump(true);
        }
        else
        {
            SetCanJump(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Blue" && playerTag == "Red")
        {
            SetCanJump(true);
            data.isRedStandOnBlue = true;
            return;
        }
        else if (tag == "Red" && playerTag == "Blue")
        {
            SetCanJump(true);
            data.isBlueStandOnRed = true;
            return;
        }

        switch (tag)
        {
            case "Elevator":
                isOnElevator = true;
                break;
            case "Box":
                isOnBox = true;
                break;
            case "Tilemap":
                isOnGround = true;
                break;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Blue" && playerTag == "Red")
        {
            SetCanJump(false);
            data.isRedStandOnBlue = false;
            return;
        }
        else if (tag == "Red" && playerTag == "Blue")
        {
            SetCanJump(false);
            data.isBlueStandOnRed = false;
            return;
        }

        switch (tag)
        {
            case "Elevator":
                isOnElevator = false;
                break;
            case "Box":
                isOnBox = false;
                break;
            case "Tilemap":
                isOnGround = false;
                break;
        }

    }

    private void SetCanJump(bool value)
    {
        if (playerTag == "Blue")
        {
            data.canBlueJump = value;
        }
        else if (playerTag == "Red")
        {
            data.canRedJump = value;
        }
    }
}
