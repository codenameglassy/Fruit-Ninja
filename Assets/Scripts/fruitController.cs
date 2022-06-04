using FirstGearGames.SmoothCameraShaker;
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

    private float spawnTime;
    public bool cut;

    public GameObject rayObjPrefab;

    public float traumaAmount;
    public ShakeData cameraShakeData;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationSpeed = Random.Range(minrotationSpeed, maxrotationSpeed);
        flowingSpeed = Random.Range(minflowingSpeed, maxflowingSpeed);
        cut = false;
        //rb.gravityScale = 0.5f;
    }
    private void Start()
    {
        spawnTime = Time.time;
    }

    public void AddForce(Vector2 direction, float force)
    {
        float totalforce = (force + flowingSpeed);
        rb.AddForce(totalforce * direction, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        if (fruitType != FruitType.bomb)
        {
            if (GameManager.instance != null)
            {
                if (Time.time - spawnTime > 1 * GameManager.instance.timeScale)
                {
                    if (cameraController.instance.CheckBound(transform.position))
                    {
                        Destroy(this.gameObject);
                        GameManager.instance.DecreseLife();
                    }
                }
            }
        }

    }

    public void CutFruit(Vector3 cutPos)
    {
        if (fruitType == FruitType.bomb)
        {
            if (cut)
                return;
            Camera.main.GetComponent<CameraShaker>().Shake(cameraShakeData);
            StartCoroutine("BombExplosionEffect");
        }
        else
        {
            Vector3 targ = cutPos;
            targ.z = 0f;
            Vector3 objectPos = transform.position;
            targ.x -= objectPos.x;
            targ.y -= objectPos.y;

            //Off by 90 degrees for some reasons
            float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg - 90;
            GameObject cutPiece = Instantiate(HalfFruit, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            Transform firsthalf;
            Transform secondhalf;

            if (cutPiece.transform.rotation.x > -90 && cutPiece.transform.rotation.x < 90)
            {
                firsthalf = cutPiece.transform.GetChild(0);
                secondhalf = cutPiece.transform.GetChild(1);
            }
            else
            {
                firsthalf = cutPiece.transform.GetChild(1);
                secondhalf = cutPiece.transform.GetChild(0);
            }
            Rigidbody2D rigidbR = firsthalf.GetComponent<Rigidbody2D>();
            if (rigidbR != null)
            {
                rigidbR.AddTorque(100);
                //rigidbR.AddForce(Vector2.left*10, ForceMode2D.Impulse);
            }
            Rigidbody2D rigidbL = secondhalf.GetComponent<Rigidbody2D>();
            if (rigidbL != null)
            {
                rigidbL.AddTorque(-100);
                //rigidbR.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            }
            if (GameManager.instance != null)
                GameManager.instance.fruitscut++;
            Destroy(this.gameObject);
        }

    }

    IEnumerator BombExplosionEffect()
    {
        int initialDegree = 0;
        GameManager.instance.PauseGame();
        while (initialDegree != 360)
        {
            GameObject rayObj = Instantiate(rayObjPrefab, transform.position, Quaternion.Euler(0, 0, initialDegree),this.transform);
            initialDegree += 45;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        UIManager.instance.SwitchCanvas(UIPanelType.blank);
        
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(this.gameObject);
        UIManager.instance.SwitchCanvas(UIPanelType.GameOver);
    }
}
