using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// public class Node {
//   public string keyword;
//   public Node parent;
//   public List<Node> children;
//   public KeywordNode nodeObj;

//   public Node() {
//     children = new List<Node>();
//   }
// }

public class KeywordNode : MonoBehaviour {
  public string keyword;
  public KeywordNode parent;
  public List<KeywordNode> children;

  void Awake() {
    children = new List<KeywordNode>();
  }
}
