using Solana.Unity.SDK.Example;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Scripts.Pattern.Singleton;

namespace VitsehLand.Scripts
{
    public class WalletManager : Singleton<WalletManager>
    {
        public GameObject WalletPanel;
        public GameObject SolanaWalletBackground;
        public GameObject WalletHolder;
        public SimpleScreen simpleScreenManager;

        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
    }
}
