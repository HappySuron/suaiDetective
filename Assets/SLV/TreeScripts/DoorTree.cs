using UnityEngine;

public class DoorTree : StateTree
{
    void Awake()
    {
        treeName = "DoorTree";

        // Создаем узлы
        Node doorClosed = new Node("Дверь закрыта");
        Node doorOpen = new Node("Дверь открыта");

        // Добавляем пути
        doorClosed.AddPath(new Path(doorOpen, "key", 1, "enabled", "Открыть дверь"));

        // Заполняем список узлов
        nodes.Add(doorClosed);
        nodes.Add(doorOpen);

        // Устанавливаем начальный узел
        currentNode = doorClosed;
    }
}
