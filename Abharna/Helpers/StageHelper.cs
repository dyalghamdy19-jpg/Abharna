namespace Abharna.Helpers
{
    // gives the name of a container stage (1 to 5)
    public static class StageHelper
    {
        public static string Name(int stage)
        {
            switch (stage)
            {
                case 1: return "Registered";
                case 2: return "In transit";
                case 3: return "At port";
                case 4: return "Customs hold";
                default: return "Released";
            }
        }
    }
}
