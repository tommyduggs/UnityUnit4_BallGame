using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    private GameObject focalPoint;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    public GameObject misslePowerupIndicator;
    public GameObject misslePrefab;
    public float speed = 15.0f;
    public bool hasPowerup = false;
    private GameObject mainCamera;
    private int powerUpStack = 0; // Using this counter to allow extending powerup time when stacking
    private bool hasMissles = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && hasMissles)
        {
            FireMissles();
        }
    }

    private void FixedUpdate()
    {
        Vector3 relativeDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (relativeDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerRB.AddForce(moveDirection * speed);
        }

        powerupIndicator.transform.position = transform.position - new Vector3(0, 0.5f, 0);
        misslePowerupIndicator.transform.position = transform.position - new Vector3(0, 0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            powerUpStack++;
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
        if(other.CompareTag("MisslePowerup"))
        {
            Destroy(other.gameObject);
            misslePowerupIndicator.SetActive(true);
            hasMissles = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRB = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (other.gameObject.transform.position - transform.position).normalized;

            enemyRB.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    private void FireMissles()
    {
        hasMissles = false;
        
        misslePowerupIndicator.SetActive(false);

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        for(int i = 0; i < enemies.Length; i++)
        {
            GameObject missle = Instantiate(misslePrefab,transform.position,misslePrefab.transform.rotation);
            missle.GetComponent<Missle>().enemy = enemies[i].gameObject;
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerUpStack--;
        if(powerUpStack <= 0)
        {
            hasPowerup = false;
            powerupIndicator.SetActive(false);
        }
    }
}
