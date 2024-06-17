using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingSystem : MonoBehaviour
{
    public enum FishingState { READYTOFISH, ONFISHING }
    public enum Fishes { FISH1, FISH2, FISH3, FISH4, FISH5, FISH6, FISH7, FISH8, FISH9, FISH10, FISH11, FISH12, FISH13, FISH14, FISH15, FISH16, FISH17, FISH18, FISH19, FISH20 }

    public FishingState fishstate;
    public TextMeshProUGUI expText, levelText, dialogueText;
    public Button fishingbutton;
    int fishinglevel = 9;
    int fishingexp = 0;
    const int expPerLevel = 80;

    void Start()
    {
        dialogueText.text = "Burası balık tutmaya uygun görünüyor...";
        fishstate = FishingState.READYTOFISH;
        UpdateUI();
    }

    void UpdateUI()
    {
        if(fishinglevel >= 10){
            levelText.text = "MAX LEVEL";
            expText.text = "MAX EXP";
        }
        else{
        expText.text = "EXP: " + fishingexp + "/" + expPerLevel;
        levelText.text = "Fishing Lvl: " + fishinglevel;}
    }

    public void StartFishing()
    {
        if (fishstate == FishingState.READYTOFISH)
        {
            StartCoroutine(OnFishingButton());
        }
    }

    private IEnumerator OnFishingButton()
    {
        fishstate = FishingState.ONFISHING;
         fishingbutton.interactable = false;
        dialogueText.text = "BALIK TUTULUYOR...";
        yield return new WaitForSeconds(2);

        int fishIndex = Random.Range(0, 10 + fishinglevel);
        Fishes caughtFish = (Fishes)fishIndex;
        dialogueText.text = caughtFish + " TUTTUNUZ";

        fishingexp += 30;
        

        if (fishingexp >= expPerLevel && fishingexp!= 10)
        {
            fishinglevel++;
            fishingexp = fishingexp % expPerLevel;
        }

        UpdateUI();
        fishingbutton.interactable = true;
        fishstate = FishingState.READYTOFISH;
    }
}
