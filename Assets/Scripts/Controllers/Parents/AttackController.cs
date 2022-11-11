using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField, Range(0f, 1f)] protected float _damage = 0.10f;
    [SerializeField] protected float _defAttackDelay = 0.3f;
    [SerializeField] protected Transform _attackPos;
    [SerializeField] protected float _attackRange = 5f;
    [SerializeField] protected LayerMask _enemyMask;

    protected float _attackDelay;

    protected abstract bool Fire();
    protected abstract void OnAwake();

    private void Awake()
    {
        OnAwake();
    }

    private void FixedUpdate()
    {
        if (_attackDelay <= 0)
        {
            if (Fire())
            {
                Debug.Log("Attacked!");

                Collider2D[] enemiesToDmg = Physics2D.OverlapCircleAll(_attackPos.position, _attackRange, _enemyMask);
                for (int i = 0; i < enemiesToDmg.Length; i++)
                {
                    Debug.Log("Enemy Hit");
                    // enemiesToDmg[i] - Lower enemy health & Stun & Knockback
                }
            }

            _attackDelay = _defAttackDelay;
        }
        else
        {
            _attackDelay -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPos.position, _attackRange);
    }
}
