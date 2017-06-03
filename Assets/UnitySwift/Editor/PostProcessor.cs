#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;

namespace UnitySwift {
    public static class PostProcessor {
		private const string OBJC_BRIDGE_HEADER_NAME = "unityswift-Swift.h";

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath) 
		{
            if(buildTarget != BuildTarget.iOS) 
			{
				return;
            }

			// So PBXProject.GetPBXProjectPath returns wrong path, we need to construct path by ourselves instead
			// var projPath = PBXProject.GetPBXProjectPath(buildPath);
			var projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
			var proj = new PBXProject();
			proj.ReadFromFile(projPath);

			var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

			// Configure build settings
			proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
			proj.SetBuildProperty(targetGuid, "SWIFT_VERSION", "Swift3");
			proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/UnitySwift/UnitySwift-Bridging-Header.h");
			proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_INTERFACE_HEADER_NAME", OBJC_BRIDGE_HEADER_NAME);
			proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");

			// Unity do not add .storyboard automatically, should add .stroyboard files to xcode project
			string[] storyboardFilePaths = Directory.GetFiles( Application.dataPath, "*.storyboard", SearchOption.AllDirectories );

			foreach( string storyboardFilePath in storyboardFilePaths ) 
			{
				string destRelativeFilePath = "Libraries/" + storyboardFilePath.Substring (Application.dataPath.Length + 1);
				string destFile = buildPath + "/" + destRelativeFilePath;
				File.Copy(storyboardFilePath, destFile);

				var fileGuid = proj.AddFile(destFile, destRelativeFilePath);
				proj.AddFileToBuild (targetGuid, fileGuid);
			}

			proj.WriteToFile(projPath);

			UpdatePlist (buildPath);

			ModifyMainFile (buildPath);
        }

		private static void UpdatePlist(string buildPath)
		{
			// Get plist
			string plistPath = buildPath + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));

			// Change value in Xcode plist
			plist.root.SetString("NSCameraUsageDescription", "Use your camera to take pictures/video");
			plist.root.SetString("NSLocationWhenInUseUsageDescription", "Use your location to save EXIF data");
			plist.root.SetString("NSMicrophoneUsageDescription", "Use your microphone to record audio for videos");
			plist.root.SetString("NSPhotoLibraryUsageDescription", "Access your photo library to save pictures/video");
			plist.root.SetBoolean("UIViewControllerBasedStatusBarAppearance", false);
			// Add your plist key-value here

			// Write to file
			File.WriteAllText(plistPath, plist.WriteToString());
		}

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
    }
}

#endif