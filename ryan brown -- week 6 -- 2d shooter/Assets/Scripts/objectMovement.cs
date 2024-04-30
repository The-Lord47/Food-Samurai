using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class objectMovement : MonoBehaviour
{
    float minSpeed = 5;
    float maxSpeed = 10;
    float maxTorque = 0.5f;
    float xRange = 4;
    float ySpawnPos = -2;
    Rigidbody rb;
    gameManager _gm;
    bladeScript _bs;
    audioManager _am;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManager>();
        _bs = GameObject.FindGameObjectWithTag("Player").GetComponent<bladeScript>();
        _am = FindObjectOfType<audioManager>();

        rb.AddForce(RandomForce(), ForceMode.Impulse);
        rb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -2)
        {
            Destroy(gameObject);
        }
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed,maxSpeed);
    }
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("redApple") || gameObject.CompareTag("greenApple") || gameObject.CompareTag("carrot") || gameObject.CompareTag("pear"))
            {
                if(gameObject.CompareTag("redApple"))
                {
                    Instantiate(_gm.splatterFXs[0], transform.position, Quaternion.LookRotation(_bs.bladeDirection), GameObject.Find("SplatterFX").transform);
                }
                if(gameObject.CompareTag("greenApple") || gameObject.CompareTag("pear"))
                {
                    Instantiate(_gm.splatterFXs[1], transform.position, Quaternion.LookRotation(_bs.bladeDirection), GameObject.Find("SplatterFX").transform);
                }
                if (gameObject.CompareTag("carrot"))
                {
                    Instantiate(_gm.splatterFXs[2], transform.position, Quaternion.LookRotation(_bs.bladeDirection), GameObject.Find("SplatterFX").transform);
                }
                ++_gm.score;
                _am.fruitSFX.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                _am.fruitSFX.GetComponent<AudioSource>().PlayOneShot(_am.fruitSFXclips[Random.Range(0, _am.fruitSFXclips.Length)]);
            }
            else if(gameObject.CompareTag("bad"))
            {
                --_gm.lives;
                Instantiate(_gm.splatterFXs[3], transform.position, Quaternion.Euler(-180,0,0), GameObject.Find("SplatterFX").transform);
                _am.bombSFX.GetComponent<AudioSource>().PlayOneShot(_am.bombSFXclip);
            }
            Destroy(gameObject);
        }
    }

}
