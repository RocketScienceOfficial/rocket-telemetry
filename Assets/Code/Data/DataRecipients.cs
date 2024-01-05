using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataRecipient
{
    void OnSetData(RecipientData recipient);
}

public interface ITelemetryDataRecipient : IDataRecipient
{
}

public interface IDownloadDataRecipient : IDataRecipient
{
}

public interface IReplayDataRecipient : IDataRecipient
{
}

public interface ISimulationDataRecipient : IDataRecipient
{
}