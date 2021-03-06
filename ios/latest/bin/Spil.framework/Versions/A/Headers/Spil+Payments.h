//
//  Spil+Payments.h
//  Spil
//
//  Created by Ignacio Calderon on 12/03/14.
//  Copyright (c) 2014 Spil Games. All rights reserved.
//

#import "Spil+Core.h"

@interface Spil (Payments)

/**
 * Sets the PaymentsDelegate and receive the proper notifications from it. Without the delegate
 * this subsystem is disabled.
 * @param	delegate The delegate to handle the events generated by the Payments subsystem.
 */
-(void) setPaymentsDelegate:(id<PaymentsDelegate>)delegate;

/**
 * Requests a payment transaction over the provided product for a quantity specified.
 * The product object it's provided as a response on the PaymentsDelegate.paymentsDidReceiveProductList: function.
 * Keep those object to start a transaction when needed.
 * @param   product     The product that the user wants to purchase.
 * @param   quantity    How many units the user wants to buy in this transaction.
 */
-(void) paymentsRequestTransaction:(PaymentsProduct*)product quantity:(int)quantity;

/**
 * Notifies Apple that the transaction has been finalized and the content has been delivered to the user.
 * You need a valid transaction id in order to close the transaction, it's not valid this method does nothing.
 * @param   transactionID   The transaction that will be closed.
 */
-(void) paymentsFinishTransaction:(NSString*)transactionID;

@end
