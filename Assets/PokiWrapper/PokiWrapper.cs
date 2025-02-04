#define POKI_ADS

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.BubbleShooterGameToolkit.Scripts.Ads.Networks.PokiWrapper
{
    public class PokiWrapper
    {
        public enum AdsState { NotStarted, Initializing, Ready };
        private static AdsState adsState = AdsState.NotStarted;

        private static IAds instance;

        public static void Init(Action onInitialized)
        {
            if (adsState != AdsState.NotStarted) return;

#if POKI_ADS
            instance = new PokiAds();
#else
            instance = new DummyAds();
#endif
            adsState = AdsState.Initializing;
            Debug.Log("PokiWrapperState " + adsState);
            instance.Init(() =>
            {
                adsState = AdsState.Ready;
                Debug.Log("PokiWrapperState " + adsState);
                onInitialized();
            });
        }

        public static void CommercialBreak(Action onAdsShowComplete)
        {
            if (instance != null)
            {
                instance.CommercialBreak(onAdsShowComplete);
            }
            else
            {
                Debug.Log("PokiWrapper.CommercialBreak: IAds instance is null");
            }
        }

        internal static void RewardedBreak(Action<bool> onRewardsShowComplete)
        {
            if (instance != null)
            {
                instance.RewardedBreak(onRewardsShowComplete);
            }
            else
            {
                Debug.Log("PokiWrapper.RewardedBreak: IAds instance is null");
            }
        }

        public static void GameplayStart()
        {
#if POKI_ADS
            if (adsState == AdsState.Ready)
            {
                PokiUnitySDK.Instance.gameplayStart();
            }
            else
            {
                UnityEngine.Debug.Log("PokiWrapper.GameplayStart: pokisdk is not ready");
            }
#endif
        }

        public static void GameplayStop()
        {
#if POKI_ADS
            if (adsState == AdsState.Ready)
            {
                PokiUnitySDK.Instance.gameplayStop();
            }
            else
            {
                UnityEngine.Debug.Log("PokiWrapper.GameplayStop: pokisdk is not ready");
            }
#endif
        }
    }

    public interface IAds
    {
        void Init(Action onInitialized);
        void CommercialBreak(Action onAdsShowComplete);
        void RewardedBreak(Action<bool> onRewardsShowComplete);
    }

    public class PokiAds : IAds
    {
        public void Init(Action onInitialized)
        {
            PokiUnitySDK.Instance.sdkInitializedCallback = () =>
            {
                Debug.Log("PokiAds Initialized");
                onInitialized();
            };
            PokiUnitySDK.Instance.init();
        }

        public void CommercialBreak(Action onAdsShowComplete)
        {
            PokiUnitySDK.Instance.commercialBreakCallBack = () =>
            {
                Debug.Log($"PokiAds.CommercialBreakCallback");
                onAdsShowComplete();
            };
            PokiUnitySDK.Instance.commercialBreak();
        }

        public void RewardedBreak(Action<bool> onRewardsShowComplete)
        {
            PokiUnitySDK.Instance.rewardedBreakCallBack = (bool withRewards) =>
            {
                Debug.Log($"PokiAds.RewardedBreakCallback {withRewards}");
                onRewardsShowComplete(withRewards);
            };
            PokiUnitySDK.Instance.rewardedBreak();
        }
    }

    public class DummyAds : IAds
    {
        public void Init(Action onInitialized)
        {
            Debug.Log("DummyAds Initialized");

            onInitialized();
        }

        public async void CommercialBreak(Action onAdsShowComplete)
        {
            Debug.Log("DummyAds CommercialBreak");
            var gameObject = new GameObject("DummyAd");
            gameObject.AddComponent<Canvas>();
            var imageObj = new GameObject("DummyImage");
            imageObj.transform.parent = gameObject.transform;
            imageObj.AddComponent<Image>();

            await Task.Delay(3000);

            UnityEngine.Object.Destroy(gameObject);

            onAdsShowComplete();
        }

        public async void RewardedBreak(Action<bool> onRewardsShowComplete)
        {
            Debug.Log("DummyAds RewardedBreak");

            await Task.Delay(1000);

            onRewardsShowComplete(true);
        }
    }
}