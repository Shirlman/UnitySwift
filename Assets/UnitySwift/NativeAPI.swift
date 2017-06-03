import Foundation
import UIKit

class NativeAPI : NSObject {
    
    static func presentNativeViewController() {
        print("\(#function) is called")
        
        UnitySubAppController.presentViewController(storyboardName: "Example", storyboardId: "ExampleStart")
    }
}
