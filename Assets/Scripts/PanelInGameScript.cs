using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelInGameScript : MonoBehaviour
{
    public int ChapterIndex { get; set; }
    public GameObject MainText { get; set; }
    public GameObject TimeBar { get; set; }
    public GameObject MainTextBorder { get; set; }
    public GameObject[] Answers { get; set; }

    private Controller controller;

    public int Points;
    public int PointsMax;

    public bool ChapterRuns;

    public float timer { get; set; }
    public bool timerEnable { get; set; }

    public Page CurrentPage { get; set; }
    public Page FirstPage { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        MainText = GameObject.Find("MainText");
        Answers = new[] {
            GameObject.Find("Answer1"),
            GameObject.Find("Answer2"),
            GameObject.Find("Answer3")
        };
        TimeBar = GameObject.Find("TimeBar");
        MainTextBorder = GameObject.Find("MainTextBorder");

        controller = GameObject.Find("GameController").GetComponent<Controller>();
    }

    public void StartChapter(int chapIndex)
    {



        if (controller == null)
            controller = GameObject.Find("GameController").GetComponent<Controller>();

        ChapterRuns = true;
        ChapterIndex = chapIndex;

        FirstPage = controller.ParsePagesFromXML(ChapterIndex, out PointsMax);        
        CurrentPage = FirstPage;

        for (int i = 0; i < 3; i++)
            Answers[i].GetComponent<Transform>().parent.gameObject.SetActive(CurrentPage.Answers[i].Length != 0);

        if (CurrentPage.TimeToAnswer > 0)
        {
            timer = CurrentPage.TimeToAnswer;
            timerEnable = true;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (!ChapterRuns)
            return;

        if (timerEnable && timer > 0.0)
            timer -= Time.deltaTime;
        if (timerEnable && timer < 0.0)
            TimerAlarm();
                
        MainText.GetComponent<Text>().text = CurrentPage.Text;
        for (int i = 0; i < Answers.Length; i++)
            Answers[i].GetComponent<Text>().text = CurrentPage.Answers[i];

        TimeBar.GetComponent<Transform>().localScale = new Vector3(timer / CurrentPage.TimeToAnswer, 1, 1);


    }

    public void GotAnswer(GameObject answer)
    {
        switch (answer.name)
        {
            case "Answer1":
                ChangePage(0);
                break;
            case "Answer2":
                ChangePage(1);
                break;
            case "Answer3":
                ChangePage(2);
                break;
        }
    }

    public void TimerAlarm()
    {
        ChangePage(3);
    }

    private void ChangePage(int nextPageIndex)
    {
        if (CurrentPage.PagesAfterAnswers[nextPageIndex] == null)
        {
            controller.PanelEndGame.SetActive(true);
            controller.PanelEndGame.GetComponent<PanelEndGameScript>().ChapterEnded(ChapterIndex, Points, PointsMax);

            ChapterRuns = false;
            timerEnable = false;
            timer = 0f;

            controller.PanelInGame.SetActive(false);
        }
        else
        {
            CurrentPage = CurrentPage.PagesAfterAnswers[nextPageIndex];

            for (int i = 0; i < 3; i++)
                Answers[i].GetComponent<Transform>().parent.gameObject.SetActive(CurrentPage.Answers[i].Length != 0);

            Points += CurrentPage.Points;

            if (CurrentPage.TimeToAnswer > 0)
            {
                timer = CurrentPage.TimeToAnswer;
                timerEnable = true;
            }
            else
            {
                timerEnable = false;
                timer = 0f;
            }
        }
    }

}


