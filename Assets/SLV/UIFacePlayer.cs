using UnityEngine;

public class UIFacePlayer : MonoBehaviour
{
    private Transform playerCamera;

    void Start()
    {
        // Найдите основную камеру
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        // Поверните UI к камере игрока
        transform.LookAt(playerCamera);
        transform.Rotate(0, 180, 0); // Убедитесь, что текст не зеркален
    }
}
