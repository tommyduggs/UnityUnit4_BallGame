using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public GameObject enemy;
    private float speed = 10f;
    private float force = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.gameObject != null)
        {
            transform.position += (enemy.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRB = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (other.gameObject.transform.position - transform.position).normalized;
            enemyRB.AddForce(awayFromPlayer * force, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
