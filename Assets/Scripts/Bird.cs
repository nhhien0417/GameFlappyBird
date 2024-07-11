using System.Collections;
using Spine.Unity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public float Velocity = 1.5f;
    public float RotationSpeed = 10f;

    public static bool isAlive = true;
    public static bool isExplosion = false;

    public Rigidbody2D Rigidbody;
    public GameObject ExplosionEffect;
    public AudioSource _WingSound, _HitSound, _PointSound, _DieSound;
    public SkeletonAnimation Animation;
    public CameraShake cameraShake;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        enabled = false;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isAlive)
        {
            Rigidbody.velocity = Vector3.up * Velocity;
            _WingSound.Play();
        }

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && isAlive)
            {
                Rigidbody.velocity = Vector3.up * Velocity;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1f, 1f), Mathf.Clamp(transform.position.y, -5f, 5f), -1);
        transform.rotation = Quaternion.Euler(0, 0, Rigidbody.velocity.y * RotationSpeed);

        if (Rigidbody.velocity.y < -3f)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(Rigidbody.velocity.y * 10f, -90f, 90f));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(55f, -90f, 20f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if (isAlive)
            {
                _HitSound.Play();
            }
            isAlive = false;
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.gravityScale = 0;

            Animation.timeScale = 0;
            enabled = false;

            _DieSound.Play();

            if (!isExplosion)
            {
                Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
                StartCoroutine(cameraShake.Shake(0.2f, 0.15f));

                isExplosion = true;
            }

            StartCoroutine(WaitForExposion());
        }
        else if (other.gameObject.tag == "Pipes")
        {
            if (isAlive)
            {
                _HitSound.Play();
            }
            isAlive = false;
            Rigidbody.velocity = Vector3.down * Velocity * 0.8f;
            Animation.timeScale = 0;

            if (!isExplosion)
            {
                Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
                StartCoroutine(cameraShake.Shake(0.3f, 0.3f));

                isExplosion = true;
            }
        }
        else if (other.gameObject.tag == "Scoring")
        {
            if (isAlive)
            {
                FindObjectOfType<GameManager>().IncreaseScore();
                _PointSound.Play();
            }
        }
    }

    private IEnumerator WaitForExposion()
    {
        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<GameManager>().GameOver();
    }
}
