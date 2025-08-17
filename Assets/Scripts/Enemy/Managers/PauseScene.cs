using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class PauseScene : MonoBehaviour
{
    [Header("Popup Settings")]
    public PopupOpener pausePopupOpener;
    
    [Header("Player Controllers")]
    public BlueController blueController;
    public RedController redController;
    
    private bool isPaused = false;
    
    void Start()
    {
        // Tự động tìm controllers nếu không được gán
        if (blueController == null)
        {
            blueController = FindObjectOfType<BlueController>();
            Debug.Log("Tìm thấy BlueController: " + (blueController != null));
        }
        if (redController == null)
        {
            redController = FindObjectOfType<RedController>();
            Debug.Log("Tìm thấy RedController: " + (redController != null));
        }
    }

    void Update()
    {
        // Tạm thời tắt auto-detect để debug
        /*
        // Auto-detect nếu popup đã đóng nhưng vẫn isPaused
        if (isPaused)
        {
            // Tìm popup có tag "Popup" hoặc tên chứa "Popup"
            GameObject[] popups = GameObject.FindGameObjectsWithTag("Untagged");
            bool hasActivePopup = false;
            
            foreach (GameObject obj in popups)
            {
                if (obj.name.Contains("Popup") && obj.activeInHierarchy)
                {
                    hasActivePopup = true;
                    break;
                }
            }
            
            // Nếu không có popup nào active mà vẫn pause thì reset
            if (!hasActivePopup)
            {
                OnPopupClosed();
            }
        }
        */
        
        // Debug liên tục để theo dõi trạng thái
        if (isPaused)
        {
            if (blueController != null)
                Debug.Log("Blue enabled: " + blueController.enabled);
            if (redController != null)
                Debug.Log("Red enabled: " + redController.enabled);
        }
    }
    
    // Method này gọi từ UI Button để hiện popup
    public void ShowPausePopup()
    {
        // Kiểm tra và reset trạng thái nếu cần
        if (isPaused)
        {
            Debug.Log("Game đã pause, reset trạng thái trước khi mở popup mới");
            isPaused = false;
        }
        
        if (pausePopupOpener != null)
        {
            isPaused = true;
            
            // Tắt controllers
            DisableControllers();
            
            // Đảm bảo PopupOpener được enable và active
            pausePopupOpener.enabled = true;
            pausePopupOpener.gameObject.SetActive(true);
            
            pausePopupOpener.OpenPopup();
            Debug.Log("Game pause - Controllers đã tắt");
        }
        else
        {
            Debug.LogWarning("PausePopupOpener chưa được gán!");
        }
    }
    
    // Method này gọi từ button Resume trong popup
    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            
            // Bật lại controllers
            EnableControllers();
            
            // Đóng popup sau khi resume để có thể mở lại
            CloseCurrentPopup();
            
            Debug.Log("Game resume - Controllers đã bật lại và popup đã đóng");
        }
    }
    
    /// <summary>
    /// Đóng popup hiện tại để có thể mở lại
    /// </summary>
    private void CloseCurrentPopup()
    {
        // Tìm và đóng popup đang active
        var activePopups = FindObjectsOfType<Ricimi.Popup>();
        foreach (var popup in activePopups)
        {
            if (popup.gameObject.activeInHierarchy)
            {
                popup.Close();
                Debug.Log("Đã đóng popup: " + popup.name);
                return;
            }
        }
        
        // Fallback: tìm object có tên chứa "Popup" và ẩn nó
        var allObjects = FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.name.Contains("Popup") && obj.activeInHierarchy)
            {
                obj.SetActive(false);
                Debug.Log("Đã ẩn popup fallback: " + obj.name);
                return;
            }
        }
        
        Debug.Log("Không tìm thấy popup để đóng");
    }
    
    // Method này gọi khi popup đóng (không cần resume)
    public void OnPopupClosed()
    {
        isPaused = false;
        
        // Bật lại controllers
        EnableControllers();
        
        Debug.Log("Popup đã đóng - Controllers đã bật lại");
    }
    
    // Method này gọi từ button Quit trong popup
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    private void DisableControllers()
    {
        Debug.Log("DisableControllers được gọi!");
        
        if (blueController != null)
        {
            blueController.enabled = false;
            Debug.Log("BlueController đã tắt: " + !blueController.enabled);
        }
        if (redController != null)
        {
            redController.enabled = false;
            Debug.Log("RedController đã tắt: " + !redController.enabled);
        }
        
        // Thêm tắt Rigidbody2D để chắc chắn không di chuyển được
        if (blueController != null)
        {
            Rigidbody2D blueRb = blueController.GetComponent<Rigidbody2D>();
            if (blueRb != null)
            {
                blueRb.velocity = Vector2.zero;
                blueRb.simulated = false;
                Debug.Log("BlueRb simulated = false");
            }
        }
        if (redController != null)
        {
            Rigidbody2D redRb = redController.GetComponent<Rigidbody2D>();
            if (redRb != null)
            {
                redRb.velocity = Vector2.zero;
                redRb.simulated = false;
                Debug.Log("RedRb simulated = false");
            }
        }
    }
    
    private void EnableControllers()
    {
        Debug.Log("EnableControllers được gọi!");
        
        if (blueController != null)
        {
            blueController.enabled = true;
            Debug.Log("BlueController đã bật: " + blueController.enabled);
        }
        if (redController != null)
        {
            redController.enabled = true;
            Debug.Log("RedController đã bật: " + redController.enabled);
        }
        
        // Bật lại Rigidbody2D
        if (blueController != null)
        {
            Rigidbody2D blueRb = blueController.GetComponent<Rigidbody2D>();
            if (blueRb != null)
            {
                blueRb.simulated = true;
                Debug.Log("BlueRb simulated = true");
            }
        }
        if (redController != null)
        {
            Rigidbody2D redRb = redController.GetComponent<Rigidbody2D>();
            if (redRb != null)
            {
                redRb.simulated = true;
                Debug.Log("RedRb simulated = true");
            }
        }
    }
    
    // Getter để check trạng thái
    public bool IsPaused()
    {
        return isPaused;
    }
    
    void OnDestroy()
    {
        // Đảm bảo bật lại controllers khi destroy
        EnableControllers();
    }
}
