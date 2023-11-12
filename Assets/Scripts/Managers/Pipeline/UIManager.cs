using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;



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

  public string initKeyword;

  public List<string> selectedKeywords;
  public List<string> positiveConditions;
  public List<string> nagativeConditions;

  public List<string> contextKeywords;
  public List<string> generatedKeywords;
  public List<string> generatedIdeas;

  public KeywordNode root;

  public static UIManager instance;
  
  public KeywordNode selectedNode;

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

  private async void UpdateGeneratedKeywords() {
    ideasField.SetActive(false);
    generatedKeywords = await OpenAI.OpenAI.instance.GetGeneratedKeywordsOpenAI(
      NodePath(),
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
    for (int i = 0; i < 8; ++i) {
      generatedText[i].text = generatedKeywords[i];
    }
  }

}
