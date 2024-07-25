using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  const float RESPAWN_TIMER = 1.0f;
  private static GameManager instance;
  private Vector3 playerOneSpawn = Vector2.zero;
  private Vector3 playerTwoSpawn = Vector2.zero;
  private int PLAYER_MASK;

  public static float levelTimer;
  private bool isTimerRunning = false;

  public static Action<float> TimerUpdate;
  [SerializeField] private GameObject playerOnePrefab;
  [SerializeField] private GameObject playerTwoPrefab;

  public void Awake() {
    Health.OnDeath += StartRespawnCoroutine;
    SceneManager.sceneLoaded += OnSceneLoaded;
    RespawnCheckpoint.OnActivate += UpdateSpawn;
    StartLine.OnStart += StartTimer;
    Portal.Finish += Finish;

    PLAYER_MASK = LayerMask.NameToLayer("Player");

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
    Health.OnDeath -= StartRespawnCoroutine;
    SceneManager.sceneLoaded -= OnSceneLoaded;
    RespawnCheckpoint.OnActivate -= UpdateSpawn;
    StartLine.OnStart -= StartTimer;
    Portal.Finish += Finish;
  }

  private void StartRespawnCoroutine(GameObject player) => StartCoroutine(RespawnPlayer(player));

  private void StartTimer() { isTimerRunning = true; }

  private void Update() {
    if (isTimerRunning) {
      levelTimer += Time.deltaTime;
      TimerUpdate?.Invoke(levelTimer);
    }
  }


  public void UpdateSpawn(Vector2 spawnPos, int playerLayer) {
    if (playerLayer == PLAYER_MASK) playerOneSpawn = spawnPos;
    if (playerLayer == PLAYER_MASK) playerTwoSpawn = spawnPos;
  }

  // initialize spawns to zero on level load
  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    levelTimer = 0;
    isTimerRunning = false;
    UpdateSpawn(Vector2.zero, PLAYER_MASK);
    UpdateSpawn(Vector2.zero, PLAYER_MASK);
  }

  private void Finish() {
    Debug.Log("Level Finished with Time: " + levelTimer);
    isTimerRunning = false;

    // if in the Tutorial Level scene
    if (SceneManager.GetActiveScene().name == "Tutorial Level") {
      SceneManager.LoadScene("Title");
    }
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

  public static void LoadTutorial() {
    SceneManager.LoadScene("Tutorial Level");
  }

  // respawn the correct prefab of the player
  private IEnumerator RespawnPlayer(GameObject player) {
    int playerLayer = player.layer;
    player.SetActive(false);
    yield return new WaitForSeconds(RESPAWN_TIMER);
    Destroy(player); // destroy this after
    if (playerLayer == PLAYER_MASK) {
      GameObject newPlayer = Instantiate(instance.playerOnePrefab, instance.playerOneSpawn, Quaternion.identity);
      newPlayer.layer = PLAYER_MASK;
    } else if (playerLayer == PLAYER_MASK) {
      GameObject newPlayer = Instantiate(instance.playerTwoPrefab, instance.playerTwoSpawn, Quaternion.identity);
      newPlayer.layer = PLAYER_MASK;
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
