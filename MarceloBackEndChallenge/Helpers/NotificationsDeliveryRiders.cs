public class NotificationsDeliveryRiders
{
    public string IdRequestRace { get; set; }

    public string Queue { get { return "notificastions_deliveryriders"; } }

    public List<Deliveryman> DeliveryDrivers { get; set; }

    public NotificationsDeliveryRiders()
    {
        IdRequestRace   = string.Empty;
        DeliveryDrivers = new List<Deliveryman>();
    }

    public NotificationsDeliveryRiders(string idRequestRace, List<Deliveryman> deliveryDrivers)
    {
        IdRequestRace   = idRequestRace;
        DeliveryDrivers = deliveryDrivers;
    }
}