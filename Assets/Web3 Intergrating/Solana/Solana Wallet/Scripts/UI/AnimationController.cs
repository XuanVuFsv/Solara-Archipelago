using UnityEngine;
using VitsehLand.Scripts;

// ReSharper disable once CheckNamespace
public class AnimationController : MonoBehaviour
{
    Animation anim;

    void Start()
    {
        anim = this.GetComponent<Animation>();
    }
    
    // Playing idle animation for menu components.
    public void PlayIdle()
    {
        anim.Play(anim.name + "-Idle");
    }

    // Playing window open animation.
    public void OpenWindow()
    {
        anim.Play("Window-In");
    }

    // Playing window close animation.
    public void CloseWindow()
    {
        anim.Play("Window-Out");
        WalletManager.Instance.HideWalletUI();
    }
}
