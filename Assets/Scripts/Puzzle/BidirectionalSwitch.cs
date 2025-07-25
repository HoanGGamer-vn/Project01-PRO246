using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class BidirectionalSwitch : MonoBehaviour
{
    [Header("Target to Move")]
    public ToggleDevice targetDevice;

    [Header("Sprites")]
    public Sprite offSprite;
    public Sprite onSprite;

    private HashSet<Collider2D> activators = new HashSet<Collider2D>();
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = offSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsRelevantTag(other))
        {
            if (activators.Add(other))
            {
                UpdateSwitchState();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsRelevantTag(other))
        {
            if (activators.Remove(other))
            {
                UpdateSwitchState();
            }
        }
    }

    private bool IsRelevantTag(Collider2D other)
    {
        return other.CompareTag("Red") || other.CompareTag("Blue");
    }

    private void UpdateSwitchState()
    {
        if (activators.Count > 0)
        {
            spriteRenderer.sprite = onSprite;
            targetDevice.TurnOn(); // Bật
        }
        else
        {
            spriteRenderer.sprite = offSprite;
            targetDevice.TurnOff(); // Tắt
        }
    }
}
