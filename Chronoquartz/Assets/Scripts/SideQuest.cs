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
    public bool isSideQuest = false;
    public GameObject buttonConfirm;


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
        if(!isSideQuest)
        {
        }
        else if(isSideQuest)
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
        if (!character.GetComponent<CharacterController>().Premium)
        {
            switch (name)
            {
                case "Daisy":
                    buttonConfirm.SetActive(false);
                    ReplaceUI(nameOfChar, "Mom said I shouldn't talk to strangers. Are you sure you own this place?", personImgs[0], "Thanks! Take this star cutter, I got it from my mom's kitchen! Maybe you can give me some more cookies sometime with it?");
                    break;
            }
        }
        else
        {
            switch (name)
            {
                case "Daisy":
                    buttonConfirm.SetActive(true);
                    ReplaceUI(name, "I love cookies! My mom won't let me have any... can you get me some? I'd like 5 circle chocolate chip cookies and 5 circle sugar cookies!", personImgs[0], "Thanks! Take this star cutter, I got it from my mom's kitchen! Maybe you can give me some more cookies sometime with it?");
                    break;
            }
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
        int cookiesupply1 = 0;
        int cookiesupply2 = 0;
        uiman.cookieSupply.TryGetValue("chocolatechipCircle", out cookiesupply1);
        uiman.cookieSupply.TryGetValue("sugarcookieCircle", out cookiesupply2);

        if(cookiesupply1 == 5 && cookiesupply2 == 5)
        {
            completed = true;
        }

        if (!completed)
        {
            description.GetComponent<TextMeshProUGUI>().text = notYet;

        } else
        {
            PopulateQuestUI(nameOfChar);
            uiman.cookieSupply["chocolatechipCircle"] -= 5;
            uiman.cookieSupply["sugarcookieCircle"] -= 5;
        }
    }
}
