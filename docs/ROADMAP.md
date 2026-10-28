# Smash Shooter — Roadmap

Shooter em terceira pessoa com knockback estilo Smash Bros (single-player vs bots).
Cair do mapa = morte. Quanto menos vida, maior o empurrão.

Atualizado a cada fase (um commit/branch por fase).

| Fase | Branch | Status | Entrega |
| --- | --- | --- | --- |
| 0 | `main` | ✅ Feito | Scripts atuais organizados em `Assets/Scripts/*` |
| 1 | `feat/core-mechanics` | 🟡 Pronto p/ testar no Unity | HealthSystem + Knockback + DeathZone |
| 2 | `feat/shooting-knockback` | ⬜ | Projétil com empurrão + FSM ranged expandida |
| 3 | `feat/enemy-melee` | ⬜ | Inimigo melee (FSM) |
| 4 | `feat/character-select` | ⬜ | 3 personagens, 2 habilidades cada |
| 5 | `feat/map-select` | ⬜ | Seleção de mapa |
| 6 | `feat/tutorial` | ⬜ | Tutorial em cena separada |
| 7 | `feat/hud-polish` | ⬜ | HUD, frags, GameManager |

## Já existente (pré-Fase 0)
Câmera third-person, inimigo ranged (FSM), mini mapa, Tab de frags
(mini mapa e frags vivem na cena/UI do Unity, fora desta pasta de scripts).

## Estrutura
```
Assets/Scripts/
  Player/   movimento, câmera
  Combat/   tiro (pool), projétil, vida, knockback
  Enemy/    IA dos inimigos
  Skills/   habilidades
  World/    DeathZone, mapas
```
