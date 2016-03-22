//
//  SecondViewController.swift
//  Andrule
//
//  Created by Константин on 22.03.16.
//  Copyright © 2016 Andrule inc. All rights reserved.
//

import UIKit
import CoreMotion

class SecondViewController: UIViewController {

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    @IBOutlet weak var progressBar: UIProgressView!
    @IBOutlet weak var XGrav: UILabel!
    @IBOutlet weak var YGrav: UILabel!
    @IBOutlet weak var ZGrav: UILabel!
    
    var motionManager = CMMotionManager();
    
    @IBAction func StartButtonClick(sender: UIButton) {
        if motionManager.deviceMotionAvailable {
            motionManager.deviceMotionUpdateInterval = 0.01
            motionManager.startDeviceMotionUpdatesToQueue(NSOperationQueue.mainQueue(), withHandler: {
                data, error in
                self.XGrav.text = String(data!.gravity.x);
                self.YGrav.text = String(data!.gravity.y);
                self.ZGrav.text = String(data!.gravity.z);
                
                self.progressBar.progress = Float((data!.gravity.x + 1)/2);
                //let rotation = atan2(data!.gravity.x, data!.gravity.y) - M_PI
                //self.progress.transform = CGAffineTransformMakeRotation(CGFloat(rotation))
            })
        }
    }
    
}

