//
//  FunctionsViewController.h
//  Native-Sample
//
//  Created by Ignacio Calderon on 11/8/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import <UIKit/UIKit.h>

@class DetailViewController;

@interface FunctionsViewController : UITableViewController

@property (strong, nonatomic) DetailViewController *detailViewController;
@property (strong, nonatomic) NSArray *functions;
@property (strong, nonatomic) NSString *overview;
@end
