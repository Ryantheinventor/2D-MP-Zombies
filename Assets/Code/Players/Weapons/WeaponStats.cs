using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObjects/WeaponStats", order = 1)]
public class WeaponStats : ScriptableObject
{
    public float fireRPM = 60;
    public int magSize = 20;
    public float reloadTime = 2;
    public int damagePerBullet = 1;
    public int bulletsPerShot = 1;
    public float shotAngle = 0;
}   
