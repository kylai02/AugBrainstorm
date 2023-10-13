using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class UIManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]
  public List<TMP_Text> keywordsText;
  public GameObject keywordNodeObj;
  public GameObject tree;

  public List<string> selectedKeywords;
  public List<string> contextKeywords;

  public KeywordNode root;

  public static UIManager instance;
  

  void Awake() {
    if (!instance) instance = this;

    contextKeywords = new List<string>();
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
    if (Input.GetKeyDown(KeyCode.A)) {
      KeywordNode cur = root;
      while (cur.children.Count > 0) cur = cur.children[0];

      NewKeywordNode("aaaaa", cur);
    }

    UpdateKeywordButtons();
  }

  public void AddSelectedWord(string s) {
    selectedKeywords.Add(s);
  }

  public void NewKeywordNode(string keyword, KeywordNode parent) {
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
      newNodeObj.transform.localPosition = new Vector3(-600, -250, 0);
    }
  }

  private void UpdateKeywordButtons() {
    for (int i = 0; i < 8; ++i) {
      int targetIndex = contextKeywords.Count - 1 - i;

      if (targetIndex >= 0)
        keywordsText[i].text = contextKeywords[targetIndex];
    }
  }

}