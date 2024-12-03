using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public GameObject uiPrefab; // Префаб UI
    private GameObject uiInstance; // Экземпляр UI

    void Start()
    {
        // Создаем UI и отключаем его
        if (uiPrefab != null)
        {
            uiInstance = Instantiate(uiPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity, transform);
            uiInstance.SetActive(false); // Скрыть по умолчанию
        }
    }

    public void ShowUI()
    {
        if (uiInstance != null)
        {
            uiInstance.SetActive(true); // Показать UI
        }
    }

    public void HideUI()
    {
        if (uiInstance != null)
        {
            uiInstance.SetActive(false); // Скрыть UI
        }
    }
}
