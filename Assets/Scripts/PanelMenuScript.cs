using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelMenuScript : MonoBehaviour
{
    public GameObject MainButton { get; set; }
    public List<GameObject> Chapters { get; set; }
    public int OpenedChapterIndex { get; set; }

    public int ChosenChapterIndex;
    public int PlayingChapterIndex { get; set; }

    private Controller controller; 
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<Controller>();

        controller.PanelInGame.SetActive(false);
        controller.PanelEndGame.SetActive(false);

        MainButton = GameObject.Find("MainButton");

        Chapters = new List<GameObject>();
        for (int i = 1; i < 5; i++)
            Chapters.Add(GameObject.Find("Chapter" + i.ToString()));

        OpenedChapterIndex = 0;
        ChosenChapterIndex = -1;
    }

    void Update()
    {
        for (int i = 0; i < OpenedChapterIndex + 1; i++)
                Chapters[i].GetComponent<Text>().color = Color.white;
            
        MainButton.GetComponent<Text>().color = new Color(0.3672f, 0.3672f, 0.3672f); 
        MainButton.GetComponent<Text>().text = "Выберите\r\nглаву";

        if (ChosenChapterIndex >=0)
        {
            Chapters[ChosenChapterIndex].GetComponent<Text>().color = new Color(0, 0.6875f, 0.2617f);

            MainButton.GetComponent<Text>().text = "Играть\r\nГлаву " + (ChosenChapterIndex+1).ToString();
            MainButton.GetComponent<Text>().color = new Color(0, 0.6875f, 0.2617f);
        }
    }

    public void ChooseChapter(GameObject chapter) 
    {
        var chapIndex = -1;
        switch (chapter.name)
        {
            case "Chapter1":
                chapIndex = 0;
                break;
            case "Chapter2":
                chapIndex = 1;
                break;
            case "Chapter3":
                chapIndex = 2;
                break;
            case "Chapter4":
                chapIndex = 3;
                break;
        }

        if (chapIndex > OpenedChapterIndex) 
            return;

        ChosenChapterIndex = chapIndex;

    }

    public void OpenChapter()
    {
        if (OpenedChapterIndex <= Chapters.Count-1)
            OpenedChapterIndex++;
    }

    public void PlayChapter()
    {
        if (ChosenChapterIndex < 0)
            return;
        PlayingChapterIndex = ChosenChapterIndex;

        controller.PanelInGame.SetActive(true);
        controller.PanelInGame.GetComponent<PanelInGameScript>().StartChapter(PlayingChapterIndex);

        controller.PanelMenu.SetActive(false);
    }
}
