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


create or replace function "GetMostUsedServicesList"("from" bigint, "to" bigint)
  returns table
          (
            "ConsumerInstance"       VARCHAR(20),
            "ConsumerMemberClass"    VARCHAR(10),
            "ConsumerMemberCode"     VARCHAR(20),
            "ConsumerSubSystemCode"  VARCHAR(100),
            "ProducerInstance"       VARCHAR(20),
            "ProducerMemberClass"    VARCHAR(10),
            "ProducerMemberCode"     VARCHAR(20),
            "ProducerSubSystemCode"  VARCHAR(100),
            "ProducerServiceCode"    VARCHAR(200),
            "ProducerServiceVersion" VARCHAR(10),
            "Count"                  bigint
          )
as
$BODY$
begin
  return query select "m"."ConsumerInstance",
                      "m"."ConsumerMemberClass",
                      "m"."ConsumerMemberCode",
                      "m"."ConsumerSubSystemCode",
                      "m"."ProducerInstance",
                      "m"."ProducerMemberClass",
                      "m"."ProducerMemberCode",
                      "m"."ProducerSubSystemCode",
                      "m"."ProducerServiceCode",
                      "m"."ProducerServiceVersion",
                      count(*)
               from "Messages" "m"
               where "m"."ProducerServiceCode" not in (select "ServiceCode" from "MetaServices")
                 and ("m"."ConsumerServerRequestInTs" >= "from" and "m"."ConsumerServerRequestInTs" <= "to")

               group by "m"."ConsumerInstance",
                        "m"."ConsumerMemberClass",
                        "m"."ConsumerMemberCode",
                        "m"."ConsumerSubSystemCode",
                        "m"."ProducerInstance",
                        "m"."ProducerMemberClass",
                        "m"."ProducerMemberCode",
                        "m"."ProducerSubSystemCode",
                        "m"."ProducerServiceCode",
                        "m"."ProducerServiceVersion"
               order by count(*) desc;

end;
$BODY$ language 'plpgsql';