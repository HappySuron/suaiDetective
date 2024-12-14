using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;
using System.Linq;
using TMPro;

public class CameraRaycast : MonoBehaviour
{
    public float rayDistance = 100f;
    public LayerMask ignoreLayerMask;
    public GameObject currentHighlightedObject;
    public Color farColor = Color.gray;
    public Color closeColor = Color.yellow;
    public Color clickedColor = Color.green;

    private Camera mainCamera;
    private CinemachineVirtualCamera virtualCamera;
    private bool isCameraFrozen = false;

    public GameObject thirdPersonControllerObject; // Ссылка на объект с контроллером персонажа
    private ThirdPersonController thirdPersonController; // Ссылка на скрипт ThirdPersonController

    public StateTree stateTree;

    private bool isMouseHeld = false;

    // UI элементы для отображения доступных опций
    private TMP_Text  ActionTextW;
    private TMP_Text  ActionTextS;
    private TMP_Text  ActionTextA;
    private TMP_Text  ActionTextD;

    private Image imageW;
    private Image imageA;
    private Image imageS;
    private Image imageD;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null)
            {
                mainCamera = brain.GetComponent<Camera>();
                if (mainCamera != null)
                {
                    Debug.Log("Камера успешно инициализирована.");
                }
                else
                {
                    Debug.LogError("Не удалось получить основную камеру.");
                }
            }
            else
            {
                Debug.LogError("CinemachineBrain не найден.");
            }
        }
        else
        {
            Debug.LogError("Виртуальная камера не найдена.");
        }

        if (thirdPersonControllerObject != null)
        {
            thirdPersonController = thirdPersonControllerObject.GetComponent<ThirdPersonController>();
            if (thirdPersonController == null)
            {
                Debug.LogError("Не найден скрипт ThirdPersonController на объекте.");
            }
        }
        else
        {
            Debug.LogError("Не указан объект с ThirdPersonController.");
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out hit, rayDistance, ~ignoreLayerMask) && !ItemInteractionUI.Instance.CheckIfActive())
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentHighlightedObject)
            {
                ResetUI();
                currentHighlightedObject = hitObject;
                stateTree = null;
                Debug.Log($"Навели на объект: {hitObject.name}");
                SetCircleColor(currentHighlightedObject, farColor);
            }

            float distance = Vector3.Distance(mainCamera.transform.position, hitObject.transform.position);
            if (distance <= 5f) // Если объект близкий
            {
                SetCircleColor(currentHighlightedObject, closeColor);
                stateTree = hitObject.GetComponent<StateTree>();
                if (Input.GetMouseButton(0) && stateTree != null) // ЛКМ удерживаем
                {
                    if (!isCameraFrozen)
                    {
                        Debug.Log($"Удерживаем ЛКМ на объекте: {hitObject.name}");
                        FreezeCamera(true);
                        isCameraFrozen = true;

                        if (thirdPersonController != null)
                        {
                            thirdPersonController.enabled = false;
                            Debug.Log("Контроллер персонажа отключен.");
                        }
                    }
                    SetCircleColor(currentHighlightedObject, clickedColor);

                    if (!isMouseHeld)
                    {
                        isMouseHeld = true;
                        ShowOptions(currentHighlightedObject);
                    }
                }
                else
                {
                    if (isCameraFrozen)
                    {
                        Debug.Log("ЛКМ отпущена.");
                        FreezeCamera(false);
                        isCameraFrozen = false;

                        if (thirdPersonController != null && !ItemInteractionUI.Instance.CheckIfActive())
                        {
                            thirdPersonController.enabled = true;
                            Debug.Log("Контроллер персонажа включен.");
                        }
                    }

                    if (isMouseHeld)
                    {
                        isMouseHeld = false;
                        ResetUI();
                    }
                }

                if (isMouseHeld)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        HandleOptionSelection(0);
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        HandleOptionSelection(1);
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        HandleOptionSelection(2);
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        HandleOptionSelection(3);
                    }
                }
            }
            else
            {
                SetCircleColor(currentHighlightedObject, farColor);
            }
        }
        else
        {
            ResetUI();
        }
    }

private void ShowOptions(GameObject obj)
{
    var availableActions = stateTree.GetAvailableActions();
    int index = 0;

    // Получение текстовых элементов
    ActionTextW = obj.transform.Find("UI/Options/W/ActionTextW")?.GetComponent<TMP_Text>();
    ActionTextA = obj.transform.Find("UI/Options/A/ActionTextA")?.GetComponent<TMP_Text>();
    ActionTextS = obj.transform.Find("UI/Options/S/ActionTextS")?.GetComponent<TMP_Text>();
    ActionTextD = obj.transform.Find("UI/Options/D/ActionTextD")?.GetComponent<TMP_Text>();

    // Получение изображений
    imageW = obj.transform.Find("UI/Options/W")?.GetComponent<Image>();
    imageA = obj.transform.Find("UI/Options/A")?.GetComponent<Image>();
    imageS = obj.transform.Find("UI/Options/S")?.GetComponent<Image>();
    imageD = obj.transform.Find("UI/Options/D")?.GetComponent<Image>();

    TMP_Text[] actionTexts = { ActionTextW, ActionTextS, ActionTextA, ActionTextD };
    Image[] actionImages = { imageW, imageS, imageA, imageD };

    // Устанавливаем текст и активируем изображения
    foreach (var action in availableActions.Values)
    {
        if (index < actionTexts.Length && actionTexts[index] != null && actionImages[index] != null)
        {
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


    private void HandleOptionSelection(int optionIndex)
    {
        var availableActions = stateTree.GetAvailableActions().Values.ToList();
        if (optionIndex >= 0 && optionIndex < availableActions.Count)
        {
            var selectedPath = availableActions[optionIndex];
            Debug.Log($"Вы выбрали: {selectedPath.uiText}");
            stateTree.MoveToNode(selectedPath.targetNode.nodeName);
            ResetUI();
            currentHighlightedObject = null;
            isMouseHeld = false;
        }
        else
        {
            Debug.LogWarning($"Неверный выбор опции: {optionIndex}");
        }
    }

    private void ResetUI()
    {
        if (currentHighlightedObject != null)
        {
            SetCircleColor(currentHighlightedObject, farColor);
            currentHighlightedObject = null;
        }

        if (ActionTextW != null && ActionTextA !=null && ActionTextS !=null && ActionTextD !=null)
        {
            TMP_Text [] actionTexts = { ActionTextW, ActionTextS, ActionTextA, ActionTextD };
            foreach (var text in actionTexts)
            {
                text.text = string.Empty;
                text.gameObject.SetActive(false);
            }
        }

        if (imageW != null && imageA !=null && imageS !=null && imageD !=null)
        {
            Image [] actionImages = { imageW, imageA, imageS, imageD };
            foreach (var image in actionImages)
            {
                image.gameObject.SetActive(false);
            }
        }
    }

    private void SetCircleColor(GameObject obj, Color color)
    {
        Transform circle = obj.transform.Find("UI/Circle");
        if (circle != null)
        {
            var image = circle.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.color = color;
            }
        }
    }

    

    private void FreezeCamera(bool freeze)
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            transposer.m_XDamping = freeze ? 1000f : 1f;
            transposer.m_YDamping = freeze ? 1000f : 1f;
            transposer.m_ZDamping = freeze ? 1000f : 1f;
        }
    }
}
