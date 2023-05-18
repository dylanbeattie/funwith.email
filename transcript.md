Good evening everyone! How are you all doing out there? I'm Dylan, and I'm here to talk to you about email. Because email is kind of a big deal. Our whole world runs on software now, right? Movies, music, your calendar, your train tickets, your bank account... it's all online. Some days it feels like you can't do anything in the modern world without creating an account first - and the first thing you do to create an account? Yep. Enter your email address. You activate a new service? Please click the link in your email. Forgotten your password? We'll send you an email. Want to invite one of your friends to come with you to the gym, or send them their ticket for the concert you're going to? Email. Email, email, email...

You'd think something that important would be pretty bulletproof, right? Well... we're going to do an experiment. And for this next part, I'm going to need your help... and I'm going to need to ask you to trust me. Because what I'd like you all to do is take out your phone and go to this web address. Then I'd like you to enter your email address... I promise I'm not storing these addresses anywhere, you're not going to end up on any mailing lists or for sale on the darkweb or anything... I'm literally going to store your email address in memory for as long as it takes to send you an email.

Enter your address, click "Next", and then stand up. That's right. Stand up and wait for my signal... ok? Everybody ready? I'm going to count down from three, and when I get to zero - not one, this isn't Visual Basic, when I get to ZERO, I want you to press "Send"... and then I want you to open your email client, and as soon as you receive that email, please click the link in it to confirm you've received it, and then you can sit down. Everybody understand? I'll count you in, you press Send, remain standing until that email arrives, and then sit down.

OK... three... two... one... SEND!

and now, we wait. Who's going to be first? There. One... two... three... that's pretty awesome, right? Look at that! Real time electronic communication!

I notice a few of you are still standing up... now, I guarantee that the code behind this is completely transparent. I'm not filtering, I'm not prioritising anything... all I'm doing here is trusting that the infrastructure that runs email is doing its job properly. And it's obviously working for *some* of you...

it's OK. I'm not going to leave the rest of you standing there all day. Here you go - you can have one of these exclusive limited-edition stickers. Folks, give them a big hand.

So what's going on? How did some of you get that email immediately, and some of you it took a few seconds to come through, and some of you are still waiting for it to arrive?

Well, folks, I got bad news for you. Sometimes, email doesn't work - and what's worse is that, when it breaks, the people who care normally can't fix it, and the people who can fix it normally don't care. And if you've ever been responsible for building and maintaining a system that sends email, you'll know that eventually somebody's going to blame you for it. Your design, your code, your servers - for something which is absolutely, positively not your fault.

So today, we're going to talk about why. You're going to learn way more than you ever wanted to know about email - not just how it works, but why. And we're going to start here: an email address.

Now, when electronic mail was first invented, all you needed was somebody's name - because there was no way to send email to somebody on a different computer. But that didn't matter, because there weren't very many computers. Some universities had a computer, some companies had a computer in their research department... and those computers were all time-sharing systems. You'd log in, read your mail, do a bit of work, log out... the thing on your desk was just a dumb terminal, and the actual computer would be a giant hulking thing in a basement somewhere.

Let's jump to 1969, because a bunch of *awesome* things got invented in 1969. That was the year that human beings walked on the moon. It was the year the Boeing 747 and the Lockheed SR71 first flew. It was the year of Woodstock, and the Summer of Love... and it was the year the internet got invented. Kinda. The internet as we know it actually evolved out of something called the ARPAnet, named after the Advanced Research Projects Agency, ARPA. The first computers were connected to ARPAnet in 1969 - and, fundamentally, the premise of ARPAnet was that one computer could send data to a second computer without them needing to be directly connected; if you were connected to the ARPA network, you could communicate with any other machine connected to the ARPA network.

Now, this isn't a talk about the gnarly history of computer networks. Partly because it just isn't... and partly also because every time I do a talk about the gnarly history of computer networks I get Grandpa Simpson in the YouTube comments getting really upset that I missed some obscure detail in how the network stack on the PDP-11 was implemented.

But I do want to mention one weird historical quirk: UUCP email addresses. You see, for a decade or two, there was this weird situation where quite a lot of computers were networked, but The Internet didn't really exist yet, and so "the network" was a sort of ad-hoc collection of whichever computers happened to be connected to each other right now. Mostly using 14.4 kilobit dialup modem connections.

So if you wanted to send an electronic mail to somebody who wasn't connected to the ARPAnet, you had to specify how to find them. Mail was delivered using something called the Unix-to-Unix Copying Protocol - UUCP - and email addresses looked like this:

The idea is that uw-beaver is a well-known system that everybody can connect to. So if you want to send mail to Michael Holley, you connect to uw-beaver and use UUCP to copy the email onto their system. Then, at some point, the machine called teltone is going to connect to uw-beaver and say "hey, any mail for me?" - and that mail gets transferred from uw-beaver to teltone. Then, at some later point, the machine called dataio is going to connect to teltone and say "hey, any messages?" - and the mail gets handed over from teltone to dataio. And then when MIchael Holley logs in to his account on dataio, he'll get a little prompt saying "You have mail".

This was called a UUCP bang path, because apparently Americans used to refer to the exclamation mark as a bang.

There's two things to bear in mind about this system. One: those bang paths might contain six, seven, eight different hops, especially if the person you were emailing wasn't in North America. And two: it was common for those machines to only connect to each other once a day. They'd dial up in the middle of the night, transfer any waiting mail and messages, maybe download a couple of patches or system updates, and then disconnect - so in some cases, an email could take more than a week to arrive. And, of course, if one of those machines was shut down, those emails would never get delivered.

