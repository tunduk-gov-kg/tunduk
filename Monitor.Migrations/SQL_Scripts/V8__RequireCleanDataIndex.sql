create index RequireCleanData_IX
    on "OpDataRecords" ("Succeeded", "ClientXRoadInstance", "ClientMemberClass", "ClientMemberCode",
                        "ServiceXRoadInstance", "ServiceMemberClass", "ServiceMemberCode", "ServiceCode",
                        "SecurityServerType", "MessageId", "IsProcessed");

