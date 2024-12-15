using System.Collections.Generic;
using UnityEngine;

public class Omniscient : MonoBehaviour
{
    public Dictionary<string, int> tokens = new Dictionary<string, int>();
    public StateTree keyTree;
    public StateTree doorTree;

    public Bar barTree;

    public BartenderTree bartenderTree;
    public static Omniscient Instance {get; private set;}
    void Start()
    {
        if(Instance != null && Instance != this){
            Destroy(this);
        } else {
            Instance = this;
        }

        // Дополнительная проверка, если Instance равен null
        if (Instance == null)
        {
            Debug.LogError("Omniscient instance is null!");
        }
        // Инициализируем жетоны
        tokens["key"] = 0;
        tokens["none"] = 0;

        tokens["noHelpForBartender"] = 0;
        tokens["didHelpForBartender"] =0;
        tokens["noTalkToMafia"] = 1;
        // doorTree.ShowAvailableActions();
        // keyTree.ShowAvailableActions();
        // doorTree.MoveToNode("Дверь открыта", this);
        // Проверим взаимодействие
        // keyTree.MoveToNode("Ключ доступен", this);
        // tokens["key"] = 1; // Дадим игроку ключ
        // doorTree.MoveToNode("Дверь открыта", this);
        // keyTree.MoveToNode("Ключ взят", this);
        // keyTree.MoveToNode("Ключ тест", this);
    }

    // Проверка наличия токенов
    public bool HasTokens(string tokenType, int amount)
    {
        return tokens.ContainsKey(tokenType) && tokens[tokenType] >= amount;
    }

    //Использование токенов
    public void UseTokens(string tokenType, int amount)
    {
        if (HasTokens(tokenType, amount))
        {
            tokens[tokenType] -= amount;
            Debug.Log($"Использовано {amount} токенов {tokenType}. Осталось: {tokens[tokenType]}.");
        }
    }


}
