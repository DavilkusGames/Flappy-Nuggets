using UnityEngine;

public class Ground : MonoBehaviour
{
    public float scrollSpeed = 1f;

    private MeshRenderer meshRenderer;
    private Transform trans;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        trans = transform;
    }

    private void Update()
    {
        float speed = scrollSpeed / trans.localScale.x;
        meshRenderer.material.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
    }
}
