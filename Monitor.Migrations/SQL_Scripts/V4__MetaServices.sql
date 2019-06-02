create table if not exists "MetaServices"
(
  "ServiceCode" varchar(100) not null
    constraint "PK_MetaServices" primary key
);

insert into "MetaServices"
values ('listMethods'),
       ('getSecurityServerOperationalData'),
       ('getSecurityServerHealthData'),
       ('getWsdl'),
       ('getSecurityServerMetrics'),
       ('listMethods');
