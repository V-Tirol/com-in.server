using MediaToolkit;
using MediaToolkit.Model;

namespace com_in.server.Helper
{
    public static class DurationGetter
    {
        public static TimeSpan GetVideoDuration(string filePath)
        {
            var inputFile = new MediaFile { Filename = filePath };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
            }

            return inputFile.Metadata.Duration;
        }
    }
}
