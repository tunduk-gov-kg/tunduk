create table if not exists public."Messages"
(
  "Id"                                      bigserial    not null
    constraint "PK_Messages"
      primary key,
  "CreatedAt"                               timestamp    not null,
  "ModifiedAt"                              timestamp,

  "MessageId"                               varchar(300) not null,
  "XRequestId"                              varchar(300),

  "MessageIssue"                            varchar(500),
  "MessageProtocolVersion"                  varchar(20)  not null,
  "MessageUserId"                           varchar(100),
  "MessageState"                            int          not null,

  "ConsumerInstance"                        varchar(50)  not null,
  "ConsumerMemberClass"                     varchar(50)  not null,
  "ConsumerMemberCode"                      varchar(50)  not null,
  "ConsumerSubSystemCode"                   varchar(100),

  "ProducerInstance"                        varchar(50)  not null,
  "ProducerMemberClass"                     varchar(50)  not null,
  "ProducerMemberCode"                      varchar(50)  not null,
  "ProducerSubSystemCode"                   varchar(50),
  "ProducerServiceCode"                     varchar(100) not null,
  "ProducerServiceVersion"                  varchar(50),

  "ProducerSecurityServerAddress"           varchar(200),
  "ProducerSecurityServerInternalIpAddress" varchar(200),
  "ConsumerSecurityServerAddress"           varchar(200),
  "ConsumerSecurityServerInternalIpAddress" varchar(200),

  "RepresentedPartyClass"                   varchar(100),
  "RepresentedPartyCode"                    varchar(100),

  "ConsumerServerRequestInTs"               bigint,
  "ConsumerServerRequestOutTs"              bigint,
  "ConsumerServerResponseInTs"              bigint,
  "ConsumerServerResponseOutTs"             bigint,

  "ProducerServerRequestInTs"               bigint,
  "ProducerServerRequestOutTs"              bigint,
  "ProducerServerResponseInTs"              bigint,
  "ProducerServerResponseOutTs"             bigint,


  "RequestAttachmentsCount"                 integer,
  "RequestSoapSize"                         integer,
  "RequestMimeSize"                         integer,

  "ResponseAttachmentsCount"                integer,
  "ResponseSoapSize"                        integer,
  "ResponseMimeSize"                        integer,

  "IsSucceeded"                             boolean      not null,
  "FaultCode"                               text,
  "FaultString"                             text
);

create index if not exists "IX_Messages_MessageId"
  on public."Messages" ("MessageId");

create index if not exists "IX_Messages_XRequestId"
  on public."Messages" ("XRequestId");

create index if not exists "IX_Messages_MessageState"
  on public."Messages" ("MessageState");

create index if not exists "IX_Messages_Consumer_Instance_SubSystem_MemberCode"
  on public."Messages" ("ConsumerInstance", "ConsumerMemberClass", "ConsumerMemberCode");

create index if not exists "IX_Messages_Producer_Instance_SubSystem_MemberCode_ServiceCode_ServiceVersion"
  on public."Messages" ("ProducerInstance", "ProducerMemberClass", "ProducerMemberCode", "ProducerServiceCode",
                        "ProducerServiceVersion");

