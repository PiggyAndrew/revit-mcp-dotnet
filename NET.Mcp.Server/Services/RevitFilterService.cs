using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using ModelContextProtocol.Protocol;
using NET.APP.API.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NET.APP.API.Services
{

    /// <summary>
    /// Interface implementation for providing Revit <see cref="T:ElementFilter" /> objects corresponding to Model Checker <see cref="T:ADSK.AIT.ModelChecker.API.DataModel.Filter" /> objects.
    /// </summary>
    public class RevitFilterService 
    {
        private readonly Document _doc;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="doc">The <see cref="T:Autodesk.Revit.DB.Document" /> to use to construct filters.</param>
        public RevitFilterService(Document doc)
        {
            _doc = doc ?? throw new ArgumentNullException("doc");
        }

        /// <inheritdoc />
        public ElementFilter GetCategoryFilter(Filter flt)
        {
            if (flt.Property != "Name")
            {
                return null;
            }
            switch (flt.Condition)
            {
                case Enums.Conditions.Equal:
                    {
                        Category val = (flt.CaseInsensitive ? ((IEnumerable)_doc.Settings.Categories).Cast<Category>().FirstOrDefault((Category x) => !x.Name.ToLower().Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase)) : ((CategoryNameMap)_doc.Settings.Categories)[flt.Value]);
                        if (val == null)
                        {
                            //flt.Check.CheckResult.ErrorMessage = string.Format(MCResources.Warning_CategoryNotFound, flt.Value);
                            break;
                        }
                        return (ElementFilter)new ElementCategoryFilter(val.Id);
                    }
                case Enums.Conditions.NotEqual:
                    {
                        List<ElementId> list3 = (from Category x in (IEnumerable)_doc.Settings.Categories
                                                 where (!flt.CaseInsensitive) ? (x.Name != flt.Value) : (!x.Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase))
                                                 select x.Id).ToList();
                        if (list3.Count > 0)
                        {
                            return (ElementFilter)new ElementMulticategoryFilter((ICollection<ElementId>)list3);
                        }
                        //flt.Check.CheckResult.ErrorMessage = string.Format(MCResources.Warning_NoCategoriesFoundWithName, flt.Value);
                        break;
                    }
                case Enums.Conditions.Contains:
                    {
                        List<ElementId> list2 = (from Category x in (IEnumerable)_doc.Settings.Categories
                                                 where (!flt.CaseInsensitive) ? x.Name.Contains(flt.Value) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(flt.Value, x.Name, CompareOptions.IgnoreCase) >= 0)
                                                 select x.Id).ToList();
                        if (list2.Count <= 0)
                        {
                            return GetNoElementsFilter();
                        }
                        return (ElementFilter)new ElementMulticategoryFilter((ICollection<ElementId>)list2);
                    }
                case Enums.Conditions.DoesNotContain:
                    {
                        List<ElementId> list = (from Category x in (IEnumerable)_doc.Settings.Categories
                                                where (!flt.CaseInsensitive) ? (!x.Name.Contains(flt.Value)) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(x.Name, flt.Value, CompareOptions.IgnoreCase) < 0)
                                                select x.Id).ToList();
                        if (list.Count <= 0)
                        {
                            return GetNoElementsFilter();
                        }
                        return (ElementFilter)new ElementMulticategoryFilter((ICollection<ElementId>)list);
                    }
            }
            return null;
        }

        /// <inheritdoc />
        public ElementFilter GetDesignOptionFilter(Filter flt)
        {
            List<DesignOption> list = ((IEnumerable)new FilteredElementCollector(_doc).OfClass(typeof(DesignOption))).Cast<DesignOption>().ToList();
            switch (flt.Condition)
            {
                case Enums.Conditions.Equal:
                case Enums.Conditions.NotEqual:
                    {
                        List<DesignOption> list5 = list.Where(delegate (DesignOption x)
                        {
                            string text2 = ((Element)x).Name.Replace(Resources.Label_DesignOptionPrimaryTag, string.Empty).Trim();
                            return (!flt.CaseInsensitive) ? (text2 == flt.Value.Trim()) : text2.Equals(flt.Value.Trim(), StringComparison.CurrentCultureIgnoreCase);
                        }).ToList();
                        if (!list5.Any())
                        {
                            return GetNoElementsFilter();
                        }
                        if (list5.Count != 1)
                        {
                            return (ElementFilter)new LogicalAndFilter((IList<ElementFilter>)((IEnumerable<DesignOption>)list5).Select((Func<DesignOption, ElementDesignOptionFilter>)((DesignOption x) => new ElementDesignOptionFilter(((Element)x).Id, flt.Condition != Enums.Conditions.Equal))).Cast<ElementFilter>().ToList());
                        }
                        return (ElementFilter)new ElementDesignOptionFilter(((Element)list5[0]).Id, flt.Condition != Enums.Conditions.Equal);
                    }
                case Enums.Conditions.Contains:
                    {
                        List<ElementFilter> list6 = list.Where(delegate (DesignOption x)
                        {
                            string text3 = ((Element)x).Name.Replace(MCResources.Label_DesignOptionPrimaryTag, string.Empty).Trim();
                            return (!flt.CaseInsensitive) ? text3.Contains(flt.Value) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(text3, flt.Value, CompareOptions.IgnoreCase) >= 0);
                        }).Select((Func<DesignOption, ElementDesignOptionFilter>)((DesignOption x) => new ElementDesignOptionFilter(((Element)x).Id))).Cast<ElementFilter>()
                            .ToList();
                        if (list6.Count <= 0)
                        {
                            return GetNoElementsFilter();
                        }
                        return (ElementFilter)new LogicalOrFilter((IList<ElementFilter>)list6);
                    }
                case Enums.Conditions.DoesNotContain:
                    {
                        List<ElementFilter> list4 = list.Where(delegate (DesignOption x)
                        {
                            string text = ((Element)x).Name.Replace(MCResources.Label_DesignOptionPrimaryTag, string.Empty).Trim();
                            return (!flt.CaseInsensitive) ? (!text.Contains(flt.Value)) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(text, flt.Value, CompareOptions.IgnoreCase) < 0);
                        }).Select((Func<DesignOption, ElementDesignOptionFilter>)((DesignOption x) => new ElementDesignOptionFilter(((Element)x).Id))).Cast<ElementFilter>()
                            .ToList();
                        if (list4.Count <= 0)
                        {
                            return GetNoElementsFilter();
                        }
                        return (ElementFilter)new LogicalOrFilter((IList<ElementFilter>)list4);
                    }
                case Enums.Conditions.Defined:
                    {
                        List<ElementFilter> list3 = new List<ElementFilter>();
                        foreach (DesignOption item in list)
                        {
                            list3.Add((ElementFilter)new ElementDesignOptionFilter(((Element)item).Id));
                        }
                        if (list3.Count <= 0)
                        {
                            return GetNoElementsFilter();
                        }
                        return (ElementFilter)new LogicalOrFilter((IList<ElementFilter>)list3);
                    }
                case Enums.Conditions.Undefined:
                    {
                        ParameterValueProvider val = new ParameterValueProvider(new ElementId((BuiltInParameter)(-1013201)));
                        FilterNumericRuleEvaluator val2 = (FilterNumericRuleEvaluator)new FilterNumericEquals();
                        return (ElementFilter)new ElementParameterFilter((FilterRule)new FilterElementIdRule((FilterableValueProvider)val, val2, ElementId.InvalidElementId));
                    }
                case Enums.Conditions.WildCard:
                case Enums.Conditions.WildCardNoMatch:
                    {
                        Regex re = new Regex(flt.Value);
                        List<ElementFilter> list2 = new List<ElementFilter>();
                        foreach (DesignOption item2 in list.Where((DesignOption x) => (flt.Condition != Enums.Conditions.WildCard) ? (!re.IsMatch(((Element)x).Name.Replace(MCResources.Label_DesignOptionPrimaryTag, string.Empty).Trim())) : re.IsMatch(((Element)x).Name.Replace(MCResources.Label_DesignOptionPrimaryTag, string.Empty).Trim())))
                        {
                            list2.Add((ElementFilter)new ElementDesignOptionFilter(((Element)item2).Id));
                        }
                        if (list2.Count <= 0)
                        {
                            return GetNoElementsFilter();
                        }
                        return (ElementFilter)new LogicalAndFilter((IList<ElementFilter>)list2);
                    }
                default:
                    return null;
            }
        }

        /// <inheritdoc />
        public ElementFilter GetFamilyFilter(Filter flt)
        {
            if (flt.Property == "Is In Place")
            {
                return null;
            }
            List<Element> source = ((IEnumerable<Element>)new FilteredElementCollector(_doc).OfClass(typeof(Family))).ToList();
            List<Family> list = null;
            switch (flt.Condition)
            {
                case Enums.Conditions.Equal:
                    list = source.Where((Element x) => (!flt.CaseInsensitive) ? (x.Name == flt.Value) : x.Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase)).Cast<Family>().ToList();
                    break;
                case Enums.Conditions.NotEqual:
                    list = source.Where((Element x) => (!flt.CaseInsensitive) ? (x.Name != flt.Value) : (!x.Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase))).Cast<Family>().ToList();
                    break;
                case Enums.Conditions.Contains:
                    list = source.Where((Element x) => (!flt.CaseInsensitive) ? x.Name.Contains(flt.Value) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(x.Name, flt.Value, CompareOptions.IgnoreCase) >= 0)).Cast<Family>().ToList();
                    break;
                case Enums.Conditions.DoesNotContain:
                    list = source.Where((Element x) => (!flt.CaseInsensitive) ? (!x.Name.Contains(flt.Value)) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(x.Name, flt.Value, CompareOptions.IgnoreCase) < 0)).Cast<Family>().ToList();
                    break;
                case Enums.Conditions.WildCard:
                case Enums.Conditions.WildCardNoMatch:
                    {
                        Regex re = new Regex(flt.Value);
                        list = source.Where((Element x) => (flt.Condition != Enums.Conditions.WildCard) ? (!re.IsMatch(x.Name)) : re.IsMatch(x.Name)).Cast<Family>().ToList();
                        break;
                    }
            }
            if (list == null || list.Count <= 0)
            {
                return GetNoElementsFilter();
            }
            List<ElementFilter> list2 = new List<ElementFilter>();
            foreach (Family item in list)
            {
                list2.Add((ElementFilter)new FamilySymbolFilter(((Element)item).Id));
                list2.AddRange((IEnumerable<ElementFilter>)((IEnumerable<ElementId>)item.GetFamilySymbolIds()).Select((Func<ElementId, FamilyInstanceFilter>)((ElementId fsID) => new FamilyInstanceFilter(_doc, fsID))));
            }
            return (ElementFilter)(list2.Count switch
            {
                0 => null,
                1 => list2[0],
                _ => (object)new LogicalOrFilter((IList<ElementFilter>)list2),
            });
        }

        /// <inheritdoc />
        //public ElementFilter GetTypeOrInstanceFilter(Filter flt)
        //{
            
        //    if (Constants.TrueValues.Any((string x) => x.ToLower() == flt.Value.ToLower()))
        //    {
        //        return (ElementFilter)new ElementIsElementTypeFilter();
        //    }
        //    return (ElementFilter)new ElementIsElementTypeFilter(true);
        //}

        /// <inheritdoc />
        public ElementFilter GetLevelFilter(Filter flt)
        {
            List<Level> list = ((IEnumerable)new FilteredElementCollector(_doc).OfClass(typeof(Level))).Cast<Level>().ToList();
            List<Level> list2 = null;
            if (flt.Property == "Name")
            {
                switch (flt.Condition)
                {
                    case Enums.Conditions.Equal:
                        list2 = list.Where((Level x) => (!flt.CaseInsensitive) ? (((Element)x).Name == flt.Value) : ((Element)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase)).ToList();
                        break;
                    case Enums.Conditions.NotEqual:
                        list2 = list.Where((Level x) => (!flt.CaseInsensitive) ? (((Element)x).Name != flt.Value) : (!((Element)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase))).ToList();
                        break;
                    case Enums.Conditions.Contains:
                        list2 = list.Where((Level x) => (!flt.CaseInsensitive) ? ((Element)x).Name.Contains(flt.Value) : (CultureInfo.CurrentCulture.CompareInfo.IndexOf(((Element)x).Name, flt.Value) >= 0)).ToList();
                        break;
                    case Enums.Conditions.DoesNotContain:
                        list2 = list.Where((Level x) => (!flt.CaseInsensitive) ? (!((Element)x).Name.Contains(flt.Value)) : (CultureInfo.CurrentCulture.CompareInfo.IndexOf(((Element)x).Name, flt.Value) < 0)).ToList();
                        break;
                    case Enums.Conditions.WildCard:
                    case Enums.Conditions.WildCardNoMatch:
                        {
                            Regex re = new Regex(flt.Value);
                            list2 = list.Where((Level x) => (flt.Condition != Enums.Conditions.WildCard) ? (!re.IsMatch(((Element)x).Name)) : re.IsMatch(((Element)x).Name)).ToList();
                            break;
                        }
                    case Enums.Conditions.Defined:
                        list2 = list;
                        break;
                    case Enums.Conditions.Undefined:
                        {
                            ParameterValueProvider val = new ParameterValueProvider(new ElementId((BuiltInParameter)(-1001952)));
                            FilterNumericRuleEvaluator val2 = (FilterNumericRuleEvaluator)new FilterNumericEquals();
                            return (ElementFilter)new ElementParameterFilter((FilterRule)new FilterElementIdRule((FilterableValueProvider)val, val2, ElementId.InvalidElementId));
                        }
                }
            }
            else
            {
                switch (flt.Condition)
                {
                    case Enums.Conditions.Equal:
                        list2 = list.Where((Level x) => Math.Abs(Math.Round(x.Elevation, 3) - double.Parse(flt.Value, CultureInfo.InvariantCulture)) < 1E-05).ToList();
                        break;
                    case Enums.Conditions.NotEqual:
                        list2 = list.Where((Level x) => Math.Abs(Math.Round(x.Elevation, 3) - double.Parse(flt.Value, CultureInfo.InvariantCulture)) > 1E-05).ToList();
                        break;
                    case Enums.Conditions.GreaterThan:
                        list2 = list.Where((Level x) => Math.Round(x.Elevation, 3) > double.Parse(flt.Value, CultureInfo.InvariantCulture)).ToList();
                        break;
                    case Enums.Conditions.LessThan:
                        list2 = list.Where((Level x) => Math.Round(x.Elevation, 3) < double.Parse(flt.Value, CultureInfo.InvariantCulture)).ToList();
                        break;
                    case Enums.Conditions.GreaterOrEqual:
                        list2 = list.Where((Level x) => Math.Round(x.Elevation, 3) >= double.Parse(flt.Value, CultureInfo.InvariantCulture)).ToList();
                        break;
                    case Enums.Conditions.LessOrEqual:
                        list2 = list.Where((Level x) => Math.Round(x.Elevation, 3) <= double.Parse(flt.Value, CultureInfo.InvariantCulture)).ToList();
                        break;
                }
            }
            if (list2 == null || list2.Count == 0)
            {
                return GetNoElementsFilter();
            }
            List<ElementFilter> list3 = new List<ElementFilter>();
            foreach (Level item in list2)
            {
                list3.Add((ElementFilter)new ElementLevelFilter(((Element)item).Id));
                list3.Add((ElementFilter)(object)GetLevelParamStringFilter(MCResources.Label_LevelSketchPlaneParameterPrefix + ((Element)item).Name, (BuiltInParameter)(-1001380)));
                list3.Add((ElementFilter)(object)GetLevelParamIdFilter(((Element)item).Id, (BuiltInParameter)(-1001383)));
                list3.Add((ElementFilter)(object)GetLevelParamIdFilter(((Element)item).Id, (BuiltInParameter)(-1001365)));
                list3.Add((ElementFilter)(object)GetLevelParamIdFilter(((Element)item).Id, (BuiltInParameter)(-1114000)));
                list3.Add((ElementFilter)(object)GetLevelParamIdFilter(((Element)item).Id, (BuiltInParameter)(-1001715)));
                list3.Add((ElementFilter)(object)GetLevelParamIdFilter(((Element)item).Id, (BuiltInParameter)(-1001651)));
            }
            return (ElementFilter)(list3.Count switch
            {
                0 => null,
                1 => list3[0],
                _ => (object)new LogicalOrFilter((IList<ElementFilter>)list3),
            });
        }

        /// <inheritdoc />
        public ElementFilter GetPhaseFilter(Filter flt)
        {
            List<Phase> list = ((IEnumerable)new FilteredElementCollector(_doc).OfClass(typeof(Phase))).Cast<Phase>().ToList();
            List<Phase> list2 = null;
            switch (flt.Condition)
            {
                case Enums.Conditions.Equal:
                    list2 = list.Where((Phase x) => (!flt.CaseInsensitive) ? (((Element)x).Name == flt.Value) : ((Element)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                case Enums.Conditions.NotEqual:
                    list2 = list.Where((Phase x) => (!flt.CaseInsensitive) ? (((Element)x).Name != flt.Value) : (!((Element)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase))).ToList();
                    break;
                case Enums.Conditions.Contains:
                    list2 = list.Where((Phase x) => (!flt.CaseInsensitive) ? ((Element)x).Name.Contains(flt.Value) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(((Element)x).Name, flt.Value, CompareOptions.IgnoreCase) >= 0)).ToList();
                    break;
                case Enums.Conditions.DoesNotContain:
                    list2 = list.Where((Phase x) => (!flt.CaseInsensitive) ? (!((Element)x).Name.Contains(flt.Value)) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(((Element)x).Name, flt.Value, CompareOptions.IgnoreCase) < 0)).ToList();
                    break;
                case Enums.Conditions.Defined:
                    list2 = list;
                    break;
                case Enums.Conditions.Undefined:
                    {
                        ParameterValueProvider val = new ParameterValueProvider(new ElementId((BuiltInParameter)(-1012101)));
                        FilterNumericRuleEvaluator val2 = (FilterNumericRuleEvaluator)new FilterNumericEquals();
                        return (ElementFilter)new ElementParameterFilter((FilterRule)new FilterElementIdRule((FilterableValueProvider)val, val2, ElementId.InvalidElementId));
                    }
                case Enums.Conditions.WildCard:
                case Enums.Conditions.WildCardNoMatch:
                    {
                        Regex re = new Regex(flt.Value);
                        list2 = list.Where((Phase x) => (flt.Condition != Enums.Conditions.WildCard) ? (!re.IsMatch(((Element)x).Name)) : re.IsMatch(((Element)x).Name)).ToList();
                        break;
                    }
            }
            if (list2 == null || list2.Count == 0)
            {
                return GetNoElementsFilter();
            }
            List<ElementFilter> list3 = new List<ElementFilter>();
            foreach (Phase item in list2)
            {
                switch (flt.Category)
                {
                    case Enums.Categories.PhaseCreated:
                        list3.Add((ElementFilter)new ElementPhaseStatusFilter(((Element)item).Id, (ElementOnPhaseStatus)4));
                        break;
                    case Enums.Categories.PhaseDemolished:
                        list3.Add((ElementFilter)new ElementPhaseStatusFilter(((Element)item).Id, (ElementOnPhaseStatus)3));
                        break;
                }
            }
            return (ElementFilter)(list3.Count switch
            {
                0 => null,
                1 => list3[0],
                _ => (object)new LogicalOrFilter((IList<ElementFilter>)list3),
            });
        }

        /// <inheritdoc />
        //public ElementFilter GetPhaseStatusFilter(Filter flt)
        //{
        //    Phase val = ((IEnumerable)new FilteredElementCollector(_doc).OfClass(typeof(Phase))).Cast<Phase>().FirstOrDefault((Phase x) => ((Element)x).Name == flt.Property);
        //    if (val == null)
        //    {
        //        flt.Check.CheckResult.ErrorMessage = string.Format(MCResources.Warning_PhaseNotFoundStatus, flt.Property);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            ElementOnPhaseStatus val2 = (ElementOnPhaseStatus)Enum.Parse(typeof(ElementOnPhaseStatus), flt.Value);
        //            return (ElementFilter)new ElementPhaseStatusFilter(((Element)val).Id, val2);
        //        }
        //        catch
        //        {
        //            flt.Check.CheckResult.ErrorMessage = string.Format(MCResources.Warning_InvalidPhaseStatus, flt.Value);
        //        }
        //    }
        //    return null;
        //}

        ///// <inheritdoc />
        //public ElementFilter GetStructuralTypeFilter(Filter flt)
        //{
        //    try
        //    {
        //        StructuralType val = (StructuralType)Enum.Parse(typeof(StructuralType), flt.Value);
        //        switch (flt.Condition)
        //        {
        //            case Enums.Conditions.Equal:
        //                return (ElementFilter)new ElementStructuralTypeFilter(val);
        //            case Enums.Conditions.NotEqual:
        //                return (ElementFilter)new ElementStructuralTypeFilter(val, true);
        //        }
        //    }
        //    catch
        //    {
        //        flt.Check.CheckResult.ErrorMessage = string.Format(MCResources.Warning_InvalidStructuralType, flt.Value);
        //    }
        //    return null;
        //}

        ///// <inheritdoc />
        //public ElementFilter GetApiTypeFilter(Filter flt)
        //{
        //    try
        //    {
        //        Type type = typeof(Element).Assembly.GetType(flt.Value);
        //        if (!(type == null))
        //        {
        //            return (ElementFilter)new ElementClassFilter(type);
        //        }
        //        flt.Check.CheckResult.ErrorMessage = string.Format(MCResources.Warning_InvalidClassName, flt.Value);
        //    }
        //    catch
        //    {
        //        flt.Check.CheckResult.ErrorMessage = string.Format(MCResources.Warning_ClassConversionError, flt.Value);
        //    }
        //    return null;
        //}

        /// <inheritdoc />
        public ElementFilter GetViewFilter(Filter flt)
        {
            if (flt.Property == "Is Defined")
            {
                return (ElementFilter)new ElementOwnerViewFilter(ElementId.InvalidElementId, (flt.Condition == Enums.Conditions.Equal && string.Equals(flt.Value, "True", StringComparison.CurrentCultureIgnoreCase)) || (flt.Condition == Enums.Conditions.NotEqual && string.Equals(flt.Value, "False", StringComparison.CurrentCultureIgnoreCase)));
            }
            List<View> source = ((IEnumerable)new FilteredElementCollector(_doc).OfClass(typeof(View))).Cast<View>().ToList();
            List<View> list = null;
            switch (flt.Condition)
            {
                case Enums.Conditions.Equal:
                    list = source.Where((View x) => (!flt.CaseInsensitive) ? (((Element)x).Name == flt.Value) : ((Element)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                case Enums.Conditions.NotEqual:
                    list = source.Where((View x) => (!flt.CaseInsensitive) ? (((Element)x).Name != flt.Value) : (!((Element)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase))).ToList();
                    break;
                case Enums.Conditions.Contains:
                    list = source.Where((View x) => (!flt.CaseInsensitive) ? ((Element)x).Name.Contains(flt.Value) : (CultureInfo.CurrentCulture.CompareInfo.IndexOf(((Element)x).Name, flt.Value, CompareOptions.IgnoreCase) >= 0)).ToList();
                    break;
                case Enums.Conditions.DoesNotContain:
                    list = source.Where((View x) => (!flt.CaseInsensitive) ? (!((Element)x).Name.Contains(flt.Value)) : (CultureInfo.CurrentCulture.CompareInfo.IndexOf(((Element)x).Name, flt.Value, CompareOptions.IgnoreCase) < 0)).ToList();
                    break;
                case Enums.Conditions.Defined:
                    return (ElementFilter)new ElementOwnerViewFilter(ElementId.InvalidElementId, true);
                case Enums.Conditions.Undefined:
                    return (ElementFilter)new ElementOwnerViewFilter(ElementId.InvalidElementId);
                case Enums.Conditions.WildCard:
                case Enums.Conditions.WildCardNoMatch:
                    {
                        Regex re = new Regex(flt.Value);
                        list = source.Where((View x) => (flt.Condition != Enums.Conditions.WildCard) ? (!re.IsMatch(((Element)x).Name)) : re.IsMatch(((Element)x).Name)).ToList();
                        break;
                    }
            }
            if (list == null || list.Count == 0)
            {
                return GetNoElementsFilter();
            }
            List<ElementFilter> list2 = new List<ElementFilter>();
            foreach (View item in list)
            {
                list2.Add((ElementFilter)new ElementOwnerViewFilter(((Element)item).Id));
            }
            return (ElementFilter)(list2.Count switch
            {
                0 => null,
                1 => list2[0],
                _ => (object)new LogicalOrFilter((IList<ElementFilter>)list2),
            });
        }

        /// <inheritdoc />
        public ElementFilter GetWorksetFilter(Filter flt)
        {
            List<Workset> source = ((IEnumerable<Workset>)new FilteredWorksetCollector(_doc)).ToList();
            List<Workset> list = null;
            switch (flt.Condition)
            {
                case Enums.Conditions.Equal:
                    list = source.Where((Workset x) => (!flt.CaseInsensitive) ? (((WorksetPreview)x).Name == flt.Value) : ((WorksetPreview)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
                case Enums.Conditions.NotEqual:
                    list = source.Where((Workset x) => (!flt.CaseInsensitive) ? (((WorksetPreview)x).Name != flt.Value) : (!((WorksetPreview)x).Name.Equals(flt.Value, StringComparison.CurrentCultureIgnoreCase))).ToList();
                    break;
                case Enums.Conditions.Contains:
                    list = source.Where((Workset x) => (!flt.CaseInsensitive) ? ((WorksetPreview)x).Name.Contains(flt.Value) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(((WorksetPreview)x).Name, flt.Value, CompareOptions.IgnoreCase) >= 0)).ToList();
                    break;
                case Enums.Conditions.DoesNotContain:
                    list = source.Where((Workset x) => (!flt.CaseInsensitive) ? (!((WorksetPreview)x).Name.Contains(flt.Value)) : (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(((WorksetPreview)x).Name, flt.Value, CompareOptions.IgnoreCase) < 0)).ToList();
                    break;
                case Enums.Conditions.WildCard:
                case Enums.Conditions.WildCardNoMatch:
                    {
                        Regex re = new Regex(flt.Value);
                        list = source.Where((Workset x) => (flt.Condition != Enums.Conditions.WildCard) ? (!re.IsMatch(((WorksetPreview)x).Name)) : re.IsMatch(((WorksetPreview)x).Name)).ToList();
                        break;
                    }
            }
            if (list == null || list.Count == 0)
            {
                return GetNoElementsFilter();
            }
            List<ElementFilter> list2 = new List<ElementFilter>();
            foreach (Workset item in list)
            {
                list2.Add((ElementFilter)new ElementWorksetFilter(((WorksetPreview)item).Id));
            }
            return (ElementFilter)(list2.Count switch
            {
                0 => null,
                1 => list2[0],
                _ => (object)new LogicalOrFilter((IList<ElementFilter>)list2),
            });
        }

        private ElementParameterFilter GetLevelParamStringFilter(string levelName, BuiltInParameter param)
        {
            ParameterValueProvider val = new ParameterValueProvider(new ElementId(param));
            FilterStringRuleEvaluator val2 = (FilterStringRuleEvaluator)new FilterStringEquals();
            return new ElementParameterFilter((FilterRule)new FilterStringRule((FilterableValueProvider)val, val2, levelName));
        }

        private ElementParameterFilter GetLevelParamIdFilter(ElementId id, BuiltInParameter param)
        {
            ParameterValueProvider val = new ParameterValueProvider(new ElementId(param));
            FilterNumericRuleEvaluator val2 = (FilterNumericRuleEvaluator)new FilterNumericEquals();
            return new ElementParameterFilter((FilterRule)new FilterElementIdRule((FilterableValueProvider)val, val2, id));
        }

        /// <summary>
        /// Returns a filter that will ensure that no elements match, used for when no elements are found to create filters from
        /// </summary>
        /// <returns></returns>
        private ElementFilter GetNoElementsFilter()
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_0007: Unknown result type (might be due to invalid IL or missing references)
            //IL_0011: Expected O, but got Unknown
            //IL_0011: Expected O, but got Unknown
            //IL_000c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0012: Expected O, but got Unknown
            return (ElementFilter)new LogicalAndFilter((ElementFilter)new ElementIsElementTypeFilter(true), (ElementFilter)new ElementIsElementTypeFilter(false));
        }
    }
}
