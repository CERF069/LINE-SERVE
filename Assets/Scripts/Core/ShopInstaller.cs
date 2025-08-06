using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    using Zenject;
    using Shop;
    public class ShopInstaller : MonoInstaller
    {
        [SerializeField] private ShopManager _shopManager;

        public override void InstallBindings()
        {
            Container.Bind<ShopManager>().FromInstance(_shopManager).AsSingle();
        }
    }

}