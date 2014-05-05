//
//  FunctionsViewController.m
//  Native-Sample
//
//  Created by Ignacio Calderon on 11/8/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import "FunctionsViewController.h"
#import "DetailViewController.h"

@interface FunctionsViewController ()
@end

@implementation FunctionsViewController

- (void)awakeFromNib {
	self.clearsSelectionOnViewWillAppear = NO;
	self.contentSizeForViewInPopover = CGSizeMake(320.0, 600.0);
    [super awakeFromNib];
}

- (void)viewDidLoad {
    [super viewDidLoad];
	// Do any additional setup after loading the view, typically from a nib.
	// initialize the _objects array to create the different sections navigation.
	
	self.detailViewController = (DetailViewController *)[[self.splitViewController.viewControllers lastObject] topViewController];
    [self tableView:nil didSelectRowAtIndexPath:[NSIndexPath indexPathForItem:0 inSection:0]];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - Table View

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
	return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	return _functions.count + 1;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:@"Cell" forIndexPath:indexPath];
	
	if(indexPath.row == 0){
		cell.textLabel.text = @"Overview";
		cell.selected = YES;
	}else{
		NSDictionary *object = _functions[indexPath.row - 1];
		cell.textLabel.text = object[@"name"];
		cell.selected = NO;
	}
    return cell;
}


-(void) tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    NSString* format = @"<p style='font-family: Arial; font-size:19;'>%@</p>";
	if(indexPath.row == 0){
		self.detailViewController.trigger.enabled = NO;
		[self.detailViewController setDescriptionText:[NSString stringWithFormat:format,
                                                       [_overview stringByReplacingOccurrencesOfString:@"\n" withString:@"<br/>"]]];
		self.detailViewController.snippet.text = @"";
    }else{
		int index = indexPath.row - 1;
		self.detailViewController.trigger.enabled = _functions[index][@"snippet"] != nil;
		self.detailViewController.snippet.text = _functions[index][@"snippet"];
        [self.detailViewController setDescriptionText:
         [NSString stringWithFormat:format,
          [[NSString stringWithContentsOfFile:
           [[NSBundle mainBundle] pathForResource:_functions[index][@"selector"] ofType:@"html"]
                                    encoding:NSUTF8StringEncoding
                                       error:nil] stringByReplacingOccurrencesOfString:@"\n" withString:@"<br/>"]
          ]];
		self.detailViewController.snippetSelector = _functions[index][@"selector"];
	}
}
@end
