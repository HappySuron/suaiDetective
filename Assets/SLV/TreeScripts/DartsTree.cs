using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class DartsTree : StateTree
{
    [Header ("ItemInfoLook")]
    public GameObject player;
    public AudioClip[] audioClips;


    [Header ("ItemDescriptionInfo")]
    public string itemName = "Дартс на стене";
    public string itemDescription = "Мишень на стене выглядит потрёпано. Цвета выцвели, цифры смазаны — особенно 20 и 18. В центре глубокая вмятина, будто не один раз туда попадали слишком сильные броски. Края помяты, рамка изношена, на ней видны следы рук игроков. Всё это вместе создаёт впечатление, что мишень уже давно служит, не выдерживая частых ударов и лишённая былого блеска.";

    public Sprite itemImage;
    void Awake()
    {
        treeName = "BarTree";

        // Создаем узлы
        Node dartsNodeStart = new Node("Дартс стоит");
        Node dartsFirstOption = new Node("Осмотрел дартс");
        Node dartsSecondOption = new Node("Исследование дартс");

        dartsFirstOption.OnEnterAction = () =>
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAA");
            AudioManager.Instance.PlayDialogue(audioClips[0], "Я не то что бы рад веселью.");
            MoveToNode("Дартс стоит");
        };

        dartsSecondOption.OnEnterAction = () =>
        {
            ItemInteractionUI.Instance.ShowItemInterface(name: itemName, description: itemDescription, image: itemImage);
            MoveToNode("Дартс стоит");
        };




        // Добавляем пути
        dartsNodeStart.AddPath(new Path(dartsFirstOption, "none", 0, uiText:"Осмотреть"));
        dartsNodeStart.AddPath(new Path(dartsSecondOption, uiText:"Исследовать"));
        dartsFirstOption.AddPath(new Path(dartsNodeStart));
        dartsSecondOption.AddPath(new Path(dartsNodeStart));

        // Заполняем список узлов


        nodes.Add(dartsNodeStart);
        nodes.Add(dartsFirstOption);
        nodes.Add(dartsSecondOption);
        // Устанавливаем начальный узел
        currentNode = dartsNodeStart;
        


    }
}
