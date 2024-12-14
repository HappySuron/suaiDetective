
using System;
using System.Collections.Generic;

[System.Serializable]

    public class Node
    {
        public string nodeName;
        public bool isInteractive;  // Флаг, можно ли взаимодействовать с узлом
        public List<Path> paths = new List<Path>();

        // Делегат для выполнения действий при входе в узел
        public Action OnEnterAction;

        public Node(string name, bool isInteractive = true)
        {
            this.nodeName = name;
            this.isInteractive = isInteractive;
        }

        // Вызывается при входе в узел
        public virtual void OnEnter()
        {
            OnEnterAction?.Invoke(); // Выполнить действие, если оно задано
        }

        // Добавляем путь
        public void AddPath(Path path)
        {
            paths.Add(path);
        }
    }
public class Path
{
    public Node targetNode;
    public string tokenType;
    public int tokensRequired;
    public string pathStatus;
    public string uiText;  // Текстовое описание для UI

    public Path(Node target, string tokenType = "none", int tokensRequired = 0, string pathStatus = "enabled", string uiText = "")
    {
        this.targetNode = target;
        this.tokenType = tokenType;
        this.tokensRequired = tokensRequired;
        this.pathStatus = pathStatus;
        this.uiText = uiText;  // Добавляем текстовое описание
    }
}