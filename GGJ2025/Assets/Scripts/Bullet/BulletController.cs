using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    public int minSpeed;
    public int maxSpeed;

    public void InitBullet() {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * new System.Random().Next(minSpeed, maxSpeed);
    }
}
