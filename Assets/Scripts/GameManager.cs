using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  public static void LoadNextLevel() {
    int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
    if (currSceneIndex < SceneManager.sceneCountInBuildSettings - 1) {
      SceneManager.LoadScene(currSceneIndex + 1);
    } else {
      Debug.LogError("No more scenes to load.");
    }
  }
}
