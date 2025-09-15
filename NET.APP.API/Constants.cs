using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.APP.API
{
    public static class Constants
    {
        /// <summary>
        /// The file extension used to rename an automated run file when complete
        /// </summary>
        public const string RunCompleteExtension = "complete";

        /// <summary>
        /// The string placeholder for running the attached checkset
        /// </summary>
        public const string AttachedCheckSetPath = "*Attached*";

        public const string StorageName = "Name";

        public const string StorageNumber = "Number";

        public const string StorageIsDefined = "Is Defined";

        public const string StorageInPlace = "Is In Place";

        public const string StorageElementType = "Is Element Type";

        public const string StorageElevation = "Elevation";

        public const string StorageValue = "Value";

        public const string StorageClassName = "Full Class Name";

        public const string StorageLocation = "Location";

        public const string StorageType = "Type";

        public const string StorageTrue = "True";

        public const string StorageFalse = "False";

        public const string StorageExisting = "Existing";

        public const string StorageNew = "New";

        public const string StorageDemolished = "Demolished";

        public const string StorageTemporary = "Temporary";

        public const string StorageFuture = "Future";

        public const string CombinedItemName = "Combined Items";

        /// <summary>
        /// Where automated run data is stored
        /// </summary>
        [Localizable(false)]
        public static string BaseFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Autodesk", "AIT", "Model Checker");

        /// <summary>
        /// Where logs are to be stored
        /// </summary>
        public static string LogFolder => Path.Combine(BaseFolder, "Logs");

        /// <summary>
        /// All lowercase supported values for true
        /// </summary>
        [Localizable(false)]
        public static List<string> TrueValues { get; } = new List<string> { "1", "true", "yes", "y", "t" };

        /// <summary>
        /// All supported lowercase values for false
        /// </summary>
        [Localizable(false)]
        public static List<string> FalseValues { get; } = new List<string> { "0", "false", "no", "n", "f" };

        /// <summary>
        /// Where automated run data is stored
        /// </summary>
        [Localizable(false)]
        public static string GetAutomatedRunFolder(int revitYear)
        {
            return Path.Combine(BaseFolder, revitYear.ToString(), "Automated Runs");
        }
    }
}
