#import "TapjoyEventPlugin.h"
#import "TapjoyConnectPlugin.h"

@implementation TapjoyEventPlugin

@synthesize myGuid = myGuid_;

- (id)init
{
	self = [super init];
    
    if (self)
    {
    }

    return self;
}

+ (id)createEventWithGuid:(NSString*)guid
{
	TapjoyEventPlugin *instance = [[TapjoyEventPlugin alloc] init];
	[instance setMyGuid:guid];

	return instance;
}

#pragma mark - TJEventDelegate

- (void)sendEventComplete:(TJEvent*)event withContent:(BOOL)contentIsAvailable
{
	NSLog(@"trying to send event complete back to TapjoyConnectPlugin");
	[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] sendEventComplete:myGuid_ withContent:contentIsAvailable];
}

- (void)sendEventFail:(TJEvent*)event error:(NSError*)error
{
	NSLog(@"trying to send event fail back to TapjoyConnectPlugin");
	[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] sendEventFail:myGuid_ error:error];
}

- (void)contentWillAppear:(TJEvent*)event
{
	[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] contentWillAppear:myGuid_];
}

- (void)contentDidAppear:(TJEvent*)event
{
	[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] contentDidAppear:myGuid_];
}

- (void)contentWillDisappear:(TJEvent*)event
{
	[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] contentWillDisappear:myGuid_];
}

- (void)contentDidDisappear:(TJEvent*)event
{
	[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] contentDidDisappear:myGuid_];
}

- (void)event:(TJEvent*)event didRequestAction:(TJEventRequest*)request
{
	[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] event:myGuid_ didRequestAction:request];
}

@end
