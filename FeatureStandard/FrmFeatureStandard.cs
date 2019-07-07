using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using DevExpress.XtraEditors.Repository;
using DataNormalization;

namespace CreateCollectEnterpriseDB
{
    public partial class FrmFeatureStandard : DevExpress.XtraEditors.XtraForm
    {
        public FrmFeatureStandard()
        {
            InitializeComponent();
        }

        string sourceFeatureName;
        string targetFeatureName;
        IFeatureClass pFeatureClass1;//标准要素类
        IFeatureClass pFeatureClass2;//原要素类
        IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
        IFeatureWorkspace pFeatureWorkspace;

        private void ComboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit1.Text == "")
                return;
            else
            {
                //
                //此处为测试代码，需要替换为加载工作空间，打开要素类的代码
                //
                targetFeatureName = comboBoxEdit1.Text;
                pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile("E:\\测试数据", 0);
                pFeatureClass1 = pFeatureWorkspace.OpenFeatureClass(targetFeatureName);

                //得到标准要素类和原要素类的属性字段列表
                List<string> s1 = AttTableOperate.get_FieldsString(pFeatureClass1);
                int index;
                for (int i = 0; i < s1.Count; i++)
                {
                    if (s1[i].ToUpper() == "FID" || s1[i].ToUpper() == "SHAPE")
                    {
                        continue;
                    }
                    else
                        index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = s1[i];
                }
            }
        }

        private void ComboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit2.Text == "")
                return;
            else
            {
                //
                //此处为测试代码，需要替换为加载工作空间，打开要素类的代码
                //
                sourceFeature.Items.Clear();
                sourceFeatureName = comboBoxEdit2.Text;
                pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile("E:\\测试数据", 0);
                pFeatureClass2 = pFeatureWorkspace.OpenFeatureClass(sourceFeatureName);

                List<string> s2 = AttTableOperate.get_FieldsString(pFeatureClass2);
                for (int j = 0; j < s2.Count; j++)
                {
                    if (s2[j].ToUpper() == "FID" || s2[j].ToUpper() == "SHAPE")
                        continue;
                    else
                    {
                        sourceFeature.Items.Add(s2[j]);
                    }
                }
            }
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.RowCount;
            for (int i = n - 1; i >= 0; i--)
                dataGridView1.Rows.RemoveAt(i);
            comboBoxEdit1.Text = "";
            comboBoxEdit2.Text = "";
        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            while (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
        }

        private void SimpleButton3_Click(object sender, EventArgs e)
        {
            #region 判断变量是否为空
            if (sourceFeatureName == null )
            {
                MessageBox.Show(null, "请选择原要素类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (targetFeatureName == null)
            {
                MessageBox.Show(null, "请选择目标要素类", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            DataNormalize DN = new DataNormalize();
            int a = dataGridView1.RowCount;
            string[,] s = new string[a, 2];
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < 2; j++)
                    s[i, j] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                int a1 = pFeatureClass1.Fields.FindField(s[i, 0]);
                int a2= pFeatureClass2.Fields.FindField(s[i, 1]);
                if (AttTableOperate.IsSameFieldType(pFeatureClass1.Fields.Field[pFeatureClass1.Fields.FindField(s[i, 0])],
                    pFeatureClass2.Fields.Field[pFeatureClass2.Fields.FindField(s[i, 1])])==false)
                {
                    MessageBox.Show("原要素字段类型不匹配！");
                    return;
                }
            }
            IFeatureClass result_FeatureClass = DN.CreateStandardFeatureClass(pFeatureClass2, pFeatureClass1, s);
            MessageBox.Show("处理完成！");
            this.Close();
        }
    }
}