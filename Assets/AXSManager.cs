using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AXSManager : Singleton<AXSManager>
{
    public float count;
    public TextMeshProUGUI AXSText;

    public int smallestReward, mediumReward, hightestReward, starRandomRangeValue, endRandomRangeValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(float count)
    {
        //Debug.Log(count);
        this.count += count;
        AXSText.text = this.count.ToString();
    }
}
