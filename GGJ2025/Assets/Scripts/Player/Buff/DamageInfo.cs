
using UnityEngine;

[System.Serializable]
public class DamageInfo
{
    public GameObject creator;
    public GameObject target;
    public float damage;

    public DamageInfo(GameObject creator, GameObject target, float damage) { 
        this.creator = creator;
        this.target = target;
        this.damage = damage;
    }
}
