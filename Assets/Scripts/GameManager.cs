using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  private static GameManager instance;
  private Vector3 playerSpawn = Vector3.zero;

  [SerializeField] private GameObject playerPrefab;

  public void Awake() {
    Health.OnDeath += RespawnPlayer;
    SceneManager.sceneLoaded += OnSceneLoaded;
    RespawnCheckpoint.OnActivate += UpdateSpawn;

    if (instance != null && instance != this) {
      Destroy(gameObject);
      return;
    }
    instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void OnDestroy() {
    Health.OnDeath -= RespawnPlayer;
    RespawnCheckpoint.OnActivate -= UpdateSpawn;
  }

  public void UpdateSpawn(Vector2 spawnPos) {
    playerSpawn = spawnPos;
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    UpdateSpawn(Vector2.zero);
  }

  public static void LoadNextLevel() {
    int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
    if (currSceneIndex < SceneManager.sceneCountInBuildSettings - 1) {
      SceneManager.LoadScene(currSceneIndex + 1);
    } else {
      SceneManager.LoadScene(0);
    }
  }

  public static void RespawnPlayer(GameObject player) {
    Destroy(player);
    Instantiate(instance.playerPrefab, instance.playerSpawn, Quaternion.identity);
  }
}
