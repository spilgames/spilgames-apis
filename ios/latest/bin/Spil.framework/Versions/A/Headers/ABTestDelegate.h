//
//  ABTestDelegate.h
//  Spil
//
//  Created by Ignacio Calderon on 11/5/12.
//  Copyright (c) 2012 Spil Games. All rights reserved.
//

/** 
 * Protocol to listen the events trigerred by the A/B Testing subsystem
 */
@protocol ABTestDelegate <NSObject>

/**
 * Method to call back after the a/b test subsystem is successfully started.
 */
-(void) abtestSessionDidStart;

/**
 * Method to call back after the a/b test subsystem is successfully ended.
 */
-(void) abtestSessionDidEnd;

/**
 * Method to call back after the a/b test subsystem receive the differences to apply over the original version.
 * The differences come expressed as an array of objects. These objects are represented as dictionaries, where,
 * always are defined the following keys:
 * <ul>
 *	<li><b>uid</b>: an ID for this resource to test. A resource can contain many elements to test. Details in the next entry. </li>
 *	<li><b>diff</b>: a dictionary with all the changes to apply to this resource. In this resource, many elements could be changed,
 *	for each element, an entry will appear in this dictionary. Each of this entry will contain a dictionary with exactly 2 keys:
 *	"new" and "old", refering to the original and value to replace with.</li>
 *	<li><b>item_class</b>: unused for the moment.</li>
 * </ul>
 */
-(void) abtestSessionDiffReceived:(NSArray*)diffs;

@end