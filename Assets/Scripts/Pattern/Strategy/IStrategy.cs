using VitsehLand.Scripts.Weapon.Primary;

namespace VitsehLand.Scripts.Pattern.Strategy
{
    // The Strategy interface declares a method for executing a stragety.
    public interface IWeaponStrategy
    {
        public void HandleLeftMouseClick();
        public void HandleRightMouseClick();
        public void SetInputData(object _inputData);
        ShootingInputData GetShootingInputData();
    }

    public interface IPrimaryWeaponStrategy : IWeaponStrategy { }

    public interface IHandGunWeaponStrategy : IWeaponStrategy { }

    public interface ICollectorWeaponStrategy : IWeaponStrategy { }
}