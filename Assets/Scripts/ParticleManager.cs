using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleManager : MonoBehaviour {
  ParticleSystem particleSystem;

  private void Awake() {
    Health.OnDeath += SpawnParticles;
    particleSystem = GetComponent<ParticleSystem>();
  }

  private void OnDestroy() {
    Health.OnDeath -= SpawnParticles;
  }

  // Go to player transform and play the particle effect
  private void SpawnParticles(GameObject player) {
    Debug.Log("the player died at: " + player.transform.position);
    gameObject.transform.position = player.transform.position;
    particleSystem.Play();
  }
}
