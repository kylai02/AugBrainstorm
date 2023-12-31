using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GeneratedwordBtn : MonoBehaviour {
  public Sprite smallSprite;
  public Sprite bigSprite;

  private Image _image;
  private TMP_Text _text;
  private RectTransform _rect;
  private bool _isSmallSprite;

  // Start is called before the first frame update
  void Start() {
    Button btn = GetComponent<Button>();
    btn.onClick.AddListener(AddNextNode);

    _image = GetComponent<Image>();
    _text = GetComponentInChildren<TMP_Text>();
    _rect = GetComponent<RectTransform>();
    _isSmallSprite = true;
  }

  void Update() {
    if (_text.text.Length > 5 && _isSmallSprite) {
      _rect.sizeDelta = new Vector2(150, 80);
      _image.sprite = bigSprite;
      _isSmallSprite = false;
    }
    else if (_text.text.Length <= 5 && !_isSmallSprite) {
      _rect.sizeDelta = new Vector2(80, 80);
      _image.sprite = smallSprite;
      _isSmallSprite = true;
    }
  }

  void AddNextNode() {
    UIManager.instance.NewKeywordNode(
      GetComponentInChildren<TMP_Text>().text,
      UIManager.instance.selectedNode
    );
  }
}
