using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float thrustForce = 1f;
    public float maxSpeed = 5;
    public float scoremutiplier = 10f; 

    [Header("other")]
    public GameObject boostFlame;

    //PRIVATE VARIABLES
    private Rigidbody2D _rb;
    private float _elaspsedTime = 0f;
    private float _score = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _elaspsedTime += Time.deltaTime;
        _score = Mathf.FloorToInt(_elaspsedTime * scoremutiplier);
        Thrust();
        FlameOn();
        Debug.Log(_score);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void Thrust()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;
            _rb.AddForce(direction * thrustForce);
            Debug.Log("Mouse Position: " + mousePos);

            if(_rb.linearVelocity.magnitude > maxSpeed)
                _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void FlameOn()
    {
        if(Mouse.current.leftButton.isPressed)
            boostFlame.SetActive(true);
        else
            boostFlame.SetActive(false);
    }
}
