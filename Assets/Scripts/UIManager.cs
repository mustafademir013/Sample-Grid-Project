using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { private set; get; }

    [SerializeField] InputField gridCountInput;

    [SerializeField] Text matchText;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ClickRebuid()
    {
        int count = int.Parse(gridCountInput.text);
        if (count > 0)
            Manager.Instance.SpawnGrids(count);
    }

    public void SetMatchText(int matchCount)
    {
        matchText.text = "Match Count:" + matchCount.ToString();
    }
}
