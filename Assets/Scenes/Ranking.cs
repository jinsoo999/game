using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Net;

public class Ranking : MonoBehaviour {

    public Text[] rank = new Text[10];

    void Start () {
        getscore();
	}

    public void getscore()
    {
        WebRequest totalscore = WebRequest.Create("http://220.69.209.170/jjs/select.php");
        WebResponse response = totalscore.GetResponse();
        StreamReader stream = new StreamReader(response.GetResponseStream());
        int j = 0;
        string firstStr = stream.ReadToEnd();
        Debug.Log(firstStr);
        string[] split = firstStr.Split(new char[] { '/' });
        for (int i = 0; i < 5; i++)
        {
            rank[i].text = i + 1 + "위 :      " + split[j];
            rank[i + 5].text = string.Format("{0:#,###}", int.Parse(split[j + 1])) + "점 " + "\n";
            j += 2;
        }
    }
}

