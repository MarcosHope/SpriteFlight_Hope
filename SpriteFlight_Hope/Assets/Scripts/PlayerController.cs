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
    public GameObject boarders;

    [Header("Settings")]
    public AudioClip backGroundClip;
    public AudioClip boosterClip;
    public AudioClip crashClip;

    //PRIVATE VARIABLES
    private Rigidbody2D _rb;
    private float _elaspsedTime = 0f;
    private float _score = 0f;
    private Label _scoreText;
    private Button _restartButton;

    //High-Score Code
    private int _highScore;
    private Label _highScoreText;
    private AudioSource _audioSource;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        _restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        _restartButton.style.display = DisplayStyle.None;
        _restartButton.clicked += ReloadScene;

        boarders.SetActive(true);
        // High-Score code 
        _highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        _highScoreText.style.display = DisplayStyle.None;
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        _highScoreText.text = "High Score: " + _highScore;
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
        int currentScore = Mathf.FloorToInt(_score);

        // High-Score code 
        if (currentScore > _highScore)
        {
            _highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.Save();
            _highScoreText.text = "Best: " + _highScore;
        }

        Instantiate(explosionEffect, transform.position, transform.rotation);
        boarders.SetActive(false);
        _restartButton.style.display = DisplayStyle.Flex;

        // High-Score code
        _highScoreText.style.display = DisplayStyle.Flex;

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
