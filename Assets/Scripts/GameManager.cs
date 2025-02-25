using Plugins.Audio.Core;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject loadingPanel;
    public PlayerCntrl player;

    public static GameManager Instance { get; private set; }

    public void DataLoaded(bool firstTime)
    {
        loadingPanel.SetActive(false);
        if (firstTime) YandexGames.Instance.GameInitialized();

        if (GameData.data.prevGameVersion != Application.version.ToString())
        {
            GameData.data.prevGameVersion = Application.version.ToString();
            GameData.SaveData();
        }
        player.enabled = true;
        player.UpdateHighscore();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (GameData.dataLoaded) DataLoaded(false);
        else if (Application.isEditor) GameData.LoadData();
        else loadingPanel.SetActive(true);

        GetComponent<SourceAudio>().Play("nuggetMusic");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        CancelInvoke();
    }
}
