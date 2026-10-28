using UnityEngine;

/// <summary>
/// Trigger posicionado ao redor/abaixo do mapa. Quem entra morre (HealthSystem.Die):
/// player renasce, inimigo é desativado. Use um BoxCollider grande com Is Trigger.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DeathZone : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        HealthSystem health = other.GetComponentInParent<HealthSystem>();
        if (health != null) health.Die();
    }
}
