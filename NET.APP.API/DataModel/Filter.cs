using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.APP.API.DataModel
{
    public class Filter : ObservableObject
    {
        //private Check _check;

        private bool _selected;

        private bool _userDefined;

        private Enums.Operators _op;

        private Enums.Categories _cat;

        private string _prop = string.Empty;

        private Enums.Conditions _cond;

        private string _val = string.Empty;

        private string _fieldTitle;

        private Enums.ValidationTypes _validation;

        private Enums.UnitClasses _unitClass;

        private Enums.Units _unit;

        private int _indentLevel;

        private bool _caseInsensitive;

        /// <summary>
        /// The ID of this filter
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The check this filter belongs to
        /// </summary>
        //public Check Check
        //{
        //    get
        //    {
        //        return _check;
        //    }
        //    set
        //    {
        //        if (_check != value)
        //        {
        //            _check = value;
        //            OnPropertyChanged("Check");
        //        }
        //    }
        //}

        /// <summary>
        /// The operator for this filter
        /// </summary>
        public Enums.Operators Operator
        {
            get
            {
                return _op;
            }
            set
            {
                if (_op != value)
                {
                    _op = value;
                    OnPropertyChanged("Operator");
                    OnPropertyChanged("ValueOptions");
                }
            }
        }

        /// <summary>
        /// The category of this filter
        /// </summary>
        public Enums.Categories Category
        {
            get
            {
                return _cat;
            }
            set
            {
                if (_cat != value)
                {
                    _cat = value;
                    OnPropertyChanged("Category");
                    OnPropertyChanged("PropertyOptions");
                    OnPropertyChanged("ValueOptions");
                    OnPropertyChanged("AllowCustomProperty");
                    OnPropertyChanged("AllowCustomValue");
                    OnPropertyChanged("DisplayText");
                    OnPropertyChanged("ConditionOptions");
                    OnPropertyChanged("ShowUnitDesignators");
                    OnPropertyChanged("AllowCaseInsensitive");
                    if (!PropertyOptions.Contains(Property) && PropertyOptions.Count > 0 && !AllowCustomProperty)
                    {
                        Property = PropertyOptions[0];
                    }
                }
            }
        }

        /// <summary>
        /// The property this filter works on
        /// </summary>
        public string Property
        {
            get
            {
                return _prop;
            }
            set
            {
                if (_prop == value)
                {
                    return;
                }
                _prop = value.Trim();
                OnPropertyChanged("ValueOptions");
                OnPropertyChanged("ConditionOptions");
                OnPropertyChanged("Property");
                OnPropertyChanged("AllowCustomValue");
                OnPropertyChanged("DisplayText");
                OnPropertyChanged("AllowCaseInsensitive");
                if (!ConditionOptions.Contains(Condition) && ConditionOptions.Count > 0)
                {
                    Condition = Enum.GetValues(typeof(Enums.Conditions)).Cast<Enums.Conditions>().FirstOrDefault((Enums.Conditions x) => x == ConditionOptions[0]);
                }
                OnPropertyChanged("Condition");
                //if (!ValueOptions.Contains(Value) && ValueOptions.Count > 0 && !AllowCustomValue)
                //{
                //    Value = ValueOptions[0];
                //}
            }
        }

        /// <summary>
        /// The condition for combining filters
        /// </summary>
        public Enums.Conditions Condition
        {
            get
            {
                return _cond;
            }
            set
            {
                if (_cond != value)
                {
                    _cond = value;
                    OnPropertyChanged("Condition");
                    OnPropertyChanged("ValueOptions");
                    OnPropertyChanged("AllowCustomValue");
                    OnPropertyChanged("DisplayText");
                    OnPropertyChanged("ShowUnitDesignators");
                    OnPropertyChanged("AllowCaseInsensitive");
                    //if (!ValueOptions.Contains(Value) && ValueOptions.Count > 0 && !AllowCustomValue)
                    //{
                    //    Value = ValueOptions[0];
                    //}
                }
            }
        }

        /// <summary>
        /// The filter value
        /// </summary>
        public string Value
        {
            get
            {
                return _val;
            }
            set
            {
                if (!(_val == value))
                {
                    _val = value;
                    OnPropertyChanged("Value");
                    OnPropertyChanged("DisplayText");
                    OnPropertyChanged("ConvertedValue");
                }
            }
        }

        /// <summary>
        /// The converted filter value
        /// </summary>
        //public string ConvertedValue
        //{
        //    get
        //    {
        //        if (Unit == Enums.Units.None)
        //        {
        //            return Value;
        //        }
        //        if (!double.TryParse(Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        //        {
        //            return string.Empty;
        //        }
        //        return Math.Round(UnitConverter.ConvertFromDbUnits(result, Unit, UnitClass), 4).ToString(CultureInfo.CurrentCulture);
        //    }
        //    set
        //    {
        //        if (Unit == Enums.Units.None)
        //        {
        //            Value = value;
        //            return;
        //        }
        //        double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out var result);
        //        Value = UnitConverter.ConvertToDbUnits(result, Unit, UnitClass).ToString(CultureInfo.InvariantCulture);
        //    }
        //}

        /// <summary>
        /// The text to display for this filter
        /// </summary>
        //public string DisplayText
        //{
        //    get
        //    {
        //        string empty = string.Empty;
        //        empty += (FieldIsUserDefined ? MCResources.Label_ValueFromUser : MCResources.Label_ValueFromCode);
        //        empty += ConvertedValue.GetDisplayValue();
        //        if (Unit != 0)
        //        {
        //            empty = empty + ((Unit == Enums.Units.Inches || Unit == Enums.Units.Feet) ? string.Empty : " ") + UnitConverter.GetUnitTag(Unit, UnitClass);
        //        }
        //        return empty;
        //    }
        //}

        /// <summary>
        /// The validation type of this filter
        /// </summary>
        public Enums.ValidationTypes Validation
        {
            get
            {
                return _validation;
            }
            set
            {
                if (_validation != value)
                {
                    _validation = value;
                    OnPropertyChanged("Validation");
                    OnPropertyChanged("FieldIsTextbox");
                    OnPropertyChanged("FieldIsCheckbox");
                }
            }
        }

        /// <summary>
        /// If the field for this filter is user defined
        /// </summary>
        public bool FieldIsUserDefined
        {
            get
            {
                return _userDefined;
            }
            set
            {
                if (_userDefined != value)
                {
                    _userDefined = value;
                    OnPropertyChanged("FieldIsUserDefined");
                    OnPropertyChanged("DisplayText");
                    OnPropertyChanged("ShowUnitDesignators");
                }
            }
        }

        /// <summary>
        /// If the field should be displayed as a checkbox
        /// </summary>
        public bool FieldIsCheckbox => _validation == Enums.ValidationTypes.Boolean;

        /// <summary>
        /// If the field should be displayed as a text box
        /// </summary>
        public bool FieldIsTextbox => _validation != Enums.ValidationTypes.Boolean;

        /// <summary>
        /// The title of the field associated with this check - only relevant if the check has
        /// a field associated with it (check the HasField property)
        /// </summary>
        public string FieldTitle
        {
            get
            {
                return _fieldTitle;
            }
            set
            {
                if (!(_fieldTitle == value))
                {
                    _fieldTitle = value;
                    OnPropertyChanged("FieldTitle");
                }
            }
        }

        /// <summary>
        /// If the operator should be shown for this filter
        /// </summary>
       // public bool ShowOperator => _check.Filters.IndexOf(this) != 0;

        /// <summary>
        /// If this filter allows custom properties
        /// </summary>
        public bool AllowCustomProperty
        {
            get
            {
                if (Category != Enums.Categories.Parameter && Category != 0 && Category != Enums.Categories.PhaseStatus && Category != Enums.Categories.APIParameter)
                {
                    return Category == Enums.Categories.HostParameter;
                }
                return true;
            }
        }

        /// <summary>
        /// If this filter allows custom values
        /// </summary>
        public bool AllowCustomValue
        {
            get
            {
                if (Category == Enums.Categories.Category && Property != "Name")
                {
                    return false;
                }
                if (Category == Enums.Categories.TypeOrInstance || Category == Enums.Categories.PhaseStatus || Category == Enums.Categories.StructuralType || Category == Enums.Categories.Redundant)
                {
                    return false;
                }
                if (Condition == Enums.Conditions.Defined || Condition == Enums.Conditions.Undefined || Condition == Enums.Conditions.HasValue || Condition == Enums.Conditions.HasNoValue || Condition == Enums.Conditions.Duplicated)
                {
                    return false;
                }
                if (Property == "Is In Place" || Property == "Is Defined")
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// If case insensitive setting is allowed
        /// </summary>
        public bool AllowCaseInsensitive
        {
            get
            {
                if (UnitClass == Enums.UnitClasses.None && AllowCustomValue)
                {
                    if (Condition != 0 && Condition != Enums.Conditions.NotEqual && Condition != Enums.Conditions.Contains)
                    {
                        return Condition == Enums.Conditions.DoesNotContain;
                    }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// If comparisons are case insensitive
        /// </summary>
        public bool CaseInsensitive
        {
            get
            {
                return _caseInsensitive;
            }
            set
            {
                if (_caseInsensitive != value)
                {
                    _caseInsensitive = value;
                    OnPropertyChanged("CaseInsensitive");
                }
            }
        }

        /// <summary>
        /// If this filter is currently selected
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// Currently available options for conditions
        /// </summary>
        public List<Enums.Conditions> ConditionOptions
        {
            get
            {
                List<Enums.Conditions> list = new List<Enums.Conditions> { Enums.Conditions.Equal };
                switch (Category)
                {
                    case Enums.Categories.Room:
                    case Enums.Categories.Space:
                        list.Add(Enums.Conditions.NotEqual);
                        if (Property == "Is Defined")
                        {
                            return list;
                        }
                        list.Add(Enums.Conditions.Contains);
                        list.Add(Enums.Conditions.DoesNotContain);
                        list.Add(Enums.Conditions.Defined);
                        list.Add(Enums.Conditions.Undefined);
                        list.Add(Enums.Conditions.WildCard);
                        list.Add(Enums.Conditions.WildCardNoMatch);
                        return list;
                    case Enums.Categories.Category:
                        if (Property == "Name")
                        {
                            list.Add(Enums.Conditions.NotEqual);
                            list.Add(Enums.Conditions.Contains);
                            list.Add(Enums.Conditions.DoesNotContain);
                        }
                        else
                        {
                            list.Clear();
                            list.Add(Enums.Conditions.Included);
                        }
                        return list;
                    case Enums.Categories.PhaseDemolished:
                    case Enums.Categories.DesignOption:
                        list.Add(Enums.Conditions.NotEqual);
                        list.Add(Enums.Conditions.Contains);
                        list.Add(Enums.Conditions.DoesNotContain);
                        list.Add(Enums.Conditions.Defined);
                        list.Add(Enums.Conditions.Undefined);
                        list.Add(Enums.Conditions.WildCard);
                        list.Add(Enums.Conditions.WildCardNoMatch);
                        return list;
                    case Enums.Categories.View:
                        if (Property == "Is Defined")
                        {
                            list.Add(Enums.Conditions.NotEqual);
                        }
                        else
                        {
                            list.Add(Enums.Conditions.NotEqual);
                            list.Add(Enums.Conditions.Contains);
                            list.Add(Enums.Conditions.DoesNotContain);
                            list.Add(Enums.Conditions.Defined);
                            list.Add(Enums.Conditions.Undefined);
                            list.Add(Enums.Conditions.WildCard);
                            list.Add(Enums.Conditions.WildCardNoMatch);
                        }
                        return list;
                    case Enums.Categories.PhaseCreated:
                        list.Add(Enums.Conditions.NotEqual);
                        list.Add(Enums.Conditions.Contains);
                        list.Add(Enums.Conditions.DoesNotContain);
                        list.Add(Enums.Conditions.Defined);
                        list.Add(Enums.Conditions.WildCard);
                        list.Add(Enums.Conditions.WildCardNoMatch);
                        return list;
                    case Enums.Categories.Family:
                        list.Add(Enums.Conditions.NotEqual);
                        if (Property == "Is In Place")
                        {
                            return list;
                        }
                        list.Add(Enums.Conditions.Contains);
                        list.Add(Enums.Conditions.DoesNotContain);
                        list.Add(Enums.Conditions.WildCard);
                        list.Add(Enums.Conditions.WildCardNoMatch);
                        return list;
                    case Enums.Categories.Workset:
                    case Enums.Categories.Type:
                        list.Add(Enums.Conditions.NotEqual);
                        list.Add(Enums.Conditions.Contains);
                        list.Add(Enums.Conditions.DoesNotContain);
                        list.Add(Enums.Conditions.WildCard);
                        list.Add(Enums.Conditions.WildCardNoMatch);
                        return list;
                    case Enums.Categories.TypeOrInstance:
                        return list;
                    case Enums.Categories.Level:
                        list.Add(Enums.Conditions.NotEqual);
                        if (Property == "Name")
                        {
                            list.Add(Enums.Conditions.Contains);
                            list.Add(Enums.Conditions.DoesNotContain);
                            list.Add(Enums.Conditions.Defined);
                            list.Add(Enums.Conditions.Undefined);
                            list.Add(Enums.Conditions.WildCard);
                            list.Add(Enums.Conditions.WildCardNoMatch);
                        }
                        else
                        {
                            list.Add(Enums.Conditions.GreaterThan);
                            list.Add(Enums.Conditions.LessThan);
                            list.Add(Enums.Conditions.GreaterOrEqual);
                            list.Add(Enums.Conditions.LessOrEqual);
                        }
                        return list;
                    case Enums.Categories.Parameter:
                    case Enums.Categories.APIParameter:
                    case Enums.Categories.HostParameter:
                        list.Add(Enums.Conditions.NotEqual);
                        list.Add(Enums.Conditions.GreaterThan);
                        list.Add(Enums.Conditions.LessThan);
                        list.Add(Enums.Conditions.GreaterOrEqual);
                        list.Add(Enums.Conditions.LessOrEqual);
                        list.Add(Enums.Conditions.Contains);
                        list.Add(Enums.Conditions.DoesNotContain);
                        list.Add(Enums.Conditions.Defined);
                        list.Add(Enums.Conditions.Undefined);
                        list.Add(Enums.Conditions.HasValue);
                        list.Add(Enums.Conditions.HasNoValue);
                        list.Add(Enums.Conditions.WildCard);
                        list.Add(Enums.Conditions.WildCardNoMatch);
                        list.Add(Enums.Conditions.Duplicated);
                        if (Category == Enums.Categories.APIParameter)
                        {
                            return list;
                        }
                        list.Add(Enums.Conditions.MatchesParameter);
                        list.Add(Enums.Conditions.DoesNotMatchParameter);
                        return list;
                    case Enums.Categories.PhaseStatus:
                    case Enums.Categories.StructuralType:
                        list.Add(Enums.Conditions.NotEqual);
                        return list;
                    case Enums.Categories.APIType:
                    case Enums.Categories.Redundant:
                    case Enums.Categories.Host:
                        return list;
                    default:
                        return Enum.GetValues(typeof(Enums.Conditions)).Cast<Enums.Conditions>().ToList();
                }
            }
        }

        /// <summary>
        /// Currently available options for properties
        /// </summary>
        public List<string> PropertyOptions
        {
            get
            {
                List<string> list = new List<string>();
                switch (Category)
                {
                    case Enums.Categories.Room:
                    case Enums.Categories.Space:
                        list.Add("Name");
                        list.Add("Number");
                        list.Add("Is Defined");
                        return list;
                    case Enums.Categories.Category:
                        list.Add("Name");
                        //list.AddRange(SharedData.BuiltInCategoryValues);
                        return list;
                    case Enums.Categories.PhaseCreated:
                    case Enums.Categories.PhaseDemolished:
                    case Enums.Categories.DesignOption:
                    case Enums.Categories.Workset:
                    case Enums.Categories.Type:
                        list.Add("Name");
                        return list;
                    case Enums.Categories.View:
                        list.Add("Name");
                        list.Add("Is Defined");
                        break;
                    case Enums.Categories.Family:
                        list.Add("Name");
                        list.Add("Is In Place");
                        return list;
                    case Enums.Categories.TypeOrInstance:
                        list.Add("Is Element Type");
                        return list;
                    case Enums.Categories.Level:
                        list.Add("Name");
                        list.Add("Elevation");
                        return list;
                    case Enums.Categories.Parameter:
                    case Enums.Categories.HostParameter:
                        //list.AddRange(SharedData.BuiltInParameterValues);
                        return list;
                    case Enums.Categories.PhaseStatus:
                    case Enums.Categories.APIParameter:
                        return list;
                    case Enums.Categories.StructuralType:
                        list.Add("Value");
                        return list;
                    case Enums.Categories.APIType:
                        list.Add("Full Class Name");
                        return list;
                    case Enums.Categories.Redundant:
                        list.Add("Location");
                        list.Add("Type");
                        return list;
                    case Enums.Categories.Host:
                        list.Add("Is Defined");
                        return list;
                }
                return list;
            }
        }

        ///// <summary>
        ///// Currently available options for values
        ///// </summary>
        //public List<string> ValueOptions
        //{
        //    get
        //    {
        //        List<string> list = new List<string>();
        //        if (Category == Enums.Categories.TypeOrInstance || Property == "Is In Place" || Property == "Is Defined" || Condition == Enums.Conditions.Defined || Condition == Enums.Conditions.HasValue || Condition == Enums.Conditions.HasNoValue || Condition == Enums.Conditions.Undefined || Condition == Enums.Conditions.Duplicated || Condition == Enums.Conditions.Included || Category == Enums.Categories.Redundant)
        //        {
        //            list.Add("True");
        //            list.Add("False");
        //        }
        //        if (Category == Enums.Categories.PhaseStatus)
        //        {
        //            list.Add("Existing");
        //            list.Add("New");
        //            list.Add("Demolished");
        //            list.Add("Temporary");
        //            list.Add("Future");
        //        }
        //        else if (Category == Enums.Categories.StructuralType)
        //        {
        //            list.AddRange(SharedData.StructuralTypeValues);
        //        }
        //        if (Condition == Enums.Conditions.MatchesParameter || Condition == Enums.Conditions.DoesNotMatchParameter)
        //        {
        //            list.AddRange(SharedData.BuiltInParameterValues);
        //        }
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// If this filter can move up in the display list
        ///// </summary>
        //public bool CanMoveUp => Check.Filters.IndexOf(this) > 0;

        ///// <summary>
        ///// If this filter can move down in the display list
        ///// </summary>
        //public bool CanMoveDown => Check.Filters.IndexOf(this) < Check.Filters.Count - 1;

        /// <summary>
        /// The unit for this filter
        /// </summary>
        public Enums.Units Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged("Unit");
                    OnPropertyChanged("ConvertedValue");
                    OnPropertyChanged("DisplayText");
                    OnPropertyChanged("UnitDisplay");
                }
            }
        }

        /// <summary>
        /// The unit class for this filter
        /// </summary>
        public Enums.UnitClasses UnitClass
        {
            get
            {
                return _unitClass;
            }
            set
            {
                if (_unitClass != value)
                {
                    _unitClass = value;
                    OnPropertyChanged("UnitClass");
                    OnPropertyChanged("UnitClassDisplay");
                    OnPropertyChanged("UnitOptions");
                    OnPropertyChanged("Unit");
                    OnPropertyChanged("UnitDisplay");
                    OnPropertyChanged("AllowUnitSelection");
                    OnPropertyChanged("ConvertedValue");
                    OnPropertyChanged("DisplayText");
                    OnPropertyChanged("AllowCaseInsensitive");
                }
            }
        }

        ///// <summary>
        ///// The display value for units
        ///// </summary>
        //public string UnitDisplay
        //{
        //    get
        //    {
        //        return Enums.GetUnitDisplay(Unit);
        //    }
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            return;
        //        }
        //        Enums.Units units = Enum.GetValues(typeof(Enums.Units)).Cast<Enums.Units>().FirstOrDefault((Enums.Units x) => Enums.GetUnitDisplay(x) == value);
        //        if (_unit != units)
        //        {
        //            if (_unit == Enums.Units.Default && double.TryParse(Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        //            {
        //                Value = UnitConverter.ConvertToDbUnits(result, units).ToString(CultureInfo.InvariantCulture);
        //            }
        //            Unit = units;
        //            OnPropertyChanged("UnitDisplay");
        //        }
        //    }
        //}

        ///// <summary>
        ///// The display value for unit class
        ///// </summary>
        //public string UnitClassDisplay
        //{
        //    get
        //    {
        //        return Enums.GetUnitClassDisplay(UnitClass);
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            Enums.UnitClasses unitClasses = Enum.GetValues(typeof(Enums.UnitClasses)).Cast<Enums.UnitClasses>().FirstOrDefault((Enums.UnitClasses x) => Enums.GetUnitClassDisplay(x) == value);
        //            if (unitClasses != _unitClass)
        //            {
        //                UnitClass = unitClasses;
        //                Unit = Enums.Units.Default;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// If unit selection is allowed
        /// </summary>
        public bool AllowUnitSelection => UnitClass != Enums.UnitClasses.None;

        /// <summary>
        /// If unit designators are to be shown
        /// </summary>
        public bool ShowUnitDesignators
        {
            get
            {
                if ((Category == Enums.Categories.Parameter || Category == Enums.Categories.HostParameter) && !FieldIsUserDefined)
                {
                    return Enums.IsConditionNumeric(Condition);
                }
                return false;
            }
        }

        /// <summary>
        /// The currently available options for units
        /// </summary>
        //public List<string> UnitOptions => UnitConverter.GetValidUnits(_unitClass).Select(Enums.GetUnitDisplay).ToList();

        /// <summary>
        /// The UI indent level of this filter
        /// </summary>
        public int IndentLevel
        {
            get
            {
                return _indentLevel;
            }
            set
            {
                if (_indentLevel != value)
                {
                    _indentLevel = value;
                    OnPropertyChanged("IndentLevel");
                }
            }
        }

        ///// <summary>
        ///// Class Constructor
        ///// </summary>
        ///// <param name="ck">The Check that this filter applies to.</param>
        ///// <param name="idOverride">If passed, the ID of the filter will be set to the passed value.  This should be used for deserializing filters.</param>
        //public Filter(Check ck, string idOverride = "")
        //{
        //    _check = ck ?? throw new ArgumentNullException("ck");
        //    if (PropertyOptions.Count > 0)
        //    {
        //        Property = PropertyOptions[0];
        //    }
        //    if (ValueOptions.Count > 0)
        //    {
        //        Value = ValueOptions[0];
        //    }
        //    Id = (string.IsNullOrEmpty(idOverride) ? Guid.NewGuid() : new Guid(idOverride));
        //}

        ///// <summary>
        ///// Create a duplicate of this filter that is a separate object but has the same values
        ///// </summary>
        ///// <returns></returns>
        //public Filter Clone(bool newId = true)
        //{
        //    return new Filter(Check, newId ? string.Empty : Id.ToString())
        //    {
        //        Category = Category,
        //        Condition = Condition,
        //        FieldIsUserDefined = FieldIsUserDefined,
        //        FieldTitle = FieldTitle,
        //        Operator = Operator,
        //        Property = Property,
        //        Validation = Validation,
        //        Value = Value,
        //        UnitClass = UnitClass,
        //        Unit = Unit,
        //        CaseInsensitive = CaseInsensitive
        //    };
        //}

        internal void NotifyOrderChanged()
        {
            OnPropertyChanged("CanMoveUp");
            OnPropertyChanged("CanMoveDown");
        }
    }
}
