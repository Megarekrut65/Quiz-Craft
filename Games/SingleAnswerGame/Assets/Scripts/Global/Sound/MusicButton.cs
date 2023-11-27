using UnityEngine;

namespace Global.Sound
{
    public class MusicButton: TurnButton
    {
        protected override void TurnOn()
        {
            float volume = LocalStorage.GetValue("saveMusic", 0.5f);
            MusicManager.VolumeSound(volume);
        } 
        protected override void TurnOff()
        {
            float volume = LocalStorage.GetValue("music", 0.5f);
            LocalStorage.SetValue("saveMusic", volume);

            MusicManager.VolumeSound(0);
        } 
    }
}