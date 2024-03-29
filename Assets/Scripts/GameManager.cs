using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject loadingPanel;

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
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (GameData.dataLoaded) DataLoaded(false);
        else if (Application.isEditor) GameData.LoadData();
        else loadingPanel.SetActive(true);
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
