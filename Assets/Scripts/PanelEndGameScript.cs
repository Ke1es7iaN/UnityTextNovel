using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelEndGameScript : MonoBehaviour
{
    public GameObject Decision { get; set; }
    public GameObject Count { get; set; }
    public GameObject Hint { get; set; }
    public GameObject PointsParam { get; set; }

    //public GameObject Chapter { get; set; }
    public int ChapterIndex { get; set; }

    private Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        Decision = GameObject.Find("Decision");
        Count = GameObject.Find("Count");
        PointsParam = GameObject.Find("PointsParam");
        Hint = GameObject.Find("Hint");

        controller = GameObject.Find("GameController").GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChapterEnded(int chapIndex, int points, int pointsMax)
    {
        ChapterIndex = chapIndex;
        switch (chapIndex)
        {
            case 0:
                Count.GetComponent<Text>().text = Mathf.RoundToInt((float)points / (float)pointsMax * 100f).ToString() + "%";
                PointsParam.GetComponent<Text>().text = "Доверие собеседника достигнуто на:";
                Hint.GetComponent<Text>().text = "Выбирайте фразы, которые помогут укрепить доверие собеседника";
                break;
            case 1:
                Count.GetComponent<Text>().text = points.ToString() + "/" + pointsMax.ToString();
                PointsParam.GetComponent<Text>().text = "Важных подробностей подмечено:";
                Hint.GetComponent<Text>().text = "Выбирайте фразы, которые подтолкнут собеседника на раскрытие важных подробностей";
                break;

            case 2: break;
            case 3: break;
        };
        Decision.GetComponent<Text>().text = "Глава не пройдена";
        Decision.GetComponent<Text>().color = new Color(0.7383f,0.1055f,0);

        if (Mathf.RoundToInt((float)points / (float)pointsMax * 100f) >= 80)
        {
            Decision.GetComponent<Text>().text = "Глава пройдена";
            Decision.GetComponent<Text>().color = new Color(0, 0.6875f, 0.2617f);

            if (ChapterIndex < controller.PanelMenu.GetComponent<PanelMenuScript>().Chapters.Count-1)
                controller.PanelMenu.GetComponent<PanelMenuScript>().OpenChapter();
        }
    }

    public void Restart()
    {
        controller.PanelInGame.SetActive(true);
        controller.PanelInGame.GetComponent<PanelInGameScript>().StartChapter(ChapterIndex);

        controller.PanelEndGame.SetActive(false);
    }

    public void ToMenu()
    {
        controller.PanelMenu.SetActive(true);
        controller.PanelMenu.GetComponent<PanelMenuScript>().ChosenChapterIndex = -1;

        controller.PanelEndGame.SetActive(false);
    }
}
