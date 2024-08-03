using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

  public static void LoadTutorial() {
    SceneManager.LoadScene("Tutorial");
  }

  public static void LoadTimeAttack() {
    SceneManager.LoadScene("Time Attack");
  }

  public static void SwitchScreenResolution() {
    // change the screen resolution
    // TODO:
  }
  public static void QuitGame() {
    Application.Quit();
  }
}
