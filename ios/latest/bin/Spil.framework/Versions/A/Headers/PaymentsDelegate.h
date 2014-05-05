//
//  PaymentsDelegate.h
//  Spil
//
//  Created by Ignacio Calderon on 10/03/14.
//  Copyright (c) 2014 Spil Games. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PaymentsProduct.h"

/**
 * Protocol to handle the events triggered by the payments subsystem.
 */
@protocol PaymentsDelegate <NSObject>

/**
 * Method to call back after the payments subsystem is successfully started.
 * Can be used to notify the game that can request payments.
 */
-(void) paymentsDidStart;

/**
 * Method to call back after if the payments subsystem couldn't be started due to any reason.
 * Can be used to track the problem, or to disable the some functionalities of the game, or
 * to use some placeholder images instead.
 * @see Spil.trackEvent:
 * @param	error	The reason why the payments subsystem failed to start.
 */
-(void) paymentsDidFailToStart:(NSError*)error;

/**
 * Method to call back after the list of availables products is retrieved from apple servers.
 * The parameter is a list of PaymentsProduct objects, that has to be store in order to request the transactions
 * upon the user's actions.
 * @param   products    A list of PaymentsProduct objects containing information about the available items.
 */
-(void) paymentsDidReceiveProductList:(NSArray*)products;

/**
 * Method to call back after a transaction was successfully charged to the user's account. 
 * It receives the transaction ID for auditing purposes (if needed) and the product that was purchased with that 
 * trasaction in order to deliver the expected content to the user.
 * @param   transactionID   The transaction id on the AppStore for this purchase.
 * @param   product         The product object that was requested for this transaction.
 */
-(void) paymentsTransactionDidSucceed:(NSString*)transactionID forProduct:(PaymentsProduct*)product;

/**
 * Method to call back after a transaction failed to be charged to the user's account.
 * It receives an error object with the reason of the failure.
 * @param	error	The reason why the transaction failed.
 */
-(void) paymentsTransactionDidFail:(NSError*)error;

/**
 *
 */
-(void) paymentsDidRestoreProduct:(PaymentsProduct*)product fromTransaction:(NSString*)transactionID;

@end
