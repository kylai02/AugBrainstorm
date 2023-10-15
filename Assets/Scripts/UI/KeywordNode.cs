using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class KeywordNode : MonoBehaviour {
  public string keyword;
  public KeywordNode parent;
  public List<KeywordNode> children;

  void Awake() {
    children = new List<KeywordNode>();
  }

  void Start() {
    Button btn = GetComponent<Button>() ?? null;
    if (btn) {
      btn.onClick.AddListener(AddNodeToSelectedNode);
    }
  }

  void AddNodeToSelectedNode() {
    UIManager.instance.ChoseSelectedNode(this);
  }
}
