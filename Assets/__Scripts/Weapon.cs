using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class WeaponDefinition {
    public eWeaponType type = eWeaponType.none;
    [Tooltip("Letter to show on the PowerUp Cube")]
    public string letter;
    [Tooltip("Color of the powerup")]
    public Color powerUpColor = Color.white;
    [Tooltip("Prefab of wep model that is attached to player ship")]
    public GameObject weaponModelPrefab;
    [Tooltip("Prefab of projectile that is fired")]
    public GameObject projectilePrefab;
    [Tooltip("Color of the projectile that is fired")]
    public Color projectileColor = Color.white;
    [Tooltip("Damage caused when a single projectile hits")]
    public float damageOnHit = 0;
    [Tooltip("Damage caused per second by laser")]
    public float damagePerSec = 0;
    [Tooltip("Seconds to delay between shots")]
    public float delayBetweenShots = 0;
    [Tooltip("Velocity of individual projectiles")]
    public float velocity = 50;


}




public enum eWeaponType {
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield
}


public class Weapon : MonoBehaviour
{

}
