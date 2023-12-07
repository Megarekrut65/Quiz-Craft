namespace Global.Sound
{
    public class SoundButton : TurnButton
    {
        protected override void TurnOn()
        {
            SoundManager.VolumeSound(0.5f);
        }

        protected override void TurnOff()
        {
            SoundManager.VolumeSound(0);
        }
    }
}