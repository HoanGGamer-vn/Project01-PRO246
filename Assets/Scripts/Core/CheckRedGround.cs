using UnityEngine;

public class CheckRedGround : MonoBehaviour
{
    public GameObject redPlayer;
    public Data data;

    private RedControl redControl;
    private float checkJumpForce;

    private bool isOnGround;
    private bool isOnElevator;
    private bool isOnBox;
    private bool isOnBlue;
    private bool isOnSlime;

    void Start()
    {
        ResetFlags();
        SetCanJump(false);
        redControl = redPlayer.GetComponent<RedControl>();
        checkJumpForce = redControl.jumpForce;
    }

    void Update()
    {
        transform.position = redPlayer.transform.position - new Vector3(0, 0.15f, 0);

        if (isOnGround || isOnElevator || isOnBox || isOnBlue || isOnSlime)
            SetCanJump(true);
        else
            SetCanJump(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Tilemap":
                isOnGround = true;
                break;
            case "Elevator":
                isOnElevator = true;
                break;
            case "Box":
                isOnBox = true;
                break;
            case "Slime":
                isOnSlime = true;
                redControl.jumpForce = checkJumpForce * 1.8f;
                break;
            case "Blue":
                isOnBlue = true;
                data.isRedStandOnBlue = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Tilemap":
                isOnGround = false;
                break;
            case "Elevator":
                isOnElevator = false;
                break;
            case "Box":
                isOnBox = false;
                break;
            case "Slime":
                isOnSlime = false;
                redControl.jumpForce = checkJumpForce;
                break;
            case "Blue":
                isOnBlue = false;
                data.isRedStandOnBlue = false;
                break;
        }
    }

    private void SetCanJump(bool value)
    {
        data.canRedJump = value;
    }

    private void ResetFlags()
    {
        isOnGround = false;
        isOnElevator = false;
        isOnBox = false;
        isOnBlue = false;
        isOnSlime = false;
    }
}
