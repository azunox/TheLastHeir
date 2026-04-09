using TheLastHeir.Runtime.Structs;

namespace TheLastHeir.Runtime.Interfaces
{
    public interface IDamageable
    {
        void OnTakeDamage(HitInfo hitInfo);
    }
}