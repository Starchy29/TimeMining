using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SideQuest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject character;
    private CharacterController charController;
    public UIManager uiman;
    public GameObject interactText;
    public GameObject taskMenu;
    public GameObject completeButton;

    public int moneyReward;

    public Sprite[] personImgs;

    public GameObject name, description, personImg;
    private string nameOfChar;

    public bool completed = false;


    // key is name, output is list of ingredients
    private Dictionary<string, Dictionary<string, int>> cookieRequest = new Dictionary<string, Dictionary<string, int>>();

    void Start()
    {
        charController = character.GetComponent<CharacterController>();
        nameOfChar = this.gameObject.name;
        PopulateQuests();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiman.isPremium)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            interactText.SetActive(false);
            taskMenu.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
            if(Input.GetKeyDown(KeyCode.E))
            {
                taskMenu.SetActive(true);
                PopulateQuestUI(nameOfChar);
            }
    }

    void PopulateQuests()
    {
        Dictionary<string, int> daisyRequest = new Dictionary<string, int>();
        daisyRequest.Add("chocolatechipcircle", 5);
        daisyRequest.Add("sugarcookiecircle", 5);
        cookieRequest.Add("Daisy", daisyRequest);
    }

    void PopulateQuestUI(string name)
    {
        switch(name)
        {
            case "Daisy":
                ReplaceUI(name, "I love cookies! My mom won't let me have any... can you get me some? I'd like 5 circle chocolate chip cookies and 5 circle sugar cookies!", personImgs[0], "Thanks! Take this star cutter, I got it from my mom's kitchen! Maybe you can give me some more cookies sometime with it?");
                break;
        }
    }

    void ReplaceUI(string nameTxt, string descriptionTxt, Sprite imgItself, string winText) 
    {
        name.GetComponent<TextMeshProUGUI>().text = nameTxt;
        personImg.GetComponent<Image>().sprite = imgItself;

        if(!completed)
        {
            description.GetComponent<TextMeshProUGUI>().text = descriptionTxt;
        } else
        {
            completeButton.SetActive(false);
            description.GetComponent<TextMeshProUGUI>().text = winText;
            personImg.GetComponent<Image>().sprite = personImgs[1];
        }

    }

    public void CheckIfMeetingRequirements(string notYet)
    {
        if(!completed)
        {
            description.GetComponent<TextMeshProUGUI>().text = notYet;

        } else
        {
            PopulateQuestUI(nameOfChar);
        }
    }
}
