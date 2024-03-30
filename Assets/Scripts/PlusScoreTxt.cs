using TMPro;
using UnityEngine;

public class PlusScoreTxt : MonoBehaviour
{
    public float lerpSpeed = 1f;
    public float posOffset = 0f;
    public Color targetColor = Color.white;
    public float lifeTime = 4.0f;

    private TMP_Text txt;
    private Transform trans;
    private Vector3 targetPos;

    private void Start()
    {
        txt = GetComponent<TMP_Text>();
        trans = transform;
        targetPos = trans.position + (Vector3.up * posOffset);
        Invoke(nameof(Remove), lifeTime);
    }

    private void Update()
    {
        txt.color = Color.Lerp(txt.color, targetColor, lerpSpeed * Time.deltaTime);
        trans.position = Vector3.Lerp(trans.position, targetPos, lerpSpeed * Time.deltaTime);
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}
