using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Solana.Unity.Wallet;
using VitsehLand.Scripts;
using VitsehLand.Scripts.Ultilities;

// ReSharper disable once CheckNamespace

namespace Solana.Unity.SDK.Example
{
    public class LoginScreen : SimpleScreen
    {
        [SerializeField]
        private TMP_InputField passwordInputField;
        [SerializeField]
        private TextMeshProUGUI passwordText;
        [SerializeField]
        private Button loginBtn; 
        [SerializeField]
        private Button loginBtnGoogle;
        [SerializeField]
        private Button loginBtnTwitter;
        [SerializeField]
        private Button loginBtnWalletAdapter;
        [SerializeField]
        private Button loginBtnSms;
        [SerializeField]
        private Button loginBtnXNFT;
        [SerializeField]
        private TextMeshProUGUI messageTxt;
        [SerializeField]
        private TMP_Dropdown dropdownRpcCluster;

        public bool hasLogin = false;

        private void OnEnable()
        {
            dropdownRpcCluster.interactable = true;
            passwordInputField.text = string.Empty;

            MyDebug.Log(Web3.Wallet);
            if (Web3.Wallet != null)
            {
                dropdownRpcCluster.interactable = false;
                manager.ShowScreen(this, "wallet_screen");
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            passwordText.text = "";

            passwordInputField.onSubmit.AddListener(delegate { LoginChecker(); });

            loginBtn.onClick.AddListener(LoginChecker);
            loginBtnGoogle.onClick.AddListener(delegate{LoginCheckerWeb3Auth(Provider.GOOGLE);});
            loginBtnTwitter.onClick.AddListener(delegate{LoginCheckerWeb3Auth(Provider.TWITTER);});
            loginBtnWalletAdapter.onClick.AddListener(LoginCheckerWalletAdapter);
            loginBtnSms.onClick.AddListener(LoginCheckerSms);
            loginBtnXNFT.onClick.AddListener(LoginCheckerWalletAdapter);
            
            loginBtnXNFT.gameObject.SetActive(false);

            if (Application.platform is RuntimePlatform.LinuxEditor or RuntimePlatform.WindowsEditor or RuntimePlatform.OSXEditor)
            {
                loginBtnWalletAdapter.onClick.RemoveListener(LoginCheckerWalletAdapter);
                loginBtnWalletAdapter.onClick.AddListener(() =>
                    Debug.LogWarning("Wallet adapter login is not yet supported in the editor"));
            }

            if(messageTxt != null)
                messageTxt.gameObject.SetActive(false);
        }
        private async void LoginChecker()
        {
            var password = passwordInputField.text;
            var account = await Web3.Instance.LoginInGameWallet(password);
            CheckAccount(account);
        }

        private async void LoginCheckerSms()
        {
            var account = await Web3.Instance.LoginWalletAdapter();
            CheckAccount(account);
        }
        
        private async void LoginCheckerWeb3Auth(Provider provider)
        {
            var account = await Web3.Instance.LoginWeb3Auth(provider);
            CheckAccount(account);
        }

        private async void LoginCheckerWalletAdapter()
        {
            if(Web3.Instance == null) return;
            var account = await Web3.Instance.LoginWalletAdapter();
            messageTxt.text = "";
            CheckAccount(account);
        }


        private void CheckAccount(Account account)
        {
            if (account != null)
            {
                dropdownRpcCluster.interactable = false;
                messageTxt.gameObject.SetActive(false);
                gameObject.SetActive(false);
                manager.ShowScreen(this, "wallet_screen");

                if (!hasLogin)
                {
                    hasLogin = true;
                    Debug.Log("Login");
                    WalletManager.Instance.HideWalletUI();
                }
            }
            else
            {
                hasLogin = false;
                passwordInputField.text = string.Empty;
                messageTxt.gameObject.SetActive(true);
            }
        }

        public void OnClose()
        {
            var wallet = GameObject.Find("wallet");
            wallet.SetActive(false);
        }
    }
}
