using UnityEngine;
using UnityEngine.Events;

namespace VitsehLand.Assets.Scripts.Ultility
{
    public class AnimationEvent : UnityEvent<string>
    {

    }

    public class WeaponAnimationEvents : MonoBehaviour
    {
        public AnimationEvent weaponAnimationEvent = new AnimationEvent();
        public void OnAnimationEvent(string eventName)
        {
            weaponAnimationEvent.Invoke(eventName);
        }
    }
}