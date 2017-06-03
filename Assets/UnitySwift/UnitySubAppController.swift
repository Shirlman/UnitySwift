//
//  UnitySubAppController.swift
//  Unity-iPhone
//
//  Created by Shirlman on 2017/5/4.
//
//

import UIKit

class UnitySubAppController: UnityAppController {
    static var unityController: UIViewController?
    
    override func startUnity(_ application: UIApplication!) {
        super.startUnity(application)
        
        UnitySubAppController.unityController = self.rootViewController
    }
        
    static func presentViewController(storyboardName : String, storyboardId : String) {
        if(unityController == nil) {
            return
        }
        
        let storyboard = UIStoryboard(name: storyboardName, bundle: nil)
        let targetVC: UIViewController? = storyboard.instantiateViewController(withIdentifier: storyboardId)
        
        if let validVC : UIViewController = targetVC {
            unityController?.present(validVC, animated: true, completion: nil)
        }
    }
}
