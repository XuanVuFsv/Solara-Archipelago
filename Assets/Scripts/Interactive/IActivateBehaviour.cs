using UnityEngine;

namespace VitsehLand.Assets.Scripts.Interactive
{
    public interface IActivateBehaviour
    {
        public void ExecuteActivateAction();
    }

    public abstract class ActivateBehaviour : MonoBehaviour, IActivateBehaviour
    {
        public virtual void ExecuteActivateAction()
        {

        }
    }
}