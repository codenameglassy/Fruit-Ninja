using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RamailoGames;

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
    public GameObject[] fruitsPrefabs;
    public GameObject bombPrefab;

    [Space(10)]
    public GameObject[] splashEffects;

    public Transform lifeSpace;

    [Header("UI Texts")]
    public TMP_Text scoreText;
    public TMP_Text fruitsCutText;
    public TMP_Text GameOverScoreText;
    public TMP_Text MaxComboText;
    public TMP_Text GameOverhighscoreText;
    public TMP_Text congratulationText;

    public List<GameObject> lifesPrefab;

    public List<Levels> levels;

    [SerializeField]private float spawnDuration=0.5f;

    [Space(10)]
    [Range(0,100)]
    [SerializeField]private int bombSpawnChance=10;

    [HideInInspector] public int lifes;
    [HideInInspector] public int score;
    [HideInInspector] public int combo=1;
    public float timeScale;
    [HideInInspector] public float fruitscut;
    public Levels activeLevel;

    private int maxCombo;
    private int startTime;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnFruits");
        timeScale = 0.75f;
        Time.timeScale = timeScale;
        activeLevel = levels[0];
        startTime =(int) Time.unscaledTime;
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

            int spawnIndex = Random.Range(0, spawnPos.Length);
            float a =Random.Range( spawnPos[spawnIndex].forceAngleLeft * Mathf.Deg2Rad, spawnPos[spawnIndex].forceAngleRight * Mathf.Deg2Rad);
            Vector3 dir = (transform.up * Mathf.Cos(a) + transform.right * Mathf.Sin(a)).normalized;
            GameObject fruit;
            if (Random.Range(0, 100) <= bombSpawnChance)
            {
                fruit = Instantiate(bombPrefab, spawnPos[spawnIndex].spawnPosition.position, Quaternion.identity);
            }
            else
            {
                int fruitIndex = Random.Range(0, fruitsPrefabs.Length);
                fruit = Instantiate(fruitsPrefabs[fruitIndex], spawnPos[spawnIndex].spawnPosition.position, Quaternion.identity);
            }
            fruit.GetComponent<fruitController>().AddForce(dir, spawnPos[spawnIndex].force);
            yield return new WaitForSeconds(spawnDuration * timeScale);
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
            GameOver();
        }
        else
        {
            Destroy(lifeSpace.GetChild(0).gameObject);
        }
    }

    public void GameOver()
    {
        UIManager.instance.SwitchCanvas(UIPanelType.GameOver);
        PauseGame();
        fruitsCutText.text = "Fruits Cut :  " + fruitscut.ToString();
        GameOverScoreText.text = "Score:          " + score.ToString();
        MaxComboText.text = "Max Combo:  " + maxCombo.ToString();
        int playTime =(int)Time.unscaledTime - startTime;
        ScoreAPI.SubmitScore(score,playTime, (bool s, string msg) => { });
        GetHighScore();
    }

    void GetHighScore()
    {
        ScoreAPI.GetData((bool s, Data_RequestData d) => {
            if (s)
            {
                if (score >= d.high_score)
                {
                    GameOverhighscoreText.text = "High Score :    " + score.ToString();
                    congratulationText.gameObject.SetActive(true);
                }
                else
                {
                    GameOverhighscoreText.text = "High Score :    " + d.high_score.ToString();
                }
            }
        });
    }

}
