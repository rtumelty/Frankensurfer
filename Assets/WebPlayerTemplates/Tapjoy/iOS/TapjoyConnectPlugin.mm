#import "TapjoyConnectPlugin.h"
#import "TapjoyEventPlugin.h"

UIViewController *UnityGetGLViewController();

static TapjoyConnectPlugin *_sharedInstance = nil; //To make TapjoyConnect Singleton

@implementation TapjoyConnectPlugin

@synthesize keyFlagValueDict = keyFlagValueDict_, callbackHandlerName = callbackHandlerName_, displayAdSize = displayAdSize_, displayAdOrientation = displayAdOrientation_, displayAdFrame = displayAdFrame_;;
@synthesize eventsDict = eventsDict_;
@synthesize enableDisplayAdAutoRefresh;

+ (void)initialize
{
	if (self == [TapjoyConnectPlugin class])
	{
		_sharedInstance = [[self alloc] init];
	}
}


+ (TapjoyConnectPlugin*)sharedTapjoyConnectPlugin
{
	return _sharedInstance;
}


- (id)init
{
	self = [super init];
    
    if (self)
    {
        tapPoints = 0;
        displayAdSize_ = TJC_DISPLAY_AD_SIZE_320X50;
        displayAdFrame_ = CGRectMake(0, 0, 320, 50);

        [Tapjoy setViewDelegate:self];
        [Tapjoy setVideoAdDelegate:self];
        
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(tjcConnectSuccess:)
                                                     name:TJC_CONNECT_SUCCESS
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(tjcConnectFail:) name:TJC_CONNECT_FAILED object:nil];
        
        // Add an observer for when Tap Points has been successfully retrieved.
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(getUpdatedPoints:)
                                                     name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(spendPoints:)
                                                     name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(awardPoints:)
                                                     name:TJC_AWARD_TAP_POINTS_RESPONSE_NOTIFICATION
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(getUpdatedPointsError:)
                                                     name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(spendPointsError:)
                                                     name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(awardPointsError:) 
                                                     name:TJC_AWARD_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR 
                                                   object:nil];
        
        // Add an observer for when a user has successfully earned currency.
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(showEarnedCurrencyAlert:) 
                                                     name:TJC_TAPPOINTS_EARNED_NOTIFICATION 
                                                   object:nil];
        
        // Get fullscreen ad Call
        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getFullScreenAd:) 
                                                     name:TJC_FULL_SCREEN_AD_RESPONSE_NOTIFICATION 
                                                   object:nil];

        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getFullScreenAdError:) 
                                                     name:TJC_FULL_SCREEN_AD_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];

        // Get daily reward ad Call
        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getDailyRewardAd:) 
                                                     name:TJC_DAILY_REWARD_RESPONSE_NOTIFICATION 
                                                   object:nil];

        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getDailyRewardAdError:) 
                                                     name:TJC_DAILY_REWARD_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];

        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(showOffersError:)
                                                     name:TJC_OFFERS_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];
    }
	return self;
}


- (BOOL)hasKeyFlag
{
    if (keyFlagValueDict_)
        return YES;
	return NO;
}


- (void)setFlagKey:(NSString*)key Value:(NSString*)value
{
	if (!keyFlagValueDict_)
		keyFlagValueDict_ = [[NSMutableDictionary alloc] init];
	[keyFlagValueDict_ setObject:value forKey:key];
}


- (void)tjcConnectSuccess:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapjoyConnectSuccess", "success");
}


- (void)tjcConnectFail:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapjoyConnectFail", "failure");
}


- (void)getUpdatedPoints:(NSNotification*)notifyObj
{
    tapPoints = [notifyObj.object intValue];
	
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsLoaded", [[NSString stringWithFormat:@"%i", tapPoints] UTF8String]);
}


- (void)getUpdatedPointsError:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsLoadedError", "Error loading Tap Points");
}


- (void)spendPoints:(NSNotification*)notifyObj
{
	tapPoints = [notifyObj.object intValue];
    
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsSpent", [[NSString stringWithFormat:@"%i", tapPoints] UTF8String]);
}


- (void)spendPointsError:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsSpendError", "Error spending Tap Points");
}


