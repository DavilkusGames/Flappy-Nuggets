using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public GameObject plusScorePrefab;
    public int scoreAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCntrl>().AddScore(scoreAmount);
            Instantiate(plusScorePrefab, new Vector3(collision.transform.position.x, transform.position.y, 12f), Quaternion.identity).transform.SetParent(transform);
        }
    }
}
