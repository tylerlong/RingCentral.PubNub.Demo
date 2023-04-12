using RingCentral;
using RingCentral.Net.PubNub;
using dotenv.net;

DotEnv.Load();

var rc = new RestClient(
    Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_ID"),
    Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_SECRET"),
    Environment.GetEnvironmentVariable("RINGCENTRAL_SERVER_URL")
);
await rc.Authorize(Environment.GetEnvironmentVariable("RINGCENTRAL_JWT_TOKEN"));
var pubNubExtension = new PubNubExtension();
await rc.InstallExtension(pubNubExtension);

var eventFilters = new[]
{
    "/restapi/v1.0/account/~/extension/~/message-store"
};
var subscription = await pubNubExtension.Subscribe(eventFilters, message =>
{
    Console.WriteLine("I got a notification:");
    Console.WriteLine(message);
});

// Wait for 60 seconds before the app exits
// In the mean time, send SMS to trigger a notification for testing purpose
await Task.Delay(60000);

/*
 * Sample output:
 I got a notification:
{"uuid":"2848583747434852364","event":"/restapi/v1.0/account/37439510/extension/850957020/message-store","timestamp":"2023-04-12T19:17:36.821Z","subscriptionId":"3ce48c91-e4c0-4123-86be-0e36c4e1c6ba","ownerId":"850957020","body":{"accountId":37439510,"extensionId":850957020,"lastUpdated":"2023-04-12T19:17:24.693Z","changes":[{"type":"SMS","newCount":1,"updatedCount":0,"newMessageIds":[1729816321020]}]}}
 */
 