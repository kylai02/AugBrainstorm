using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ConditionBtn : MonoBehaviour {
  public Sprite smallSprite;
  public Sprite bigSprite;

  private Image _image;
  private TMP_Text _text;
  private RectTransform _rect;
  private bool _isSmallSprite;

  void Start() {
    Button btn = GetComponent<Button>();
    btn.onClick.AddListener(DeleteThisCondition);

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

  void DeleteThisCondition() {
    UIManager.instance.DeleteCondition(gameObject);
  }
}
