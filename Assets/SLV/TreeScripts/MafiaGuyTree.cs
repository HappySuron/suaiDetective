using UnityEngine;

public class MafiaGuyTree : StateTree
{
    public AudioClip[] audioClips;
    void Awake()
    {
        treeName = "MafiaGuyTree";


        Node startNode = new Node("Мафия стоит");
        Node diologNode = new Node("Говорим с мафией");

        Node diologNode1 = new Node("Помогли");
        Node diologNode2 = new Node("Не помогли");

        Node endNode1 = new Node("Мафия с деньгами");
        Node endNode2 = new Node("Мафия без денег");

        startNode.AddPath(new Path(startNode, uiText:"Осмотреть"));
        startNode.AddPath(new Path(diologNode, uiText:"Поговорить"));
        diologNode.AddPath(new Path(diologNode1, uiText:"Заплатить за бармена"));
        diologNode.AddPath(new Path(diologNode2, uiText:"Отказаться помогать"));
        diologNode1.AddPath(new Path(endNode1));
        diologNode2.AddPath(new Path(endNode2));


        startNode.OnEnterAction = () =>
        {
            Debug.Log("Debug Actions works good");
            AudioManager.Instance.PlayDialogue(audioClips[5], "Иногда в барах нет членов мафии. Никогда не видел эти 'иногда'");
        };

        diologNode.OnEnterAction = () =>
        {
            if (!DialogUI.Instance.CheckIfDialogActive())
            {
                // Если диалог не активен, показываем его
                DialogUI.Instance.ShowDialog(this);
            }
            Omniscient.Instance.tokens["noTalkToMafia"] -= 1;
            AudioManager.Instance.PlayDialogue(audioClips[0], "Что же у нас тут. Если у тебя нет денег бармена, то ты тратишь мое время. Нет денег - мы не уйдем.");
        };

        diologNode1.OnEnterAction = () =>
        {
            AudioClip[] clips = { audioClips[1], audioClips[3] };
            string[] subtitles = { "Хорошо, я заплачу за бармена. Вот деньги.", "Повезло бармену что у него есть такие друзья. Но помни он все еще нам должен с процентами" };
            AudioManager.Instance.PlayDialogueSequence(clips, subtitles);
            Omniscient.Instance.tokens["didHelpForBartender"] += 1;
            DialogUI.Instance.HideDialog();
            MoveToNode("Мафия с деньгами");
        };

        diologNode2.OnEnterAction = () =>
        {
            AudioClip[] clips = { audioClips[2], audioClips[4] };
            string[] subtitles = { "Я не буду вмешиваться. Разбирайтесь сами.", "Справедливо. Надеюсь бармен понимает, чем рискует, мы всегда получаем то что хотим" };
            AudioManager.Instance.PlayDialogueSequence(clips, subtitles);
            DialogUI.Instance.HideDialog();
            Omniscient.Instance.tokens["noHelpForBartender"] += 1;
            MoveToNode("Мафия без денег");

        };


        // diologNode1.AddPath(new Path(stateAfterDiolog1));
        // diologNode2.AddPath(new Path(stateAfterDiolog2));

        nodes.Add(startNode);
        nodes.Add(diologNode);
        nodes.Add(diologNode1);
        nodes.Add(diologNode2);
        nodes.Add(endNode1);
        nodes.Add(endNode2);


        currentNode = startNode;
    }

}