- (void)awardPoints:(NSNotification*)notifyObj
{
	tapPoints = [notifyObj.object intValue];
	
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsAwarded", [[NSString stringWithFormat:@"%i", tapPoints] UTF8String]);
}


- (void)awardPointsError:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsAwardError", "Error awarding Tap Points");
}


- (int)queryTapPoints
{
	return tapPoints;
}


- (void)getFullScreenAd:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of fullscreen ad
	// or its Max Number of count has exceeded its limit
	UnitySendMessage([callbackHandlerName_ UTF8String], "FullScreenAdLoaded", "success");
}


- (void)getFullScreenAdError:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of fullscreen ad
	// or its Max Number of count has exceeded its limit
	UnitySendMessage([callbackHandlerName_ UTF8String], "FullScreenAdError", "failure");
}


- (void)getDailyRewardAd:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of daily reward ad
	UnitySendMessage([callbackHandlerName_ UTF8String], "DailyRewardAdLoaded", "success");
}


- (void)getDailyRewardAdError:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of daily reward ad
	UnitySendMessage([callbackHandlerName_ UTF8String], "DailyRewardAdError", "failure");
}


- (void)showEarnedCurrencyAlert:(NSNotification*)notifyObj
{
	NSNumber *tapPointsEarned = notifyObj.object;
	earnedCurrencyAmount = [tapPointsEarned intValue];
	
	UnitySendMessage([callbackHandlerName_ UTF8String], "CurrencyEarned", [[NSString stringWithFormat:@"%i", earnedCurrencyAmount] UTF8String]);
}

- (void)showOffersError:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "ShowOffersError", "failure");
}

- (void)moveDisplayAdToX:(int)x toY:(int)y
{
	displayAdFrame_.origin = CGPointMake(x, y);
}


- (void)showDisplayAd
{
	// Get the TJCAdView, which is a subclass of UIView.
    TJCAdView *adView = [Tapjoy getDisplayAdView];
    // Set the frame, in case moveDisplayAd.. has changed it.
	[adView setFrame:displayAdFrame_];
    // Get the Unity GL ViewController
    UIViewController *glView = UnityGetGLViewController();
    [glView.view addSubview:(UIView *)adView];
}


- (void)hideDisplayAd
{
	UIView *adView = (UIView*)[Tapjoy getDisplayAdView];
	
	[adView removeFromSuperview];
}


- (void)dealloc
{
	[super dealloc];
}

- (BOOL)shouldRefreshAd
{
       return [self shouldDisplayAdAutoRefresh];
}

#pragma mark Tapjoy Display Ads Delegate Methods

- (void)didReceiveAd:(TJCAdView*)adView
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "DisplayAdLoaded", "success");
}


- (void)didFailWithMessage:(NSString*)msg
{
	NSLog(@"No Tapjoy Display Ads available");
}


- (NSString*)adContentSize
{
	return displayAdSize_;
}


#pragma mark Tapjoy Video Ads Delegate Methods

- (void)videoAdBegan
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "VideoAdStart", "Video Ad has started");
}


- (void)videoAdClosed
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "VideoAdComplete", "Video Ad has been closed");
}

- (void)videoAdError:(NSString *)errorMsg
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "VideoAdError", [errorMsg UTF8String]);
}


#pragma mark Tapjoy View Delegate Methods

- (void)viewDidAppearWithType:(int)viewType
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "ViewOpened", [[NSString stringWithFormat:@"%i", viewType] UTF8String]);
}

- (void)viewDidDisappearWithType:(int)viewType
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "ViewClosed", [[NSString stringWithFormat:@"%i", viewType] UTF8String]);
}

#pragma mark Tapjoy Event Methods

- (void)createEventWithGuid:(NSString *)guid name:(NSString *)name parameter:(NSString *)parameter
{
	// Create dictionary if its empty
	if (!eventsDict_)
		eventsDict_ = [[NSMutableDictionary alloc] init];
	
	// TODO: error check on guid
	TapjoyEventPlugin *tjevt = [TapjoyEventPlugin createEventWithGuid:guid];

	// TODO: Allow for string param to be used
	TJEvent *evt = [TJEvent eventWithName:name delegate:tjevt];
	[eventsDict_ setObject:evt forKey:guid];

	// TODO: Send success fail callback?
}

