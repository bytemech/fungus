// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Sets the active profile that the Save Variable and Load Variable commands will use. This is useful to crete multiple player save games. Once set, the profile applies across all Flowcharts and will also persist across scene loads.
    /// </summary>
    [CommandInfo("Save",
                 "Set Save Profile",
                 "Sets the active profile that the Save Variable and Load Variable commands will use. This is useful to crete multiple player save games. Once set, the profile applies across all Flowcharts and will also persist across scene loads.")]
    [AddComponentMenu("")]
    public class SetSaveProfile : Command
    {
        [Tooltip("Name of save profile to make active.")]
        [SerializeField] protected string saveProfileName = "";

        #region Public members

        public override void OnEnter()
        {
            FungusManager.Instance.UserProfileManager.ChangeProfile(saveProfileName);

            Continue();
        }

        public override string GetSummary()
        {
            return saveProfileName;
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        #endregion Public members
    }
}
