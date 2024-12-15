using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class PaperTree : StateTree
{
    [Header ("ItemInfoLook")]
    public GameObject player;
    public AudioClip[] audioClips;


    [Header ("ItemDescriptionInfo")]
    public string itemName = "Письмо мафии";
    public string itemDescription = "Ты ведь помнишь наш уговор?\n" +
"Деньги должны были вернуться к нам ещё две недели назад. Но вместо этого я слышу только пустое эхо.\n\n" +
"Я надеюсь, ты не решил, что можешь играть с нашим терпением. Семья Мортелли всегда выполняет свои обещания — и ждет того же от своих друзей.\n\n" +
"Ты ведь не хочешь, чтобы мы перешли от слов к действиям? Помни, у каждого долга есть своя цена. И если ты не отдашь то, что должен, мы заберём другое.\n\n" +
"Даю тебе ещё один шанс, до конца недели. После этого разговор будет совсем другим.\n\n" +
"С уважением,\n" +
"А. Мортелли";

    public Sprite itemImage;
    void Awake()
    {
        treeName = "BarTree";

        // Создаем узлы
        Node paperNodeStart = new Node("Бумага лежит");
        Node paperFirstOption = new Node("Осмотрел бумаги");
        Node paperSecondOption = new Node("Исследование бумаги");

        paperFirstOption.OnEnterAction = () =>
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAA");
            AudioManager.Instance.PlayDialogue(audioClips[0], "Он и вправду им должен и много.");
            MoveToNode("Бумага лежит");
        };

        paperSecondOption.OnEnterAction = () =>
        {
            ItemInteractionUI.Instance.ShowItemInterface(name: itemName, description: itemDescription, image: itemImage);
            MoveToNode("Бумага лежит");
        };




        // Добавляем пути
        paperNodeStart.AddPath(new Path(paperFirstOption, "none", 0, uiText:"Осмотреть"));
        paperNodeStart.AddPath(new Path(paperSecondOption, uiText:"Исследовать"));
        paperFirstOption.AddPath(new Path(paperNodeStart));
        paperSecondOption.AddPath(new Path(paperNodeStart));

        // Заполняем список узлов


        nodes.Add(paperNodeStart);
        nodes.Add(paperFirstOption);
        nodes.Add(paperSecondOption);
        // Устанавливаем начальный узел
        currentNode = paperNodeStart;
        


    }
}
