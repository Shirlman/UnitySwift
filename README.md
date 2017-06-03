# UnitySwift

参考自https://github.com/miyabi/unity-swift

## 功能
1. Unity调用Native界面，并返回Unity界面
2. Unity build xcode工程时自动打包并配置swift工程
3. 支持自动打包storyboard文件
4. 自动配置Info.plist，添加工程所需权限
5. 可以在Unity里管理整个swift工程

## 使用
1. 下载Unity工程
2. 打开并build xcode工程
3. 神奇的地方在这里，无需任何配置，直接运行

## 自动配置swift工程
主要使用了PostProcessBuild做了一些后期处理，参考 [PostProcessor.cs](./Assets/UnitySwift/Editor/PostProcessor.cs)。
具体请参考：https://github.com/miyabi/unity-swift

## Unity和native界面相互切换
1. 新建UnitySubAppController.swift文件，继承UnityAppController，接管Unity的启动，将Unity的ViewController保存下来，有了ViewController，后面就方面做页面的跳转操作了，参考[UnitySubAppController.swift](./Assets/UnitySwift/UnitySubAppController.swift)
2. 通过PostProcessor修改xcode工程中main.mm，改变启动的UnityAppController为UnitySubAppController
```csharp
private static void ModifyMainFile(string buildPath)
{
	// Redirect UnityAppController to custom UnitySubAppController
	string mainFilePath = buildPath + "/Classes/main.mm";
	string mainContent = File.ReadAllText (mainFilePath);

	// Import object-c swift bridge header
	if(!mainContent.Contains(OBJC_BRIDGE_HEADER_NAME))
	{
		mainContent = mainContent.Insert(0, string.Format("#include \"{0}\"\n", OBJC_BRIDGE_HEADER_NAME));
	}

	// Update startup UIApplicationDelegate
	mainContent = mainContent.Replace (
		"[NSString stringWithUTF8String: AppControllerClassName]",
		"NSStringFromClass([UnitySubAppController class])");

	// Write to file
	File.WriteAllText (mainFilePath, mainContent);
}
```
