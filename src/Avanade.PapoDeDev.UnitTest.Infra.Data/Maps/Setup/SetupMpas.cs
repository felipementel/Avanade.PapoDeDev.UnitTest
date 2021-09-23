namespace Avanade.PapoDeDev.UnitTest.Infra.Data.Maps.Setup
{
    public static class SetupMaps
    {
        public static void ConfigureMaps()
        {
            DocumentMap.Configure();
            AccountMap.Configure();
            CustomerMap.Configure();
        }
    }
}
