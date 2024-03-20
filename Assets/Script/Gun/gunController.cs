using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GunController : MonoBehaviour
{
    public weaponSettings weaponSettings; // Reference to the gun settings
    public GameObject bulletPrefab; //Reference to the bullet prefab
    private GameObject crosshair;
    public float num = 100;
    GameObject canvasUI;
    float referencePixelsPerUnit = 100;
    public float cooldownTime = 0.5f;
    private bool canShoot = true;
    void Start()
    {
        GameObject canvasObject = GameObject.Find("UI");
        CanvasScaler canvasScaler = canvasObject.GetComponent<CanvasScaler>();
        if (canvasScaler != null)
        {
            referencePixelsPerUnit = canvasScaler.referencePixelsPerUnit;
        }

        crosshair = GameObject.FindWithTag("UI").transform.Find("crosshair").gameObject;
    }

    void Update()
    {
        // Shooting mechanic
        // if ((weaponSettings.automatic && Input.GetMouseButton(0)) || (!weaponSettings.automatic && Input.GetMouseButtonDown(0)))
        // {

        // }
    }

    public void Shoot()
    {
        // Get reticle
        RectTransform reticleRect = crosshair.GetComponent<RectTransform>();
        Transform player = transform.parent.parent.parent;
        Vector2 reticleSize = reticleRect.sizeDelta;
        Vector3 reticlePosition = crosshair.transform.position;
        reticlePosition = Camera.main.ScreenToWorldPoint(reticlePosition);

        // Calculate boundaries of the reticle square
        float minX = reticlePosition.x - reticleSize.x / 2 / referencePixelsPerUnit;
        float maxX = reticlePosition.x + reticleSize.x / 2 / referencePixelsPerUnit;
        float minY = reticlePosition.y - reticleSize.y / 2 / referencePixelsPerUnit;
        float maxY = reticlePosition.y + reticleSize.y / 2 / referencePixelsPerUnit;

        // Generate random coordinates within the reticle square
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector3 direction = new Vector3(randomX, randomY, 0) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Ensure the bullet always shoots away from the player
        Transform playerTransform = transform.parent.parent.parent;
        if (Vector3.Dot(direction, playerTransform.position - transform.position) > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
        }
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1/weaponSettings.fireRate);
        canShoot = true;
    }

    public bool CheckShoot()
    {
        return canShoot;
    }

}