But for folks lucky enough to be on the ARPAnet, you could just... send them an email. Modern email standards are really the result of hundreds of different people, all working on different pieces of the puzzle, borrowing ideas and code from each other... but one innovation that has really stood the test of was due to one person, Ray Tomlinson. in 1971, Ray was working at a company in Massachusetts called Bolt Beranek and Newman; he was working on a way to route electronic messages across the ARPAnet, and he had the bright idea of using the at-sign - so if you wanted to send a message to alice, and you knew alice had an account on the machine called mit-multics, you'd use Ray's SNDMSG program to send a message to alice@mit-multics.

You notice that machine name is just a name - it's not mit-multics dot anything. Well, until the late 1970s, that's how the internet worked. DNS hadn't been invented yet; there was no such thing as a top-level domain. If you needed to know the network address of uw-beaver, you looked it up in the hosts file. The hosts file still exists today, but back in the days of ARPA, it looked like this: a hostname, a numeric address - which was just a single number - and then the name and the phone number of the person who managed that system. The hosts file was managed by this lady: Elizabeth "Jake" Feinler. She ran the Network Information Systems Center at Stanford, and if you wanted to add a new computer to the internet, you'd talk to Jake. She, or one of her team, would tell you what numeric address you could use, and make a new entry in the hosts file for you.

I managed to track down a PDF of the ARPANET DIRECTORY from June 1974, which lists 89 hosts, and the contact details of around 900 people - engineers, academics, system administrators: basically a printed directory of the entire network and all the people who were connected to it. It was very open, very transparent... the technical barriers to getting any kind of connection at all were sufficiently formidable that anybody who managed to get online was considered to have earned the right to be there. But things got easier. Computers and computing time got cheaper, more and more people got access to the network via their university or their employer...

Now, there are two kinds of people in the world. There are people who look at a thing and go "hey, this is useful." And there are people who look at a thing and go "wow, lots of people find this useful, I wonder if I can use it to get rich."

In May 1978, Gary Thuerk had an idea. Gary was a marketing rep at the Digital Equipment Corporation - DEC. DEC was launching a new computer, the DEC-20, which had built-in support for ARPAnet protocols. DEC was based on the US east coast, and didn't have a whole lot of presence on the other side of the country... so Gary got somebody to go through the ARPAnet directory and type in the email addresses of every ARPAnet user in the western United States.

393 of them.

This appears to be the first instance of unsolicited commercial email: maybe the first instance anywhere of anybody using a computer network as an advertising medium. Some recipients got pretty upset about it. The University of Utah claimed that Gary Thuerk had sent so many mails that he'd crashed their system... well, technically he had, because they were so low on disk space that a single mail message was enough to crash their system and make it unbootable.

There was an official complaint: remember, ARPAnet was funded by the US Department of Defense, so if you stepped out of line, you'd get dressed down by an actual US army officer - in this instance, Major Raymond Czahor, who asked Jake to forward this to all the people who'd recived the original message:

ON 2 MAY 78 DIGITAL EQUIPMENT CORPORATION (DEC) SENT OUT AN ARPANET MESSAGE ADVERTISING THEIR NEW COMPUTER SYSTEMS. THIS WAS A FLAGRANT VIOLATION OF THE USE OF ARPANET AS THE NETWORK IS TO BE USED FOR OFFICIAL U.S. GOVERNMENT BUSINESS ONLY. APPROPRIATE ACTION IS BEING TAKEN TO PRECLUDE ITS OCCURRENCE AGAIN.

IN ENFORCEMENT OF THIS POLICY DCA IS DEPENDENT ON THE ARPANET SPONSORS, AND HOST AND TIP LIAISONS. IT IS IMPERATIVE YOU INFORM YOUR USERS AND CONTRACTORS WHO ARE PROVIDED ARPANET ACCESS THE MEANING OF THIS POLICY.

THANK YOU FOR YOUR COOPERATION.

MAJOR RAYMOND CZAHOR

CHIEF, ARPANET MANAGEMENT BRANCH, DCA

But turns out spam wasn't the only thing invented that week which would still be a massive pain in the ass nearly fifty years later... that was also the week that one Richard Stallman invented reply-guying. In a message which is very on brand for Mr Stallman, he posted:

1) I didn't receive the DEC message, but I can't imagine I would have been bothered if I have. I get tons of uninteresting mail, and system announcements about babies born, etc. At least a demo MIGHT have been interesting.
2) The amount of harm done by any of the cited "unfair" things the net has been used for is clearly very small. And if they have found any people any jobs, clearly they have done good. If I had a job to offer, I would offer it to my friends first. Is this "evil"? Must I advertise in a paper in every city in the US with population over 50,000 and then go to all of them to interview, all in the name of fairness? Some people, I am afraid, would think so. Such a great insistence on fairness would destort everyone's lives and do much more harm than good. So I state unashamedly that I am in favor of seeing jobs offered via whatever.
3) It has just been suggested that we impose someone's standards on us because otherwise he MIGHT do so. Well, if you feel that those standards are right and necessary, go right ahead and support them. But if you disagree with them, as I do, why hand your opponents the victory on a silver platter? By the suggested reasoning, we should always follow the political views that we don't believe in, and especially those of terrorists, in anticipation of their attempts to impose them on us. If those who think that the job offers are bad are going to try to prevent them, then those of us who think they are unrepugnant should uphold our views. Besides, I doubt that anyone can successfully force a site from outside to impose censorship, if the people there don't fundamentally agree with the desirability of it.
4) Would a dating service for people on the net be "frowned upon" by DCA? I hope not. But even if it is, don't let that stop you from notifying me via net mail if you start one.

... so there you go. I wasn't in this conversation, but I'm going to tell you what I think anyway, even though nobody asked for my opinion, oh, and by the way, let me know if you think there's any way I could use this to get laid.

