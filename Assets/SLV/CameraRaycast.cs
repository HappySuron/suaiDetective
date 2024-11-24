using UnityEngine;
using Cinemachine;

public class CameraRaycast : MonoBehaviour
{
    public float rayDistance = 100f; // Максимальная дистанция луча
    public LayerMask ignoreLayerMask; // Маска слоев для игнорирования
    public GameObject currentHighlightedObject; // Текущий выделенный объект
    public Color farColor = Color.gray; // Цвет круга для объекта вне досягаемости
    public Color closeColor = Color.yellow; // Цвет круга для объекта в досягаемости
    public Color clickedColor = Color.green; // Цвет круга при удержании ЛКМ

    private Camera mainCamera; // Ссылка на камеру
    private bool isClicking = false; // Флаг для отслеживания удержания ЛКМ
    private CinemachineVirtualCamera virtualCamera; // Ссылка на виртуальную камеру
    private bool isCameraFrozen = false; // Флаг для замораживания камеры
    private CinemachineTransposer transposer; // Ссылка на Cinemachine Transposer для управления движением камеры

    public Transform target; // Ссылка на объект, за которым следить

    public void Start()
    {
        // Получаем ссылку на виртуальную камеру
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>(); // Получаем доступ к компоненту Transposer для управления камерой
            if (transposer != null)
            {
                Debug.Log("Cinemachine Transposer найден.");
            }
            else
            {
                Debug.LogError("Не удалось найти CinemachineTransposer.");
            }

            // Получаем объект камеры через CinemachineBrain
            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null)
            {
                mainCamera = brain.GetComponent<Camera>();
                if (mainCamera != null)
                {
                    Debug.Log("Виртуальная камера найдена! Камера получена.");
                }
                else
                {
                    Debug.LogError("Не удалось найти камеру через CinemachineBrain.");
                }
            }
            else
            {
                Debug.LogError("CinemachineBrain не найден.");
            }
        }
        else
        {
            Debug.LogError("Виртуальная камера не найдена!");
        }

        if (mainCamera == null)
        {
            Debug.LogError("Камера не найдена!");
        }
    }

    public void Update()
    {
        if (mainCamera == null) return;

        // Создаем луч из центра камеры
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green); // Отладочный луч

        if (Physics.Raycast(ray, out hit, rayDistance, ~ignoreLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Проверяем, изменился ли объект
            if (hitObject != currentHighlightedObject)
            {
                if (currentHighlightedObject != null)
                {
                    Debug.Log($"Перестали смотреть на объект: {currentHighlightedObject.name}");
                }

                Debug.Log($"Смотрим на объект: {hitObject.name}");
                ResetUI(); // Сбрасываем UI для предыдущего объекта

                currentHighlightedObject = hitObject;
                SetCircleColor(currentHighlightedObject, farColor); // Устанавливаем цвет круга
                SetOptionsActive(currentHighlightedObject, true); // Включаем UI
            }

            // Проверяем расстояние до объекта
            float distance = Vector3.Distance(mainCamera.transform.position, hitObject.transform.position);
            if (distance <= 5f)
            {
                SetCircleColor(currentHighlightedObject, closeColor); // Близкий объект

                // Проверка удержания ЛКМ
                if (Input.GetMouseButton(0))
                {
                    isClicking = true; // Фиксируем удержание
                    SetCircleColor(currentHighlightedObject, clickedColor); // Зеленый круг
                    FreezeCamera(true); // Замораживаем камеру при удержании ЛКМ
                }

                // Проверка отпускания ЛКМ
                if (isClicking && Input.GetMouseButtonUp(0))
                {
                    ProcessOptions(currentHighlightedObject); // Обработка выбора
                    isClicking = false; // Сбрасываем удержание
                    FreezeCamera(false); // Размораживаем камеру при отпускании ЛКМ
                }
            }
            else
            {
                SetCircleColor(currentHighlightedObject, farColor); // Слишком далеко
            }
        }
        else
        {
            ResetUI(); // Сбрасываем UI, если луч никуда не попал
        }
    }

    private void ResetUI()
    {
        if (currentHighlightedObject != null)
        {
            Debug.Log($"Объект {currentHighlightedObject.name} больше не выделен.");
            SetCircleColor(currentHighlightedObject, farColor); // Цвет по умолчанию
            SetOptionsActive(currentHighlightedObject, false); // Отключаем Options
            currentHighlightedObject = null;
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
                image.color = color; // Меняем цвет круга
            }
        }
    }

    private void SetOptionsActive(GameObject obj, bool isActive)
    {
        Transform options = obj.transform.Find("UI/Options");
        if (options != null)
        {
            options.gameObject.SetActive(isActive); // Включаем/выключаем Options
        }
    }

    private void ProcessOptions(GameObject obj)
    {
        Transform options = obj.transform.Find("UI/Options");
        if (options != null)
        {
            RectTransform inspect = options.Find("Inspect")?.GetComponent<RectTransform>();
            RectTransform action = options.Find("Action")?.GetComponent<RectTransform>();

            // Получаем текущую позицию мыши
            Vector2 mousePosition = Input.mousePosition;

            // Проверяем, где была отпущена мышь
            if (inspect != null && RectTransformUtility.RectangleContainsScreenPoint(inspect, mousePosition))
            {
                Debug.Log("Inspected");
                FreezeCamera(true); // Замораживаем камеру при выборе объекта
            }
            else if (action != null && RectTransformUtility.RectangleContainsScreenPoint(action, mousePosition))
            {
                Debug.Log("Actioned");
                FreezeCamera(true); // Замораживаем камеру при выборе действия
            }
        }
    }

    // Функция для заморозки камеры
    private void FreezeCamera(bool freeze)
    {
        isCameraFrozen = freeze;

        if (freeze)
        {
            // Отключаем движение и вращение камеры
            transposer.m_XDamping = 0;
            transposer.m_YDamping = 0;
            transposer.m_ZDamping = 0;
        }
        else
        {
            // Восстанавливаем движение и вращение камеры
            transposer.m_XDamping = 1;
            transposer.m_YDamping = 1;
            transposer.m_ZDamping = 1;
        }
    }
}
