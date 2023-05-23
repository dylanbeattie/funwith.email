namespace FunWithEmail.WebApp.Models; 

public enum MailStatus {
	Accepted,
	Queued,
	Sent,
	DeliveredToInbox,
	DeliveredToJunk,
	Error
}
