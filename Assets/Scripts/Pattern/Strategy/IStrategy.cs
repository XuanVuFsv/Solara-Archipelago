using VitsehLand.Scripts.Weapon.Primary;

namespace VitsehLand.Scripts.Pattern.Strategy
{
    // The Command interface declares a method for executing a command.
    public interface IWeaponStragety
    {
        public void HandleLeftMouseClick();
        public void HandleRightMouseClick();
        public void SetInputData(object _inputData);
        ShootingInputData GetShootingInputData();
    }

    public interface IPrimaryWeaponStragety : IWeaponStragety
    {

    }

    public interface IHandGunWeaponStragety : IWeaponStragety
    {

    }

    public interface ICollectorWeaponStragety : IWeaponStragety
    {

    }
}