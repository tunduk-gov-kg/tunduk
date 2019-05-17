create table if not exists public."OpDataRecords"
(
  "Id"                           bigserial             not null
    constraint "PK_OperationalDataRecords"
      primary key,
  "CreatedAt"                    timestamp             not null,
  "ModifiedAt"                   timestamp,

  "ClientXRoadInstance"          varchar(50),
  "ClientMemberClass"            varchar(50),
  "ClientMemberCode"             varchar(50),
  "ClientSecurityServerAddress"  varchar(200),
  "ClientSubsystemCode"          varchar(100),
  "ServiceXRoadInstance"         varchar(50),
  "ServiceMemberClass"           varchar(50),
  "ServiceMemberCode"            varchar(50),
  "ServiceSubsystemCode"         varchar(50),
  "ServiceCode"                  varchar(100),
  "ServiceVersion"               varchar(50),
  "ServiceSecurityServerAddress" varchar(200),

  "XRequestId"                   varchar(300),
  "MessageId"                    varchar(300),
  "MessageIssue"                 varchar(500),
  "MessageProtocolVersion"       varchar(20),
  "MessageUserId"                varchar(100),
  "MonitoringDataTs"             bigint,
  "RepresentedPartyClass"        varchar(50),
  "RepresentedPartyCode"         varchar(50),
  "RequestAttachmentCount"       integer,
  "RequestInTs"                  bigint,
  "RequestOutTs"                 bigint,
  "RequestSoapSize"              integer,
  "RequestMimeSize"              integer,
  "ResponseAttachmentCount"      integer,
  "ResponseInTs"                 bigint,
  "ResponseOutTs"                bigint,
  "ResponseSoapSize"             integer,
  "ResponseMimeSize"             integer,
  "SecurityServerInternalIp"     text,
  "SecurityServerType"           text,
  "Succeeded"                    boolean,
  "SoapFaultCode"                text,
  "SoapFaultString"              text,
  "IsProcessed"                  boolean default false not null
);

alter table public."OpDataRecords"
  owner to postgres;

create index if not exists "IX_OperationalDataRecords_MessageId"
  on public."OpDataRecords" ("MessageId");

create index if not exists "IX_OperationalDataRecords_XRequestId"
  on public."OpDataRecords" ("XRequestId");

create index if not exists "IX_OperationalDataRecords_IsProcessed"
  on public."OpDataRecords" ("IsProcessed");

