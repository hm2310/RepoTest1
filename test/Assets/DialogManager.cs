using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public TextAsset dialogDataFile;
    public TMP_Text nameText;
    public TMP_Text dialogText;

    public int dialogIndex;
    public string[] dialogRows;

    public Button nextButton;
    public GameObject optionButton;
    public Transform buttonGroup;

    // Start is called before the first frame update
    void Start()
    {
        ReadText(dialogDataFile);
        ShowDialogRow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string _name, string _text)
    {
        nameText.text = _name;
        dialogText.text = _text;

    }
    public void ReadText(TextAsset _textAsset){
        dialogRows = _textAsset.text.Split('\n');


    }

    public void ShowDialogRow(){
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if((cells[0])== "#" && int.Parse(cells[1]) == dialogIndex)
            {
                UpdateText(cells[2],cells[3]);
                dialogIndex = int.Parse(cells[4]);
                nextButton.gameObject.SetActive(true);
                break;
            }
            else if((cells[0])== "&" && int.Parse(cells[1]) == dialogIndex)
            {
                nextButton.gameObject.SetActive(false);
                GenerateOption(i);
                break;
            }
            else if((cells[0])== "END" && int.Parse(cells[1]) == dialogIndex)
            {   
                Debug.Log("THE END");
            }
        }
    }

    public void OnClickNext()
    {
        ShowDialogRow();
    }

    public void GenerateOption(int _index)
    {   
        string[] cells = dialogRows[_index].Split(',');
        if(cells[0] == "&")
        {
            GameObject button = Instantiate(optionButton, buttonGroup);
            //go to event
            button.GetComponentInChildren<TMP_Text>().text = cells[3];
            button.GetComponent<Button>().onClick.AddListener(delegate { OnOptionClick(int.Parse(cells[4]));});
            GenerateOption(_index + 1);
        }

    }

    public void OnOptionClick(int _idx)
    {
        dialogIndex = _idx;
        ShowDialogRow();
        for(int i = 0; i < buttonGroup.childCount; i++)
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
        }
    }
}   

