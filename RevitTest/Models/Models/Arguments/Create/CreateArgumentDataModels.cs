using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using BUD.Drainage.Revit.Addin.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BUD.Drainage.Revit.Addin.Models.Arguments.Create
{
    /// <summary>
    /// 放置数据
    /// </summary>
    public class PlacementData
    {

        /// <summary>
        /// 输入单位，默认为米
        /// </summary>
        [JsonProperty("input_unit")]
        public ForgeTypeId InputUnit { get; set; } = UnitTypeId.Meters;

        /// <summary>
        /// 输出单位，默认为英寸
        /// </summary>
        [JsonProperty("output_unit")]
        public ForgeTypeId OutputUnit { get; set; } = UnitTypeId.Feet;
        /// <summary>
        /// 放置方法：point, line, face
        /// </summary>
        [JsonProperty("method")]
        public PlacementMethod Method { get; set; }

        private List<XYZ> _points;
        /// <summary>
        /// 放置点坐标，对于点放置需要1个点，对于线放置需要2个点
        /// </summary>
        [JsonProperty("points")]
        [JsonConverter(typeof(XYZListConverter))]
        public List<XYZ> Points
        {
            get { return GetConvertedPoints(); }
            set { _points = value; }
        }

        /// <summary>
        /// 宿主元素ID，用于在宿主上放置元素，如在墙上放置门窗
        /// </summary>
        [JsonProperty("host_id")]
        [JsonConverter(typeof(ElementIdConverter))]
        public ElementId HostId { get; set; }

        /// <summary>
        /// 标高名称，用于指定元素所在的标高
        /// </summary>
        [JsonProperty("level_id")]
        [JsonConverter(typeof(ElementIdConverter))]
        public ElementId LevelId { get; set; }

        /// <summary>
        /// 结构类型，默认为NonStructural
        /// </summary>
        [JsonProperty("structural_type")]
        public StructuralType StructuralType { get; set; } = StructuralType.NonStructural;

        /// <summary>
        /// 视图ID，用于在特定视图中创建详图元素
        /// </summary>
        [JsonProperty("view_id")]
        [JsonConverter(typeof(ElementIdConverter))]
        public ElementId ViewId { get; set; }

        /// <summary>
        /// 转换坐标点的单位，从输入单位转换为输出单位
        /// </summary>
        /// <param name="document">Revit文档，用于获取单位转换器</param>
        /// <returns>转换后的坐标点列表</returns>
        public List<XYZ> GetConvertedPoints()
        {
            if (_points == null || _points.Count == 0)
                return _points;

            List<XYZ> convertedPoints = new List<XYZ>();

            // 获取单位转换器
            ForgeTypeId fromUnit = InputUnit;
            ForgeTypeId toUnit = OutputUnit;

            // 创建单位转换因子
            double conversionFactor = UnitUtils.Convert(1.0, fromUnit, toUnit);
            if (!fromUnit.Equals(UnitTypeId.Meters))
            {
                // 如果输入单位不是米，先转换为内部单位
                conversionFactor = UnitUtils.ConvertToInternalUnits(1.0, fromUnit) * conversionFactor;
            }

            // 转换每个点的坐标
            foreach (XYZ point in _points)
            {
                // 创建新的XYZ对象，应用转换因子
                XYZ convertedPoint = new XYZ(
                    point.X * conversionFactor,
                    point.Y * conversionFactor,
                    point.Z * conversionFactor
                );

                convertedPoints.Add(convertedPoint);
            }

            return convertedPoints;
        }
    }

    /// <summary>
    /// 尺寸数据
    /// </summary>
    public class DimensionsData
    {
        /// <summary>
        /// 高度
        /// </summary>
        [JsonProperty("height")]
        public double Height { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        [JsonProperty("width")]
        public double Width { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        [JsonProperty("depth")]
        public double Depth { get; set; }

        /// <summary>
        /// 是否为结构构件
        /// </summary>
        [JsonProperty("structural")]
        public bool Structural { get; set; }
    }

    /// <summary>
    /// 放置方法枚举
    /// </summary>
    public enum PlacementMethod
    {
        /// <summary>
        /// 基于点放置
        /// </summary>
        Point,

        /// <summary>
        /// 基于线放置
        /// </summary>
        Line,

        /// <summary>
        /// 基于面放置
        /// </summary>
        Face
    }
}