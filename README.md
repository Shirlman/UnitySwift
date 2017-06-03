# UnitySwift

参考自https://github.com/miyabi/unity-swift

简单的Unity调用swift工程，功能如下：
1. Unity调用Native界面，并返回Unity界面
2. Unity build xcode工程时自动打包并配置swift工程
3. 支持自动打包storyboard文件
4. 自动配置Info.plist，添加swift所需权限
5. 可以在Unity里管理整个swift工程

## 使用
1. 下载Unity工程
2. 打开并build xcode工程
3. 神奇的地方在这里，无需任何配置，直接运行

## 原理
主要使用了PostProcessBuild做了一些后期处理，参考 [PostProcessor.cs](./Assets/UnitySwift/Editor/PostProcessor.cs)
