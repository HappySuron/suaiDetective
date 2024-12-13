using UnityEngine;

public class KeyTree : StateTree
{
    void Awake()
    {
        treeName = "KeyTree";

        // Создаем узлы и указываем, можно ли с ними взаимодействовать
        Node keyAvailable = new Node("Ключ доступен", true);
        Node keyTaken = new Node("Ключ взят", true);
        Node keyTest = new Node("Ключ тест", false);  // Этот узел нельзя выбрать

        // Добавляем пути с текстами для UI
        keyAvailable.AddPath(new Path(keyTaken, "key", 0, "enabled", "Взять ключ"));
        keyTaken.AddPath(new Path(keyTest, "key", 0, "enabled", "Использовать ключ"));

        // Заполняем список узлов
        nodes.Add(keyAvailable);
        nodes.Add(keyTaken);
        nodes.Add(keyTest);

        // Устанавливаем начальный узел
        currentNode = keyAvailable;
    }
}
