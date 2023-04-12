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
