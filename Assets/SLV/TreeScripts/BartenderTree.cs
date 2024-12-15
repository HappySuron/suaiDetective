using UnityEngine;

public class BartenderTree : StateTree
{

    public AudioClip[] audioClips;
    void Awake()
    {
        treeName = "bartenderTree";


        Node startNode = new Node("Бармен стоит");
        Node diologNode = new Node("Говорим с барменом");

        Node diologNode1 = new Node("Решили помочь");
        Node diologNode2 = new Node("Нам все равно");

        Node diologNode3 = new Node("Говорим после мафии (плоха)");
        Node diologNode4 = new Node("Говорим после мафии (хорошо)");

        Node endNode1 = new Node("Грустный бармен");
        Node endNode2 = new Node("Счастливый бармен");


        Node diologNode5 = new Node("Уже говорили с мафией");
        Node diologNode6 = new Node("Уже говорили с мафией");


        startNode.OnEnterAction = () =>
        {
            Debug.Log("Debug Actions works good");
            AudioManager.Instance.PlayDialogue(audioClips[0], "Бармен... Как всегда в работе...");
        };

        
        diologNode.OnEnterAction = () =>
        {
            if (!DialogUI.Instance.CheckIfDialogActive())
            {
                // Если диалог не активен, показываем его
                DialogUI.Instance.ShowDialog(this);
            }
            AudioManager.Instance.PlayDialogue(audioClips[7], "Детектив! Слушайте я немного занят, так что если вы не волшебник, что заставит мафию исчезнуть, мне все равно.");

            Debug.Log(Omniscient.Instance.tokens["noHelpForBartender"]);
            Debug.Log(Omniscient.Instance.tokens["didHelpForBartender"]);
            Debug.Log(Omniscient.Instance.tokens["noTalkToMafia"]);
        };

        diologNode1.OnEnterAction = () =>
        {
            AudioClip[] clips = { audioClips[2], audioClips[9] };
            string[] subtitles = { "Ладно, я помогу. Так что тебе надо?", "Спасибо. Все что мне от вас нужно это поговорить с мафией и прийти к заключению." };
            AudioManager.Instance.PlayDialogueSequence(clips, subtitles);
            DialogUI.Instance.HideDialog();
        };

        diologNode2.OnEnterAction = () =>
        {
            AudioClip[] clips = { audioClips[1], audioClips[8] };
            string[] subtitles = { "Мне не интересно.", "Да я догадывался. Таким как я никогда не дают руку помощи." };
            AudioManager.Instance.PlayDialogueSequence(clips, subtitles);
            DialogUI.Instance.HideDialog();
            MoveToNode("Грустный бармен");
            Omniscient.Instance.tokens["noHelpForBartender"] = 10;

        };


        
        diologNode3.OnEnterAction = () =>
        {
            if (!DialogUI.Instance.CheckIfDialogActive())
            {
                // Если диалог не активен, показываем его
                DialogUI.Instance.ShowDialog(this);
            }
            AudioClip[] clips = { audioClips[3], audioClips[11] };
            string[] subtitles = {"Я ничего не сделал. Ты сам по себе", "Я понял. Чтож предется разбираться самому. Спасибо за честность." };
            AudioManager.Instance.PlayDialogueSequence(clips, subtitles);
            DialogUI.Instance.HideDialog();
            MoveToNode("Грустный бармен");
        };

        diologNode4.OnEnterAction = () =>
        {
            if (!DialogUI.Instance.CheckIfDialogActive())
            {
                // Если диалог не активен, показываем его
                DialogUI.Instance.ShowDialog(this);
            }
            AudioClip[] clips = { audioClips[4], audioClips[10] };
            string[] subtitles = {"Я поговорил с мафией", "*Очень много благодарности*" };
            AudioManager.Instance.PlayDialogueSequence(clips, subtitles);
            DialogUI.Instance.HideDialog();
            MoveToNode("Счастливый бармен");
        };

        diologNode6.OnEnterAction = () =>
        {
            MoveToNode("Говорим после мафии (плоха)");
        };

        diologNode5.OnEnterAction = () =>
        {
            MoveToNode("Говорим после мафии (хорошо)");
        };




        startNode.AddPath(new Path(startNode, uiText:"Осмотреть"));
        startNode.AddPath(new Path(diologNode, uiText:"Поговить"));


        diologNode.AddPath(new Path(diologNode1, "noTalkToMafia", 1, uiText:"Ладно. Я помогу."));
        diologNode.AddPath(new Path(diologNode2, "noTalkToMafia", 1, uiText:"Не интересно"));


        diologNode1.AddPath(new Path(diologNode3, "noHelpForBartender", 1, uiText:"Поговорить"));
        diologNode1.AddPath(new Path(diologNode4, "didHelpForBartender", 1, uiText:"Поговорить +"));


        diologNode.AddPath(new Path(diologNode5, "didHelpForBartender", 1, uiText:"Я уже говорил с ним"));
        diologNode.AddPath(new Path(diologNode6, "noHelpForBartender", 1, uiText:"Я уже говорил с ним"));


        diologNode5.AddPath(new Path(diologNode4));
        diologNode6.AddPath(new Path(diologNode3));

        diologNode2.AddPath(new Path(endNode1));
        diologNode3.AddPath(new Path(endNode1));
        diologNode4.AddPath(new Path(endNode2));

        // diologNode1.AddPath(new Path(stateAfterDiolog1));
        // diologNode2.AddPath(new Path(stateAfterDiolog2));

        nodes.Add(startNode);
        nodes.Add(diologNode);
        nodes.Add(diologNode1);
        nodes.Add(diologNode2);
        nodes.Add(diologNode3);
        nodes.Add(diologNode4);
        nodes.Add(diologNode5);
        nodes.Add(diologNode6);

        nodes.Add(endNode1);
        nodes.Add(endNode2);
        // nodes.Add(stateAfterDiolog1);
        // nodes.Add(stateAfterDiolog2);


        currentNode = startNode;
    }
}
