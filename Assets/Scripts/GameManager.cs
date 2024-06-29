using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  private static GameManager instance;
  private Vector3 playerOneSpawn = Vector2.zero;
  private Vector3 playerTwoSpawn = Vector2.zero;
  private int playerOneID; // the gameObject layer
  private int playerTwoID; // the gameObject layer
  [SerializeField] private GameObject playerOnePrefab;
  [SerializeField] private GameObject playerTwoPrefab;

  public void Awake() {
    Health.OnDeath += RespawnPlayer;
    SceneManager.sceneLoaded += OnSceneLoaded;
    RespawnCheckpoint.OnActivate += UpdateSpawn;
    playerOneID = LayerMask.NameToLayer("Player 1");
    playerTwoID = LayerMask.NameToLayer("Player 2");

    // singleton pattern
    if (instance != null && instance != this) {
      Destroy(gameObject);
      return;
    } else {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  private void OnDestroy() {
    Health.OnDeath -= RespawnPlayer;
    RespawnCheckpoint.OnActivate -= UpdateSpawn;
  }

  public void UpdateSpawn(Vector2 spawnPos, int playerLayer) {
    if (playerLayer == playerOneID) playerOneSpawn = spawnPos;
    if (playerLayer == playerTwoID) playerTwoSpawn = spawnPos;
  }

  // initialize spawns to zero on level load
  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    UpdateSpawn(Vector2.zero, playerOneID);
    UpdateSpawn(Vector2.zero, playerTwoID);
  }

  // load the next level defined in build settings
  public static void LoadNextLevel() {
    int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
    if (currSceneIndex < SceneManager.sceneCountInBuildSettings - 1) {
      SceneManager.LoadScene(currSceneIndex + 1);
    } else {
      SceneManager.LoadScene(0);
    }
  }

  // respawn the correct prefab of the player
  public static void RespawnPlayer(GameObject player) {
    int playerLayer = player.layer;
    Destroy(player);
    if (playerLayer == LayerMask.NameToLayer("Player 1")) {
      Instantiate(instance.playerOnePrefab, instance.playerOneSpawn, Quaternion.identity);
    } else if (player.layer == LayerMask.NameToLayer("Player 2")) {
      Instantiate(instance.playerTwoPrefab, instance.playerTwoSpawn, Quaternion.identity);
    } else {
      Debug.LogError("Player Layer not known");
    }

  }

  public static void SwitchScreenResolution() {
    Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 144);
  }

  // TODO: this seems really unnecessary
  public static void QuitGame() {
    Application.Quit();
  }
}
