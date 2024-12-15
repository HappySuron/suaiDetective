using System.Collections.Generic;
using UnityEngine;

public class StateTree : MonoBehaviour
{
    public string treeName;
    public Node currentNode;
    public List<Node> nodes = new List<Node>();

    // Метод для перехода в узел
    public void MoveToNode(string targetNodeName)
    {
        Omniscient _omniscient = Omniscient.Instance;
        if (_omniscient == null)
        {
            Debug.LogError("Omniscient instance is null!");
            return;
        }

        Node targetNode = nodes.Find(node => node.nodeName == targetNodeName);
        if (targetNode == null)
        {
            Debug.Log($"Узел {targetNodeName} не найден в дереве {treeName}.");
            return;
        }

        Path availablePath = currentNode.paths.Find(path => path.targetNode == targetNode);
        if (availablePath == null)
        {
            Debug.Log($"Нет доступного пути к узлу {targetNodeName}.");
            return;
        }

        if (_omniscient.HasTokens(availablePath.tokenType, availablePath.tokensRequired))
        {
          //  _omniscient.UseTokens(availablePath.tokenType, availablePath.tokensRequired);
            currentNode = targetNode;

            Debug.Log($"Перешли в узел: {currentNode.nodeName}");
            currentNode.OnEnter(); // Выполняем действие узла
        }
        else
        {
            Debug.Log($"Недостаточно токенов для перехода в узел: {targetNode.nodeName}.");
        }
    }

    // Метод для вывода доступных действий
    public void ShowAvailableActions()
    {
        Omniscient _omniscient = Omniscient.Instance;
        if (_omniscient==null) Debug.Log("dfsdfasdfadsfasdfasdfsadf");
        Debug.Log("Доступные действия:");
        foreach (var path in currentNode.paths)
        {
            // Проверяем, доступен ли путь
            if (path.pathStatus == "enabled" && currentNode.isInteractive)
            {
                // Если токены доступны для перехода, выводим текст
                Debug.Log($"- {path.uiText} (Токен: {path.tokenType}, Требуется: {path.tokensRequired})");
            }
        }
    }

        // Метод для получения доступных действий как словарь
    public Dictionary<string, Path> GetAvailableActions()
    {
        Omniscient _omniscient = Omniscient.Instance;
        Debug.Log("GOT HERE");

        if (_omniscient == null)
        {
            Debug.LogError("Omniscient instance is null!");
        }

        Dictionary<string, Path> actions = new Dictionary<string, Path>();

        Debug.Log($"Итерация по путям узла {currentNode.paths.Count} шт.");

        foreach (var path in currentNode.paths)
        {
            Debug.Log($"Проверка пути: {path.uiText}, Статус: {path.pathStatus}, Токены требуемые: {path.tokensRequired}");

            if (path.pathStatus == "enabled" &&
                _omniscient.HasTokens(path.tokenType, path.tokensRequired))
            {
                actions[path.uiText] = path; // Текст действия как ключ
                Debug.Log($"Добавлено действие: {path.uiText}");
            }
            else
            {
                Debug.Log($"Действие {path.uiText} не добавлено. Условия не выполнены.");
            }
        }

        Debug.Log($"Общее количество доступных действий: {actions.Count}");

        return actions;
    }


    // Метод для получения доступных действий как список
    public List<Path> GetAvailableActionsAsList()
    {
   
        Omniscient _omniscient = Omniscient.Instance;
        
        // Получаем доступные действия как словарь
        Dictionary<string, Path> availableActions = GetAvailableActions();
        
        // Логируем полученные доступные действия
        Debug.Log($"Полученные доступные действия (количество: {availableActions.Count}):");
        foreach (var action in availableActions)
        {
            Debug.Log($"Ключ: {action.Key}, Текст действия: {action.Value.uiText}, Целевой узел: {action.Value.targetNode.nodeName}");
        }

        // Преобразуем словарь в список
        return new List<Path>(availableActions.Values);
    }

}
