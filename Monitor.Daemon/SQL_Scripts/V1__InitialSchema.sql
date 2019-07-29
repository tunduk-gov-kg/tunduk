create table if not exists public."Servers"
(
    "Id"              bigserial    not null
        constraint "PK_Servers"
            primary key,
    "Instance"        varchar(100) not null,
    "MemberClass"     varchar(100) not null,
    "MemberCode"      varchar(100) not null,
    "Code"            varchar(200) not null,
    "NextRecordsFrom" timestamp    not null
);

alter table public."Servers"
    owner to postgres;

create unique index if not exists "IX_SecurityServers_MemberId_SecurityServerCode"
    on public."Servers" ("Instance", "MemberClass", "MemberCode", "Code");

