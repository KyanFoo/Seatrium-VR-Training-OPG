using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR.InteractionSystem;

public class RotaryKnobBehaviour : MonoBehaviour
{
    [Header("Secondary Script")]
    //All variables' data are retreive from this script//
    public RotaryCircularDrive rotarycirculardrive;
    private float outAngleValue;

    [Header("Primary Script")]
    //Gameobject's Z Roation//
    private float currentZRotation;
    private float previousZRotation;

    //There can only be interval points from 1 to 10//
    //***Avoid using "7" as it does not give a whole number//
    [Range(1, 10)]
    public int intervalPoint;
    //An Array that would be filled with interval points//
    public int[] myArray;
    private int num;

    //public Vector3 currentAngle;
    public int targetRotation;

    //Variables to check how many intervals are there//
    public int numberOfVariables;
    //Variables to check which interval the knob is at//
    public int numberPlace = 0;

    public float currentRotationX;
    public float currentRotationY;

    public float lowerValue;
    public float upperValue;

    public bool LoadSharingSceneStartSynchro = false;

    // Start is called before the first frame update
    void Start()
    {
        //Automatic notify the "Circular Drive" Script to enable limited.//
        rotarycirculardrive.limited = true;

        //Creating the interval points of the knobs//
        //If there are "4" interval points, the angles of the points are 90, 180, 270, 360//
        myArray = new int[intervalPoint + 1];
        num = 360 / intervalPoint;
        //Within the array, the first value would be "0"//
        myArray[0] = 0;
        for (int i = 0; i < intervalPoint; i++)
        {
            //Create an Array to check how many points there are and what is the rotation angle//
            myArray[i + 1] = (num * (i + 1));
        }

        //Checking on how many variables are there in the gameobject//
        numberOfVariables = myArray.Length;
        numberPlace = 0;

        //Set your Maximum & Minium Angle//
        rotarycirculardrive.minAngle = -2.0f;

        //Maximum Angles can varied based on the gameobject's interval points//
        rotarycirculardrive.maxAngle = myArray[numberOfVariables - 2] + 2;

    }

    // Update is called once per frame
    void Update()
    {
        //Retreive data of "outAngle" from "RotatyCircularDrive"//
        outAngleValue = rotarycirculardrive.outAngle;

        //Retreive data of the gameobject Z Rotation//
        currentZRotation = transform.rotation.eulerAngles.z;

        foreach (int value in myArray)
        {
            if (value <= currentZRotation)
            {
                lowerValue = value;
            }
            if (value >= currentZRotation)
            {
                upperValue = value;
                break;
            }
        }
        float lowerDifference = Mathf.Abs(currentZRotation - lowerValue);
        float upperDifference = Mathf.Abs(upperValue - currentZRotation);

        if (lowerDifference < upperDifference)
        {
            //Debug.Log("The closest value is: " + lowerValue);
            numberPlace = (int)lowerValue;
            targetRotation = numberPlace;
            currentRotationX = transform.rotation.eulerAngles.x;
            currentRotationY = transform.rotation.eulerAngles.y;
            SnapRotate();
        }
        else
        {
            //Debug.Log("The closest value is: " + upperValue);
            numberPlace = (int)upperValue;
            targetRotation = numberPlace;
            currentRotationX = transform.rotation.eulerAngles.x;
            currentRotationY = transform.rotation.eulerAngles.y;
            SnapRotate();
        }

        for (int i = 0; i < myArray.Length; i++)
        {
            if (myArray[i] == numberPlace)
            {
                numberPlace = i;
            }
        }
    }
    void SnapRotate()
    {
        //Snap the GameObject to the rotation position of the "targetRotaion"//
        transform.rotation = Quaternion.Euler(currentRotationX, currentRotationY, targetRotation);
    }
    public void LoadSharingRotate()
    {
        Debug.Log("Hello");
        transform.Rotate(0, 0, 180);
        LoadSharingSceneStartSynchro = true;
    }
}
