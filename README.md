# PhatTools
A simple set of tools for Streamer Bot to make doing a couple things easier in code. Nothing special really.

Download the latest: https://github.com/Phat32/PhatTools/releases

## RestManager

```csharp
RestManager(string Url)
```
  - Url - The root URL that will be used for all requests

```csharp
restManager.Get(string endPoint, IEnumerable<KeyValuePair<string, string>> args = null)
```
  - endPoint - The HTTP endpoint to consume. Do not include parameters or query string, use args for this.
  - args (optional) - A collections of parameters to be added to the request

```csharp
restManager.Post(string endPoint, object obj, IEnumerable<KeyValuePair<string, string>> args = null)
```
  - endPoint - The HTTP endpoint to consume. Do not include parameters or query string, use args for this.
  - obj - The Object that will be transformed into JSON and included as the body of the POST request. If passing JSON as the object, convert to string.
  - args (optional) - A collections of parameters to be added to the request

```csharp
restManager.DeleteFromBody(string endPoint, object obj)
```
  - endPoint - The HTTP endpoint to consume
  - obj - The object that will be transformed into JSON and included as the body in the DELETE request.
  
```csharp
restManager.DeleteFromParam(string endPoint, KeyValuePair<string, string> arg)
```
  - endPoint - The HTTP endpoint to consume. Do not include parameters or query string, use args for this.
  - arg - The item to delete using the query string

## JsonManager

```csharp
JsonManager(string jsonText)
```
  - jsonText - Parses the text into a JSON object that is used for all requests

```csharp
JsonManager(JObject json)
```
  - json - The JSON Object that will be used for all requests

```csharp
jsonManager.KeyContains(string key)
```
  - key - The part of the key that will be checked across all keys at this level of the object to see if they contain this key to allow for partial matches.

```csharp
jsonManager.ValueContains(string value)
```
  - value - The part of the value that will be checked across all values at this level of the object to see if they contain this value to allow for partial matches.

# Example implementing PPDS HTTP Controller

```csharp
/* **************************

PPDS Streamer Bot Integration

Description: Adds integration with PPDS and Streamer Bot to allow Chatters to choose a Duck 
			as their Buddy, look up the ducks and change their buddies.

SBAuthor: Phat32
Twitch: twitch.tv/phat32

PPDS Mod: KK964
GitHub: https://github.com/KK964/PPDuckSim-HTTP-Controller

************************** */
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using PhatTools;
using Newtonsoft.Json.Linq;

public class CPHInline
{
	//Set to the URL of your PPDS instance
	//eg: http://127.0.0.1:25523
    private RestManager _restManager = new RestManager("http://127.0.0.1:25523/");
    private JsonManager _jsonManager;
    public string GetDucks()
    {
        var json = JObject.Parse(_restManager.Get("ducks").ToString());
        return json["data"].ToString();
    }

    public void Spectate(KeyValuePair<string, JToken> duck)
    {
        var json = new JObject();
        json.Add("duckId", duck.Key);
        _restManager.Post("spectate", json.ToString());
    }

    public void Name(KeyValuePair<string, JToken> duck, string name)
    {
        var json = new JObject();
        json.Add("duckId", duck.Key);
        json.Add("name", name);
        _restManager.Post("name", json.ToString());
    }

    public bool FindDuck()
    {
        var key = args["rawInput"].ToString();
        JsonManager jsonManager = new JsonManager(GetDucks());
        var duck = jsonManager.KeyContains(key);
        if (!string.IsNullOrWhiteSpace(duck.Key))
        {
            if (!string.IsNullOrWhiteSpace(duck.Value.ToString()) && duck.Value.ToString() != "Pick Me")
            {
                CPH.SendMessage(key + " duck is @" + duck.Value.ToString() + " buddy");
            }
            else
            {
                CPH.SendMessage(key + " duck is currently a free floating duck looking for a buddy");
            }

            Spectate(duck);
            return true;
        }
        else
        {
            CPH.SendMessage("Sorry, I can't find a " + key + "(sp?) duck currently in the pool. Try checking back later, they might just join us.");
        }

        return false;
    }

    public bool MyDuck()
    {
        var user = args["user"].ToString();
        JsonManager jsonManager = new JsonManager(GetDucks());
        var duck = jsonManager.ValueContains(user);
        if (!string.IsNullOrWhiteSpace(duck.Key))
        {
            CPH.SendMessage("@" + user + " your buddy Duck is " + duck.Key);
            Spectate(duck);
        }
        else
        {
            CPH.SendMessage("Doesn't seem like you have selected a Duck to be your buddy, take a look!");
        }

        return true;
    }

    public bool AssignDuck()
    {
        var key = args["rawInput"].ToString();
        var user = args["user"].ToString();
        JsonManager jsonManager = new JsonManager(GetDucks());
        var newDuck = jsonManager.KeyContains(key);
        var oldDuck = jsonManager.ValueContains(user);
        if (!string.IsNullOrWhiteSpace(newDuck.Key))
        {
            if (!string.IsNullOrWhiteSpace(newDuck.Value.ToString()) && newDuck.Value.ToString() != "Pick Me")
            {
                if (newDuck.Value.ToString() == user)
                {
                    CPH.SendMessage("You are such a silly goose @" + user + "! " + key + " Duck is already your buddy!");
                }
                else
                {
                    CPH.SendMessage("Sorry @" + user + ", but the " + key + " Duck is already buddies with @" + newDuck.Value.ToString());
                }
            }
            else
            {
                Name(newDuck, user);
                CPH.SendMessage("You got it! " + newDuck.Key + " is now your buddy @" + user + "!");
                CPH.Wait(500);
                Spectate(newDuck);
                if (!string.IsNullOrWhiteSpace(oldDuck.Key))
                {
                    CPH.Wait(500);
                    Name(oldDuck, "Pick Me");
                    CPH.SendMessage(oldDuck.Key + " is now free floating looking for a buddy!");
                }

                return true;
            }
        }
        else
        {
            CPH.SendMessage("Sorry, I can't find a Duck " + key + "(sp?) currently in the pool. Try checking back later, they might just join us.");
        }

        return false;
    }
}
```
