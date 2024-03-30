using UnityEngine;

public class FullscreenSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 screenSize = Vector2.zero;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (new Vector2(Screen.width, Screen.height) != screenSize)
        {

            float spriteWidth = spriteRenderer.sprite.bounds.size.x;
            float screenHeight = Camera.main.orthographicSize * 2f;
            float screenWidth = screenHeight * Screen.width / Screen.height;

            Vector3 scale = transform.localScale;
            scale.x = screenWidth / spriteWidth;
            transform.localScale = scale;

            screenSize = new Vector2(Screen.width, Screen.height);
        }
    }
}