using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S {get; private set; }

    [Header("Inscribed")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;
    public AudioSource audioSource;
    public AudioClip gotHitSFX;
    public AudioClip deathSFX;

    [Header("Dynamic")] [Range(0, 4)] [SerializeField]
    private  float _shieldLevel = 1;

    [Tooltip("This field holds a reference to the last triggering gameobject")]
    private GameObject lastTriggerGo = null;

    public delegate void WeaponFireDelegate();

    public event WeaponFireDelegate fireEvent;


    void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }
    
    void Awake() {
        if (S == null) {
            S = this;
        }
        else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        //fireEvent += TempFire;

        ClearWeapons();
        weapons[0].SetType(eWeaponType.blaster);
    }



    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis*pitchMult, hAxis*rollMult, 0);

    if (Input.GetAxis("Jump") == 1 && fireEvent != null) {
        fireEvent();
    }
    }


    void OnTriggerEnter(Collider other) {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        PowerUp pUp = go.GetComponent<PowerUp>();
        if (enemy != null) {
            shieldLevel--;
            PlayGotHitSFX();
            Destroy(go);
        }
        else if(pUp != null) {
                AbsorbPowerUp(pUp);
        }
        else {
            Debug.LogWarning("Shield trigger hit by non enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(PowerUp pUp) {
        Debug.Log("Absorbed Power Up" + pUp.type);
        switch (pUp.type) {
            case eWeaponType.shield:
                shieldLevel++;
                break;
            
            default:
                if (pUp.type == weapons[0].type) {
                    Weapon weap = GetEmptyWeaponSlot();
                    if (weap != null) {
                        weap.SetType(pUp.type);
                    }
                }
                else {
                    ClearWeapons();
                    weapons[0].SetType(pUp.type);
                }
                break;
                
        }
        pUp.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel {
        get { return (_shieldLevel); }
        private set {
            _shieldLevel = Mathf.Min(value, 4);
            // If the shield is going to be set to less than zero
            if (value < 0) {
                PlayHeroDeathSFX();
                Destroy(this.gameObject); // destroy hero
                Main.HERO_DIED();
            }
        }
    }

    Weapon GetEmptyWeaponSlot() {
        for (int i = 0; i < weapons.Length; i++){
            if (weapons[i].type == eWeaponType.none) {
                return (weapons[i]);
            }
        }
        return (null);
    }


    void ClearWeapons() {
        foreach (Weapon w in weapons) {
            w.SetType(eWeaponType.none);
        }
    }

    void PlayGotHitSFX() {
        if (gotHitSFX != null) {
            audioSource.PlayOneShot(gotHitSFX, 0.9f);
        }
    }


    void PlayHeroDeathSFX() {
        if (deathSFX != null) {
            GameObject deathSFXObj = new GameObject("DeathSFX");
            AudioSource tempAudioSource = deathSFXObj.AddComponent<AudioSource>();

            tempAudioSource.clip = deathSFX;
            tempAudioSource.volume = 0.4f;
            tempAudioSource.time = 0.1f; // start the audio inward slightly


            tempAudioSource.Play();


            Destroy(deathSFXObj, deathSFX.length);
        }
    }
}
