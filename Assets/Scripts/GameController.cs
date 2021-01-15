using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance = null;

    // 管道预制件
    public GameObject pipesPrefabs;

    public Text countText;
    public GameObject gameOverTips;

    // 管道产生的频率，每几秒产生一个
    public float createPipesRate = 3f;

    // 管道中心位置的y最小值
    public float minPipPosY = -1f;
    // 管道中心位置的y最大值
    public float maxPipPosY = 4f;
    // 初始化管道的位置
    public Vector2 startPipPos = new Vector2(-12f, 0f);

    // 统计已经成功过了几个管道
    private int count = 0;
    // 小鸟是否已经死了
    [HideInInspector] public bool isGameOver;

    // 缓存管道的链表，用来复用管道
    private List<GameObject> pipes = new List<GameObject>();
    // 管道缓存的个数
    private const int PIPESTOTAL = 8;
    // 当前管道下标，用来更新管道
    private int currPipesIndex = 0;

    //间隔
    private float distance = 6f;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        count = 0;
        countText.text = "得分: " + count.ToString();

        isGameOver = false;
        gameOverTips.SetActive(false);
        // 开始时，创建出管道缓存
        InitPipesPool();
    }

    private void Update() {

        if (isGameOver && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            GameRestart();
        }
    }

    public void PassOnePip() {
        count++;
        countText.text = "得分: " + count.ToString();
    }

    public void GameOver() {
        if (isGameOver) return;

        isGameOver = true;
        gameOverTips.SetActive(true);
    }

    private void GameRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 初始化管道缓存池
    private void InitPipesPool() {
        Vector2 tempPos = startPipPos;
        float tempX = 0f;
        float randomPosY = 0f;

        for (int i = 0; i < PIPESTOTAL; ++i) {
            tempX += distance;
            randomPosY = Random.Range(minPipPosY, maxPipPosY);
            tempPos.x = startPipPos.x + tempX;
            tempPos.y = randomPosY;

            GameObject obj = Instantiate(pipesPrefabs, tempPos, Quaternion.identity);

            pipes.Add(obj);
        }
    }

    // 更新当前管道的位置
    public void UpdatePipesPosition() {
        int index = currPipesIndex - 1;
        if (index < 0)
        {
            index = PIPESTOTAL - 1;
        }

        float randomPosY = Random.Range(minPipPosY, maxPipPosY);
        Vector2 position = new Vector2(pipes[index].transform.position.x + distance, randomPosY);
        pipes[currPipesIndex].transform.position = position;

        currPipesIndex = (currPipesIndex + 1) % PIPESTOTAL;
    }
}
