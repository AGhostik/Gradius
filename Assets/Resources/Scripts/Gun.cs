using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAxis
{
    A, B, X, Y
}

//not Projectile !!!
[System.Serializable]
public class Projectiles
{
    public GameObject Projectile;
    public AudioClip shotSound;
    [Range(0f, 5f)]
    public float volume = 1;

    [Header("DEBUG")]
    public int projectileCount = 0;
    public int currentProjectileNumber = 0;
    public GameObject[] projectilesCached;
    public Projectile[] projectilesCachedComponent;
    public GameObject[] projectilesCached_companion;
    public Projectile[] projectilesCachedComponent_companion;
}

public class Gun : MonoBehaviour {

    [Header("Ammunition")]
    [HideInInspector]
    public int currentProjectileLevel = 0;
    public List<Projectiles> ProjectileLevels;

    [Header("Start position")]
    public GameObject muzzle;
    public GameObject Companion;

    [Header("Stats")]
    public int damageUP = 0;
    [Range(0.1f, 10f)]
    public float firerate = 1;
    [Range(0.1f, 10f)]
    public float firerateCap = 0.1f;    
    public float PauseLength = 0;
    [Range(0f, 5f)]
    public float RandomStartPause = 0;
    public int shotsBetweenPause = 3;

    [Header("Rotate projectile")]
    public bool rotate = false;
    public bool clockwise = false;
    [Range(0f, 359f)]
    public float minAngle = 0;
    [Range(0f, 359f)]
    public float maxAngle = 360;
    public float rotateSpeed = 30;

    [Header("Control")]
    public bool autoShot = true;
    public InputAxis input;

    private int shotCount;
    private float angle;           
    private float firerateTimer;
    private float pauseTimer;
    
    private Transform gunTransform;
    private Transform muzzleTransform;

    private AudioSource gunAudio;

    private void Awake()
    {
        gunTransform = transform;
        gunAudio = GetComponent<AudioSource>();
        CacheProjectiles();
    }

    void Start () {
        muzzleTransform = muzzle == null ? null : muzzle.transform;
        angle = minAngle;

        shotCount = shotsBetweenPause;
        firerateTimer = firerate;

        if (RandomStartPause > 0)
        {
            pauseTimer = Random.Range(0, RandomStartPause);
        }
        else
        {
            pauseTimer = 0;
        }
	}
    
    void Update () {
        UpdateAngle();
        ShotTimers();
        if (autoShot)
        {
            Shot();
        }
    }

    private void OnEnable()
    {
        if (!autoShot)
        {
            switch (input)
            {
                case InputAxis.A:
                    EventController.InputA += Shot;
                    break;
                case InputAxis.B:
                    EventController.InputB += Shot;
                    break;
                case InputAxis.X:
                    EventController.InputX += Shot;
                    break;
                case InputAxis.Y:
                    EventController.InputY += Shot;
                    break;
            }
        }
    }

    private void OnDisable()
    {
        switch (input)
        {
            case InputAxis.A:
                EventController.InputA -= Shot;
                break;
            case InputAxis.B:
                EventController.InputB -= Shot;
                break;
            case InputAxis.X:
                EventController.InputX -= Shot;
                break;
            case InputAxis.Y:
                EventController.InputY -= Shot;
                break;
        }
    }
    
    private void UpdateAngle()
    {
        if (rotate)
        {     
            float delta = rotateSpeed * Time.deltaTime;
            if (clockwise)
            {
                angle += delta;
                if (angle < minAngle || angle > maxAngle)
                {
                    angle = minAngle;
                }
            }
            else
            {
                angle -= delta;
                if (angle < minAngle || angle > maxAngle)
                {
                    angle = maxAngle;
                }
            }
        }

        if (angle > maxAngle)
        {
            angle -= maxAngle;
        }
    }

    private void CreateProjectile()
    {
        int i = ProjectileLevels[currentProjectileLevel].currentProjectileNumber;
        Projectiles pj = ProjectileLevels[currentProjectileLevel];

        if (pj.projectilesCached[i].activeInHierarchy == false)
        {            
            pj.projectilesCachedComponent[i].Damage += damageUP;

            if (rotate)
            {
                pj.projectilesCachedComponent[i].Angle = angle;
            }

            if (muzzle != null)
            {
                pj.projectilesCached[i].transform.position = muzzleTransform.position;
            }
            else
            {
                pj.projectilesCached[i].transform.position = gunTransform.position;
            }

            if (Companion != null && Companion.activeInHierarchy == true)
            {
                pj.projectilesCached_companion[i].transform.position = Companion.transform.position;
                pj.projectilesCachedComponent_companion[i].Damage += damageUP;
                pj.projectilesCached_companion[i].SetActive(true);                
            }
            pj.projectilesCached[i].SetActive(true);
            playSound();
        }

        if (i < ProjectileLevels[currentProjectileLevel].projectileCount - 1)
        {
            ProjectileLevels[currentProjectileLevel].currentProjectileNumber++;
        }
        else
        {
            ProjectileLevels[currentProjectileLevel].currentProjectileNumber = 0;
        }
    }

    private void Shot()
    {
        if (ProjectileLevels.Count > 0 && ProjectileLevels[currentProjectileLevel] != null)
        {
            if (shotCount < shotsBetweenPause)
            {
                if (firerateTimer >= firerate)
                {
                    CreateProjectile();

                    shotCount++;
                    firerateTimer = 0;
                    pauseTimer = 0;
                }
            }
            else
            {
                if (pauseTimer >= PauseLength)
                {
                    shotCount = 0;
                }
            }
        }
    }

