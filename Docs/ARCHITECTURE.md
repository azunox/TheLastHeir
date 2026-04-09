# The Last Heir - Architecture Document

## 1. 프로젝트 구조
- Not Yet

## 2. 핵심 아키텍처
### 2.1 단일 책임 원칙 (SRP)
- 각 Handler는 단일 책임에 집중
  - PlayerMovementHandler: 이동, 회전, 점프, 중력, 구르기 처리
  - PlayerAnimationHandler: 애니메이션 관련 처리 (Blend, Attack, Root Motion)
  - PlayerCombatHandler: 공격, 데미지 적용, 타겟 처리
  - PlayerInputHandler: 키보드/마우스 입력과 이벤트 발행


### 2.2 이벤트 기반 구조
- 입력과 행동을 분리
  - PlayerInputHandler → 이벤트 발생 (OnJump, OnAttack, OnRoll, OnHeavyAttack)
  - 각 Handler에서 이벤트 구독 후 로직 실행

### 2.3 Damage 계산 구조
- Damage 구조체: 타입별 데미지 관리
- DamageNegation: 경감률 계산
- 모든 계산은 **독립적인 구조체와 ScriptableObject**로 분리


### 2.4 클래스 간 의존성
- Player → 모든 Handler 소유
- Handler들은 owner 참조로 Player 속성 접근
- Handler끼리 직접 호출하지 않고 이벤트를 통해 연결

### 3. Combat System
- Not Yet

## 4. Movement & Animation Flow
1. InputHandler에서 입력 수집 (move, sprint, jump, roll)
2. MovementHandler:
   - 이동 계산 (카메라 기준 방향)
   - 회전 계산 (SmoothDampAngle)
   - 중력 적용, 구르기 처리
3. AnimationHandler:
   - BlendTree 업데이트
   - 공격, 점프, 구르기 애니메이션 재생
4. SRP 적용 → 입력/이동/애니메이션 독립


## 5. 설계 원칙
- **단일 책임 원칙(SRP)**: 각 클래스/핸들러는 하나의 책임만
- **이벤트 중심 아키텍처**: 입력과 행동 로직 분리
- **재사용 가능 구조**: Damage, entityOwnedHandler 등
- **확장성**: 새로운 공격, 스킬, 적 타입 추가 시 최소 변경
- **테스트 용이성**: Handler 단위로 테스트 가능

## 6. 향후 계획
1. Combat system 설계 완료 → DamageCollider, 공격 이벤트, 계산 로직 구조 정의 등
2. Enemy Entity 설계 → Health, AI, 대미지 수신 로직
3. 문제 지속 갱신 → 코드 변경시 ARCHITECTURE.md 업데이트