//
//  Spil+Ads.h
//  Spil
//
//  Created by Ignacio Calderon on 11/6/13.
//  Copyright (c) 2013 Spil Games. All rights reserved.
//

#import "Spil+Core.h"

@interface Spil (Ads)

/**
 * Sets the AdsDelegate and receive the proper notifications from it. Without the delegate
 * this subsystem is disabled.
 * @param	delegate The delegate to handle the events generated by the Ads subsystem.
 */
-(void) setAdsDelegate:(id<AdsDelegate>)delegate;

/**
 * Turns on/off if the ads should be displayed. The ads are displayed by default. 
 * @param	state	Indicates if the ads should be displayed or not.
 */
-(void) adsEnabled:(BOOL)state;

/**
 * Shows the More Games screen right away.
 */
-(void) adsShowMoreGames;

/**
 * Shows an ad right away, using the default location. 
 * @see adsNextInterstitial:
 */
-(void) adsNextInterstitial;

/**
 * Shows an ad right away, using the specified location.
 * Use the location parameter to indicate where the ad is being displayed, for instance, 
 * "mainmenu", "store", "pausemenu". This helps to improve the advertisement campaigns.
 * @param	location	Location to be used for this interstitial
 */
-(void) adsNextInterstitial:(NSString*)location;

/**
 * Caches the next interstitial image to speed up the load time. Uses the default location.
 * @see adsCacheNextInterstitial:
 */
-(void) adsCacheNextInterstitial;

/**
 * Caches the next interstitial image to speed up the load time.
 * Use the location parameter to indicate where the ad is being displayed, for instance,
 * "mainmenu", "store", "pausemenu". This helps to improve the advertisement campaigns.
 * @param	location	Location to be used for this interstitial
 */
-(void) adsCacheNextInterstitial:(NSString*)location;

/**
 * Sets the delegate to handle the events received by the in-game ads system.
 * Without this delegate the InGameAds subsytem is disabled.
 * @param	delegate	The ads delegate who is going to handle the events.
 */
-(void) setInGameAdsDelegate:(id<InGameAdsDelegate>)delegate;

/**
 * Makes a request to get an advert (on the default location) and return it to the invoker when it's done through the
 * InGameAdsDelegate implementation set up prior the call to this method.
 * This methods returns right away and gives NO if there is not a valid chartboost instance, YES otherwise.
 * Although this method returns YES, it doesn't mean the ad will be in fact retrieved and returned.
 * @see adsRequestInGameAd:atLocation:
 * @param	size	The width and height desired for this ad.
 * @return	NO if there chartboost provider is not valid. YES otherwise.
 */
-(BOOL) adsRequestInGameAd:(CGSize)size;

/**
 * Makes a request to get an advert and return it to the invoker when it's done through the
 * InGameAdsDelegate implementation set up prior the call to this method.
 * Use the location parameter to indicate where the ad is being displayed, for instance,
 * "mainmenu", "store", "pausemenu". This helps to improve the advertisement campaigns.
 * This methods returns right away and gives NO if there is not a valid chartboost instance, YES otherwise.
 * Although this method returns YES, it doesn't mean the ad will be in fact retrieved and returned.
 * @param	size	The width and height desired for this ad.
 * @param	location	Location to be used for this interstitial
 * @return	NO if there chartboost provider is not valid. YES otherwise.
 */
-(BOOL) adsRequestInGameAd:(CGSize)size atLocation:(NSString*)location;

/**
 * Retrieves the JSON description of the assets, and pass it back to the invoker block
 * a decoded JSON format.
 * The invoker is the responsable for scale and download the asset if it's present.
 * The usage of this method if discourage and should only be used if you know the whole
 * Chartboost's workflow properly, use adsRequestInGameAd: instead.
 * @see adsRequestInGameAd:
 * @param	callback	A callback that will receive the information as a dictionary that can be serialized.
 */
-(void) adsRequestInGameAdAsset:(void(^)(NSDictionary*, NSError*))callback;

/**
 * Retrieves the JSON description of the assets, and pass it back to the invoker block
 * a decoded JSON format.
 * The invoker is the responsable for scale and download the asset if it's present.
 * Use the location parameter to indicate where the ad is being displayed, for instance,
 * "mainmenu", "store", "pausemenu". This helps to improve the advertisement campaigns.
 * The usage of this method if discourage and should only be used if you know the whole
 * Chartboost's workflow properly, use adsRequestInGameAd:atLocation: instead. 
 * @param	callback	A callback that will receive the information as a dictionary that can be serialized.
 * @param	location	Location to be used for this interstitial
 */
-(void) adsRequestInGameAdAsset:(void(^)(NSDictionary*, NSError*))callback atLocation:(NSString*)location;

/**
 * Marks the ad as shown, this will be use only for the unity plugin. Use this method if and only if you used
 * adsRequestInGameAdAsset to get the asset and render the ad by yourself.
 * The usage of this method if discourage and should only be used if you know the whole
 * Chartboost's workflow properly.
 * @param	adId	The advert id returned by chartboost assets lib.
 */
-(void) adsMarkInGameAdAsShown:(NSString*)adId;

@end
