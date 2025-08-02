using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Authenticator : MonoBehaviour
{
    [Header("Button References")]
    [Tooltip("Các GameObjects buttons từ 1-5 để nhân vật trigger")]
    public GameObject[] inputButtons = new GameObject[5];
    
    [Tooltip("Display GameObject chứa các Symbols")]
    public GameObject displayObject;
    
    [Tooltip("Các Symbols GameObjects con trong Display (Symbols_1 đến Symbols_5)")]
    public GameObject[] symbolObjects = new GameObject[5];
    
    [Header("Symbol Sprites")]
    [Tooltip("Sprites cho symbols 1-5 (Không bắt buộc nếu dùng GameObject system)")]
    public Sprite[] symbolSprites = new Sprite[5];
    
    [Tooltip("Sprite khi button chưa được trigger")]
    public Sprite buttonUnpressedSprite;
    
    [Tooltip("Sprite khi button đã được trigger")]
    public Sprite buttonPressedSprite;
    
    [Header("Authentication Settings")]
    [Tooltip("Độ dài mã cần nhập")]
    public int codeLength = 5;
    
    [Tooltip("Có random mã mới khi sai không")]
    public bool randomizeOnWrong = true;
    
    [Tooltip("Tags của nhân vật có thể trigger buttons")]
    public string[] playerTags = { "Blue", "Red", "Player" };
    
    [Header("Events")]
    [Tooltip("Event khi authentication thành công")]
    public UnityEvent OnAuthenticationSuccess;
    
    [Tooltip("Event khi authentication thất bại")]
    public UnityEvent OnAuthenticationFailed;
    
    [Header("Object Movement")]
    [Tooltip("Object sẽ được di chuyển sau khi hoàn thành authentication")]
    public Transform objectToMove;
    
    [Tooltip("Vị trí đích để di chuyển object đến")]
    public Transform targetPosition;
    
    [Tooltip("Tốc độ di chuyển object")]
    public float moveSpeed = 2f;
    
    [Tooltip("Có di chuyển object sau khi hoàn thành không")]
    public bool enableMovement = true;
    
    // Private variables
    private int[] targetCode;        // Mã đúng cần nhập
    private List<int> currentInput;  // Input hiện tại của user
    private bool isCompleted = false;
    private bool isMoving = false;   // Kiểm tra object có đang di chuyển không
    
    void Start()
    {
        InitializeAuthenticator();
        GenerateNewCode();
    }
    
    /// <summary>
    /// Khởi tạo authenticator
    /// </summary>
    private void InitializeAuthenticator()
    {
        currentInput = new List<int>();
        
        // Auto-find symbols nếu chưa được gán
        AutoFindSymbols();
        
        // Setup trigger components cho từng button GameObject
        for (int i = 0; i < inputButtons.Length; i++)
        {
            if (inputButtons[i] != null)
            {
                // Thêm hoặc lấy ButtonTriggerHandler component
                ButtonTriggerHandler triggerHandler = inputButtons[i].GetComponent<ButtonTriggerHandler>();
                if (triggerHandler == null)
                {
                    triggerHandler = inputButtons[i].AddComponent<ButtonTriggerHandler>();
                }
                
                // Setup trigger handler
                int buttonNumber = i + 1; // Button 1-5
                triggerHandler.Initialize(this, buttonNumber, playerTags);
                
                // Đảm bảo có Collider2D và isTrigger = true
                Collider2D collider = inputButtons[i].GetComponent<Collider2D>();
                if (collider == null)
                {
                    collider = inputButtons[i].AddComponent<BoxCollider2D>();
                }
                collider.isTrigger = true;
                
                // Set initial button sprite
                SetButtonVisual(i, false);
            }
        }
        
        Debug.Log("Authenticator đã được khởi tạo với " + inputButtons.Length + " trigger buttons");
    }
    
    /// <summary>
    /// Tự động tìm symbols trong Display object
    /// </summary>
    private void AutoFindSymbols()
    {
        if (displayObject == null) return;
        
        // Tìm symbols theo tên
        for (int i = 0; i < symbolObjects.Length; i++)
        {
            if (symbolObjects[i] == null)
            {
                // Tìm theo tên "Symbols_1", "Symbols_2", etc.
                Transform symbolTransform = displayObject.transform.Find($"Symbols_{i + 1}");
                if (symbolTransform != null)
                {
                    symbolObjects[i] = symbolTransform.gameObject;
                    Debug.Log($"Auto-found Symbol_{i + 1}: {symbolObjects[i].name}");
                }
                else
                {
                    // Fallback: tìm theo tên "Symbol_1", "Symbol_2", etc.
                    symbolTransform = displayObject.transform.Find($"Symbol_{i + 1}");
                    if (symbolTransform != null)
                    {
                        symbolObjects[i] = symbolTransform.gameObject;
                        Debug.Log($"Auto-found Symbol_{i + 1}: {symbolObjects[i].name}");
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Xử lý khi nhân vật trigger button (được gọi từ ButtonTriggerHandler)
    /// </summary>
    public void OnButtonTriggered(int buttonNumber)
    {
        if (isCompleted) return;
        
        Debug.Log("Button " + buttonNumber + " được trigger bởi nhân vật");
        
        // Thêm input
        currentInput.Add(buttonNumber);
        
        // Cập nhật visual button
        SetButtonVisual(buttonNumber - 1, true);
        
        // Kiểm tra input
        CheckInput();
    }
    
    /// <summary>
    /// Tạo mã mới ngẫu nhiên (không có số lặp lại)
    /// </summary>
    private void GenerateNewCode()
    {
        // Tạo danh sách các số từ 1-5
        List<int> availableNumbers = new List<int> { 1, 2, 3, 4, 5 };
        targetCode = new int[codeLength];
        
        // Đảm bảo codeLength không vượt quá 5 (vì chỉ có 5 số khác nhau)
        int actualLength = Mathf.Min(codeLength, 5);
        
        // Shuffle và lấy số lượng cần thiết
        for (int i = 0; i < actualLength; i++)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count);
            targetCode[i] = availableNumbers[randomIndex];
            availableNumbers.RemoveAt(randomIndex); // Xóa số đã chọn để tránh lặp lại
        }
        
        // Hiển thị mã trên display
        UpdateDisplay();
        
        // Reset input
        ResetInput();
        
        Debug.Log($"Mã mới (không lặp) - Độ dài {actualLength}: " + string.Join("-", System.Array.FindAll(targetCode, x => x != 0)));
    }
    
    /// <summary>
    /// Cập nhật display hiển thị mã cần nhập
    /// </summary>
    private void UpdateDisplay()
    {
        // Tắt tất cả symbols trước
        for (int i = 0; i < symbolObjects.Length; i++)
        {
            if (symbolObjects[i] != null)
            {
                symbolObjects[i].SetActive(false);
            }
        }
        
        // Lấy độ dài thực tế của mã (chỉ đếm các số khác 0)
        int actualCodeLength = System.Array.FindAll(targetCode, x => x != 0).Length;
        
        // Chỉ hiển thị symbol hiện tại cần nhập (theo currentInput.Count)
        if (currentInput.Count < actualCodeLength && targetCode != null)
        {
            int currentSymbolIndex = targetCode[currentInput.Count] - 1; // Symbol 1-5 -> index 0-4
            
            if (currentSymbolIndex >= 0 && currentSymbolIndex < symbolObjects.Length)
            {
                if (symbolObjects[currentSymbolIndex] != null)
                {
                    symbolObjects[currentSymbolIndex].SetActive(true);
                    Debug.Log($"Hiển thị Symbol_{currentSymbolIndex + 1} (vị trí {currentInput.Count + 1}/{actualCodeLength})");
                }
            }
        }
        
        // Đảm bảo Display object được active
        if (displayObject != null)
        {
            displayObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// Kiểm tra input hiện tại
    /// </summary>
    private void CheckInput()
    {
        int currentPosition = currentInput.Count - 1; // Vị trí vừa nhập (0-based)
        int actualCodeLength = System.Array.FindAll(targetCode, x => x != 0).Length; // Độ dài thực tế
        
        // Kiểm tra input vừa nhập có đúng không
        if (currentPosition >= actualCodeLength || currentInput[currentPosition] != targetCode[currentPosition])
        {
            // Sai rồi
            Debug.Log($"Input sai! Nhập {currentInput[currentPosition]}, cần {targetCode[currentPosition]} tại vị trí {currentPosition + 1}");
            OnAuthenticationFail();
            return;
        }
        
        // Đúng rồi, kiểm tra xem đã hoàn thành chưa
        if (currentInput.Count == actualCodeLength)
        {
            Debug.Log("Authentication thành công!");
            OnAuthenticationComplete();
        }
        else
        {
            // Chưa hoàn thành, cập nhật display để hiện symbol tiếp theo
            Debug.Log($"Đúng! Vị trí {currentPosition + 1}/{actualCodeLength} hoàn thành. Hiển thị symbol tiếp theo...");
            UpdateDisplay();
        }
    }
    
    /// <summary>
    /// Xử lý khi authentication thành công
    /// </summary>
    private void OnAuthenticationComplete()
    {
        isCompleted = true;
        
        // Tắt tất cả symbols khi hoàn thành
        for (int i = 0; i < symbolObjects.Length; i++)
        {
            if (symbolObjects[i] != null)
            {
                symbolObjects[i].SetActive(false);
            }
        }
        
        // Trigger success event
        OnAuthenticationSuccess?.Invoke();
        
        // Di chuyển object nếu được bật
        if (enableMovement && objectToMove != null && targetPosition != null)
        {
            StartCoroutine(MoveObjectToTarget());
        }
        
        Debug.Log("🎉 Authenticator hoàn thành!");
    }
    
    /// <summary>
    /// Xử lý khi authentication thất bại
    /// </summary>
    private void OnAuthenticationFail()
    {
        Debug.Log("❌ Authentication thất bại! Sẽ reset sau 1 giây...");
        
        // Trigger failed event
        OnAuthenticationFailed?.Invoke();
        
        // Reset sau delay nhỏ
        StartCoroutine(ResetAfterDelay());
    }
    
    /// <summary>
    /// Reset sau khi thất bại
    /// </summary>
    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        
        if (randomizeOnWrong)
        {
            Debug.Log("🔄 Tạo mã mới do input sai");
            GenerateNewCode(); // Đã có UpdateDisplay() bên trong
        }
        else
        {
            Debug.Log("🔄 Reset input, giữ nguyên mã");
            ResetInput(); // Đã có UpdateDisplay() bên trong
        }
    }
    
    /// <summary>
    /// Reset input và button visuals
    /// </summary>
    private void ResetInput()
    {
        currentInput.Clear();
        isCompleted = false;
        
        // Reset tất cả button visuals
        for (int i = 0; i < inputButtons.Length; i++)
        {
            SetButtonVisual(i, false);
        }
        
        // Cập nhật display để hiển thị symbol đầu tiên
        UpdateDisplay();
        
        Debug.Log("Input đã được reset, hiển thị symbol đầu tiên");
    }
    
    /// <summary>
    /// Cập nhật visual của button GameObject
    /// </summary>
    private void SetButtonVisual(int buttonIndex, bool isPressed)
    {
        if (buttonIndex < 0 || buttonIndex >= inputButtons.Length) return;
        if (inputButtons[buttonIndex] == null) return;
        
        // Tìm SpriteRenderer để đổi sprite
        SpriteRenderer spriteRenderer = inputButtons[buttonIndex].GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isPressed ? buttonPressedSprite : buttonUnpressedSprite;
        }
        else
        {
            // Fallback: tìm Image component (nếu là UI)
            Image imageComponent = inputButtons[buttonIndex].GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = isPressed ? buttonPressedSprite : buttonUnpressedSprite;
            }
        }
    }
    
    #region Public Methods
    
    /// <summary>
    /// Tạo mã mới (có thể gọi từ script khác)
    /// </summary>
    public void GenerateNewCodePublic()
    {
        GenerateNewCode();
    }
    
    /// <summary>
    /// Reset authenticator
    /// </summary>
    public void ResetAuthenticator()
    {
        ResetInput();
        Debug.Log("Authenticator đã được reset");
    }
    
    /// <summary>
    /// Di chuyển object đến vị trí đích sau khi hoàn thành
    /// </summary>
    private IEnumerator MoveObjectToTarget()
    {
        if (objectToMove == null || targetPosition == null)
        {
            Debug.LogWarning("Object to Move hoặc Target Position chưa được gán!");
            yield break;
        }
        
        isMoving = true;
        Vector3 startPos = objectToMove.position;
        Vector3 targetPos = targetPosition.position;
        
        Debug.Log($"🚀 Bắt đầu di chuyển {objectToMove.name} từ {startPos} đến {targetPos}");
        
        // Di chuyển object với tốc độ moveSpeed
        while (Vector3.Distance(objectToMove.position, targetPos) > 0.01f)
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        // Đảm bảo object đã đến đúng vị trí
        objectToMove.position = targetPos;
        isMoving = false;
        
        Debug.Log($"✅ {objectToMove.name} đã đến vị trí đích: {targetPos}");
    }
    
    /// <summary>
    /// Kiểm tra có object đang di chuyển không
    /// </summary>
    public bool IsObjectMoving()
    {
        return isMoving;
    }
    
    /// <summary>
    /// Kiểm tra đã hoàn thành chưa
    /// </summary>
    public bool IsCompleted()
    {
        return isCompleted;
    }
    
    /// <summary>
    /// Lấy mã hiện tại (debug)
    /// </summary>
    public string GetCurrentCode()
    {
        return targetCode != null ? string.Join("-", targetCode) : "No code";
    }
    
    /// <summary>
    /// Set mã cụ thể (testing)
    /// </summary>
    public void SetCode(int[] code)
    {
        if (code != null && code.Length <= codeLength)
        {
            targetCode = new int[code.Length];
            System.Array.Copy(code, targetCode, code.Length);
            UpdateDisplay();
            ResetInput();
            Debug.Log("Đã set mã: " + string.Join("-", targetCode));
        }
    }
    
    /// <summary>
    /// Set object và target position cho movement
    /// </summary>
    public void SetMovementObjects(Transform objToMove, Transform targetPos, float speed = 2f)
    {
        objectToMove = objToMove;
        targetPosition = targetPos;
        moveSpeed = speed;
        enableMovement = true;
        Debug.Log($"Đã set movement: {objToMove?.name} → {targetPos?.name} (tốc độ: {speed})");
    }
    
    /// <summary>
    /// Bật/tắt tính năng di chuyển object
    /// </summary>
    public void SetMovementEnabled(bool enabled)
    {
        enableMovement = enabled;
        Debug.Log("Movement " + (enabled ? "bật" : "tắt"));
    }
    
    /// <summary>
    /// Di chuyển object ngay lập tức (không cần hoàn thành authentication)
    /// </summary>
    public void MoveObjectNow()
    {
        if (objectToMove != null && targetPosition != null)
        {
            StartCoroutine(MoveObjectToTarget());
        }
        else
        {
            Debug.LogWarning("Chưa set Object to Move hoặc Target Position!");
        }
    }
    
    #endregion
}

/// <summary>
/// Component để handle trigger events cho button GameObjects
/// </summary>
public class ButtonTriggerHandler : MonoBehaviour
{
    private Authenticator authenticator;
    private int buttonNumber;
    private string[] allowedTags;
    private bool isTriggered = false;
    
    /// <summary>
    /// Khởi tạo trigger handler
    /// </summary>
    public void Initialize(Authenticator auth, int btnNumber, string[] playerTags)
    {
        authenticator = auth;
        buttonNumber = btnNumber;
        allowedTags = playerTags;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        if (authenticator == null) return;
        
        // Kiểm tra tag của object trigger
        foreach (string tag in allowedTags)
        {
            if (other.CompareTag(tag))
            {
                isTriggered = true;
                authenticator.OnButtonTriggered(buttonNumber);
                Debug.Log($"Button {buttonNumber} triggered bởi {other.name} (tag: {tag})");
                
                // Delay nhỏ trước khi có thể trigger lại
                StartCoroutine(ResetTriggerDelay());
                break;
            }
        }
    }
    
    /// <summary>
    /// Reset trigger state sau delay
    /// </summary>
    private IEnumerator ResetTriggerDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isTriggered = false;
    }
    
    /// <summary>
    /// Reset trigger state ngay lập tức
    /// </summary>
    public void ResetTrigger()
    {
        isTriggered = false;
    }
}
