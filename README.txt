CreateConnectGDB

1、功能描述：
	对矢量数据进行标准化操作（将要素类A的某几个属性提取出来作为标准要素类B的对应属性字段，同时
	复制要素形状）

2、开发环境：
	操作系统：windows10
	编程语言：c#
	开发工具：vs2017、ArcEngine10.4
	平台：.net Framework4.6

3、解决方案中目录结构：
	|---README.txt				//说明文档
	|---FrmFeatureStandard.cs	//矢量数据标准化界面和代码
	|---AttTableOperate.cs		//属性表操作的类
	|---DataNormalization.cs	//要素标准化的类
	|---GeometryOperate.cs		//几何操作的类（暂未引用）

4、备注：
		1)原要素类和标准要素类的选取仅仅为测试，实际情况下需要替换为加载工作空间，
	打开要素类的代码；
		2)测试数据中“仓库模板库.shp”为标准的数据格式，“要导入的数据.shp”为需要处理
	的数据。