# funwith.email
A demo app I built as part of my talk "[Email vs Capitalism, or Why We Can't Have Nice Things](https://dylanbeattie.net/talks/email-vs-capitalism.html)"

The demo app is hosted at [https://funwith.email/](https://funwith.email/), although I only enable the mail functions when I'm actually delivering the talk, otherwise it could be used to send junk.

## What it does:

1. Users browse to https://funwith.email/go/. A form asks them to enter their email address.
2. If the address is valid, a new MailItem is created and added to the Tracker. The Tracker maintains a record of all mail items since the app was last restarted (it's a singleton component wrapped around a `ConcurrentDictionary`).
3. The user is redirected to `/go/queue/{guid}` - the "please stand up and wait for the signal" page
4. When I give the signal, they click the "Go!" button. (The one that says DO NOT CLICK THIS BUTTON YET.)
5. When they click "Go", the app will queue their email message to be sent, update the tracker for that mail item, and show "Thanks - now check your email."

The email message contains two links - one to verify the email, the other to report that it went to their junk mail folder.

When they click the link, it opens https://funwith.email/verify/{GUID}

Their email is marked as received, and the user gets a message saying "thank you! You can sit down now."

Email status

* **Accepted** - email's in the system and will be sent when they get the signal
* **Queued** - email has been added to the task queue
* **Sent** - email has been sent.
* **DeliveredToInbox**  - the user's received the mail and clicked the link.
* **DeliveredToJunk** - the user's recieved the email but clicked the link saying it went to their junk mail
* **Error** - something went wrong sending the email.

## Behind the scenes:

https://funwith.email/status shows the number of emails in the system. Email statuses update in real time using SignalR.

When an email is sent, it's added to the Tracker, and then placed in the MailQueue. The queue is a wrapped around a [System.Threading.Channels.Channel](https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/) - an asynchronous way of passing data between threads hosted in the same process. This means the app can run multiple MailSenders in parallel, with round-robin style load distribution - when a new item is placed in the MailQueue, the first available MailSender will consume that item and send the associated email; if all MailSenders are busy, items remain in the MailQueue until a sender becomes available.

SMTP relays are defined in app configuration, and on startup, an instance of the MailSender hosted services is created for each SMTP relay defined in the app's config file - so if you have five sets of SMTP credentials, you get five MailSender services.

There's no database. Mail status is stored by the `StatusTracker`, which is built around a `ConcurrentDictionary`; when the application restarts, all history is lost, and email addresses are never stored anywhere except in memory.o the background worker service which actually sends the emails.



