using UnityEngine;

namespace Global.Sound
{
    public class SoundButton: TurnButton
    {
        protected override void TurnOn()
        {
            float volume = LocalStorage.GetValue("saveSound", 0.5f);
            SoundManager.VolumeSound(volume);
        } 
        protected override void TurnOff()
        {
            float volume = LocalStorage.GetValue("sound", 0.5f);
            LocalStorage.SetValue("saveSound", volume);

            SoundManager.VolumeSound(0);
        } 
    }
}