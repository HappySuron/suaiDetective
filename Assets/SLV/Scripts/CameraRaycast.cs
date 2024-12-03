using UnityEngine;
using Cinemachine;
using StarterAssets;

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

    private GameObject currentOptionsObject; // Объект UI Options, на который наведена мышь

    // Опции, доступные при удержании ЛКМ
    private string[] options = new string[] { "Inspect", "Action", "Talk", "Examine" };
    private bool isMouseHeld = false;

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

        // Пытаемся получить ссылку на скрипт ThirdPersonController на другом объекте
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

        // Создаем луч для наведения из центра экрана
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out hit, rayDistance, ~ignoreLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentHighlightedObject)
            {
                ResetUI();
                currentHighlightedObject = hitObject;
                Debug.Log($"Навели на объект: {hitObject.name}");
                SetCircleColor(currentHighlightedObject, farColor);
            }

            float distance = Vector3.Distance(mainCamera.transform.position, hitObject.transform.position);
            if (distance <= 5f) // Если объект близкий
            {
                SetCircleColor(currentHighlightedObject, closeColor);

                if (Input.GetMouseButton(0)) // ЛКМ удерживаем
                {
                    if (!isCameraFrozen)
                    {
                        Debug.Log($"Удерживаем ЛКМ на объекте: {hitObject.name}");
                        FreezeCamera(true);
                        isCameraFrozen = true;
                        SetOptionsActive(currentHighlightedObject, true);
                        // Отключаем контроллер персонажа
                        if (thirdPersonController != null)
                        {
                            thirdPersonController.enabled = false;
                            Debug.Log("Контроллер персонажа отключен.");
                        }
                    }
                    SetCircleColor(currentHighlightedObject, clickedColor); // Цвет круга изменится на "кликнутый"

                    // Показываем доступные опции
                    if (!isMouseHeld)
                    {
                        isMouseHeld = true;
                        ShowOptions();
                    }
                }
                else // ЛКМ отпущена
                {
                    if (isCameraFrozen)
                    {
                        Debug.Log("ЛКМ отпущена.");
                        FreezeCamera(false);
                        isCameraFrozen = false;

                        // Включаем контроллер персонажа
                        if (thirdPersonController != null)
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

                // Обработка выбора опций через WASD
                if (isMouseHeld)
                {
                    if (Input.GetKeyDown(KeyCode.W)) // W для Inspect
                    {
                        Debug.Log("Вы выбрали: Inspect");
                    }
                    if (Input.GetKeyDown(KeyCode.S)) // S для Action
                    {
                        Debug.Log("Вы выбрали: Action");
                    }
                    if (Input.GetKeyDown(KeyCode.A)) // A для Talk
                    {
                        Debug.Log("Вы выбрали: Talk");
                    }
                    if (Input.GetKeyDown(KeyCode.D)) // D для Examine
                    {
                        Debug.Log("Вы выбрали: Examine");
                    }
                }
            }
            else // Если объект далеко
            {
                SetCircleColor(currentHighlightedObject, farColor);
            }
        }
        else
        {
            ResetUI();
        }
    }

    // Замораживаем или разблокируем камеру
    private void FreezeCamera(bool freeze)
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            transposer.m_XDamping = freeze ? 1000f : 1f;
            transposer.m_YDamping = freeze ? 1000f : 1f;
            transposer.m_ZDamping = freeze ? 1000f : 1f;
            Debug.Log($"Камера заморожена: {freeze}");
        }
    }

    // Сбросить UI (если объект не наведен)
    private void ResetUI()
    {
        if (currentHighlightedObject != null)
        {
            Debug.Log($"Сброс выделения объекта: {currentHighlightedObject.name}");
            SetCircleColor(currentHighlightedObject, farColor);
            SetOptionsActive(currentHighlightedObject, false);
            currentHighlightedObject = null;
        }
    }

    // Изменение цвета круга
    private void SetCircleColor(GameObject obj, Color color)
    {
        Transform circle = obj.transform.Find("UI/Circle");
        if (circle != null)
        {
            var image = circle.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.color = color;
                Debug.Log($"Цвет круга установлен: {color}");
            }
        }
    }

    // Включение/выключение опций UI
    private void SetOptionsActive(GameObject obj, bool isActive)
    {
        Transform options = obj.transform.Find("UI/Options");
        if (options != null)
        {
            options.gameObject.SetActive(isActive);
            Debug.Log($"UI Options для {obj.name} активны: {isActive}");
        }
    }

    // Показываем доступные опции
    private void ShowOptions()
    {
        Debug.Log("Опции доступны. Нажмите W, S, A, D для выбора.");
    }
}
