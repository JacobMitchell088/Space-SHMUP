using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float powerUpDropChance = 1f;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSFX;

    protected bool calledShipDestroyed = false;

    protected BoundsCheck bndCheck;

    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
    }

    void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }



    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)) {
            Destroy(gameObject);
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll) {
        GameObject otherGO = coll.gameObject;
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null) {
            if (bndCheck.isOnScreen) {
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
                PlayHitSFX();
                if (health <= 0) {
                    if (!calledShipDestroyed) {
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    PlayDeathSFX();
                    Destroy(this.gameObject);
                }
            }
            Destroy(otherGO);
        }
        else {
            print ("Enemy hit by non projectile hero: " + otherGO.name);
        }
    }

    protected void PlayHitSFX() {
        if (hitSound != null) {
            audioSource.PlayOneShot(hitSound, 0.5f);
        }
    }
    
    protected void PlayDeathSFX() {
        if (deathSFX != null) {
            GameObject deathSFXObj = new GameObject("DeathSFX");
            AudioSource tempAudioSource = deathSFXObj.AddComponent<AudioSource>();

            tempAudioSource.clip = deathSFX;
            tempAudioSource.volume = 0.75f;
            tempAudioSource.time = 0.1f; // start the audio inward slightly

            tempAudioSource.Play();


            Destroy(deathSFXObj, deathSFX.length);
        }
    }

    protected void HandleDeath() { // Made to ensure we can play death VFXs / Audio before deleting this actor
    
    }
}
