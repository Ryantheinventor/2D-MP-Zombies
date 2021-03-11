using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponStats weaponStats;
    private float curWaitTime;

    public GameObject bulletFab;

    // Update is called once per frame
    void Update()
    {
        if (weaponStats != null) 
        {
            curWaitTime += Time.deltaTime;
            float fireWaitTime = 1 / (weaponStats.fireRPM / 60);
            if (Input.GetMouseButton(0) && fireWaitTime <= curWaitTime) 
            {
                for (int i = 0; i < weaponStats.bulletsPerShot; i++) 
                {
                    GameObject newBullet = Instantiate(bulletFab, transform.position, transform.rotation);
                    newBullet.transform.eulerAngles += new Vector3(0, 0, Random.Range(weaponStats.shotAngle / 2 * -1, weaponStats.shotAngle / 2));
                    curWaitTime = 0;
                    newBullet.GetComponent<BulletController>().damage = weaponStats.damagePerBullet;
                }
                

            }
        }
        

    }
}
