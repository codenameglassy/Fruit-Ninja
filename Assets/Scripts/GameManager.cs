using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct SpawnPos
{
    public Transform spawnPosition;

    [Range(-90,90)]
    public int forceAngleLeft;
    [Range(-90, 90)]
    public int forceAngleRight;
    [Range(0,1000)]
    public float force;
}
[System.Serializable]
public struct Levels
{
    public int Level;
    public float timescale;
    public int score;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Tooltip("Position To spwan fruits from")]
    public SpawnPos[] spawnPos;

    [Space(10)]
    public GameObject[] fruits;

    [Space(10)]
    public GameObject[] splashEffects;

    public Transform lifeSpace;

    public TMP_Text scoreText;
    public TMP_Text fruitsCutText;
    public TMP_Text GameOverScoreText;
    public TMP_Text MaxComboText;
    public TMP_Text GameOverhighscoreText;
    public TMP_Text congratulationText;

    public List<GameObject> lifesPrefab;

    public int lifes;

    public int score;
    public int combo=1;

    public float timeScale;


    public float fruitscut;

    private int maxCombo;

    public List<Levels> levels;

    public Levels activeLevel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnFruits");
        timeScale = 1f;
        Time.timeScale = timeScale;
        soundManager.instance.PlaySound(SoundType.backgroundSound);
        activeLevel = levels[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        UIManager.instance.DisableCombo();
       
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = timeScale;
    }

    public void SlowGame(float value)
    {
        Time.timeScale = value; 
    }

    IEnumerator SpawnFruits()
    {
        while (true)
        {
            int fruitindex = Random.Range(0, fruits.Length);
            int spawnIndex = Random.Range(0, spawnPos.Length);
            float a =Random.Range( spawnPos[spawnIndex].forceAngleLeft * Mathf.Deg2Rad, spawnPos[spawnIndex].forceAngleRight * Mathf.Deg2Rad);
            Vector3 dir = (transform.up * Mathf.Cos(a) + transform.right * Mathf.Sin(a)).normalized;
            GameObject fruit =Instantiate(fruits[fruitindex],spawnPos[spawnIndex].spawnPosition.position,Quaternion.identity);
            fruit.GetComponent<fruitController>().AddForce(dir, spawnPos[spawnIndex].force);
            yield return new WaitForSeconds(0.5f*timeScale);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (SpawnPos pos in spawnPos)
        {
            float a = pos.forceAngleLeft * Mathf.Deg2Rad;
            Vector3 dir = (transform.up * Mathf.Cos(a) + transform.right *Mathf.Sin(a)).normalized;
            Gizmos.DrawRay(pos.spawnPosition.position, dir*pos.force);
            float b = pos.forceAngleRight * Mathf.Deg2Rad;
            Vector3 dirb = (transform.up * Mathf.Cos(b) + transform.right * Mathf.Sin(b)).normalized;
            Gizmos.DrawRay(pos.spawnPosition.position, dirb*pos.force);
        }
    }

    public void PlaySplash(Vector2 position,Color splashColor,float scale)
    {
        int splashEffectIndex = Random.Range(0, splashEffects.Length);
        GameObject splashObject = Instantiate(splashEffects[splashEffectIndex], position, Quaternion.identity);
        splashObject.GetComponent<SpriteRenderer>().color = splashColor;
        splashObject.transform.localScale = splashObject.transform.localScale*scale;
        Destroy(splashObject, 5);
    }
    private void SwitchLevel()
    {
        timeScale = activeLevel.timescale;
        Time.timeScale = timeScale;
    }
    public void AddScore(int amount)
    {
        score += amount*combo;
        scoreText.text = score.ToString();
        for (int i = levels.Count-1; i >= 0; i--)
        {
            if(score>levels[i].score )
            {
                activeLevel = levels[i];
                SwitchLevel();
                return;
            }
        }
    }

    public void increaseCombo()
    {
        combo++;
        if (combo > maxCombo)
            maxCombo = combo;
    }

    public void DecreseLife()
    {
        lifes--;


        //GameOver State
        if (lifes <= 0)
        {
            UIManager.instance.SwitchCanvas(UIPanelType.GameOver);
            PauseGame();
            if (score > PlayerPrefs.GetInt("Highscore"))
            {
                PlayerPrefs.SetInt("Highscore" ,score);
                congratulationText.gameObject.SetActive(true);
            }
            fruitsCutText.text = "Fruits Cut :  " + fruitscut.ToString();
            GameOverScoreText.text = "Score:          " + score.ToString();
            MaxComboText.text = "Max Combo:  " + maxCombo.ToString();
            GameOverhighscoreText.text = "High Score :    " + PlayerPrefs.GetInt("Highscore").ToString();

        }
        else
        {
            Destroy(lifeSpace.GetChild(0).gameObject);
        }
        //StartCoroutine("DecreaseLife");
    }

    IEnumerator DecreaseLife()
    {
        yield return null;
    }
}
