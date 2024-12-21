using Solana.Unity.SDK.Example;
using Solana.Unity.Wallet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Scripts.Pattern.Singleton;
using Sirenix;
using Sirenix.OdinInspector;
using System.Xml;

namespace VitsehLand.Scripts
{
    public class WalletManager : Singleton<WalletManager>
    {
        public GameObject WalletPanel;
        public GameObject SolanaWalletBackground;
        public GameObject WalletHolder;
        public SimpleScreen simpleScreenManager;

        [ShowInInspector]
        public Dictionary<string, GameObject> lands= new Dictionary<string, GameObject>();
        public List<string> landNames = new List<string>();
        public List<GameObject> landObjects = new List<GameObject>();

        public bool isEnable = false;
        public bool assignLandsOnStart;

        public List<string> activeLandNames = new List<string>();
        public string nftLandCollectionAddress = "";

        public override void Awake()
        {
            base.Awake();

            if (assignLandsOnStart)
            {
                for (int i = 0; i < landObjects.Count; i++)
                {
                    lands.Add(landNames[i], landObjects[i]);
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (isEnable)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            gameObject.SetActive(isEnable);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                WalletPanel.SetActive(!WalletPanel.activeSelf);

                if (WalletPanel.activeSelf)
                {
                    //Set up cursor
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    //Set up cursor
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }

        public void HideWalletUI()
        {
            //Set up cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            WalletPanel.SetActive(false);
            SolanaWalletBackground.SetActive(false);
            WalletHolder.SetActive(false);
            simpleScreenManager.HideScreen();
        }

        public void ShowWalletUI()
        {
            //SolanaWalletBackground.SetActive(true);
            simpleScreenManager.ShowScreen();
            WalletHolder.SetActive(true);
        }

        public void UpdateHoldingLandNFTs(List<TokenItem> tokenItems)
        {
            Debug.Log("Updating...");
            int totalLands = 0;
            activeLandNames.Clear();
            for (int i = 0; i < tokenItems.Count; i++)
            {
                if (nftLandCollectionAddress != tokenItems[i].GetCollectionAddress()) continue;
                Debug.Log(tokenItems[i].GetCollectionAddress());

                string name = tokenItems[i].GetName();
                if (landNames.IndexOf(name) < 0) continue;
                activeLandNames.Add(name);
                Debug.Log(name);

                if (totalLands >= 5) break;
            }

            for (int i = 0; i < landNames.Count; i++)
            {
                if (activeLandNames.IndexOf(landNames[i]) >= 0)
                {
                    landObjects[i].gameObject.SetActive(true);
                }
                else
                {
                    landObjects[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
