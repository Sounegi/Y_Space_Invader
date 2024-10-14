using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public Player player;

    [SerializeField] bool gameSet;
    [SerializeField] bool spawnRock;

    [SerializeField] int round;
    [SerializeField] int enemyNum;
    [SerializeField] int enemyCount;
    [SerializeField] int spawnCount;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject bigrockPrefab;
    [SerializeField] List<Transform> spawnPoint;
    [SerializeField] List<Transform> rockSpawn;
    [SerializeField] GameObject[] enemies;

    public float spawnTime = 9.0f;
    public float maxSpawnTime = 10.0f;

    public int highScore;
    public int curScore;

    [SerializeField] Text highScoreText;
    [SerializeField] Text scoreText;
    [SerializeField] Text roundText;

    [SerializeField] GameObject endPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameSet = false;
        spawnRock = false;
        round = 1;
        enemyNum = 5;
        enemyCount = 0;
        spawnCount = 0;
        UpdateRound();
        UpdateScore(0);
        endPanel.SetActive(false);
        Time.timeScale = 1;
        player.playerDied.AddListener(PlayerLose);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!gameSet)
        {
            if(enemyCount == 0)
            {
                enemies = new GameObject[enemyNum];
            }
            SpawnEnemy();
            //StartCoroutine(SpawnEnemy(enemyCount));
        }
        if(gameSet && enemyCount == 0)
        {
            round += 1;
            enemyNum += 5;
            spawnCount = 0;
            if(maxSpawnTime >= 1f)
            {
                maxSpawnTime -= 0.5f;
            }
            else
            {
                maxSpawnTime = 1f;
            }
            UpdateRound();
            UpdateScore(500);
            gameSet = false;
        }
        if (spawnRock)
        {
            StartCoroutine(ReleaseRock());
            spawnRock = false;
        }
    }

    void SpawnEnemy()
    {
        if(enemyCount >= enemyNum)
        {
            return;
        }
        if (spawnTime >= maxSpawnTime)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint[spawnCount %5].position, Quaternion.identity);
            enemy.GetComponent<Enemy>().Id = spawnCount;
            enemy.GetComponent<Enemy>().die.AddListener(UpdateEnemyCount);
            enemies[spawnCount] = enemy;
            enemyCount += 1;
            spawnCount += 1;
            spawnTime = 0f;
            if(spawnCount == enemyNum)
            {
                gameSet = true;
                spawnRock = true;
            }
        }
        else
        {
            spawnTime = spawnTime + Time.deltaTime;
        }
        
    }

    IEnumerator ReleaseRock()
    {
        bool big = (0.7 < Random.Range(0.0f, 1.0f));
        bool dir = (0.5 < Random.Range(0.0f, 1.0f));
        GameObject rock = Instantiate((big ? bigrockPrefab : rockPrefab), (dir ? new Vector3(rockSpawn[0].position.x, rockSpawn[0].position.y + Random.Range(-2f,2f), rockSpawn[0].position.z) : new Vector3(rockSpawn[1].position.x, rockSpawn[1].position.y + Random.Range(-2f, 2f), rockSpawn[1].position.z)), Quaternion.identity);
        rock.GetComponent<Rigidbody2D>().velocity = new Vector2((dir? 3f:-3f), 0.0f);
        yield return new WaitForSeconds(8f);
        spawnRock = true;
    }

    void UpdateEnemyCount(int id)
    {
        enemies[id].GetComponent<Enemy>().die.RemoveListener(UpdateEnemyCount);
        Destroy(enemies[id].gameObject);
        UpdateScore(100);
        
        enemyCount -= 1;
    }

    private void UpdateScore(int point)
    {
        curScore += point;
        if(curScore >= highScore)
        {
            highScore = curScore;
            UpdateHScore();
        }
        scoreText.text = "Score: "+curScore;
    }

    public void PlayerLose()
    {
        player.playerDied.RemoveListener(PlayerLose);
        Time.timeScale = 0;
        endPanel.SetActive(true);
    }

    void UpdateHScore()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    void UpdateRound()
    {
        roundText.text = "Round " + round;
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
