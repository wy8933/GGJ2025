using Unity.VisualScripting;
using UnityEngine;
using ObjectPoolings;
[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    public int minSpeed;
    public int maxSpeed; 
    public float damage = 10f;
    public float lifeTime;
    public float lifeTimer;
    private bool isReleased = false;
    public DamageType damageType;
    PrefabPool pool;

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer < 0 &&!isReleased) {
            pool.Release(gameObject);
            isReleased = true;
        }
    }


    public void InitBullet(PrefabPool pool) {
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
                DamageInfo damageInfo = new DamageInfo(gameObject, other.gameObject, damage, damageType);
                DamageManager.Instance.ManageDamage(damageInfo);
            }

            if (!isReleased) {
                pool.Release(gameObject);
                isReleased = true;
            }
        }
        else if (other.CompareTag("Wall")) 
        {
            if (!isReleased)
            {
                pool.Release(gameObject);
                isReleased = true;
            }
        }
    }
}
