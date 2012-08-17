namespace Ghostly
{
    public class Version
    {
        public const int GhostlyMajorVersion = 0;
        public const int GhostlyMinorVersion = 1;
        public const int GhostlyPatchVersion = 0;

        public static string GhostlyVersionString
        {
            get
            {
#if DEBUG
                return GhostlyMajorVersion + "." + GhostlyMajorVersion + "." + GhostlyMajorVersion;
#else
                return GhostlyMajorVersion + "." + GhostlyMajorVersion + "." + GhostlyMajorVersion + "-pre";
#endif
            }
        }

        public static string GhostlyVersion
        {
            get { return "v" + GhostlyVersionString; }
        }
    }
}
