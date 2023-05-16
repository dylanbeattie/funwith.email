# funwith.email
A demo app I built as part of my talk "Email vs Capitalism, or Why We Can't Have Nice Things"

## What it does:

Users browse to https://funwith.email/

A form asks them to enter their email address.

When they do, the app will:

* generate a unique identifier for them
* Queue an email message to be sent
* Show a page saying "thanks! Now check your email!"

The email message contains a link to verify receipt. When they click the link, it opens https://funwith.email/verify/{GUID}

Their email is marked as received.

## Behind the scenes:

https://funwith.email/status shows the number of emails in the system - LIVE (we'll use SignalR for this)

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