Most people who received Gary Thuerk's email just deleted it. But... quite a few of them went along to the open days he was advertising to take a look at the new DECSYSTEM-20. And quite a few of *those* people ended up buying one. Thuerk reckons they sold "thirteen or fourteen million dollars worth" of DEC systems as a result of that email campaign.

In the aftermath of the incident, the consensus was: ARPAnet was not for marketing, don't do it again - but that the users could be trusted to behave themselves.

So there you go. When I was born, in August 1978, we already had email addresses with @-signs in them, we had spam, we had Richard Stallman using the internet to be obnoxious... in a lot of ways, it's all uncannily familiar.

Things are about to get interesting, though. You know that whole thing about how people are baby boomers, Generation X, Generation Z, millennials... well, there's a micro-generation buried in there. It's the generation I belong to: we're called Millennial Falcons, because we're the people who were born between the release of Star Wars, in 1977, and the release of Return of the Jedi, in 1983. Any of my fellow Millennial Falcons out there in the crowd today?

Turns out the biggest Millennial Falcon of them all is the internet itself, because the fundamental protocols that form the foundation of the internet, even today, were all created during that window. When Star Wars came out in 1977, computers connected to the ARPAnet, network addresses were just a single number, and they communicated using a thing called NCP - the Network Control Protocol.

By the time Return of the Jedi came out in 1983, the ARPAnet had become the Internet. TCP/IP had replaced NCP - complete with those dotted-quad IPv4 addresses that we're still using today. The HOSTS file maintained by Jake and her team at Stanford had been replaced by a distributed database called the Domain Naming System - DNS. Hostnames weren't just a single machine name any more: they were hierarchical.

And when it came to sending email, there was a new player in town: SMTP. The Simple Mail Transfer Protocol.

SMTP was created in 1981 by a guy called Jon Postel. You might have heard of a thing called Postel's Law, which has become a sort of golden rule for implementing network stacks and protocols: "be conservative in what you send, and liberal in what you accept". In other words: when you're pushing data to somebody else's system, don't rely on quirks and obscure edge cases - but if somebody else is pushing data to *your* system, and that data contains those quirks and edge cases... try to make sense of it, and accept it if you can.

And, since we're on the subject of quirks and edge cases... let's talk about email addresses.

Quick show of hands: how many of you folks out there every had to write some code to validate an email address? Do you think you got it right?

Hands up if you've ever tried to validate an email address using a regular expression... sure you have. C'mon. You can admit it. We're all friends here. I've certainly done it.

OK, maybe even regular expressions is overkill. I mean, you know a valid email address when you see it, right?

[thor@avengers.com](mailto:thor@avengers.com)

[iron.man@avengers.com](mailto:iron.man@avengers.com)

[the-amazing-spiderman@avengers.com](mailto:the-amazing-spiderman@avengers.com)

T'[Challa@avengers.com](mailto:Challa@avengers.com)

