using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    public RectTransform textTransform;
    public float scrollSpeed = 10.0f;

    // Start is called before the first frame update
    void OnEnable()
    {
        textTransform.anchoredPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTravelled = scrollSpeed * Time.deltaTime;
        float textHeight = textTransform.rect.height;
        textHeight += (textTransform.parent as RectTransform).rect.height;

        Vector3 position = textTransform.anchoredPosition;
        position.y += distanceTravelled;

        if (position.y > textHeight)
        {
            position = Vector3.zero;
        }

        textTransform.anchoredPosition = position;
    }
}
