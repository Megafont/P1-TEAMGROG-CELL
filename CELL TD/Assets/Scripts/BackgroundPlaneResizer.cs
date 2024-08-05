using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BackgroundPlaneResizer : MonoBehaviour
{
    [Tooltip("The distance the plane will be in front of the camera. This value should always be within the near and far clipping planes of the camera, or it will be culled from the view. The default near clipping plane is 0.3 units in front of camera, and the default far clippng plane is 1000 units in front of the camera.")]
    [Min(0.31f)] // This weird value is to make sure the plane must always be positioned at least a tad further in front of the camera than the default near clipping plane.
    [SerializeField]
    private float _DistanceFromCamera = 10f;

    [Tooltip("How much to increase the final scale of the plane. This is needed to prevent gaps on the edges of the screen as parts of the plane move in and out from the shader effect.")]
    [Min(0)]
    [SerializeField]
    private float _ScaleAdjustment = 0.01f;

    [Tooltip("This should be turned off if the camera is facing down the positive z-axis, or on if it is facing straight down like in the level scenes.")]
    [SerializeField]
    private bool _ScaleZInsteadOfY = false;



    private Camera _MainCam;
    private Vector3 _OriginalScale;

    private void Awake()
    {
        _MainCam = Camera.main;
        _OriginalScale = transform.localScale;

        MakePlaneFillScreenAtDistance(_DistanceFromCamera);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_MainCam != Camera.main)
            _MainCam = Camera.main;


        MakePlaneFillScreenAtDistance(_DistanceFromCamera);
    }

    /// <summary>
    /// This function positions and resizes the plane to fill the screen at the specified distance from the camera.
    /// </summary>
    private void MakePlaneFillScreenAtDistance(float distanceFromCamera)
    {
        transform.position = _MainCam.transform.position + _MainCam.transform.forward * distanceFromCamera;
        float heightScale = (2.0f * Mathf.Tan(0.5f * _MainCam.fieldOfView * Mathf.Deg2Rad) * _DistanceFromCamera) / 10.0f; // Default plane is 10 units on x and z axis.
        heightScale += _ScaleAdjustment;
        float widthScale = heightScale * _MainCam.aspect;

        if (!_ScaleZInsteadOfY)
            transform.localScale = new Vector3(widthScale, heightScale, 1);
        else
            transform.localScale = new Vector3(widthScale, 1, heightScale);
    }
}