    private void ShotTimers()
    {
        if (firerateTimer < firerate)
        {
            firerateTimer += Time.deltaTime;
        }

        if (shotCount >= shotsBetweenPause)
        {
            if (pauseTimer < PauseLength)
            {
                pauseTimer += Time.deltaTime;
            }
        }
    }

    private void playSound()
    {
        if (ProjectileLevels[currentProjectileLevel].shotSound != null)
        {
            if (ProjectileLevels[currentProjectileLevel].shotSound != null && ProjectileLevels[currentProjectileLevel].volume > 0)
            {
                gunAudio.PlayOneShot(ProjectileLevels[currentProjectileLevel].shotSound, ProjectileLevels[currentProjectileLevel].volume);
            }
        }
    }

    private void CacheProjectiles()
    {
        SceneObjectContainer.AddGunContainer(gameObject.name);
        int projectileArraySize = ProjectileLevels.Count;
        for (int i = 0; i < projectileArraySize; i++)
        {
            int projectileCount;
            Projectile tempPj = ProjectileLevels[i].Projectile.GetComponent<Projectile>();
            GameObject pjCollector = new GameObject();
            GameObject pjCollector_companion = new GameObject();
            GameObject efCollector = new GameObject();
            GameObject efCollector_companion = new GameObject();

            pjCollector.name = ProjectileLevels[i].Projectile.name + "_collection";
            pjCollector_companion.name = ProjectileLevels[i].Projectile.name + "_collection_companion";
            efCollector.name = ProjectileLevels[i].Projectile.name + "_ef_collection";
            efCollector_companion.name = ProjectileLevels[i].Projectile.name + "_ef_collection_companion";

            pjCollector.transform.parent = SceneObjectContainer.Projectiles_gun_current.transform;
            pjCollector_companion.transform.parent = SceneObjectContainer.Projectiles_gun_current.transform;
            efCollector.transform.parent = SceneObjectContainer.Projectiles_gun_current.transform;
            efCollector_companion.transform.parent = SceneObjectContainer.Projectiles_gun_current.transform;

            //projectileCount calculate
            if (PauseLength == 0)
            {
                projectileCount = (int)(tempPj.Range / (tempPj.Speed * firerateCap) + 0.9f);
            }
            else
            {
                float pauseRange = tempPj.Speed * PauseLength;
                float rangeWoPause = tempPj.Range - pauseRange * (int)(tempPj.Range / ((shotsBetweenPause ) * firerateCap + pauseRange));
                projectileCount = (int)(rangeWoPause / (tempPj.Speed * firerateCap) + 0.9f);
            }
            ProjectileLevels[i].projectilesCached = new GameObject[projectileCount];
            ProjectileLevels[i].projectilesCachedComponent = new Projectile[projectileCount];

            if (Companion != null)
            {
                ProjectileLevels[i].projectilesCached_companion = new GameObject[projectileCount];
                ProjectileLevels[i].projectilesCachedComponent_companion = new Projectile[projectileCount];
            }
            else
            {
                Destroy(pjCollector_companion);
                Destroy(efCollector_companion);
            }

            ProjectileLevels[i].projectileCount = projectileCount;

            //Instantiate projectiles
            for (int j = 0; j < projectileCount; j++)
            {
                //player
                GameObject temp;
                temp = Instantiate(ProjectileLevels[i].Projectile, pjCollector.transform);
                temp.SetActive(false);

                ProjectileLevels[i].projectilesCached[j] = temp;
                ProjectileLevels[i].projectilesCachedComponent[j] = temp.GetComponent<Projectile>();

                GameObject ef_pierce = ProjectileLevels[i].projectilesCachedComponent[j].PierceEffect_cached;
                if (ef_pierce != null)
                {
                    ef_pierce.transform.SetParent(efCollector.transform);
                }

                GameObject ef_die = ProjectileLevels[i].projectilesCachedComponent[j].DieEffect_cached;
                if (ef_die != null)
                {
                    ef_die.transform.SetParent(efCollector.transform);
                }
                

                //companion
                if (Companion != null)
                {
                    temp = Instantiate(ProjectileLevels[i].Projectile, pjCollector_companion.transform);
                    temp.SetActive(false);                    
                    ProjectileLevels[i].projectilesCached_companion[j] = temp;
                    ProjectileLevels[i].projectilesCachedComponent_companion[j] = temp.GetComponent<Projectile>();

                    GameObject ef_pierce_companion = ProjectileLevels[i].projectilesCachedComponent_companion[j].PierceEffect_cached;
                    if (ef_pierce_companion != null)
                    {
                        ef_pierce_companion.transform.SetParent(efCollector_companion.transform);
                    }

                    GameObject ef_die_companion = ProjectileLevels[i].projectilesCachedComponent_companion[j].DieEffect_cached;
                    if (ef_die_companion != null)
                    {
                        ef_die_companion.transform.SetParent(efCollector_companion.transform);
                    }                    
                }
            }

            //Destroy empty effect collections
            if (efCollector.transform.childCount == 0)
            {
                Destroy(efCollector);
            }
            if (efCollector_companion.transform.childCount == 0)
            {
                Destroy(efCollector_companion);
            }
        }
    }
}