using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrowingResourceManagerUI : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI resourceValueText;
    public GrowingResourceManager owner;

    // Start is called before the first frame update
    void Start()
    {
        fillImage.rectTransform.localScale = new Vector3(owner.GetCurrentResourceValueRatio(), 1, 1);
        resourceValueText.text = owner.CurrentResourceValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!owner.outOfResource)
        {
            //Debug.Log(owner.GetCurrentResourceValueRatio());
            fillImage.rectTransform.localScale = new Vector3(owner.GetCurrentResourceValueRatio(), 1, 1);
            resourceValueText.text = ((int)owner.CurrentResourceValue).ToString();
        }
    }
}
