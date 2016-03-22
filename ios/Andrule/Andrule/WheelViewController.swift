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

    @IBOutlet weak var BrakeSlider: UISlider!
    @IBOutlet weak var RuleSlider: UISlider!
    @IBOutlet weak var ruleLabel: UILabel!
    @IBOutlet weak var brakeLabel: UILabel!
    @IBOutlet weak var atanLabel: UILabel!
    
    var updateActive = false;
    var minRotationValue = 0;
    var maxRotaionValue = 30000;
    var lastRotation = 0;
    var rotation = 0;
    var brake = 0;
    var rule = 0;
    var motionManager = CMMotionManager();
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        motionManager.deviceMotionUpdateInterval = 0.01
        SettingSliders()
        StartGetSensor()
    }

    override func didReceiveMemoryWarning() {
        
        super.didReceiveMemoryWarning()
        
        // Dispose of any resources that can be recreated.
    }
    
    @IBAction func BrakeSliderChanged(sender: UISlider) {
        self.brake = Int(BrakeSlider.value);
        self.brakeLabel.text = String(self.brake);
    }
    
    @IBAction func RuleSliderChanged(sender: UISlider) {
        self.rule = Int(RuleSlider.value);
        self.ruleLabel.text = String(self.rule)
    }
    
    func StartGetSensor() -> Void {
        if (motionManager.deviceMotionAvailable == false) {
            debugPrint("error");
            return;
        }
        motionManager.startDeviceMotionUpdatesToQueue(NSOperationQueue.mainQueue(), withHandler: {
            data, error in
            self.rotation = self.TransformRotation(data!.gravity.x, y: data!.gravity.y);
            self.atanLabel.text = String(self.rotation);
        })

    }
    
    func TransformRotation(x: Double, y: Double) -> Int {
        let rotation = 32400 - Int((atan2(x, y) + M_PI) * 10000);
        
        if (rotation > -16400 && rotation < 0) {
            return 50;
        }
        if (rotation < -16500) {
            return 32400;
        }
        return rotation
    }
    
    func SettingSliders() -> Void {
        self.BrakeSlider.transform = CGAffineTransformMakeRotation(CGFloat(-M_PI_2));
        self.RuleSlider.transform = CGAffineTransformMakeRotation(CGFloat(-M_PI_2));

        let thumbImage = UIImage.init(named: "slider_2")
        self.BrakeSlider.setThumbImage(thumbImage, forState: UIControlState.Normal)
        self.RuleSlider.setThumbImage(thumbImage, forState: UIControlState.Normal)
    }
}

