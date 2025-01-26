using Unity.VisualScripting;
using UnityEngine;
using ObjectPoolings;
[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    public Animator animator;
    public int minSpeed;
    public int maxSpeed; 
    public float baseDamage = 10f;
    private float _damage;
    public float lifeTime;
    public float lifeTimer;
    private bool isReleased = false;
    public DamageType damageType;
    PrefabPool pool;

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer < 0 &&!isReleased)
        {
            animator.SetBool("IsExplode", true);
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
        CheckBubblePop();
    }

    public void InitBullet(PrefabPool pool, float damageMult) {
        _damage = baseDamage * damageMult;
        lifeTimer = lifeTime;
        isReleased = false;
        this.pool = pool;
        GetComponent<Rigidbody>().linearVelocity = transform.forward * new System.Random().Next(minSpeed, maxSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out BaseEnemy enemy))
            {
                DamageInfo damageInfo = new DamageInfo(gameObject, other.gameObject, _damage, damageType);
                DamageManager.Instance.ManageDamage(damageInfo);
            }
            if (!isReleased)
            {
                animator.SetBool("IsExplode", true);
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
        else if (other.CompareTag("Wall")) 
        {
            if (!isReleased)
            {
                animator.SetBool("IsExplode", true);
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
    }

    public void CheckBubblePop() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BubbleExplosion")&&animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= animator.GetCurrentAnimatorStateInfo(0).length) {
            pool.Release(gameObject);
            isReleased = true;
        }
    }
}
