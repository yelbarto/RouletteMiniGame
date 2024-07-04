using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.Domain;
using RouletteMiniGame.RouletteScene.RouletteArea;
using RouletteMiniGame.RouletteScene.WalletArea;
using UnityEngine;
using Util;
using Util.Addressables.Runtime;

namespace RouletteMiniGame
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private WinView winView;
        [SerializeField] private WalletView walletView;
        [SerializeField] private RouletteDataSO rouletteOrderData;
        [SerializeField] private Transform uiParent;
        
        private CancellationTokenSource _gameLifetimeCts;
        private RoulettePresenter _roulettePresenter;
        private WalletRepository _walletRepository;
        private WalletPresenter _walletPresenter;

        protected override void Awake()
        {
            base.Awake();

            _gameLifetimeCts = new CancellationTokenSource();
            _walletRepository = new WalletRepository();
            var addressableFacade = new AddressablesFacade();
            addressableFacade.InitializeAddressablesAsync(_gameLifetimeCts.Token).Forget();
            _walletPresenter = new WalletPresenter(walletView, _walletRepository, addressableFacade);
            _roulettePresenter = new RoulettePresenter(_walletPresenter, rouletteOrderData, 
                addressableFacade, _walletRepository, winView, uiParent);
        }

        protected override void SingletonDestroyed()
        {
            _roulettePresenter.Dispose();
            _walletPresenter.Dispose();
            _gameLifetimeCts.Cancel();
        }
    }
}