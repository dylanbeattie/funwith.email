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





