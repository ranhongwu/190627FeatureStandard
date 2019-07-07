using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace DataNormalization
{
    /// <summary>
    /// 要素标准化类
    /// </summary>
    class DataNormalize
    {
        /// <summary>
        /// 创建标准要素类
        /// </summary>
        /// <param name="sourceFeatureClass">原要素类</param>
        /// <param name="target_FeatureClass">标准要素类</param>
        /// <param name="contrastTable">字段对照表，二维数组</param>
        /// <returns>返回创建的标准要素类</returns>
        public IFeatureClass CreateStandardFeatureClass(IFeatureClass sourceFeatureClass,IFeatureClass target_FeatureClass
            ,string[,] contrastTable)
        {
            IFeature pFeature;
            IFeatureCursor sourceCursor = sourceFeatureClass.Search(null, false);
            //用IFeatureBuffer提高运算速度
            IFeatureBuffer pFeaBuffer = null;
            IFeatureCursor targetCursor = target_FeatureClass.Insert(true);
            while ((pFeature = sourceCursor.NextFeature()) != null)
            {
                pFeaBuffer = target_FeatureClass.CreateFeatureBuffer();
                pFeaBuffer.Shape = pFeature.Shape;
                for(int i = 0; i < contrastTable.Length / 2; i++)
                {
                    pFeaBuffer.set_Value(target_FeatureClass.FindField(contrastTable[i,0]), pFeature.Value[sourceFeatureClass.Fields.FindField(contrastTable[i,1])]);
                }
                targetCursor.InsertFeature(pFeaBuffer);
            }
            targetCursor.Flush();
            //运用IfeatureBuffer和IfeatureCursor时需要手动释放非托管对象
            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceCursor);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaBuffer);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(targetCursor);

            return target_FeatureClass;
        }
    }
}
