# funwith.email
A demo app I built as part of my talk "Email vs Capitalism, or Why We Can't Have Nice Things"

## What it does:

1. Users browse to https://funwith.email/go/. A form asks them to enter their email address.
2. If the address is valid, a new MailItem is created and added to the Tracker. The Tracker maintains a record of all mail items since the app was last restarted (it's a singleton component wrapped around a `ConcurrentDictionary`).
3. The user is redirected to `/go/wait/{guid}` - the "please stand up and wait for the signal" page
4. When I give the signal, they click the "Go!" button
5. When they click "Go", the app will queue their email message to be sent, update the tracker for that mail item, and show "Thanks - now check your email."

The email message contains a link to verify receipt. When they click the link, it opens https://funwith.email/verify/{GUID}

Their email is marked as received, and the user gets a message saying "thank you! You can sit down now."

Email status

* **Accepted** - email's in the system and will be sent when they get the signal
* **Queued** - email has been added to the task queue
* **Sent** - email has been sent.
* **DeliveredToInbox**  - the user's received the mail and clicked the link.
* **DeliveredToJunk** - the user's recieved the email but clicked the link saying it went to their junk mail
* **Error** - something went wrong sending the email.

## Behind the scenes:

https://funwith.email/status shows the number of emails in the system - LIVE (we'll use SignalR for this)

* 
* How many are queued (but not sent yet)
* How many are sent (but not verified yet)
* How many are verified

There's no database: messages are stored in memory in a big static dictionary, so nothing's ever persisted to storage.

Actions:

GET /home/index - shows the email input prompt

POST /home/index

Takes a single string input "email"

* Generates a GUID
* Adds an entry to the status dictionary [GUID] with (email, status = Queued)
* Adds a `Func<ValueTask>` to the background worker service which actually sends the emails
* 

Settings and User Secrets

SMTP settings for local/dev systems are managed using dotnet user secrets.

All of the user secret JSON files are stored in a single folder. On Windows it is `%APPDATA%\Microsoft\UserSecrets`, and on Linux/MacOS it is `~/.microsoft/usersecrets`.

All you need to do is take all of those JSON files and put them in the same folder on the other machine. And you are done!

This does raise the point again that *user secrets* isn't really a secret store. It uses un-encrypted JSON files stored in a folder that is not part of your project folder. The main usefulness comes from the fact that you can't accidentally commit them to version control. And they also enable per-developer settings. The name might not be the best one :)



