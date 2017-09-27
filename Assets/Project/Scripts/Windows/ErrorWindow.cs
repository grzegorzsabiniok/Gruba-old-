using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ErrorWindow : Window {

    public static ErrorWindow main;
    public Text log;
    private void Awake()
    {
        main = this;
    }
    public static void NewError(string _error)
    {
        main.log.text += _error + "\n";
    }
}
