using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float initialForce = 100f; // Initial force applied to the bullet
    public float timer = 10f;

    // public GameObject afterimage;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * initialForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        // GameObject afterimageBullet = Instantiate(afterimage, transform.position - transform.forward * 0.1f, transform.rotation);
        // Destroy(afterimageBullet, 0.1f);
        if (timer<0)
        {
            Destroy(gameObject);
        }
    }
}
