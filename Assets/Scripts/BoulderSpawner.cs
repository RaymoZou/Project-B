using UnityEngine;
using UnityEngine.UIElements;

// spawns boulders randomly
public class BoulderSpawner : MonoBehaviour {

  const float SPAWN_RATE = 2.0f;
  const float RANGE = 10f; // x spawn (from origin)
  const float HEIGHT = 20.0f; // spawn height
  [SerializeField] Boulder boulderPrefab;

  private void SpawnBoulder() {
    if (!boulderPrefab) Debug.LogError("assign boulder in inspector!");
    Vector2 spawnPosition = new(Random.Range(-RANGE, RANGE), HEIGHT);
    Instantiate(boulderPrefab, spawnPosition, Quaternion.identity);
  }

  private void Start() {
    InvokeRepeating("SpawnBoulder", 0, SPAWN_RATE);
  }
}
