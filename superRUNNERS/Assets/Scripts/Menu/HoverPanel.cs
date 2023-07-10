using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverPanel : MonoBehaviour
{
    public static HoverPanel instance;

    [SerializeField]
    private GameObject panel;
    [Header("Score text")]
    [SerializeField]
    private TextMeshProUGUI killScore;
    [SerializeField]
    private TextMeshProUGUI roomScore;

    [Header("Descriptions")]
    [SerializeField]
    private GameObject easyDesc;
    [SerializeField]
    private GameObject normalDesc;
    [SerializeField]
    private GameObject hardDesc;
    private GameObject selectedDesc = null;

    private readonly string easyFilename = "score00.dat";
    private readonly string normalFilename = "score01.dat";
    private readonly string hardFilename = "score02.dat";

    private struct DiffScores
    {
        public int kills;
        public int rooms;
    }
    private DiffScores easy, normal, hard;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        easy = LoadHighScores(easyFilename);
        normal = LoadHighScores(normalFilename);
        hard = LoadHighScores(hardFilename);
    }

    private DiffScores LoadHighScores(string fileName)
    {
        DiffScores loadedScore = new DiffScores { kills = 0, rooms = 0};

        if (FileManager.LoadFromFile(fileName, out var json))
        {
            Score score = new Score();
            score.LoadJson(json);

            loadedScore.kills = score.KillScore;
            loadedScore.rooms = score.RoomScore;
        }

        return loadedScore; 
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowPanel(Difficulty difficulty)
    {
        DiffScores loadedScore = new DiffScores();

        switch (difficulty)
        {
            case Difficulty.easy:
                loadedScore = easy;
                selectedDesc = easyDesc;
                break;
            case Difficulty.normal:
                loadedScore = normal;
                selectedDesc = normalDesc;
                break;
            case Difficulty.hard:
                loadedScore = hard;
                selectedDesc = hardDesc;
                break;
        }

        killScore.SetText(loadedScore.kills.ToString());
        roomScore.SetText(loadedScore.rooms.ToString());
        selectedDesc.SetActive(true);
        panel.SetActive(true);
    }

    public void HidePanel()
    {
        panel.SetActive(false);

        if (selectedDesc != null)
        {
            selectedDesc.SetActive(false);
            selectedDesc = null;
        }
    }
}
