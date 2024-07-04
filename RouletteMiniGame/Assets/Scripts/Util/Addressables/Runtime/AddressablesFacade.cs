using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Util.Addressables.Runtime.Abstraction;

namespace Util.Addressables.Runtime
{
    public class AddressablesFacade : IAddressablesFacade
    {
        public async UniTask InitializeAddressablesAsync(CancellationToken token)
        {
            var initializeHandler = UnityEngine.AddressableAssets.Addressables.InitializeAsync();
            while (initializeHandler.Status is AsyncOperationStatus.None)
            {
                await UniTask.Yield(token);
            }
            if (initializeHandler.Status is AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Addressables initialized");
            }
            else
            {
                Debug.LogError($"Failed to initialize addressables {initializeHandler.OperationException.GetType()} {initializeHandler.OperationException.Message} {initializeHandler.OperationException.StackTrace}");
            }
        }
        
        public bool IsResourceExists(string key)
        {
            return UnityEngine.AddressableAssets.Addressables.ResourceLocators.Any(resourceLocator => resourceLocator.Locate(key, null, out _));
        }
        
        /// <summary>
        /// Returned operation handle needs to be released for the asset to be unloaded
        /// </summary>
        /// <param name="assetAddress">Addressable key</param>
        /// <param name="token">Cancellation token</param>
        /// <typeparam name="TObject">Desired object type</typeparam>
        /// <returns></returns>
        public async UniTask<AsyncOperationHandle<TObject>> LoadAssetAsync<TObject>(string assetAddress, 
            CancellationToken token)
        {
            var assetLoadHandler = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<TObject>(assetAddress);
            while (assetLoadHandler.Status is AsyncOperationStatus.None)
            {
                await UniTask.Yield(token);
            }
            if (assetLoadHandler.Status is AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Asset loaded: {assetAddress}");
            }
            else
            {
                Debug.LogError($"Failed to load asset {assetAddress} {assetLoadHandler.OperationException.GetType()} {assetLoadHandler.OperationException.Message} {assetLoadHandler.OperationException.StackTrace}");
            }

            return assetLoadHandler;
        }

        public void Release<TObject>(AsyncOperationHandle<TObject> handle)
        {
            UnityEngine.AddressableAssets.Addressables.Release(handle);
        }
    }
}