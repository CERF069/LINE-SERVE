namespace Core
{ 
    using Zenject;
    using Racket_System;
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<PlayerInputProvider>()
                .To<ScreenClickInputProvider>()
                .AsSingle();
        }
    }
}
