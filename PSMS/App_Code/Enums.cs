using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public enum PSMSCookie
{
    Preferred_Status_CHOICE,
    Preferred_Status_PR,
    Preferred_Status_Invitation,
    Preferred_Status_Selection,
    Preferred_Status_PO,
    Preferred_Status_GrnSrn,
    Preferred_Status_Payment
}

public enum MessageType
{
    Error=0,
    Success=1,
    Validation=2
}

public enum ConnectionType
{
    Open,
    Close,
    OpenGOClose
}
