using UnityEngine;
using TMPro;
using Plugins.Audio.Core;

public class PlayerCntrl : MonoBehaviour
{
    public enum GameState { Menu, Playing, GameOver };

    public Animator uiAnim;
    public TMP_Text scoreTxt;
    public TextTranslator highscoreTxt;

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
        if (obj.CompareTag("Obstacle"))
        {
            state = GameState.GameOver;
            rb.simulated = false;
            uiAnim.Play("transitionToGameOver");
            audio.Play("ded");

            if (score > GameData.data.highscore)
            {
                GameData.data.highscore = score;
                GameData.SaveData();
                YandexGames.Instance.SaveToLeaderboard(GameData.data.highscore);
            }
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
            }
            rb.velocity = Vector2.up * velocity;
            audio.Play("pop");
        }
    }

    public void AddScore(int scoreCount)
    {
        score += scoreCount;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreTxt.text = ' ' + score.ToString();
    }

    public void UpdateHighscore()
    {
        highscoreTxt.AddAdditionalText(GameData.data.highscore.ToString());
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
        uiAnim.Play("inMenu");
        trans.position = startPos;
        trans.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (state == GameState.Playing)
            trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.Euler(0, 0, rb.velocity.y * rotationSpeed), lerpSpeed * Time.fixedDeltaTime);
    }
}
