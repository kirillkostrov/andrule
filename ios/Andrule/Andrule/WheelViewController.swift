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

    @IBOutlet weak var progressBar: UIProgressView!
    @IBOutlet weak var XGrav: UILabel!
    @IBOutlet weak var TestImage: UIImageView!
    @IBOutlet weak var atan: UILabel!
    @IBOutlet weak var YGrav: UILabel!
    @IBOutlet weak var ZGrav: UILabel!
    @IBOutlet weak var StartButton: UIImageView!
    
    var updateActive = false;
    var minRotationValue = 0;
    var maxRotaionValue = 30000;
    var lastRotation = 0;
    var motionManager = CMMotionManager();
    
    override func viewDidLoad() {
        super.viewDidLoad()
        motionManager.deviceMotionUpdateInterval = 0.01
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBAction func StartButtonClick(sender: UIButton) {
        updateActive = !updateActive;
        if (motionManager.deviceMotionAvailable == false) {
            atan.text = "Error";
            return;
        }
        if (!updateActive){
            motionManager.stopDeviceMotionUpdates()
            
            return;
        }
        motionManager.startDeviceMotionUpdatesToQueue(NSOperationQueue.mainQueue(), withHandler: {
            data, error in
            let rotation = self.TransformRotation(data!.gravity.x, y: data!.gravity.y);
            self.XGrav.text = String(data!.gravity.x);
            self.YGrav.text = String(data!.gravity.y);
            self.ZGrav.text = String(data!.gravity.z);
            self.atan.text = String(rotation)
            
            let transformImage = CGFloat(atan2(data!.gravity.x, data!.gravity.y) + M_PI)
            self.TestImage.transform = CGAffineTransformMakeRotation(transformImage * -1)
        })
    }
    
    func TransformRotation(x: Double, y: Double) -> Int {
        let rotation = Int((atan2(x, y) + M_PI) * 10000);
        if (rotation > 45000) {
            lastRotation = rotation;
            return 500;
        }
        if (rotation > 31000 && rotation < 45000) {
            lastRotation = rotation;
            return 30500;
        }
        return rotation
    }
    
}

