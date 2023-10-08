using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]
  public List<TMP_Text> keywordsText;

  public List<string> selectedKeywords;
  public List<string> contextKeywords;

  public static UIManager instance;
  

  void Awake() {
    if (!instance) instance = this;

    contextKeywords = new List<string>();
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

    UpdateKeywordButtons();
  }

  public void AddSelectedWord(string s) {
    selectedKeywords.Add(s);
  }

  private void UpdateKeywordButtons() {
    for (int i = 0; i < 8; ++i) {
      int targetIndex = contextKeywords.Count - 1 - i;

      if (targetIndex >= 0)
        keywordsText[i].text = contextKeywords[targetIndex];
    }
  }

}
