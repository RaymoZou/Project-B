using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleManager : MonoBehaviour {
  ParticleSystem myParticleSystem;

  private void Awake() {
    Health.OnDeath += SpawnParticles;
    myParticleSystem = GetComponent<ParticleSystem>();
  }

  private void OnDestroy() {
    Health.OnDeath -= SpawnParticles;
  }

  private void SpawnParticles(GameObject player) {
    gameObject.transform.position = player.transform.position;
    myParticleSystem.Play();
  }
}
