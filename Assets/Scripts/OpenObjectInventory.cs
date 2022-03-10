using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenObjectInventory : MonoBehaviour
{
    [SerializeField]
    GameObject objectsPanel;

    bool isOpen = false;
    Vector3 start;
    Vector3 end;

    public void EnableOptionsPanel()
    {
        int indexToActivate = transform.GetSiblingIndex();

        for(int i = 0; i < objectsPanel.transform.childCount; i++)
        {
            objectsPanel.transform.GetChild(i).gameObject.SetActive(false); //-10 300
        }

        objectsPanel.transform.GetChild(indexToActivate).gameObject.SetActive(true);
    }

    public void OpenClosePanel()
    {
        RectTransform yourRect = this.GetComponent<RectTransform>();
        RectTransform panelRect = objectsPanel.GetComponent<RectTransform>();
        if (!isOpen)
        {
            StartCoroutine(switchCamera(yourRect.anchoredPosition, new Vector3(-330, yourRect.anchoredPosition.y, 0), yourRect));
            StartCoroutine(switchCamera(panelRect.anchoredPosition, new Vector3(-10, panelRect.anchoredPosition.y, 0), panelRect));
            isOpen = !isOpen;
        }
        else
        {
            StartCoroutine(switchCamera(yourRect.anchoredPosition, new Vector3(-20, yourRect.anchoredPosition.y, 0), yourRect));
            StartCoroutine(switchCamera(panelRect.anchoredPosition, new Vector3(300, panelRect.anchoredPosition.y, 0), panelRect));
            isOpen = !isOpen;
        }
    }

    IEnumerator switchCamera(Vector3 start, Vector3 end, RectTransform rect)
    {
        var animSpeed = 0.5f;

        float progress = 0.0f;  //This value is used for LERP

        while (progress < 1.0f)
        {
            if(isOpen)
                rect.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(start, end, progress);
            else
                rect.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(start, end, progress);
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime * animSpeed;
        }

        //Set final transform
        rect.GetComponent<RectTransform>().anchoredPosition = end;
    }
}
