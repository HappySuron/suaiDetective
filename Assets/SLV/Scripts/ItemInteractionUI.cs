using UnityEngine;
using UnityEngine.UI; // Для кнопки UI
using TMPro;
using StarterAssets;

public class ItemInteractionUI : MonoBehaviour
{
    public static ItemInteractionUI Instance;
    [Header("UI Elements")]
    public GameObject interactionPanel;  // Панель, которая будет появляться при осмотре

    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;     // Текст, который будет отображаться в интерфейсе

    public Image itemImage;
    public Button closeButton;           // Кнопка, которая будет скрывать интерфейс

    public ThirdPersonController thirdPersonController;

    public CameraRaycast cameraRaycast;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Убираем панель при старте
        interactionPanel.SetActive(false);

        // Привязываем обработчик к кнопке
        closeButton.onClick.AddListener(ClosePanel);
    }

    // Метод для показа интерфейса
    public void ShowItemInterface(string name ,string description, Sprite image)
    {
        // Показываем панель
        interactionPanel.SetActive(true);

        // Устанавливаем текст для отображения
        itemDescriptionNameText.text = name;
        itemDescriptionText.text = description;
        itemImage.sprite = image;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Отменяет захват курсора
        thirdPersonController.enabled = false;
        //cameraRaycast.enabled = false;
    }

    // Метод для закрытия интерфейса
    private void ClosePanel()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        thirdPersonController.enabled = true;
        //cameraRaycast.enabled = true;
        interactionPanel.SetActive(false);
    }


    public bool CheckIfActive()
    {
        return interactionPanel.activeSelf; // Возвращает true, если панель активна
    }
}
