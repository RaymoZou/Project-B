using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  private static GameManager instance;
  private Vector3 playerSpawn = new Vector3(0, 1, 0);

  [SerializeField] private GameObject playerPrefab;


  public void Awake() {
    instance = this;
    DontDestroyOnLoad(gameObject);
    Health.OnDeath += RespawnPlayer;
  }

  private void OnDestroy() {
    Health.OnDeath -= RespawnPlayer;
  }


  public static void LoadNextLevel() {
    int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
    if (currSceneIndex < SceneManager.sceneCountInBuildSettings - 1) {
      SceneManager.LoadScene(currSceneIndex + 1);
    } else {
      Debug.LogError("No more scenes to load.");
    }
  }

  public static void RespawnPlayer(GameObject player) {
    Destroy(player);
    Instantiate(instance.playerPrefab, instance.playerSpawn, Quaternion.identity);
  }
}
