
using StarterAssets;
using UnityEngine;


public enum GameMode
{
    Exploration,      // Исследование мира
    CatScene,         // Диалог/кат-сцена
    ObjectInspection  // Рассмотрение объекта
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameMode CurrentGameMode { get; private set; } = GameMode.Exploration;

    ThirdPersonController thirdPersonController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Входим в режим диалога/кат-сцены
    public void EnterCatScene()
    {
        CurrentGameMode = GameMode.CatScene;
        Debug.Log("Вход в режим диалога/кат-сцены");
    }

    // Выходим из режима диалога/кат-сцены
    public void ExitCatScene()
    {
        CurrentGameMode = GameMode.Exploration;
        Debug.Log("Выход из режима диалога/кат-сцены");
    }

    // Входим в режим осмотра объекта
    public void EnterObjectInspection()
    {
        CurrentGameMode = GameMode.ObjectInspection;
        Debug.Log("Вход в режим осмотра объекта");
    }

    // Выходим из режима осмотра объекта
    public void ExitObjectInspection()
    {
        CurrentGameMode = GameMode.Exploration;
        Debug.Log("Выход из режима осмотра объекта");
    }

    public GameMode CheckCurrentMode(){
        return CurrentGameMode;
    }
}
