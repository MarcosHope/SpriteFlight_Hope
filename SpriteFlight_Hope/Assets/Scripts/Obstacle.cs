using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Size")]
    public float minSize = 0.5f;
    public float maxSize = 2.0f;

    [Header("Speed")]
    public float minSpeed = 50f;
    public float maxSpeed = 200f;
    public float spinSpeed = 10f;

    [Header("Bounce")]
    public GameObject bounceEffectPrefab;

    //PRIVATE VARIABLES
    private Rigidbody2D _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        //Size
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        //Speed
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        _rb.AddForce(randomDirection * randomSpeed);

        //Spin
        float randomTorque = Random.Range(-spinSpeed, spinSpeed);
        _rb.AddTorque(randomTorque);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity);

        Destroy(bounceEffect, 1f);
    }
}
