using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitController : MonoBehaviour
{
    public GameObject FullFruit;
    public GameObject HalfFruit01;
    public GameObject HalfFruit02;

    public FruitType fruitType;

    [SerializeField] private float maxrotationSpeed;
    [SerializeField] private float minrotationSpeed;
    [SerializeField] private float maxflowingSpeed;
    [SerializeField] private float minflowingSpeed;

    private float rotationSpeed;
    private float flowingSpeed;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationSpeed = Random.Range(minrotationSpeed, maxrotationSpeed);
        flowingSpeed = Random.Range(minflowingSpeed, maxflowingSpeed);
    }

    public void AddForce(Vector2 direction,float force)
    {
        rb.AddForce((force+flowingSpeed) * direction, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,rotationSpeed*Time.deltaTime));
    }
}
