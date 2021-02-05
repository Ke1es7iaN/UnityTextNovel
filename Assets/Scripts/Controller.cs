using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject PanelMenu;
    public GameObject PanelInGame;
    public GameObject PanelEndGame;
    void Start()
    {
        PanelMenu = GameObject.Find("Panel_Menu");
        PanelInGame = GameObject.Find("Panel_InGame");
        PanelEndGame = GameObject.Find("Panel_EndGame");
    }

    public Page ParsePagesFromXML(int chapIndex, out int pointsMax)
    {
        var fileName = "";
        switch (chapIndex)
        {
            case 0:
                fileName = "chapter1_pages";
                break;
            case 1:
                fileName = "chapter2_pages";
                break;
            case 2:
                fileName = "chapter3_pages";
                break;
            case 3:
                fileName = "chapter4_pages";
                break;
        }
        TextAsset textAsset = (TextAsset)Resources.Load(fileName);

        var parsedPages = new Dictionary<string, Page>();


        var xDoc = new XmlDocument();
        xDoc.LoadXml(textAsset.text);
        var xRoot = xDoc.DocumentElement;
        
        pointsMax = int.Parse(xRoot.GetAttribute("pointsMax"));

        foreach (XmlNode xNode in xRoot.ChildNodes)
        {
            if (xNode.NodeType == XmlNodeType.Comment)
                continue;
            var id = xNode.Attributes.GetNamedItem("id").Value;
            var dict = xNode.ChildNodes.Cast<XmlNode>().ToDictionary(n => n.Name, n => n.InnerText);
            var answersKeys = dict.Keys.Where(k => k.StartsWith("answ"));
            var onePage = new Page(
                dict["text"],
                int.Parse(dict["time"]),
                int.Parse(dict["points"]),
                answersKeys.Select(k => dict[k]).ToArray(),
                new Page[answersKeys.Count()]);
            parsedPages.Add(id, onePage);
        }

        foreach (var kv in parsedPages)
        {
            if (kv.Key.Length < 2) continue;

            var parentId = kv.Key.Substring(0, kv.Key.Length - 1);
            var answN = int.Parse(kv.Key.Substring(kv.Key.Length - 1));
            parsedPages[parentId].PagesAfterAnswers[answN - 1] = kv.Value;
        }

        return parsedPages["1"];
    }

}

public class Page
{
    public string Text { get; set; }
    public int TimeToAnswer { get; set; }
    public int Points { get; set; }
    public string[] Answers { get; set; }
    public Page[] PagesAfterAnswers { get; set; }

    public Page()
    { }

    public Page(string text, int time, int points, string[] answers, Page[] pagesAnsw)
    {
        Text = text;
        TimeToAnswer = time;
        Points = points;
        Answers = answers;
        PagesAfterAnswers = pagesAnsw;
    }


}
