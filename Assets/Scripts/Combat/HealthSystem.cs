using System;
using UnityEngine;

/// <summary>
/// Vida estilo Smash: vai de 100% a 0%. Nunca morre por dano — só por queda do mapa (DeathZone).
/// Quanto MENOS vida, MAIOR o multiplicador de knockback (ver KnockbackMultiplier).
/// Usado por player e inimigos. Substitui o EnemyHealth (mantido só por compatibilidade).
/// </summary>
public class HealthSystem : MonoBehaviour
{
    /// <summary>(vítima, matador). Matador é null se caiu sozinho. Use para o sistema de frags.</summary>
    public static event Action<HealthSystem, GameObject> OnAnyDeath;

    [Header("Vida")]
    public float maxHealth = 100f;

    [Header("Knockback")]
    [Tooltip("Bônus de knockback com 0% de vida. 1 = o dobro do empurrão base.")]
    public float extraKnockbackAtZeroHealth = 1f;

    [Header("Morte / Respawn")]
    [Tooltip("Player: true (renasce). Inimigos: false (morre ao cair).")]
    public bool respawnOnDeath = false;
    public Transform respawnPoint;
    [Tooltip("Tempo (s) em que o último atacante ainda conta como o matador de uma queda.")]
    public float killCreditWindow = 5f;

    public bool Invulnerable { get; set; }
    public float CurrentHealth { get; private set; }
    public float HealthPercent => maxHealth <= 0f ? 0f : CurrentHealth / maxHealth * 100f;
    public GameObject LastAttacker { get; private set; }

    /// <summary>Multiplicador aplicado ao empurrão: 1.0 com 100% de vida até (1 + extra) com 0%.</summary>
    public float KnockbackMultiplier => 1f + (1f - HealthPercent / 100f) * extraKnockbackAtZeroHealth;

    /// <summary>Disparado quando a vida muda (percentual 0–100). Para a UI.</summary>
    public event Action<float> OnPercentChanged;
    public event Action<HealthSystem, GameObject> OnDied;

    private float lastAttackTime = float.NegativeInfinity;
    private float lastDeathTime = float.NegativeInfinity;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private KnockbackReceiver knockback;
    private Rigidbody rb;

    void Awake()
    {
        knockback = GetComponent<KnockbackReceiver>();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        CurrentHealth = maxHealth;
    }

    void Start()
    {
        OnPercentChanged?.Invoke(HealthPercent);
    }

    /// <param name="attacker">Quem causou o dano (para créditos de frag). Pode ser null.</param>
    public void TakeDamage(float amount, GameObject attacker = null)
    {
        if (Invulnerable || amount <= 0f) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);
        if (attacker != null && attacker != gameObject)
        {
            LastAttacker = attacker;
            lastAttackTime = Time.time;
        }
        OnPercentChanged?.Invoke(HealthPercent);
    }

    /// <summary>Chamado pela DeathZone quando sai do mapa.</summary>
    public void Die()
    {
        if (Time.time - lastDeathTime < 0.2f) return; // evita morte dupla (vários colliders)
        lastDeathTime = Time.time;

        GameObject killer = (LastAttacker != null && Time.time - lastAttackTime <= killCreditWindow)
            ? LastAttacker : null;

        OnDied?.Invoke(this, killer);
        OnAnyDeath?.Invoke(this, killer);

        if (respawnOnDeath) Respawn();
        else gameObject.SetActive(false);
    }

    public void Respawn()
    {
        Vector3 pos = respawnPoint != null ? respawnPoint.position : startPosition;
        Quaternion rot = respawnPoint != null ? respawnPoint.rotation : startRotation;

        if (knockback != null) knockback.ResetState();
        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        transform.SetPositionAndRotation(pos, rot);
        if (rb != null) rb.position = pos;

        CurrentHealth = maxHealth;
        LastAttacker = null;
        OnPercentChanged?.Invoke(HealthPercent);
    }
}
