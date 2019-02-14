using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class MorseIdentificationScript : MonoBehaviour
{
    public KMAudio Audio;

    public KMSelectable LeftButton;
    public KMSelectable RightButton;
    public KMSelectable SubmitButton;

    public TextMesh displayedMorse;
    public TextMesh displayedCharacter;

    private int MorseDisplayNumber = 0;

    private int CharacterDisplayNumber = 0;

    public string[] characterlist;

    public string[] characterdisplaylist;

    private bool needyactive;

    void Awake()
    {
        GetComponent<KMNeedyModule>().OnNeedyActivation += OnNeedyActivation;
        GetComponent<KMNeedyModule>().OnNeedyDeactivation += OnNeedyDeactivation;
        GetComponent<KMNeedyModule>().OnTimerExpired += OnTimerExpired;
        LeftButton.OnInteract += delegate () { LeftInteract(); return false; };
        RightButton.OnInteract += delegate () { RightInteract(); return false; };
        SubmitButton.OnInteract += delegate () { SubmitInteract(); return false; };
        displayedMorse.text = "";
    }

    void LeftInteract()
    {
        if (needyactive)
        {
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            LeftButton.AddInteractionPunch();
            if (CharacterDisplayNumber == 0)
            {
                CharacterDisplayNumber = CharacterDisplayNumber + 35;
            }
            else
            {
                CharacterDisplayNumber = CharacterDisplayNumber - 1;
            }
            displayedCharacter.text = characterdisplaylist[CharacterDisplayNumber];
        }
    }

    void RightInteract()
    {
        if (needyactive)
        {
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            RightButton.AddInteractionPunch();
            if (CharacterDisplayNumber == 35)
            {
                CharacterDisplayNumber = CharacterDisplayNumber - 35;
            }
            else
            {
                CharacterDisplayNumber = CharacterDisplayNumber + 1;
            }
            displayedCharacter.text = characterdisplaylist[CharacterDisplayNumber];
        }
    }

    void SubmitInteract()
    {       
        if (needyactive)
        {
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            SubmitButton.AddInteractionPunch();
            if (MorseDisplayNumber == CharacterDisplayNumber)
            {
                Audio.PlaySoundAtTransform("solvesound", transform);
                Solve();
            }
            else
            {
                GetComponent<KMNeedyModule>().OnStrike();
                Solve();
            }
        }
    }

    protected bool Solve()
    {
        GetComponent<KMNeedyModule>().OnPass();
        displayedMorse.text = "";
        needyactive = false;
        return false;
    }

    protected void OnNeedyActivation()
    {
        ChooseMorseCharacter();
        needyactive = true;
    }

    void ChooseMorseCharacter()
    {
        MorseDisplayNumber = UnityEngine.Random.Range(0, 36);
        displayedMorse.text = characterlist[MorseDisplayNumber];
    }

    protected void OnNeedyDeactivation()
    {

    }

    protected void OnTimerExpired()
    {
        GetComponent<KMNeedyModule>().OnStrike();
    }
}