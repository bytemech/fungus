﻿// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Fungus manager singleton. Manages access to all Fungus singletons in a consistent manner.
    /// </summary>
    [RequireComponent(typeof(UserProfileManagerComponent))]
    [RequireComponent(typeof(CameraManager))]
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(EventDispatcher))]
    [RequireComponent(typeof(GlobalVariables))]
    [RequireComponent(typeof(MainAudioMixer))]
    [RequireComponent(typeof(SaveManager))]
    [RequireComponent(typeof(NarrativeLog))]
    public sealed class FungusManager : MonoBehaviour
    {
        private static volatile FungusManager instance;  // The keyword "volatile" is friendly to the multi-thread.
        private static bool applicationIsQuitting = false;
        private static readonly object _lock = new object();  // The keyword "readonly" is friendly to the multi-thread.

        private void Awake()
        {
            if (instance == null)
                instance = this;

            UserProfileManager = GetComponent<UserProfileManagerComponent>();
            CameraManager = GetComponent<CameraManager>();
            MusicManager = GetComponent<MusicManager>();
            EventDispatcher = GetComponent<EventDispatcher>();
            GlobalVariables = GetComponent<GlobalVariables>();
            MainAudioMixer = GetComponent<MainAudioMixer>();
            SaveManager = GetComponent<SaveManager>();
            NarrativeLog = GetComponent<NarrativeLog>();

            SaveManager.SaveFileManager.Init(UserProfileManager.UserProfileManager);
            SaveManagerSignals.OnSaveReset += SaveManagerSignals_OnSaveReset;

            MainAudioMixer.Init();
            MusicManager.Init();
        }

        private void SaveManagerSignals_OnSaveReset()
        {
            TextVariationHandler.ClearHistory();
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed,
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        private void OnDestroy()
        {
            applicationIsQuitting = true;
        }

        #region Public methods

        /// <summary>
        /// Gets the camera manager singleton instance.
        /// </summary>
        public CameraManager CameraManager { get; private set; }

        /// <summary>
        /// Gets the music manager singleton instance.
        /// </summary>
        public MusicManager MusicManager { get; private set; }

        /// <summary>
        /// Gets the event dispatcher singleton instance.
        /// </summary>
        public EventDispatcher EventDispatcher { get; private set; }

        /// <summary>
        /// Gets the global variable singleton instance.
        /// </summary>
        public GlobalVariables GlobalVariables { get; private set; }

        public MainAudioMixer MainAudioMixer { get; private set; }

        /// <summary>
        /// Gets the save manager singleton instance.
        /// </summary>
        public SaveManager SaveManager { get; private set; }

        /// <summary>
        /// Gets the history manager singleton instance.
        /// </summary>
        public NarrativeLog NarrativeLog { get; private set; }

        public UserProfileManagerComponent UserProfileManager { get; private set; }

        /// <summary>
        /// Gets the FungusManager singleton instance.
        /// </summary>
        public static FungusManager Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("FungusManager.Instance() was called while application is quitting. Returning null instead.");
                    return null;
                }

                // Use "double checked locking" algorithm to implement the singleton for this "FungusManager" class, which can improve performance.
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            var go = new GameObject();
                            go.name = "FungusManager";
                            if (Application.isPlaying)
                                DontDestroyOnLoad(go);
                            instance = go.AddComponent<FungusManager>();
                        }
                    }
                }

                return instance;
            }
        }

        public static void ForceApplicationQuitting(bool b)
        {
            applicationIsQuitting = b;
        }

        #endregion Public methods
    }
}
