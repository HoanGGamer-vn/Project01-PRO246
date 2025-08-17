using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class DoorController : MonoBehaviour
{
    private int count;
    public Data data;
    private SpriteRenderer door;
    public Sprite openDoorSprite;
    public Sprite closedDoorSprite;
    
    [Header("Completion Settings")]
    [Tooltip("Object sẽ được kích hoạt khi hoàn thành điều kiện")]
    public GameObject targetObjectToActivate;
    
    [Tooltip("Có tự động ẩn object này sau khi kích hoạt target không")]
    public bool hideThisAfterActivation = false;
    
    [Tooltip("Thời gian delay trước khi kích hoạt (giây)")]
    public float activationDelay = 0f;
    
    // Private variables
    private bool hasActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        door = GetComponent<SpriteRenderer>();
        door.sprite = closedDoorSprite;
        data.hasKey = false;
        data.isCompleted = false;
        hasActivated = false;
    }
    void Update()
    {
        if (data.hasKey && count == 2)
        {
            door.sprite = openDoorSprite;

            // Chỉ set completed và kích hoạt object một lần
            if (!data.isCompleted)
            {
                data.isCompleted = true;
                //Debug.Log("Điều kiện hoàn thành! Kích hoạt target object...");
                ActivateTargetObject();
            }
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
    
    /// <summary>
    /// Kích hoạt target object khi hoàn thành điều kiện
    /// </summary>
    private void ActivateTargetObject()
    {
        if (hasActivated || targetObjectToActivate == null)
            return;
            
        if (activationDelay > 0)
        {
            StartCoroutine(ActivateWithDelay());
        }
        else
        {
            DoActivation();
        }
    }
    
    /// <summary>
    /// Coroutine để activate với delay
    /// </summary>
    private IEnumerator ActivateWithDelay()
    {
        Debug.Log("Chờ " + activationDelay + " giây trước khi kích hoạt...");
        yield return new WaitForSeconds(activationDelay);
        DoActivation();
    }
    
    /// <summary>
    /// Thực hiện việc kích hoạt object
    /// </summary>
    private void DoActivation()
    {
        if (targetObjectToActivate != null)
        {
            // Kích hoạt object
            targetObjectToActivate.SetActive(true);
            Debug.Log("Đã kích hoạt object: " + targetObjectToActivate.name);
            
            // Thử trigger các component đặc biệt
            TriggerSpecialComponents();
            
            hasActivated = true;
            
            // Ẩn object này nếu được yêu cầu
            if (hideThisAfterActivation)
            {
                gameObject.SetActive(false);
                Debug.Log("Đã ẩn DoorController object");
            }
        }
    }
    
    /// <summary>
    /// Trigger các component đặc biệt trong target object
    /// </summary>
    private void TriggerSpecialComponents()
    {
        if (targetObjectToActivate == null) return;
        
        // Đưa object lên trước tất cả UI elements
        BringToFront(targetObjectToActivate);
        
        // Nếu có Popup component thì mở popup trực tiếp
        var popup = targetObjectToActivate.GetComponent<Ricimi.Popup>();
        if (popup != null)
        {
            Debug.Log("Tìm thấy Popup component, đang mở popup trực tiếp...");
            popup.Open();
            return;
        }
        
        // Nếu có PopupOpener thì mở popup
        var popupOpener = targetObjectToActivate.GetComponent<Ricimi.PopupOpener>();
        if (popupOpener != null)
        {
            Debug.Log("Tìm thấy PopupOpener, đang mở popup...");
            popupOpener.OpenPopup();
            return;
        }
        
        // Tìm Popup trong children
        var childPopup = targetObjectToActivate.GetComponentInChildren<Ricimi.Popup>();
        if (childPopup != null)
        {
            Debug.Log("Tìm thấy Popup trong children, đang mở popup...");
            BringToFront(childPopup.gameObject);
            childPopup.Open();
            return;
        }
        
        // Tìm PopupOpener trong children
        var childPopupOpener = targetObjectToActivate.GetComponentInChildren<Ricimi.PopupOpener>();
        if (childPopupOpener != null)
        {
            Debug.Log("Tìm thấy PopupOpener trong children, đang mở popup...");
            BringToFront(childPopupOpener.gameObject);
            childPopupOpener.OpenPopup();
            return;
        }
        
        // Nếu có Button thì trigger onClick
        var button = targetObjectToActivate.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            Debug.Log("Tìm thấy Button, đang trigger onClick...");
            button.onClick.Invoke();
            return;
        }
        
        // Nếu có Animator thì trigger animation
        var animator = targetObjectToActivate.GetComponent<Animator>();
        if (animator != null)
        {
            Debug.Log("Tìm thấy Animator, đang trigger animation...");
            animator.SetTrigger("Activate"); // Cần có trigger "Activate" trong Animator
            return;
        }
        
        // Nếu có ParticleSystem thì play
        var particles = targetObjectToActivate.GetComponent<ParticleSystem>();
        if (particles != null)
        {
            Debug.Log("Tìm thấy ParticleSystem, đang play...");
            particles.Play();
            return;
        }
        
        Debug.Log("Object được kích hoạt nhưng không có component đặc biệt để trigger");
    }
    
    /// <summary>
    /// Đưa GameObject lên trước tất cả UI elements
    /// </summary>
    private void BringToFront(GameObject obj)
    {
        if (obj == null) return;
        
        // Tìm Canvas chứa object này
        Canvas parentCanvas = obj.GetComponentInParent<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogWarning("Không tìm thấy Canvas parent cho object: " + obj.name);
            return;
        }
        
        // Đặt Canvas sorting order cao nhất
        Canvas objCanvas = obj.GetComponent<Canvas>();
        if (objCanvas != null)
        {
            // Nếu object có Canvas riêng, đặt sorting order cao
            objCanvas.sortingOrder = 9999;
            objCanvas.overrideSorting = true;
            Debug.Log("Đã đặt Canvas sorting order = 9999 cho: " + obj.name);
        }
        else
        {
            // Nếu không có Canvas riêng, đưa lên đầu trong hierarchy
            obj.transform.SetAsLastSibling();
            Debug.Log("Đã đưa object lên đầu hierarchy: " + obj.name);
        }
        
        // Nếu có GraphicRaycaster, đảm bảo nó hoạt động
        var raycaster = obj.GetComponent<UnityEngine.UI.GraphicRaycaster>();
        if (raycaster != null)
        {
            raycaster.enabled = true;
        }
        
        // Đặt tất cả children cũng lên trước
        foreach (Transform child in obj.transform)
        {
            var childCanvas = child.GetComponent<Canvas>();
            if (childCanvas != null)
            {
                childCanvas.sortingOrder = 9999;
                childCanvas.overrideSorting = true;
            }
        }
    }
    
    /// <summary>
    /// Reset door state và deactivate target object
    /// </summary>
    public void ResetDoor()
    {
        data.hasKey = false;
        data.isCompleted = false;
        count = 0;
        door.sprite = closedDoorSprite;
        hasActivated = false;
        
        // Deactivate target object
        if (targetObjectToActivate != null)
        {
            targetObjectToActivate.SetActive(false);
            Debug.Log("Đã deactivate target object");
        }
        
        // Hiện lại door nếu đã bị ẩn
        if (hideThisAfterActivation)
        {
            gameObject.SetActive(true);
        }
        
        Debug.Log("Door đã được reset");
    }
    
    /// <summary>
    /// Kiểm tra trạng thái đã activated chưa
    /// </summary>
    public bool IsActivated()
    {
        return hasActivated;
    }
}
