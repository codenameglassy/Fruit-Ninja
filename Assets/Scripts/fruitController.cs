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
    [SerializeField] private float powerUpDuration=2;
    [SerializeField] private float slowEffects=2;
    [SerializeField] private float combofruitLife=2;

    public GameObject rayObjPrefab;
    public GameObject lighteningEffectPrefab;
    public GameObject combolighteningEffectPrefab;

    [Space(10)]
    [Header("For camera Shake")]
    public float traumaAmount;
    public ShakeData cameraShakeData;

    private float rotationSpeed;
    private float flowingSpeed;
    private float spawnTime;
    private Rigidbody2D rb;

    public bool canRotate;
    [HideInInspector] public bool cut;
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
        canRotate=true;

}

public void AddForce(Vector2 direction, float force)
    {
        float totalforce = (force + flowingSpeed);
        rb.AddForce(totalforce * direction, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        
        if (GameManager.instance != null)
        {
            if (Time.time - spawnTime > 1 * GameManager.instance.timeScale)
            {
                if (cameraController.instance.CheckBound(transform.position))
                {
                    
                    //if(fruitType != FruitType.bomb)
                    //{
                    //    GameManager.instance.DecreseLife();
                    //}
                    Destroy(this.gameObject);

                }
            }
        }
        if (fruitType == FruitType.bomb)
        {
            if (Time.timeScale == 0)
            {
                GetComponent<AudioSource>().volume = 0;
            }
        }
    }


    private float comboLightAngle;
    private float lastCut;
    private float lastComboCut;
    public void CutFruit(Vector3 cutPos)
    {
        if (fruitType == FruitType.bomb)
        {
            if (cut)
                return;
            Camera.main.GetComponent<CameraShaker>().Shake(cameraShakeData);
            StartCoroutine("BombExplosionEffect");
        }
        else if (fruitType == FruitType.lighteningPowerUp)
        {
            GameObject lighteningEffect = Instantiate(lighteningEffectPrefab,transform.position,Quaternion.identity);
            GameManager.instance.SlowGame(slowEffects, powerUpDuration);
            Destroy(lighteningEffect,slowEffects*0.5f);
            Destroy(this.gameObject);
        }
        else if (fruitType == FruitType.comboMelon)
        {
            if (!cut)
            {
                comboLightAngle = 0;
                if(GameManager.instance!=null)
                    GameManager.instance.PauseWithCollision(combofruitLife);
                Destroy(this.gameObject, combofruitLife);
                cut = true;
            }
            else
            {
                if ((Time.unscaledTime - lastCut) > 0.05f)
                {
                    GameObject combolight = Instantiate(combolighteningEffectPrefab, transform.position, Quaternion.Euler(new(0, 0, comboLightAngle)), this.transform);
                    comboLightAngle += 15;
                    lastCut = Time.unscaledTime;
                    Camera.main.GetComponent<CameraShaker>().Shake(cameraShakeData);

                    GameManager.instance.AddScore(10);
                    soundManager.instance.PlaySound(SoundType.slashSound);
                    GameManager.instance.PlaySplash(transform.position, Color.red, 1.2f);

                    if ((Time.unscaledTime - lastComboCut) < 1)
                    {
                        GameManager.instance.increaseCombo();

                        UIManager.instance.ShowCombo(Camera.main.WorldToScreenPoint(transform.position), (int)GameManager.instance.combo);
                    }
                    else
                    {
                        GameManager.instance.combo = 1;
                        UIManager.instance.DisableCombo();
                    }

                    lastComboCut = Time.unscaledTime;
                }
            }
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
            }
            Rigidbody2D rigidbL = secondhalf.GetComponent<Rigidbody2D>();
            if (rigidbL != null)
            {
                rigidbL.AddTorque(-100);
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
        soundManager.instance.PlaySound(SoundType.explosion);
        while (initialDegree != 360)
        {
            Instantiate(rayObjPrefab, transform.position, Quaternion.Euler(0, 0, initialDegree), this.transform);
            initialDegree += 45;
            yield return new WaitForSecondsRealtime(0.2f);
        }
        UIManager.instance.SwitchCanvas(UIPanelType.blank);
        
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(this.gameObject);
        GameManager.instance.GameOver();
    }
}
