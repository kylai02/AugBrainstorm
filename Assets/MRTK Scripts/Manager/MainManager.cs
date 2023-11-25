using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

using TMPro;


public class MainManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]
  // public List<TMP_Text> keywordsText;
  public List<TMP_Text> generatedKeywordsText;
  // public List<TMP_Text> ideasText;

  // public GameObject ideasField;
  public GameObject genBtnField;
  public GameObject tree;
  // public GameObject conditionsField;
  // public GameObject conditionBtnObj;

  // MainManager always holds the root of the Tree;
  public NodeSphere root;

  [HeaderAttribute("-------   Prefab    ------- ")]
  public GameObject nodePrefab;

  [HeaderAttribute("-------   Debug    ------- ")]
  public string initKeyword;
  public NodeSphere selectedNode;

  [HeaderAttribute("-------   temp public    ------- ")]
  public List<string> generatedKeywords;
  // public List<string> selectedKeywords;
  // public List<string> positiveConditions;
  // public List<string> nagativeConditions;

  // public List<GameObject> positiveConditionBtns;

  // public List<string> contextKeywords;
  // public List<string> generatedIdeas;

  // Singleton
  public static MainManager instance;
  

  // private List<string> conditions = new List<string> {"Event", "Music"}; // For testing
  // public bool _isPositive;

  void Awake() {
    if (!instance) instance = this;
  
    root = GameObject.Find("Root").GetComponent<NodeSphere>();
    // contextKeywords = new List<string>();

    // for (int i = 0; i < 8; ++i) {
    //   generatedKeywords.Add("");
    // }

    // _isPositive = true;
  }

  // void Start() {
  //   // Debug: test KeywordNode
  //   KeywordNode a = new KeywordNode("aaaaa", root);
  //   root.children.Add(a);
  //   KeywordNode b = new KeywordNode("bbbbb", a);
  //   a.children.Add(b);
  //   KeywordNode c = new KeywordNode("ccccc", a);
  //   a.children.Add(c);
  // }

  void Start() {
    // DEBUG: init node
    AddNode(initKeyword, root);


  //   // DEBUG: init conditions
  //   AddCondition("outdoor");
  //   AddCondition("game");
  }

  void Update() {

  //   // DEBUG: test contextKeywords
  //   if (Input.GetKeyDown(KeyCode.Space)) {
  //     string output = "";
  //     foreach (string s in contextKeywords) {
  //       output += s + " ";
  //     }

  //     Debug.Log(output);
  //     Debug.Log(contextKeywords[0]);
  //     Debug.Log(contextKeywords.Count);
  //     Debug.Log(contextKeywords.Capacity);
  //   }

  //   // DEBUG: test node UI
  //   // if (Input.GetKeyDown(KeyCode.A)) {
  //   //   KeywordNode cur = root;
  //   //   while (cur.children.Count > 0) cur = cur.children[0];

  //   //   NewKeywordNode("aaaaa", cur);
  //   // }

  //   // DEBUG: test condition toggle
  //   if (Input.GetKeyDown(KeyCode.P)) {
  //     _isPositive = !_isPositive;
  //   }

  //   if (Input.GetKeyDown(KeyCode.C)) {
  //     AddCondition("game");
  //   }

  //   UpdateKeywordButtons();
    UpdateGenKeywordsText();
  }

  public async void ChoseSelectedNode() {
    selectedNode = NodeSphere.SelectedNode;

    // UpdateGeneratedKeywords();
    genBtnField.SetActive(false);
    generatedKeywords = await OpenAI.OpenAI.instance.GetGeneratedKeywordsOpenAI(
      preKeywords: NodePath(),
      conditions: new List<string>(),
      requestKeywordNumber: 8
    );
    genBtnField.transform.position = selectedNode.transform.position;
    genBtnField.SetActive(true);
  }

  // private async void UpdateGeneratedKeywords() {
    // ideasField.SetActive(false);
    // List<string> positiveCond = new List<string>();
    // foreach (GameObject btn in positiveConditionBtns) {
    //   positiveCond.Add(btn.GetComponentInChildren<TMP_Text>().text);
    // }

  //   generatedKeywords = await OpenAI.OpenAI.instance.GetGeneratedKeywordsOpenAI(
  //     NodePath(),
  //     positiveCond,
  //     8
  //   );
    
  // }

  // public void AddSelectedWord(string s) {
  //   // selectedKeywords.Add(s);

  //   if (_isPositive) {
  //     positiveConditions.Add(s);
  //   }
  //   else {
  //     nagativeConditions.Add(s);
  //   }
  // }


  public void AddNode(string keyword, NodeSphere parent) {
    genBtnField.SetActive(false);
    GameObject newNodeObj = Instantiate(nodePrefab);
    newNodeObj.transform.SetParent(tree.transform); // Set new node obj under

    NodeSphere newNode = newNodeObj.GetComponent<NodeSphere>();

    // Set new node attribute
    newNode.keyword = keyword;
    newNode.parent = parent;
    parent.children.Add(newNode);
    newNodeObj.GetComponentInChildren<TMP_Text>().text = keyword;

    // Set new node position
    if (parent != root) {
      newNodeObj.transform.localPosition = new Vector3(
        parent.transform.localPosition.x + 0.26f,
        parent.transform.localPosition.y,
        parent.transform.localPosition.z
      );
    }
    else {
      newNodeObj.transform.localPosition = new Vector3(0, -0.1f, 0.5f);
    }
  }

  // public async void GenerateIdea() {
  //   generatedIdeas = await OpenAI.OpenAI.instance.GetGeneratedIdeasOpenAI(
  //     NodePath(),
  //     3,
  //     12
  //   );

  //   ideasField.SetActive(true);
  //   for (int i = 0; i < 4; ++i) {
  //     ideasText[i].text = generatedIdeas[i];
  //   }
  // }
  
  // public void AddCondition(string condition) {
  //   GameObject newCondition = Instantiate(conditionBtnObj);
  //   newCondition.transform.SetParent(conditionsField.transform);

  //   newCondition.GetComponentInChildren<TMP_Text>().text = condition;
  //   positiveConditionBtns.Add(newCondition);

  //   AdjustConditionBtnsPos();
  // }

  // public void DeleteCondition(GameObject target) {
  //   positiveConditionBtns.Remove(target);
  //   Destroy(target);

  //   AdjustConditionBtnsPos();
  // }

  // private void AdjustConditionBtnsPos() {
  //   for (int i = 0; i < positiveConditionBtns.Count; ++i) {
  //     GameObject btn = positiveConditionBtns[i];

  //     btn.transform.localPosition = new Vector3(
  //       160 * (i % 4),
  //       -100 * (i / 4),
  //       btn.transform.localPosition.z
  //     );
  //   }
  // }


  private List<string> NodePath() {
    List<string> arr = new List<string>();
    
    NodeSphere cur = NodeSphere.SelectedNode;
    while (cur != root) {
      arr.Add(cur.keyword);
      cur = cur.parent;
    }
    arr.Reverse();
    return arr;
  }

  // private void UpdateKeywordButtons() {
  //   for (int i = 0; i < 8; ++i) {
  //     int targetIndex = contextKeywords.Count - 1 - i;

  //     if (targetIndex >= 0)
  //       keywordsText[i].text = contextKeywords[targetIndex];
  //   }
  // }

  private void UpdateGenKeywordsText() {
    int len = Math.Min(generatedKeywordsText.Count, generatedKeywords.Count);
    for (int i = 0; i < len; ++i) {
      generatedKeywordsText[i].text = generatedKeywords[i];
    }

    if (len < 8) {
      for (int i = len; i < 8; ++i) {
        generatedKeywordsText[i].text = "";
      }
    }
  }
}
