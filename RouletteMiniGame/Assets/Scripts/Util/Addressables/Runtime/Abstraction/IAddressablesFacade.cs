using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Util.Addressables.Runtime.Abstraction
{
    public interface IAddressablesFacade
    {
        UniTask InitializeAddressablesAsync(CancellationToken token);
        UniTask<AsyncOperationHandle<TObject>> LoadAssetAsync<TObject>(string assetAddress, CancellationToken token);
        bool IsResourceExists(string key);
        void Release<TObject>(AsyncOperationHandle<TObject> handle);
    }
}