using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float thrustForce = 1f;
    public float maxSpeed = 5;
    public float scoremutiplier = 10f; 
    public UIDocument uiDocument;

    [Header("other")]
    public GameObject boostFlame;
    public GameObject explosionEffect;

    //PRIVATE VARIABLES
    private Rigidbody2D _rb;
    private float _elaspsedTime = 0f;
    private float _score = 0f;
    private Label _scoreText;
    private Button _restartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        _restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        _restartButton.style.display = DisplayStyle.None;
        _restartButton.clicked += ReloadScene;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateScore();
        Thrust();
        FlameOn();
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        _restartButton.style.display = DisplayStyle.Flex;
        Destroy(gameObject);
    }

    private void CalculateScore()
    {
        _elaspsedTime += Time.deltaTime;
        _score = Mathf.FloorToInt(_elaspsedTime * scoremutiplier);
        _scoreText.text = "Score: " + _score;
    }

    private void Thrust()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;
            _rb.AddForce(direction * thrustForce);
           

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

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
