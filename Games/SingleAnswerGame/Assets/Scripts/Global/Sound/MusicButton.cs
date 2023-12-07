namespace Global.Sound
{
    public class MusicButton : TurnButton
    {
        protected override void TurnOn()
        {
            MusicManager.VolumeSound(0.3f);
        }

        protected override void TurnOff()
        {
            MusicManager.VolumeSound(0);
        }
    }
}