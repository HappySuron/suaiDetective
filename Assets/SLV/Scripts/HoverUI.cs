using UnityEngine;

public class HoverUI : MonoBehaviour
{
    public GameObject circleUI; // Ссылка на белый круг в Canvas
    public Transform attachmentPoint; // Точка привязки (например, над головой объекта)
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (circleUI != null)
            circleUI.SetActive(false); // Скрыть круг по умолчанию
    }

    private void Update()
    {
        if (circleUI != null)
        {
            // Преобразуем мировые координаты точки привязки объекта в экранные координаты
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(attachmentPoint.position);

            // Обновляем позицию UI в Canvas
            circleUI.transform.position = screenPosition;
        }
    }

    private void OnMouseEnter()
    {
        if (circleUI != null)
        {
            circleUI.SetActive(true); // Показать круг, когда курсор на объекте
        }
    }

    private void OnMouseExit()
    {
        if (circleUI != null)
        {
            circleUI.SetActive(false); // Скрыть круг, когда курсор уходит с объекта
        }
    }
}
