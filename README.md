# PMAPICSharpGeneric
A generic C# library for [Sign-Up.to's Permission Marketing API (PMAPI)](https://dev.sign-up.to/)

For full documentation on Sign-Up.to's Permission Marketing API please see their DEV site at: https://dev.sign-up.to/

## Examples

### Adding a folder

```csharp
Client client = new Client(new HashAuthenticator(1234, 12345, "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"));
Hashtable attributes = new Hashtable();

attributes.Add("name", "Example folder name");

Response response = client.post("folder", attributes);

if (response.IsError)
{
  // Something went wrong
  Console.WriteLine(response.Error.Message);
}
else
{
  Console.WriteLine("Created folder:" + response.Data[0]["name"] + " id:" + response.Data[0]["id"]);
}
```
