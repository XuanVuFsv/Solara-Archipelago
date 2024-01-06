using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShardManager : Singleton<ShardManager>
{
    public TextMeshProUGUI text;

    [SerializeField]
    private int shardCount = 0;

    public int ShardCount
    {
        get { return shardCount;  }
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = shardCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddShard(int addedShardCount)
    {
        shardCount += addedShardCount;
        text.text = shardCount.ToString();
    }

    public bool UseShard(int usedShardCount)
    {
        if (shardCount - usedShardCount < 0) return false;
        shardCount -= usedShardCount;
        text.text = shardCount.ToString();
        return true;
    }
}
