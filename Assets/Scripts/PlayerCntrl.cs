using UnityEngine;

public class PlayerCntrl : MonoBehaviour
{
    public enum GameState { Menu, Playing, GameOver };

    public Animator uiAnim;

    public float velocity = 1.5f;
    public float rotationSpeed = 10f;
    public float lerpSpeed = 3f;

    private Rigidbody2D rb;
    private Transform trans;

    private GameState state = GameState.Menu;
    private Vector3 startPos = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = transform;
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
        }
    }

    public void Retry()
    {
        if (state != GameState.GameOver) return;
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
