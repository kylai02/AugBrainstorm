using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node {
  public string keyword;
  public Node parent;
  public List<Node> children;
  public KeywordNode nodeObj;

  public Node() {
    children = new List<Node>();
  }
}


public class KeywordNode : MonoBehaviour {
  public Node node;

  void Awake() {
    node = new Node();
    node.nodeObj = this;
  }
}
