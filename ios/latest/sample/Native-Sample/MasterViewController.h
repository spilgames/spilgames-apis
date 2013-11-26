//
//  MasterViewController.h
//  Native-Sample
//
//  Created by Ignacio Calderon on 11/8/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import <UIKit/UIKit.h>

@class DetailViewController;

@interface MasterViewController : UITableViewController

@property (strong, nonatomic) DetailViewController *detailViewController;
@end
