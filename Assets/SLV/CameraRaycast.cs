using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    public float rayDistance = 100f; // Максимальная дистанция луча
    public LayerMask ignoreLayerMask; // Маска слоев для игнорирования
    public GameObject currentHighlightedObject; // Текущий выделенный объект

    private Camera mainCamera; // Ссылка на основную камеру

    void Start()
    {
        // Получаем ссылку на основную камеру
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Проверяем, если камера не найдена
        if (mainCamera == null)
        {
            Debug.LogError("Основная камера не найдена!");
            return;
        }

        // Создаем луч из центра экрана
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Нарисуем луч в сцене для визуализации
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        // Используем Raycast с маской слоев
        if (Physics.Raycast(ray, out hit, rayDistance, ~ignoreLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Проверяем, является ли объект тем же, что и текущий выделенный
            if (hitObject != currentHighlightedObject)
            {
                // Если был выделен другой объект, выводим сообщение в консоль
                if (currentHighlightedObject != null)
                {
                    Debug.Log("Объект покидает область выделения: " + currentHighlightedObject.name);
                }

                // Устанавливаем новый выделенный объект
                currentHighlightedObject = hitObject;

                // Выводим сообщение о новом выделенном объекте
                Debug.Log("Объект выделен: " + currentHighlightedObject.name);
            }
        }
        else
        {
            // Если луч не попал ни в один объект, сбрасываем выделение и выводим сообщение
            if (currentHighlightedObject != null)
            {
                Debug.Log("Объект покидает область выделения: " + currentHighlightedObject.name);
                currentHighlightedObject = null; // Сбрасываем текущий объект
            }
        }
    }
}
