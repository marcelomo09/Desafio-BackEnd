public class NotificationsReqDeliveryRidersResponse
{
    public string IdRequestRace { get; set; }

    public string IdDeliveryman { get; set; }

    public string Name { get; set; }

    public string CNPJ { get; set; }

    public string CNH { get; set; }

    public float RequestRaceValue { get; set; }

    public NotificationsReqDeliveryRidersResponse()
    {
        IdRequestRace = string.Empty;
        IdDeliveryman = string.Empty;
        Name          = string.Empty;
        CNPJ          = string.Empty;
        CNH           = string.Empty;
    }

    public NotificationsReqDeliveryRidersResponse(NotificationsRequestDeliveryRiders notification)
    {
        IdRequestRace    = notification.IdRequestRace;
        IdDeliveryman    = notification.IdDeliveryman;
        Name             = notification.Name;
        CNPJ             = notification.CNPJ;
        CNH              = notification.CNH;
        RequestRaceValue = notification.RequestRaceValue;
    }
}