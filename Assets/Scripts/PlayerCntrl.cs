using UnityEngine;
using TMPro;
using Plugins.Audio.Core;

public class PlayerCntrl : MonoBehaviour
{
    public enum GameState { Menu, Playing, GameOver };

    public Animator uiAnim;
    public TMP_Text scoreTxt;
    public TextTranslator highscoreTxt;
    public ObstacleSpawner obstacleSpawner;
    public Ground ground;

    public float velocity = 1.5f;
    public float rotationSpeed = 10f;
    public float lerpSpeed = 3f;

    private int score = 0;

    private Rigidbody2D rb;
    private Transform trans;
    private SourceAudio audio;

    private GameState state = GameState.Menu;
    private Vector3 startPos = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = transform;
        audio = GetComponent<SourceAudio>();
        startPos = trans.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != GameState.Playing) return;
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Obstacle") || obj.CompareTag("Ground"))
        {
            state = GameState.GameOver;
            rb.simulated = false;
            uiAnim.Play("transitionToGameOver");
            audio.PlayOneShot("ded");
            obstacleSpawner.enabled = false;
            ground.enabled = false;

            if (score > GameData.data.highscore)
            {
                GameData.data.highscore = score;
                UpdateHighscore();
                GameData.SaveData();
                YandexGames.Instance.SaveToLeaderboard(GameData.data.highscore);
            }
            ObstacleCntrl[] obstacles = FindObjectsByType<ObstacleCntrl>(FindObjectsSortMode.None);
            foreach (var obstacle in obstacles) obstacle.enabled = false;
        }
    }

    private void Update()
    {
        if (state == GameState.GameOver) return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (state == GameState.Menu)
            {
                state = GameState.Playing;
                rb.simulated = true;
                uiAnim.Play("transitionToGame");
                obstacleSpawner.enabled = true;
                ground.enabled = true;
            }
            rb.velocity = Vector2.up * velocity;
            audio.PlayOneShot("pop");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameData.data.highscore = 0;
            GameData.SaveData();
            UpdateHighscore();
        }
    }

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        UpdateScore();

        if (scoreAmount > 1) audio.PlayOneShot("score_5");
        else audio.PlayOneShot("score_1");
    }

    public void UpdateScore()
    {
        scoreTxt.text = score.ToString();
    }

    public void UpdateHighscore()
    {
        highscoreTxt.AddAdditionalText(' ' + GameData.data.highscore.ToString());
    }

    public void RetryRequest()
    {
        if (state != GameState.GameOver) return;
        YandexGames.Instance.ShowAd(Retry);
    }

    public void Retry()
    {
        score = 0;
        UpdateScore();
        state = GameState.Menu;
        uiAnim.Play("transitionToMenu");
        trans.position = startPos;
        trans.rotation = Quaternion.identity;

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i]);
            obstacles[i] = null;
        }
    }

    private void FixedUpdate()
    {
        if (state == GameState.Playing)
        {
            trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.Euler(0, 0, rb.velocity.y * rotationSpeed), lerpSpeed * Time.fixedDeltaTime);
            trans.position = new Vector3(0f, trans.position.y, trans.position.z);
        }
    }
}