- (void)sendEventWithGuid:(NSString *)guid
{
	// TODO: nil check
	[[eventsDict_ objectForKey:guid] send];
}

- (void)presentEventWithGuid:(NSString *)guid
{
	// TODO: nil check
	[[eventsDict_ objectForKey:guid] presentContentWithViewController:UnityGetGLViewController()];
}

#pragma mark - Tapjoy Static Event Delegate Methods

- (void)sendEventComplete:(NSString *)guid withContent:(BOOL)contentIsAvailable
{
	if (contentIsAvailable)
		UnitySendMessage([callbackHandlerName_ UTF8String], "SendEventCompleteWithContent", [guid UTF8String]);
	else
		UnitySendMessage([callbackHandlerName_ UTF8String], "SendEventComplete", [guid UTF8String]);
}

- (void)sendEventFail:(NSString *)guid error:(NSError*)error
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "SendEventFail", [guid UTF8String]);
}

- (void)contentWillAppear:(NSString *)guid
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "ContentWillAppear", [guid UTF8String]);
}

- (void)contentDidAppear:(NSString *)guid
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "ContentDidAppear", [guid UTF8String]);
}

- (void)contentWillDisappear:(NSString *)guid
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "ContentWillDisappear", [guid UTF8String]);
}

- (void)contentDidDisappear:(NSString *)guid
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "ContentDidDisappear", [guid UTF8String]);
}

- (void)event:(NSString *)guid didRequestAction:(TJEventRequest*)request
{
	//TODO: use json encoding
	NSString *message = [NSString stringWithFormat: @"%@,%d,%@,%d", guid, request.type, request.identifier, request.quantity];
	UnitySendMessage([callbackHandlerName_ UTF8String], "DidRequestAction", [message UTF8String]);
}

@end







