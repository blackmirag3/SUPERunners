using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunFire : MonoBehaviour
{
    [SerializeField]
    private GunData gunData;

    // Bullet
    public GameObject bullet;

    // Bullet Force
    public float shootForce;

    // Gun stats
    public int ammoLeft, bulletsShot;
    public float rpm;
    public float spread;
    public float timeBetweenBullets;
    public int bulletsPerShot;
    public int magSize;

    bool shooting, canShoot;

    [HideInInspector] 
    public Camera playerCam;
    public Transform attackPoint;
    [HideInInspector] 
    public GameObject muzzleFlash;
    [HideInInspector] 
    public TextMeshProUGUI ammoDisplay;

    [SerializeField] private string camName = "PlayerCam";
    [SerializeField] private string guiTextname = "AmmoIndicator";

    // bug fixing
    public bool allowInvoke = true;

    public AudioSource shootingSound;

    private void Awake()
    {
        shootForce = gunData.shootForce;
        rpm = gunData.fireRate;
        spread = gunData.spread;
        timeBetweenBullets = gunData.timeBetweenBullets;
        bulletsPerShot = gunData.bulletsPerShot;
        magSize = gunData.magSize;
        
        canShoot = true;
    }

    void Start()
    {
        ammoLeft = magSize;
        playerCam = GameObject.Find(camName).GetComponent<Camera>();
        ammoDisplay = GameObject.Find(guiTextname).GetComponent<TextMeshProUGUI>();
        shootingSound = GetComponent<AudioSource>();
    }

    
    private void Update()
    {
        MyInput();

        if (ammoDisplay != null)
            ammoDisplay.SetText(ammoLeft.ToString());
    }

    private void MyInput()
    {
        // Check for auto/semi-auto firing mechanism
        if (gunData.isAuto)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Shoot
        if (canShoot && shooting && ammoLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }

    }

 
    private void Shoot()
    {
        
        shootingSound.Play();
        canShoot = false;

        // Find exact hit pos using a raycast
        // (0.5f, 0.5f, 0) is middle of screen
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 targetPoint;
        // Check if ray hits
        if (Physics.Raycast(ray, out RaycastHit hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        // Calculate direction from attackPoint to targetPoint
        Vector3 dir = targetPoint - attackPoint.position;

        // Calculate new dir with spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);
        Vector3 dirSpread = dir + new Vector3(x, y, z);

        // Instantiate bullet
        GameObject currBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currBullet.SetActive(true);

        // Rotate bullet to shooting direction
        currBullet.transform.forward = dirSpread.normalized;

        // Add force to bullet
        currBullet.GetComponent<Rigidbody>().AddForce(dirSpread.normalized * shootForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        ammoLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", rpm);
            allowInvoke = false; // Invoke once only
        }

        // More than one bullet per tap
        if (bulletsShot < bulletsPerShot && ammoLeft > 0)
            Invoke("Shoot", timeBetweenBullets);
    }

    private void ResetShot()
    {
        canShoot = true;
        allowInvoke = true;
    }

    // Reset gun when picked up (for now)
    private void OnDisable()
    {
        if (ammoDisplay != null)
            ammoDisplay.SetText("\n");
        ammoLeft = magSize;
    }
}
