# Fase 1 — Como configurar no Unity

## Decisão de design (revise!)
"Vida" vai de **100% → 0%**. Quanto **menos** vida, **maior** o empurrão:
`multiplicador = 1 + (1 - vida/100) * extraKnockbackAtZeroHealth` (padrão: 1.0x a 100% → 2.0x a 0%).
Vida 0% **não mata** — só a queda do mapa mata.
Se preferir o estilo Smash (contador de dano subindo 0% → 999%), é só inverter a UI/fórmula em `HealthSystem`.

## Player
1. Adicione `HealthSystem` (Respawn On Death = ✅, arraste um `Respawn Point` opcional) e `KnockbackReceiver`.
2. O `Rigidbody` do player precisa estar **não-kinematic**. Recomendado: Freeze Rotation X/Z.
3. `PlayerMove` já respeita o stun automaticamente.

## Inimigos (prefab)
1. **Remova** `EnemyHealth` (legado) e adicione `HealthSystem` (Respawn On Death = ❌) + `KnockbackReceiver`.
2. O `KnockbackReceiver` cria/usa um `Rigidbody` kinematic enquanto o `NavMeshAgent` anda,
   e troca para físico durante o empurrão. Mantenha o `NavMeshAgent`.

## Mapa
1. Crie um GameObject vazio `DeathZone` com `BoxCollider` **Is Trigger** gigante cobrindo
   abaixo e ao redor da arena, e adicione o script `DeathZone`.
2. Player e inimigos precisam de `Rigidbody` (já têm) para disparar o trigger.

## Como testar
1. Atire num inimigo: vida cai 25% por tiro, empurrão cresce a cada tiro.
2. Empurre-o para fora da plataforma → ele desativa ao entrar na DeathZone.
3. Caia do mapa com o player → renasce com 100%.
4. Frags: `HealthSystem.OnAnyDeath(vítima, matador)` é o evento para ligar no seu sistema de frags atual.
