//
//  MasterViewController.m
//  Native-Sample
//
//  Created by Ignacio Calderon on 11/8/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import "MasterViewController.h"

#import "DetailViewController.h"
#import "FunctionsViewController.h"

@interface MasterViewController () {
    NSMutableArray *_objects;
	int _index;
}
@end

@implementation MasterViewController

- (void)awakeFromNib {
	self.clearsSelectionOnViewWillAppear = NO;
	self.contentSizeForViewInPopover = CGSizeMake(320.0, 600.0);
    [super awakeFromNib];
}

- (void)viewDidLoad {
    [super viewDidLoad];
	// Do any additional setup after loading the view, typically from a nib.
	// initialize the _objects array to create the different sections navigation.
	
	_objects = [NSArray arrayWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"Documentation" ofType:@"plist"]];
	self.detailViewController = (DetailViewController *)[[self.splitViewController.viewControllers lastObject] topViewController];
}

-(void) viewDidAppear:(BOOL)animated{
    self.detailViewController.welcome.hidden = NO;
    self.detailViewController.description.hidden = YES;
    self.detailViewController.snippet.hidden = YES;
    self.detailViewController.navigationItem.title = @"Requirements";
}

-(void) viewDidDisappear:(BOOL)animated{
    self.detailViewController.welcome.hidden = YES;
    self.detailViewController.description.hidden = NO;
    self.detailViewController.snippet.hidden = NO;
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
	return _objects.count;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:@"Cell" forIndexPath:indexPath];

	NSDictionary *object = _objects[indexPath.row];
	cell.textLabel.text = object[@"name"];
	
    return cell;
}

-(void) prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender{
	//get the segue receptor an set the functions array.
    FunctionsViewController* fvc = (FunctionsViewController*)[segue destinationViewController];
	fvc.functions = _objects[_index][@"functions"];
	fvc.overview = _objects[_index][@"overview"];
    
    [self.detailViewController.description loadHTMLString:_objects[_index][@"overview"] baseURL:nil];
	self.detailViewController.snippet.text = @"";
	self.detailViewController.trigger.enabled = NO;
    self.detailViewController.navigationItem.title = _objects[_index][@"name"];
}

-(NSIndexPath *) tableView:(UITableView *)tableView willSelectRowAtIndexPath:(NSIndexPath *)indexPath{
	_index = indexPath.row;
    return indexPath;
}

@end
