using UnityEngine;

public enum WeaponType{
    MachineGun,
    Shotgun
}

[System.Serializable]
public class PlayerStats
{
    public WeaponType weaponType;
}
