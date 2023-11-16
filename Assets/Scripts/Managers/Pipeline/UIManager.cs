using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using System;



public class UIManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]
  public List<TMP_Text> keywordsText;
  public List<TMP_Text> generatedText;
  public List<TMP_Text> ideasText;

  public GameObject keywordNodeObj;
  public GameObject tree;
  public GameObject ideasField;
  public GameObject generatedKeywordsField;
  public GameObject conditionsField;
  public GameObject conditionBtnObj;

  public string initKeyword;

  public List<string> selectedKeywords;
  public List<string> positiveConditions;
  public List<string> nagativeConditions;

  public List<GameObject> positiveConditionBtns;

  public List<string> contextKeywords;
  public List<string> generatedKeywords;
  public List<string> generatedIdeas;

  public KeywordNode root;

  public static UIManager instance;
  
  public KeywordNode selectedNode;

  // private List<string> conditions = new List<string> {"Event", "Music"}; // For testing
  public bool _isPositive;

  void Awake() {
    if (!instance) instance = this;

    contextKeywords = new List<string>();

    for (int i = 0; i < 8; ++i) {
      generatedKeywords.Add("");
    }

    _isPositive = true;
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
    NewKeywordNode(initKeyword, root);


    // DEBUG: init conditions
    AddCondition("outdoor");
    AddCondition("game");
  }

  void Update() {

    // DEBUG: test contextKeywords
    if (Input.GetKeyDown(KeyCode.Space)) {
      string output = "";
      foreach (string s in contextKeywords) {
        output += s + " ";
      }

      Debug.Log(output);
      Debug.Log(contextKeywords[0]);
      Debug.Log(contextKeywords.Count);
      Debug.Log(contextKeywords.Capacity);
    }

    // DEBUG: test node UI
    // if (Input.GetKeyDown(KeyCode.A)) {
    //   KeywordNode cur = root;
    //   while (cur.children.Count > 0) cur = cur.children[0];

    //   NewKeywordNode("aaaaa", cur);
    // }

    // DEBUG: test condition toggle
    if (Input.GetKeyDown(KeyCode.P)) {
      _isPositive = !_isPositive;
    }

    if (Input.GetKeyDown(KeyCode.C)) {
      AddCondition("game");
    }

    UpdateKeywordButtons();
    UpdateGeneratedKeywordsButtons();
  }

  public void AddSelectedWord(string s) {
    // selectedKeywords.Add(s);

    if (_isPositive) {
      positiveConditions.Add(s);
    }
    else {
      nagativeConditions.Add(s);
    }
  }

  public void ChoseSelectedNode(KeywordNode node) {
    selectedNode = node;
    if (selectedKeywords.Count != 0) {
      foreach (string s in selectedKeywords) {
        node.keyword += ", ";
        node.keyword += s;
      }
      selectedKeywords.Clear();
      node.GetComponentInChildren<TMP_Text>().text = node.keyword;
    }

    UpdateGeneratedKeywords();
  }

  public void NewKeywordNode(string keyword, KeywordNode parent) {
    generatedKeywordsField.SetActive(false);
    GameObject newNodeObj = Instantiate(keywordNodeObj);
    newNodeObj.transform.SetParent(tree.transform);

    KeywordNode newNode = newNodeObj.GetComponent<KeywordNode>();

    newNode.keyword = keyword;
    newNode.parent = parent;
    parent.children.Add(newNode);
    newNodeObj.GetComponentInChildren<TMP_Text>().text = keyword;

    if (parent != root) {
      newNodeObj.transform.localPosition = new Vector3(
        parent.transform.localPosition.x + 200,
        parent.transform.localPosition.y,
        parent.transform.localPosition.z
      );
    }
    else {
      newNodeObj.transform.localPosition = new Vector3(-600, -150, 0);
    }
  }

  public async void GenerateIdea() {
    generatedIdeas = await OpenAI.OpenAI.instance.GetGeneratedIdeasOpenAI(
      NodePath(),
      3,
      12
    );

    ideasField.SetActive(true);
    for (int i = 0; i < 4; ++i) {
      ideasText[i].text = generatedIdeas[i];
    }
  }
  
  public void AddCondition(string condition) {
    GameObject newCondition = Instantiate(conditionBtnObj);
    newCondition.transform.SetParent(conditionsField.transform);

    newCondition.GetComponentInChildren<TMP_Text>().text = condition;
    positiveConditionBtns.Add(newCondition);

    AdjustConditionBtnsPos();
  }

  public void DeleteCondition(GameObject target) {
    positiveConditionBtns.Remove(target);
    Destroy(target);

    AdjustConditionBtnsPos();
  }

  private void AdjustConditionBtnsPos() {
    for (int i = 0; i < positiveConditionBtns.Count; ++i) {
      GameObject btn = positiveConditionBtns[i];

      btn.transform.localPosition = new Vector3(
        160 * (i % 4),
        -100 * (i / 4),
        btn.transform.localPosition.z
      );
    }
  }

  private async void UpdateGeneratedKeywords() {
    ideasField.SetActive(false);
    List<string> positiveCond = new List<string>();
    foreach (GameObject btn in positiveConditionBtns) {
      positiveCond.Add(btn.GetComponentInChildren<TMP_Text>().text);
    }

    generatedKeywords = await OpenAI.OpenAI.instance.GetGeneratedKeywordsOpenAI(
      NodePath(),
      positiveCond,
      8
    );
    
    generatedKeywordsField.transform.position = selectedNode.transform.position;
    generatedKeywordsField.SetActive(true);
  }

  private List<string> NodePath() {
    List<string> arr = new List<string>();
    
    KeywordNode cur = selectedNode;
    while (cur != root) {
      arr.Add(cur.keyword);
      cur = cur.parent;
    }
    arr.Reverse();
    return arr;
  }

  private void UpdateKeywordButtons() {
    for (int i = 0; i < 8; ++i) {
      int targetIndex = contextKeywords.Count - 1 - i;

      if (targetIndex >= 0)
        keywordsText[i].text = contextKeywords[targetIndex];
    }
  }

  private void UpdateGeneratedKeywordsButtons() {
    int len = Math.Min(generatedText.Count, generatedKeywords.Count);
    for (int i = 0; i < len; ++i) {
      generatedText[i].text = generatedKeywords[i];
    }
  }
}
