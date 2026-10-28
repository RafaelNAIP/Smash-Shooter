using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Recebe empurrões. Força final = força base * HealthSystem.KnockbackMultiplier / resistência.
/// Funciona em player (Rigidbody comum) e em inimigos com NavMeshAgent: o agent é desligado
/// durante o voo e religado (Warp) ao pousar sobre o NavMesh.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class KnockbackReceiver : MonoBehaviour
{
    [Tooltip("Tempo sem controle após ser empurrado.")]
    public float stunDuration = 0.3f;
    [Tooltip("Inclinação para cima do empurrão (0 = horizontal).")]
    public float upwardBias = 0.25f;
    [Tooltip("Divide a força recebida. Escudos/personagens resistentes usam > 1.")]
    public float resistance = 1f;
    [Tooltip("Distância máxima para reencaixar no NavMesh ao pousar (só inimigos).")]
    public float navMeshSnapDistance = 1.5f;

    public bool IsStunned => Time.time < stunEndTime;
    /// <summary>True enquanto o NavMeshAgent está desligado (inimigo voando). A IA deve pausar.</summary>
    public bool IsAirborne => agentSuspended;

    private Rigidbody rb;
    private HealthSystem health;
    private NavMeshAgent agent;
    private float stunEndTime;
    private bool agentSuspended;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<HealthSystem>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            // Enquanto o agent controla o movimento, o Rigidbody não pode brigar com ele.
            rb.isKinematic = true;
            rb.constraints |= RigidbodyConstraints.FreezeRotation;
        }
    }

    /// <param name="direction">Direção do empurrão (será normalizada).</param>
    /// <param name="force">Velocidade base do empurrão (m/s), antes do multiplicador de vida.</param>
    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (direction.sqrMagnitude < 0.0001f || force <= 0f) return;

        float multiplier = health != null ? health.KnockbackMultiplier : 1f;
        Vector3 dir = (direction.normalized + Vector3.up * upwardBias).normalized;

        SuspendAgent();
        // VelocityChange: independe da massa do Rigidbody.
        rb.AddForce(dir * (force * multiplier / Mathf.Max(0.01f, resistance)), ForceMode.VelocityChange);
        stunEndTime = Time.time + stunDuration;
    }

    /// <summary>Cancela stun e velocidade (usado no respawn).</summary>
    public void ResetState()
    {
        stunEndTime = 0f;
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (agentSuspended) TryResumeAgent(force: true);
    }

    void Update()
    {
        if (agentSuspended && !IsStunned) TryResumeAgent(force: false);
    }

    void SuspendAgent()
    {
        if (agent == null || agentSuspended) return;
        agent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        agentSuspended = true;
    }

    void TryResumeAgent(bool force)
    {
        bool grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 1.1f);
        if (!force && !grounded) return;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, navMeshSnapDistance, NavMesh.AllAreas))
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            agent.enabled = true;
            agent.Warp(hit.position);
            agentSuspended = false;
        }
        // Sem NavMesh por perto: continua em queda livre até a DeathZone.
    }
}
