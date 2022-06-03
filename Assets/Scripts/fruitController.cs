using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitController : MonoBehaviour
{
    //public GameObject FullFruit;
    public GameObject HalfFruit;

    public FruitType fruitType;

    [SerializeField] private float maxrotationSpeed;
    [SerializeField] private float minrotationSpeed;
    [SerializeField] private float maxflowingSpeed;
    [SerializeField] private float minflowingSpeed;

    private float rotationSpeed;
    private float flowingSpeed;
    public Rigidbody2D rb;

    public bool cut;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationSpeed = Random.Range(minrotationSpeed, maxrotationSpeed);
        flowingSpeed = Random.Range(minflowingSpeed, maxflowingSpeed);
        cut = false;
        //rb.gravityScale = 0.5f;
    }

    public void AddForce(Vector2 direction,float force)
    {
        float totalforce = (force + flowingSpeed);
        rb.AddForce( totalforce* direction, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,rotationSpeed*Time.deltaTime));
    }

    public void CutFruit()
    {
        GameObject cutPiece = Instantiate(HalfFruit,transform.position,transform.rotation);
        
        Transform firsthalf = cutPiece.transform.GetChild(0);
        Rigidbody2D rigidbR = firsthalf.GetComponent<Rigidbody2D>();
        if (rigidbR != null)
        {
            rigidbR.AddTorque(100);
            //rigidbR.AddForce(Vector2.left*10, ForceMode2D.Impulse);
        }
        Transform secondhalf = cutPiece.transform.GetChild(1);
        Rigidbody2D rigidbL = secondhalf.GetComponent<Rigidbody2D>();
        if (rigidbL != null)
        {
            rigidbL.AddTorque(-100);
            //rigidbR.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        }

        Destroy(this.gameObject);
        
    }
}
