using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class KeywordButton : MonoBehaviour {
  void Start() {
    Button btn = GetComponent<Button>();
    btn.onClick.AddListener(AddTextToSelectedWord);
  }
  
  // Pass selected keyword to UIManager
  void AddTextToSelectedWord() {
    string keyword = GetComponentInChildren<TMP_Text>().text;

    UIManager.instance.AddSelectedWord(keyword);
  }
}
