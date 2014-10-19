#import <Foundation/Foundation.h>
#import <Tapjoy/Tapjoy.h>

@interface TapjoyEventPlugin : NSObject
{
	// This is set to the guid of the object that will implement the callback functions for handling Tapjoy events.
	NSString *myGuid_;	
}

@property (nonatomic, copy) NSString *myGuid;

+ (id)createEventWithGuid:(NSString*)guid;

// Tapjoy Events
- (void)sendEventComplete:(TJEvent*)event withContent:(BOOL)contentIsAvailable;
- (void)sendEventFail:(TJEvent*)event error:(NSError*)error;
- (void)contentWillAppear:(TJEvent*)event;
- (void)contentDidAppear:(TJEvent*)event;
- (void)contentWillDisappear:(TJEvent*)event;
- (void)contentDidDisappear:(TJEvent*)event;
- (void)event:(TJEvent*)event didRequestAction:(TJEventRequest*)request;

@end