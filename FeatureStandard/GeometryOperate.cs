using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace DataNormalization
{
    /// <summary>
    /// 几何操作的类
    /// </summary>
    class GeometryOperate
    {
        /// <summary>
        /// 判断两个要素类是否具有相同的几何
        /// </summary>
        /// <param name="featureClass1">待比较要素1</param>
        /// <param name="featureClass2">待比较要素2</param>
        /// <returns>返回两要素几何比较的布尔值，相同返回true</returns>
        public static bool IsSameGeo(IFeatureClass featureClass1,IFeatureClass featureClass2)
        {
            IFeatureCursor pFeatureCursorA = featureClass1.Search(null,false);
            IFeatureCursor pFeatureCursorB = featureClass2.Search(null, true);
            IFeature pFeatureA, pFeatureB;
            List<IFeature> list_FeatureA = new List<IFeature>();
            List<IFeature> list_FeatureB = new List<IFeature>();
            if (featureClass1.FeatureCount(null) != featureClass2.FeatureCount(null))
                return false;
            if (featureClass1.Extension != featureClass2.Extension)
                return false;
            else
            {
                while ((pFeatureA = pFeatureCursorA.NextFeature()) != null&& (pFeatureB = pFeatureCursorB.NextFeature()) != null)
                {
                    list_FeatureA.Add(pFeatureA);
                    list_FeatureB.Add(pFeatureB);
                }
                object.ReferenceEquals(list_FeatureA, list_FeatureB);
                return true;
            }
        }

        /// <summary>
        /// 将FeatureClass导出为shapefile
        /// </summary>
        /// <param name="pInFeatureClass">待导出的FeatureClass</param>
        /// <param name="pPath">导出的路径</param>
        public static void ExportFeature(IFeatureClass pInFeatureClass, string pPath)
        {
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            string parentPath = pPath.Substring(0, pPath.LastIndexOf('\\'));
            string fileName = pPath.Substring(pPath.LastIndexOf('\\') + 1, pPath.Length - pPath.LastIndexOf('\\') - 1);
            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(parentPath, fileName, null, 0);
            // Cast for IName        
            IName name = (IName)pWorkspaceName;
            //Open a reference to the access workspace through the name object        
            IWorkspace pOutWorkspace = (IWorkspace)name.Open();


            IDataset pInDataset = pInFeatureClass as IDataset;
            IFeatureClassName pInFCName = pInDataset.FullName as IFeatureClassName;
            IWorkspace pInWorkspace = pInDataset.Workspace;
            IDataset pOutDataset = pOutWorkspace as IDataset;
            IWorkspaceName pOutWorkspaceName = pOutDataset.FullName as IWorkspaceName;
            IFeatureClassName pOutFCName = new FeatureClassNameClass();
            IDatasetName pDatasetName = pOutFCName as IDatasetName;
            pDatasetName.WorkspaceName = pOutWorkspaceName;
            pDatasetName.Name = pInFeatureClass.AliasName;
            IFieldChecker pFieldChecker = new FieldCheckerClass();
            pFieldChecker.InputWorkspace = pInWorkspace;
            pFieldChecker.ValidateWorkspace = pOutWorkspace;
            IFields pFields = pInFeatureClass.Fields;
            IFields pOutFields;
            IEnumFieldError pEnumFieldError;
            pFieldChecker.Validate(pFields, out pEnumFieldError, out pOutFields);
            IFeatureDataConverter pFeatureDataConverter = new FeatureDataConverterClass();
            pFeatureDataConverter.ConvertFeatureClass(pInFCName, null, null, pOutFCName, null, pOutFields, "", 100, 0);
        }
}
}
