//
//  PaymentsProduct.h
//  Spil
//
//  Created by Ignacio Calderon on 10/03/14.
//  Copyright (c) 2014 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>
@class SKProduct;

@interface PaymentsProduct : NSObject

-(id) initWithProduct:(SKProduct*)product;

@property (readwrite) NSString* localizedTitle;
@property (readwrite) NSString* localizedDescription;
@property (readwrite) NSString* localizedPrice;
@property (readwrite) NSString* productIdentifier;
@property (readwrite) BOOL downloadable;
@property (readwrite) NSArray* downloadContentLengths;
@property (readwrite) NSString* downloadContentVersion;
@end
