using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem Instance;

    //Vars
    private int _moneyAmount = 500;

    //Getters
    public int MoneyAmount
    {
        get { return _moneyAmount; }
    }

    //References
    [SerializeField]
    private TMP_Text _moneyText;

    [SerializeField]
    private AudioSource _audioPlayer;
    [SerializeField]
    private AudioClip _notEnough;

    [SerializeField]
    private GameObject _lowMoneyText;

    void Start()
    {
        UpdateDisplay(_moneyAmount);
    }

    //Change the text to display how much money the play has
    private void UpdateDisplay(int amount)
    {
        _moneyText.text = amount.ToString();
    }

    //Money Logic
    public bool AddCurrency(int amount)
    {
        //If value is positive, add
        if (amount >= 0)
        {
            StartCoroutine(AnimateText(Color.green));
            _moneyAmount += amount;
            return true;
        }
        //If negative, subtract but make sure that money can be subtracted
        else
        {
            SubtractCurrency(Mathf.Abs(amount));
            return false;
        }
    }

    public bool SubtractCurrency(int amount)
    {
        if (_moneyAmount - amount >= 0)
        {
            StartCoroutine(AnimateText(Color.red));
            _moneyAmount -= amount;
            return true;
        }
        StartCoroutine(ShowText());
        _audioPlayer.clip = _notEnough;
        _audioPlayer.Play();

        StartCoroutine(AnimateText(Color.red));
        return false;
        
    }

    [ContextMenu("AddCurrency")]
    public void Add()
    {
        AddCurrency(10);
    }

    [ContextMenu("SubCurrency")]
    public void Sub()
    {
        AddCurrency(-10);
    }

    //This visually changes the text
    private IEnumerator AnimateText(Color referenceColor)
    {
        Color intialColor = _moneyText.color;
        int initialAmount = _moneyAmount;
        float elapsedTime = 0.0f;
        float duration = 0.5f; //Change this for longer animation

        //This calculates and lerps the animation
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            _moneyText.color = Color.Lerp(intialColor, referenceColor, t);
            UpdateDisplay((int)Mathf.Lerp((float)initialAmount,(float)_moneyAmount,t));

            yield return null;
        }

        _moneyText.color = referenceColor;

        //This calls the same function, but returns the text to white. This may need to be changed if the text color is no longer white.
        StartCoroutine(AnimateText(Color.white));
    }

    private IEnumerator ShowText()
    {
        _lowMoneyText.SetActive(true);
        yield return new WaitForSeconds(2);
        _lowMoneyText.SetActive(false);
    }
}
