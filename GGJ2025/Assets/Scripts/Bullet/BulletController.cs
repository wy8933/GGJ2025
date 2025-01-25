using Unity.VisualScripting;
using UnityEngine;
using ObjectPoolings;
[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    public int minSpeed;
    public int maxSpeed; 
    public float damage = 10f;
    public DamageType damageType;
    PrefabPool pool; 
    public void InitBullet(PrefabPool pool) {
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

            pool.Release(gameObject);
        }
        else if (other.CompareTag("Wall")) 
        {
            pool.Release(gameObject);
        }
    }
}
