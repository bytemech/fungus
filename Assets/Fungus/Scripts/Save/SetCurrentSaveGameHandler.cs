﻿// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Helper component to replace and restore the SaveHandler with a set of defined components.
    /// </summary>
    public class SetCurrentSaveGameHandler : MonoBehaviour
    {
        protected ISaveHandler previousSaveHandler;
        protected SaveFileManager sm;

        public SaveDataItemSerializerComponent[] saveDataItemSerializerComponents;

        public void OnEnable()
        {
            sm = FungusManager.Instance.SaveManager.SaveFileManager;
            previousSaveHandler = sm.CurrentSaveHandler;

            var newHandler = DefaultSaveGameSaveHandler.CreateDefaultWithSerializers();

            foreach (var item in saveDataItemSerializerComponents)
            {
                newHandler.SaveDataItemSerializers.Add(item.SaveDataItemSerializer);
            }

            sm.CurrentSaveHandler = newHandler;
        }

        public void OnDisable()
        {
            if (sm != null)
                sm.CurrentSaveHandler = previousSaveHandler;
        }
    }
}
