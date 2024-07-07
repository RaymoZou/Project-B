using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelTimerUI : MonoBehaviour {
  private TextMeshProUGUI timer;


  private void Awake() {
    timer = GetComponent<TextMeshProUGUI>();
    timer.text = 0.ToString();
    GameManager.TimerUpdate += UpdateTimer;
  }

  private void UpdateTimer(float time) {
    timer.text = Math.Round(time, 3).ToString();
  }
}
