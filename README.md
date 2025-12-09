# Cosmic Escape

**3D Boss Rush Shooting Runner Game**
> Game Programming Final Team Project (Team 4)

## Project Overview
**Cosmic Escape**는 우주 행성을 탈출하기 위해 각기 다른 패턴을 가진 3종류의 보스를 공략하는 **3D 보스 러시 슈팅 게임**입니다.

단순히 달리는 러너 장르에 탄막 슈팅과 보스 레이드 요소를 결합하였으며, 플레이어는 보스의 패턴을 학습하고 회피하며 공략하는 성취감을 느낄 수 있습니다.

## Game Features
### **Stage 1 (Humanoid - Golem)**
* Throw Rock
* Roar
* Jump Attack
* Magic Attack
### **Stage 2 (Monster - Crab)**
* Spike Attack
* Jump Attack
### **Stage 3 (Spaceship)**
* Basic Fire
* Fan Shape Fire
* FireWall
* Spiral Shape Fire
* Ring Shape Fire
* Side Fire

## Controls

| Action | Key | Description |
| :--- | :--- | :--- |
| **Move** | `←` `→` | 좌우 이동 |
| **Jump** | `SPACE` | 점프 |
| **Dash** | `Left Shift`(only A,D) | 빠른 이동(회피기) |
| **Attack** | `V` | 자동 공격 ON/OFF Toggle |
| **Target** | `1` ~ `4` | 타겟 변경 (Stage 2 전용) |

---

### System Architecture
* **Singleton GameManager:** `InitScene`에서 초기화되어 `DontDestroyOnLoad`를 통해 씬 간 데이터를 유지하고 게임 루프를 관리합니다.
* **Abstract Boss Class:** 모든 보스는 `Boss(BossMain.cs)` 추상 클래스를 상속받아 공통 기능(HP 관리, 피격 `TakeDamage`)과 개별 패턴(`AttackRoutine`)을 분리하여 구현했습니다.
