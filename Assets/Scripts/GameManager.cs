using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  private static GameManager instance;
  private Vector3 playerSpawn = Vector3.zero;

  private int playerOneLayerIndex;
  private int playerTwoLayerIndex;
  [SerializeField] private GameObject playerOnePrefab;
  [SerializeField] private GameObject playerTwoPrefab;

  public void Awake()
  {
    playerOneLayerIndex = playerOnePrefab.layer;
    playerTwoLayerIndex = playerTwoPrefab.layer;
    Health.OnDeath += RespawnPlayer;
    SceneManager.sceneLoaded += OnSceneLoaded;
    RespawnCheckpoint.OnActivate += UpdateSpawn;

    if (instance != null && instance != this)
    {
      Destroy(gameObject);
      return;
    }
    instance = this;
    DontDestroyOnLoad(gameObject);
  }

  private void OnDestroy()
  {
    Health.OnDeath -= RespawnPlayer;
    RespawnCheckpoint.OnActivate -= UpdateSpawn;
  }

  public void UpdateSpawn(Vector2 spawnPos)
  {
    playerSpawn = spawnPos;
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    UpdateSpawn(Vector2.zero);
  }

  public static void LoadNextLevel()
  {
    int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
    if (currSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
    {
      SceneManager.LoadScene(currSceneIndex + 1);
    } else
    {
      SceneManager.LoadScene(0);
    }
  }

  public static void RespawnPlayer(GameObject player)
  {
    int playerLayer = player.layer;
    Destroy(player);
    if (playerLayer == LayerMask.NameToLayer("Player 1"))
    {
      Instantiate(instance.playerOnePrefab, instance.playerSpawn, Quaternion.identity);
    } else if (player.layer == LayerMask.NameToLayer("Player 2"))
    {
      Instantiate(instance.playerTwoPrefab, instance.playerSpawn, Quaternion.identity);
    } else
    {
      Debug.LogError("Player Layer not known");
    }

  }
}
