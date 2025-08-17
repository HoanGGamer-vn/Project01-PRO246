using UnityEngine;

public class PausePlayerOnTrigger : MonoBehaviour
{
    // Tắt/bật PlayerController2D của nhân vật đi vào/ra collider, kiểm tra tag "red" hoặc "blue"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Red") || other.CompareTag("Blue"))
        {
            PlayerController2D pc = other.GetComponent<PlayerController2D>();
            if (pc != null)
                pc.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Red") || other.CompareTag("Blue"))
        {
            PlayerController2D pc = other.GetComponent<PlayerController2D>();
            if (pc != null)
                pc.enabled = true;
        }
    }
}
