![PMAPI logo](https://sapp.s3.amazonaws.com/github/sutpmapi.png  "")


# PMAPICSharpGeneric
A generic C# library for [Sign-Up.to's Permission Marketing API (PMAPI)](https://dev.sign-up.to/)

## Examples

For full documentation on Sign-Up.to's Permission Marketing API please refer to the [dev site](https://dev.sign-up.to/)

### Example 1: Adding a folder

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

### Example 2: Retrieving subscribers

```csharp
Client client = new Client(new HashAuthenticator(1234, 12345, "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"));
Hashtable attributes = new Hashtable();

Response response = client.get("subscriber", attributes);

if (response.IsError)
{
  // Something went wrong
  Console.WriteLine(response.Error.Message);
}
else
{                
  foreach(var subscriber in response.Data)
  {
    Console.WriteLine("Email address:" + subscriber["email"] + " id:" + subscriber["id"]);
  }
}

```
