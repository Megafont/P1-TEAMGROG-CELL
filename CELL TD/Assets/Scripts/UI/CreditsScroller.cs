using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class CreditsScroller : MonoBehaviour
{
    [Tooltip("The top level credits object that contains all of the credits text.")]
    [SerializeField]
    private RectTransform _TopLevelCreditsObjectRectTransform;

    [Tooltip("This is the starting y-position of the credits. It should have them entirely below the bottom of the screen.")]
    [SerializeField]
    private int _StartingYPosition = -600;

    [Tooltip("How fast (in units per second) that the credits should scroll.")]
    [SerializeField]
    private float _ScrollSpeed = 5f;

    [Tooltip("Whether or not the credits will scroll again after they've completed. If set to true, the delay is specified by the DelayBeforeStartSetting.")]
    [SerializeField]
    private bool _RestartAfterDone = true;

    [Tooltip("How long (in seconds) that the credits screen will wait before it scrolls them again.")]
    [SerializeField]
    private float _DelayBeforeRestart = 5.0f;



    private bool _IsScrolling;

    private float _RestartTimer;



    void Awake()
    {
        //InputSystem.onAnyButtonPress.CallOnce(OnAnyButtonPress);        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCreditsYPosition(_StartingYPosition);
        _IsScrolling = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (_IsScrolling)
        {
            // Move the credits up a little.
            SetCreditsYPosition(_TopLevelCreditsObjectRectTransform.anchoredPosition.y + _ScrollSpeed * Time.deltaTime);

            // Check if they've gone off the top of the screen.
            if (_TopLevelCreditsObjectRectTransform.anchoredPosition.y > _TopLevelCreditsObjectRectTransform.sizeDelta.y - _StartingYPosition)
            {
                _IsScrolling = false;

                _RestartTimer = 0f;
            }
        }
        else if (_RestartAfterDone)
        {
            // Increment the restart timer and see if the restart delay has elapsed yet.
            _RestartTimer += Time.deltaTime;
            if (_RestartTimer >= _DelayBeforeRestart)
            {
                // The restart delay has elapsed, so move the credits object back to the starting y-position
                // and then set the flag so it starts scrolling again.
                SetCreditsYPosition(_StartingYPosition);
                _IsScrolling = true;
            }
        }
    }

    private void OnDestroy()
    {
        
    }

    private void SetCreditsYPosition(float yPosition)
    {
        Vector2 position = _TopLevelCreditsObjectRectTransform.anchoredPosition;
        position.y = yPosition;
        _TopLevelCreditsObjectRectTransform.anchoredPosition = position;
    }

    private void OnAnyButtonPress(InputControl control)
    {
        GameManager.Instance.SetGameState(typeof(GameState_MainMenu));
    }
}