// Converts C style string to NSString
NSString* tjCreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* tjMakeStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {
	

	void _SetCallbackHandler(const char* handlerName)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] setCallbackHandlerName:[NSString stringWithUTF8String:handlerName]];
		NSLog(@"callbackHandlerName: %@", [[TapjoyConnectPlugin sharedTapjoyConnectPlugin] callbackHandlerName]);
	}
	
	
	void _RequestTapjoyConnect(const char* appID, const char* secretKey)
	{
		[Tapjoy setPlugin:@"unity"];

		if ([[TapjoyConnectPlugin sharedTapjoyConnectPlugin] keyFlagValueDict])
			[Tapjoy requestTapjoyConnect:tjCreateNSString(appID) secretKey:tjCreateNSString(secretKey) options:[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] keyFlagValueDict]];
		else
			[Tapjoy requestTapjoyConnect:tjCreateNSString(appID) secretKey:tjCreateNSString(secretKey)];
	}


	void _SetFlagKeyValue(const char* flagKey, const char* flagValue)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] setFlagKey:tjCreateNSString(flagKey) Value:tjCreateNSString(flagValue)];
	}
	
	
	void _EnableLogging(bool enable)
	{
		[Tapjoy enableLogging:enable];
	}


	void _ActionComplete(const char* actionID)
	{
		[Tapjoy actionComplete:tjCreateNSString(actionID)];
	}
	
	
	void _SetUserID(const char* userID)
	{
		[Tapjoy setUserID:tjCreateNSString(userID)];
	}
	
	
	void _ShowOffers(void)
	{
		// Displays the offer wall.
		[Tapjoy showOffersWithViewController:UnityGetGLViewController()];
	}
    
    
    void _ShowOffersWithCurrencyID(const char* currencyID, bool isSelectorVisible)
    {
        // Displays the offer wall.
		[Tapjoy showOffersWithCurrencyID:tjCreateNSString(currencyID) withViewController:UnityGetGLViewController() withCurrencySelector:isSelectorVisible];
    }	
	

	void _GetFullScreenAd(void)
	{
		// Initiates a request to get fullscreen ad data.
		[Tapjoy getFullScreenAd];
	}
    
    
    void _GetFullScreenAdWithCurrencyID(const char* currencyID)
    {
        // Initiates a request to get fullscreen ad data.
		[Tapjoy getFullScreenAdWithCurrencyID:tjCreateNSString(currencyID)];
    }


    void _SetCurrencyMultiplier(float multiplier)
    {
    	// Sets the currency multiplier, and fires off a network call to notify the server.
    	[Tapjoy setCurrencyMultiplier:multiplier];
    }
	

	void _ShowFullScreenAd(void)
	{
		// Shows the default full screen fullscreen ad ad.
		[Tapjoy showFullScreenAdWithViewController:UnityGetGLViewController()];
	}


	void _GetDailyRewardAd(void)
	{
		[Tapjoy getDailyRewardAd];
	}


	void _GetDailyRewardAdWithCurrencyID(const char* currencyID)
	{
		[Tapjoy getDailyRewardAdWithCurrencyID:tjCreateNSString(currencyID)];
	}


	void _ShowDailyRewardAd(void)
	{
		// Shows the default daily reward ad.
		[Tapjoy showDailyRewardAdWithViewController:UnityGetGLViewController()];
	}
	
	
	void _ShowDefaultEarnedCurrencyAlert(void)
	{
		// Pops up a UIAlert notifying the user that they have successfully earned some currency.
		// This is the default alert, so you may place a custom alert here if you choose to do so.
		[Tapjoy showDefaultEarnedCurrencyAlert];
	}
	
	
	void _GetTapPoints(void)
	{	
		[Tapjoy getTapPoints];
	}
	
	
	void _SpendTapPoints(int points)
	{
		[Tapjoy spendTapPoints:points];
	}
	
	
	void _AwardTapPoints(int points)
	{	
		[Tapjoy awardTapPoints:points];
	}
	
	
	int _QueryTapPoints(void)
	{
		return [[TapjoyConnectPlugin sharedTapjoyConnectPlugin] queryTapPoints];
	}
	
	
	void _GetDisplayAd(void)
	{
		[Tapjoy getDisplayAdWithDelegate:[TapjoyConnectPlugin sharedTapjoyConnectPlugin]];
	}
	

	void _ShowDisplayAd(void)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] showDisplayAd];
	}


	void _HideDisplayAd(void)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] hideDisplayAd];
	}
	

    void _GetDisplayAdWithCurrencyID(const char* currencyID)
	{
		[Tapjoy getDisplayAdWithDelegate:[TapjoyConnectPlugin sharedTapjoyConnectPlugin] currencyID:tjCreateNSString(currencyID)];
	}
    
    
	void _SetDisplayAdSize(const char* size)
	{
        NSString* adSize = tjCreateNSString(size);
    
        NSArray* sizeComponents = [adSize componentsSeparatedByString:@"x"];
        if (sizeComponents && sizeComponents.count == 2)
            [TapjoyConnectPlugin sharedTapjoyConnectPlugin].displayAdFrame = CGRectMake(0, 0, [sizeComponents[0] floatValue], [sizeComponents[1] floatValue]);
    
        [[TapjoyConnectPlugin sharedTapjoyConnectPlugin] setDisplayAdSize:adSize];
	}


	void _EnableDisplayAdAutoRefresh(bool enable)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] setEnableDisplayAdAutoRefresh:enable];
	}
	
    
	void _MoveDisplayAd(int x, int y)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] moveDisplayAdToX:x toY:y];
	}
	
	
	void _SetTransitionEffect(int transition)
	{
		[Tapjoy setTransitionEffect:(TJCTransitionEnum)transition];
	}
	

	void _SendIAPEvent(const char*  name, float price, int quantity, const char*  currencyCode)
	{
		[Tapjoy sendIAPEvent:tjCreateNSString(name) price:price quantity:quantity currencyCode:tjCreateNSString(currencyCode)];
	}

	//#pragma mark - Tapjoy Event C Layer

	void _CreateEvent(const char* guid, const char* name, const char* param)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] createEventWithGuid:tjCreateNSString(guid) name:tjCreateNSString(name) parameter:tjCreateNSString(param)];
	}

	void _SendEvent(const char* guid)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] sendEventWithGuid:tjCreateNSString(guid)];
	}

	void _ShowEvent(const char* guid)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] presentEventWithGuid:tjCreateNSString(guid)];	
	}
}

