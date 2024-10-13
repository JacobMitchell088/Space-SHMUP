using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyShield))]
public class Enemy_4 : Enemy
{
    private EnemyShield[] allShields;
    private EnemyShield thisShield;

    void Start() {
        allShields = GetComponentsInChildren<EnemyShield>();
        thisShield = GetComponent<EnemyShield>();

    }

    public override void Move() {

    }



    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;


        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null) {
            Destroy(otherGO);

            if (bndCheck.isOnScreen) {
                GameObject hitGO = coll.contacts[0].otherCollider.gameObject;
                if (hitGO == otherGO) {
                    hitGO = coll.contacts[0].otherCollider.gameObject;
                }

                float dmg = Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;


                bool shieldFound = false;
                foreach (EnemyShield es in allShields) {
                    if (es.gameObject == hitGO) {
                        es.TakeDamage(dmg);
                        shieldFound = true;
                    }
                }

                if (!shieldFound) thisShield.TakeDamage(dmg);

                if (thisShield.isActive) return;

                if (!calledShipDestroyed) {
                    Main.SHIP_DESTROYED(this);
                    calledShipDestroyed = true;

                }

                Destroy(gameObject);
            }
            else {
                Debug.Log("Enemy 4 hit by non projectile hero" + otherGO.name);
            }
        }
    }








}
