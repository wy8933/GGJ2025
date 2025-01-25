using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    public float speed;

    public void InitBullet() {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }
}