"Steve 'Bob' O'Thicket"@(nope)[dylanbeattie.net](http://dylanbeattie.net)

[ALICE@spookymail.com](mailto:ALICE@spookymail.com)

[alice@spookymail.com](mailto:alice@spookymail.com)

[RIFF HERE THROUGH A WHOLE BUNCH OF EMAIL ADDRESSES]

Every single one of those email addresses is *technically* valid... you know what that means in practice? Well... actually, not a lot. And to understand why, we need to dive into how an email address actually works.

The only thing we can say with any kind of certainty is that a valid email address has to have at least one @-sign in it. Yep, at least one. If you want, you can have sixty-three of them, but you can't have none.

The reason why the @-sign matters is that it divides the address into a local part and a domain part. The domain part controls where the email's going, and the local part controls what happens when it gets there.

So, the domain part - the bit on the right-hand side of the @-sign. That's almost always a domain like [company.com](http://company.com) - or a subdomain, like, [sales.company.com](http://sales.company.com). It doesn't have to be. It can be an IP address, like this one. It can even be an IPv6 address, if you really want.

The reason for it is that the system that's sending the email, and any other intermediate servers which it passes through along the way? They've gotta be able to tell where it's going. If it's an IP address? Easy. That's where it's going.

If it's a domain, though? Well, now we need to find out which server handles email for that domain... and this is just one of the many, many things that DNS takes care of for us.

We're going to do a DNS lookup. The particular records we're interested in are called the mail exchange records - DNS shortens this to MX.

So let's open a terminal, and we're going to use the nslookup tool. The cool kids don't use nslookup, they use a thing called dig, but dig isn't included with Windows so we're going to use nslookup for this.

set type=MX

and then the name of the domain we're sending email to. We'll go for [codewithrockstar.com](http://codewithrockstar.com)

and there we go. Those are the host names of the servers which accept incoming email for anybody at [codewithrockstar.com](http://codewithrockstar.com).

So let's do it. Let's send an email. We're gonna pick the server with the lowest preference value, and we're going to connect to it on port 25, because that's the port used by SMTP.

We're gonna do this REAL old school. Original 1980s flavour SMTP.

HELO [dylanbeattie.net](http://dylanbeattie.net)

Now, remember paper letters? Snail mail? Like when Grandma still sends you a birthday card every year and you panic because normally the only printed letters you get are scary letters from the government about your taxes?

So here's a letter. Now, we *could* deliver this by going all the way to Timbuktu, finding the right address, and putting it in the mailbox. But, most of the time, that's not how we deliver mail. No, we're gonna put it in a mailbox and let the post offices do the rest... Royal Mail is gonna pick this up, and take it to a sorting office, and it'll probably end up on a plane to Amsterdam, and then another plane to Lagos, and then a truck from Lagos to Mali, and then, finally, it'll end up on a delivery worker's cart in Timbuktu.

Email works the same way. When you hit "Send" on an email, your device delivers it to something called a mail relay: the digital equivalent of a post box. There's a couple of reasons for this.

First: reliability. Imagine if we went all the way to Timbuktu and found they were closed. That'd suck, right? By using the postal service, that becomes somebody else's problem - we send mail to the relay server, which will store it until it can forward it to the next server along. Sometimes that'll be the destination, but not necessarily; my outgoing mail server might pass that mail on to another relay server, which could pass it on to *another* relay server, and so on.

What's particularly interesting about this is that there's nothing, anywhere in any of the underlying protocols that specifies how quickly email has to be delivered. Most internet protocols are published as something called a "request for comments", shortened to RFC, and the current version of the SMTP spec is RFC5321, published in 2008.

Section 4.5.4.1 of that RFC is called "sending strategy", and it advises:

The sender MUST delay retrying a particular destination after one attempt has failed. In general, the retry interval SHOULD be at least 30 minutes; however, more sophisticated and variable strategies will be beneficial when the SMTP client can determine the reason for non-delivery.

Retries continue until the message is transmitted or the sender gives up; the give-up time generally needs to be at least 4-5 days. It MAY be appropriate to set a shorter maximum number of retries for non- delivery notifications and equivalent error messages than for standard messages. The parameters to the retry algorithm MUST be configurable.

So that says: if you can't deliver it, you MUST wait a bit before trying again, you SHOULD wait at least 30 minutes, and you shouldn't give up trying for 4 or 5 days.

Any of you use Microsoft? Sure you do. One person down the front going "hell no! linux for ever!"... but you know the thing where you sign in to Microsoft and it sends you a one-time verification code? What happens if that takes five days? You go to log in to the Azure dashboard on a Monday morning, need to go through verification... your boss pings you on Wednesday, says "hey, is that new release live yet?" and you're like "no I'm waiting for my Azure code" and you go back to playing Minecraft...

There's another problem, though. A problem I'm sure you're all intimately familiar with...

Email was invented by a bunch of hippies in California in the 1970s. In the aftermath of Gary Thuerk's first spam incident, there was, maybe, a tiny window where we could have fundamentally changed the way email worked. But we didn't. Instead, people just agreed to be nice... and what a brilliant system that turned out to be.

The biggest problem with email is that it's a completely free way to send a message to anybody anywhere on the planet. The cost of sending a single email is, effectively, zero.

Now, I'm going to say that again, slowly: the biggest problem with email is that it's a completely free way to send a message to anybody anywhere on the planet.

There is a perfectly reasonable response to that statement, which is "what? How can THAT be a problem? That sounds amazing! That sounds like the most awesome thing ever invented!"

...and it kinda is. But like just about every awesome thing ever invented, it's all fun and games until humans get involved. Y'know. Us. It turns out that those techno-utopian ideals that came out of Berkeley and Stanford in the 1970s don't actually scale terribly well, and perhaps the most prominent example of that is unsolicited commercial email. Spam.

Here's the problem, in a nutshell. You're an SMTP server, Somebody, out there on the internet, connects to you and says "HELO! I've got an email here for [bob@funwith.email](mailto:bob@funwith.email)" - what do you do?

In the 80s and the early 90s, pretty much every SMTP server on the internet was what's known as an open relay. Getting email to work at at all was hard enough without all sorts of complicated restrictions on who was allowed to do what, and so pretty much anybody could connect to any SMTP server and say "hey, you don't know me, but I got this message needs to go to... somebody, somewhere else on the internet: can you help?" - and you'd get back a 250 OK. Which meant that just about anybody could take a list of email addresses - hundreds, thousands, maybe millions? - and write a relatively simple program that just sent email to all those people, by connecting to any SMTP relay they could find.

When we eventually realised this was a bad idea, we had a problem. How do you fix spam without breaking email in the process? Well, bad news, folks: the answer appears to be that you can't.

There's a lovely quote from a US Park Ranger about trying to design bear-proof garbage cans...

"There is considerable overlap between the intelligence of the smartest bears and the dumbest tourists."

When it comes to filtering email, we have the same problem. There's a considerable overlap between the smartest spammers and the dumbest legitimate senders; there are companies out there sending unsolicited marketing email who get absolutely every detail right, in terms of security, authentication, DNS records.... and then there's your friends working at some hot new startup who thought it would be cool to host their own email and turns out they don't actually know the first thing about running an email server. Part of the problem, of course, is that spam isn't a binary proposition. Some email is DEFINITELY junk. Some email is not only unsolicited, it's downright malicious. Some email is definitely legit. And then there's a whole kind of grey area in the middle. I get a *lot* of email which I've technically signed up for - mainly from bars and coffee shops where I had to put in my email address to get on the wifi, that kind of thing. I don't *want* it, but that doesn't make it spam.

So... what can we do to work out if a message is legitimate or not?

Just about every anti-spam technique that we've developed falls into one of two categories. We can accept delivery, and then look at the message and filter it based on content - or we can try to prevent it from being delivered in the first place. Now, we haven't even started talking about what an email message actually looks like yet... we're still just looking at the outside of the envelope.

But let's go back to talking about SMTP relays.

An open relay works kinda like this: it'll take email *from* anywhere, and deliver it *to* anywhere. And as we've seen, that's generally considered to be a bad idea.

So most modern SMTP servers are configured in one of two ways.

Outgoing servers: these will accept mail from a small, trusted selection of senders, but they don't care where it's going.

Incoming servers: will accept mail from just about anybody - but only if it's going to a particular domain.

We'll talk about outgoing servers first, 'cos that's kind of the easy part.

First: you can lock them down by network address. If any of you uses the SMTP server provided by your ISP, that's how it works. You connect from a trusted IP address, you're allowed to send mail - and if you use it to send spam, your ISP looks up the logs, finds the IP address you connected from, looks in their big customer database to find out which of their customers was using that IP, and sends you a threatening email saying "don't do that again or we'll disconnect you".

That worked brilliantly in the days when you got your email from the same place you got your internet: anybody out there still using the email address provided by their ISP? We got any folks here whose email is @[btinternet.com?](http://btinternet.com?) Any [aol.com](http://aol.com) ? any [sky.com](http://sky.com) ? A few. But in the age of smartphones and roaming data, that model doesn't work so well, because we send email from all *over* the place. So these days, if you want to send an outgoing email, you need to authenticate. Gmail, Office 365, any of the big marketing mail platforms like Mailchimp or Sendgrid: you need to use authenticated SMTP. It runs on a different port - normally port 587 - and requires a username and password.

In theory, there's nothing to stop any of you setting up an open relay. One particularly high-profile example is a guy call John Gilmore. John's a sort of extreme libertarian cypherpunk; he believes that restricting access to network services is a form of censorship, and has said that "the net interprets censorship as damage and routes around it". John owns a domain called [toad.com](http://toad.com), which is one of the oldest domains still in existence, first registered in 1987, and he still has an open relay on [hop.toad.com](http://hop.toad.com)

Now, you remember that little email demo we ran at the start of the talk? I thought it might be fun to randomly route some of those emails through the [toad.com](http://toad.com) relay to see how many of them would get delivered... but turns out, you can't. That demo is hosted on Microsoft Azure, and anything that's running in Azure cannot connect to the internet on port 25. It's completely blocked. Same with Amazon, same with Google Cloud: if you want to host apps in the cloud which send email, you've got to relay it via an external SMTP server, and you can't use port 25.

So the biggest challenge to running an open relay is finding somebody who'll let you host it. Azure won't, Google, won't, Amazon won't. If you've got an app on your phone that'll let you run a port scan, you'll find that mobile data networks block port 25. My home broadband *does* allow outgoing connections on port 25, but if I actually used it to run a mail relay, sooner or later somebody would complain and my ISP would probably disconnect me.

One of the reasons John Gilmore has given for running an open SMTP relay is that it means his friends can use it to send email when they're travelling... well, I'm not sure where John's friends are getting their internet access from when they're travelling, but given that every mobile data network, public wifi point, hotel, airport, etc. I've seen blocks outgoing traffic on port 25, I'm not sure that stands up.

The other thing about an open relay, though, is that it's almost completely untraceable. If I send you email through the [toad.com](http://toad.com) relay, I can spoof literally everything: the From: address, my name, the headers... the only part of that entire transaction which I can't fake is the network address I'm sending it from. And if that's a public wifi point, or a pay-as-you-go SIM card? Yeah. You'll never be able to trace that back to me. I suspect that's the real reason John Gilmore still runs an open relay: because that idea, of being able to connect to public services anonymously, without leaving any kind of footprints? That's the libertarian ethos in a nutshell.

[libertarianism?]

Anyway. Let's look at the other end of the connection: incoming servers.

Let's say we're a server running on Sendgrid's network, and we need to send a message to somebody with a Gmail address. We're going to do an nslookup, get a list of mail servers, pick the one with the lowest preference number, and connect to it.

Yep, on port 25, with no authentication. We don't *have* authentication - because one of the fundamental principles that makes email work in the first place is that I can send email to somebody who uses Gmail without me having to have a Google account. I can send email to folks using Office 365 even if I'm not a Microsoft customer.

But... we just decided that was bad, right?

Well, the first part is pretty easy. If you connect to Gmail's SMTP relay and say "hello, I've got an email here for [billg@microsoft.com](mailto:billg@microsoft.com)", the server's going to tell you to piss off. If you're running a small SMTP provider for your own domain, you just set it up to reject email to any domain except yours. Not a big deal. If you're running a service like Gmail or Office 365, which hosts email for literally thousands of other people's domains, it means running custom SMTP software that's linked to a live database of all the domains you're hosting right now, all the customer who haven't paid their bills, anybody who's over their storage quota... but, y'know, those are well-defined technical problems. Solving them at scale is complicated, but it's deterministic: throw enough money, hardware and smart people at it, and you'll get it done.

That's pretty much a solved problem now. According to the Internet Engineering Task Force, in 1998, 55% of mail servers were open relays; by 2002 that number was down to 2%. But it didn't solve spam. Not even close.

So let's say the address is legit: we're gmail, and somebody's knocking at the door going "hey, I have an email here for [dylan.beattie@gmail.com](mailto:dylan.beattie@gmail.com)".

We can’t ask them for a username and password, because as we’ve already established, that would sort of defeat the point of email. So how do we determine if the person knocking at the door is legit?

First, let’s check the IP address they’re connecting from. More specifically, let’s check whether other people have reported that IP address as suspicious. There’s a bunch of services out there which claim to maintain lists of “good” and “bad” IP addresses… nice idea, right? Well, yeah, when it works, it’s great. But this approach has two problems. One: if the bad people get access to an IP address - say, via a root kit, or malware - they can look those databases to see whether that IP’s already there. If not? Woohoo! Spam time!

But also… anybody can report any IP address for sending spam, any time.

If you’re relying on email as part of a key business process, and somebody reports you for spam, you’re gonna have a bad time: suddenly your customers whose email providers rely on those databases might stop getting your emails.

So this one kinda fails for the same reason that the ARPAnet approach didn’t work: when you’re trusting everybody else to behave themselves, it only takes a handful of bad people to ruin it for the rest of us.

So: how can we build a system where the bad people don’t get to participate?

The first really successful approach here was something called SPF: Sender Protection Framework. I send email from [dylanbeattie.net](http://dylanbeattie.net) - and nobody else is allowed to use that domain. If you get email claiming it’s from [dylanbeattie.net](http://dylanbeattie.net), and it isn’t from me, then somebody’s playing silly buggers.

More specifically: I only send email through a small number of mail relays. I send mail through Gmail’s SMTP relays, I send mail through Fastmail’s SMTP relays, and I will very occasionally send email directly from my house.

What SPF allows me to do is to create a record that says “Hey. I’m [dylanbeattie.net](http://dylanbeattie.net). Any email from me will come from THESE IP addresses” — and I can include a note that tells other systems what they should do with email that doesn’t.

And then I can share that rule with the rest of the world by creating a DNS record. Yep, DNS again. DNS really is awesome. So anybody out there who receives one of those messages can do a DNS lookup, find my SPF rules, compare those to the message it’s received, and decide whether to accept it, flag is as suspicious, or reject it.

SPF isn’’t just an elegant solution to this particular problem; it’s also a nice example of how to evolve a protocol like email without breaking anything. It’s up to individual providers to adopt SPF for their own platforms, and decide how aggressively to enforce it - and for folks who are actually sending email, they didn’t need to create those SPF records right away, but over time, they’d notice messages being rejected because some providers started rejecting any mail without a valid SPF record… one or two at first, then maybe a few messages a week, until eventually somebody decides they should probably set up this SPF thing.

[DKIM here]

And so, after a lot of complicated tricks involving DNS, we finally get to this point… the recipient’s SMTP server says “OK, you look legit, I trust you… HIT ME.”

Actually, no. One tiny detail we haven’t looked at yet: the local part.

The global email and DNS infrastructure exists to make sure that email gets delivered to the right server, and that’s all based on the domain part. The bit on the right-hand side of the at—sign.

The local part, though? The internet doesn’t care about that. As long as it’s *valid*, your message will get delivered.

Question: are these the same email address?

The answer is… it depends.

They’re the same domain, sure. That’s in the spec: DNS host and domain names are 7-bit ASCII, case insensitive. Mostly. There are standards out there for domains that use extended characters, and even top-level domains using non-Latin alphabets…

[punycode stuff here]

But if I’m running my own email server, I’m absolutely allowed to say that the local part is case sensitive. I’m not saying that’s a *good* idea… but modern SMTP evolved out of a set of protocols built for Unix systems where usernames, mailboxes, and file names were all case sensitive, and if you wanted to install an SMTP gateway on your DEC-20 minicomputer, you needed a way to map SMTP addresses onto your existing user accounts.

Does anybody care about this? The hell they do. At least once a week, I find some web form somewhere where I enter my email address and it puts it all into uppercase for me. Mostly airline websites - which, I suspect, is because even if your airline has a nice shiny mobile app, the system behind is probably still an IBM mainframe from 1977 that can’t handle lowercase letters, let alone Unicode…

So there’s Postel’s Law in action again: you *can* run an email provider with case-sensitive email addresses, but if you do, you’re probably going to make life difficult for yourself and everybody else is going to think you’re a weirdo.

OK, so, we’ve e’ve dealt with the domain part, MX records, black hole lists, SPF, DKIM, the local part… now, finally, we can actually send the damn email.

DATA

The simplest email - the “hello, world” of email messages, if you like - looks like this:

Hello world

That’s it. As long as we got all the other bits right, that’s going to get delivered. And the recipient will get this… we don’t know who it’s from, we don’t know who it’s for - I mean, it’s probably for us, ‘cos we’re the ones reading it, and it doesn’t have a subject - but it says hello world.

See, all that stuff on the outside of the proverbial envelope? That’s not part of the message, so if we want to send a properly-formatted message, we need to repeat ourselves a bit.

From: Dylan Beattie <[dylan@dylanbeattie.net](mailto:dylan@dylanbeattie.net)>

To: yada yada yada

Subject: this

Hello, World

There. Much nicer. And, as long as we stick to good old 7-bit 1960-s flavoured American ASCII, we can put as much as we want in there. Now, for 1960s-flavoured American humans, that’s just peachy… along with leaded gasoline and Richard Nixon. The rest of the world? Not so much.

If you want to know all about the wonderful world of character encodings, I got a whole other talk about that which you can find on YouTube. When it comes to email, what you’ve gotta remember is that the network only guarantees 7-bit ASCII… so if you want your emails to arrive reliably, you’ve gotta figure out how to encode them as 7-bit ASCII, and then how to decode them at the other end.

Let’s meet MIME: the Multpart Internet Mail Extension.

[talk about MIME here]

Now, this is all, in theory, very awesome.

So let’s say we’re developers, and we’re building a website that sells tickets. Conference tickets, airline tickets, railway tickets… don’t care. There’s a pretty well-established workflow for this kind of thing: go on the website, pay the money, and we’ll send you an email.

Key things about this email:

One: it’s gotta be readable. There must be a valid, meaningful plain text version of the message. Why? Because I said so, that’s why. Because there’s a hundred million billion things that just work better if your email includes a sensible plain text version. Search. Screen readers. Previews. Those little pop ups you get on your Fitbit telling you what the email says.

Two: it’s gotta be SEXY. There’s gotta be an HTML version of the email with fonts, and colours, and images, and it’s gotta *pop* and *jump out at you*. Why? Because the client’s marketing team said so, and they’re the ones paying the bills.

Ok, partly that. But partly also because that’s what our customers expect. Their perception of our company - or rather, of our clients’ company — is informed by a whole myriad of subtle factors, and having clean, attractive, professionally-designed emails can make a big difference to how they feel about the fact they’ve just given us all their money.

Three: we’ve got to include the ticket. As a PDF attachment, so they can print it and bring it with them on the day, so that if they drop their phone in the toilet on the way to the concert they can still actually get in to see the show.

When I first started writing websites in 1992 - yeah, I’m *old* - they were static HTML ‘cos we had no idea how to do anything else. But by the end of the 1990s, we’d figured out how to build data—driven web apps. Pretty much every website in the world today uses this kind of design: some of the stuff on the page is application code, which is managed and maintained by developers - and some of it is content, which is managed by somebody else. The application code is the gnarly stuff. It’s got dependency injection, and unit tests, and we keep it all in GitHub and deploy it using pipelines.

The content? That’s just records in a database, right? We don’t care about that stuff; that’s our clients problem. They break it, they fix it.

And then there’s email. If your application sends email, there’s almost certainly some content in there which your client would like to be able to change. But there’s also a load of stuff they can actually break. If you’re sending personalised emails - and if we’re sending out things like concert tickets, that’s *exactly* what we’re doing - your email templates are going to contain a bunch of placeholders, maybe even logic like conditional blocks or loops — so you can’t just edit that template and hope it works. You’ve gotta test the changes with a bunch of different scenarios… and remember, if you screw this up, you’re not just going to get errors on your website; you’re going to generate malformed emails full of weird formatting bugs, and send them to your customers.

There’s a lot of commercial providers out there doing email-as-a-service: Mailchimp, Sendgrid, yada, yada. Most of them offer an API, and some kind of templating language… but what I haven’t seen any of them provide is any kind of revision control for your templates. Sure, you can use their web interface to write rich HTML emails that include placeholder fields and loops, and then test it with various fragments of JSON… you ever tried explaining to your client’s marketing coordinator what JSON is? Yeah. JSON is for nerds.

So anything more complex than a simple “Hello {firstName}”, it’s probably going to be the developers who end up maintaining those email templates.

Now, there’s a sort of spectrum of how much fun different jobs in IT are. Up here, this is inventing your own web framework — which must be fun, because everybody keeps doing it. Here, this is the everyday good stuff: test—driven development, designing Microservices, building deployment pipelines. Down here is integrating with Microsoft Dynamics CRM. Down here is drinking rancid yak’s milk out of a wrestler’s jockstrap… and then here, this is writing HTML email templates.

HTML email is one of humanity’s horrible mistakes, and here’s why.

When you send somebody an email, you have no control over how, when, and where, they are going to read it. They might read it in Pine using a Linux terminal. They might read it in the mail app on their Android phone, or in Microsoft Outlook, or in any one of the hundreds of web-based email systems out there. Or in some obscure Windows application you’ve never heard of. There are too many different email readers out there to even list, let alone to try to test your code on all of them… and when it comes to rendering HTML email, they’re all different.

You can’t just render all the HTML. HTML in 2023 means CSS, JavaScript, animations, HTTP requests, JS modules… so that mail reader has to decide which bits of email it’s going to support, and which bits its going to ignore. Now throw in the fact that your email message is probably being displayed inside the mail reader’s user interface, which is also a web page.

First: where's that connection coming from?

Now, time for a bit of context, 'cos some of you might be wondering how I know all this stuff. I know more about delivering email than is probably healthy. For fifteen years, I ran the dev team at a company called Spotlight. One of Spotlight's most successful products was a job information service for professional actors: if you're working at the BBC, or you're putting on a play or making a commercial, you could go onto Spotlight and put out what's called a casting breakdown: describe the kind of actors you're looking for: gender, age, height, appearance, any special skills like being able to speak Welsh or play the cello, and then we'd email that breakdown to all the actors matching the spec. I built the first version of that system in classic ASP in 2002, when it had to send about two hundred emails at a time; over the years we rolled it out to more customers, added more features, and it became incredibly successful.

By the time I left Spotlight in 2018, we had over fifty thousand professional actors in our database, plus about five thousand agents and agencies who represented those actors. On a typical day we'd see over a hundred of these breakdowns; the average brief would match maybe five percent of the actors on our system. Five percent of fifty thousand actors is two and a half thousand... times one hundred breakdowns? That's a quarter of a million emails. A day. Not a quarter of a million duplicates of the same email: a quarter of a million unique, personalized email messages... and because almost all our clients were in the UK, that's not even evenly distributed. Monday to Friday, around 3pm, we'd see a MASSIVE spike. In the UK, all the agents and casting directors had just got back from their big boozy showbusiness lunch and decided to put in a couple of hours hard work before heading to the pub... just around the time that New York finished their second skinny latte of the day and the folks in Hollywood finished their power breakfast and started yelling at their assistants. Fifty thousand emails an hour - more than emails per second, sustained, for a couple of hours a day, every day.

Now, something you need to understand about professional actors: most of them don't do a whole lot of acting. On any given day, the average actor is not working in a theatre, or shooting a movie. No, they're at home, clicking "refresh" on their email, over and over again, because they're hoping that next time they refresh their email, that one perfect casting breakdown will land in their inbox and that'll be their ticket to fame and fortune. We used to get *phone calls* from actors saying "hey, I haven't had any breakdown emails today, is there a problem with the system?" - and answering those phone calls took time, and time costs money. So my team and I had a serious incentive to get really, REALLY good at making sure email got delivered, and creating as much transparency as we could around the whole delivery process.

And, because I'm an idiot, the whole time this is going on, I'm also running my own email server for me and a bunch of my friends. I had Fedora Linux running on an old Compaq workstation, with Qmail and Courier-IMAP, and that hosted all my personal email and all my websites for nearly fifteen years. That machine was bulletproof: it once stayed up for six hundred and twenty days without a reboot, and the only reason I had to shut it down was a power failure that knocked out the whole of London's West End for about four hours and we had to prioritise the production servers and shut down anything that wasn't mission-critical.

Well, kinda. I can *connect* to Google's SMTP server. I'm gonna send EHLO [dylanbeattie.net](http://dylanbeattie.net)

"did you say helo? No, I said EHLO. but that's close enough."

We said a while ago that the biggest problem with email is that it's a completely free way to send a message to anybody on the planet... but that's also what makes it awesome. I get a few dozen emails a week from real people, people I've never met. Folks asking questions about things I'm working on, inviting me to conferences and events, sending me lyrics for a song they wrote about Kubernetes.

I have kind of a weird email setup. All my email is hosted and managed by a company called Fastmail, who are excellent: I've used them for about fifteen years. But I don't use their UI. I use Gmail. I have Fastmail configured to forward copies of everything to Gmail, partly as a backup.

Setting up funwith.email

Website: Jekyll + Github Pages

MX: goes to [fastmail.com](http://fastmail.com)

Subdomains:

ggle.funwith.email > goes to Google Workspace

o365.funwith.email > goes to Office 365

proton.funwith.email > goes to Protonmail

Yahoo.funwith.email > goes to Yahoo! mail

zoho.funwith.email > goes to Zoho Mail

Two approaches. Block the connection - or accept it, then delete it.

Paul Graham. Bayesian filtering.

Add bit about DKIM and SPF

THE WHOLE FIRST PART is about: does the email get delivered?

THE SECOND PART: What does it actually look like?

"works on my machine"

UTF-7 MIME,

Tools to design and test emails.

Talk about iloveyou.vbs

Setup relays:

funwith.email -> to fastmail

Subdomains

.funwith.email => Google Apps

outlook.funwith.email => O365

Now, in my head, email has a sort of Richter scale of how obnoxious it is.

Level 0 is the email I actually want: the stuff that, if it doesn't arrive, I'm going to have to chase it. Or worse, make a telephone call. It's replies to emails I've sent to real people about work stuff, about events like this one we're at right now. Or it's things like airline tickets, order confirmations...

Level 1 is mailing lists. Still written by a person, but I'm one of a few hundred recipients who all get a copy. But it's good stuff. It's mailing lists I've signed up for, I know exactly where that email's coming from, and I know how to make it stop.

Level 2 is, you know, marketing. I bought something from a company a few years back, or I gave them my email address in order to get on the wifi, and now they email me every week to tell me about their special offers. Now, some companies do this *exceptionally* well. There's a music shop in the UK called Andertons, and their email marketing is *phenomenally* good: I'll often get emails from them that are like "hey, that thing you were looking at on our website last month? Thought you might like to know it's on special offer now."

Most companies do this so badly it's laughable. I get about a dozen emails a week from bars and coffee shops that aren't even on continent where I live; places I visited once, five years ago, and had to give them my email to get on the wifi... and the unsubscribe doesn't work, and I don't care *that* much, so I just sort of ignore it. But if anybody needs to know where the good drinks offers are in downtown Minnesota right now, I can probably hook you up.

Level 3 is the scams. Email pretending to be from DHL

  *

Scams, and why they're stupid

  *

Backscatter

https://www.computerworld.com/article/2539767/unsung-innovators--gary-thuerk--the-father-of-spam.html

https://www.templetons.com/brad/spamreact.html

Major

There were a handful of other standards for what's now called "non-internet electronic mail". One that a handful of you might remember was a system called Compuserve: subscribers to Compuserve could send electronic mail, but only to other Compuserve users - and it worked kinda like phone numbers; somebody's Compuserve address was a ten-digit number with a comma in the middle. AOL Screen Names was another one - if one AOL user wanted to send mail to another AOL user, all they needed was a screen name... and if an AOL user wanted to send mail to somebody on the internet. One of the greatest U-turns in the history of software happened

But there's a whole lot of computers and networks that aren't connected to ARPA yet - and a lot more machines that don't have any kind of permanent network connection 'cos they're using dial-up networking, which introduces a whole bunch of

NS hasn't been invented yet. IP addresses won't be invented for another decade or so. So if you want to send an electronic mail to somebody on a different system, possibly even on a different network, you need to get a little bit creative.

email:

how addresses work

local part

domain part

MX records

DKIM, SPF

WE HAVE A CONNECTION!

Now what happens? Let's unpack the actual SMTP protocol

SYNOPSIS

We're not quite sure exactly when email was invented. Sometime around 1971. But we know exactly when junk email was invented: May 3rd, 1978, when Gary Thuerk emailed 400 people an advertisement for DEC computers. It made a lot of people very angry... but it also sold a few computers, and so junk email was born.

Fast forward half a century, and the relationship between email and commerce has never been more complicated. In one sense, the utopian ideal of free, decentralised, electronic communication has come true... email is the ultimate cross-network, cross-platform communication protocol. In another sense, it's an arms race: mail providers and ISPs implement ever more stringent checks and policies to prevent junk mail, and if that means the occasional important message gets sent to junk by mistake, then hey, no big deal... until you're trying to send out e-tickets and discover that every company who uses Mimecast has decided your mail relay is sending junk. Marketing teams want beautiful, colourful, responsive emails, but their customers' mail clients are still using a subset of HTML 3.2 that doesn't even support CSS rules. And let's not even get started on how you design an email when half your readers will be using "dark mode" so everything ends up on a black background.

Email is too big to change, too broken to fix... and too important to ignore. So let's look at what we need to know to get it right. We'll learn about DNS, about MX and DKIM and SPF records. We'll learn about how MIME actually works (and what happens when it doesn't). We'll learn about tools like Papercut, Mailtrap, Mailjet, Foundation, and how to incorporate them into your development process. If you're lucky, you'll even learn about UTF-7, the most cursed encoding in the history of information systems. Modern email is hacks top of hacks on top of hacks... but, hey, it's also how you got your ticket to be here today, so why not come along and find out how it actually works?

THE FORMAT

0-5: Intro spiel. The email exercise: get everybody to send themselves an email. See how many of them get through.

Here's what's SUPPOSED to happen.