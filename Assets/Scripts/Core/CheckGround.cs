using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public GameObject player;
    private string playerTag;
    public Data data;

    void Start()
    {
        playerTag = player.tag;
        SetCanJump(false);
    }

    void Update()
    {
        transform.position = player.transform.position - new Vector3(0, 0.15f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Tilemap" || tag == "Elevator")
        {
            SetCanJump(true);
        }

        if(tag == "Blue" && playerTag == "Red")
        {
            SetCanJump(true);
            data.isRedStandOnBlue = true;
            Debug.Log("Red is standing on Blue tile");
        }
        else if (tag == "Red" && playerTag == "Blue")
        {
            SetCanJump(true);
            data.isBlueStandOnRed = true;
            Debug.Log("Blue is standing on Red tile");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Tilemap" || tag == "Elevator")
        {
            SetCanJump(false);
        }
        if (tag == "Blue" && playerTag == "Red")
        {
            SetCanJump(false);
            data.isRedStandOnBlue = false;
        }
        else if (tag == "Red" && playerTag == "Blue")
        {
            SetCanJump(false);
            data.isBlueStandOnRed = false;
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
