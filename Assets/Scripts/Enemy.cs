using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private Rigidbody enemyRB;
    private float speed = 3.0f;
    private float harderSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if(gameObject.name == "Harder Enemy(Clone)")
        {
            speed = harderSpeed;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        enemyRB.AddForce(playerDirection * speed);

        if(transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}
