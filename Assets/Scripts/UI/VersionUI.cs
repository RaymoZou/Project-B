using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Script for updating the UI text for the Unity version
[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionUI : MonoBehaviour

{
  // TODO: formatting
  private TextMeshProUGUI versionText;

  void Awake()
  {
    versionText = GetComponent<TextMeshProUGUI>();
  }

  void Start()
  {
    versionText.text = "ALPHA " + Application.version;
  }

}
