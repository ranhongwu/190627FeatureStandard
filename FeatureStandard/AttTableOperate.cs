using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;


namespace DataNormalization
{
    /// <summary>
    /// 属性表操作类
    /// </summary>
    class AttTableOperate
    {
        /// <summary>
        /// 获取待要素类的所有属性字段名
        /// </summary>
        /// <param name="pFeatureClass">待复制要素类</param>
        /// <returns>返回待复制要素类的所有属性字段名</returns>
        public static List<string> get_FieldsString(IFeatureClass pFeatureClass)
        {
            IFields pFields = pFeatureClass.Fields;
            IField pField;
            List<string> s = new List<string>();
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                pField = pFields.Field[i];
                s.Add(pField.Name);
            }
            return s;
        }

        /// <summary>
        /// 判断两个属性字段类型是否一样
        /// </summary>
        /// <param name="a">待比较的属性字段a</param>
        /// <param name="b">待比较的属性字段b</param>
        /// <returns>返回比较结果的布尔值，类型相同返回true，类型不同返回false</returns>
        public static bool IsSameFieldType(IField a,IField b)
        {
            if (a.Type == b.Type)
                return true;
            else
                return false;
        }
    }
}
