using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CityModeToExplore : MonoBehaviour
{
    public static CityModeToExplore Instance;

    [SerializeField]
    private Animator transition;

    [SerializeField]
    private GameObject ToFollow;

    [SerializeField]
    private CinemachineBrain brain;

    [SerializeField]
    private GameObject joystick;

    public bool isBuilding = true;

    GameObject[] cityGameObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        cityGameObjects = GameObject.FindGameObjectsWithTag("City");
        //Debug.Log(cityGameObjects.Length + " " + cityGameObjects[cityGameObjects.Length - 1].name);
    }

    public void TrnaistionToPlayer()
    {
        StartCoroutine(switchCamera());
        GameObject visualizer = GameObject.Find("GhostVisualizer");
        if(visualizer.transform.childCount > 0)
        {
            GridBuildingSystem.Instance.RemoveVisual();
        }
    }

    IEnumerator switchCamera()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        brain.enabled = true;

        ToFollow.SetActive(true);
        if(cityGameObjects.Length >= 1)
        {
            ToFollow.GetComponent<CharacterController>().enabled = false;
            ToFollow.transform.position = cityGameObjects[cityGameObjects.Length - 1].transform.position;
            ToFollow.GetComponent<CharacterController>().enabled = true;
        }

        GameObject.Find("Canvas").SetActive(false);

#if UNITY_ANDROID
        joystick.SetActive(true);
        isBuilding = false;
#endif

        yield return new WaitForSeconds(1f);

        transition.SetTrigger("End");
    }
}
