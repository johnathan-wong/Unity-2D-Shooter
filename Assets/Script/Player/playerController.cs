using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    // Player Setting Variable
    public float moveSpd = 4f;
    private Rigidbody2D rb;
    // Player Equipment Variable
    private Transform rHand;
    private Transform lHand;
    private Transform curHand;

    // Game Variable
    private bool isPickingup = false;
    private GameObject canvasObject;
    private List<GameObject> canPickupItems = new List<GameObject>();
    private int[] onHandRanged = { -1, -1 };
    private int[] onHandMelee = { -1, -1 };
    private GameObject pickingThis;
    private bool left = true;
    private bool canShoot = true;

    // System Var
    private GameObject gameManager;
    private weaponManager weaponDatabase;


    void Start()
    {
        // Define Player body
        rb = GetComponent<Rigidbody2D>();
        rHand = transform.Find("Body/rHand");
        lHand = transform.Find("Body/lHand");
        curHand = rHand;
        // 
        canvasObject = GameObject.FindWithTag("UI");
        gameManager = GameObject.FindWithTag("GameController");
        weaponDatabase = gameManager.GetComponent<weaponManager>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Pickups Tracker on enter
        if (other.CompareTag("Pickups"))
        {
            canPickupItems.Add(other.gameObject);
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        // Pickups Tracker on exit
        if (other.CompareTag("Pickups"))
        {
            if (isPickingup && canPickupItems[0] == other.gameObject)
            {
                isPickingup = !isPickingup;
                GameObject handSelector = canvasObject.transform.Find("hand_Selector").gameObject;
                handSelectorMovement selectorMovement = handSelector.GetComponent<handSelectorMovement>();
                selectorMovement.target = null;
                handSelector.SetActive(false);
                // selectorMovement.isPickingup = false;
            }
            canPickupItems.Remove(other.gameObject);
        }

    }


    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));
        Vector3 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Character Head Rotation
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Character Shoot Control
        GunController curGun = curHand.GetComponentInChildren<GunController>();
        if (curGun && canShoot)
        {
            if ((curGun.weaponSettings.automatic && Input.GetMouseButton(0)) || (!curGun.weaponSettings.automatic && Input.GetMouseButtonDown(0)))
            {
                GunController gunInLHand = lHand.GetComponentInChildren<GunController>();
                GunController gunInRHand = rHand.GetComponentInChildren<GunController>();
                // Handle Dual Wielding
                if ((gunInLHand != null && gunInRHand != null) || gunInRHand.weaponSettings.handType != weaponSettings.HandType.OneHanded)
                {
                    if (left && gunInLHand.CheckShoot())
                    {
                        gunInLHand.Shoot();
                        left = false;
                        StartCoroutine(Cooldown(1 / curGun.weaponSettings.fireRate / 2));
                    }
                    else if (!left && gunInRHand.CheckShoot())
                    {
                        gunInRHand.Shoot();
                        left = true;
                        StartCoroutine(Cooldown(1 / curGun.weaponSettings.fireRate / 2));
                    }

                }
                // Handle Left Hand ONLY
                else if (gunInLHand != null)
                {
                    if (gunInLHand.CheckShoot())
                    {
                        gunInLHand.Shoot();
                    }
                }
                // Handle Right Hand ONLY
                else if (gunInRHand != null)
                {
                    if (gunInRHand.CheckShoot())
                    {
                        gunInRHand.Shoot();
                    }
                }


            }

        }


        // Character Pickups Control
        if (Input.GetKeyDown(KeyCode.F) && canPickupItems.Count > 0 && !isPickingup)
        {
            // Enter Pickup
            isPickingup = true;
            pickingThis = canPickupItems[0];

            GameObject handSelector = canvasObject.transform.Find("hand_Selector").gameObject;
            WeaponWheel wheel = handSelector.GetComponent<WeaponWheel>();
            wheel.EnterSelection();
            handSelector.SetActive(true);

        }
        else if ((Input.GetKeyUp(KeyCode.F) || canPickupItems.Count == 0) && isPickingup)
        {
            // Exit Pickup
            isPickingup = false;

            GameObject handSelector = canvasObject.transform.Find("hand_Selector").gameObject;
            WeaponWheel wheel = handSelector.GetComponent<WeaponWheel>();
            int selectedID = wheel.ExitSelection();
            handSelector.SetActive(false);

            // put on hand
            gunPickup thisItem = pickingThis.GetComponent<gunPickup>();
            weaponSettings itm1 = weaponDatabase.WeaponData(onHandRanged[0]);
            weaponSettings itm2 = weaponDatabase.WeaponData(onHandRanged[1]);
            // Debug.Log(itm1);
            // Debug.Log(itm2);

            GameObject childObject = Instantiate(thisItem.gunPrefab);
            weaponSettings weaponData = childObject.GetComponent<GunController>().weaponSettings;
            childObject.SetActive(false);
            switch (selectedID)
            {
                case 1:
                    // Right Hand
                    // TODO: This will need to fix (dropping weapon when full)
                    pickingThis.SetActive(false);

                    if (itm2 != null)
                    {
                        Instantiate(itm2.onGround, transform.position, Quaternion.identity);
                        if (rHand.childCount > 0)
                        {
                            foreach (Transform child in rHand)
                            {
                                Destroy(child.gameObject);
                            }
                        }


                        Debug.Log(onHandRanged[1]);
                        if (itm2.handType == weaponSettings.HandType.TwoHanded)
                        {
                            if (lHand.childCount > 0)
                            {
                                foreach (Transform child in lHand)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            if (weaponData.handType == weaponSettings.HandType.TwoHanded)
                            {
                                onHandRanged[0] = weaponData.id;
                            }
                            else
                            {
                                onHandRanged[0] = -1;
                            }

                        }
                    }
                    onHandRanged[1] = weaponData.id;
                    childObject.SetActive(true);
                    childObject.transform.parent = rHand;
                    childObject.transform.localPosition = Vector3.zero;
                    childObject.transform.localRotation = Quaternion.identity;
                    Destroy(pickingThis);
                    // TODO: apply offset

                    break;

                case 0:
                    // Left Hand
                    pickingThis.SetActive(false);
                    if (itm1 != null)
                    {
                        Instantiate(itm1.onGround, transform.position, Quaternion.identity);
                        if (lHand.childCount > 0)
                        {
                            foreach (Transform child in lHand)
                            {
                                Destroy(child.gameObject);
                            }
                        }
                        if (itm1.handType == weaponSettings.HandType.TwoHanded)
                        {
                            if (rHand.childCount > 0)
                            {
                                foreach (Transform child in rHand)
                                {
                                    Destroy(child.gameObject);
                                }
                            }
                            if (weaponData.handType == weaponSettings.HandType.TwoHanded)
                            {
                                onHandRanged[1] = weaponData.id;
                            }
                            else
                            {
                                onHandRanged[1] = -1;
                            }

                        }
                    }
                    onHandRanged[0] = weaponData.id;
                    childObject.SetActive(true);
                    childObject.transform.parent = lHand;
                    childObject.transform.localPosition = Vector3.zero;
                    childObject.transform.localRotation = Quaternion.identity;
                    Destroy(pickingThis);
                    // TODO: apply offset
                    break;

                default:
                    Debug.Log("Invalid Section on Weapon Wheel ID-" + selectedID);
                    Destroy(childObject);
                    break;
            }
        }

    }

    IEnumerator Cooldown(float t)
    {
        canShoot = false;
        yield return new WaitForSeconds(t);
        canShoot = true;
    }

    void FixedUpdate()
    {
        // Character Movement Control
        float moveH = Input.GetAxisRaw("Horizontal");
        float moveV = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(moveH, moveV, 0f);
        movement.Normalize();
        transform.position += movement * moveSpd * Time.fixedDeltaTime;
    }
}
