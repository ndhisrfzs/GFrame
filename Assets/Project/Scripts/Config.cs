namespace GFrame
{
    public class Config : Singleton<Config>
    {
        private Config() { }

        public ulong[] KeyWithLogin;
        public ulong[] KeyWithGame;
    }
}
