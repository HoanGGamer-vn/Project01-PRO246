using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "Game/Data")]
public class Data : ScriptableObject
{
    public bool isRedStatic;
    public bool isRedGrounded;

    public bool isBlueStatic;
    public bool isBlueGrounded;

    public bool hasKey;
    public bool isCompleted;

    public bool canRedMoveRight;
    public bool canRedMoveLeft;
    public bool canBlueMoveRight;
    public bool canBlueMoveLeft;
}
