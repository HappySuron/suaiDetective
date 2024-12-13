using UnityEngine;

public class Bar : StateTree
{
    void Awake()
    {
        treeName = "BarTree";

        // Создаем узлы
        Node barFirstOne = new Node("Бар первичный узел");
        Node barSecond1 = new Node("Бар вторичный узел 1");
        Node barSecond2 = new Node("Бар вторичный узел 2");
        Node barSecond3 = new Node("Бар вторичный узел 3");
        Node barSecond4 = new Node("Бар вторичный узел 4");
        Node barThird1 = new Node("Бар третичный узел 1");
        Node barThird2 = new Node("Бар третичный узел 2");

        // Добавляем пути
        barFirstOne.AddPath(new Path(barSecond1, "none", 0, uiText:"Перейти в первичный 1"));
        barFirstOne.AddPath(new Path(barSecond2, uiText:"Перейти во вторичный 2"));
        barFirstOne.AddPath(new Path(barSecond3, uiText:"Перейти во вторичный 3"));
        barFirstOne.AddPath(new Path(barSecond4, uiText:"Перейти во вторичный 4"));

        barSecond1.AddPath(new Path(barThird1, uiText:"Перейти в третичный 1"));
        barSecond1.AddPath(new Path(barThird2, uiText:"Перейти в третичный 2"));
        // Заполняем список узлов

        nodes.Add(barFirstOne);
        nodes.Add(barSecond1);
        nodes.Add(barSecond2);
        nodes.Add(barSecond3);
        nodes.Add(barSecond4);
        nodes.Add(barThird1);
        nodes.Add(barThird2);
        // Устанавливаем начальный узел
        currentNode = barFirstOne;
    }
}
