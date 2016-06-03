// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.Util.Pattern;

namespace Assets.Sources.NativePlatform
{
    // Public methods here may be referenced from native code
    public class UnityApi: InGameSingleton<UnityApi>
    {
        private static bool instanceCreated;

        public void Awake()
        {
            if (instanceCreated)
            {
                DestroyObject(gameObject);
            }
            else
            {
                instanceCreated = true;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void Start()
        {
            NativeApi.OnUnityApiAvailable(gameObject.name);
        }

        // this may be thought as Main() for UnityApp
        public void StartGame()
        {
            //GameObject.FindGameObjectWithTag(Tags.LoadingManager).GetComponent<LoadingSceneManager>().StartLoading();
        }

        public void StopGame()
        {
            //SceneManager.LoadScene(GameScene.LoadingScene.GetFileName());
        }
    }
}
