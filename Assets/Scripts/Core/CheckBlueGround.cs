using UnityEngine;

public class CheckBlueGround : MonoBehaviour
{
    public GameObject bluePlayer;
    public Data data;

    private BlueControl blueControl;
    private float checkJumpForce;

    private bool isOnGround;
    private bool isOnElevator;
    private bool isOnBox;
    private bool isOnRed;
    private bool isOnSlime;

    void Start()
    {
        ResetFlags();
        SetCanJump(false);
        blueControl = bluePlayer.GetComponent<BlueControl>();
        checkJumpForce = blueControl.jumpForce;
    }

    void Update()
    {
        transform.position = bluePlayer.transform.position - new Vector3(0, 0.15f, 0);

        if (isOnGround || isOnElevator || isOnBox || isOnRed || isOnSlime)
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
                blueControl.jumpForce = checkJumpForce * 1.8f;
                break;
            case "Red":
                isOnRed = true;
                data.isBlueStandOnRed = true;
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
                blueControl.jumpForce = checkJumpForce;
                break;
            case "Red":
                isOnRed = false;
                data.isBlueStandOnRed = false;
                break;
        }
    }

    private void SetCanJump(bool value)
    {
        data.canBlueJump = value;
    }

    private void ResetFlags()
    {
        isOnGround = false;
        isOnElevator = false;
        isOnBox = false;
        isOnRed = false;
        isOnSlime = false;
    }
}
