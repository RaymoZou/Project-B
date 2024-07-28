using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(RectTransform))]
public class LevelTimerUI : MonoBehaviour {
  private TextMeshProUGUI timer;
  private RectTransform rect;
  private RectTransform parent;
  const int FONT_SIZE = 144;

  private void Awake() {
    timer = GetComponent<TextMeshProUGUI>();
    rect = GetComponent<RectTransform>();
    parent = gameObject.GetComponentInParent<RectTransform>();
    if (!parent) Debug.LogError("no parent found");
    timer.text = 0.ToString();
    GameManager.TimerUpdate += UpdateTimer;
    Portal.Finish += EnlargeTimer;
  }

  private void OnDestroy() {
    Portal.Finish -= EnlargeTimer;
  }

  private void EnlargeTimer() {
    parent.anchorMin = new Vector2(0.5f, 0.5f);
    parent.anchorMax = new Vector2(0.5f, 0.5f);
    parent.pivot = new Vector2(0.5f, 0.5f);
    rect.anchoredPosition = Vector2.zero;
    timer.alignment = TextAlignmentOptions.Center;
    timer.fontSize = FONT_SIZE;
  }

  private void UpdateTimer(float time) {
    timer.text = Math.Round(time, 3).ToString();
  }
}
