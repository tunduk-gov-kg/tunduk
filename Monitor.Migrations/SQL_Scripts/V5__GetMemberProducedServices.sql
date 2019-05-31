create or replace function "GetMemberProducedServices"("from" bigint,
                                                       "to" bigint,
                                                       "instance" varchar(10),
                                                       "memberClass" varchar(10),
                                                       "memberCode" varchar(100))
  returns TABLE
          (
            "ProducerInstance"       varchar(10),
            "ProducerMemberClass"    varchar(10),
            "ProducerMemberCode"     varchar(50),
            "ProducerSubSystemCode"  varchar(100),
            "ProducerServiceCode"    varchar(100),
            "ProducerServiceVersion" varchar(10),
            "ConsumerInstance"       varchar(10),
            "ConsumerMemberClass"    varchar(10),
            "ConsumerMemberCode"     varchar(50),
            "ConsumerSubSystemCode"  varchar(100),
            "Failed"                 bigint,
            "Succeeded"              bigint
          )
  language plpgsql
as
$$
begin
  return query select "m"."ProducerInstance",
                      "m"."ProducerMemberClass",
                      "m"."ProducerMemberCode",
                      "m"."ProducerSubSystemCode",
                      "m"."ProducerServiceCode",
                      "m"."ProducerServiceVersion",
                      "m"."ConsumerInstance",
                      "m"."ConsumerMemberClass",
                      "m"."ConsumerMemberCode",
                      "m"."ConsumerSubSystemCode",
                      sum(case when not "m"."IsSucceeded" then 1 end),
                      sum(case when "m"."IsSucceeded" then 1 end)
               from "Messages" "m"
               where "m"."ProducerServiceCode" not in (select "ServiceCode" from "MetaServices")
                 and (
                   ("m"."ConsumerServerRequestInTs" >= "from" and "m"."ConsumerServerRequestInTs" <= "to")
                   or ("m"."ProducerServerRequestInTs" >= "from" and "m"."ProducerServerRequestInTs" <= "to")
                 )
                 and "m"."ProducerInstance" = "instance"
                 and "m"."ProducerMemberClass" = "memberClass"
                 and "m"."ProducerMemberCode" = "memberCode"

               group by "m"."ProducerInstance",
                        "m"."ProducerMemberClass",
                        "m"."ProducerMemberCode",
                        "m"."ProducerSubSystemCode",
                        "m"."ProducerServiceCode",
                        "m"."ProducerServiceVersion",
                        "m"."ConsumerInstance",
                        "m"."ConsumerMemberClass",
                        "m"."ConsumerMemberCode",
                        "m"."ConsumerSubSystemCode"
               order by "m"."ProducerMemberClass",
                        "m"."ProducerMemberCode",
                        "m"."ProducerSubSystemCode",
                        "m"."ProducerServiceCode", 
                        count(*) desc;

end;
$$;