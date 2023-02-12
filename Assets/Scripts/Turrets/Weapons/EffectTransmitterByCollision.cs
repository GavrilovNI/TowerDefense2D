using Game.Enemies;
using Game.EnemyEffects.Effects.Abstract;

namespace Game.Turrets.Weapons
{
    public class EffectTransmitterByCollision : WeaponWithEffects<EnemyEffect>
    {
        private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
        {
            var enemy = collision.transform.GetComponent<Enemy>();
            if(enemy != null)
                ApplyEffects(enemy);
        }
    }
}
