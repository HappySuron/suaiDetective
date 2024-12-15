using UnityEngine;
using UnityEngine.UI; // Для кнопки UI
using TMPro;
using StarterAssets;
using System.Linq;

public class DialogUI : MonoBehaviour
{
    public static DialogUI Instance;

    [Header("UI Elements")]
    public GameObject dialogPanel;  // Панель, которая будет появляться во время диалога

    private TMP_Text ActionTextW;
    private TMP_Text ActionTextS;
    private TMP_Text ActionTextA;
    private TMP_Text ActionTextD;

    private Image imageW;
    private Image imageA;
    private Image imageS;
    private Image imageD;

    public ThirdPersonController thirdPersonController;

    public CameraRaycast cameraRaycast;  // Обратная ссылка на CameraRaycast для взаимодействия с опциями

    public StateTree currentCharector;

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
        dialogPanel.SetActive(false);
    }

    private void Update()
    {
        if (CheckIfDialogActive())
        {
            // Логика для обработки нажатий WASD, когда диалог открыт
            if (Input.GetKeyDown(KeyCode.W))
            {
                HandleOptionSelection(0);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                HandleOptionSelection(1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                HandleOptionSelection(2);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                HandleOptionSelection(3);
            }
        }
    }

    // Метод для отображения диалога
    public void ShowDialog(StateTree charactorStateTree)
    {
        currentCharector = charactorStateTree;
        dialogPanel.SetActive(true);
        thirdPersonController.enabled = false;

        // Показать доступные опции, если они есть
        if (charactorStateTree != null)
        {
            ShowOptions();
        }
    }

    // Метод для обработки выбора опции
    private void HandleOptionSelection(int optionIndex)
    {
        if (currentCharector != null)
        {
            var availableActions = currentCharector.GetAvailableActions().Values.ToList();
            if (optionIndex >= 0 && optionIndex < availableActions.Count)
            {
                var selectedPath = availableActions[optionIndex];
                Debug.Log($"Вы выбрали: {selectedPath.uiText}");
                currentCharector.MoveToNode(selectedPath.targetNode.nodeName);
                ResetUI();
            }
            else
            {
                Debug.LogWarning($"Неверный выбор опции: {optionIndex}");
            }
        }
    }

    // Метод для отображения опций для выбранного объекта
    private void ShowOptions()
    {

        var availableActions = currentCharector.GetAvailableActions();
        int index = 0;

        // Получение текстовых элементов
        ActionTextW = this.transform.Find("Options/W/ActionTextW")?.GetComponent<TMP_Text>();
        ActionTextA = this.transform.Find("Options/A/ActionTextA")?.GetComponent<TMP_Text>();
        ActionTextS = this.transform.Find("Options/S/ActionTextS")?.GetComponent<TMP_Text>();
        ActionTextD = this.transform.Find("Options/D/ActionTextD")?.GetComponent<TMP_Text>();

        
        // Получение изображений
        imageW = this.transform.Find("Options/W")?.GetComponent<Image>();
        imageA = this.transform.Find("Options/A")?.GetComponent<Image>();
        imageS = this.transform.Find("Options/S")?.GetComponent<Image>();
        imageD = this.transform.Find("Options/D")?.GetComponent<Image>();

        TMP_Text[] actionTexts = { ActionTextW, ActionTextS, ActionTextA, ActionTextD };
        Image[] actionImages = { imageW, imageS, imageA, imageD };

        // Устанавливаем текст и активируем изображения
        foreach (var action in availableActions.Values)
        {
            if (index < actionTexts.Length && actionTexts[index] != null && actionImages[index] != null)
            {
                Debug.Log("Устанавливаем текст: " + action.uiText);
                actionTexts[index].text = action.uiText; // Устанавливаем текст опции
                actionTexts[index].gameObject.SetActive(true); // Делаем текст активным
                actionImages[index].gameObject.SetActive(true); // Делаем изображение активным
            }
            index++;
        }

        // Очищаем оставшиеся элементы и скрываем изображения
        for (int i = index; i < actionTexts.Length; i++)
        {
            if (actionTexts[i] != null)
            {
                actionTexts[i].text = string.Empty; // Очищаем текст
                actionTexts[i].gameObject.SetActive(false); // Делаем текст неактивным
            }

            if (actionImages[i] != null)
            {
                actionImages[i].gameObject.SetActive(false); // Делаем изображение неактивным
            }
        }

        Debug.Log("Опции и изображения обновлены.");
    }

    private void ResetUI()
    {
        if (ActionTextW != null && ActionTextA != null && ActionTextS != null && ActionTextD != null)
        {
            TMP_Text[] actionTexts = { ActionTextW, ActionTextS, ActionTextA, ActionTextD };
            foreach (var text in actionTexts)
            {
                text.text = string.Empty;
                text.gameObject.SetActive(false);
            }
        }

        if (imageW != null && imageA != null && imageS != null && imageD != null)
        {
            Image[] actionImages = { imageW, imageA, imageS, imageD };
            foreach (var image in actionImages)
            {
                image.gameObject.SetActive(false);
            }
        }
    }

    // Проверка, активен ли диалог
    public bool CheckIfDialogActive()
    {
        return dialogPanel.activeSelf; // Возвращает true, если панель активна
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
        thirdPersonController.enabled = true; // Включаем управление персонажем снова
    }

}