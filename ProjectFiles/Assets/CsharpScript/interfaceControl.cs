﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class interfaceControl : MonoBehaviour
{

    public GameObject panel;
    public GameObject mappanel;
    public GameObject itempanel;
    public GameObject toolpanel;
    public GameObject borders;
    public GameObject reminderText;

    public Animator vkit;
    public Animator canvasInitilization;

    public Transform parent;
    public Transform child;
    public Transform character;
    public Transform mapPanel;
    public Transform toolPanel;
    public Transform vkitHolder;
    public Transform holderPosIndicator;

    private bool inputBlock = false;

    public Animation panelFade;
    public Animation itemshown;

    private Vector3 pos;
    private Vector3 vkitLocalPos;
    public Vector3 vkitMountPos = new Vector3(0, 0f, -0.8f);
    private Vector3 mapLocalPos;
    private Vector3 originalVkitPos;
    private Quaternion rot;
    private Quaternion vkitLocalRot;

    //boolean values
    private bool clickedMap = false;
    private static bool playerhoverMap = false;

    private bool clickedTools = false;
    private static bool playerhoverTools = false;

    private bool clickedInventory = false;
    private bool hitE;
    private bool playerClickedTab = false;
    // Use this for initialization
    void Start()
    {

        pos = GameObject.Find("sysInterface").GetComponent<Transform>().localPosition;
        rot = GameObject.Find("sysInterface").GetComponent<Transform>().localRotation;

        vkitLocalPos = GameObject.Find("vkit").GetComponent<Transform>().localPosition;
        vkitLocalRot = GameObject.Find("vkit").GetComponent<Transform>().localRotation;
        originalVkitPos = vkitHolder.localPosition;
        mapLocalPos = mapPanel.localPosition;
    }


    //tools button
    public void mouseHoverOnTools()
    {
        if (clickedInventory == false && clickedMap == false)
        {
            playerhoverTools = true;
            if (!clickedTools)
            {
                panelFade.Play();
            }
        }
    }
    public void mouseExitOnTools()
    {
        if (clickedTools == false && clickedInventory == false && clickedMap == false)
        {
            playerhoverTools = false;
            panelFade.Play("panelFadein");
        }

    }
    public void mouseClickOnTools()
    {
        playerhoverTools = true;
        if (clickedInventory)
        {
            clickedInventory = false;
            mouseExitOnInventory();
        }
        if (clickedMap)
        {
            clickedMap = false;
            playerhoverMap = false;
        }
        clickedTools = !clickedTools;


    }

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    //map button
    public void mouseHoverOnMap()
    {
        if (clickedInventory == false && !clickedTools)
        {
            playerhoverMap = true;
            if (clickedMap == false)
            {
                panelFade.Play();
            }
        }
    }

    public void mouseExitOnMap()
    {
        if (clickedMap == false && clickedInventory == false && !clickedTools)
        {
            playerhoverMap = false;
            panelFade.Play("panelFadein");
        }
    }
    public void mouseClickOnMap()
    {
        playerhoverMap = true;
        if (clickedInventory)
        {
            clickedInventory = false;
            mouseExitOnInventory();
        }
        if (clickedTools)
        {
            clickedTools = false;
            playerhoverTools = false;
        }
        clickedMap = !clickedMap;

    }

    //inventory button
    public void mouseHoverOnInventory()
    {
        if (!itempanel.activeInHierarchy && clickedMap == false && !clickedTools)
        {
            itempanel.SetActive(true);
            itemshown["cubeMove"].layer = 5;
            itemshown["cubeMove"].speed = 2.0f;
            itemshown["cubeRot"].speed = 0.5f;
            itemshown.Blend("cubeRot");
            itemshown.Play();
            panelFade.Play();
        }
    }
    public void mouseExitOnInventory()
    {

        if (clickedInventory == false && clickedMap == false && !clickedTools)
        {
            itemshown.Play("cubeBack");

            panelFade.Play("panelFadein");

            if (playerhoverMap || playerhoverTools)
            {
                panelFade.Play();
            }

            if (itempanel.activeInHierarchy)
            {
                StartCoroutine(DelayCloseUp(0.2f, itempanel));
            }
        }
    }
    public void mouseClickOnInventory()
    {
        if (clickedMap)
        {
            clickedMap = false;
            mouseExitOnMap();
        }
        if (clickedTools)
        {
            clickedTools = false;
            mouseExitOnTools();
        }

        clickedInventory = !clickedInventory;
        if (clickedInventory)
        {
            mouseHoverOnInventory();
        }

        //itempanel.SetActive(true);
    }
    public void mouseClickOnItem(int num)
    {
        Debug.Log("click on item " + num);
    }



    IEnumerator DelayCloseUp(float secs, GameObject theObject)
    {
        yield return new WaitForSeconds(secs);
        theObject.SetActive(false);
    }

    IEnumerator DelayforCanvasStart(float secs)
    {
        yield return new WaitForSeconds(secs);
        borders.SetActive(true);
        canvasInitilization.SetTrigger("startInitilaze");
    }
    IEnumerator DelayforCanvasEnd()
    {
        canvasInitilization.SetTrigger("startInitilaze");
        StartCoroutine(DelayCloseUp(3f, borders));
        yield return new WaitForSeconds(2f);
        vkit.SetBool("playerHitTab", playerClickedTab);
    }
    IEnumerator DelayforReminder()
    {
        reminderText.GetComponent<Text>().text = "The projection kit cannot be released here.\n Try a broader space.";
        reminderText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(2f);
        reminderText.GetComponent<Text>().enabled = false;
    }

    //all the functions above have np

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(holderPosIndicator.position, holderPosIndicator.TransformDirection(Vector3.forward), out hit, 1f))
        {
            vkitHolder.localPosition = Vector3.Lerp(vkitHolder.localPosition, new Vector3(originalVkitPos.x, originalVkitPos.y, originalVkitPos.z-hit.distance), 0.1f);
        }
        else if (Physics.Raycast(holderPosIndicator.position, holderPosIndicator.TransformDirection(Vector3.back), out hit, 1f))
        {
            vkitHolder.localPosition = Vector3.Lerp(vkitHolder.localPosition, new Vector3(originalVkitPos.x, originalVkitPos.y, originalVkitPos.z + hit.distance), 0.1f);
        }
        else if (Physics.Raycast(holderPosIndicator.position, holderPosIndicator.TransformDirection(Vector3.left), out hit, 1f))
        {
            vkitHolder.localPosition = Vector3.Lerp(vkitHolder.localPosition, new Vector3(originalVkitPos.x + hit.distance, originalVkitPos.y, originalVkitPos.z), 0.1f);
        }
        else if (Physics.Raycast(holderPosIndicator.position, holderPosIndicator.TransformDirection(Vector3.right), out hit, 1f))
        {
            vkitHolder.localPosition = Vector3.Lerp(vkitHolder.localPosition, new Vector3(originalVkitPos.x - hit.distance, originalVkitPos.y, originalVkitPos.z), 0.1f);
        }
        else if (vkit.GetCurrentAnimatorStateInfo(0).IsName("default"))
        {
            vkitHolder.localPosition = Vector3.Lerp(vkitHolder.localPosition, originalVkitPos, 0.1f);
        }
    }

    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Tab) || OVRInput.GetDown(OVRInput.Button.SecondaryShoulder)) && !inputBlock)
        {
            //open Vkit
            //collision check if there is anything in front of the player that can block the robot

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), 5) && !playerClickedTab)
            {
                //Debug.Log("Hit something!");
                StartCoroutine(DelayforReminder());
            }
            else {
                playerClickedTab = !playerClickedTab;

                bool isActive = borders.activeInHierarchy;
                if (isActive == false)
                {
                    vkit.transform.localRotation = vkitLocalRot;
                    vkit.transform.localPosition = vkitLocalPos;
                    vkit.SetBool("playerHitTab", playerClickedTab);
                    inputBlock = true;
                    StartCoroutine(DelayforCanvasStart(3f));
                    //make player not moveable when opening interface
                    GameObject.Find("FPSController").GetComponent<OVRPlayerController>().Acceleration = 0f;
                }
                else {
                    StartCoroutine(DelayforCanvasEnd());
                    GameObject.Find("FPSController").GetComponent<OVRPlayerController>().Acceleration = 3.5f;
                }
            }
        }
        //active panel when canvas initilization animation finished!!

        if (vkit.GetCurrentAnimatorStateInfo(0).IsName("hanging") && canvasInitilization.GetCurrentAnimatorStateInfo(0).IsName("1122"))
        {
            //activate panel and set the parent to FPScontroler so that the y axis is freezed
            inputBlock = false;
            panel.SetActive(true);
            //transform.SetParent(parent);
            if (Input.GetKeyDown(KeyCode.E) || OVRInput.GetDown(OVRInput.Button.Four))
            {
                hitE = !hitE;
            }

            if (hitE)
            {
                //transform.SetParent(environment);
            }
            else {
                transform.SetParent(character);
                transform.localPosition = Vector3.Lerp(transform.localPosition, vkitMountPos, 0.1f);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 0.1f);
            }

        }
        else {
            //deactivate panel and set the parent back to camera
            panel.SetActive(false);
            transform.SetParent(child);


            if (vkit.GetCurrentAnimatorStateInfo(0).IsName("default"))
            {
                //reset position and rotation
                if (Vector3.Distance(transform.localPosition, pos) < 0.01f)
                {
                    transform.localPosition = pos;
                }
                else {

                    Debug.Log("heress");
                    transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.1f);
                    //vkit.transform.localPosition = Vector3.Lerp(vkit.transform.localPosition, vkitLocalPos, 0.1f);

                }
                transform.localRotation = rot;
            }


        }

        if (vkit.GetCurrentAnimatorStateInfo(0).IsName("closeVkit00"))
        {
            vkit.transform.localEulerAngles = new Vector3(0f, -160f, 0f);
        }

        if (playerhoverMap == true)
        {
            mapPanel.localPosition = Vector3.Lerp(mapPanel.localPosition, Vector3.zero, 0.2f);
            mappanel.SetActive(true);
        }
        else {
            mapPanel.localPosition = Vector3.Lerp(mapPanel.localPosition, mapLocalPos, 0.2f);

            if (Vector3.Distance(mapPanel.localPosition, mapLocalPos) < 0.01f)
                mappanel.SetActive(false);
        }

        if (playerhoverTools)
        {
            toolPanel.localPosition = Vector3.Lerp(toolPanel.localPosition, Vector3.zero, 0.2f);
            toolpanel.SetActive(true);
        }
        else
        {
            toolPanel.localPosition = Vector3.Lerp(toolPanel.localPosition, mapLocalPos, 0.2f);

            if (Vector3.Distance(toolPanel.localPosition, mapLocalPos) < 0.01f)
                toolpanel.SetActive(false);
        }

    }
}
