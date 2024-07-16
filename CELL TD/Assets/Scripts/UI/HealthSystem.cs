using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private int _MaxHealth = 100; // This is just the default value; change it in the inspector instead.


    //Vars    
    private int _CurrentHealth;
    private bool _warning = false;

    //Getters
    public int HealthAmount
    {
        get { return _CurrentHealth; }
    }

    //References
    [SerializeField]
    private TMP_Text _healthText;

    [SerializeField]
    private AudioSource _audioPlayer;

    [SerializeField]
    private AudioClip _damageClip;
    [SerializeField]
    private AudioClip _fatalClip;
    [SerializeField]
    private AudioClip _warningClip;


    void Awake()
    {
        _CurrentHealth = _MaxHealth;
    }

    void Start()
    {
        UpdateDisplay(_CurrentHealth);
    }

    //Change the text to display how much health the player has
    private void UpdateDisplay(int amount)
    {
        _healthText.text = amount.ToString();
    }

    //Money Logic
    public bool AddHealth(int amount)
    {
        //No more logic if health is already 0
        if (_CurrentHealth <= 0)
        {
            return false;
        }

        int oldHealthValue = _CurrentHealth;


        // Change the health by the specified amount.
        _CurrentHealth = Mathf.Clamp(_CurrentHealth + amount, 0, _MaxHealth);

        if (_CurrentHealth <= 25 && !_warning)
        {
            _audioPlayer.clip = _warningClip;
            _audioPlayer.Play();
            _warning = true;
        }

        //If value is positive, update the screen.
        if (_CurrentHealth > 0)
        {
            StartCoroutine(AnimateText(Color.blue, oldHealthValue, _CurrentHealth));
            return true;
        }
        else // The player's health has reached 0.
        {
            _audioPlayer.clip = _fatalClip;
            _audioPlayer.Play();

            StartCoroutine(AnimateText(Color.red, oldHealthValue, _CurrentHealth));

            GameOverScreen.Show();

            return false;
        }
    }

    public void TakeDamage(int amount)
    {
        amount = Mathf.Abs(amount);
        _audioPlayer.clip = _damageClip;
        _audioPlayer.Play();

        AddHealth(-amount);
    }

    //This visually changes the text
    private IEnumerator AnimateText(Color referenceColor, int initialAmount, int finalAmount)
    {
        Color intialColor = _healthText.color;
        float elapsedTime = 0.0f;
        float duration = 0.5f; //Change this for longer animation

        //This calculates and lerps the animation
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            _healthText.color = Color.Lerp(intialColor, referenceColor, t);
            UpdateDisplay((int)Mathf.Lerp((float)initialAmount,(float)finalAmount,t));

            yield return null;
        }

        _healthText.color = referenceColor;

        //This calls the same function, but returns the text to white. This may need to be changed if the text color is no longer white.
        StartCoroutine(AnimateText(Color.white, _CurrentHealth, _CurrentHealth));
    }
}
