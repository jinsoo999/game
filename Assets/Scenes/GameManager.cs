using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Net;


public class GameManager : MonoBehaviour {
    
    public InputField _InputField;
    
    public void insert()
    {
        string address = "http://220.69.209.170/jjs/insert.php";
        WWWForm Form = new WWWForm();
        Form.AddField("name", _InputField.text);
        Form.AddField("score", Score_Manager.score);

        WWW wwwURL = new WWW(address, Form);
    }
}
