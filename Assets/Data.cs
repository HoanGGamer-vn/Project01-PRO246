using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "Game/Data")]
public class Data : ScriptableObject
{
    public bool isRedStatic;
    public bool isRedGrounded;

    public bool isBlueStatic;
    public bool isBlueGrounded;
}
