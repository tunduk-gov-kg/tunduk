create or replace function "GetMemberConsumedServices"("from" bigint,
                                                       "to" bigint,
                                                       "instance" varchar(10),
                                                       "memberClass" varchar(10),
                                                       "memberCode" varchar(100))
  returns TABLE
          (
            "ConsumerInstance" varchar(10),
            "ConsumerMemberClass" varchar(10),
            "ConsumerMemberCode" varchar(50),
            "ConsumerSubSystemCode" varchar(100),
            "ProducerInstance" varchar(10),
            "ProducerMemberClass" varchar(10),
            "ProducerMemberCode" varchar(50),
            "ProducerSubSystemCode" varchar(100),
            "ProducerServiceCode" varchar(100),
            "ProducerServiceVersion" varchar(10),
            "Failed" bigint,
            "Succeeded" bigint
          )
  language plpgsql
as
$$
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
                      sum(case when not "m"."IsSucceeded" then 1 end),
                      sum(case when "m"."IsSucceeded" then 1 end)
               from "Messages" "m"
               where "m"."ProducerServiceCode" not in (select "ServiceCode" from "MetaServices")
                 and (
                   ("m"."ConsumerServerRequestInTs" >= "from" and "m"."ConsumerServerRequestInTs" <= "to")
                   or ("m"."ProducerServerRequestInTs" >= "from" and "m"."ProducerServerRequestInTs" <= "to")
                 )
                 and "m"."ConsumerInstance" = "instance"
                 and "m"."ConsumerMemberClass" = "memberClass"
                 and "m"."ConsumerMemberCode" = "memberCode"

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
               order by "m"."ConsumerSubSystemCode",
                        count(*) desc;

end;
$$;
