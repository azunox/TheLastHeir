namespace TheLastHeir.Runtime.Enums
{
    public enum PlayerStateEnum
    {
        Idle = 0,           // 대기
        Move,               // 이동 중 (걷기)
        Sprint,             // 달리기
        Jump,               // 점프
        Fall,               // 낙하
        Attack,             // 공격
        SkillCast,          // 스킬 시전
        Dodge,              // 회피
        Guard,              // 방어
        Hit,                // 피격
        Die,                // 사망
        Interact            // 상호작용
    }
}