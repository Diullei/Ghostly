namespace Ghostly
{
    public class Version
    {
        public const int GhostlyMajorVersion = 0;
        public const int GhostlyMinorVersion = 2;
        public const int GhostlyPatchVersion = 1;

        public static string GhostlyVersionString
        {
            get
            {
#if RELEASE
                return GhostlyMajorVersion + "." + GhostlyMinorVersion + "." + GhostlyPatchVersion;
#else
                return GhostlyMajorVersion + "." + GhostlyMinorVersion + "." + GhostlyPatchVersion + "-pre";
#endif
            }
        }

        public static string GhostlyVersion
        {
            get { return "v" + GhostlyVersionString; }
        }
    }
}
