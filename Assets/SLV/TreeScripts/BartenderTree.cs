using UnityEngine;

public class BartenderTree : StateTree
{

    public AudioClip audioClip;
    void Awake()
    {
        treeName = "bartenderTree";


        Node startNode = new Node("Бармен стоит");
        Node diologNode = new Node("Говорим с барменом");

        Node diologNode1 = new Node("Спросили как сам");
        Node diologNode2 = new Node("Послали");

        Node stateAfterDiolog1 = new Node("Бармен доволен");
        Node stateAfterDiolog2 = new Node("Бармен недоволен");

        startNode.OnEnterAction = () =>
        {
            Debug.Log("Debug Actions works good");
            AudioManager.Instance.PlayDialogue(audioClip, "осмотрел бармена");
        };

        startNode.AddPath(new Path(startNode, uiText:"Осмотреть"));
        startNode.AddPath(new Path(diologNode, uiText:"Поговить"));


        diologNode.AddPath(new Path(diologNode1, uiText:"Как сам"));
        diologNode.AddPath(new Path(diologNode1, uiText:"FU"));


        diologNode1.AddPath(new Path(stateAfterDiolog1));
        diologNode2.AddPath(new Path(stateAfterDiolog2));

        nodes.Add(startNode);
        nodes.Add(diologNode);
        nodes.Add(diologNode1);
        nodes.Add(diologNode2);
        nodes.Add(stateAfterDiolog1);
        nodes.Add(stateAfterDiolog2);


        currentNode = startNode;
    }
}
