using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : PersistentSingleton<OptionUI>
{
    public GameObject thisUI;
    public GameObject optionButton;

    private void Start()
    {
        optionButton.SetActive(true);
    }

    public void OpenUI()
    {
        thisUI.gameObject.SetActive(true);
    }
    public void CloseUI()
    {
        thisUI.gameObject.SetActive(false);
    }
}
