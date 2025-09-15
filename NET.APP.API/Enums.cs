using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.APP.API
{

    /// <summary>
    /// Model checker related enums
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// The results of a check
        /// </summary>
        public enum CheckResults
        {
            Pass,
            Fail,
            Error,
            Report,
            NotRun
        }

        /// <summary>
        /// Types of units to use for unit conversion
        /// </summary>
        public enum UnitTypes
        {
            Imperial,
            Metric
        }

        /// <summary>
        /// Class of units to use for conversion
        /// </summary>
        public enum UnitClasses
        {
            None,
            Length,
            Area,
            Volume,
            Angle
        }

        /// <summary>
        /// Unit to use for conversion
        /// </summary>
        public enum Units
        {
            None = 0,
            Default = 10,
            Feet = 100,
            Inches = 101,
            Meters = 102,
            Decimenters = 103,
            Centimeters = 104,
            Millimeters = 105,
            SquareFeet = 200,
            SquareInches = 201,
            SquareMeters = 202,
            SquareCentimeters = 203,
            SquareMillimeters = 204,
            Acres = 205,
            Hectacres = 206,
            CubicFeet = 300,
            CubicYards = 301,
            CubicInches = 302,
            CubicMeters = 303,
            CubicCentimeters = 304,
            CubicMillimeters = 305,
            Liters = 306,
            USGallons = 307,
            Degrees = 400,
            Radians = 401,
            Grads = 402
        }

        /// <summary>
        /// How elements are to be listed in dialogs
        /// </summary>
        public enum ListMode
        {
            Name,
            Value
        }

        /// <summary>
        /// Possible types of checks
        /// </summary>
        public enum CheckTypes
        {
            Custom = 0,
            FileSize = 1000,
            UnusedElements = 1001,
            AreaSpaceSchemes = 1002,
            VolumeComputations = 1003,
            LineStyles = 1004,
            ObjectStyles = 1005,
            ProjectInformation = 1006,
            WarningCount = 1007,
            DesignOptions = 1008,
            RasterImages = 1009,
            ViewsNotOnSheets = 1010,
            ViewsWithHiddenElements = 1011,
            Worksets = 1012,
            WorksetPlacement = 1013,
            LinkedCad = 1014,
            PinnedLinks = 1015,
            LinkMethod = 1016,
            BrowserOrganization = 1017,
            PhaseElements = 1018,
            Keynotes = 1019,
            Legends = 1020,
            RevitVersion = 1021,
            SharedParametersFile = 1022,
            ProjectCoordinates = 1023,
            ImportedSkp = 1024,
            TenLargestFamilies = 1025,
            FileName = 1026,
            FileNamesAndPaths = 2000,
            MissingFiles = 2001,
            PropertySetsDoNotMatch = 2002,
            PropertiesDoNotMatch = 2003,
            PropertySetsNotAttached = 2004,
            EmptyPropertyValues = 2005,
            PlaceholderValues = 2006,
            ListDefinitionValues = 2007,
            ClassificationDefinitionValues = 2008,
            FileNameFormat = 2009
        }

        /// <summary>
        /// Condition for determining the result of a check
        /// </summary>
        public enum ResultConditions
        {
            FailNoElements,
            FailMatchingElements,
            CountOnly,
            CountAndList
        }

        /// <summary>
        /// Possible filter operators
        /// </summary>
        public enum Operators
        {
            And,
            Or,
            Exclude
        }

        /// <summary>
        /// Possible filter categories
        /// </summary>
        public enum Categories
        {
            Category,
            Level,
            PhaseCreated,
            PhaseDemolished,
            PhaseStatus,
            View,
            TypeOrInstance,
            DesignOption,
            Room,
            Space,
            Workset,
            StructuralType,
            Family,
            APIType,
            Parameter,
            APIParameter,
            Redundant,
            Type,
            Host,
            HostParameter
        }

        /// <summary>
        /// Possible filter conditions
        /// </summary>
        public enum Conditions
        {
            Equal,
            NotEqual,
            GreaterThan,
            GreaterOrEqual,
            LessThan,
            LessOrEqual,
            Contains,
            DoesNotContain,
            WildCard,
            WildCardNoMatch,
            Defined,
            Undefined,
            HasValue,
            HasNoValue,
            Duplicated,
            Included,
            MatchesParameter,
            DoesNotMatchParameter
        }

        /// <summary>
        /// Possible filter validation types.
        /// </summary>
        public enum ValidationTypes
        {
            None,
            Boolean,
            Numeric,
            Integer,
            Length
        }

        /// <summary>
        /// Possible Options for Automated Run Cleanup
        /// </summary>
        public enum AutomatedRunCleanupOptions
        {
            None,
            Delete,
            Rename,
            Move
        }

        /// <summary>
        /// All possible options for operators
        /// </summary>
        public static IEnumerable<Operators> OperatorOptions => Enum.GetValues(typeof(Operators)).Cast<Operators>();

        /// <summary>
        /// All possible options for validation
        /// </summary>
        public static IEnumerable<ValidationTypes> ValidationOptions => from ValidationTypes x in Enum.GetValues(typeof(ValidationTypes))
                                                                        orderby x
                                                                        select x;

        /// <summary>
        /// All possible options for result conditions
        /// </summary>
        public static IEnumerable<ResultConditions> ResultConditionOptions => Enum.GetValues(typeof(ResultConditions)).Cast<ResultConditions>();

        /// <summary>
        /// All possible options for unit class
        /// </summary>
        public static IEnumerable<UnitClasses> UnitClassOptions => Enum.GetValues(typeof(UnitClasses)).Cast<UnitClasses>();

        /// <summary>
        /// All possible options for category
        /// </summary>
        public static IEnumerable<Categories> CategoryOptions => from Categories x in Enum.GetValues(typeof(Categories))
                                                                 orderby x.ToString()
                                                                 select x;

        //internal static string GetOperatorDisplay(Operators op)
        //{
        //    return op switch
        //    {
        //        Operators.And => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CommonResources.Label_And),
        //        Operators.Or => MCResources.Filter_Operators_Or,
        //        Operators.Exclude => MCResources.Filter_Operators_Exclude,
        //        _ => throw new ArgumentOutOfRangeException("op", op, null),
        //    };
        //}

        //internal static string GetValidationTypeDisplay(ValidationTypes validation)
        //{
        //    return validation switch
        //    {
        //        ValidationTypes.None => MCResources.Label_ValidationTypes_None,
        //        ValidationTypes.Boolean => MCResources.Label_ValidationTypes_Boolean,
        //        ValidationTypes.Numeric => MCResources.Label_ValidationTypes_Numeric,
        //        ValidationTypes.Integer => MCResources.Label_ValidationTypes_Integer,
        //        ValidationTypes.Length => MCResources.Label_ValidationTypes_Length,
        //        _ => throw new ArgumentOutOfRangeException("validation", validation, null),
        //    };
        //}

        ///// <summary>
        ///// Returns the UI display string for a given condition
        ///// </summary>
        ///// <param name="cnd">The condition whose display string we are looking for.</param>
        ///// <returns></returns>
        //internal static string GetConditionDisplay(Conditions cnd)
        //{
        //    return cnd switch
        //    {
        //        Conditions.Equal => "=",
        //        Conditions.GreaterOrEqual => ">=",
        //        Conditions.GreaterThan => ">",
        //        Conditions.LessOrEqual => "<=",
        //        Conditions.LessThan => "<",
        //        Conditions.NotEqual => "≠",
        //        Conditions.WildCard => MCResources.Filter_Conditions_Short_MatchesRegex,
        //        Conditions.WildCardNoMatch => MCResources.Filter_Conditions_Short_DoesntMatchRegex,
        //        Conditions.Contains => MCResources.Filter_Conditions_Short_Contains,
        //        Conditions.DoesNotContain => MCResources.Filter_Conditions_Short_DoesNotContain,
        //        Conditions.Defined => MCResources.Filter_Conditions_Short_Defined,
        //        Conditions.Undefined => MCResources.Filter_Conditions_Short_Undefined,
        //        Conditions.HasValue => MCResources.Filter_Conditions_Short_HasValue,
        //        Conditions.HasNoValue => MCResources.Filter_Conditions_Short_NoValue,
        //        Conditions.Duplicated => MCResources.Filter_Conditions_Short_Duplicated,
        //        Conditions.Included => MCResources.Filter_Conditions_Short_Included,
        //        Conditions.MatchesParameter => MCResources.Filter_Conditions_Short_MatchesParameter,
        //        Conditions.DoesNotMatchParameter => MCResources.Filter_Conditions_Short_NotMatchParameter,
        //        _ => Utility.ExpandByCapitals(cnd.ToString()),
        //    };
        //}

        ///// <summary>
        ///// Returns the UI display string for a given condition in written out format
        ///// </summary>
        ///// <param name="cnd">The condition whose display string we are looking for.</param>
        ///// <param name="isHostParameterFilter">If this is a host parameter filter or not.</param>
        ///// <returns></returns>
        //internal static string GetConditionDisplayLong(Conditions cnd, bool isHostParameterFilter = false)
        //{
        //    switch (cnd)
        //    {
        //        case Conditions.Equal:
        //            return MCResources.Filter_Conditions_Equal;
        //        case Conditions.GreaterOrEqual:
        //            return MCResources.Filter_Conditions_GreaterOrEqual;
        //        case Conditions.GreaterThan:
        //            return MCResources.Filter_Conditions_GreaterThan;
        //        case Conditions.LessOrEqual:
        //            return MCResources.Filter_Conditions_LessOrEqual;
        //        case Conditions.LessThan:
        //            return MCResources.Filter_Conditions_LessThan;
        //        case Conditions.NotEqual:
        //            return MCResources.Filter_Conditions_Unequal;
        //        case Conditions.Contains:
        //            return MCResources.Filter_Conditions_Contains;
        //        case Conditions.DoesNotContain:
        //            return MCResources.Filter_Conditions_DoesNotContain;
        //        case Conditions.Defined:
        //            return MCResources.Filter_Conditions_Defined;
        //        case Conditions.Undefined:
        //            return MCResources.Filter_Conditions_Undefined;
        //        case Conditions.HasValue:
        //            return MCResources.Filter_Conditions_NotEmpty;
        //        case Conditions.HasNoValue:
        //            return MCResources.Filter_Conditions_Empty;
        //        case Conditions.MatchesParameter:
        //            if (!isHostParameterFilter)
        //            {
        //                return MCResources.Filter_Conditions_MatchesParameter;
        //            }
        //            return MCResources.Filter_Conditions_MatchesHostParameter;
        //        case Conditions.DoesNotMatchParameter:
        //            if (!isHostParameterFilter)
        //            {
        //                return MCResources.Filter_Conditions_DoesNotMatchParameter;
        //            }
        //            return MCResources.Filter_Conditions_DoesNotMatchHostParameter;
        //        case Conditions.WildCard:
        //            return MCResources.Filter_Conditions_Wildcard;
        //        case Conditions.WildCardNoMatch:
        //            return MCResources.Filter_Conditions_WildcardNoMatch;
        //        default:
        //            return Utility.ExpandByCapitals(cnd.ToString());
        //    }
        //}

        //internal static string GetCategoryDisplay(Categories category)
        //{
        //    return category switch
        //    {
        //        Categories.Category => MCResources.Filter_Categories_Category,
        //        Categories.Level => MCResources.Filter_Categories_Level,
        //        Categories.PhaseCreated => MCResources.Filter_Categories_PhaseCreated,
        //        Categories.PhaseDemolished => MCResources.Filter_Categories_PhaseDemolished,
        //        Categories.PhaseStatus => MCResources.Filter_Categories_PhaseStatus,
        //        Categories.View => MCResources.Filter_Categories_View,
        //        Categories.TypeOrInstance => MCResources.Filter_Categories_TypeOrInstance,
        //        Categories.DesignOption => MCResources.Filter_Categories_DesignOption,
        //        Categories.Room => MCResources.Filter_Categories_Room,
        //        Categories.Space => MCResources.Filter_Categories_Space,
        //        Categories.Workset => MCResources.Filter_Categories_Workset,
        //        Categories.StructuralType => MCResources.Filter_Categories_StructuralType,
        //        Categories.Family => MCResources.Filter_Categories_Family,
        //        Categories.APIType => MCResources.Filter_Categories_ApiType,
        //        Categories.Parameter => MCResources.Filter_Categories_Parameter,
        //        Categories.APIParameter => MCResources.Label_ApiParameter,
        //        Categories.Redundant => MCResources.Filter_Categories_Redundant,
        //        Categories.Type => MCResources.Filter_Categories_Type,
        //        Categories.Host => MCResources.Filter_Categories_Host,
        //        Categories.HostParameter => MCResources.Filter_Categories_HostParameter,
        //        _ => throw new ArgumentOutOfRangeException("category", category, null),
        //    };
        //}

        //internal static string GetUnitDisplay(Units unit)
        //{
        //    return unit switch
        //    {
        //        Units.None => MCResources.Units_None,
        //        Units.Default => MCResources.Units_Default,
        //        Units.Feet => MCResources.Units_Ft,
        //        Units.Inches => MCResources.Units_Inch,
        //        Units.Meters => MCResources.Units_M,
        //        Units.Decimenters => MCResources.Units_Dm,
        //        Units.Centimeters => MCResources.Units_Cm,
        //        Units.Millimeters => MCResources.Units_mm,
        //        Units.SquareFeet => MCResources.Units_sqft,
        //        Units.SquareInches => MCResources.Units_sqin,
        //        Units.SquareMeters => MCResources.Units_sqm,
        //        Units.SquareCentimeters => MCResources.Units_sqcm,
        //        Units.SquareMillimeters => MCResources.Units_sqmm,
        //        Units.Acres => MCResources.Units_Acre,
        //        Units.Hectacres => MCResources.Units_Hectacre,
        //        Units.CubicFeet => MCResources.Units_CuFt,
        //        Units.CubicYards => MCResources.Units_CuYd,
        //        Units.CubicInches => MCResources.Units_CuInch,
        //        Units.CubicMeters => MCResources.Units_CuM,
        //        Units.CubicCentimeters => MCResources.Units_CuCm,
        //        Units.CubicMillimeters => MCResources.Units_CuMm,
        //        Units.Liters => MCResources.Units_Liter,
        //        Units.USGallons => MCResources.Units_Gallon,
        //        Units.Degrees => MCResources.Units_Deg,
        //        Units.Radians => MCResources.Units_Rad,
        //        Units.Grads => MCResources.Units_Grad,
        //        _ => throw new ArgumentOutOfRangeException("unit", unit, null),
        //    };
        //}

        //internal static string GetUnitClassDisplay(UnitClasses unitClass)
        //{
        //    return unitClass switch
        //    {
        //        UnitClasses.None => MCResources.UnitClass_None,
        //        UnitClasses.Length => MCResources.UnitClass_Length,
        //        UnitClasses.Area => MCResources.UnitClass_Area,
        //        UnitClasses.Volume => MCResources.UnitClass_Volume,
        //        UnitClasses.Angle => MCResources.UnitClass_Angle,
        //        _ => throw new ArgumentOutOfRangeException("unitClass", unitClass, null),
        //    };
        //}

        //internal static string GetResultConditionDisplay(ResultConditions cond)
        //{
        //    return cond switch
        //    {
        //        ResultConditions.FailMatchingElements => MCResources.Label_FailCondition_ReportMatching,
        //        ResultConditions.FailNoElements => MCResources.Label_FailCondition_NoElements,
        //        ResultConditions.CountOnly => MCResources.Label_FailCondition_CountOnly,
        //        ResultConditions.CountAndList => MCResources.Label_FailCondition_CountAndList,
        //        _ => string.Empty,
        //    };
        //}

        //internal static string GetPrebuiltDescription(CheckTypes checkType)
        //{
        //    return checkType switch
        //    {
        //        CheckTypes.AreaSpaceSchemes => MCResources.CheckDescription_AreaSpaceSchemes,
        //        CheckTypes.Custom => MCResources.CheckDescription_Custom,
        //        CheckTypes.DesignOptions => MCResources.CheckDescription_DesignOptions,
        //        CheckTypes.FileSize => MCResources.CheckDescription_FileSize,
        //        CheckTypes.LineStyles => MCResources.CheckDescription_LineStyles,
        //        CheckTypes.ObjectStyles => MCResources.CheckDescription_ObjectStyles,
        //        CheckTypes.ProjectInformation => MCResources.CheckDescription_ProjectInfo,
        //        CheckTypes.RasterImages => MCResources.CheckDescription_RasterImages,
        //        CheckTypes.UnusedElements => MCResources.CheckDescription_Unused,
        //        CheckTypes.ViewsNotOnSheets => MCResources.CheckDescription_ViewNotOnSheets,
        //        CheckTypes.ViewsWithHiddenElements => MCResources.CheckDescription_ViewsHiddenElements,
        //        CheckTypes.VolumeComputations => MCResources.CheckDescription_VolumeComputations,
        //        CheckTypes.WarningCount => MCResources.CheckDescription_WarningCount,
        //        CheckTypes.WorksetPlacement => MCResources.CheckDescription_WorksetPlacement,
        //        CheckTypes.Worksets => MCResources.CheckDescription_Worksets,
        //        CheckTypes.FileName => MCResources.CheckDescription_FileName,
        //        CheckTypes.LinkedCad => MCResources.CheckDescription_LinkedCad,
        //        CheckTypes.PinnedLinks => MCResources.CheckDescription_PinnedLinks,
        //        CheckTypes.LinkMethod => MCResources.CheckDescription_LinkMethod,
        //        CheckTypes.BrowserOrganization => MCResources.CheckDescription_BrowserOrganization,
        //        CheckTypes.PhaseElements => MCResources.CheckDescription_PhaseElements,
        //        CheckTypes.Keynotes => MCResources.CheckDescription_Keynotes,
        //        CheckTypes.Legends => MCResources.CheckDescription_Legends,
        //        CheckTypes.RevitVersion => MCResources.CheckDescription_RevitVersion,
        //        CheckTypes.SharedParametersFile => MCResources.CheckDescription_SharedParameters,
        //        CheckTypes.ProjectCoordinates => MCResources.CheckDescription_ProjectCoordinates,
        //        CheckTypes.ImportedSkp => MCResources.CheckDescription_ImportedSkp,
        //        CheckTypes.TenLargestFamilies => MCResources.CheckDescription_LargestFamilies,
        //        CheckTypes.FileNamesAndPaths => CommonResources.CheckDescription_FileNamesAndPaths,
        //        CheckTypes.MissingFiles => CommonResources.CheckDescription_MissingFiles,
        //        CheckTypes.PropertySetsDoNotMatch => CommonResources.CheckDescription_PropertySetsDoNotMatch,
        //        CheckTypes.PropertiesDoNotMatch => CommonResources.CheckDescription_PropertiesDoNotMatch,
        //        CheckTypes.PropertySetsNotAttached => CommonResources.CheckDescription_PropertySetsNotAttached,
        //        CheckTypes.EmptyPropertyValues => CommonResources.CheckDescription_EmptyPropertyValues,
        //        CheckTypes.PlaceholderValues => CommonResources.CheckDescription_PlaceholderValues,
        //        CheckTypes.ListDefinitionValues => CommonResources.CheckDescription_ListDefinitionValues,
        //        CheckTypes.ClassificationDefinitionValues => CommonResources.CheckDescription_ClassificationDefinitionValues,
        //        CheckTypes.FileNameFormat => CommonResources.CheckDescription_FileNameFormat,
        //        _ => MCResources.Warning_InvalidCheckType,
        //    };
        //}

        //internal static string GetPrebuiltTitle(CheckTypes checkType)
        //{
        //    return checkType switch
        //    {
        //        CheckTypes.Custom => MCResources.CheckTitle_Custom,
        //        CheckTypes.FileSize => MCResources.CheckTitle_FileSize,
        //        CheckTypes.UnusedElements => MCResources.CheckTitle_Unused,
        //        CheckTypes.AreaSpaceSchemes => MCResources.CheckTitle_AreaSpace,
        //        CheckTypes.VolumeComputations => MCResources.CheckTitle_VolumeComputations,
        //        CheckTypes.LineStyles => MCResources.CheckTitle_LineStyles,
        //        CheckTypes.ObjectStyles => MCResources.CheckTitle_ObjectStyles,
        //        CheckTypes.ProjectInformation => MCResources.CheckTitle_ProjectInfo,
        //        CheckTypes.WarningCount => MCResources.CheckTitle_WarningCount,
        //        CheckTypes.DesignOptions => MCResources.CheckTitle_DesignOptions,
        //        CheckTypes.RasterImages => MCResources.CheckTitle_RasterImages,
        //        CheckTypes.ViewsNotOnSheets => MCResources.CheckTitle_ViewsNotOnSheets,
        //        CheckTypes.ViewsWithHiddenElements => MCResources.CheckTitle_ViewsHiddenElements,
        //        CheckTypes.Worksets => MCResources.CheckTitle_Worksets,
        //        CheckTypes.WorksetPlacement => MCResources.CheckTitle_WorksetPlacement,
        //        CheckTypes.FileName => MCResources.CheckTitle_FileName,
        //        CheckTypes.LinkedCad => MCResources.CheckTitle_LinkedCad,
        //        CheckTypes.PinnedLinks => MCResources.CheckTitle_PinnedLinks,
        //        CheckTypes.LinkMethod => MCResources.CheckTitle_LinkMethod,
        //        CheckTypes.BrowserOrganization => MCResources.CheckTitle_BrowserOrganization,
        //        CheckTypes.PhaseElements => MCResources.CheckTitle_PhaseElements,
        //        CheckTypes.Keynotes => MCResources.CheckTitle_Keynotes,
        //        CheckTypes.Legends => MCResources.CheckTitle_Legends,
        //        CheckTypes.RevitVersion => MCResources.CheckTitle_RevitVersion,
        //        CheckTypes.SharedParametersFile => MCResources.CheckTitle_SharedParameters,
        //        CheckTypes.ProjectCoordinates => MCResources.CheckTitle_ProjectCoordinates,
        //        CheckTypes.ImportedSkp => MCResources.CheckTitle_ImportedSkp,
        //        CheckTypes.TenLargestFamilies => MCResources.CheckTitle_LargestFamilies,
        //        CheckTypes.FileNamesAndPaths => CommonResources.CheckTitle_FileNamesAndPaths,
        //        CheckTypes.MissingFiles => CommonResources.CheckTitle_MissingFiles,
        //        CheckTypes.PropertySetsDoNotMatch => CommonResources.CheckTitle_PropertySetsDoNotMatch,
        //        CheckTypes.PropertiesDoNotMatch => CommonResources.CheckTitle_PropertiesDoNotMatch,
        //        CheckTypes.PropertySetsNotAttached => CommonResources.CheckTitle_PropertySetsNotAttached,
        //        CheckTypes.EmptyPropertyValues => CommonResources.CheckTitle_EmptyPropertyValues,
        //        CheckTypes.PlaceholderValues => CommonResources.CheckTitle_PlaceholderValues,
        //        CheckTypes.ListDefinitionValues => CommonResources.CheckTitle_ListDefinitionValues,
        //        CheckTypes.ClassificationDefinitionValues => CommonResources.CheckTitle_ClassificationDefinitionValues,
        //        CheckTypes.FileNameFormat => CommonResources.CheckTitle_FileNameFormat,
        //        _ => MCResources.Warning_InvalidCheckType,
        //    };
        //}

        internal static bool IsConditionNumeric(Conditions condition)
        {
            if (condition != 0 && condition != Conditions.GreaterOrEqual && condition != Conditions.GreaterThan && condition != Conditions.LessOrEqual && condition != Conditions.LessThan)
            {
                return condition == Conditions.NotEqual;
            }
            return true;
        }

        internal static bool ConditionRequiresNumeric(Conditions condition)
        {
            if (condition != Conditions.GreaterOrEqual && condition != Conditions.GreaterThan && condition != Conditions.LessOrEqual)
            {
                return condition == Conditions.LessThan;
            }
            return true;
        }
    }
}
